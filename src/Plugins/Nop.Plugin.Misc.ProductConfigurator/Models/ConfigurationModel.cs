using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.ProductConfigurator.Models;

public record ConfigurationModel : BaseNopModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsConfigurable { get; set; }
    public List<ConfigurationGroupModel> ConfigurationGroups { get; set; } = new();
    public List<DimensionRuleModel> DimensionRules { get; set; } = new();
    public PriceBreakdownModel PriceBreakdown { get; set; } = new();
    public Dictionary<string, object> SelectedOptions { get; set; } = new();
}

public record ConfigurationGroupModel : BaseNopEntityModel
{
    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurationGroup.Fields.Name")]
    public string Name { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurationGroup.Fields.DisplayName")]
    public string DisplayName { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurationGroup.Fields.Description")]
    public string Description { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurationGroup.Fields.IsRequired")]
    public bool IsRequired { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurationGroup.Fields.GroupType")]
    public int GroupType { get; set; }
    public string GroupTypeName { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurationGroup.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    public List<ConfigurationOptionModel> Options { get; set; } = new();
    public int? SelectedOptionId { get; set; }
}

public record ConfigurationOptionModel : BaseNopEntityModel
{
    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurationOption.Fields.Name")]
    public string Name { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurationOption.Fields.DisplayName")]
    public string DisplayName { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurationOption.Fields.Description")]
    public string Description { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurationOption.Fields.PriceAdjustment")]
    public decimal PriceAdjustment { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurationOption.Fields.PriceAdjustmentType")]
    public int PriceAdjustmentType { get; set; }
    public string PriceAdjustmentTypeName { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurationOption.Fields.IsDefault")]
    public bool IsDefault { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurationOption.Fields.IsActive")]
    public bool IsActive { get; set; }

    public string ImageUrl { get; set; } = string.Empty;
    public int ConfigurationGroupId { get; set; }
    public bool IsAvailable { get; set; } = true;
}

public record DimensionRuleModel : BaseNopEntityModel
{
    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.DimensionRule.Fields.DimensionType")]
    public string DimensionType { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.DimensionRule.Fields.MinValue")]
    public decimal MinValue { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.DimensionRule.Fields.MaxValue")]
    public decimal MaxValue { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.DimensionRule.Fields.DefaultValue")]
    public decimal DefaultValue { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.DimensionRule.Fields.StepValue")]
    public decimal StepValue { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.DimensionRule.Fields.Unit")]
    public string Unit { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.DimensionRule.Fields.PricePerUnit")]
    public decimal PricePerUnit { get; set; }

    public bool IsRequired { get; set; }
    public int ProductConfigurationId { get; set; }
    public decimal? SelectedValue { get; set; }
}

public record PriceBreakdownModel : BaseNopModel
{
    public decimal BasePrice { get; set; }
    public decimal MaterialPrice { get; set; }
    public decimal DimensionPrice { get; set; }
    public decimal FeaturePrice { get; set; }
    public decimal ManufacturingPrice { get; set; }
    public decimal SetupFee { get; set; }
    public decimal TotalPrice { get; set; }
    public List<PriceBreakdownItemModel> Items { get; set; } = new();
}

public record PriceBreakdownItemModel : BaseNopModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
}