using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Persistence.Data.Containers
{
    public interface INotificationContainer
    {
        IUserActions<TNotification> CreateNotification<TNotification>() where TNotification : class;
    }
}
