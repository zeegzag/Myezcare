using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Controllers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ScheduleBatchServices")]
    [PrimaryKey("ScheduleBatchServiceID")]
    [Sort("CreatedDate", "DESC")]
    public class ScheduleBatchService : BaseEntity
    {
        public long ScheduleBatchServiceID { get; set; }

        [Required(ErrorMessageResourceName = "ScheduleBatchServiceTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ScheduleBatchServiceName { get; set; }

        [Required(ErrorMessageResourceName = "ScheduleBatchServiceNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public int ScheduleBatchServiceType { get; set; }

        public string ScheduleIDs { get; set; }

        public string ScheduleBatchServiceStatus { get; set; }
        public string ServiceStatusDescription { get; set; }
        public string FilePath { get; set; }
        
        public enum ScheduleBatchServiceStatuses
        {
            Initiated,
            InProgress,
            Failed,
            Completed
        }
        public enum ScheduleBatchServiceTypes
        {
            [Display(ResourceType = typeof(Resource), Name = "SendEmail")]
            SendEmail=1,
            [Display(ResourceType = typeof(Resource), Name = "SendSMS")]
            SendSMS = 2,
            [Display(ResourceType = typeof(Resource), Name = "GenerateMail")]
            GenerateMailNotice = 3
        }
    }
}
