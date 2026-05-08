using Microsoft.EntityFrameworkCore;
using Payment.Infrastructure;
using Payment.Infrastructure.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);
builder.AddInfrastructureCaching();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(Payment.Application.Features.Payments.PaymentResponse).Assembly));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var writeDb = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
    await writeDb.Database.EnsureCreatedAsync();

    var readDb = scope.ServiceProvider.GetRequiredService<PaymentReadDbContext>();
    await readDb.Database.EnsureCreatedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/api/payments/scalar");
    app.MapGet("/", () => Results.Redirect("/api/payments/scalar/v1")).ExcludeFromDescription();
}

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();
