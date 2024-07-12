namespace Officify.AppHost;

public static class AzureFunctionExtensions
{
    public static IResourceBuilder<ExecutableResource> AddAzureFunction<TServiceMetadata>(
        this IDistributedApplicationBuilder builder,
        string name,
        int port)
        where TServiceMetadata : IProjectMetadata, new()
    {
        var metadata = new TServiceMetadata();
        var projectDirectory = Path.GetDirectoryName(metadata.ProjectPath);
        if (string.IsNullOrEmpty(projectDirectory))
        {
            throw new InvalidOperationException($"Project directory for service {name} is invalid");
        }

        var funcArgs = new[]
        {
            "start",
            "--port",
            $"{port}"
        };
        var resource = new ExecutableResource(name, "func", projectDirectory);
        return builder.AddResource(resource)
            .WithArgs(funcArgs)
            .WithHttpEndpoint(targetPort: port, port: port, isProxied: false)
            .WithOtlpExporter();
    }
}