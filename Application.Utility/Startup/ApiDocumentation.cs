using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.Utility.Startup
{
    public static class ApiDocumentation
    {
        public static IServiceCollection AddApiDocumentation(this IServiceCollection services,
            string serviceName, Action<SwaggerGenOptions> setupAction = null)
        {
            if (setupAction != null) services.ConfigureSwaggerGen(setupAction);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(serviceName, new Info {Title = $"{serviceName} API", Version = "v1"});

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT authorization",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
            });

            return services;
        }

        public static IApplicationBuilder UseApiDocumentation(this IApplicationBuilder app, string serviceName)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint($"/swagger/{serviceName}/swagger.json", serviceName + " API"); });
            return app;
        }
    }
}