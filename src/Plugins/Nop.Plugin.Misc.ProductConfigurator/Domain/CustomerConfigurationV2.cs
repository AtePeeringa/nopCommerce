using Nop.Core;

namespace Nop.Plugin.Misc.ProductConfigurator.Domain;

/// <summary>
/// Represents a customer's product configuration
/// </summary>
public partial class CustomerConfigurationV2 : BaseEntity
{
    /// <summary>
    /// Gets or sets the customer ID
    /// </summary>
    public int CustomerId { get; set; }
    
    /// <summary>
    /// Gets or sets the configurable product ID
    /// </summary>
    public int ConfigurableProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the base nopCommerce product ID
    /// </summary>
    public int ProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the configuration name (for saved configurations)
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the selected attribute values (JSON)
    /// Format: {"Material": "Aluminum", "Thickness": "2", "Width": "100", "Height": "50", "Holes": "4", "Technology": "Laser"}
    /// </summary>
    public string SelectedValues { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets calculated dimensions (JSON)
    /// Format: {"area": 0.005, "perimeter": 0.3, "volume": 0.00001}
    /// </summary>
    public string CalculatedDimensions { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the total calculated price
    /// </summary>
    public decimal TotalPrice { get; set; }
    
    /// <summary>
    /// Gets or sets the detailed price breakdown (JSON)
    /// </summary>
    public string PriceBreakdown { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the generated BOM data (JSON)
    /// </summary>
    public string BomData { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the production/CAM data (JSON)
    /// </summary>
    public string ProductionData { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the configuration summary text
    /// </summary>
    public string Summary { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets whether the configuration is complete and valid
    /// </summary>
    public bool IsComplete { get; set; }
    
    /// <summary>
    /// Gets or sets whether this is a saved/named configuration
    /// </summary>
    public bool IsSaved { get; set; }
    
    /// <summary>
    /// Gets or sets whether the configuration has been added to cart
    /// </summary>
    public bool IsInCart { get; set; }
    
    /// <summary>
    /// Gets or sets the order item ID if purchased
    /// </summary>
    public int? OrderItemId { get; set; }
    
    /// <summary>
    /// Gets or sets validation results (JSON)
    /// </summary>
    public string ValidationResults { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the created date
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }
    
    /// <summary>
    /// Gets or sets the updated date
    /// </summary>
    public DateTime UpdatedOnUtc { get; set; }
}