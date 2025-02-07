


using Management.Core.Models;

namespace Management.Core.Interfaces.JWT;


public interface IJWTSettingsRepository
{
    JwtSettings GetJwtSettings();
}