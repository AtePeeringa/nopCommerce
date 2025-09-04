using System.Text.Json;
using Nop.Data;
using Nop.Plugin.Misc.ProductConfigurator.Domain;
using Nop.Plugin.Misc.ProductConfigurator.Models;
using System.Text.RegularExpressions;

namespace Nop.Plugin.Misc.ProductConfigurator.Services;

/// <summary>
/// Implementation of rules engine service
/// </summary>
public partial class RulesEngineService : IRulesEngineService
{
    private readonly IRepository<ConfigurationRule> _ruleRepository;
    private readonly IRepository<ProductAttribute> _attributeRepository;
    private readonly IRepository<AttributeOption> _optionRepository;

    public RulesEngineService(
        IRepository<ConfigurationRule> ruleRepository,
        IRepository<ProductAttribute> attributeRepository,
        IRepository<AttributeOption> optionRepository)
    {
        _ruleRepository = ruleRepository;
        _attributeRepository = attributeRepository;
        _optionRepository = optionRepository;
    }

    public virtual async Task<IList<AttributeOption>> GetAvailableOptionsAsync(int configurableProductId, string attributeName, Dictionary<string, object> currentConfiguration)
    {
        // Get the attribute
        var attribute = await _attributeRepository.Table
            .FirstOrDefaultAsync(a => a.ConfigurableProductId == configurableProductId && a.Name == attributeName);
        
        if (attribute == null)
            return new List<AttributeOption>();

        // Get all options for this attribute
        var allOptions = await _optionRepository.Table
            .Where(o => o.ProductAttributeId == attribute.Id && o.IsActive)
            .OrderBy(o => o.DisplayOrder)
            .ToListAsync();

        // Get applicable rules
        var rules = await GetApplicableRulesAsync(configurableProductId, currentConfiguration);
        
        var availableOptions = new List<AttributeOption>(allOptions);
        
        // Apply rules to filter options
        foreach (var rule in rules.Where(r => r.TargetAttribute == attributeName && r.IsActive))
        {
            var ruleResult = await ExecuteRuleAsync(rule, currentConfiguration);
            
            switch (rule.Action)
            {
                case RuleAction.Exclude:
                case RuleAction.Hide:
                    if (ruleResult.HiddenOptions.Any())
                    {
                        availableOptions.RemoveAll(o => ruleResult.HiddenOptions.Contains(o.Id));
                    }
                    break;
                    
                case RuleAction.ShowOnly:
                    if (ruleResult.ShowOnlyOptions.Any())
                    {
                        availableOptions = availableOptions.Where(o => ruleResult.ShowOnlyOptions.Contains(o.Id)).ToList();
                    }
                    break;
            }
        }

        return availableOptions;
    }

    public virtual async Task<ConfigurationValidationResult> ValidateConfigurationAsync(int configurableProductId, Dictionary<string, object> configuration)
    {
        var result = new ConfigurationValidationResult { IsValid = true };

        // Get all validation rules
        var rules = await _ruleRepository.Table
            .Where(r => r.ConfigurableProductId == configurableProductId && 
                       r.RuleType == RuleType.Validation && 
                       r.IsActive)
            .OrderBy(r => r.Priority)
            .ToListAsync();

        foreach (var rule in rules)
        {
            if (await EvaluateRuleAsync(rule, configuration))
            {
                var ruleResult = await ExecuteRuleAsync(rule, configuration);
                
                if (ruleResult.Errors.Any())
                {
                    result.IsValid = false;
                    result.Errors.AddRange(ruleResult.Errors);
                }
                
                if (ruleResult.Warnings.Any())
                {
                    result.Warnings.AddRange(ruleResult.Warnings);
                }
            }
        }

        // Validate required attributes
        var attributes = await _attributeRepository.Table
            .Where(a => a.ConfigurableProductId == configurableProductId && a.IsRequired && a.IsActive)
            .ToListAsync();

        foreach (var attribute in attributes)
        {
            if (!configuration.ContainsKey(attribute.Name) || 
                string.IsNullOrEmpty(configuration[attribute.Name]?.ToString()))
            {
                result.IsValid = false;
                result.Errors.Add($"{attribute.DisplayName} is required");
            }
        }

        return result;
    }

