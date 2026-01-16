using Serilog;

namespace MyNet.API.Extensions
{
    public static class SerilogExtension
    {
        public static ConfigureHostBuilder AddSerilogExt(this ConfigureHostBuilder hostBuilder, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
            hostBuilder.UseSerilog();

            return hostBuilder;
        }
    }
}
