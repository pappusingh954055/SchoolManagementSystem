using FluentValidation.AspNetCore;
using Identity.API.Extensions;
using Identity.Application.Interfaces;
using Identity.Domain.Users;
using Identity.Infrastructure;
using Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

using System.Security.Claims;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// -------------------- Controllers --------------------
builder.Services.AddControllers()
    .AddFluentValidation();

// -------------------- Application --------------------
builder.Services.AddIdentityApplication();




// -------------------- Infrastructure -----------------
builder.Services.AddIdentityInfrastructure(builder.Configuration);



//builder.Configuration
//    .AddJsonFile("appsettings.json", optional: false)
//    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
//    .AddEnvironmentVariables();

// Password hasher
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IAuthService, AuthService>();


// -------------------- JWT Authentication --------------
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            RoleClaimType = ClaimTypes.Role, // 🔴 REQUIRED
            NameClaimType = ClaimTypes.NameIdentifier,

            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(jwt["Key"]!))
        };
    });


builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    db.Database.Migrate(); // applies migrations, creates DB if not exists
}


app.MapControllers();


app.Run();
