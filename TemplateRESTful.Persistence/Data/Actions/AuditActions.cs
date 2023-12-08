using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TemplateRESTful.Persistence.Data.Actions
{
    public interface IAuditActions {}

    public class AuditActions : IAuditActions
    {
        public string UserId { get; } = null;
        public string Username { get; } = null;

        public AuditActions(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                UserId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            else if (httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name) != null)
            {
                Username = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name).Value;
            }
        }
    }
}
