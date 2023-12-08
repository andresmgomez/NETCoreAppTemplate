using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

using TemplateRESTful.API.Middlewares;
using TemplateRESTful.Domain.Models.Account;

namespace TemplateRESTful.API
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope =  host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("app");

                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await Persistence.Seeding.DefaultRoles.SeedAsync(userManager, roleManager);
                    await Persistence.Seeding.DefaultSuperAdmin.SeedAsync(userManager, roleManager);
                  
                    logger.LogInformation("Finished Seeding Database with Sample Users");

                } catch (Exception error)
                {
                    logger.LogWarning(error, "There has been a problem with seeding Identity Database");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // Logger to troubleshot responses
                .ConfigureSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}