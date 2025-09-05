using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Misc.ProductConfigurator.Infrastructure;

/// <summary>
/// Represents the plugin event consumer for admin menu integration
/// </summary>
public class EventConsumer : BaseAdminMenuCreatedEventConsumer
{
    #region Fields

    private readonly IPermissionService _permissionService;

    #endregion

    #region Ctor

    public EventConsumer(IPluginManager<IPlugin> pluginManager, IPermissionService permissionService)
        : base(pluginManager)
    {
        _permissionService = permissionService;
    }

    #endregion

    #region Utilities

    protected override async Task<bool> CheckAccessAsync()
    {
        return await _permissionService.AuthorizeAsync("ManageProductConfigurator");
    }

    protected override Task<AdminMenuItem> GetAdminMenuItemAsync(IPlugin plugin)
    {
        return Task.FromResult(plugin.GetAdminMenuItem("fas fa-cogs"));
    }

    #endregion

    #region Properties

    protected override string PluginSystemName => "Misc.ProductConfigurator";
    
    protected override MenuItemInsertType InsertType => MenuItemInsertType.TryAfterThanBefore;
    
    protected override string AfterMenuSystemName => "Catalog.Products";
    
    protected override string BeforeMenuSystemName => "Configuration";

    #endregion
}