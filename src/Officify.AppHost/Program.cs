var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Officify_Api_Host>("api");

builder.AddProject<Projects.Officify_Web_Host>("web")
    .WithEndpoint(
        port: 7099,
        scheme: "https"
    )
    .WithReference(apiService);

builder.Build().Run();
