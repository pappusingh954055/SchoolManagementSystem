using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using School.Application;
using School.Infrastructure;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ---------------- Controllers ----------------
//builder.Services.AddControllers();

// ---------------- FluentValidation (LATEST WAY) ----------------
builder.Services.AddValidatorsFromAssembly(
    typeof(School.Application.DependencyInjection).Assembly);

// ---------------- Application Layer ----------------
builder.Services.AddSchoolApplication();

// ---------------- Infrastructure Layer ----------------
builder.Services.AddSchoolInfrastructure(builder.Configuration);

// ---------------- JWT Authentication ----------------
var jwt = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.NameIdentifier,

            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!)
            )
        };
    });

builder.Services.AddAuthorization();

// ---------------- OpenAPI + Scalar ----------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
var app = builder.Build();

// ---------------- HTTP PIPELINE ----------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // Authorize button
}

app.UseHttpsRedirection();

// ✅ REQUIRED FOR PHOTO ACCESS
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
