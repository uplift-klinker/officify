using Officify.Infra.Host.Auth;
using Officify.Infra.Host.Common;
using Officify.Infra.Host.Persistence;
using Officify.Infra.Host.Service;
using Officify.Infra.Host.Web;

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
    [InlineData(ServiceStack.Name, typeof(ServiceStack))]
    [InlineData(AuthStack.Name, typeof(AuthStack))]
    [InlineData(PersistenceStack.Name, typeof(PersistenceStack))]
    [InlineData(WebStack.Name, typeof(WebStack))]
    public void WhenGettingStackThenReturnsTheCorrectStack(string stackName, Type stackType)
    {
        var actual = $"dev-{stackName}".GetStackTypeFromStackName();

        actual.Should().Be(stackType);
    }
}