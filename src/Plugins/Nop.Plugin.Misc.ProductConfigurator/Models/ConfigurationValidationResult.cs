namespace Nop.Plugin.Misc.ProductConfigurator.Models;

public partial class ConfigurationValidationResult
{
    public bool IsValid { get; set; }
    
    public List<string> Errors { get; set; } = new();
    
    public List<string> Warnings { get; set; } = new();
}