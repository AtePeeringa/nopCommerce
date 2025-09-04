using Nop.Plugin.Misc.ProductConfigurator.Domain;
using Nop.Plugin.Misc.ProductConfigurator.Models;

namespace Nop.Plugin.Misc.ProductConfigurator.Services;

/// <summary>
/// Rules engine service for handling cascading parameters and validation
/// </summary>
public partial interface IRulesEngineService
{
    /// <summary>
    /// Gets all available options for a given attribute based on current configuration
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <param name="attributeName">Attribute name</param>
    /// <param name="currentConfiguration">Current configuration state</param>
    /// <returns>List of available options</returns>
    Task<IList<AttributeOption>> GetAvailableOptionsAsync(int configurableProductId, string attributeName, Dictionary<string, object> currentConfiguration);
    
    /// <summary>
    /// Validates a configuration against all rules
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <param name="configuration">Configuration to validate</param>
    /// <returns>Validation result</returns>
    Task<ConfigurationValidationResult> ValidateConfigurationAsync(int configurableProductId, Dictionary<string, object> configuration);
    
    /// <summary>
    /// Applies auto-setting rules to a configuration
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <param name="configuration">Current configuration</param>
    /// <returns>Updated configuration with auto-set values</returns>
    Task<Dictionary<string, object>> ApplyAutoSetRulesAsync(int configurableProductId, Dictionary<string, object> configuration);
    
    /// <summary>
    /// Gets all applicable rules for a configuration
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <param name="configuration">Current configuration</param>
    /// <returns>List of applicable rules</returns>
    Task<IList<ConfigurationRule>> GetApplicableRulesAsync(int configurableProductId, Dictionary<string, object> configuration);
    
    /// <summary>
    /// Evaluates a single rule against a configuration
    /// </summary>
    /// <param name="rule">Rule to evaluate</param>
    /// <param name="configuration">Configuration to check</param>
    /// <returns>True if rule conditions are met</returns>
    Task<bool> EvaluateRuleAsync(ConfigurationRule rule, Dictionary<string, object> configuration);
    
    /// <summary>
    /// Executes a rule action on a configuration
    /// </summary>
    /// <param name="rule">Rule to execute</param>
    /// <param name="configuration">Configuration to modify</param>
    /// <returns>Result of rule execution</returns>
    Task<RuleExecutionResult> ExecuteRuleAsync(ConfigurationRule rule, Dictionary<string, object> configuration);
    
    /// <summary>
    /// Gets configuration warnings and suggestions
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <param name="configuration">Configuration to analyze</param>
    /// <returns>List of warnings and suggestions</returns>
    Task<IList<string>> GetConfigurationWarningsAsync(int configurableProductId, Dictionary<string, object> configuration);
    
    /// <summary>
    /// Gets all rules for a configurable product
    /// </summary>
    /// <param name="configurableProductId">Configurable product ID</param>
    /// <returns>List of configuration rules</returns>
    Task<IList<ConfigurationRule>> GetRulesAsync(int configurableProductId);
    
    /// <summary>
    /// Creates a new configuration rule
    /// </summary>
    /// <param name="rule">Rule to create</param>
    Task InsertRuleAsync(ConfigurationRule rule);
    
    /// <summary>
    /// Updates an existing configuration rule
    /// </summary>
    /// <param name="rule">Rule to update</param>
    Task UpdateRuleAsync(ConfigurationRule rule);
    
    /// <summary>
    /// Deletes a configuration rule
    /// </summary>
    /// <param name="rule">Rule to delete</param>
    Task DeleteRuleAsync(ConfigurationRule rule);
}