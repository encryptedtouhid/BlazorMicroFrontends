# Blazor WebAssembly Micro Frontends Demo

This project demonstrates a complete, **fully functional** implementation of the micro frontends architecture using Blazor WebAssembly.

![image](https://github.com/user-attachments/assets/b3c39dad-9536-4c6d-b89e-29485d35f705)


## 🎯 What You'll Find Here

This is a **working, runnable solution** that showcases real-world micro frontends patterns with:
- Dynamic module loading and registration
- Inter-module communication via event bus
- Shared services with dependency injection
- Error boundaries for module isolation
- Real-time event monitoring and logging

## 🏗️ Architecture Overview

This solution implements a production-ready micro frontends architecture with the following components:

- **Host.App**: Main Blazor WebAssembly application that hosts and orchestrates micro frontend modules
- **Shared.Contracts**: Contains interfaces and shared types used across all modules (`IMicroFrontendHost`, `IMicroFrontend`, `IMicroFrontendEventBus`)
- **Shared.Infrastructure**: Provides core services (`MicroFrontendEventBus`, `ModuleLoader`, `SharedUserContext`)
- **OrderManagement.Module**: Complete micro frontend for order creation, processing, and management
- **ProductCatalog.Module**: Complete micro frontend for product browsing, search, and selection

## 🚀 Features Demonstrated

### ✅ Core Micro Frontend Architecture
- **Dynamic module loading and registration** - Modules are registered at runtime through the host
- **Inter-module communication via event bus** - Loose coupling through publish/subscribe pattern
- **Shared services and dependency injection** - User context, permissions, and services across modules
- **Error boundaries for module isolation** - `ModuleErrorBoundary` components prevent cascade failures
- **Centralized user context and permissions** - Shared authentication and role-based access

### ✅ Order Management Module (`/orders`)
- **Order creation and management** - Create new orders with customer details
- **Order status tracking** - Pending, Processing, Completed, Cancelled states
- **Customer information handling** - Full customer profiles and order history
- **Order processing workflow** - Complete business process implementation
- **Real-time order statistics** - Live dashboard with order counts and totals

### ✅ Product Catalog Module (`/products`)
- **Product browsing and search** - Full-text search across product catalog
- **Category filtering** - Filter by Electronics, Clothing, Books, Home & Garden
- **Product selection for orders** - Cross-module integration via events
- **Inventory management** - Stock tracking and availability display
- **Discount handling** - Price calculations with discount support
- **Add to cart functionality** - Triggers events to other modules

### ✅ System Features
- **Real-time event logging** (`/events`) - Monitor all inter-module communication
- **Module statistics and monitoring** - Performance and usage tracking
- **User permissions and role-based access** - Admin, Manager, User roles
- **Responsive design with Bootstrap** - Mobile-friendly responsive UI
- **Error handling and recovery** - Graceful error boundaries with retry options
- **Live event testing** - Send custom events to test module communication

## 🛠️ Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code
- Modern web browser with WebAssembly support

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/encryptedtouhid/BlazorMicroFrontends.git
cd BlazorMicroFrontends
```

### 2. Restore Dependencies
```powershell
dotnet restore
```

### 3. Build the Solution
```powershell
dotnet build
```

### 4. Run the Application
```powershell
dotnet run --project src/Host.App/
```

The application will be available at `http://localhost:5000` (or the port shown in the console).

> **Note**: The first build may take a few minutes as it downloads all necessary packages and builds the WebAssembly runtime.

## 📱 Using the Application

### Dashboard (`/`)
- **System overview** - View application status and loaded modules
- **User context display** - Current user information and permissions
- **Module statistics** - Real-time statistics for each micro frontend module
- **Quick navigation** - Access links to all major application areas

### Order Management (`/orders`)
- **View existing orders** - Complete order list with customer information and status
- **Create new orders** - Full order creation workflow with customer selection
- **Process pending orders** - Change order status and track progress
- **Order details** - Detailed view with line items, totals, and customer data
- **Order statistics** - Live dashboard showing order counts by status

### Product Catalog (`/products`)
- **Browse product catalog** - Paginated product listing with images and details
- **Search functionality** - Real-time search across product names and descriptions
- **Category filtering** - Filter by Electronics, Clothing, Books, Home & Garden
- **Product details** - Comprehensive product information with pricing
- **Add to cart** - Product selection triggers events to other modules
- **Inventory status** - Real-time stock availability

### Event Monitor (`/events`)
- **Real-time event stream** - Live monitoring of all inter-module communication
- **Event statistics** - Count and categorization of events by type
- **Event testing** - Send custom events to test module communication
- **Pause/Resume** - Control event display for detailed analysis
- **Event details** - JSON payload inspection for debugging

## 🏗️ Project Structure

```
BlazorMicroFrontends/
├── src/
│   ├── Host.App/                    # Main Blazor WebAssembly host application
│   │   ├── Pages/                   # Application pages (Index, Orders, Products, Events)
│   │   │   ├── Index.razor          # Dashboard with module overview
│   │   │   ├── Orders.razor         # Order management page
│   │   │   ├── Products.razor       # Product catalog page
│   │   │   └── Events.razor         # Real-time event monitoring
│   │   ├── Shared/                  # Shared UI components
│   │   │   ├── MainLayout.razor     # Main application layout
│   │   │   ├── NavMenu.razor        # Navigation menu
│   │   │   ├── UserInfo.razor       # User context display
│   │   │   └── ModuleErrorBoundary.razor # Error boundary for modules
│   │   ├── MicroFrontendHost.cs     # Host implementation
│   │   ├── Program.cs               # Application entry point with DI setup
│   │   └── wwwroot/                 # Static assets and service worker
│   ├── Shared.Contracts/            # Shared interfaces and models
│   │   ├── Interfaces.cs            # Core interfaces (IMicroFrontendHost, etc.)
│   │   └── Models.cs                # Shared models and events
│   ├── Shared.Infrastructure/       # Shared services implementation
│   │   ├── MicroFrontendEventBus.cs # Event bus implementation
│   │   ├── ModuleLoader.cs          # Dynamic module loading
│   │   └── SharedUserContext.cs     # User context service
│   ├── OrderManagement.Module/      # Order management micro frontend
│   │   ├── Components/              # Module-specific components
│   │   │   └── OrderManagementComponent.razor # Main order UI
│   │   ├── Models/                  # Module-specific models
│   │   │   └── Order.cs             # Order domain model
│   │   └── OrderManagementModule.cs # Module implementation
│   └── ProductCatalog.Module/       # Product catalog micro frontend
│       ├── Components/              # Module-specific components
│       │   └── ProductCatalogComponent.razor # Main product UI
│       ├── Models/                  # Module-specific models
│       │   └── Product.cs           # Product domain model
│       └── ProductCatalogModule.cs  # Module implementation
├── BlazorMicroFrontends.sln        # Visual Studio solution file
├── README.md                       # This documentation
└── blogpost.md                     # Detailed architecture blog post
```

## 🔧 Key Implementation Details

### Module Registration
Modules are automatically registered on application startup in `MainLayout.razor`:

```csharp
protected override async Task OnInitializedAsync()
{
    // Register modules on startup
    var orderModule = new OrderManagement.Module.OrderManagementModule();
    var productModule = new ProductCatalog.Module.ProductCatalogModule();
    
    await Host.RegisterModuleAsync(orderModule);
    await Host.RegisterModuleAsync(productModule);
    
    // Load critical modules
    await Host.LoadModuleAsync("OrderManagement");
    await Host.LoadModuleAsync("ProductCatalog");
}
```

### Inter-Module Communication
Modules communicate via the event bus using a publish/subscribe pattern:

```csharp
// Publishing an event from ProductCatalog
await EventBus.PublishAsync("product.selected", new ProductSelectedEvent
{
    ProductId = product.Id,
    ProductName = product.Name,
    Price = product.Price
});

// Subscribing to events in OrderManagement
EventBus.Subscribe<ProductSelectedEvent>("product.selected", async (productData) =>
{
    await AddProductToCurrentOrder(productData);
    StateHasChanged();
});
```

### Error Boundaries
Each module is wrapped in an error boundary to prevent cascade failures:

```razor
<ModuleErrorBoundary ModuleName="OrderManagement">
    <OrderManagementComponent />
</ModuleErrorBoundary>
```

### Shared Services
Modules access shared services through dependency injection:

```csharp
// In module components
@inject ISharedUserContext UserContext
@inject IMicroFrontendEventBus EventBus

// Usage in component
var user = UserContext.User;
var hasPermission = await UserContext.HasPermissionAsync("orders.create");
```

### Global Using Statements
The host project uses global using statements for shared namespaces:

```xml
<ItemGroup>
  <Using Include="Shared.Contracts" />
  <Using Include="Shared.Infrastructure" />
</ItemGroup>
```

## 🎯 Development Guidelines

### Adding New Modules

1. Create a new project following the naming pattern `[ModuleName].Module`
2. Implement the `IMicroFrontend` interface
3. Create your module components
4. Register the module in the host application
5. Add routing if needed

### Module Communication

- Use the event bus for loose coupling between modules
- Define clear event contracts in `Shared.Contracts`
- Subscribe to relevant events in your module's initialization
- Publish events for actions that other modules might care about

### Shared Services

- Add new shared service interfaces to `Shared.Contracts`
- Implement services in `Shared.Infrastructure`
- Register services in the host application's dependency injection container

## 🔍 Monitoring and Debugging

### Real-Time Event Log
The `/events` page provides comprehensive monitoring of all inter-module communication:
- **Module lifecycle events** (loading, initialization, errors)
- **Business logic events** (order creation, product selection)
- **Navigation events** (page changes, module activation)
- **System events** (error handling, user actions)

### Browser Developer Tools
- **Console tab**: View WebAssembly module loading and JavaScript interop
- **Network tab**: Monitor assembly downloads and API calls
- **Application tab**: Inspect local storage and service worker
- **Performance tab**: Profile WASM execution and rendering

### Event Bus Debugging
Use the event testing feature on the Events page:
```json
// Example test event
{
  "productId": "123",
  "action": "add_to_cart",
  "quantity": 2
}
```

### Module Status Monitoring
The dashboard shows real-time module status including:
- Registration status
- Load times
- Error counts
- Event statistics

## 🚀 Production Considerations

### Performance Optimization
- Enable assembly trimming and AOT compilation
- Implement lazy loading for non-critical modules
- Use intelligent caching strategies

### Deployment
- Each module can be deployed independently
- Use semantic versioning for module compatibility
- Implement proper CI/CD pipelines for each module

### Security
- Implement proper authentication and authorization
- Validate user permissions at the module level
- Use secure communication between modules

## 📚 Learn More

This implementation demonstrates the concepts from the blog post **"Building Micro Frontends with Blazor WebAssembly: A C# Developer's Guide to Modern Frontend Architecture"**.

### Key Benefits Demonstrated:
- **✅ Unified Development Stack**: Single C# codebase across all modules and shared services
- **✅ Code Reusability**: Shared models, validation logic, and business rules
- **✅ Type Safety**: Strong typing throughout the application with compile-time checking
- **✅ Team Productivity**: Familiar .NET tooling, debugging, and development patterns
- **✅ Module Isolation**: Independent development, testing, and deployment of modules
- **✅ Enterprise Ready**: Error handling, monitoring, user management, and permissions

### Real-World Applications:
This pattern is ideal for:
- **Enterprise dashboards** and back-office applications
- **Multi-team development** with shared .NET expertise
- **Complex business applications** requiring strong type safety
- **Applications with shared business logic** between frontend and backend

### Performance Considerations:
- Initial bundle size: ~2-4MB (acceptable for enterprise internal apps)
- Cold start time: 1-3 seconds (reasonable for business applications)
- Runtime performance: Excellent for complex business logic
- Not recommended for: Public websites requiring SEO or mobile-first applications

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is provided as an educational example. Feel free to use and modify as needed for your own projects.

## 🐛 Troubleshooting

### Common Build Issues

**Error: "The type or namespace name 'Contracts' does not exist"**
- **Solution**: Clean and rebuild the solution
```powershell
dotnet clean
dotnet restore
dotnet build
```

**Error: "Multiple components use the tag 'ErrorBoundary'"**
- **Solution**: This occurs if you have both a custom ErrorBoundary and the built-in one. Use `ModuleErrorBoundary` for custom error boundaries.

**Error: "Cannot await 'void'"**
- **Solution**: Check that async methods return `Task` or `Task<T>`, not `void`.

### Runtime Issues

**Modules not loading**
- Check browser developer tools console for JavaScript errors
- Verify all project references are correctly set up
- Ensure modules implement `IMicroFrontend` interface

**Events not communicating between modules**
- Check that EventBus is properly injected in both publishing and subscribing modules
- Verify event names match exactly (case-sensitive)
- Use the Events page (`/events`) to monitor event flow

**Performance Issues**
- Enable assembly trimming for production builds
- Consider lazy loading non-critical modules
- Use browser developer tools to profile WASM performance
