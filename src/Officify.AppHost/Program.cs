using Officify.AppHost;

var builder = DistributedApplication.CreateBuilder(args);
builder.AddContainer("azurite", "mcr.microsoft.com/azure-storage/azurite")
    .WithEndpoint(targetPort: 10000, name: "blob")
    .WithEndpoint(targetPort: 10001, name: "queue")
    .WithEndpoint(targetPort: 10001, name: "table");

builder.AddProject<Projects.Officify_Service_Host>("service");
builder.AddAzureFunction<Projects.Officify_Function_Host>("function", 7071);

builder.AddProject<Projects.Officify_Web_Host>("web")
    .WithEndpoint(
        port: 7099,
        scheme: "https"
    );

builder.Build().Run();
