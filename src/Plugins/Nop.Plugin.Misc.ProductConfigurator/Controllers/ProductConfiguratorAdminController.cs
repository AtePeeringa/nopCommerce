using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.ProductConfigurator.Domain;
using Nop.Plugin.Misc.ProductConfigurator.Models.Admin;
using Nop.Plugin.Misc.ProductConfigurator.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using System.Text.Json;

namespace Nop.Plugin.Misc.ProductConfigurator.Controllers;

[AuthorizeAdmin]
[Area(AreaNames.Admin)]
[Route("Admin/ProductConfigurator/[action]")]
public partial class ProductConfiguratorAdminController : BasePluginController
{
    private readonly IPermissionService _permissionService;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;
    private readonly IProductService _productService;
    private readonly IWorkContext _workContext;
    private readonly IRulesEngineService _rulesEngineService;
    private readonly IFlexiblePricingService _flexiblePricingService;
    private readonly IBomService _bomService;
    private readonly IProductionService _productionService;

    public ProductConfiguratorAdminController(
        IPermissionService permissionService,
        ILocalizationService localizationService,
        INotificationService notificationService,
        IProductService productService,
        IWorkContext workContext,
        IRulesEngineService rulesEngineService,
        IFlexiblePricingService flexiblePricingService,
        IBomService bomService,
        IProductionService productionService)
    {
        _permissionService = permissionService;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _productService = productService;
        _workContext = workContext;
        _rulesEngineService = rulesEngineService;
        _flexiblePricingService = flexiblePricingService;
        _bomService = bomService;
        _productionService = productionService;
    }

    public virtual IActionResult Configure()
    {
        if (!_permissionService.Authorize(StandardPermissionProvider.ManagePlugins))
            return AccessDeniedView();

        return View("~/Plugins/Misc.ProductConfigurator/Views/Admin/Configure.cshtml");
    }

