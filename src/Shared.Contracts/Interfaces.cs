namespace Shared.Contracts;

/// <summary>
/// Interface for the micro frontend host that manages modules
/// </summary>
public interface IMicroFrontendHost
{
    Task LoadModuleAsync(string moduleName);
    Task<T> GetSharedServiceAsync<T>();
    void PublishEvent(string eventName, object data);
    Task RegisterModuleAsync(IMicroFrontend module);
}

/// <summary>
/// Interface that all micro frontend modules must implement
/// </summary>
public interface IMicroFrontend
{
    string ModuleName { get; }
    string Route { get; }
    string DisplayName { get; }
    string Version { get; }
    Task InitializeAsync(IMicroFrontendHost host);
    Task<Type> GetComponentTypeAsync();
}

/// <summary>
/// Interface for shared user context across modules
/// </summary>
public interface ISharedUserContext
{
    CurrentUser User { get; }
    Task<bool> HasPermissionAsync(string permission);
    Task<string> GetUserTokenAsync();
}

/// <summary>
/// Interface for module event communication
/// </summary>
public interface IMicroFrontendEventBus
{
    void Subscribe<T>(string eventName, Func<T, Task> handler);
    Task PublishAsync(string eventName, object data);
    void Unsubscribe(string eventName);
}

/// <summary>
/// Interface for dynamic module loading
/// </summary>
public interface IModuleLoader
{
    Task<T> LoadModuleAsync<T>(string moduleUrl) where T : class;
    Task<bool> IsModuleLoadedAsync(string moduleName);
    Task UnloadModuleAsync(string moduleName);
}
