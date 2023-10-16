using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class BulkEmployeeTimeSlotDetails
    {
        public long EmployeeTimeSlotDetailID { get; set; }
        public long EmployeeTimeSlotMasterID { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public string Notes { get; set; }

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

        [Required(ErrorMessageResourceName = "DayRequired", ErrorMessageResourceType = typeof(Resource))]
        public int[] SelectedDays { get; set; }

        [Required(ErrorMessageResourceName = "EmployeeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string EmployeeIDs { get; set; }

        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? EndDate { get; set; }

        public bool IsEndDateAvailable { get; set; }

    }
}
