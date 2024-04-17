using Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => 
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

HubConnection hubConnection = new HubConnectionBuilder()
    .WithUrl(new Uri("https://localhost:7285/clientHub"))
    .Build();

try
{
    await hubConnection.StartAsync(); 
}
catch(Exception e)
{
    Console.WriteLine(e);
}

await builder.Build().RunAsync();