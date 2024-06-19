using System.Collections.Immutable;
using Officify.Infra.Host.Auth;
using Officify.Infra.Host.Persistence;
using Officify.Infra.Host.Service;
using Officify.Infra.Host.Web;

namespace Officify.Infra.Host.Common;

public static class StringExtensions
{
    private static readonly ImmutableDictionary<string, Type> StackNameToStackType = ImmutableDictionary<string, Type>
        .Empty
        .Add(ServiceStack.Name, typeof(ServiceStack))
        .Add(WebStack.Name, typeof(WebStack))
        .Add(AuthStack.Name, typeof(AuthStack))
        .Add(PersistenceStack.Name, typeof(PersistenceStack));

    private static readonly ImmutableArray<string> ValidStackNames = [.. StackNameToStackType.Keys];

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

        var stack = splitStack[1];
        if (StackNameToStackType.TryGetValue(stack, out var stackType))
        {
            return stackType;
        }

        throw new InvalidOperationException(
            $"Target stack was '{stack}' which was not found in: {string.Join(",", ValidStackNames)}"
        );
    }

    public static bool EqualsIgnoreCase(this string? actual, string? expected)
    {
        if (actual == null)
            return expected == null;

        return actual.Equals(expected, StringComparison.OrdinalIgnoreCase);
    }
}