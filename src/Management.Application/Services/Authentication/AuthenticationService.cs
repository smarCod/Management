


using Management.Core.Models.Authentication;
using Management.Core.Interfaces.JWT;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Management.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IGenerateToken _generateToken;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthenticationService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IGenerateToken generateToken,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _generateToken = generateToken;
        _roleManager = roleManager;
    }

    public async Task<AuthenticationResult> Register(RegisterRequest request)
    {
        var user = new IdentityUser { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return await _generateToken.CreateToken(user);
        }
        else
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

    public async Task CreateRoles()
    {
        string[] roleNames = { "Admin", "User", "Manager" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    public async Task<UserRoleRequest> AddRoleToUser(UserRoleRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new Exception($"User with email {request.Email} not found.");
        }
        var result = await _userManager.AddToRoleAsync(user, request.Role);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Failed to add role {request.Role} to user {request.Email}: {errors}");
        }
        return new UserRoleRequest(request.Email, request.Role);
    }

    public async Task<AuthenticationResult> Login(UserRequest userRequest)
    {
        var user = await _userManager.FindByNameAsync(userRequest.username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, userRequest.password))
        {
            throw new Exception("Invalid credentials");
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return await _generateToken.CreateToken(user);
    }
}

