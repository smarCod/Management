
using Management.Infrastructure.Repositories.SettingsRepository;
using Management.Core.Models;
using Management.Core.Interfaces.JWT;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Management.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options => 
              options.UseSqlite(connectionString));
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddScoped<IJWTSettingsRepository, JWTSettingsRepository>();

        return services;
    }
}


