using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TemplateRESTful.Persistence.Data;
using TemplateRESTful.Persistence.Data.Users;
using TemplateRESTful.Persistence.Storage.DbContexts;

namespace TemplateRESTful.Web.Extensions
{
    public static class PersistenceExtension
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistenceContexts(configuration);
            services.AddEntityRepositories(configuration);
        }
        private static void AddPersistenceContexts(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        }

        private static void AddEntityRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IEntityAsyncActions<>), typeof(EntityAsyncActions<>));
            services.AddTransient<IUserAccountRepository, UserAccountRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
