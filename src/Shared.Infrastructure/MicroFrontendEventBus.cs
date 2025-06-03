using Microsoft.Extensions.Logging;
using Shared.Contracts;
using System.Collections.Concurrent;

namespace Shared.Infrastructure;

/// <summary>
/// Event bus implementation for communication between micro frontend modules
/// </summary>
public class MicroFrontendEventBus : IMicroFrontendEventBus
{
    private readonly ConcurrentDictionary<string, List<Func<object, Task>>> _handlers = new();
    private readonly ILogger<MicroFrontendEventBus> _logger;

    public MicroFrontendEventBus(ILogger<MicroFrontendEventBus> logger)
    {
        _logger = logger;
    }

    public void Subscribe<T>(string eventName, Func<T, Task> handler)
    {
        _logger.LogDebug("Subscribing to event: {EventName}", eventName);
        
        _handlers.AddOrUpdate(eventName,
            new List<Func<object, Task>> { data => handler((T)data) },
            (key, existing) =>
            {
                existing.Add(data => handler((T)data));
                return existing;
            });
    }

    public async Task PublishAsync(string eventName, object data)
    {
        _logger.LogDebug("Publishing event: {EventName}", eventName);

        if (_handlers.TryGetValue(eventName, out var handlers))
        {
            var tasks = handlers.Select(async handler =>
            {
                try
                {
                    await handler(data);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error handling event {EventName}", eventName);
                }
            });

            await Task.WhenAll(tasks);
        }
    }

    public void Unsubscribe(string eventName)
    {
        _logger.LogDebug("Unsubscribing from event: {EventName}", eventName);
        _handlers.TryRemove(eventName, out _);
    }
}
