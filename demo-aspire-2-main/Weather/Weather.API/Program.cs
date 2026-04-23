using Scalar.AspNetCore;
using Weather.Infrastructure;
using Weather.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Aspire service defaults (telemetry, health checks, service discovery)
builder.AddServiceDefaults();

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

// Controllers
builder.Services.AddControllers();

// Infrastructure — EF Core + PostgreSQL (connection string from Aspire)
builder.Services.AddInfrastructure(builder.Configuration);

// Infrastructure — Redis Caching (connection string from Aspire)
builder.AddInfrastructureCaching();

// Application — MediatR (scan Weather.Application assembly)
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(Weather.Application.Features.WeatherForecasts.WeatherForecastResponse).Assembly));

var app = builder.Build();

// Auto-migrate on startup
using (var scope = app.Services.CreateScope())
{
    var writeDb = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
    await writeDb.Database.MigrateAsync();

    var readDb = scope.ServiceProvider.GetRequiredService<WeatherReadDbContext>();
    await readDb.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<Weather.Infrastructure.Persistence.WeatherDataSeeder>();
    await seeder.SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/api/weather/scalar");
    app.MapGet("/", () => Results.Redirect("/api/weather/scalar/v1")).ExcludeFromDescription();
}

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();
