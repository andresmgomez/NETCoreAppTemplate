using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TemplateRESTful.Web.Configuration
{
    public static class ResourceConfiguration
    {
        public static void UseLanguageResource(this IApplicationBuilder app)
        {
            app.UseRequestLocalization(app.ApplicationServices.
                    GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);            
        }
    }
}
