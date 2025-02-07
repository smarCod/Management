


using Management.Api.Extensions;
using Management.Api.Groups;
using Management.Application;
using Management.Application.Services.Authentication;
using Management.Infrastructure;
using Management.Infrastructure.Authentication;
using Management.Infrastructure.Department;
using Management.Core.Interfaces.JWT;

using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddInfrastructureDepartment(builder.Configuration)
    .ConfigureIdentity()
    .ConfigureAuthentication(builder.Configuration)
    .ConfigureAuthorization();
    
// Scoped-Dienste (flexibel einfügbar)
builder.Services.AddScoped<IGenerateToken, GenerateToken>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();

// builder.Services.AddCors(options =>
//     {
//         options.AddPolicy("AllowAllOrigins",
//             builder =>
//             {
//                 builder.AllowAnyOrigin()
//                     .AllowAnyMethod()
//                     .AllowAnyHeader();
//             });
//     });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

// app.UseCors("AllowAllOrigins");
app.MapGroup("").GroupDepartmentApi().WithTags("Department");

app.Run();

//  error CS0122: 'Der Zugriff auf "Program" ist aufgrund des Schutzgrads nicht möglich.
public partial class Program { }



// using Management.Application;
// using Management.Application.Services.JWTService;
// using Management.Application.Services.Authentication;
// using Management.Infrastructure;
// using Management.Infrastructure.Authentication;
// using Management.Core.Models.Authentication;

// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.Http.HttpResults;
// using Microsoft.IdentityModel.Tokens;
// using System.Text;

// var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddApplication().AddInfrastructure(builder.Configuration);

// builder.Services.AddScoped<IGenerateToken, GenerateToken>();

// builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//     .AddEntityFrameworkStores<AppDbContext>();

// builder.Services.AddEndpointsApiExplorer();
// //   builder.Services.AddSwaggerGen();

// builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
//             ValidAudience = builder.Configuration["JwtSettings:Audience"],
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))

//         };

//         Console.WriteLine($"JwtSettings Key: {builder.Configuration["JwtSettings:Key"]}, Length: {builder.Configuration["JwtSettings:Key"].Length}");
//     });

// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Admin"));
// });

// builder.Services.AddOpenApi();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

// app.UseHttpsRedirection();
// app.UseAuthentication();
// app.UseAuthorization();

// app.MapGet("/", (IJWTSettingService _jWTSettingService) =>
// {
//     var result = _jWTSettingService.GetJwtSettingsService();
//     return Results.Ok(new
//     {
//         result.Key,
//         result.Issuer,
//         result.Audience
//     });
// });

// app.MapGet("/settings", (IJWTSettingService _jWTSettingService) =>
// {
//     return Results.Ok(_jWTSettingService.GetJwtSettingsService());
// });

// app.MapPost("/register", async (RegisterRequest _registerRequest, IAuthenticationService _authenticationService) =>
// {
//     AuthenticationResult authenticationResult = await _authenticationService.Register(_registerRequest);

//     return Results.Ok(authenticationResult);
// });

// app.Run();



