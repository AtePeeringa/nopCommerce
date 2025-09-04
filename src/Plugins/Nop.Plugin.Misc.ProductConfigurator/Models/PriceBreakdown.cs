namespace Nop.Plugin.Misc.ProductConfigurator.Models;

public partial class PriceBreakdown
{
    public decimal BasePrice { get; set; }
    
    public decimal MaterialPrice { get; set; }
    
    public decimal DimensionPrice { get; set; }
    
    public decimal FeaturePrice { get; set; }
    
    public decimal ManufacturingPrice { get; set; }
    
    public decimal SetupFee { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public List<PriceBreakdownItem> Items { get; set; } = new();
}

public partial class PriceBreakdownItem
{
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    
    public string Category { get; set; } = string.Empty;
}