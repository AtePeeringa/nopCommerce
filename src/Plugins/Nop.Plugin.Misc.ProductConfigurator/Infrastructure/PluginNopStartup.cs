using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.ProductConfigurator.Services;

namespace Nop.Plugin.Misc.ProductConfigurator.Infrastructure;

/// <summary>
/// Represents the object for configuring services on application startup
/// </summary>
public class PluginNopStartup : INopStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Register plugin services
        services.AddScoped<IRulesEngineService, RulesEngineService>();
        services.AddScoped<IFlexiblePricingService, FlexiblePricingService>();
        services.AddScoped<IBomService, BomService>();
        services.AddScoped<IProductionService, ProductionService>();
        services.AddScoped<IProductConfigurationService, ProductConfigurationService>();
        services.AddScoped<IPricingService, PricingService>();
        services.AddScoped<ICustomerConfigurationService, CustomerConfigurationService>();
    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
    }

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 3000;
}