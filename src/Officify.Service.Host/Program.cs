using Officify.ServiceDefaults.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddAspNetCoreServiceProviderDefaults();

var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();
