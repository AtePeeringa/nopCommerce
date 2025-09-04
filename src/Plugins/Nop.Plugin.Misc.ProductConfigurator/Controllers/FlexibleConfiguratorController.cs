using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Plugin.Misc.ProductConfigurator.Models.Public;
using Nop.Plugin.Misc.ProductConfigurator.Services;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Controllers;
using Nop.Web.Framework.Controllers;
using System.Text.Json;

namespace Nop.Plugin.Misc.ProductConfigurator.Controllers;

public partial class FlexibleConfiguratorController : BasePublicController
{
    private readonly IRulesEngineService _rulesEngineService;
    private readonly IFlexiblePricingService _flexiblePricingService;
    private readonly IBomService _bomService;
    private readonly IProductionService _productionService;
    private readonly IProductService _productService;
    private readonly IWorkContext _workContext;
    private readonly ICustomerService _customerService;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;

    public FlexibleConfiguratorController(
        IRulesEngineService rulesEngineService,
        IFlexiblePricingService flexiblePricingService,
        IBomService bomService,
        IProductionService productionService,
        IProductService productService,
        IWorkContext workContext,
        ICustomerService customerService,
        ILocalizationService localizationService,
        INotificationService notificationService)
    {
        _rulesEngineService = rulesEngineService;
        _flexiblePricingService = flexiblePricingService;
        _bomService = bomService;
        _productionService = productionService;
        _productService = productService;
        _workContext = workContext;
        _customerService = customerService;
        _localizationService = localizationService;
        _notificationService = notificationService;
    }

