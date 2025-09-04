namespace Nop.Plugin.Misc.ProductConfigurator.Models;

/// <summary>
/// Generated Bill of Materials for a configuration
/// </summary>
public partial class GeneratedBom
{
    /// <summary>
    /// Gets or sets the configurable product ID
    /// </summary>
    public int ConfigurableProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the configuration summary
    /// </summary>
    public string ConfigurationSummary { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the generated date
    /// </summary>
    public DateTime GeneratedOn { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Gets or sets the BOM items
    /// </summary>
    public List<GeneratedBomItem> Items { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the total material cost
    /// </summary>
    public decimal TotalMaterialCost { get; set; }
    
    /// <summary>
    /// Gets or sets the total weight
    /// </summary>
    public decimal TotalWeight { get; set; }
    
    /// <summary>
    /// Gets or sets the lead time in days
    /// </summary>
    public int LeadTimeDays { get; set; }
    
    /// <summary>
    /// Gets or sets critical components that may affect delivery
    /// </summary>
    public List<string> CriticalComponents { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the configuration used to generate this BOM
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the dimensions used in calculations
    /// </summary>
    public Dictionary<string, decimal> Dimensions { get; set; } = new();
    
    /// <summary>
    /// Gets or sets BOM generation warnings
    /// </summary>
    public List<string> Warnings { get; set; } = new();
}

/// <summary>
/// Individual BOM item with calculated quantities
/// </summary>
public partial class GeneratedBomItem
{
    /// <summary>
    /// Gets or sets the component ID
    /// </summary>
    public int ComponentId { get; set; }
    
    /// <summary>
    /// Gets or sets the part number
    /// </summary>
    public string PartNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the component type
    /// </summary>
    public BomComponentType ComponentType { get; set; }
    
    /// <summary>
    /// Gets or sets the required quantity
    /// </summary>
    public decimal Quantity { get; set; }
    
    /// <summary>
    /// Gets or sets the unit of measure
    /// </summary>
    public string Unit { get; set; } = "EA";
    
    /// <summary>
    /// Gets or sets the cost per unit
    /// </summary>
    public decimal CostPerUnit { get; set; }
    
    /// <summary>
    /// Gets or sets the total cost for this item
    /// </summary>
    public decimal TotalCost { get; set; }
    
    /// <summary>
    /// Gets or sets the waste factor applied
    /// </summary>
    public decimal WasteFactor { get; set; }
    
    /// <summary>
    /// Gets or sets the supplier information
    /// </summary>
    public string SupplierInfo { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the lead time in days
    /// </summary>
    public int LeadTimeDays { get; set; }
    
    /// <summary>
    /// Gets or sets whether this is a critical component
    /// </summary>
    public bool IsCritical { get; set; }
    
    /// <summary>
    /// Gets or sets the calculation details
    /// </summary>
    public string CalculationDetails { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets specifications for this item
    /// </summary>
    public Dictionary<string, object> Specifications { get; set; } = new();
}

/// <summary>
/// BOM export formats
/// </summary>
public enum BomExportFormat
{
    Csv = 1,
    Xml = 2,
    Json = 3,
    Excel = 4,
    Pdf = 5
}

/// <summary>
/// BOM validation result
/// </summary>
public partial class BomValidationResult
{
    /// <summary>
    /// Gets or sets whether the BOM is valid
    /// </summary>
    public bool IsValid { get; set; } = true;
    
    /// <summary>
    /// Gets or sets validation errors
    /// </summary>
    public List<string> Errors { get; set; } = new();
    
    /// <summary>
    /// Gets or sets validation warnings
    /// </summary>
    public List<string> Warnings { get; set; } = new();
    
    /// <summary>
    /// Gets or sets missing components
    /// </summary>
    public List<string> MissingComponents { get; set; } = new();
}