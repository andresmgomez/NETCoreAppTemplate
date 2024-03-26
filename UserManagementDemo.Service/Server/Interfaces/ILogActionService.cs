using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserManagementDemo.Domain.Models.DTOs;

namespace UserManagementDemo.Service.Server.Interfaces
{
    public interface ILogActionService
    {
        Task AddEntityLogAsync(string action, string userId);
        Task<List<AuditLoginDto>> GetEntityLogsAsync(string userId);
    }
}
