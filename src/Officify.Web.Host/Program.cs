using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Officify.Web.Host;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOfficifyWebHost(api =>
{
    api.BaseUrl = "http://localhost:5000";
});

await builder.Build().RunAsync();
