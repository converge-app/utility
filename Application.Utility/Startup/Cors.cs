using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Utility.Startup
{
    public static class Cors
    {
        public static IServiceCollection AddMultipleDomainSupport(this IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("AllowAll",
                    p => { p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials(); });
            });

            return services;
        }

        public static IApplicationBuilder UseMultipleDomainSupport(this IApplicationBuilder app)
        {
            app.UseCors("AllowAll");
            return app;
        }
    }
}