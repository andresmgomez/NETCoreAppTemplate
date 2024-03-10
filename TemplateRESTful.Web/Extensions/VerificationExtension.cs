using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TemplateRESTful.Web.Extensions
{
    public static class VerificationExtension
    {
        public static void AddVerificationLayer(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddSocialVerification(configuration);
            services.AddTwoFactorVerification(configuration);
        }

        private static void AddSocialVerification(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication().AddGoogle("Sign in with Google", options =>
            {
                var googleAuth = configuration.GetSection("Authentication:Google");
                options.ClientId = googleAuth["ClientID"];
                options.ClientSecret = googleAuth["ClientSecret"];
                options.SignInScheme = IdentityConstants.ExternalScheme;
            });
        }

        private static void AddTwoFactorVerification(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(options =>
                options.AddPolicy("TwoFactorEnabled", actions => actions.RequireClaim("amr", "mfa")));
        }
    }
}
