using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TemplateRESTful.Persistence.Data;
using TemplateRESTful.Persistence.Data.General;
using TemplateRESTful.Persistence.Data.General.IRepository;

namespace TemplateRESTful.Persistence.Extensions
{
    public static class PersistenceExtension
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddEntityPersistence(configuration);
            services.AddEntityRepositories(configuration);
        }
        private static void AddEntityPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        private static void AddEntityRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IEntityAsyncActions<>), typeof(EntityAsyncActions<>));
            services.AddTransient<ILogEntityActions, LogEntityActions>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
