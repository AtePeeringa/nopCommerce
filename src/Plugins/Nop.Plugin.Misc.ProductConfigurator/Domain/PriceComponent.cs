using Nop.Core;

namespace Nop.Plugin.Misc.ProductConfigurator.Domain;

/// <summary>
/// Represents a price component for flexible pricing calculations
/// </summary>
public partial class PriceComponent : BaseEntity
{
    /// <summary>
    /// Gets or sets the configurable product ID
    /// </summary>
    public int ConfigurableProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the component name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the component type
    /// </summary>
    public PriceComponentType ComponentType { get; set; }
    
    /// <summary>
    /// Gets or sets the pricing method for this component
    /// </summary>
    public PricingMethod PricingMethod { get; set; }
    
    /// <summary>
    /// Gets or sets the conditions for when this component applies (JSON)
    /// </summary>
    public string Conditions { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the price calculation formula
    /// Examples:
    /// - "area * 15.50" (€15.50 per m²)
    /// - "perimeter * 2.30" (€2.30 per meter perimeter)
    /// - "holes * 3.50" (€3.50 per hole)
    /// - "if(thickness > 2, basePrice * 1.2, basePrice)" (20% surcharge for thickness > 2mm)
    /// - "max(area * 8.50, 25.00)" (minimum €25)
    /// </summary>
    public string Formula { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the base price for this component
    /// </summary>
    public decimal BasePrice { get; set; }
    
    /// <summary>
    /// Gets or sets the minimum price for this component
    /// </summary>
    public decimal MinimumPrice { get; set; }
    
    /// <summary>
    /// Gets or sets the maximum price for this component
    /// </summary>
    public decimal MaximumPrice { get; set; }
    
    /// <summary>
    /// Gets or sets whether this is a one-time setup fee
    /// </summary>
    public bool IsSetupFee { get; set; }
    
    /// <summary>
    /// Gets or sets whether this component affects the total or is informational only
    /// </summary>
    public bool AffectsTotal { get; set; } = true;
    
    /// <summary>
    /// Gets or sets quantity breaks (JSON array of {quantity, pricePerUnit} objects)
    /// </summary>
    public string QuantityBreaks { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the calculation priority (lower executes first)
    /// </summary>
    public int Priority { get; set; }
    
    /// <summary>
    /// Gets or sets whether the component is active
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Gets or sets the created date
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }
    
    /// <summary>
    /// Gets or sets the updated date
    /// </summary>
    public DateTime UpdatedOnUtc { get; set; }
}

/// <summary>
/// Types of price components
/// </summary>
public enum PriceComponentType
{
    /// <summary>
    /// Base material cost
    /// </summary>
    BaseMaterial = 1,
    
    /// <summary>
    /// Area-based pricing (per m²)
    /// </summary>
    AreaBased = 2,
    
    /// <summary>
    /// Perimeter-based pricing (per meter)
    /// </summary>
    PerimeterBased = 3,
    
    /// <summary>
    /// Per-hole pricing
    /// </summary>
    PerHole = 4,
    
    /// <summary>
    /// Technology/process surcharge (printing, milling, cutting)
    /// </summary>
    ProcessSurcharge = 5,
    
    /// <summary>
    /// Material thickness surcharge
    /// </summary>
    ThicknessSurcharge = 6,
    
    /// <summary>
    /// Setup/tooling fee
    /// </summary>
    SetupFee = 7,
    
    /// <summary>
    /// Quantity discount
    /// </summary>
    QuantityDiscount = 8,
    
    /// <summary>
    /// Rush order surcharge
    /// </summary>
    RushSurcharge = 9,
    
    /// <summary>
    /// Finishing cost (adhesive, coating, etc.)
    /// </summary>
    Finishing = 10,
    
    /// <summary>
    /// Machine time cost
    /// </summary>
    MachineTime = 11,
    
    /// <summary>
    /// Custom component with formula
    /// </summary>
    Custom = 12
}