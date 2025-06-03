namespace Shared.Contracts;

/// <summary>
/// Represents the current user in the system
/// </summary>
public class CurrentUser
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
    public DateTime LastLoginDate { get; set; }
    public bool IsAuthenticated { get; set; }
}

/// <summary>
/// Base class for module events
/// </summary>
public abstract class ModuleEvent
{
    public string ModuleName { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string EventId { get; set; } = Guid.NewGuid().ToString();
}

/// <summary>
/// Event fired when a module encounters an error
/// </summary>
public class ModuleErrorEvent : ModuleEvent
{
    public string ErrorMessage { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
    public string ErrorType { get; set; } = string.Empty;
}

/// <summary>
/// Event fired when a module is loaded
/// </summary>
public class ModuleLoadedEvent : ModuleEvent
{
    public string Version { get; set; } = string.Empty;
    public TimeSpan LoadTime { get; set; }
}

/// <summary>
/// Event fired when a module navigation occurs
/// </summary>
public class ModuleNavigationEvent : ModuleEvent
{
    public string FromRoute { get; set; } = string.Empty;
    public string ToRoute { get; set; } = string.Empty;
    public Dictionary<string, object> Parameters { get; set; } = new();
}

/// <summary>
/// Configuration for a micro frontend module
/// </summary>
public class ModuleConfiguration
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Route { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
    public Dictionary<string, string> Settings { get; set; } = new();
    public List<string> Dependencies { get; set; } = new();
}
