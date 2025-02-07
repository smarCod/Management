

using Microsoft.AspNetCore.Identity;


namespace Management.Core.Models.Authentication;

public record AuthenticationResult(IdentityUser User, string Token);
