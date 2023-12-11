using Microsoft.AspNetCore.Builder;

namespace TemplateRESTful.API.Configuration
{
    public static class MainConfiguration
    {
        public static void UseSwaggerInterface(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    "v1.0"
                );
                options.RoutePrefix = "swagger";
                options.DisplayRequestDuration();
            });
        }
    }
}