    [HttpGet]
    [Route("configurator/{productId:int}")]
    public virtual async Task<IActionResult> Configure(int productId)
    {
        if (productId <= 0)
            return NotFound();

        var product = await _productService.GetProductByIdAsync(productId);
        if (product == null || !product.Published)
            return NotFound();

        // Check if product is configurable
        // This would need the actual service to check ConfigurableProduct table
        
        var model = await PrepareFlexibleConfiguratorModelAsync(productId);
        
        return View("~/Plugins/Misc.ProductConfigurator/Views/FlexibleConfigurator/Configure.cshtml", model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> GetAvailableOptions([FromBody] GetAvailableOptionsRequest request)
    {
        try
        {
            var availableOptions = new Dictionary<string, object>();

            // Get available options for each attribute based on current configuration
            foreach (var attributeName in request.AttributeNames)
            {
                var options = await _rulesEngineService.GetAvailableOptionsAsync(
                    request.ConfigurableProductId, 
                    attributeName, 
                    request.CurrentConfiguration);

                availableOptions[attributeName] = options.Select(o => new
                {
                    id = o.Id,
                    name = o.Name,
                    displayName = o.DisplayName,
                    description = o.Description,
                    sku = o.Sku,
                    isDefault = o.IsDefault,
                    imageUrl = o.ImageUrl,
                    colorCode = o.ColorCode,
                    technicalData = string.IsNullOrEmpty(o.TechnicalData) ? null : JsonSerializer.Deserialize<object>(o.TechnicalData),
                    materialProperties = string.IsNullOrEmpty(o.MaterialProperties) ? null : JsonSerializer.Deserialize<object>(o.MaterialProperties)
                }).ToList();
            }

            return Json(new { success = true, availableOptions = availableOptions });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> ValidateConfiguration([FromBody] ValidateConfigurationRequest request)
    {
        try
        {
            var validationResult = await _rulesEngineService.ValidateConfigurationAsync(
                request.ConfigurableProductId, 
                request.Configuration);

            var warnings = await _rulesEngineService.GetConfigurationWarningsAsync(
                request.ConfigurableProductId, 
                request.Configuration);

            return Json(new
            {
                success = true,
                isValid = validationResult.IsValid,
                errors = validationResult.Errors,
                warnings = validationResult.Warnings.Concat(warnings).ToList()
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> CalculatePrice([FromBody] CalculatePriceRequest request)
    {
        try
        {
            // Apply auto-set rules first
            var updatedConfiguration = await _rulesEngineService.ApplyAutoSetRulesAsync(
                request.ConfigurableProductId, 
                request.Configuration);

            // Calculate dimensions
            var dimensions = await _flexiblePricingService.CalculateDimensionsAsync(
                request.ConfigurableProductId, 
                updatedConfiguration);

            // Calculate price breakdown
            var priceBreakdown = await _flexiblePricingService.CalculatePriceAsync(
                request.ConfigurableProductId, 
                updatedConfiguration, 
                dimensions);

            return Json(new
            {
                success = true,
                updatedConfiguration = updatedConfiguration,
                dimensions = dimensions,
                priceBreakdown = new
                {
                    basePrice = priceBreakdown.BasePrice,
                    materialCost = priceBreakdown.MaterialCost,
                    areaBasedCost = priceBreakdown.AreaBasedCost,
                    perimeterBasedCost = priceBreakdown.PerimeterBasedCost,
                    holeCost = priceBreakdown.HoleCost,
                    processCost = priceBreakdown.ProcessCost,
                    finishingCost = priceBreakdown.FinishingCost,
                    setupFees = priceBreakdown.SetupFees,
                    quantityDiscount = priceBreakdown.QuantityDiscount,
                    machineTimeCost = priceBreakdown.MachineTimeCost,
                    rushSurcharge = priceBreakdown.RushSurcharge,
                    totalPrice = priceBreakdown.TotalPrice,
                    items = priceBreakdown.Items.Select(item => new
                    {
                        name = item.Name,
                        description = item.Description,
                        price = item.Price,
                        formula = item.Formula,
                        componentType = item.ComponentType.ToString(),
                        variables = item.Variables,
                        affectsTotal = item.AffectsTotal
                    }).ToList(),
                    dimensions = priceBreakdown.Dimensions,
                    warnings = priceBreakdown.Warnings
                }
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> SaveConfiguration([FromBody] SaveConfigurationRequest request)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (customer == null || await _customerService.IsGuestAsync(customer))
        {
            return Json(new { success = false, message = "Please log in to save your configuration" });
        }

        try
        {
            // Validate the configuration
            var validationResult = await _rulesEngineService.ValidateConfigurationAsync(
                request.ConfigurableProductId, 
                request.Configuration);

            if (!validationResult.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Configuration is not valid",
                    errors = validationResult.Errors
                });
            }

            // Calculate final price
            var dimensions = await _flexiblePricingService.CalculateDimensionsAsync(
                request.ConfigurableProductId, 
                request.Configuration);
            
            var priceBreakdown = await _flexiblePricingService.CalculatePriceAsync(
                request.ConfigurableProductId, 
                request.Configuration, 
                dimensions);

            // Save customer configuration
            // This would need the actual service implementation
            
            return Json(new
            {
                success = true,
                message = await _localizationService.GetResourceAsync("Plugins.Misc.ProductConfigurator.ConfigurationSaved"),
                totalPrice = priceBreakdown.TotalPrice
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> GenerateQuote([FromBody] GenerateQuoteRequest request)
    {
        try
        {
            // Validate the configuration
            var validationResult = await _rulesEngineService.ValidateConfigurationAsync(
                request.ConfigurableProductId, 
                request.Configuration);

            if (!validationResult.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "Configuration is not valid",
                    errors = validationResult.Errors
                });
            }

            // Calculate dimensions, pricing, BOM, and production plan
            var dimensions = await _flexiblePricingService.CalculateDimensionsAsync(
                request.ConfigurableProductId, 
                request.Configuration);

            var priceBreakdown = await _flexiblePricingService.CalculatePriceAsync(
                request.ConfigurableProductId, 
                request.Configuration, 
                dimensions);

            var bom = await _bomService.GenerateBomAsync(
                request.ConfigurableProductId, 
                request.Configuration, 
                dimensions);

            var productionPlan = await _productionService.GenerateProductionPlanAsync(
                request.ConfigurableProductId, 
                request.Configuration, 
                dimensions);

            return Json(new
            {
                success = true,
                quote = new
                {
                    configuration = request.Configuration,
                    dimensions = dimensions,
                    pricing = new
                    {
                        totalPrice = priceBreakdown.TotalPrice,
                        breakdown = priceBreakdown.Items.Select(item => new
                        {
                            name = item.Name,
                            description = item.Description,
                            price = item.Price,
                            componentType = item.ComponentType.ToString()
                        }).ToList()
                    },
                    bom = new
                    {
                        totalMaterialCost = bom.TotalMaterialCost,
                        leadTimeDays = bom.LeadTimeDays,
                        items = bom.Items.Take(10).Select(item => new // Limit for summary
                        {
                            partNumber = item.PartNumber,
                            description = item.Description,
                            quantity = item.Quantity,
                            unit = item.Unit,
                            totalCost = item.TotalCost
                        }).ToList()
                    },
                    production = new
                    {
                        totalProductionTime = productionPlan.TotalProductionTime,
                        totalProductionCost = productionPlan.TotalProductionCost,
                        estimatedDeliveryDate = productionPlan.EstimatedDeliveryDate,
                        workCenters = productionPlan.WorkCenters
                    }
                }
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    #region Utilities

    private async Task<FlexibleConfiguratorModel> PrepareFlexibleConfiguratorModelAsync(int productId)
    {
        var product = await _productService.GetProductByIdAsync(productId);
        
        // This would need actual service implementations to load configurable product data
        var model = new FlexibleConfiguratorModel
        {
            ProductId = productId,
            ProductName = product.Name,
            BasePrice = product.Price,
            ConfigurableProductId = 1, // This would come from lookup
            PrimaryUnit = "m²",
            Attributes = new List<FlexibleAttributeModel>
            {
                // Example attributes - these would be loaded from database
                new FlexibleAttributeModel
                {
                    Name = "Material",
                    DisplayName = "Material",
                    AttributeType = "Material",
                    InputType = "SingleSelect",
                    IsRequired = true,
                    Options = new List<FlexibleAttributeOptionModel>
                    {
                        new FlexibleAttributeOptionModel { Id = 1, Name = "Aluminum", DisplayName = "Aluminum", IsDefault = true },
                        new FlexibleAttributeOptionModel { Id = 2, Name = "Steel", DisplayName = "Stainless Steel" },
                        new FlexibleAttributeOptionModel { Id = 3, Name = "Acrylic", DisplayName = "Acrylic" }
                    }
                },
                new FlexibleAttributeModel
                {
                    Name = "Thickness",
                    DisplayName = "Thickness (mm)",
                    AttributeType = "Dimension",
                    InputType = "SingleSelect",
                    IsRequired = true,
                    Options = new List<FlexibleAttributeOptionModel>
                    {
                        new FlexibleAttributeOptionModel { Id = 4, Name = "1", DisplayName = "1mm" },
                        new FlexibleAttributeOptionModel { Id = 5, Name = "2", DisplayName = "2mm", IsDefault = true },
                        new FlexibleAttributeOptionModel { Id = 6, Name = "3", DisplayName = "3mm" }
                    }
                },
                new FlexibleAttributeModel
                {
                    Name = "Width",
                    DisplayName = "Width (mm)",
                    AttributeType = "Dimension",
                    InputType = "Numeric",
                    IsRequired = true,
                    MinValue = 10,
                    MaxValue = 1000,
                    DefaultValue = "100",
                    StepValue = 1
                },
                new FlexibleAttributeModel
                {
                    Name = "Height",
                    DisplayName = "Height (mm)",
                    AttributeType = "Dimension",
                    InputType = "Numeric",
                    IsRequired = true,
                    MinValue = 10,
                    MaxValue = 1000,
                    DefaultValue = "50",
                    StepValue = 1
                },
                new FlexibleAttributeModel
                {
                    Name = "Holes",
                    DisplayName = "Number of Holes",
                    AttributeType = "Modification",
                    InputType = "Numeric",
                    IsRequired = false,
                    MinValue = 0,
                    MaxValue = 20,
                    DefaultValue = "0",
                    StepValue = 1
                },
                new FlexibleAttributeModel
                {
                    Name = "Adhesive",
                    DisplayName = "Adhesive Backing",
                    AttributeType = "Mounting",
                    InputType = "Boolean",
                    IsRequired = false
                },
                new FlexibleAttributeModel
                {
                    Name = "Technology",
                    DisplayName = "Manufacturing Technology",
                    AttributeType = "Technology",
                    InputType = "SingleSelect",
                    IsRequired = true,
                    Options = new List<FlexibleAttributeOptionModel>
                    {
                        new FlexibleAttributeOptionModel { Id = 7, Name = "Laser", DisplayName = "Laser Cutting", IsDefault = true },
                        new FlexibleAttributeOptionModel { Id = 8, Name = "Milling", DisplayName = "CNC Milling" },
                        new FlexibleAttributeOptionModel { Id = 9, Name = "Printing", DisplayName = "Digital Printing" }
                    }
                }
            }
        };

        return model;
    }

    #endregion
}

// Request models for AJAX endpoints
public class GetAvailableOptionsRequest
{
    public int ConfigurableProductId { get; set; }
    public List<string> AttributeNames { get; set; } = new();
    public Dictionary<string, object> CurrentConfiguration { get; set; } = new();
}

public class ValidateConfigurationRequest
{
    public int ConfigurableProductId { get; set; }
    public Dictionary<string, object> Configuration { get; set; } = new();
}

public class CalculatePriceRequest
{
    public int ConfigurableProductId { get; set; }
    public Dictionary<string, object> Configuration { get; set; } = new();
}

public class SaveConfigurationRequest
{
    public int ConfigurableProductId { get; set; }
    public Dictionary<string, object> Configuration { get; set; } = new();
    public string ConfigurationName { get; set; } = string.Empty;
}

public class GenerateQuoteRequest
{
    public int ConfigurableProductId { get; set; }
    public Dictionary<string, object> Configuration { get; set; } = new();
}