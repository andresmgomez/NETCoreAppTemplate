using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TemplateRESTful.Domain.Models.Account;
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