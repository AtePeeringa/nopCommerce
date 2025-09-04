using Nop.Core;

namespace Nop.Plugin.Misc.ProductConfigurator.Domain;

public partial class ProductConfiguration : BaseEntity
{
    public int ProductId { get; set; }
    
    public bool IsConfigurable { get; set; }
    
    public string ConfigurationGroups { get; set; } = string.Empty;
    
    public string PricingRules { get; set; } = string.Empty;
    
    public bool EnableCascadingParameters { get; set; }
    
    public DateTime CreatedOnUtc { get; set; }
    
    public DateTime UpdatedOnUtc { get; set; }
}