using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Student.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddStudentApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}
