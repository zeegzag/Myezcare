using System;
using System.Collections.Generic;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public interface INotificationConfigurationDataProvider
    {
        ServiceResponse GetNotificationConfigurationDetails(NotificationConfigurationModel notificationConfiguration);
        ServiceResponse SaveNotificationConfigurationDetails(NotificationConfigurationModel notificationConfiguration, long loggedInUserId);
        List<NameValueData> GetRoles();
    }
}
