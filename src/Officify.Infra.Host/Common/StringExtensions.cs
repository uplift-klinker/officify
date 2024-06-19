using Officify.Infra.Host.Auth;
using Officify.Infra.Host.Persistence;
using Officify.Infra.Host.Service;
using Officify.Infra.Host.Web;

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
            ServiceStack.Name,
            WebStack.Name,
            AuthStack.Name,
            PersistenceStack.Name
        };
        return layerName switch
        {
            ServiceStack.Name => typeof(ServiceStack),
            AuthStack.Name => typeof(AuthStack),
            WebStack.Name => typeof(WebStack),
            PersistenceStack.Name => typeof(PersistenceStack),
            _
                => throw new InvalidOperationException(
                    $"Target layer was '{layerName}' which was not found in: {string.Join(",", validLayerNames)}"
                )
        };
    }

    public static bool EqualsIgnoreCase(this string? actual, string? expected)
    {
        if (actual == null)
            return expected == null;

        return actual.Equals(expected, StringComparison.OrdinalIgnoreCase);
    }
}