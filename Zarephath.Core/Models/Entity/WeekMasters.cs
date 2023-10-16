using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("WeekMasters")]
    [PrimaryKey("WeekMasterID")]
    [Sort("StartDate", "DESC")]
    public class WeekMaster : BaseEntity
    {
        public long WeekMasterID { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? StartDate { get; set; }


        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? EndDate { get; set; }

        [Ignore]
        public string DisplayName
        {
            get
            {

                if (StartDate != null && EndDate != null)
                {
                    string name = string.IsNullOrEmpty(Name) ? "" : (" | "+Name );
                    return string.Format("{1:MM/dd/yy} - {2:MM/dd/yy}{0} ", name, StartDate.Value,
                                         EndDate.Value).ToUpper();
                }
                return "";
            }
        }

        [Ignore]
        public List<int> WeekDaysToHide
        {
            get
            {
                if (StartDate != null && EndDate != null)
                {
                    List<int> days = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
                    DateTime sdate = new DateTime(StartDate.Value.Ticks);
                    while (sdate <= EndDate)
                    {
                        days.Remove((int)sdate.DayOfWeek);
                        sdate=sdate.AddDays(1);
                    }
                    return days;
                }
                return null;
            }
        }
    }
}
