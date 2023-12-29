using Microsoft.Extensions.DependencyInjection;
using TemplateRESTful.Infrastructure.Server;

namespace TemplateRESTful.API.Extensions
{
    public static class EssentialExtension
    {
        public static void AddEssentialServices(this IServiceCollection services) 
        {
            services.ConfigureLoggerService();
        }
        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddSingleton<IServerLogger, ServerLogger>();
    }
}
