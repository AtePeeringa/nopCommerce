using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.ProductConfigurator.Models.Admin;

public record ConfigurableProductSearchModel : BaseSearchModel
{
    [NopResourceDisplayName("Admin.Catalog.Products.List.SearchProductName")]
    public string SearchProductName { get; set; } = string.Empty;

    [NopResourceDisplayName("Admin.Catalog.Products.List.SearchCategory")]
    public int SearchCategoryId { get; set; }
}

public record ConfigurableProductListModel : BasePagedListModel<ConfigurableProductModel>
{
}

public record ConfigurableProductModel : BaseNopEntityModel
{
    public ConfigurableProductModel()
    {
        AvailableProducts = new List<SelectListItem>();
        AvailablePricingMethods = new List<SelectListItem>();
        SecondaryUnits = new List<string>();
    }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurableProduct.Fields.Product")]
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurableProduct.Fields.Name")]
    public string Name { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurableProduct.Fields.Description")]
    public string Description { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurableProduct.Fields.IsConfigurable")]
    public bool IsConfigurable { get; set; } = true;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurableProduct.Fields.PrimaryUnit")]
    public string PrimaryUnit { get; set; } = "m²";

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurableProduct.Fields.SecondaryUnits")]
    public IList<string> SecondaryUnits { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurableProduct.Fields.BasePricingMethod")]
    public int BasePricingMethod { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurableProduct.Fields.BasePrice")]
    public decimal BasePrice { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurableProduct.Fields.MinimumConstraints")]
    public string MinimumConstraints { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurableProduct.Fields.MaximumConstraints")]
    public string MaximumConstraints { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurableProduct.Fields.EnableBomGeneration")]
    public bool EnableBomGeneration { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurableProduct.Fields.BomTemplate")]
    public string BomTemplate { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurableProduct.Fields.ProductionTemplate")]
    public string ProductionTemplate { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurableProduct.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.ConfigurableProduct.Fields.IsActive")]
    public bool IsActive { get; set; } = true;

    public IList<SelectListItem> AvailableProducts { get; set; }
    public IList<SelectListItem> AvailablePricingMethods { get; set; }
}

public record AttributeSearchModel : BaseSearchModel
{
    public int ConfigurableProductId { get; set; }
}

public record AttributeListModel : BasePagedListModel<AttributeModel>
{
}

public record AttributeModel : BaseNopEntityModel
{
    public AttributeModel()
    {
        AvailableAttributeTypes = new List<SelectListItem>();
        AvailableInputTypes = new List<SelectListItem>();
        Options = new List<AttributeOptionModel>();
    }

    public int ConfigurableProductId { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Attribute.Fields.Name")]
    public string Name { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Attribute.Fields.DisplayName")]
    public string DisplayName { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Attribute.Fields.Description")]
    public string Description { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Attribute.Fields.AttributeType")]
    public int AttributeType { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Attribute.Fields.InputType")]
    public int InputType { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Attribute.Fields.IsRequired")]
    public bool IsRequired { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Attribute.Fields.AffectsPricing")]
    public bool AffectsPricing { get; set; } = true;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Attribute.Fields.AffectsBom")]
    public bool AffectsBom { get; set; } = true;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Attribute.Fields.AffectsProduction")]
    public bool AffectsProduction { get; set; } = true;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Attribute.Fields.ValidationRules")]
    public string ValidationRules { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Attribute.Fields.DefaultValue")]
    public string DefaultValue { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Attribute.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Attribute.Fields.IsActive")]
    public bool IsActive { get; set; } = true;

    public IList<SelectListItem> AvailableAttributeTypes { get; set; }
    public IList<SelectListItem> AvailableInputTypes { get; set; }
    public IList<AttributeOptionModel> Options { get; set; }
}

public record AttributeOptionModel : BaseNopEntityModel
{
    public int ProductAttributeId { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.AttributeOption.Fields.Name")]
    public string Name { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.AttributeOption.Fields.DisplayName")]
    public string DisplayName { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.AttributeOption.Fields.Description")]
    public string Description { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.AttributeOption.Fields.Sku")]
    public string Sku { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.AttributeOption.Fields.IsDefault")]
    public bool IsDefault { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.AttributeOption.Fields.ImageUrl")]
    public string ImageUrl { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.AttributeOption.Fields.ColorCode")]
    public string ColorCode { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.AttributeOption.Fields.TechnicalData")]
    public string TechnicalData { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.AttributeOption.Fields.MaterialProperties")]
    public string MaterialProperties { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.AttributeOption.Fields.BomData")]
    public string BomData { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.AttributeOption.Fields.ProductionData")]
    public string ProductionData { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.AttributeOption.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.AttributeOption.Fields.IsActive")]
    public bool IsActive { get; set; } = true;
}

public record RuleSearchModel : BaseSearchModel
{
    public int ConfigurableProductId { get; set; }
}

public record RuleListModel : BasePagedListModel<RuleModel>
{
}

public record RuleModel : BaseNopEntityModel
{
    public RuleModel()
    {
        AvailableRuleTypes = new List<SelectListItem>();
        AvailableRuleActions = new List<SelectListItem>();
    }

    public int ConfigurableProductId { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Rule.Fields.Name")]
    public string Name { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Rule.Fields.Description")]
    public string Description { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Rule.Fields.RuleType")]
    public int RuleType { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Rule.Fields.Action")]
    public int Action { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Rule.Fields.Conditions")]
    public string Conditions { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Rule.Fields.TargetAttribute")]
    public string TargetAttribute { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Rule.Fields.TargetOptions")]
    public string TargetOptions { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Rule.Fields.RuleParameters")]
    public string RuleParameters { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Rule.Fields.ErrorMessage")]
    public string ErrorMessage { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Rule.Fields.Priority")]
    public int Priority { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.Rule.Fields.IsActive")]
    public bool IsActive { get; set; } = true;

    public IList<SelectListItem> AvailableRuleTypes { get; set; }
    public IList<SelectListItem> AvailableRuleActions { get; set; }
}

public record PriceComponentSearchModel : BaseSearchModel
{
    public int ConfigurableProductId { get; set; }
}

public record PriceComponentListModel : BasePagedListModel<PriceComponentModel>
{
}

public record PriceComponentModel : BaseNopEntityModel
{
    public int ConfigurableProductId { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.PriceComponent.Fields.Name")]
    public string Name { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.PriceComponent.Fields.Description")]
    public string Description { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.PriceComponent.Fields.ComponentType")]
    public int ComponentType { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.PriceComponent.Fields.PricingMethod")]
    public int PricingMethod { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.PriceComponent.Fields.Conditions")]
    public string Conditions { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.PriceComponent.Fields.Formula")]
    public string Formula { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.PriceComponent.Fields.BasePrice")]
    public decimal BasePrice { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.PriceComponent.Fields.MinimumPrice")]
    public decimal MinimumPrice { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.PriceComponent.Fields.MaximumPrice")]
    public decimal MaximumPrice { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.PriceComponent.Fields.IsSetupFee")]
    public bool IsSetupFee { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.PriceComponent.Fields.AffectsTotal")]
    public bool AffectsTotal { get; set; } = true;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.PriceComponent.Fields.QuantityBreaks")]
    public string QuantityBreaks { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.PriceComponent.Fields.Priority")]
    public int Priority { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductConfigurator.Admin.PriceComponent.Fields.IsActive")]
    public bool IsActive { get; set; } = true;
}