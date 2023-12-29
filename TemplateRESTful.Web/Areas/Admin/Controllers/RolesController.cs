using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using TemplateRESTful.Domain.Models.Entities;

namespace TemplateRESTful.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public RolesController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([Required] string roleName)
        {
            if (ModelState.IsValid) 
            {
                IdentityResult createRole = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (createRole.Succeeded) 
                {
                    return RedirectToAction("/Admin/Index");
                } else
                {
                    foreach (IdentityError error in createRole.Errors) 
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return ViewComponent("AddRoleViewComponent", roleName);
        }

    }
}
