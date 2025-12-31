using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using School.Application.Interfaces;
using School.Infrastructure.Persistence;

namespace School.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSchoolInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<SchoolDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("SchoolDb")));

        services.AddScoped<ISchoolRepository, SchoolRepository>();

        return services;
    }
}
