namespace Nop.Plugin.Misc.ProductConfigurator.Models;

/// <summary>
/// Result of rule execution
/// </summary>
public partial class RuleExecutionResult
{
    /// <summary>
    /// Gets or sets whether the rule execution was successful
    /// </summary>
    public bool Success { get; set; } = true;
    
    /// <summary>
    /// Gets or sets the execution message
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the modified configuration
    /// </summary>
    public Dictionary<string, object> ModifiedConfiguration { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the attributes that were affected
    /// </summary>
    public List<string> AffectedAttributes { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the options that should be hidden
    /// </summary>
    public List<int> HiddenOptions { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the options that should be shown only
    /// </summary>
    public List<int> ShowOnlyOptions { get; set; } = new();
    
    /// <summary>
    /// Gets or sets warning messages
    /// </summary>
    public List<string> Warnings { get; set; } = new();
    
    /// <summary>
    /// Gets or sets error messages
    /// </summary>
    public List<string> Errors { get; set; } = new();
}