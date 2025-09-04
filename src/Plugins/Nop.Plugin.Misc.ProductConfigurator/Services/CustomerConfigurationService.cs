using System.Text.Json;
using Nop.Data;
using Nop.Plugin.Misc.ProductConfigurator.Domain;

namespace Nop.Plugin.Misc.ProductConfigurator.Services;

public partial class CustomerConfigurationService : ICustomerConfigurationService
{
    private readonly IRepository<CustomerConfiguration> _customerConfigurationRepository;
    private readonly IProductConfigurationService _productConfigurationService;

    public CustomerConfigurationService(
        IRepository<CustomerConfiguration> customerConfigurationRepository,
        IProductConfigurationService productConfigurationService)
    {
        _customerConfigurationRepository = customerConfigurationRepository;
        _productConfigurationService = productConfigurationService;
    }

    public virtual async Task<CustomerConfiguration?> GetCustomerConfigurationAsync(int customerId, int productId)
    {
        return await _customerConfigurationRepository.Table
            .FirstOrDefaultAsync(cc => cc.CustomerId == customerId && cc.ProductId == productId);
    }

    public virtual async Task<CustomerConfiguration?> GetCustomerConfigurationByIdAsync(int id)
    {
        return await _customerConfigurationRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<CustomerConfiguration>> GetCustomerConfigurationsAsync(int customerId)
    {
        var query = _customerConfigurationRepository.Table
            .Where(cc => cc.CustomerId == customerId)
            .OrderByDescending(cc => cc.UpdatedOnUtc);

        return await query.ToListAsync();
    }

    public virtual async Task InsertCustomerConfigurationAsync(CustomerConfiguration customerConfiguration)
    {
        await _customerConfigurationRepository.InsertAsync(customerConfiguration);
    }

    public virtual async Task UpdateCustomerConfigurationAsync(CustomerConfiguration customerConfiguration)
    {
        await _customerConfigurationRepository.UpdateAsync(customerConfiguration);
    }

    public virtual async Task DeleteCustomerConfigurationAsync(CustomerConfiguration customerConfiguration)
    {
        await _customerConfigurationRepository.DeleteAsync(customerConfiguration);
    }

    public virtual async Task SaveCustomerConfigurationAsync(int customerId, int productId, Dictionary<string, object> configuration, decimal totalPrice)
    {
        var existingConfiguration = await GetCustomerConfigurationAsync(customerId, productId);
        var configurationSummary = await GenerateConfigurationSummaryAsync(productId, configuration);

        if (existingConfiguration != null)
        {
            existingConfiguration.ConfigurationData = JsonSerializer.Serialize(configuration);
            existingConfiguration.TotalPrice = totalPrice;
            existingConfiguration.ConfigurationSummary = configurationSummary;
            existingConfiguration.UpdatedOnUtc = DateTime.UtcNow;
            
            await UpdateCustomerConfigurationAsync(existingConfiguration);
        }
        else
        {
            var newConfiguration = new CustomerConfiguration
            {
                CustomerId = customerId,
                ProductId = productId,
                ConfigurationData = JsonSerializer.Serialize(configuration),
                TotalPrice = totalPrice,
                ConfigurationSummary = configurationSummary,
                IsCompleted = false,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };
            
            await InsertCustomerConfigurationAsync(newConfiguration);
        }
    }

    public virtual async Task<string> GenerateConfigurationSummaryAsync(int productId, Dictionary<string, object> configuration)
    {
        var summary = new List<string>();
        
        var productConfiguration = await _productConfigurationService.GetProductConfigurationByProductIdAsync(productId);
        if (productConfiguration == null)
            return string.Empty;

        var configurationGroups = await _productConfigurationService.GetConfigurationGroupsByProductConfigurationIdAsync(productConfiguration.Id);
        
        foreach (var group in configurationGroups)
        {
            var groupKey = $"Group_{group.Id}";
            if (configuration.TryGetValue(groupKey, out var optionIdValue) && 
                int.TryParse(optionIdValue?.ToString(), out var optionId))
            {
                var option = await _productConfigurationService.GetConfigurationOptionByIdAsync(optionId);
                if (option != null)
                {
                    summary.Add($"{group.DisplayName}: {option.DisplayName}");
                }
            }
        }

        var dimensionRules = await _productConfigurationService.GetDimensionRulesByProductConfigurationIdAsync(productConfiguration.Id);
        
        foreach (var dimensionRule in dimensionRules)
        {
            var dimensionKey = $"Dimension_{dimensionRule.DimensionType}";
            if (configuration.TryGetValue(dimensionKey, out var dimensionValue) && 
                decimal.TryParse(dimensionValue?.ToString(), out var value))
            {
                summary.Add($"{dimensionRule.DimensionType}: {value} {dimensionRule.Unit}");
            }
        }

        return string.Join("; ", summary);
    }
}