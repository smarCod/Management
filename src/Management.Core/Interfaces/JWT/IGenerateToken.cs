

using Management.Core.Models.Authentication;

using Microsoft.AspNetCore.Identity;

namespace Management.Core.Interfaces.JWT;

public interface IGenerateToken
{
    Task<AuthenticationResult> CreateToken(IdentityUser user);
}

