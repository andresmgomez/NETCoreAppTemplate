using Microsoft.Extensions.DependencyInjection;

using TemplateRESTful.Infrastructure.Server;

namespace TemplateRESTful.API.Extensions
{
    public static class CoreServerExtension
    {
        public static void AddServerLayer(this IServiceCollection services) 
        {
            services.AddServerServices();
        }
        public static void AddServerServices(this IServiceCollection services) =>
            services.AddSingleton<IServerLogger, ServerLogger>();
    }
}
