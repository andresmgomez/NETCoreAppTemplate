using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateRESTful.Domain.Enums.Account;
using TemplateRESTful.Domain.Models.Account;

namespace TemplateRESTful.Persistence.Seeding
{
    public class DefaultBasicUser
    {
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var regularUser1 = new ApplicationUser
            {
                FirstName = "Raymond",
                LastName = "Amaya",
                UserName = "raymaya27",
                Email = "ray.amaya@domain.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                IsActive = false
            };

            await userManager.CreateAsync(regularUser1, "RYMaya0956&%!");
            await userManager.AddToRoleAsync(regularUser1, UserRoles.RegularUser.ToString());
           
            var regularUser2 = new ApplicationUser
            {
                FirstName = "Howard",
                LastName = "Williams",
                UserName = "hwilliam29",
                Email = "howard.will@domain.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                IsActive = false
            };

            await userManager.CreateAsync(regularUser2, "HWilliam02745*&!");
            await userManager.AddToRoleAsync(regularUser2, UserRoles.RegularUser.ToString());

            var regularUser3 = new ApplicationUser
            {
                FirstName = "Natalia",
                LastName = "Gonzales",
                UserName = "ntgonzales25",
                Email = "nata.gonzales@domain.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = false,
                IsActive = false
            };

            await userManager.CreateAsync(regularUser3, "NTgonzales07218*^%");
            await userManager.AddToRoleAsync(regularUser3, UserRoles.RegularUser.ToString());
        }
    }
}
