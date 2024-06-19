using Officify.Infra.Host.Auth;
using Officify.Infra.Host.Common;

namespace Officify.Infra.Host.Tests.Common;

public class TypeExtensionsTests
{
    [Fact]
    public void WhenCreatingDeploymentMethodThenReturnsMethodWithTypeAsGenericParameterToRunAsyncMethod()
    {
        var method = typeof(AuthStack).CreateDeploymentRunAsyncMethodForStack();

        method.Name.Should().Be("RunAsync");
        method.IsGenericMethod.Should().BeTrue();
        method.GetParameters().Should().BeEmpty();
        ;
        method.GetGenericArguments().Should().HaveCount(1);
        method.GetGenericArguments()[0].Should().Be(typeof(AuthStack));
        method.Should().Return<Task<int>>();
    }
}