using Nop.Plugin.Misc.ProductConfigurator.Domain;
using Nop.Plugin.Misc.ProductConfigurator.Models;

namespace Nop.Plugin.Misc.ProductConfigurator.Services;

public partial interface IProductConfigurationService
{
    Task<ProductConfiguration?> GetProductConfigurationByProductIdAsync(int productId);
    
    Task<ProductConfiguration?> GetProductConfigurationByIdAsync(int id);
    
    Task InsertProductConfigurationAsync(ProductConfiguration productConfiguration);
    
    Task UpdateProductConfigurationAsync(ProductConfiguration productConfiguration);
    
    Task DeleteProductConfigurationAsync(ProductConfiguration productConfiguration);
    
    Task<IList<ConfigurationGroup>> GetConfigurationGroupsByProductConfigurationIdAsync(int productConfigurationId);
    
    Task<ConfigurationGroup?> GetConfigurationGroupByIdAsync(int id);
    
    Task InsertConfigurationGroupAsync(ConfigurationGroup configurationGroup);
    
    Task UpdateConfigurationGroupAsync(ConfigurationGroup configurationGroup);
    
    Task DeleteConfigurationGroupAsync(ConfigurationGroup configurationGroup);
    
    Task<IList<ConfigurationOption>> GetConfigurationOptionsByGroupIdAsync(int configurationGroupId);
    
    Task<ConfigurationOption?> GetConfigurationOptionByIdAsync(int id);
    
    Task InsertConfigurationOptionAsync(ConfigurationOption configurationOption);
    
    Task UpdateConfigurationOptionAsync(ConfigurationOption configurationOption);
    
    Task DeleteConfigurationOptionAsync(ConfigurationOption configurationOption);
    
    Task<IList<DimensionRule>> GetDimensionRulesByProductConfigurationIdAsync(int productConfigurationId);
    
    Task<DimensionRule?> GetDimensionRuleByIdAsync(int id);
    
    Task InsertDimensionRuleAsync(DimensionRule dimensionRule);
    
    Task UpdateDimensionRuleAsync(DimensionRule dimensionRule);
    
    Task DeleteDimensionRuleAsync(DimensionRule dimensionRule);
    
    Task<bool> IsProductConfigurableAsync(int productId);
    
    Task<ConfigurationValidationResult> ValidateConfigurationAsync(int productId, Dictionary<string, object> configuration);
    
    Task<IList<ConfigurationOption>> GetAvailableOptionsAsync(int productId, Dictionary<string, object> currentConfiguration);
}