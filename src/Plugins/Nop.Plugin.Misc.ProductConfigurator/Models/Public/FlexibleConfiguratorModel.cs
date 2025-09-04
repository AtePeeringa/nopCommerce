using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.ProductConfigurator.Models.Public;

/// <summary>
/// Flexible configurator model for customer-facing interface
/// </summary>
public record FlexibleConfiguratorModel : BaseNopModel
{
    public FlexibleConfiguratorModel()
    {
        Attributes = new List<FlexibleAttributeModel>();
        CurrentConfiguration = new Dictionary<string, object>();
        PriceBreakdown = new FlexiblePriceBreakdownModel();
    }

    /// <summary>
    /// Gets or sets the base nopCommerce product ID
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Gets or sets the configurable product ID
    /// </summary>
    public int ConfigurableProductId { get; set; }

    /// <summary>
    /// Gets or sets the product name
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the base price
    /// </summary>
    public decimal BasePrice { get; set; }

    /// <summary>
    /// Gets or sets the primary unit of measure
    /// </summary>
    public string PrimaryUnit { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of configurable attributes
    /// </summary>
    public IList<FlexibleAttributeModel> Attributes { get; set; }

    /// <summary>
    /// Gets or sets the current configuration state
    /// </summary>
    public Dictionary<string, object> CurrentConfiguration { get; set; }

    /// <summary>
    /// Gets or sets the price breakdown
    /// </summary>
    public FlexiblePriceBreakdownModel PriceBreakdown { get; set; }

    /// <summary>
    /// Gets or sets calculated dimensions
    /// </summary>
    public Dictionary<string, decimal> Dimensions { get; set; } = new();

    /// <summary>
    /// Gets or sets configuration warnings
    /// </summary>
    public List<string> Warnings { get; set; } = new();

    /// <summary>
    /// Gets or sets validation errors
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// Gets or sets whether the configuration is valid and complete
    /// </summary>
    public bool IsValid { get; set; } = true;
}

/// <summary>
/// Flexible attribute model with cascading options
/// </summary>
public record FlexibleAttributeModel : BaseNopModel
{
    public FlexibleAttributeModel()
    {
        Options = new List<FlexibleAttributeOptionModel>();
    }

    /// <summary>
    /// Gets or sets the attribute ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the attribute name (internal)
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the attribute type
    /// </summary>
    public string AttributeType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the input type
    /// </summary>
    public string InputType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether this attribute is required
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Gets or sets the available options
    /// </summary>
    public IList<FlexibleAttributeOptionModel> Options { get; set; }

    /// <summary>
    /// Gets or sets the selected value
    /// </summary>
    public object? SelectedValue { get; set; }

    /// <summary>
    /// Gets or sets the default value
    /// </summary>
    public string DefaultValue { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the minimum value (for numeric inputs)
    /// </summary>
    public decimal? MinValue { get; set; }

    /// <summary>
    /// Gets or sets the maximum value (for numeric inputs)
    /// </summary>
    public decimal? MaxValue { get; set; }

    /// <summary>
    /// Gets or sets the step value (for numeric inputs)
    /// </summary>
    public decimal? StepValue { get; set; }

    /// <summary>
    /// Gets or sets the unit (for numeric inputs)
    /// </summary>
    public string Unit { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets validation rules
    /// </summary>
    public string ValidationRules { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the attribute is currently visible
    /// </summary>
    public bool IsVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the attribute is currently enabled
    /// </summary>
    public bool IsEnabled { get; set; } = true;
}

/// <summary>
/// Flexible attribute option with availability state
/// </summary>
public record FlexibleAttributeOptionModel : BaseNopModel
{
    /// <summary>
    /// Gets or sets the option ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the option name (internal)
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the SKU
    /// </summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether this is the default option
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// Gets or sets the image URL
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color code
    /// </summary>
    public string ColorCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets technical data
    /// </summary>
    public Dictionary<string, object> TechnicalData { get; set; } = new();

    /// <summary>
    /// Gets or sets material properties
    /// </summary>
    public Dictionary<string, object> MaterialProperties { get; set; } = new();

    /// <summary>
    /// Gets or sets whether this option is currently available (based on cascading rules)
    /// </summary>
    public bool IsAvailable { get; set; } = true;

    /// <summary>
    /// Gets or sets whether this option is currently selected
    /// </summary>
    public bool IsSelected { get; set; }

    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }
}

/// <summary>
/// Price breakdown model for customer display
/// </summary>
public record FlexiblePriceBreakdownModel : BaseNopModel
{
    public FlexiblePriceBreakdownModel()
    {
        Items = new List<FlexiblePriceItemModel>();
        Dimensions = new Dictionary<string, decimal>();
        Warnings = new List<string>();
    }

    /// <summary>
    /// Gets or sets the base price
    /// </summary>
    public decimal BasePrice { get; set; }

    /// <summary>
    /// Gets or sets the material cost
    /// </summary>
    public decimal MaterialCost { get; set; }

    /// <summary>
    /// Gets or sets the area-based cost
    /// </summary>
    public decimal AreaBasedCost { get; set; }

    /// <summary>
    /// Gets or sets the perimeter-based cost
    /// </summary>
    public decimal PerimeterBasedCost { get; set; }

    /// <summary>
    /// Gets or sets the hole cost
    /// </summary>
    public decimal HoleCost { get; set; }

    /// <summary>
    /// Gets or sets the process cost
    /// </summary>
    public decimal ProcessCost { get; set; }

    /// <summary>
    /// Gets or sets the finishing cost
    /// </summary>
    public decimal FinishingCost { get; set; }

    /// <summary>
    /// Gets or sets the setup fees
    /// </summary>
    public decimal SetupFees { get; set; }

    /// <summary>
    /// Gets or sets the quantity discount
    /// </summary>
    public decimal QuantityDiscount { get; set; }

    /// <summary>
    /// Gets or sets the total price
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Gets or sets detailed price items
    /// </summary>
    public IList<FlexiblePriceItemModel> Items { get; set; }

    /// <summary>
    /// Gets or sets calculated dimensions
    /// </summary>
    public Dictionary<string, decimal> Dimensions { get; set; }

    /// <summary>
    /// Gets or sets pricing warnings
    /// </summary>
    public List<string> Warnings { get; set; }
}

/// <summary>
/// Individual price item for detailed breakdown
/// </summary>
public record FlexiblePriceItemModel : BaseNopModel
{
    /// <summary>
    /// Gets or sets the item name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the calculated price
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the formula used (for transparency)
    /// </summary>
    public string Formula { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the component type
    /// </summary>
    public string ComponentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets calculation variables
    /// </summary>
    public Dictionary<string, object> Variables { get; set; } = new();

    /// <summary>
    /// Gets or sets whether this affects the total price
    /// </summary>
    public bool AffectsTotal { get; set; } = true;
}