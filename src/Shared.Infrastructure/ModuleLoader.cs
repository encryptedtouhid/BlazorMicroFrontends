using Microsoft.Extensions.Logging;
using Shared.Contracts;
using System.Collections.Concurrent;
using System.Reflection;

namespace Shared.Infrastructure;

/// <summary>
/// Dynamic module loader for micro frontend modules
/// </summary>
public class ModuleLoader : IModuleLoader
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ModuleLoader> _logger;
    private readonly ConcurrentDictionary<string, Assembly> _loadedModules = new();
    private readonly ConcurrentDictionary<string, DateTime> _loadTimes = new();

    public ModuleLoader(HttpClient httpClient, ILogger<ModuleLoader> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<T> LoadModuleAsync<T>(string moduleUrl) where T : class
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        try
        {
            _logger.LogInformation("Loading module from: {ModuleUrl}", moduleUrl);

            if (_loadedModules.TryGetValue(moduleUrl, out var cachedAssembly))
            {
                _logger.LogDebug("Module already loaded, returning cached instance");
                return CreateModuleInstance<T>(cachedAssembly);
            }

            var assemblyBytes = await _httpClient.GetByteArrayAsync(moduleUrl);
            var assembly = Assembly.Load(assemblyBytes);
            
            _loadedModules[moduleUrl] = assembly;
            _loadTimes[moduleUrl] = DateTime.UtcNow;

            stopwatch.Stop();
            _logger.LogInformation("Module loaded successfully in {ElapsedMs}ms", stopwatch.ElapsedMilliseconds);

            return CreateModuleInstance<T>(assembly);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Failed to load module from {ModuleUrl}", moduleUrl);
            throw;
        }
    }

    public Task<bool> IsModuleLoadedAsync(string moduleName)
    {
        var isLoaded = _loadedModules.Values.Any(assembly => 
            assembly.GetTypes().Any(t => t.Name.Contains(moduleName)));
        
        return Task.FromResult(isLoaded);
    }

    public Task UnloadModuleAsync(string moduleName)
    {
        // Note: In .NET, you can't truly unload assemblies without unloading the AppDomain
        // This is a limitation of the current .NET runtime
        _logger.LogWarning("Module unloading is not fully supported in .NET - module will remain in memory");
        
        var keysToRemove = _loadedModules.Keys
            .Where(key => key.Contains(moduleName))
            .ToList();

        foreach (var key in keysToRemove)
        {
            _loadedModules.TryRemove(key, out _);
            _loadTimes.TryRemove(key, out _);
        }

        return Task.CompletedTask;
    }

    private T CreateModuleInstance<T>(Assembly assembly) where T : class
    {
        var moduleType = assembly.GetTypes()
            .FirstOrDefault(t => typeof(T).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        if (moduleType == null)
        {
            _logger.LogError("No suitable module type found in assembly");
            throw new InvalidOperationException($"No type implementing {typeof(T).Name} found in assembly");
        }

        var instance = Activator.CreateInstance(moduleType) as T;
        if (instance == null)
        {
            _logger.LogError("Failed to create instance of module type");
            throw new InvalidOperationException($"Failed to create instance of {moduleType.Name}");
        }

        return instance;
    }
}
