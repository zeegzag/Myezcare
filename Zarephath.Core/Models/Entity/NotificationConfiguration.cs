using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("[notif].[NotificationConfigurations]")]
    [PrimaryKey("NotificationConfigurationID")]
    [Sort("NotificationConfigurationID", "DESC")]
    public class NotificationConfiguration : BaseEntity
    {
        public long NotificationConfigurationID { get; set; }
        public string ConfigurationName { get; set; }
        public string Description { get; set; }
        public long NotificationEventID { get; set; }
        public long EmailTemplateID { get; set; }
        public long SMSTemplateID { get; set; }
        public bool IsDeleted { get; set; }
    }

}
