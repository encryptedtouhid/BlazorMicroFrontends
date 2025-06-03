using Host.App;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Shared.Contracts;
using Shared.Infrastructure;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register HTTP client
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register shared services
builder.Services.AddScoped<IMicroFrontendEventBus, MicroFrontendEventBus>();
builder.Services.AddScoped<IModuleLoader, ModuleLoader>();
builder.Services.AddScoped<ISharedUserContext, SharedUserContext>();
builder.Services.AddScoped<IMicroFrontendHost, MicroFrontendHost>();

// Register logging
builder.Services.AddLogging();

await builder.Build().RunAsync();
