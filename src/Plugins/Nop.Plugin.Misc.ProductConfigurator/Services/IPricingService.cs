using Nop.Plugin.Misc.ProductConfigurator.Domain;
using Nop.Plugin.Misc.ProductConfigurator.Models;

namespace Nop.Plugin.Misc.ProductConfigurator.Services;

public partial interface IPricingService
{
    Task<decimal> CalculateConfigurationPriceAsync(int productId, Dictionary<string, object> configuration, decimal basePrice);
    
    Task<PriceBreakdown> GetPriceBreakdownAsync(int productId, Dictionary<string, object> configuration, decimal basePrice);
    
    Task<IList<PricingRule>> GetPricingRulesByProductConfigurationIdAsync(int productConfigurationId);
    
    Task<PricingRule?> GetPricingRuleByIdAsync(int id);
    
    Task InsertPricingRuleAsync(PricingRule pricingRule);
    
    Task UpdatePricingRuleAsync(PricingRule pricingRule);
    
    Task DeletePricingRuleAsync(PricingRule pricingRule);
    
    Task<decimal> CalculateDimensionPriceAsync(DimensionRule dimensionRule, decimal value);
    
    Task<decimal> ApplyPricingRuleAsync(PricingRule pricingRule, Dictionary<string, object> configuration, decimal currentPrice);
}