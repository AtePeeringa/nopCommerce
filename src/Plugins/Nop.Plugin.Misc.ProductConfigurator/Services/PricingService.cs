using System.Data;
using System.Text.Json;
using Nop.Data;
using Nop.Plugin.Misc.ProductConfigurator.Domain;
using Nop.Plugin.Misc.ProductConfigurator.Models;

namespace Nop.Plugin.Misc.ProductConfigurator.Services;

public partial class PricingService : IPricingService
{
    private readonly IRepository<PricingRule> _pricingRuleRepository;
    private readonly IProductConfigurationService _productConfigurationService;

    public PricingService(
        IRepository<PricingRule> pricingRuleRepository,
        IProductConfigurationService productConfigurationService)
    {
        _pricingRuleRepository = pricingRuleRepository;
        _productConfigurationService = productConfigurationService;
    }

    public virtual async Task<decimal> CalculateConfigurationPriceAsync(int productId, Dictionary<string, object> configuration, decimal basePrice)
    {
        var priceBreakdown = await GetPriceBreakdownAsync(productId, configuration, basePrice);
        return priceBreakdown.TotalPrice;
    }

    public virtual async Task<PriceBreakdown> GetPriceBreakdownAsync(int productId, Dictionary<string, object> configuration, decimal basePrice)
    {
        var priceBreakdown = new PriceBreakdown
        {
            BasePrice = basePrice,
            TotalPrice = basePrice
        };

        var productConfiguration = await _productConfigurationService.GetProductConfigurationByProductIdAsync(productId);
        if (productConfiguration == null)
            return priceBreakdown;

        await CalculateMaterialPriceAsync(productConfiguration.Id, configuration, priceBreakdown);
        await CalculateDimensionPriceAsync(productConfiguration.Id, configuration, priceBreakdown);
        await CalculateFeaturePriceAsync(productConfiguration.Id, configuration, priceBreakdown);
        await CalculateManufacturingPriceAsync(productConfiguration.Id, configuration, priceBreakdown);
        await ApplyPricingRulesAsync(productConfiguration.Id, configuration, priceBreakdown);

        priceBreakdown.TotalPrice = priceBreakdown.BasePrice + priceBreakdown.MaterialPrice + 
                                   priceBreakdown.DimensionPrice + priceBreakdown.FeaturePrice + 
                                   priceBreakdown.ManufacturingPrice + priceBreakdown.SetupFee;

        return priceBreakdown;
    }

    public virtual async Task<IList<PricingRule>> GetPricingRulesByProductConfigurationIdAsync(int productConfigurationId)
    {
        var query = _pricingRuleRepository.Table
            .Where(pr => pr.ProductConfigurationId == productConfigurationId && pr.IsActive)
            .OrderBy(pr => pr.Priority);

        return await query.ToListAsync();
    }

    public virtual async Task<PricingRule?> GetPricingRuleByIdAsync(int id)
    {
        return await _pricingRuleRepository.GetByIdAsync(id);
    }

    public virtual async Task InsertPricingRuleAsync(PricingRule pricingRule)
    {
        await _pricingRuleRepository.InsertAsync(pricingRule);
    }

    public virtual async Task UpdatePricingRuleAsync(PricingRule pricingRule)
    {
        await _pricingRuleRepository.UpdateAsync(pricingRule);
    }

    public virtual async Task DeletePricingRuleAsync(PricingRule pricingRule)
    {
        await _pricingRuleRepository.DeleteAsync(pricingRule);
    }

    public virtual async Task<decimal> CalculateDimensionPriceAsync(DimensionRule dimensionRule, decimal value)
    {
        if (string.IsNullOrEmpty(dimensionRule.PricingFormula))
        {
            return value * dimensionRule.PricePerUnit;
        }

        return await EvaluatePricingFormulaAsync(dimensionRule.PricingFormula, new Dictionary<string, object> 
        { 
            { "value", value },
            { "pricePerUnit", dimensionRule.PricePerUnit }
        });
    }

    public virtual async Task<decimal> ApplyPricingRuleAsync(PricingRule pricingRule, Dictionary<string, object> configuration, decimal currentPrice)
    {
        if (!await EvaluateConditionsAsync(pricingRule.Conditions, configuration))
            return currentPrice;

        var variables = new Dictionary<string, object>(configuration)
        {
            { "currentPrice", currentPrice },
            { "basePrice", currentPrice }
        };

        return await EvaluatePricingFormulaAsync(pricingRule.Formula, variables);
    }

