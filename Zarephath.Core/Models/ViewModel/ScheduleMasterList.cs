using System;
using Zarephath.Core.Infrastructure;

namespace Zarephath.Core.Models.ViewModel
{
    public class ScheduleMasterList
    {
        public long ScheduleID { get; set; }
        public long ReferralID { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }
        public long FacilityID { get; set; }
        public string FacilityName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ScheduleStatusID { get; set; }
        public string ScheduleStatusName { get; set; }
        public string PlacementRequirement { get; set; }
        public string Comments { get; set; }
        public long PickUpLocation { get; set; }
        public string PickupLocationName { get; set; }
        public long DropOffLocation { get; set; }
        public string DropOffLocationName { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public bool PermissionForEmail { get; set; }
        public bool PermissionForSMS { get; set; }
        public bool PermissionForVoiceMail { get; set; }
        public string WhoCancelled { get; set; }
        public DateTime? WhenCancelled { get; set; }
        public string CancelReason { get; set; }
        public string BehavioralIssue { get; set; }
        public bool IsReschedule { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public long RegionID { get; set; }
        public string RegionName { get; set; }
        public bool PermissionForMail { get; set; }
        public long Row { get; set; }
        public int Count { get; set; }
        public string Age { get; set; }

        public bool EmailSent { get; set; }
        public bool SMSSent { get; set; }
        public bool NoticeSent { get; set; }

        public bool PCMVoiceMail { get; set; }
        public bool PCMMail { get; set; }
        public bool PCMSMS { get; set; }
        public bool PCMEmail { get; set; }

        public long? EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmpEmail { get; set; }
        public string EmpMobile { get; set; }
        public string EmpAddress { get; set; }
        public string PatAddress { get; set; }



        public bool? TempIsPatientAttendedSchedule { get; set; }
        public bool? IsPatientAttendedSchedule { get; set; }
        public string AbsentReason { get; set; }
        public long CareTypeId { get; set; }
        public string CareType { get; set; }


        public long? EmployeeVisitID { get; set; }
        public long? ReferralTSDateID { get; set; }
    }
}

