

using Management.Core.Models;

namespace Management.Application.Services.JWTService;


public interface IJWTSettingService
{
    JwtSettings GetJwtSettingsService();
}