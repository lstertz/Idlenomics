using Client;
using Client.Edge;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => 
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
    .AddJsonFile(builder.HostEnvironment.IsDevelopment() ? 
        "appsettings.Development.json" : "appsettings.json")
    .Build());
builder.Services.AddSingleton<IEdgeConnector, EdgeConnector>();

await builder.Build().RunAsync();