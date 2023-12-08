using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateRESTful.Infrastructure.Utilities;
using TemplateRESTful.Persistence.Data.Actions;

namespace TemplateRESTful.Persistence.Data.Containers
{
    public class NotificationContainer : INotificationContainer
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITempActionsContainer _temporaryActions;

        public NotificationContainer(
            IHttpContextAccessor httpContextAccessor, ITempActionsContainer temporaryActions)
        {
            _httpContextAccessor = httpContextAccessor;
            _temporaryActions = temporaryActions;
        }

        public IUserActions<T> CreateNotification<T>() where T : class
        {
            if (_httpContextAccessor.HttpContext.Request.isNotyfAjaxRequest())
            {
                return new UserActionsContainer<T>();
            }

            return new TempActionsContainer<T>(_temporaryActions);
        }
    }
}
