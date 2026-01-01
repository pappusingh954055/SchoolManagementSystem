using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Employee.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddEmployeeApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(
                Assembly.GetExecutingAssembly()));

        return services;
    }
}
