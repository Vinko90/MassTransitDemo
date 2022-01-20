using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MTBlazorApp;
using MTBlazorApp.Utils;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<IOrderService, OrderService>(client =>
{
    //Set the base address of the OrderService API
    client.BaseAddress = new Uri("http://localhost:7100");
});

builder.Services.AddMudServices();

await builder.Build().RunAsync();