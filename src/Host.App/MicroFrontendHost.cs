using Microsoft.Extensions.Logging;
using Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Host.App;

/// <summary>
/// Host implementation for managing micro frontend modules
/// </summary>
public class MicroFrontendHost : IMicroFrontendHost
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMicroFrontendEventBus _eventBus;
    private readonly IModuleLoader _moduleLoader;
    private readonly ILogger<MicroFrontendHost> _logger;
    private readonly Dictionary<string, IMicroFrontend> _registeredModules = new();

    public MicroFrontendHost(
        IServiceProvider serviceProvider,
        IMicroFrontendEventBus eventBus,
        IModuleLoader moduleLoader,
        ILogger<MicroFrontendHost> logger)
    {
        _serviceProvider = serviceProvider;
        _eventBus = eventBus;
        _moduleLoader = moduleLoader;
        _logger = logger;
    }

    public async Task LoadModuleAsync(string moduleName)
    {
        try
        {
            _logger.LogInformation("Loading module: {ModuleName}", moduleName);
            
            // In a real implementation, you would load from a remote URL
            // For this demo, we'll simulate loading from registered modules
            if (_registeredModules.TryGetValue(moduleName, out var module))
            {
                await module.InitializeAsync(this);
                
                await _eventBus.PublishAsync("module.loaded", new ModuleLoadedEvent
                {
                    ModuleName = moduleName,
                    Version = module.Version,
                    LoadTime = TimeSpan.FromMilliseconds(100) // Simulated load time
                });
                
                _logger.LogInformation("Module {ModuleName} loaded successfully", moduleName);
            }
            else
            {
                _logger.LogWarning("Module {ModuleName} not found in registry", moduleName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load module {ModuleName}", moduleName);
            
            await _eventBus.PublishAsync("module.error", new ModuleErrorEvent
            {
                ModuleName = moduleName,
                ErrorMessage = ex.Message,
                StackTrace = ex.StackTrace,
                ErrorType = ex.GetType().Name
            });
        }
    }

    public Task<T> GetSharedServiceAsync<T>()
    {
        var service = _serviceProvider.GetService<T>();
        if (service == null)
        {
            throw new InvalidOperationException($"Service of type {typeof(T).Name} is not registered");
        }
        
        return Task.FromResult(service);
    }

    public void PublishEvent(string eventName, object data)
    {
        _ = Task.Run(async () => await _eventBus.PublishAsync(eventName, data));
    }

    public Task RegisterModuleAsync(IMicroFrontend module)
    {
        _logger.LogInformation("Registering module: {ModuleName}", module.ModuleName);
        _registeredModules[module.ModuleName] = module;
        return Task.CompletedTask;
    }

    public IEnumerable<IMicroFrontend> GetRegisteredModules()
    {
        return _registeredModules.Values;
    }
}
