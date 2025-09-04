using Nop.Core;

namespace Nop.Plugin.Misc.ProductConfigurator.Domain;

public partial class ConfigurationOption : BaseEntity
{
    public int ConfigurationGroupId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string DisplayName { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public decimal PriceAdjustment { get; set; }
    
    public PriceAdjustmentType PriceAdjustmentType { get; set; }
    
    public bool IsDefault { get; set; }
    
    public int DisplayOrder { get; set; }
    
    public string ImageUrl { get; set; } = string.Empty;
    
    public string AdditionalData { get; set; } = string.Empty;
    
    public string DependsOnOptions { get; set; } = string.Empty;
    
    public string ConflictingOptions { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedOnUtc { get; set; }
    
    public DateTime UpdatedOnUtc { get; set; }
}

public enum PriceAdjustmentType
{
    FixedAmount = 1,
    Percentage = 2,
    Multiplier = 3,
    PerUnit = 4
}