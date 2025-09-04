using System.Text.Json;
using Nop.Data;
using Nop.Plugin.Misc.ProductConfigurator.Domain;
using Nop.Plugin.Misc.ProductConfigurator.Models;

namespace Nop.Plugin.Misc.ProductConfigurator.Services;

public partial class ProductConfigurationService : IProductConfigurationService
{
    private readonly IRepository<ProductConfiguration> _productConfigurationRepository;
    private readonly IRepository<ConfigurationGroup> _configurationGroupRepository;
    private readonly IRepository<ConfigurationOption> _configurationOptionRepository;
    private readonly IRepository<DimensionRule> _dimensionRuleRepository;

    public ProductConfigurationService(
        IRepository<ProductConfiguration> productConfigurationRepository,
        IRepository<ConfigurationGroup> configurationGroupRepository,
        IRepository<ConfigurationOption> configurationOptionRepository,
        IRepository<DimensionRule> dimensionRuleRepository)
    {
        _productConfigurationRepository = productConfigurationRepository;
        _configurationGroupRepository = configurationGroupRepository;
        _configurationOptionRepository = configurationOptionRepository;
        _dimensionRuleRepository = dimensionRuleRepository;
    }

    public virtual async Task<ProductConfiguration?> GetProductConfigurationByProductIdAsync(int productId)
    {
        return await _productConfigurationRepository.Table
            .FirstOrDefaultAsync(pc => pc.ProductId == productId);
    }

    public virtual async Task<ProductConfiguration?> GetProductConfigurationByIdAsync(int id)
    {
        return await _productConfigurationRepository.GetByIdAsync(id);
    }

    public virtual async Task InsertProductConfigurationAsync(ProductConfiguration productConfiguration)
    {
        await _productConfigurationRepository.InsertAsync(productConfiguration);
    }

    public virtual async Task UpdateProductConfigurationAsync(ProductConfiguration productConfiguration)
    {
        await _productConfigurationRepository.UpdateAsync(productConfiguration);
    }

    public virtual async Task DeleteProductConfigurationAsync(ProductConfiguration productConfiguration)
    {
        await _productConfigurationRepository.DeleteAsync(productConfiguration);
    }

    public virtual async Task<IList<ConfigurationGroup>> GetConfigurationGroupsByProductConfigurationIdAsync(int productConfigurationId)
    {
        var query = _configurationGroupRepository.Table
            .Where(cg => cg.ProductConfigurationId == productConfigurationId)
            .OrderBy(cg => cg.DisplayOrder);

        return await query.ToListAsync();
    }

    public virtual async Task<ConfigurationGroup?> GetConfigurationGroupByIdAsync(int id)
    {
        return await _configurationGroupRepository.GetByIdAsync(id);
    }

    public virtual async Task InsertConfigurationGroupAsync(ConfigurationGroup configurationGroup)
    {
        await _configurationGroupRepository.InsertAsync(configurationGroup);
    }

    public virtual async Task UpdateConfigurationGroupAsync(ConfigurationGroup configurationGroup)
    {
        await _configurationGroupRepository.UpdateAsync(configurationGroup);
    }

    public virtual async Task DeleteConfigurationGroupAsync(ConfigurationGroup configurationGroup)
    {
        await _configurationGroupRepository.DeleteAsync(configurationGroup);
    }

    public virtual async Task<IList<ConfigurationOption>> GetConfigurationOptionsByGroupIdAsync(int configurationGroupId)
    {
        var query = _configurationOptionRepository.Table
            .Where(co => co.ConfigurationGroupId == configurationGroupId && co.IsActive)
            .OrderBy(co => co.DisplayOrder);

        return await query.ToListAsync();
    }

    public virtual async Task<ConfigurationOption?> GetConfigurationOptionByIdAsync(int id)
    {
        return await _configurationOptionRepository.GetByIdAsync(id);
    }

    public virtual async Task InsertConfigurationOptionAsync(ConfigurationOption configurationOption)
    {
        await _configurationOptionRepository.InsertAsync(configurationOption);
    }

    public virtual async Task UpdateConfigurationOptionAsync(ConfigurationOption configurationOption)
    {
        await _configurationOptionRepository.UpdateAsync(configurationOption);
    }

    public virtual async Task DeleteConfigurationOptionAsync(ConfigurationOption configurationOption)
    {
        await _configurationOptionRepository.DeleteAsync(configurationOption);
    }

    public virtual async Task<IList<DimensionRule>> GetDimensionRulesByProductConfigurationIdAsync(int productConfigurationId)
    {
        var query = _dimensionRuleRepository.Table
            .Where(dr => dr.ProductConfigurationId == productConfigurationId)
            .OrderBy(dr => dr.DisplayOrder);

        return await query.ToListAsync();
    }

    public virtual async Task<DimensionRule?> GetDimensionRuleByIdAsync(int id)
    {
        return await _dimensionRuleRepository.GetByIdAsync(id);
    }

