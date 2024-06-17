var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Officify_Api>("api");

builder.AddProject<Projects.Officify_Web>("web")
    .WithEndpoint(
        port: 7099,
        scheme: "https"
    )
    .WithReference(apiService);

builder.Build().Run();
