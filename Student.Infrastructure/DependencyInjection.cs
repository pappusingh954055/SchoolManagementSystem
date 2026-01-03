using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Student.Application.Interfaces;
using Student.Infrastructure.Persistence;
using Student.Infrastructure.Repositories;

namespace Student.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddStudentInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<StudentDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("Default")));

        services.AddScoped<IStudentRepository, StudentRepository>();

        return services;
    }
}
