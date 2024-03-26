using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using UserManagementDemo.Persistence.Storage.DbContexts;
using UserManagementDemo.Persistence.Storage;
using UserManagementDemo.Persistence.Data.Interfaces;
using UserManagementDemo.Persistence.Data.Generic;

namespace UserManagementDemo.Persistence.Extensions
{
    public static class PersistenceExtension
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddPersistenceDatabases(configuration);
            services.AddPersistenceRepositories(configuration);
        }

        private static void AddPersistenceDatabases(
          this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseInMemoryDatabase("IdentityDb"));

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

                services.AddDbContext<ApplicationDbContext>(
                    options => options.UseSqlServer(configuration.GetConnectionString("ApplicationConnection")));
            }
        }

        private static void AddPersistenceRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
