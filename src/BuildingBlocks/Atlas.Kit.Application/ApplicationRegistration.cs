using System.Globalization;
using System.Reflection;
using Atlas.Kit.Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Kit.Application;

public static class ApplicationRegistration
{
    /// <summary>Rejestruje MediatR, walidatory i pipeline walidacji z podanego assembly warstwy Application.</summary>
    public static IServiceCollection AddKitApplication(this IServiceCollection services, Assembly assembly)
    {
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("pl");

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);
        return services;
    }
}