    public virtual async Task<Dictionary<string, object>> ApplyAutoSetRulesAsync(int configurableProductId, Dictionary<string, object> configuration)
    {
        var modifiedConfiguration = new Dictionary<string, object>(configuration);

        var autoSetRules = await _ruleRepository.Table
            .Where(r => r.ConfigurableProductId == configurableProductId && 
                       r.RuleType == RuleType.AutoSet && 
                       r.IsActive)
            .OrderBy(r => r.Priority)
            .ToListAsync();

        foreach (var rule in autoSetRules)
        {
            if (await EvaluateRuleAsync(rule, modifiedConfiguration))
            {
                var ruleResult = await ExecuteRuleAsync(rule, modifiedConfiguration);
                
                // Merge the modified configuration
                foreach (var kvp in ruleResult.ModifiedConfiguration)
                {
                    modifiedConfiguration[kvp.Key] = kvp.Value;
                }
            }
        }

        return modifiedConfiguration;
    }

    public virtual async Task<IList<ConfigurationRule>> GetApplicableRulesAsync(int configurableProductId, Dictionary<string, object> configuration)
    {
        var allRules = await _ruleRepository.Table
            .Where(r => r.ConfigurableProductId == configurableProductId && r.IsActive)
            .OrderBy(r => r.Priority)
            .ToListAsync();

        var applicableRules = new List<ConfigurationRule>();

        foreach (var rule in allRules)
        {
            if (await EvaluateRuleAsync(rule, configuration))
            {
                applicableRules.Add(rule);
            }
        }

        return applicableRules;
    }

