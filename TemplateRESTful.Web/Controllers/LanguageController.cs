using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace TemplateRESTful.Web.Controllers
{
    public class LanguageController : Controller
    {
        [AllowAnonymous]
        [HttpPost]
        public IActionResult SetLanguage(string language, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(language)),

                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                }
            );
            
            return LocalRedirect(returnUrl);
        }
    }
}
