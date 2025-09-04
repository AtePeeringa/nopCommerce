namespace Nop.Plugin.Misc.ProductConfigurator.Models;

/// <summary>
/// Flexible price breakdown with formula-based calculations
/// </summary>
public partial class FlexiblePriceBreakdown
{
    /// <summary>
    /// Gets or sets the base price from the configurable product
    /// </summary>
    public decimal BasePrice { get; set; }
    
    /// <summary>
    /// Gets or sets the material costs
    /// </summary>
    public decimal MaterialCost { get; set; }
    
    /// <summary>
    /// Gets or sets the area-based costs (per m²)
    /// </summary>
    public decimal AreaBasedCost { get; set; }
    
    /// <summary>
    /// Gets or sets the perimeter-based costs (per meter)
    /// </summary>
    public decimal PerimeterBasedCost { get; set; }
    
    /// <summary>
    /// Gets or sets the per-hole costs
    /// </summary>
    public decimal HoleCost { get; set; }
    
    /// <summary>
    /// Gets or sets technology/process costs
    /// </summary>
    public decimal ProcessCost { get; set; }
    
    /// <summary>
    /// Gets or sets finishing costs (adhesive, coating, etc.)
    /// </summary>
    public decimal FinishingCost { get; set; }
    
    /// <summary>
    /// Gets or sets setup fees
    /// </summary>
    public decimal SetupFees { get; set; }
    
    /// <summary>
    /// Gets or sets quantity discounts (negative value)
    /// </summary>
    public decimal QuantityDiscount { get; set; }
    
    /// <summary>
    /// Gets or sets machine time costs
    /// </summary>
    public decimal MachineTimeCost { get; set; }
    
    /// <summary>
    /// Gets or sets rush order surcharges
    /// </summary>
    public decimal RushSurcharge { get; set; }
    
    /// <summary>
    /// Gets or sets the total calculated price
    /// </summary>
    public decimal TotalPrice { get; set; }
    
    /// <summary>
    /// Gets or sets detailed breakdown items
    /// </summary>
    public List<FlexiblePriceItem> Items { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the calculated dimensions used in pricing
    /// </summary>
    public Dictionary<string, decimal> Dimensions { get; set; } = new();
    
    /// <summary>
    /// Gets or sets warnings about the pricing calculation
    /// </summary>
    public List<string> Warnings { get; set; } = new();
}

/// <summary>
/// Individual price calculation item
/// </summary>
public partial class FlexiblePriceItem
{
    /// <summary>
    /// Gets or sets the component name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description/calculation details
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the calculated price
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Gets or sets the formula used
    /// </summary>
    public string Formula { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the component type
    /// </summary>
    public PriceComponentType ComponentType { get; set; }
    
    /// <summary>
    /// Gets or sets calculation variables
    /// </summary>
    public Dictionary<string, object> Variables { get; set; } = new();
    
    /// <summary>
    /// Gets or sets whether this affects the total price
    /// </summary>
    public bool AffectsTotal { get; set; } = true;
}