using Nop.Core;

namespace Nop.Plugin.Misc.ProductConfigurator.Domain;

public partial class PricingRule : BaseEntity
{
    public int ProductConfigurationId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public PricingRuleType RuleType { get; set; }
    
    public string Conditions { get; set; } = string.Empty;
    
    public string Formula { get; set; } = string.Empty;
    
    public decimal MinimumPrice { get; set; }
    
    public decimal MaximumPrice { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public int Priority { get; set; }
    
    public DateTime CreatedOnUtc { get; set; }
    
    public DateTime UpdatedOnUtc { get; set; }
}

public enum PricingRuleType
{
    BasePrice = 1,
    MaterialMultiplier = 2,
    SizeMultiplier = 3,
    ComplexityMultiplier = 4,
    QuantityDiscount = 5,
    SetupFee = 6,
    ConditionalAdjustment = 7
}