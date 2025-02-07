


using Management.Core.Models;
using Management.Core.Models.Authentication;
using Management.Core.Interfaces.JWT;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;

namespace Management.Infrastructure.Authentication;

public class NullIdentityUser : IdentityUser
{
    public NullIdentityUser()
    {
        Id = string.Empty;
        UserName = string.Empty;
        Email = string.Empty;
    }
}

public class GenerateToken : IGenerateToken
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtSettings _jwtSettings;

    public GenerateToken(UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthenticationResult> CreateToken(IdentityUser user)
    {
        if(user == null)
        {
            user = new NullIdentityUser();
            // throw new ArgumentNullException(nameof(user));
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        var jti = Guid.NewGuid().ToString();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, jti),
            new Claim(ClaimTypes.Role, "NormalUser"),
            new Claim(ClaimTypes.Name, user.UserName!)
        };

        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new AuthenticationResult(user, new JwtSecurityTokenHandler().WriteToken(token));
    }
}