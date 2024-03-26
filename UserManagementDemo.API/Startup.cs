using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using UserManagementDemo.API.Configuration;
using UserManagementDemo.API.Extensions;
using UserManagementDemo.API.Middlewares;
using UserManagementDemo.Persistence.Extensions;
using UserManagementDemo.Service.Extension;

namespace UserManagementDemo.API
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServerLayer();
            services.AddAssemblyPackages();

            services.AddPersistenceLayer(_configuration);
            services.AddInfrastructureLayer(_configuration);
            services.AddServiceLayer(_configuration);
            
            services.AddUserInterface();
            services.AddControllers();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerInterface();
            app.UseHttpsRedirection();
            app.UseMiddleware<ErrorMiddleware>();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}