


using Management.Application.Services.JWTService;
using Management.Application.Services.Authentication;
using Management.Infrastructure;
using Management.Core.Models.Authentication;
using Management.Core.Models.DepartmentModels.DepartmentMediator.Queries;
using Management.Core.Models.DepartmentModels.DepartmentMediator.Command;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MediatR;

namespace Management.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole>();
        return services;
    }

    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var key = configuration["JwtSettings:Key"];
                if(string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException(nameof(key), "Key cannot be null or empty.");
                }
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });

        return services;
    }

    public static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Admin"));
        });

        return services;
    }

    public static IServiceCollection AddOpenApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        // services.AddSwaggerGen();
        return services;
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapGet("/", (IJWTSettingService jwtSettingService) =>
        {
            var result = jwtSettingService.GetJwtSettingsService();
            return Results.Ok(new
            {
                result.Key,
                result.Issuer,
                result.Audience
            });
        });

        app.MapGet("/settings", (IJWTSettingService jwtSettingService) =>
        {
            return Results.Ok(jwtSettingService.GetJwtSettingsService());
        });

        app.MapPost("/register", async (RegisterRequest registerRequest, IAuthenticationService authenticationService) =>
        {
            AuthenticationResult authenticationResult = await authenticationService.Register(registerRequest);
            return Results.Ok(authenticationResult);
        });

        app.MapPost("/addrollen", async (IAuthenticationService _authenticationService) =>
        {
            await _authenticationService.CreateRoles();
            return Results.Ok("Roles created successfully.");
        });

        app.MapPost("/addroletouser", async (UserRoleRequest _request, IAuthenticationService _authenticationService) =>
        {
            await _authenticationService.AddRoleToUser(_request);
            return Results.Ok("Role zum User geadded");
        });

        app.MapPost("/login", async (UserRequest _userRequest, IAuthenticationService _authenticationService) =>
        {
            AuthenticationResult authenticationResult = await _authenticationService.Login(_userRequest);
            
            if (authenticationResult != null && !string.IsNullOrEmpty(authenticationResult.Token))
            {
                return Results.Ok(new
                {
                    success = true,
                    token = authenticationResult.Token,
                    expiresAt = DateTime.UtcNow.AddMinutes(60).ToString("yyyy-MM-ddTHH:mm:ss")
                });
            }
            else
            {
                return Results.BadRequest(new { error = "Authentication failed" });
            }
        });

        // //  Department Queries
        // app.MapGroup("V1").MapGet("/department/getalldepartmentsmediator", async (IMediator _mediator) =>
        // {
        //     // var result = await _mediator.Send(new RaumfahrzeugQuery());
        //     var query = new GetAllDepartmentsQuery();
        //     var result = await _mediator.Send(query);
        //     return Results.Ok(result);
        // });
        // app.MapGet("/department/getdepartmentbyidmediator/{id:int}", async (IMediator _mediator, int id) =>
        // {
        //     // var result = await _mediator.Send(new RaumfahrzeugQuery());
        //     var query = new GetDepartmentByIdQuery { Id = id };
        //     var result = await _mediator.Send(query);
        //     return Results.Ok(result);
        // });

        // //  Department Commands
        // app.MapPost("/department/postdepartmentmediator/{name}", async (IMediator _mediator, string name) =>
        // {
        //     var query = new PostDepartmentCommand { name = name };
        //     var result = await _mediator.Send(query);
        //     return Results.Ok(result);
        // });

        // app.MapPut("/department/updatedepartmentmediator/{id:int}/{name}", async (IMediator _mediator, int id, string name) =>
        // {
        //     var query = new UpdateDepartmentCommand { Id = id, Name = name };
        //     var result = await _mediator.Send(query);
        //     return Results.Ok(result);
        // });

        // app.MapDelete("/department/deletedepartmentmediator/{id:int}", async (IMediator _mediator, int id) =>
        // {
        //     var query = new DeleteDepartmentCommand(id);
        //     var result = await _mediator.Send(query);
        //     return Results.Ok(result);
        // });

        return app;
    }
}
