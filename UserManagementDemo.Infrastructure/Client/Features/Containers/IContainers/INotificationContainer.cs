﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementDemo.Infrastructure.Data.Containers
{
    public interface INotificationContainer
    {
        IUserActionsContainer<TNotification> CreateNotification<TNotification>() where TNotification : class;
    }
}
