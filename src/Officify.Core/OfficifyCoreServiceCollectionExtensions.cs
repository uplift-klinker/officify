using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Officify.Core.Common;
using Officify.Core.Common.Validation;

namespace Officify.Core;

public static class OfficifyCoreServiceCollectionExtensions
{
    private static readonly Assembly CoreAssembly =
        typeof(OfficifyCoreServiceCollectionExtensions).Assembly;

    public static IServiceCollection AddOfficifyCore(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(CoreAssembly);
            cfg.AddOpenBehavior(typeof(FluentValidationPipelineBehavior<,>));
        });
        services.AddValidatorsFromAssembly(CoreAssembly);
        services.AddScoped<IMessageBus, MessageBus>();
        return services;
    }
}