    [HttpGet]
    public virtual async Task<IActionResult> ConfigurableProductsList()
    {
        if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            return AccessDeniedView();

        var model = new ConfigurableProductSearchModel();
        return View("~/Plugins/Misc.ProductConfigurator/Views/Admin/ConfigurableProductsList.cshtml", model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> ConfigurableProductsList(ConfigurableProductSearchModel searchModel)
    {
        if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            return AccessDeniedDataTablesJson();

        // Implementation for loading configurable products list
        var model = new ConfigurableProductListModel();
        
        return Json(model);
    }

    [HttpGet]
    public virtual async Task<IActionResult> CreateConfigurableProduct()
    {
        if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            return AccessDeniedView();

        var model = new ConfigurableProductModel();
        await PrepareConfigurableProductModelAsync(model);
        
        return View("~/Plugins/Misc.ProductConfigurator/Views/Admin/CreateConfigurableProduct.cshtml", model);
    }

    [HttpPost]
    [ParameterBasedOnFormName("save-continue", "continueEditing")]
    public virtual async Task<IActionResult> CreateConfigurableProduct(ConfigurableProductModel model, bool continueEditing)
    {
        if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            return AccessDeniedView();

        if (ModelState.IsValid)
        {
            var configurableProduct = new ConfigurableProduct
            {
                ProductId = model.ProductId,
                Name = model.Name,
                Description = model.Description,
                IsConfigurable = model.IsConfigurable,
                PrimaryUnit = model.PrimaryUnit,
                SecondaryUnits = JsonSerializer.Serialize(model.SecondaryUnits ?? new List<string>()),
                BasePricingMethod = (PricingMethod)model.BasePricingMethod,
                BasePrice = model.BasePrice,
                MinimumConstraints = model.MinimumConstraints,
                MaximumConstraints = model.MaximumConstraints,
                EnableBomGeneration = model.EnableBomGeneration,
                BomTemplate = model.BomTemplate,
                ProductionTemplate = model.ProductionTemplate,
                DisplayOrder = model.DisplayOrder,
                IsActive = model.IsActive,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };

            // Save the configurable product (this would need the actual service implementation)
            // await _configurableProductService.InsertConfigurableProductAsync(configurableProduct);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            if (!continueEditing)
                return RedirectToAction("ConfigurableProductsList");

            return RedirectToAction("EditConfigurableProduct", new { id = configurableProduct.Id });
        }

        await PrepareConfigurableProductModelAsync(model);
        return View("~/Plugins/Misc.ProductConfigurator/Views/Admin/CreateConfigurableProduct.cshtml", model);
    }

    [HttpGet]
    public virtual async Task<IActionResult> EditConfigurableProduct(int id)
    {
        if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            return AccessDeniedView();

        // Load configurable product and prepare model
        var model = new ConfigurableProductModel { Id = id };
        await PrepareConfigurableProductModelAsync(model);
        
        return View("~/Plugins/Misc.ProductConfigurator/Views/Admin/EditConfigurableProduct.cshtml", model);
    }

    [HttpGet]
    public virtual async Task<IActionResult> AttributesList(int configurableProductId)
    {
        if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            return AccessDeniedView();

        var model = new AttributeSearchModel { ConfigurableProductId = configurableProductId };
        return View("~/Plugins/Misc.ProductConfigurator/Views/Admin/AttributesList.cshtml", model);
    }

    [HttpGet]
    public virtual async Task<IActionResult> CreateAttribute(int configurableProductId)
    {
        if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            return AccessDeniedView();

        var model = new AttributeModel { ConfigurableProductId = configurableProductId };
        await PrepareAttributeModelAsync(model);
        
        return View("~/Plugins/Misc.ProductConfigurator/Views/Admin/CreateAttribute.cshtml", model);
    }

    [HttpGet]
    public virtual async Task<IActionResult> RulesList(int configurableProductId)
    {
        if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            return AccessDeniedView();

        var model = new RuleSearchModel { ConfigurableProductId = configurableProductId };
        return View("~/Plugins/Misc.ProductConfigurator/Views/Admin/RulesList.cshtml", model);
    }

    [HttpGet]
    public virtual async Task<IActionResult> CreateRule(int configurableProductId)
    {
        if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            return AccessDeniedView();

        var model = new RuleModel { ConfigurableProductId = configurableProductId };
        await PrepareRuleModelAsync(model);
        
        return View("~/Plugins/Misc.ProductConfigurator/Views/Admin/CreateRule.cshtml", model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> TestRule([FromBody] TestRuleRequest request)
    {
        if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            return Json(new { success = false, message = "Access denied" });

        try
        {
            var rule = new ConfigurationRule
            {
                ConfigurableProductId = request.ConfigurableProductId,
                Conditions = request.Conditions,
                Action = (RuleAction)request.Action,
                TargetAttribute = request.TargetAttribute,
                TargetOptions = request.TargetOptions,
                RuleParameters = request.RuleParameters
            };

            var isApplicable = await _rulesEngineService.EvaluateRuleAsync(rule, request.TestConfiguration);
            var result = await _rulesEngineService.ExecuteRuleAsync(rule, request.TestConfiguration);

            return Json(new
            {
                success = true,
                isApplicable = isApplicable,
                result = new
                {
                    success = result.Success,
                    message = result.Message,
                    modifiedConfiguration = result.ModifiedConfiguration,
                    affectedAttributes = result.AffectedAttributes,
                    hiddenOptions = result.HiddenOptions,
                    showOnlyOptions = result.ShowOnlyOptions,
                    warnings = result.Warnings,
                    errors = result.Errors
                }
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpGet]
    public virtual async Task<IActionResult> PriceComponentsList(int configurableProductId)
    {
        if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            return AccessDeniedView();

        var model = new PriceComponentSearchModel { ConfigurableProductId = configurableProductId };
        return View("~/Plugins/Misc.ProductConfigurator/Views/Admin/PriceComponentsList.cshtml", model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> TestPricing([FromBody] TestPricingRequest request)
    {
        if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            return Json(new { success = false, message = "Access denied" });

        try
        {
            var dimensions = await _flexiblePricingService.CalculateDimensionsAsync(request.ConfigurableProductId, request.Configuration);
            var priceBreakdown = await _flexiblePricingService.CalculatePriceAsync(request.ConfigurableProductId, request.Configuration, dimensions);

            return Json(new
            {
                success = true,
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
                    totalPrice = priceBreakdown.TotalPrice,
                    items = priceBreakdown.Items,
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
    public virtual async Task<IActionResult> GenerateBom([FromBody] GenerateBomRequest request)
    {
        if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            return Json(new { success = false, message = "Access denied" });

        try
        {
            var dimensions = await _flexiblePricingService.CalculateDimensionsAsync(request.ConfigurableProductId, request.Configuration);
            var bom = await _bomService.GenerateBomAsync(request.ConfigurableProductId, request.Configuration, dimensions);

            return Json(new
            {
                success = true,
                bom = new
                {
                    items = bom.Items.Select(item => new
                    {
                        partNumber = item.PartNumber,
                        description = item.Description,
                        quantity = item.Quantity,
                        unit = item.Unit,
                        costPerUnit = item.CostPerUnit,
                        totalCost = item.TotalCost,
                        componentType = item.ComponentType.ToString(),
                        isCritical = item.IsCritical,
                        leadTimeDays = item.LeadTimeDays,
                        calculationDetails = item.CalculationDetails
                    }),
                    totalMaterialCost = bom.TotalMaterialCost,
                    leadTimeDays = bom.LeadTimeDays,
                    warnings = bom.Warnings
                }
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> GenerateProductionPlan([FromBody] GenerateProductionPlanRequest request)
    {
        if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            return Json(new { success = false, message = "Access denied" });

        try
        {
            var dimensions = await _flexiblePricingService.CalculateDimensionsAsync(request.ConfigurableProductId, request.Configuration);
            var productionPlan = await _productionService.GenerateProductionPlanAsync(request.ConfigurableProductId, request.Configuration, dimensions);

            return Json(new
            {
                success = true,
                productionPlan = new
                {
                    operations = productionPlan.Operations.Select(op => new
                    {
                        operationName = op.OperationName,
                        operationType = op.OperationType.ToString(),
                        workCenter = op.WorkCenter,
                        sequenceOrder = op.SequenceOrder,
                        setupTime = op.SetupTime,
                        cycleTime = op.CycleTime,
                        totalTime = op.TotalTime,
                        operationCost = op.OperationCost,
                        calculationDetails = op.CalculationDetails
                    }),
                    totalProductionTime = productionPlan.TotalProductionTime,
                    totalSetupTime = productionPlan.TotalSetupTime,
                    totalProductionCost = productionPlan.TotalProductionCost,
                    estimatedDeliveryDate = productionPlan.EstimatedDeliveryDate,
                    workCenters = productionPlan.WorkCenters,
                    warnings = productionPlan.Warnings
                }
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    #region Utilities

    private async Task PrepareConfigurableProductModelAsync(ConfigurableProductModel model)
    {
        // Prepare dropdown lists for products, pricing methods, etc.
        model.AvailableProducts = await _productService.GetAllProductsAsync()
            .ContinueWith(t => t.Result.Select(p => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList());

        model.AvailablePricingMethods = Enum.GetValues<PricingMethod>()
            .Select(pm => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = ((int)pm).ToString(),
                Text = pm.ToString()
            }).ToList();
    }

    private async Task PrepareAttributeModelAsync(AttributeModel model)
    {
        model.AvailableAttributeTypes = Enum.GetValues<AttributeType>()
            .Select(at => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = ((int)at).ToString(),
                Text = at.ToString()
            }).ToList();

        model.AvailableInputTypes = Enum.GetValues<AttributeInputType>()
            .Select(it => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = ((int)it).ToString(),
                Text = it.ToString()
            }).ToList();
    }

    private async Task PrepareRuleModelAsync(RuleModel model)
    {
        model.AvailableRuleTypes = Enum.GetValues<RuleType>()
            .Select(rt => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = ((int)rt).ToString(),
                Text = rt.ToString()
            }).ToList();

        model.AvailableRuleActions = Enum.GetValues<RuleAction>()
            .Select(ra => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = ((int)ra).ToString(),
                Text = ra.ToString()
            }).ToList();
    }

    #endregion
}

// Request models for AJAX endpoints
public class TestRuleRequest
{
    public int ConfigurableProductId { get; set; }
    public string Conditions { get; set; } = string.Empty;
    public int Action { get; set; }
    public string TargetAttribute { get; set; } = string.Empty;
    public string TargetOptions { get; set; } = string.Empty;
    public string RuleParameters { get; set; } = string.Empty;
    public Dictionary<string, object> TestConfiguration { get; set; } = new();
}

public class TestPricingRequest
{
    public int ConfigurableProductId { get; set; }
    public Dictionary<string, object> Configuration { get; set; } = new();
}

public class GenerateBomRequest
{
    public int ConfigurableProductId { get; set; }
    public Dictionary<string, object> Configuration { get; set; } = new();
}

public class GenerateProductionPlanRequest
{
    public int ConfigurableProductId { get; set; }
    public Dictionary<string, object> Configuration { get; set; } = new();
}