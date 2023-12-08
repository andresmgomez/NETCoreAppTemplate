using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace TemplateRESTful.API.Middlewares
{
    public static class LoggerMiddlware
    {
        public static IHostBuilder ConfigureSerilog(this IHostBuilder hostBuilder) 
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables().Build();

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            SerilogHostBuilderExtensions.UseSerilog(hostBuilder);

            return hostBuilder;
        }
    }
}
