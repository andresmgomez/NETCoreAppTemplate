using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using TemplateRESTful.Web.Extensions;
using TemplateRESTful.Domain.Notifications.Toast;
using TemplateRESTful.Domain.Notifications.Notyf;
using TemplateRESTful.Web.Configuration;

namespace TemplateRESTful.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddNuGetPackages();
            services.AddInfrastructureLayer(_configuration);
            services.AddPersistenceLayer(_configuration);

            services.AddMultilingualLanguages();
            services.AddControllersWithViews();
            services.AddApplicationLayer(_configuration);
  
            services.AddNotyfNotification(settings =>
            {
                settings.DurationInSeconds = 5;
                settings.IsDimissible = true;
                settings.HasRippleEffect = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseLanguageResource();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "admin",
                    pattern: "{area=Dashboard}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