    private async Task CalculateMaterialPriceAsync(int productConfigurationId, Dictionary<string, object> configuration, PriceBreakdown priceBreakdown)
    {
        var configurationGroups = await _productConfigurationService.GetConfigurationGroupsByProductConfigurationIdAsync(productConfigurationId);
        var materialGroups = configurationGroups.Where(cg => cg.GroupType == ConfigurationGroupType.Material);

        foreach (var group in materialGroups)
        {
            var groupKey = $"Group_{group.Id}";
            if (configuration.TryGetValue(groupKey, out var optionIdValue) && 
                int.TryParse(optionIdValue?.ToString(), out var optionId))
            {
                var option = await _productConfigurationService.GetConfigurationOptionByIdAsync(optionId);
                if (option != null)
                {
                    var adjustmentAmount = CalculatePriceAdjustment(option, priceBreakdown.BasePrice);
                    priceBreakdown.MaterialPrice += adjustmentAmount;
                    
                    priceBreakdown.Items.Add(new PriceBreakdownItem
                    {
                        Name = option.DisplayName,
                        Description = option.Description,
                        Price = adjustmentAmount,
                        Category = "Material"
                    });
                }
            }
        }
    }

    private async Task CalculateDimensionPriceAsync(int productConfigurationId, Dictionary<string, object> configuration, PriceBreakdown priceBreakdown)
    {
        var dimensionRules = await _productConfigurationService.GetDimensionRulesByProductConfigurationIdAsync(productConfigurationId);

        foreach (var dimensionRule in dimensionRules)
        {
            var dimensionKey = $"Dimension_{dimensionRule.DimensionType}";
            if (configuration.TryGetValue(dimensionKey, out var dimensionValue) && 
                decimal.TryParse(dimensionValue?.ToString(), out var value))
            {
                var dimensionPrice = await CalculateDimensionPriceAsync(dimensionRule, value);
                priceBreakdown.DimensionPrice += dimensionPrice;
                
                priceBreakdown.Items.Add(new PriceBreakdownItem
                {
                    Name = $"{dimensionRule.DimensionType} ({value} {dimensionRule.Unit})",
                    Description = $"Dimension pricing for {dimensionRule.DimensionType}",
                    Price = dimensionPrice,
                    Category = "Dimension"
                });
            }
        }
    }

    private async Task CalculateFeaturePriceAsync(int productConfigurationId, Dictionary<string, object> configuration, PriceBreakdown priceBreakdown)
    {
        var configurationGroups = await _productConfigurationService.GetConfigurationGroupsByProductConfigurationIdAsync(productConfigurationId);
        var featureGroups = configurationGroups.Where(cg => cg.GroupType == ConfigurationGroupType.Feature);

        foreach (var group in featureGroups)
        {
            var groupKey = $"Group_{group.Id}";
            if (configuration.TryGetValue(groupKey, out var optionIdValue) && 
                int.TryParse(optionIdValue?.ToString(), out var optionId))
            {
                var option = await _productConfigurationService.GetConfigurationOptionByIdAsync(optionId);
                if (option != null)
                {
                    var adjustmentAmount = CalculatePriceAdjustment(option, priceBreakdown.BasePrice);
                    priceBreakdown.FeaturePrice += adjustmentAmount;
                    
                    priceBreakdown.Items.Add(new PriceBreakdownItem
                    {
                        Name = option.DisplayName,
                        Description = option.Description,
                        Price = adjustmentAmount,
                        Category = "Feature"
                    });
                }
            }
        }
    }

