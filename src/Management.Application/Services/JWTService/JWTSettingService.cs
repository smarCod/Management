

using Management.Core.Models;
using Management.Core.Interfaces.JWT;

namespace Management.Application.Services.JWTService;

public class JWTSettingService : IJWTSettingService
{
    private readonly IJWTSettingsRepository _jWTSettingsRepository;

    public JWTSettingService(IJWTSettingsRepository jWTSettingsRepository)
    {
        _jWTSettingsRepository = jWTSettingsRepository;
    }

    public JwtSettings GetJwtSettingsService()
    {
        return _jWTSettingsRepository.GetJwtSettings();
    }
}