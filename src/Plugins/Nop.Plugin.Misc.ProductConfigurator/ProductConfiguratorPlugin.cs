using Nop.Core;
using Nop.Data;
using Nop.Plugin.Misc.ProductConfigurator.Domain;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;

namespace Nop.Plugin.Misc.ProductConfigurator;

public class ProductConfiguratorPlugin : BasePlugin
{
    private readonly IRepository<ProductConfiguration> _productConfigurationRepository;
    private readonly IRepository<ConfigurationGroup> _configurationGroupRepository;
    private readonly IRepository<ConfigurationOption> _configurationOptionRepository;
    private readonly IRepository<DimensionRule> _dimensionRuleRepository;
    private readonly IRepository<PricingRule> _pricingRuleRepository;
    private readonly IRepository<CustomerConfiguration> _customerConfigurationRepository;
    private readonly ILocalizationService _localizationService;
    private readonly IWebHelper _webHelper;

    public ProductConfiguratorPlugin(
        IRepository<ProductConfiguration> productConfigurationRepository,
        IRepository<ConfigurationGroup> configurationGroupRepository,
        IRepository<ConfigurationOption> configurationOptionRepository,
        IRepository<DimensionRule> dimensionRuleRepository,
        IRepository<PricingRule> pricingRuleRepository,
        IRepository<CustomerConfiguration> customerConfigurationRepository,
        ILocalizationService localizationService,
        IWebHelper webHelper)
    {
        _productConfigurationRepository = productConfigurationRepository;
        _configurationGroupRepository = configurationGroupRepository;
        _configurationOptionRepository = configurationOptionRepository;
        _dimensionRuleRepository = dimensionRuleRepository;
        _pricingRuleRepository = pricingRuleRepository;
        _customerConfigurationRepository = customerConfigurationRepository;
        _localizationService = localizationService;
        _webHelper = webHelper;
    }

    public override string GetConfigurationPageUrl()
    {
        return _webHelper.GetStoreLocation() + "Admin/ProductConfigurator/Configure";
    }

