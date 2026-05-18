using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace BuildingBlocks.SerilogLogging;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, cfg) =>
        {
            cfg.ReadFrom.Configuration(ctx.Configuration)
               .Enrich.FromLogContext()
               .Enrich.WithProperty("Application", ctx.HostingEnvironment.ApplicationName)
               .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName)
               .WriteTo.Console(
                   outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}");

            if (ctx.HostingEnvironment.IsProduction())
                cfg.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
        });

        builder.Services.AddSerilog();
        return builder;
    }

    public static WebApplication UseSerilogRequestLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        return app;
    }
}
