using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Persistence.Data.Contexts;
using TemplateRESTful.Service.Client.Actions.Queries;
using TemplateRESTful.Web.Controllers;

namespace TemplateRESTful.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuditController : RootController<AuditController>
    {
        private readonly IAccessUserContext _userContext;
        public List<AuditLoginDto> RecentUsers { get; set; }

        public AuditController(IAccessUserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<IActionResult> Index()
        {
            var auditAccounts = await _mediator.Send(new GetLoginLogsQuery()
            {
                userId = _userContext.UserId,
            });

            RecentUsers = auditAccounts.ApiResponse;
            return View(RecentUsers);

        }
    }
}
