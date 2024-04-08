using Officify.Infra.Host.Applications;
using Officify.Infra.Host.Auth;
using Officify.Infra.Host.Common;
using Officify.Infra.Host.Persistence;

namespace Officify.Infra.Host.Tests.Common;

public class StringExtensionsTests
{
    [Fact]
    public void WhenStackNameIsMissingThenThrowsException()
    {
        var act = () => "".GetStackTypeFromStackName();

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void WhenStackNameIsMissingLayerNameThenThrowsException()
    {
        var act = () => "dev".GetStackTypeFromStackName();

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void WhenStackNameIsNotValidThenThrowsException()
    {
        var act = () => "dev-notreally".GetStackTypeFromStackName();

        act.Should().Throw<InvalidOperationException>();
    }

    [Theory]
    [InlineData(ApiStack.LayerName, typeof(ApiStack))]
    [InlineData(AuthStack.LayerName, typeof(AuthStack))]
    [InlineData(PersistenceStack.LayerName, typeof(PersistenceStack))]
    [InlineData(SiteStack.LayerName, typeof(SiteStack))]
    public void WhenGettingStackThenReturnsTheCorrectStack(string stackName, Type stackType)
    {
        var actual = $"dev-{stackName}".GetStackTypeFromStackName();

        actual.Should().Be(stackType);
    }
}
