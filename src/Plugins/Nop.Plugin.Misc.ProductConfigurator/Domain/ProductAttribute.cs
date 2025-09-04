using Nop.Core;

namespace Nop.Plugin.Misc.ProductConfigurator.Domain;

/// <summary>
/// Represents a product attribute (Material, Thickness, Structure, Holes, Adhesive, Technology, etc.)
/// </summary>
public partial class ProductAttribute : BaseEntity
{
    /// <summary>
    /// Gets or sets the configurable product ID
    /// </summary>
    public int ConfigurableProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the attribute name (Material, Thickness, etc.)
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
    public AttributeType AttributeType { get; set; }
    
    /// <summary>
    /// Gets or sets the input type for the attribute
    /// </summary>
    public AttributeInputType InputType { get; set; }
    
    /// <summary>
    /// Gets or sets whether this attribute is required
    /// </summary>
    public bool IsRequired { get; set; }
    
    /// <summary>
    /// Gets or sets whether this attribute affects pricing
    /// </summary>
    public bool AffectsPricing { get; set; } = true;
    
    /// <summary>
    /// Gets or sets whether this attribute affects BOM
    /// </summary>
    public bool AffectsBom { get; set; } = true;
    
    /// <summary>
    /// Gets or sets whether this attribute affects production
    /// </summary>
    public bool AffectsProduction { get; set; } = true;
    
    /// <summary>
    /// Gets or sets validation rules (JSON)
    /// </summary>
    public string ValidationRules { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets default value (JSON)
    /// </summary>
    public string DefaultValue { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the display order
    /// </summary>
    public int DisplayOrder { get; set; }
    
    /// <summary>
    /// Gets or sets whether the attribute is active
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
/// Attribute types for categorization
/// </summary>
public enum AttributeType
{
    /// <summary>
    /// Material selection (Aluminum, Steel, Acrylic)
    /// </summary>
    Material = 1,
    
    /// <summary>
    /// Thickness/dimensions
    /// </summary>
    Dimension = 2,
    
    /// <summary>
    /// Structure/finish options
    /// </summary>
    Structure = 3,
    
    /// <summary>
    /// Hole patterns/modifications
    /// </summary>
    Modification = 4,
    
    /// <summary>
    /// Adhesive/mounting options
    /// </summary>
    Mounting = 5,
    
    /// <summary>
    /// Technology/manufacturing process
    /// </summary>
    Technology = 6,
    
    /// <summary>
    /// Quantity/measurements
    /// </summary>
    Quantity = 7,
    
    /// <summary>
    /// General feature
    /// </summary>
    Feature = 8
}

/// <summary>
/// Input types for attributes
/// </summary>
public enum AttributeInputType
{
    /// <summary>
    /// Single selection dropdown/radio
    /// </summary>
    SingleSelect = 1,
    
    /// <summary>
    /// Multiple selection checkboxes
    /// </summary>
    MultiSelect = 2,
    
    /// <summary>
    /// Numeric input
    /// </summary>
    Numeric = 3,
    
    /// <summary>
    /// Text input
    /// </summary>
    Text = 4,
    
    /// <summary>
    /// Boolean checkbox
    /// </summary>
    Boolean = 5,
    
    /// <summary>
    /// Date picker
    /// </summary>
    Date = 6,
    
    /// <summary>
    /// File upload
    /// </summary>
    File = 7,
    
    /// <summary>
    /// Color picker
    /// </summary>
    Color = 8,
    
    /// <summary>
    /// Range slider
    /// </summary>
    Range = 9
}