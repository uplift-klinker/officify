using Officify.Infra.Host.Applications;
using Officify.Infra.Host.Auth;
using Officify.Infra.Host.Persistence;

namespace Officify.Infra.Host.Common;

public static class StringExtensions
{
    public static Type GetStackTypeFromStackName(this string? stackName)
    {
        if (string.IsNullOrWhiteSpace(stackName))
        {
            throw new InvalidOperationException("No stack name provided");
        }

        var splitStack = stackName.Split('-');
        if (splitStack.Length < 2)
        {
            throw new InvalidOperationException(
                $"Expected stack name to be {{env}}-{{stack}} but found {stackName}"
            );
        }

        var layerName = splitStack[1];
        var validLayerNames = new[]
        {
            ApiStack.LayerName,
            SiteStack.LayerName,
            AuthStack.LayerName,
            PersistenceStack.LayerName
        };
        return layerName switch
        {
            ApiStack.LayerName => typeof(ApiStack),
            AuthStack.LayerName => typeof(AuthStack),
            SiteStack.LayerName => typeof(SiteStack),
            PersistenceStack.LayerName => typeof(PersistenceStack),
            _
                => throw new InvalidOperationException(
                    $"Target layer was '{layerName}' which was not found in: {string.Join(",", validLayerNames)}"
                )
        };
    }
}
