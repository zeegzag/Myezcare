using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;
using Newtonsoft.Json;
using Zarephath.Core.Controllers;

namespace Zarephath.Core.Models.Entity
{
    [TableName("EmployeeDayOffs")]
    [PrimaryKey("EmployeeDayOffID")]
    [Sort("EmployeeDayOffID", "DESC")]
    public class EmployeeDayOff : BaseEntity
    {
        public long EmployeeDayOffID { get; set; }

        [Required(ErrorMessageResourceName = "EmployeeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long EmployeeID { get; set; }

        //[JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [Required(ErrorMessageResourceName = "StartTimeRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? StartTime { get; set; }

        //[JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [Required(ErrorMessageResourceName = "EndTimeRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? EndTime { get; set; }


        public string DayOffStatus { get; set; }

        public long? ActionTakenBy { get; set; }
        public DateTime? ActionTakenDate { get; set; }

        [Required(ErrorMessageResourceName = "CommentReasonRequired", ErrorMessageResourceType = typeof(Resource))]
        public string EmployeeComment { get; set; }


        public string ApproverComment { get; set; }
        public bool IsDeleted { get; set; }

        [Required(ErrorMessageResourceName = "PTOTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long DayOffTypeID { get; set; }


        [Ignore]
        public string StrDayOffTypeID {
            get
            {
                if (DayOffTypeID > 0)
                {
                    EmpDayOffType enumDisplayStatus = (EmpDayOffType) DayOffTypeID;
                    return enumDisplayStatus.ToString();
                }
                return null;

            }
        }


        public enum EmpDayOffType
        {
            Other = 1,
            Sick  = 2,
            Vacation = 3
        }

        public enum EmployeeDayOffStatus
        {
            InProgress=1,
            Approved=2,
            Denied=3
        }

        [Ignore]
        public string StrStartTime {
            get
            {
                //03/03/2018 3:00 am
                return String.Format("{0: " +Constants.DbDateTimeFormat + "}", StartTime);
            }
        }
        [Ignore]
        public string StrEndTime {
            get
            {
                return String.Format("{0: " + Constants.DbDateTimeFormat + " }", EndTime);
            }
        }

    }
}
