using Nop.Core;

namespace Nop.Plugin.Misc.ProductConfigurator.Domain;

/// <summary>
/// Represents a configurable product (abstract product like "Sheet material")
/// </summary>
public partial class ConfigurableProduct : BaseEntity
{
    /// <summary>
    /// Gets or sets the base nopCommerce product ID
    /// </summary>
    public int ProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the configurable product
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets whether this product is configurable
    /// </summary>
    public bool IsConfigurable { get; set; } = true;
    
    /// <summary>
    /// Gets or sets the primary unit of measure (m², piece, meter, etc.)
    /// </summary>
    public string PrimaryUnit { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets secondary units (for perimeter calculations etc.)
    /// </summary>
    public string SecondaryUnits { get; set; } = string.Empty; // JSON array of additional units
    
    /// <summary>
    /// Gets or sets the base pricing method
    /// </summary>
    public PricingMethod BasePricingMethod { get; set; } = PricingMethod.PerPiece;
    
    /// <summary>
    /// Gets or sets the base price per unit
    /// </summary>
    public decimal BasePrice { get; set; }
    
    /// <summary>
    /// Gets or sets minimum dimensions/quantities
    /// </summary>
    public string MinimumConstraints { get; set; } = string.Empty; // JSON
    
    /// <summary>
    /// Gets or sets maximum dimensions/quantities
    /// </summary>
    public string MaximumConstraints { get; set; } = string.Empty; // JSON
    
    /// <summary>
    /// Gets or sets whether BOM generation is enabled
    /// </summary>
    public bool EnableBomGeneration { get; set; }
    
    /// <summary>
    /// Gets or sets BOM template data
    /// </summary>
    public string BomTemplate { get; set; } = string.Empty; // JSON
    
    /// <summary>
    /// Gets or sets production/CAM data template
    /// </summary>
    public string ProductionTemplate { get; set; } = string.Empty; // JSON
    
    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }
    
    /// <summary>
    /// Gets or sets whether the product is active
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
/// Pricing methods for configurable products
/// </summary>
public enum PricingMethod
{
    /// <summary>
    /// Fixed price per piece
    /// </summary>
    PerPiece = 1,
    
    /// <summary>
    /// Price per square meter
    /// </summary>
    PerSquareMeter = 2,
    
    /// <summary>
    /// Price per meter (perimeter)
    /// </summary>
    PerMeter = 3,
    
    /// <summary>
    /// Combined area and perimeter pricing
    /// </summary>
    AreaPlusPerimeter = 4,
    
    /// <summary>
    /// Weight-based pricing
    /// </summary>
    PerKilogram = 5,
    
    /// <summary>
    /// Volume-based pricing
    /// </summary>
    PerCubicMeter = 6,
    
    /// <summary>
    /// Custom formula-based pricing
    /// </summary>
    CustomFormula = 7
}