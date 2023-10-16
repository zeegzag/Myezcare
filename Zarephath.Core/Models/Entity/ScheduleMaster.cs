using System;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ScheduleMasters")]
    [PrimaryKey("ScheduleID")]
    [Sort("UpdatedDate", "DESC")]
    public class ScheduleMaster : BaseEntity
    {
        public long ScheduleID { get; set; }
        public long ReferralID { get; set; }
        public long? FacilityID { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long PickUpLocation { get; set; }
        public long DropOffLocation { get; set; }
        [Required(ErrorMessageResourceName = "ScheduleStatusRequired", ErrorMessageResourceType = typeof(Resource))]
        public long ScheduleStatusID { get; set; }
        public string Comments { get; set; }
        public bool IsAssignedToTransportationGroupUp { get; set; }
        public bool IsAssignedToTransportationGroupDown { get; set; }
        public bool IsDeleted { get; set; }
        [RequiredIf(@"ScheduleStatus.ScheduleStatuses.Cancelled==ScheduleStatusID", ErrorMessageResourceName = "WhoCancelledRequired", ErrorMessageResourceType = typeof(Resource))]
        public string WhoCancelled { get; set; }
        [RequiredIf(@"ScheduleStatus.ScheduleStatuses.Cancelled==ScheduleStatusID", ErrorMessageResourceName = "WhenCancelledRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? WhenCancelled { get; set; }
        [RequiredIf(@"ScheduleStatus.ScheduleStatuses.Cancelled==ScheduleStatusID", ErrorMessageResourceName = "CancelleReasonRequired", ErrorMessageResourceType = typeof(Resource))]
        public string CancelReason { get; set; }
        public bool IsReschedule { get; set; }

        public DateTime? WeekEmailDate { get; set; }
        public DateTime? WeekSMSDate { get; set; }

        public long? WeekMasterID { get; set; }

        public bool EmailSent { get; set; }
        public bool SMSSent { get; set; }
        public bool NoticeSent { get; set; }


        public long? EmployeeID { get; set; }

        //Action as Keep Transportation Assignment or Remove transportation assignment.
        [Ignore]
        public int TransportationAssignmentAction { get; set; }

        [Ignore]
        public bool DayView { get; set; }

        public int FrequencyChoice { get; set; }
        public int Frequency { get; set; }
        public int DaysOfWeek { get; set; }
        public int WeeklyInterval { get; set; }
        public int MonthlyInterval { get; set; }
        public bool IsSundaySelected { get; set; }

        public bool IsMondaySelected { get; set; }
        public bool IsTuesdaySelected { get; set; }
        public bool IsWednesdaySelected { get; set; }
        public bool IsThursdaySelected { get; set; }
        public bool IsFridaySelected { get; set; }
        public bool IsSaturdaySelected { get; set; }
        public bool IsFirstWeekOfMonthSelected { get; set; }
        public bool IsSecondWeekOfMonthSelected { get; set; }
        public bool IsThirdWeekOfMonthSelected { get; set; }
        public bool IsFourthWeekOfMonthSelected { get; set; }
        public bool IsLastWeekOfMonthSelected { get; set; }
        public bool IsOneTimeEvent { get; set; }
        public string RecurrencePattern { get; set; }
        public string FrequencyTypeOptions { get; set; }
        public string MonthlyIntervalOptions { get; set; }
        public string ScheduleRecurrence { get; set; }


    }
}
