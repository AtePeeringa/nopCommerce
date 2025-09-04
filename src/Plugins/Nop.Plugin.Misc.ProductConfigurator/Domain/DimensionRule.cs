using Nop.Core;

namespace Nop.Plugin.Misc.ProductConfigurator.Domain;

public partial class DimensionRule : BaseEntity
{
    public int ProductConfigurationId { get; set; }
    
    public string DimensionType { get; set; } = string.Empty;
    
    public decimal MinValue { get; set; }
    
    public decimal MaxValue { get; set; }
    
    public decimal DefaultValue { get; set; }
    
    public decimal StepValue { get; set; } = 1;
    
    public string Unit { get; set; } = string.Empty;
    
    public decimal PricePerUnit { get; set; }
    
    public string PricingFormula { get; set; } = string.Empty;
    
    public bool IsRequired { get; set; }
    
    public int DisplayOrder { get; set; }
    
    public string ValidationRules { get; set; } = string.Empty;
    
    public DateTime CreatedOnUtc { get; set; }
    
    public DateTime UpdatedOnUtc { get; set; }
}