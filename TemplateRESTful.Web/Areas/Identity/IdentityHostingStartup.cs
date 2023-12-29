using System;
using Microsoft.AspNetCore.Hosting;

using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Persistence.Storage;

[assembly: HostingStartup(typeof(TemplateRESTful.Web.Areas.Identity.IdentityHostingStartup))]
namespace TemplateRESTful.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}