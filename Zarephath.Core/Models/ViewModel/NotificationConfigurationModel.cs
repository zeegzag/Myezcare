using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Amazon.Runtime.Internal;
using ExpressiveAnnotations.Attributes;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class NotificationConfigurationModel
    {
        public long RoleID { get; set; }
        public List<NCItem> NotificationConfigurationList { get; set; }
        public string SelectedNotificationConfigurationIDs { get; set; }
    }

    public class NCItem
    {
        public long NotificationConfigurationID { get; set; }
        public string ConfigurationName { get; set; }
        public bool IsSelected { get; set; }
    }

}
