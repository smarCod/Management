


using Management.Core.Models;
using Management.Core.Interfaces.JWT;

using Microsoft.Extensions.Options;

namespace Management.Infrastructure.Repositories.SettingsRepository;

public class JWTSettingsRepository : IJWTSettingsRepository
{
    private readonly JwtSettings _jwtSettings;

    public JWTSettingsRepository(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public JwtSettings GetJwtSettings()
    {
        return _jwtSettings;
    }
}