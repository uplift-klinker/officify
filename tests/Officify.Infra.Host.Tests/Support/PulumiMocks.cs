using System.Collections.Immutable;
using Pulumi.Testing;

namespace Officify.Infra.Host.Tests.Support;

public class PulumiMocks : IMocks
{
    public Task<(string? id, object state)> NewResourceAsync(MockResourceArgs args)
    {
        var outputs = ImmutableDictionary.CreateBuilder<string, object>();

        outputs.AddRange(args.Inputs);
        if (!args.Inputs.ContainsKey("name"))
        {
            outputs.Add("name", LocateActualResourceName(args));
        }
        args.Id ??= $"{args.Name}_id";
        return Task.FromResult<(string?, object state)>((args.Id, outputs));
    }

    public Task<object> CallAsync(MockCallArgs args)
    {
        return Task.FromResult<object>(args.Args);
    }

    private string LocateActualResourceName(MockResourceArgs args)
    {
        var name = args.Name ?? "name";
        var nameKey = LocateResourceNameKey(args);
        if (string.IsNullOrWhiteSpace(nameKey))
            return name;

        var keyValue = args.Inputs.TryGetValue(nameKey, out var value) ? value : null;
        if (keyValue is string actualName)
            return actualName;

        return name;
    }

    private string? LocateResourceNameKey(MockResourceArgs args)
    {
        if (args.Type == "azure-native:resources:ResourceGroup")
        {
            return "resourceGroupName";
        }

        if (args.Type == "azure-native:storage:StorageAccount")
        {
            return "accountName";
        }

        return null;
    }
}