    public override async Task InstallAsync()
    {
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Plugins.Misc.ProductConfigurator.Title"] = "Product Configurator",
            ["Plugins.Misc.ProductConfigurator.Description"] = "Configure custom products with materials, dimensions, and options",
            
            // Admin area
            ["Plugins.Misc.ProductConfigurator.Admin.ProductConfigurations"] = "Product Configurations",
            ["Plugins.Misc.ProductConfigurator.Admin.ProductConfiguration.Fields.Product"] = "Product",
            ["Plugins.Misc.ProductConfigurator.Admin.ProductConfiguration.Fields.IsConfigurable"] = "Is Configurable",
            ["Plugins.Misc.ProductConfigurator.Admin.ProductConfiguration.Fields.EnableCascadingParameters"] = "Enable Cascading Parameters",
            
            // Configuration Groups
            ["Plugins.Misc.ProductConfigurator.Admin.ConfigurationGroup.Fields.Name"] = "Name",
            ["Plugins.Misc.ProductConfigurator.Admin.ConfigurationGroup.Fields.DisplayName"] = "Display Name",
            ["Plugins.Misc.ProductConfigurator.Admin.ConfigurationGroup.Fields.Description"] = "Description",
            ["Plugins.Misc.ProductConfigurator.Admin.ConfigurationGroup.Fields.IsRequired"] = "Is Required",
            ["Plugins.Misc.ProductConfigurator.Admin.ConfigurationGroup.Fields.GroupType"] = "Group Type",
            ["Plugins.Misc.ProductConfigurator.Admin.ConfigurationGroup.Fields.DisplayOrder"] = "Display Order",
            
            // Configuration Options
            ["Plugins.Misc.ProductConfigurator.Admin.ConfigurationOption.Fields.Name"] = "Name",
            ["Plugins.Misc.ProductConfigurator.Admin.ConfigurationOption.Fields.DisplayName"] = "Display Name",
            ["Plugins.Misc.ProductConfigurator.Admin.ConfigurationOption.Fields.Description"] = "Description",
            ["Plugins.Misc.ProductConfigurator.Admin.ConfigurationOption.Fields.PriceAdjustment"] = "Price Adjustment",
            ["Plugins.Misc.ProductConfigurator.Admin.ConfigurationOption.Fields.PriceAdjustmentType"] = "Price Adjustment Type",
            ["Plugins.Misc.ProductConfigurator.Admin.ConfigurationOption.Fields.IsDefault"] = "Is Default",
            ["Plugins.Misc.ProductConfigurator.Admin.ConfigurationOption.Fields.IsActive"] = "Is Active",
            
            // Dimension Rules
            ["Plugins.Misc.ProductConfigurator.Admin.DimensionRule.Fields.DimensionType"] = "Dimension Type",
            ["Plugins.Misc.ProductConfigurator.Admin.DimensionRule.Fields.MinValue"] = "Minimum Value",
            ["Plugins.Misc.ProductConfigurator.Admin.DimensionRule.Fields.MaxValue"] = "Maximum Value",
            ["Plugins.Misc.ProductConfigurator.Admin.DimensionRule.Fields.DefaultValue"] = "Default Value",
            ["Plugins.Misc.ProductConfigurator.Admin.DimensionRule.Fields.StepValue"] = "Step Value",
            ["Plugins.Misc.ProductConfigurator.Admin.DimensionRule.Fields.Unit"] = "Unit",
            ["Plugins.Misc.ProductConfigurator.Admin.DimensionRule.Fields.PricePerUnit"] = "Price per Unit",
            
            // Customer facing
            ["Plugins.Misc.ProductConfigurator.Configure"] = "Configure Product",
            ["Plugins.Misc.ProductConfigurator.Material"] = "Material",
            ["Plugins.Misc.ProductConfigurator.Dimensions"] = "Dimensions",
            ["Plugins.Misc.ProductConfigurator.Features"] = "Features",
            ["Plugins.Misc.ProductConfigurator.Manufacturing"] = "Manufacturing",
            ["Plugins.Misc.ProductConfigurator.Technique"] = "Technique",
            ["Plugins.Misc.ProductConfigurator.PriceBreakdown"] = "Price Breakdown",
            ["Plugins.Misc.ProductConfigurator.TotalPrice"] = "Total Price",
            ["Plugins.Misc.ProductConfigurator.AddToCart"] = "Add to Cart",
            ["Plugins.Misc.ProductConfigurator.SaveConfiguration"] = "Save Configuration",
            
            // Group Types
            ["Plugins.Misc.ProductConfigurator.GroupType.Material"] = "Material",
            ["Plugins.Misc.ProductConfigurator.GroupType.Dimension"] = "Dimension",
            ["Plugins.Misc.ProductConfigurator.GroupType.Manufacturing"] = "Manufacturing",
            ["Plugins.Misc.ProductConfigurator.GroupType.Feature"] = "Feature",
            ["Plugins.Misc.ProductConfigurator.GroupType.Technique"] = "Technique",
            
            // Price Adjustment Types
            ["Plugins.Misc.ProductConfigurator.PriceAdjustmentType.FixedAmount"] = "Fixed Amount",
            ["Plugins.Misc.ProductConfigurator.PriceAdjustmentType.Percentage"] = "Percentage",
            ["Plugins.Misc.ProductConfigurator.PriceAdjustmentType.Multiplier"] = "Multiplier",
            ["Plugins.Misc.ProductConfigurator.PriceAdjustmentType.PerUnit"] = "Per Unit",
            
            // Validation messages
            ["Plugins.Misc.ProductConfigurator.Validation.Required"] = "This field is required",
            ["Plugins.Misc.ProductConfigurator.Validation.DimensionRange"] = "Value must be between {0} and {1} {2}",
            ["Plugins.Misc.ProductConfigurator.Validation.ConfigurationIncomplete"] = "Please complete all required configuration options",
        });

        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Misc.ProductConfigurator");
        await base.UninstallAsync();
    }
}