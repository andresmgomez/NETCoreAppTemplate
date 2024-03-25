using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TemplateRESTful.Domain.Models.DTOs;

namespace TemplateRESTful.Service.Server.Interfaces
{
    public interface ILogActionService
    {
        Task AddEntityLogAsync(string action, string userId);
        Task<List<AuditLoginDto>> GetEntityLogsAsync(string userId);
    }
}
