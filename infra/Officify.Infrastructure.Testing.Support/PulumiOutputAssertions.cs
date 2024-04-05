using FluentAssertions.Primitives;
using Pulumi;

namespace Officify.Infrastructure.Testing.Support;

public class PulumiOutputAssertions<T>(Output<T> subject)
    : ReferenceTypeAssertions<Output<T>, PulumiOutputAssertions<T>>(subject)
{
    protected override string Identifier => "output";

    public async Task<AndConstraint<ObjectAssertions>> HaveValueAsync(T value)
    {
        var actual = await Subject.GetValueAsync();
        return actual.Should().Be(value);
    }
}
