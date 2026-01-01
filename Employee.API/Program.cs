using Employee.Infrastructure;
using Employee.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddEmployeeInfrastructure(builder.Configuration);
builder.Services.AddEmployeeApplication();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// ❌ REMOVE THIS FOR NOW
// app.UseAuthorization();

app.MapControllers();

app.Run();
