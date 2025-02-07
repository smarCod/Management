


using Management.Core.Models.Authentication;


namespace Management.Application.Services.Authentication;

public interface IAuthenticationService
{
    Task<AuthenticationResult> Register(RegisterRequest request);
    Task<AuthenticationResult> Login(UserRequest userRequest);
    // Task<AuthenticationResult> GenerateToken(IdentityUser user);
    Task CreateRoles();
    Task<UserRoleRequest> AddRoleToUser(UserRoleRequest request);
}