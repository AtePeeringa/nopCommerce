using Nop.Core;

namespace Nop.Plugin.Misc.ProductConfigurator.Domain;

/// <summary>
/// Represents an option for a product attribute (e.g., Aluminum, Stainless Steel for Material attribute)
/// </summary>
public partial class AttributeOption : BaseEntity
{
    /// <summary>
    /// Gets or sets the product attribute ID
    /// </summary>
    public int ProductAttributeId { get; set; }
    
    /// <summary>
    /// Gets or sets the option name/value
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
    /// Gets or sets the SKU/part number for this option
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
    /// Gets or sets the color code (for visual representation)
    /// </summary>
    public string ColorCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets additional technical data (JSON)
    /// </summary>
    public string TechnicalData { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets material properties (density, thickness options, etc.) (JSON)
    /// </summary>
    public string MaterialProperties { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets BOM data for this option (JSON)
    /// </summary>
    public string BomData { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets production data (machining parameters, etc.) (JSON)
    /// </summary>
    public string ProductionData { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }
    
    /// <summary>
    /// Gets or sets whether the option is active
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