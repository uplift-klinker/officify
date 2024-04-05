using System.Collections.Immutable;
using Pulumi.Testing;

namespace Officify.Infrastructure.Testing.Support;

public class PulumiMocks : IMocks
{
    public ImmutableArray<MockResourceArgs> NewResourceArgs { get; private set; } =
        ImmutableArray<MockResourceArgs>.Empty;

    public Task<(string? id, object state)> NewResourceAsync(MockResourceArgs args)
    {
        NewResourceArgs = NewResourceArgs.Add(args);
        return Task.FromResult<(string? id, object state)>((args.Id, args.Inputs));
    }

    public Task<object> CallAsync(MockCallArgs args)
    {
        return Task.FromResult<object>(args);
    }
}