using Nop.Core;

namespace Nop.Plugin.Misc.ProductConfigurator.Domain;

public partial class ConfigurationGroup : BaseEntity
{
    public int ProductConfigurationId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string DisplayName { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public bool IsRequired { get; set; }
    
    public int DisplayOrder { get; set; }
    
    public ConfigurationGroupType GroupType { get; set; }
    
    public DateTime CreatedOnUtc { get; set; }
    
    public DateTime UpdatedOnUtc { get; set; }
}

public enum ConfigurationGroupType
{
    Material = 1,
    Dimension = 2,
    Manufacturing = 3,
    Feature = 4,
    Technique = 5
}