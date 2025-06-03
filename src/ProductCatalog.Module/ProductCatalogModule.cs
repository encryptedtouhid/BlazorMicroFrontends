using Shared.Contracts;

namespace ProductCatalog.Module;

/// <summary>
/// Product Catalog micro frontend module
/// </summary>
public class ProductCatalogModule : IMicroFrontend
{
    public string ModuleName => "ProductCatalog";
    public string Route => "/products";
    public string DisplayName => "Product Catalog";
    public string Version => "1.0.0";

    private IMicroFrontendHost? _host;

    public async Task InitializeAsync(IMicroFrontendHost host)
    {
        _host = host;
        
        // Subscribe to order events to track product demand
        var eventBus = await host.GetSharedServiceAsync<IMicroFrontendEventBus>();
        eventBus.Subscribe<object>("order.created", OnOrderCreated);
        eventBus.Subscribe<object>("order.product.added", OnProductAddedToOrder);
        
        // Publish module initialization event
        host.PublishEvent("module.loaded", new ModuleLoadedEvent
        {
            ModuleName = ModuleName,
            Version = Version,
            LoadTime = TimeSpan.FromMilliseconds(75)
        });
    }

    public Task<Type> GetComponentTypeAsync()
    {
        return Task.FromResult(typeof(Components.ProductCatalogComponent));
    }

    private async Task OnOrderCreated(object data)
    {
        // Handle order creation events to update inventory
        if (_host != null)
        {
            _host.PublishEvent("product.demand.updated", new { 
                ModuleName = ModuleName,
                OrderData = data,
                Timestamp = DateTime.UtcNow
            });
        }
    }

    private async Task OnProductAddedToOrder(object data)
    {
        // Handle product being added to orders
        if (_host != null)
        {
            _host.PublishEvent("product.usage.tracked", new { 
                ModuleName = ModuleName,
                ProductData = data,
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
