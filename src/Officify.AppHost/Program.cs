var builder = DistributedApplication.CreateBuilder(args);

var serviceHost = builder.AddProject<Projects.Officify_Service_Host>("service");

builder.AddProject<Projects.Officify_Web_Host>("web")
    .WithEndpoint(
        port: 7099,
        scheme: "https"
    )
    .WithReference(serviceHost);

builder.Build().Run();
