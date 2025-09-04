using Nop.Plugin.Misc.ProductConfigurator.Domain;

namespace Nop.Plugin.Misc.ProductConfigurator.Services;

public partial interface ICustomerConfigurationService
{
    Task<CustomerConfiguration?> GetCustomerConfigurationAsync(int customerId, int productId);
    
    Task<CustomerConfiguration?> GetCustomerConfigurationByIdAsync(int id);
    
    Task<IList<CustomerConfiguration>> GetCustomerConfigurationsAsync(int customerId);
    
    Task InsertCustomerConfigurationAsync(CustomerConfiguration customerConfiguration);
    
    Task UpdateCustomerConfigurationAsync(CustomerConfiguration customerConfiguration);
    
    Task DeleteCustomerConfigurationAsync(CustomerConfiguration customerConfiguration);
    
    Task SaveCustomerConfigurationAsync(int customerId, int productId, Dictionary<string, object> configuration, decimal totalPrice);
    
    Task<string> GenerateConfigurationSummaryAsync(int productId, Dictionary<string, object> configuration);
}