using Officify.Infra.Host.Applications;
using Officify.Infra.Host.Auth;
using Officify.Infra.Host.Persistence;
using Pulumi;

var targetLayer = Environment.GetEnvironmentVariable("TARGET_LAYER_NAME");
var layerCreator = GetLayerCreator(targetLayer);
return await Deployment.RunAsync(layerCreator);

static Func<Task> GetLayerCreator(string? layerName)
{
    var validLayerNames = new[]
    {
        ApiStack.LayerName,
        SiteStack.LayerName,
        AuthStack.LayerName,
        PersistenceStack.LayerName
    };

    return layerName switch
    {
        ApiStack.LayerName => () => Task.FromResult(new ApiStack()),
        AuthStack.LayerName => () => Task.FromResult(new AuthStack()),
        SiteStack.LayerName => () => Task.FromResult(new SiteStack()),
        PersistenceStack.LayerName => () => Task.FromResult(new PersistenceStack()),
        _
            => throw new InvalidOperationException(
                $"Target layer was {layerName} which was not found in: {string.Join(",", validLayerNames)}"
            )
    };
}
