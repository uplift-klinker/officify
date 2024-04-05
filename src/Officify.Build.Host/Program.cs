using Cake.Frosting;
using Officify.Build.Host.Contexts;
using Officify.Build.Host.Lifetimes;

namespace Officify.Build.Host;

public static class Program
{
    public static int Main(string[] args)
    {
        return new CakeHost()
            .UseContext<OfficifyBuildContext>()
            .UseLifetime<OfficifyBuildLifetime>()
            .Run(args);
    }
}
