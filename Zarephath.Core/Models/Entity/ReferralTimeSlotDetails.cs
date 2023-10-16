using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralTimeSlotDetails")]
    [PrimaryKey("ReferralTimeSlotDetailID")]
    [Sort("ReferralTimeSlotDetailID", "DESC")]
    public class ReferralTimeSlotDetail : BaseEntity
    {
        public long ReferralTimeSlotDetailID { get; set; }
        public long ReferralTimeSlotMasterID { get; set; }

        //public DateTime ScheduleDate { get; set; }

        [Required(ErrorMessageResourceName = "DayRequired", ErrorMessageResourceType = typeof(Resource))]
        public int Day { get; set; }

        [Ignore]
        public string StrDayName
        {
            get
            {
                return Day == 0 ? string.Empty : Common.SetWeekDays().First(c => c.Value == Day).Name;
            }
        }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public string Notes { get; set; }

        public bool UsedInScheduling { get; set; }

        public bool IsDeleted { get; set; }

        [Ignore]
        [RegularExpression(Constants.RegxTimeFormat, ErrorMessageResourceName = "InvalidTimeMsg", ErrorMessageResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "StartTimeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string StrStartTime
        {
            get
            {

                if (StartTime.TotalMilliseconds > 0)
                {
                    DateTime time = DateTime.Today.Add(StartTime);
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
                    StartTime = DateTime.Now.TimeOfDay;
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
                if (EndTime.TotalMilliseconds > 0)
                {
                    DateTime time = DateTime.Today.Add(EndTime);
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
                    EndTime = DateTime.Now.TimeOfDay;
                }
            }
        }


        [ResultColumn]
        public long RemainingSlotCount { get; set; }
        [Ignore]
        [Required(ErrorMessageResourceName = "DayRequired", ErrorMessageResourceType = typeof(Resource))]
        public int[] SelectedDays { get; set; }

        public long? CareTypeId { get; set; }

        public long? ReferralBillingAuthorizationID { get; set; }

        [Ignore]
        public long ReferralID { get; set; }

        [Ignore]
        public bool IsForcePatientSchedules { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool AnyTimeClockIn { get; set; }
    }
}
