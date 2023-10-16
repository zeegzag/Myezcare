using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;
using Zarephath.Core.Infrastructure;

namespace Zarephath.Core.Models.Entity
{
    [TableName("VisitTasks")]
    [PrimaryKey("VisitTaskID")]
    [Sort("VisitTaskID", "DESC")]
    public class VisitTask
    {
        public long VisitTaskID { get; set; }

        [Display(ResourceType = typeof(Resource), Name = "TaskType")]
        [Required(ErrorMessageResourceName = "TaskTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string VisitTaskType { get; set; }


        [Display(ResourceType = typeof(Resource), Name = "TaskDetail")]
        [Required(ErrorMessageResourceName = "TaskDetailRequired", ErrorMessageResourceType = typeof(Resource))]
        public string VisitTaskDetail { get; set; }
        
        public long? VisitTaskCategoryID { get; set; }

        public long? VisitTaskSubCategoryID { get; set; }

        //[Required(ErrorMessageResourceName = "ServiceCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        //[Range(1, long.MaxValue, ErrorMessageResourceName = "ServiceCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long ServiceCodeID { get; set; }

        [ResultColumn]
        public string ServiceCode { get; set; }

        [ResultColumn]
        public string CareTypeTitle { get; set; }

        public bool IsRequired { get; set; }

        public bool IsDefault { get; set; }

        public bool SendAlert { get; set; }

        private long? _minimumTimeRequired;
        
        [RegularExpression(Constants.RegxVisitTime, ErrorMessageResourceName = "TimeInvalid", ErrorMessageResourceType = typeof(Resource))]
        public long? MinimumTimeRequired {
            get { return _minimumTimeRequired == 0 ? null : _minimumTimeRequired; }
            set { _minimumTimeRequired = value; }
        }

        public bool IsDeleted { get; set; }

        [Required(ErrorMessageResourceName = "VisitTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string VisitType { get; set; }
        [Required(ErrorMessageResourceName = "CareTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string CareType { get; set; }

        public string Frequency { get; set; }

        public string TaskCode { get; set; }
        public string TaskOption { get; set; }
        public bool DefaultTaskOption { get; set; }

        public enum TaskType
        {
            Task=1,
            Conclusion=2
        }
    }
}
