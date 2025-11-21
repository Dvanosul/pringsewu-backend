
namespace Sindika.AspNet.app015.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(o =>
                o.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("*")
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                }));

            return services;
        }
    }
}
