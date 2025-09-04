using Nop.Core;

namespace Nop.Plugin.Misc.ProductConfigurator.Domain;

public partial class CustomerConfiguration : BaseEntity
{
    public int CustomerId { get; set; }
    
    public int ProductId { get; set; }
    
    public string ConfigurationData { get; set; } = string.Empty;
    
    public decimal TotalPrice { get; set; }
    
    public string ConfigurationSummary { get; set; } = string.Empty;
    
    public bool IsCompleted { get; set; }
    
    public DateTime CreatedOnUtc { get; set; }
    
    public DateTime UpdatedOnUtc { get; set; }
}