    public virtual async Task<bool> EvaluateRuleAsync(ConfigurationRule rule, Dictionary<string, object> configuration)
    {
        if (string.IsNullOrEmpty(rule.Conditions))
            return true;

        try
        {
            var conditions = JsonSerializer.Deserialize<List<RuleCondition>>(rule.Conditions);
            if (conditions == null || !conditions.Any())
                return true;

            // All conditions must be true (AND logic)
            foreach (var condition in conditions)
            {
                if (!EvaluateCondition(condition, configuration))
                    return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public virtual async Task<RuleExecutionResult> ExecuteRuleAsync(ConfigurationRule rule, Dictionary<string, object> configuration)
    {
        var result = new RuleExecutionResult
        {
            ModifiedConfiguration = new Dictionary<string, object>(configuration)
        };

        try
        {
            switch (rule.Action)
            {
                case RuleAction.SetValue:
                    await ExecuteSetValueAction(rule, result);
                    break;
                    
                case RuleAction.Exclude:
                case RuleAction.Hide:
                    await ExecuteHideAction(rule, result);
                    break;
                    
                case RuleAction.ShowOnly:
                    await ExecuteShowOnlyAction(rule, result);
                    break;
                    
                case RuleAction.LimitRange:
                    await ExecuteLimitRangeAction(rule, result);
                    break;
                    
                case RuleAction.ShowWarning:
                    result.Warnings.Add(rule.ErrorMessage);
                    break;
                    
                case RuleAction.ShowError:
                    result.Errors.Add(rule.ErrorMessage);
                    break;
                    
                case RuleAction.CustomAction:
                    await ExecuteCustomAction(rule, result);
                    break;
            }
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = ex.Message;
        }

        return result;
    }

    public virtual async Task<IList<string>> GetConfigurationWarningsAsync(int configurableProductId, Dictionary<string, object> configuration)
    {
        var warnings = new List<string>();

        var rules = await GetApplicableRulesAsync(configurableProductId, configuration);
        
        foreach (var rule in rules.Where(r => r.Action == RuleAction.ShowWarning))
        {
            var ruleResult = await ExecuteRuleAsync(rule, configuration);
            warnings.AddRange(ruleResult.Warnings);
        }

        return warnings;
    }

    public virtual async Task<IList<ConfigurationRule>> GetRulesAsync(int configurableProductId)
    {
        return await _ruleRepository.Table
            .Where(r => r.ConfigurableProductId == configurableProductId)
            .OrderBy(r => r.Priority)
            .ToListAsync();
    }

    public virtual async Task InsertRuleAsync(ConfigurationRule rule)
    {
        await _ruleRepository.InsertAsync(rule);
    }

    public virtual async Task UpdateRuleAsync(ConfigurationRule rule)
    {
        await _ruleRepository.UpdateAsync(rule);
    }

    public virtual async Task DeleteRuleAsync(ConfigurationRule rule)
    {
        await _ruleRepository.DeleteAsync(rule);
    }

    #region Utilities

    private static bool EvaluateCondition(RuleCondition condition, Dictionary<string, object> configuration)
    {
        if (!configuration.TryGetValue(condition.Attribute, out var actualValue))
            return false;

        var actualString = actualValue?.ToString() ?? string.Empty;
        var expectedString = condition.Value?.ToString() ?? string.Empty;

        return condition.Operator.ToLower() switch
        {
            "equals" => string.Equals(actualString, expectedString, StringComparison.OrdinalIgnoreCase),
            "not_equals" => !string.Equals(actualString, expectedString, StringComparison.OrdinalIgnoreCase),
            "contains" => actualString.Contains(expectedString, StringComparison.OrdinalIgnoreCase),
            "not_contains" => !actualString.Contains(expectedString, StringComparison.OrdinalIgnoreCase),
            "starts_with" => actualString.StartsWith(expectedString, StringComparison.OrdinalIgnoreCase),
            "ends_with" => actualString.EndsWith(expectedString, StringComparison.OrdinalIgnoreCase),
            "greater_than" => decimal.TryParse(actualString, out var actualDecimal1) && 
                            decimal.TryParse(expectedString, out var expectedDecimal1) && 
                            actualDecimal1 > expectedDecimal1,
            "less_than" => decimal.TryParse(actualString, out var actualDecimal2) && 
                         decimal.TryParse(expectedString, out var expectedDecimal2) && 
                         actualDecimal2 < expectedDecimal2,
            "greater_equal" => decimal.TryParse(actualString, out var actualDecimal3) && 
                             decimal.TryParse(expectedString, out var expectedDecimal3) && 
                             actualDecimal3 >= expectedDecimal3,
            "less_equal" => decimal.TryParse(actualString, out var actualDecimal4) && 
                          decimal.TryParse(expectedString, out var expectedDecimal4) && 
                          actualDecimal4 <= expectedDecimal4,
            "in" => expectedString.Split(',').Select(s => s.Trim()).Contains(actualString, StringComparer.OrdinalIgnoreCase),
            "not_in" => !expectedString.Split(',').Select(s => s.Trim()).Contains(actualString, StringComparer.OrdinalIgnoreCase),
            "regex" => Regex.IsMatch(actualString, expectedString, RegexOptions.IgnoreCase),
            _ => false
        };
    }

    private async Task ExecuteSetValueAction(ConfigurationRule rule, RuleExecutionResult result)
    {
        if (string.IsNullOrEmpty(rule.RuleParameters))
            return;

        var parameters = JsonSerializer.Deserialize<Dictionary<string, object>>(rule.RuleParameters);
        if (parameters == null || !parameters.TryGetValue("value", out var value))
            return;

        result.ModifiedConfiguration[rule.TargetAttribute] = value;
        result.AffectedAttributes.Add(rule.TargetAttribute);
    }

    private async Task ExecuteHideAction(ConfigurationRule rule, RuleExecutionResult result)
    {
        if (string.IsNullOrEmpty(rule.TargetOptions))
            return;

        var targetOptions = JsonSerializer.Deserialize<List<int>>(rule.TargetOptions);
        if (targetOptions != null)
        {
            result.HiddenOptions.AddRange(targetOptions);
        }
    }

    private async Task ExecuteShowOnlyAction(ConfigurationRule rule, RuleExecutionResult result)
    {
        if (string.IsNullOrEmpty(rule.TargetOptions))
            return;

        var targetOptions = JsonSerializer.Deserialize<List<int>>(rule.TargetOptions);
        if (targetOptions != null)
        {
            result.ShowOnlyOptions.AddRange(targetOptions);
        }
    }

    private async Task ExecuteLimitRangeAction(ConfigurationRule rule, RuleExecutionResult result)
    {
        if (string.IsNullOrEmpty(rule.RuleParameters))
            return;

        var parameters = JsonSerializer.Deserialize<Dictionary<string, object>>(rule.RuleParameters);
        if (parameters == null)
            return;

        // This would typically modify validation constraints for the target attribute
        // Implementation depends on specific requirements
    }

    private async Task ExecuteCustomAction(ConfigurationRule rule, RuleExecutionResult result)
    {
        // Implement custom rule logic here
        // This could execute JavaScript, call external APIs, etc.
        // For now, just placeholder
        result.Message = "Custom action executed";
    }

    #endregion
}

/// <summary>
/// Rule condition model for JSON deserialization
/// </summary>
public class RuleCondition
{
    public string Attribute { get; set; } = string.Empty;
    public string Operator { get; set; } = string.Empty;
    public object? Value { get; set; }
}