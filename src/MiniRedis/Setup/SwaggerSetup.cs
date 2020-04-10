namespace MiniRedis.Setup
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;

    public static class SwaggerSetup
    {
        internal static IServiceCollection SetupSwagger(
            this IServiceCollection services)
        {
            var info = new OpenApiInfo
            {
                Title = "Mini Redis",
                Version = "v1"
            };

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", info);
                c.MapType<Exception>(() => new OpenApiSchema
                {
                    Type = "object"
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(
            this IApplicationBuilder app)
        {
            app.UseSwagger();

            var swaggerEndpoint = $"/swagger/v1/swagger.json";

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(swaggerEndpoint, "Mini Redis");
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });

            return app;
        }
    }
}
