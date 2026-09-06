using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using QarzAlHasana.Application.Common.Behaviors;

namespace QarzAlHasana.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);

            
            // Avvalin AddOpenBehavior = birooni-tarin laye.
            // Logging bayad birun-e Validation bashe ta
            // request-haye rad-shode ham log beshan.
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}