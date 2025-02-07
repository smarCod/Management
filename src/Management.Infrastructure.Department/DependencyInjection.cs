
using Management.Core.Interfaces.DepartmentInterfaces.RepositoryInterfaces;
using Management.Infrastructure.Department.Data;
using Management.Infrastructure.Department.Repositories.DepartmentRepositories;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Management.Infrastructure.Department;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDepartment(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DepartmentConnection");
        services.AddDbContext<AppDbContextDepartment>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IDepartmentRepository, DepartmentRepository>();

        return services;
    }
}


