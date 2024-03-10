using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using TemplateRESTful.Domain.Enums.Account;
using TemplateRESTful.Domain.Models.Entities;

namespace TemplateRESTful.Persistence.Seeding
{
    public static class DefaultSuperAdmin
    {
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var testingUser = new ApplicationUser
            {
                FirstName = "Miguel",
                LastName = "Gomez",
                UserName = "andresmgomez23@gmail.com",
                Email = "andresmgomez23@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true,
            };

            if (userManager.Users.All(user => user.Id != testingUser.Id))
            {
                var existingUser = await userManager.FindByEmailAsync(testingUser.Email);

                if (existingUser == null)
                {
                    await userManager.CreateAsync(testingUser, "JMgc0608%!");
                    await userManager.AddToRoleAsync(testingUser, UserRoles.AccountUser.ToString());
                    await userManager.AddToRoleAsync(testingUser, UserRoles.Administrator.ToString());
                    await userManager.SetTwoFactorEnabledAsync(testingUser, true);
                }
            }
        }
    }
}