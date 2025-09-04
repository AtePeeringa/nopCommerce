using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.ProductConfigurator.Domain;
using Nop.Plugin.Misc.ProductConfigurator.Models;
using Nop.Plugin.Misc.ProductConfigurator.Services;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Framework.Controllers;
using System.Text.Json;

namespace Nop.Plugin.Misc.ProductConfigurator.Controllers;

public partial class ProductConfiguratorController : BasePublicController
{
    private readonly IProductConfigurationService _productConfigurationService;
    private readonly IPricingService _pricingService;
    private readonly ICustomerConfigurationService _customerConfigurationService;
    private readonly IProductService _productService;
    private readonly IWorkContext _workContext;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;
    private readonly ICustomerService _customerService;

    public ProductConfiguratorController(
        IProductConfigurationService productConfigurationService,
        IPricingService pricingService,
        ICustomerConfigurationService customerConfigurationService,
        IProductService productService,
        IWorkContext workContext,
        ILocalizationService localizationService,
        INotificationService notificationService,
        ICustomerService customerService)
    {
        _productConfigurationService = productConfigurationService;
        _pricingService = pricingService;
        _customerConfigurationService = customerConfigurationService;
        _productService = productService;
        _workContext = workContext;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _customerService = customerService;
    }

    public virtual async Task<IActionResult> Configure(int productId)
    {
        if (productId <= 0)
            return NotFound();

        var product = await _productService.GetProductByIdAsync(productId);
        if (product == null)
            return NotFound();

        if (!await _productConfigurationService.IsProductConfigurableAsync(productId))
        {
            return RedirectToRoute("Product", new { SeName = await _productService.GetProductSeNameAsync(product) });
        }

        var model = await PrepareConfigurationModelAsync(productId);
        
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (customer != null && !await _customerService.IsGuestAsync(customer))
        {
            var existingConfiguration = await _customerConfigurationService.GetCustomerConfigurationAsync(customer.Id, productId);
            if (existingConfiguration != null && !string.IsNullOrEmpty(existingConfiguration.ConfigurationData))
            {
                var savedConfiguration = JsonSerializer.Deserialize<Dictionary<string, object>>(existingConfiguration.ConfigurationData);
                if (savedConfiguration != null)
                {
                    model.SelectedOptions = savedConfiguration;
                    await UpdateModelWithSelections(model, savedConfiguration);
                }
            }
        }

        return View("~/Plugins/Misc.ProductConfigurator/Views/ProductConfigurator/Configure.cshtml", model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> UpdateConfiguration(int productId, [FromBody] Dictionary<string, object> configuration)
    {
        if (productId <= 0)
            return Json(new { success = false, message = "Invalid product" });

        try
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
                return Json(new { success = false, message = "Product not found" });

            var validation = await _productConfigurationService.ValidateConfigurationAsync(productId, configuration);
            if (!validation.IsValid)
            {
                return Json(new { 
                    success = false, 
                    message = "Configuration validation failed", 
                    errors = validation.Errors 
                });
            }

            var priceBreakdown = await _pricingService.GetPriceBreakdownAsync(productId, configuration, product.Price);
            var availableOptions = await _productConfigurationService.GetAvailableOptionsAsync(productId, configuration);

            return Json(new { 
                success = true, 
                priceBreakdown = priceBreakdown,
                availableOptions = availableOptions.Select(o => new { 
                    id = o.Id, 
                    groupId = o.ConfigurationGroupId,
                    isAvailable = true 
                }),
                totalPrice = priceBreakdown.TotalPrice
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> SaveConfiguration(int productId, [FromBody] Dictionary<string, object> configuration)
    {
        if (productId <= 0)
            return Json(new { success = false, message = "Invalid product" });

        var customer = await _workContext.GetCurrentCustomerAsync();
        if (customer == null || await _customerService.IsGuestAsync(customer))
        {
            return Json(new { success = false, message = "Please log in to save configuration" });
        }

        try
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
                return Json(new { success = false, message = "Product not found" });

            var validation = await _productConfigurationService.ValidateConfigurationAsync(productId, configuration);
            if (!validation.IsValid)
            {
                return Json(new { 
                    success = false, 
                    message = "Configuration validation failed", 
                    errors = validation.Errors 
                });
            }

            var totalPrice = await _pricingService.CalculateConfigurationPriceAsync(productId, configuration, product.Price);
            
            await _customerConfigurationService.SaveCustomerConfigurationAsync(customer.Id, productId, configuration, totalPrice);

            return Json(new { 
                success = true, 
                message = await _localizationService.GetResourceAsync("Plugins.Misc.ProductConfigurator.ConfigurationSaved")
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> GetAvailableOptions(int productId, [FromBody] Dictionary<string, object> configuration)
    {
        if (productId <= 0)
            return Json(new { success = false, message = "Invalid product" });

        try
        {
            var availableOptions = await _productConfigurationService.GetAvailableOptionsAsync(productId, configuration);
            
            return Json(new { 
                success = true, 
                options = availableOptions.Select(o => new { 
                    id = o.Id,
                    groupId = o.ConfigurationGroupId,
                    name = o.Name,
                    displayName = o.DisplayName,
                    description = o.Description,
                    priceAdjustment = o.PriceAdjustment,
                    priceAdjustmentType = o.PriceAdjustmentType,
                    imageUrl = o.ImageUrl
                })
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    private async Task<ConfigurationModel> PrepareConfigurationModelAsync(int productId)
    {
        var product = await _productService.GetProductByIdAsync(productId);
        var productConfiguration = await _productConfigurationService.GetProductConfigurationByProductIdAsync(productId);
        
        if (product == null || productConfiguration == null)
            throw new ArgumentException("Product or configuration not found");

        var model = new ConfigurationModel
        {
            ProductId = productId,
            ProductName = product.Name,
            BasePrice = product.Price,
            TotalPrice = product.Price,
            IsConfigurable = productConfiguration.IsConfigurable
        };

        var configurationGroups = await _productConfigurationService.GetConfigurationGroupsByProductConfigurationIdAsync(productConfiguration.Id);
        
        foreach (var group in configurationGroups)
        {
            var groupModel = new ConfigurationGroupModel
            {
                Id = group.Id,
                Name = group.Name,
                DisplayName = group.DisplayName,
                Description = group.Description,
                IsRequired = group.IsRequired,
                GroupType = (int)group.GroupType,
                GroupTypeName = await _localizationService.GetResourceAsync($"Plugins.Misc.ProductConfigurator.GroupType.{group.GroupType}"),
                DisplayOrder = group.DisplayOrder
            };

            var options = await _productConfigurationService.GetConfigurationOptionsByGroupIdAsync(group.Id);
            foreach (var option in options)
            {
                var optionModel = new ConfigurationOptionModel
                {
                    Id = option.Id,
                    Name = option.Name,
                    DisplayName = option.DisplayName,
                    Description = option.Description,
                    PriceAdjustment = option.PriceAdjustment,
                    PriceAdjustmentType = (int)option.PriceAdjustmentType,
                    PriceAdjustmentTypeName = await _localizationService.GetResourceAsync($"Plugins.Misc.ProductConfigurator.PriceAdjustmentType.{option.PriceAdjustmentType}"),
                    IsDefault = option.IsDefault,
                    IsActive = option.IsActive,
                    ImageUrl = option.ImageUrl,
                    ConfigurationGroupId = option.ConfigurationGroupId
                };

                groupModel.Options.Add(optionModel);
                
                if (option.IsDefault)
                {
                    groupModel.SelectedOptionId = option.Id;
                }
            }

            model.ConfigurationGroups.Add(groupModel);
        }

        var dimensionRules = await _productConfigurationService.GetDimensionRulesByProductConfigurationIdAsync(productConfiguration.Id);
        foreach (var dimensionRule in dimensionRules)
        {
            var dimensionModel = new DimensionRuleModel
            {
                Id = dimensionRule.Id,
                DimensionType = dimensionRule.DimensionType,
                MinValue = dimensionRule.MinValue,
                MaxValue = dimensionRule.MaxValue,
                DefaultValue = dimensionRule.DefaultValue,
                StepValue = dimensionRule.StepValue,
                Unit = dimensionRule.Unit,
                PricePerUnit = dimensionRule.PricePerUnit,
                IsRequired = dimensionRule.IsRequired,
                ProductConfigurationId = dimensionRule.ProductConfigurationId,
                SelectedValue = dimensionRule.DefaultValue
            };

            model.DimensionRules.Add(dimensionModel);
        }

        model.PriceBreakdown = new PriceBreakdownModel
        {
            BasePrice = product.Price,
            TotalPrice = product.Price
        };

        return model;
    }

    private async Task UpdateModelWithSelections(ConfigurationModel model, Dictionary<string, object> configuration)
    {
        foreach (var group in model.ConfigurationGroups)
        {
            var groupKey = $"Group_{group.Id}";
            if (configuration.TryGetValue(groupKey, out var optionIdValue) && 
                int.TryParse(optionIdValue?.ToString(), out var optionId))
            {
                group.SelectedOptionId = optionId;
            }
        }

        foreach (var dimension in model.DimensionRules)
        {
            var dimensionKey = $"Dimension_{dimension.DimensionType}";
            if (configuration.TryGetValue(dimensionKey, out var dimensionValue) && 
                decimal.TryParse(dimensionValue?.ToString(), out var value))
            {
                dimension.SelectedValue = value;
            }
        }

        var priceBreakdown = await _pricingService.GetPriceBreakdownAsync(model.ProductId, configuration, model.BasePrice);
        
        model.PriceBreakdown = new PriceBreakdownModel
        {
            BasePrice = priceBreakdown.BasePrice,
            MaterialPrice = priceBreakdown.MaterialPrice,
            DimensionPrice = priceBreakdown.DimensionPrice,
            FeaturePrice = priceBreakdown.FeaturePrice,
            ManufacturingPrice = priceBreakdown.ManufacturingPrice,
            SetupFee = priceBreakdown.SetupFee,
            TotalPrice = priceBreakdown.TotalPrice,
            Items = priceBreakdown.Items.Select(i => new PriceBreakdownItemModel
            {
                Name = i.Name,
                Description = i.Description,
                Price = i.Price,
                Category = i.Category
            }).ToList()
        };

        model.TotalPrice = priceBreakdown.TotalPrice;
    }
}