    private async Task CalculateManufacturingPriceAsync(int productConfigurationId, Dictionary<string, object> configuration, PriceBreakdown priceBreakdown)
    {
        var configurationGroups = await _productConfigurationService.GetConfigurationGroupsByProductConfigurationIdAsync(productConfigurationId);
        var manufacturingGroups = configurationGroups.Where(cg => cg.GroupType == ConfigurationGroupType.Manufacturing);

        foreach (var group in manufacturingGroups)
        {
            var groupKey = $"Group_{group.Id}";
            if (configuration.TryGetValue(groupKey, out var optionIdValue) && 
                int.TryParse(optionIdValue?.ToString(), out var optionId))
            {
                var option = await _productConfigurationService.GetConfigurationOptionByIdAsync(optionId);
                if (option != null)
                {
                    var adjustmentAmount = CalculatePriceAdjustment(option, priceBreakdown.BasePrice);
                    priceBreakdown.ManufacturingPrice += adjustmentAmount;
                    
                    priceBreakdown.Items.Add(new PriceBreakdownItem
                    {
                        Name = option.DisplayName,
                        Description = option.Description,
                        Price = adjustmentAmount,
                        Category = "Manufacturing"
                    });
                }
            }
        }
    }

    private async Task ApplyPricingRulesAsync(int productConfigurationId, Dictionary<string, object> configuration, PriceBreakdown priceBreakdown)
    {
        var pricingRules = await GetPricingRulesByProductConfigurationIdAsync(productConfigurationId);

        foreach (var rule in pricingRules)
        {
            var rulePrice = await ApplyPricingRuleAsync(rule, configuration, priceBreakdown.TotalPrice);
            
            if (rule.RuleType == PricingRuleType.SetupFee)
            {
                priceBreakdown.SetupFee += rulePrice;
            }
            else
            {
                var adjustment = rulePrice - priceBreakdown.TotalPrice;
                if (Math.Abs(adjustment) > 0.01m)
                {
                    priceBreakdown.Items.Add(new PriceBreakdownItem
                    {
                        Name = rule.Name,
                        Description = rule.Description,
                        Price = adjustment,
                        Category = "Pricing Rule"
                    });
                }
            }
        }
    }

    private static decimal CalculatePriceAdjustment(ConfigurationOption option, decimal basePrice)
    {
        return option.PriceAdjustmentType switch
        {
            PriceAdjustmentType.FixedAmount => option.PriceAdjustment,
            PriceAdjustmentType.Percentage => basePrice * (option.PriceAdjustment / 100m),
            PriceAdjustmentType.Multiplier => basePrice * option.PriceAdjustment,
            _ => option.PriceAdjustment
        };
    }

    private static async Task<decimal> EvaluatePricingFormulaAsync(string formula, Dictionary<string, object> variables)
    {
        if (string.IsNullOrEmpty(formula))
            return 0;

        try
        {
            var table = new DataTable();
            var expression = formula;

            foreach (var variable in variables)
            {
                expression = expression.Replace($"{{{variable.Key}}}", variable.Value.ToString());
            }

            var result = table.Compute(expression, null);
            return Convert.ToDecimal(result);
        }
        catch
        {
            return 0;
        }
    }

    private static async Task<bool> EvaluateConditionsAsync(string conditions, Dictionary<string, object> configuration)
    {
        if (string.IsNullOrEmpty(conditions))
            return true;

        try
        {
            var conditionRules = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(conditions);
            if (conditionRules == null)
                return true;

            return conditionRules.All(condition => EvaluateCondition(condition, configuration));
        }
        catch
        {
            return true;
        }
    }

    private static bool EvaluateCondition(Dictionary<string, object> condition, Dictionary<string, object> configuration)
    {
        if (!condition.TryGetValue("field", out var fieldObj) || 
            !condition.TryGetValue("operator", out var operatorObj) || 
            !condition.TryGetValue("value", out var expectedValue))
            return true;

        var field = fieldObj.ToString();
        var operatorType = operatorObj.ToString();

        if (!configuration.TryGetValue(field, out var actualValue))
            return false;

        return operatorType switch
        {
            "equals" => actualValue.ToString() == expectedValue.ToString(),
            "not_equals" => actualValue.ToString() != expectedValue.ToString(),
            "contains" => actualValue.ToString().Contains(expectedValue.ToString(), StringComparison.OrdinalIgnoreCase),
            "greater_than" => decimal.TryParse(actualValue.ToString(), out var actual1) && 
                            decimal.TryParse(expectedValue.ToString(), out var expected1) && 
                            actual1 > expected1,
            "less_than" => decimal.TryParse(actualValue.ToString(), out var actual2) && 
                         decimal.TryParse(expectedValue.ToString(), out var expected2) && 
                         actual2 < expected2,
            _ => true
        };
    }
}