using Nop.Core;

namespace Nop.Plugin.Misc.ProductConfigurator.Domain;

/// <summary>
/// Represents a configuration rule for cascading parameters and constraints
/// </summary>
public partial class ConfigurationRule : BaseEntity
{
    /// <summary>
    /// Gets or sets the configurable product ID
    /// </summary>
    public int ConfigurableProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the rule name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the rule type
    /// </summary>
    public RuleType RuleType { get; set; }
    
    /// <summary>
    /// Gets or sets the rule action
    /// </summary>
    public RuleAction Action { get; set; }
    
    /// <summary>
    /// Gets or sets the conditions that trigger this rule (JSON)
    /// Format: [{"attribute": "Material", "operator": "equals", "value": "Aluminum"}, ...]
    /// </summary>
    public string Conditions { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the target attribute for this rule
    /// </summary>
    public string TargetAttribute { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the target options (for require/exclude rules) (JSON array)
    /// </summary>
    public string TargetOptions { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the rule parameters (JSON)
    /// For SET_VALUE: {"value": "someValue"}
    /// For LIMIT_RANGE: {"min": 1, "max": 10}
    /// For CUSTOM: {"formula": "thickness * 2", "message": "Custom validation"}
    /// </summary>
    public string RuleParameters { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the error message for validation rules
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the rule priority (lower numbers execute first)
    /// </summary>
    public int Priority { get; set; }
    
    /// <summary>
    /// Gets or sets whether the rule is active
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Gets or sets the created date
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }
    
    /// <summary>
    /// Gets or sets the updated date
    /// </summary>
    public DateTime UpdatedOnUtc { get; set; }
}

/// <summary>
/// Types of configuration rules
/// </summary>
public enum RuleType
{
    /// <summary>
    /// Compatibility rule (requires/excludes certain combinations)
    /// </summary>
    Compatibility = 1,
    
    /// <summary>
    /// Dependency rule (one attribute depends on another)
    /// </summary>
    Dependency = 2,
    
    /// <summary>
    /// Validation rule (validates user input)
    /// </summary>
    Validation = 3,
    
    /// <summary>
    /// Auto-setting rule (automatically sets values)
    /// </summary>
    AutoSet = 4,
    
    /// <summary>
    /// Range limitation rule
    /// </summary>
    RangeLimitation = 5,
    
    /// <summary>
    /// Custom business logic rule
    /// </summary>
    Custom = 6
}

/// <summary>
/// Actions that rules can perform
/// </summary>
public enum RuleAction
{
    /// <summary>
    /// Require specific options to be selected
    /// </summary>
    Require = 1,
    
    /// <summary>
    /// Exclude/disable specific options
    /// </summary>
    Exclude = 2,
    
    /// <summary>
    /// Show specific options only
    /// </summary>
    ShowOnly = 3,
    
    /// <summary>
    /// Hide specific options
    /// </summary>
    Hide = 4,
    
    /// <summary>
    /// Set a specific value automatically
    /// </summary>
    SetValue = 5,
    
    /// <summary>
    /// Limit the range of allowed values
    /// </summary>
    LimitRange = 6,
    
    /// <summary>
    /// Show a warning message
    /// </summary>
    ShowWarning = 7,
    
    /// <summary>
    /// Show an error and prevent continuation
    /// </summary>
    ShowError = 8,
    
    /// <summary>
    /// Execute custom logic
    /// </summary>
    CustomAction = 9
}