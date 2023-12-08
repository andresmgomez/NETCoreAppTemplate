using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;

using TemplateRESTful.Web.Resources;

namespace TemplateRESTful.Web.Extensions
{
    public static class MultilingualExtension
    {
        public static void AddMultilingualLanguages(this IServiceCollection services)
        {
            #region Registering LocalResources
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            #endregion

            services.AddMvc().AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor
                .LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedResource));
                });

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddHttpContextAccessor();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var cultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("it"),
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });
        }
    }
}
