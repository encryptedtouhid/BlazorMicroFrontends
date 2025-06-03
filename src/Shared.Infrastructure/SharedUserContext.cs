using Shared.Contracts;

namespace Shared.Infrastructure;

/// <summary>
/// Shared user context implementation
/// </summary>
public class SharedUserContext : ISharedUserContext
{
    private CurrentUser _currentUser;

    public SharedUserContext()
    {
        // Initialize with a demo user for this example
        _currentUser = new CurrentUser
        {
            Id = "demo-user-123",
            Username = "demo.user",
            Email = "demo.user@example.com",
            DisplayName = "Demo User",
            IsAuthenticated = true,
            LastLoginDate = DateTime.UtcNow.AddHours(-2),
            Roles = new List<string> { "User", "OrderManager", "ProductViewer" },
            Permissions = new List<string> 
            { 
                "orders.view", 
                "orders.create", 
                "orders.edit", 
                "products.view",
                "dashboard.access"
            }
        };
    }

    public CurrentUser User => _currentUser;

    public Task<bool> HasPermissionAsync(string permission)
    {
        var hasPermission = _currentUser.IsAuthenticated && 
                           _currentUser.Permissions.Contains(permission);
        
        return Task.FromResult(hasPermission);
    }

    public Task<string> GetUserTokenAsync()
    {
        // In a real implementation, this would return a JWT or other auth token
        return Task.FromResult($"demo-token-{_currentUser.Id}");
    }

    public void UpdateUser(CurrentUser user)
    {
        _currentUser = user;
    }
}
