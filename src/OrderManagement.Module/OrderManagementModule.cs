using Microsoft.AspNetCore.Components;
using Shared.Contracts;

namespace OrderManagement.Module;

/// <summary>
/// Order Management micro frontend module
/// </summary>
public class OrderManagementModule : IMicroFrontend
{
    public string ModuleName => "OrderManagement";
    public string Route => "/orders";
    public string DisplayName => "Order Management";
    public string Version => "1.0.0";

    private IMicroFrontendHost? _host;
    private ISharedUserContext? _userContext;

    public async Task InitializeAsync(IMicroFrontendHost host)
    {
        _host = host;
        _userContext = await host.GetSharedServiceAsync<ISharedUserContext>();
        
        // Subscribe to relevant events
        var eventBus = await host.GetSharedServiceAsync<IMicroFrontendEventBus>();
        eventBus.Subscribe<object>("product.selected", OnProductSelected);
        
        // Publish module initialization event
        host.PublishEvent("module.loaded", new ModuleLoadedEvent
        {
            ModuleName = ModuleName,
            Version = Version,
            LoadTime = TimeSpan.FromMilliseconds(50)
        });
    }

    public Task<Type> GetComponentTypeAsync()
    {
        return Task.FromResult(typeof(Components.OrderManagementComponent));
    }    private Task OnProductSelected(object data)
    {
        // Handle product selection from other modules
        if (_host != null)
        {
            _host.PublishEvent("order.product.added", new { 
                ModuleName = ModuleName,
                ProductData = data,
                Timestamp = DateTime.UtcNow
            });
        }
        return Task.CompletedTask;
    }
}
