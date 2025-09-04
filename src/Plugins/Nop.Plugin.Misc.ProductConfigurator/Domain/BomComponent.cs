using Nop.Core;

namespace Nop.Plugin.Misc.ProductConfigurator.Domain;

/// <summary>
/// Represents a Bill of Materials component
/// </summary>
public partial class BomComponent : BaseEntity
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
    public BomComponentType ComponentType { get; set; }
    
    /// <summary>
    /// Gets or sets the part number/SKU
    /// </summary>
    public string PartNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the supplier information (JSON)
    /// </summary>
    public string SupplierInfo { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the conditions when this component is required (JSON)
    /// </summary>
    public string RequiredConditions { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the quantity formula
    /// Examples: "1" (fixed), "area / 0.25" (sheets needed), "holes" (fasteners)
    /// </summary>
    public string QuantityFormula { get; set; } = "1";
    
    /// <summary>
    /// Gets or sets the unit of measure
    /// </summary>
    public string Unit { get; set; } = "EA";
    
    /// <summary>
    /// Gets or sets the cost per unit
    /// </summary>
    public decimal CostPerUnit { get; set; }
    
    /// <summary>
    /// Gets or sets the waste factor (e.g., 1.1 for 10% waste)
    /// </summary>
    public decimal WasteFactor { get; set; } = 1.0m;
    
    /// <summary>
    /// Gets or sets the lead time in days
    /// </summary>
    public int LeadTimeDays { get; set; }
    
    /// <summary>
    /// Gets or sets whether this component is critical for production
    /// </summary>
    public bool IsCritical { get; set; }
    
    /// <summary>
    /// Gets or sets additional specifications (JSON)
    /// </summary>
    public string Specifications { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }
    
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
/// Types of BOM components
/// </summary>
public enum BomComponentType
{
    /// <summary>
    /// Raw material (sheet, rod, plate)
    /// </summary>
    RawMaterial = 1,
    
    /// <summary>
    /// Fasteners (screws, bolts, rivets)
    /// </summary>
    Fastener = 2,
    
    /// <summary>
    /// Adhesive or bonding agent
    /// </summary>
    Adhesive = 3,
    
    /// <summary>
    /// Finishing material (paint, coating, anodizing)
    /// </summary>
    Finishing = 4,
    
    /// <summary>
    /// Consumable (cutting fluid, abrasives)
    /// </summary>
    Consumable = 5,
    
    /// <summary>
    /// Packaging material
    /// </summary>
    Packaging = 6,
    
    /// <summary>
    /// Hardware (mounting brackets, spacers)
    /// </summary>
    Hardware = 7,
    
    /// <summary>
    /// Label or marking material
    /// </summary>
    Marking = 8
}