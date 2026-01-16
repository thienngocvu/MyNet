using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace MyNet.API.Extensions
{
    public static class HealthChecksExtension
    {
        public static IServiceCollection AddHealthChecksExt(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");
            if (!string.IsNullOrEmpty(connectionString))
            {
                services.AddHealthChecks()
                    .AddNpgSql(
                        connectionString: connectionString,
                        name: "postgresql",
                        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy,
                        tags: new[] { "db", "sql", "postgres" }
                    );
            }

            return services;
        }

        public static IEndpointRouteBuilder UseHealthChecksExt(this IEndpointRouteBuilder app, string path = "/health")
        {
            app.MapHealthChecks(path, new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            return app;
        }
    }
}
