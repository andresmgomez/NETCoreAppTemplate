using System.Text;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Persistence.Storage;
using TemplateRESTful.Service.Common.Account;
using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Infrastructure.Identity;
using TemplateRESTful.Service.Common.Identity;

namespace TemplateRESTful.API.Extensions
{
    public static class InfrastructureExtension
    {
        public static void AddInfrastructureLayer(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityInfrastructure(configuration);
        }

        private static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseInMemoryDatabase("IdentityDb"));
            } else
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                    migration => migration.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)
                ));

            }

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<IdentityContext>().AddDefaultUI().AddDefaultTokenProviders();

            #region Identity Service
            services.AddScoped<IAuthorizeService, AuthorizeService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            #endregion

            services.Configure<SecureToken>(configuration.GetSection("JWTSettings"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            .AddJwtBearer(settings =>
            {
                settings.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWTSettings:Issuer"],
                    ValidAudience = configuration["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                };
            });

        }
    }
}