    public virtual async Task InsertDimensionRuleAsync(DimensionRule dimensionRule)
    {
        await _dimensionRuleRepository.InsertAsync(dimensionRule);
    }

    public virtual async Task UpdateDimensionRuleAsync(DimensionRule dimensionRule)
    {
        await _dimensionRuleRepository.UpdateAsync(dimensionRule);
    }

    public virtual async Task DeleteDimensionRuleAsync(DimensionRule dimensionRule)
    {
        await _dimensionRuleRepository.DeleteAsync(dimensionRule);
    }

    public virtual async Task<bool> IsProductConfigurableAsync(int productId)
    {
        var productConfiguration = await GetProductConfigurationByProductIdAsync(productId);
        return productConfiguration?.IsConfigurable == true;
    }

    public virtual async Task<ConfigurationValidationResult> ValidateConfigurationAsync(int productId, Dictionary<string, object> configuration)
    {
        var result = new ConfigurationValidationResult { IsValid = true };
        
        var productConfiguration = await GetProductConfigurationByProductIdAsync(productId);
        if (productConfiguration == null)
        {
            result.IsValid = false;
            result.Errors.Add("Product is not configurable");
            return result;
        }

        var configurationGroups = await GetConfigurationGroupsByProductConfigurationIdAsync(productConfiguration.Id);
        
        foreach (var group in configurationGroups.Where(g => g.IsRequired))
        {
            var groupKey = $"Group_{group.Id}";
            if (!configuration.ContainsKey(groupKey) || configuration[groupKey] == null)
            {
                result.IsValid = false;
                result.Errors.Add($"Required group '{group.DisplayName}' is not configured");
                continue;
            }

            if (int.TryParse(configuration[groupKey].ToString(), out var optionId))
            {
                var option = await GetConfigurationOptionByIdAsync(optionId);
                if (option == null || option.ConfigurationGroupId != group.Id || !option.IsActive)
                {
                    result.IsValid = false;
                    result.Errors.Add($"Invalid option selected for group '{group.DisplayName}'");
                }
            }
        }

        var dimensionRules = await GetDimensionRulesByProductConfigurationIdAsync(productConfiguration.Id);
        foreach (var dimensionRule in dimensionRules.Where(dr => dr.IsRequired))
        {
            var dimensionKey = $"Dimension_{dimensionRule.DimensionType}";
            if (!configuration.ContainsKey(dimensionKey) || configuration[dimensionKey] == null)
            {
                result.IsValid = false;
                result.Errors.Add($"Required dimension '{dimensionRule.DimensionType}' is not specified");
                continue;
            }

            if (decimal.TryParse(configuration[dimensionKey].ToString(), out var value))
            {
                if (value < dimensionRule.MinValue || value > dimensionRule.MaxValue)
                {
                    result.IsValid = false;
                    result.Errors.Add($"Dimension '{dimensionRule.DimensionType}' must be between {dimensionRule.MinValue} and {dimensionRule.MaxValue} {dimensionRule.Unit}");
                }
            }
        }

        return result;
    }

    public virtual async Task<IList<ConfigurationOption>> GetAvailableOptionsAsync(int productId, Dictionary<string, object> currentConfiguration)
    {
        var availableOptions = new List<ConfigurationOption>();
        
        var productConfiguration = await GetProductConfigurationByProductIdAsync(productId);
        if (productConfiguration == null || !productConfiguration.EnableCascadingParameters)
        {
            return availableOptions;
        }

        var configurationGroups = await GetConfigurationGroupsByProductConfigurationIdAsync(productConfiguration.Id);
        
        foreach (var group in configurationGroups)
        {
            var options = await GetConfigurationOptionsByGroupIdAsync(group.Id);
            
            foreach (var option in options)
            {
                if (IsOptionAvailable(option, currentConfiguration))
                {
                    availableOptions.Add(option);
                }
            }
        }

        return availableOptions;
    }

    private static bool IsOptionAvailable(ConfigurationOption option, Dictionary<string, object> currentConfiguration)
    {
        if (string.IsNullOrEmpty(option.DependsOnOptions) && string.IsNullOrEmpty(option.ConflictingOptions))
            return true;

        if (!string.IsNullOrEmpty(option.DependsOnOptions))
        {
            var dependencies = JsonSerializer.Deserialize<List<int>>(option.DependsOnOptions) ?? new List<int>();
            var hasAllDependencies = dependencies.All(depId => 
                currentConfiguration.Values.Any(v => v.ToString() == depId.ToString()));
            
            if (!hasAllDependencies)
                return false;
        }

        if (!string.IsNullOrEmpty(option.ConflictingOptions))
        {
            var conflicts = JsonSerializer.Deserialize<List<int>>(option.ConflictingOptions) ?? new List<int>();
            var hasConflicts = conflicts.Any(conflictId => 
                currentConfiguration.Values.Any(v => v.ToString() == conflictId.ToString()));
            
            if (hasConflicts)
                return false;
        }

        return true;
    }
}