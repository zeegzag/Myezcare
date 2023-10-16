using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("EmployeeTimeSlotDetails")]
    [PrimaryKey("EmployeeTimeSlotDetailID")]
    [Sort("EmployeeTimeSlotDetailID", "DESC")]
    public class EmployeeTimeSlotDetail : BaseEntity
    {
        public long EmployeeTimeSlotDetailID { get; set; }
        public long EmployeeTimeSlotMasterID { get; set; }

        public DateTime ScheduleDate { get; set; }

        //[Required(ErrorMessageResourceName = "DayRequired", ErrorMessageResourceType = typeof(Resource))]
        public int Day { get; set; }

        [Ignore]
        public string StrDayName
        {
            get
            {
                return Day == 0 ? string.Empty : Common.SetWeekDays().First(c => c.Value == Day).Name;
            }
        }

        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public string Notes { get; set; }

        public bool IsDeleted { get; set; }
        public bool AllDay { get; set; }
        public bool Is24Hrs { get; set; }

        [Ignore]
        [RegularExpression(Constants.RegxTimeFormat, ErrorMessageResourceName = "InvalidTimeMsg", ErrorMessageResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "StartTimeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string StrStartTime
        {
            get
            {

                if (StartTime.HasValue)
                {
                    DateTime time = DateTime.Today.Add(StartTime.Value);
                    return time.ToString("hh:mm tt");
                }
                return "";
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    DateTime timeOnly = DateTime.ParseExact(value, "hh:mm tt",
                                                            System.Globalization.CultureInfo.CurrentCulture);
                    StartTime = timeOnly.TimeOfDay;
                }
                else
                {
                    StartTime = null;
                }
            }
        }


        [Ignore]
        //^(hh:mm tt)$
        [RegularExpression(Constants.RegxTimeFormat, ErrorMessageResourceName = "InvalidTimeMsg", ErrorMessageResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "EndTimeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string StrEndTime
        {
            get
            {
                if (EndTime.HasValue)
                {
                    DateTime time = DateTime.Today.Add(EndTime.Value);
                    return time.ToString("hh:mm tt");
                }
                return "";
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    DateTime timeOnly = DateTime.ParseExact(value, "hh:mm tt",
                                                            System.Globalization.CultureInfo.CurrentCulture);
                    EndTime = timeOnly.TimeOfDay;
                }
                else
                {
                    EndTime = null;
                }
            }
        }

        [Ignore]
        [Required(ErrorMessageResourceName = "DayRequired", ErrorMessageResourceType = typeof(Resource))]
        public int[] SelectedDays { get; set; }

        [ResultColumn]
        public long RemainingSlotCount { get; set; }

        [Ignore]
        public long EmployeeID { get; set; }
        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? EndDate { get; set; }

        public bool IsEndDateAvailable { get; set; }

    }
}
