using Serilog;

namespace Sindika.AspNet.app015.Configuration
{
    public static class LoggingConfiguration
    {
        public static void ConfigureSerilog(WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            });
        }
    }
}
