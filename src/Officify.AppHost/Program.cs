var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Officify_Api>("api");

builder.AddProject<Projects.Officify_Web>("web")
    .WithReference(apiService);
builder.Build().Run();
