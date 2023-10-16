using PetaPoco;
using System;
using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("EmployeeVisits")]
    [PrimaryKey("EmployeeVisitID")]
    [Sort("EmployeeVisitID", "DESC")]
    public class EmployeeVisit : BaseEntity
    {
        public long EmployeeVisitID { get; set; }
        public long ScheduleID { get; set; }
        public DateTime ClockInTime { get; set; }
        public DateTime ClockOutTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool SurveyCompleted { get; set; }
        public string SurveyComment { get; set; }
        public bool IsByPassClockIn { get; set; }
        public bool IsByPassClockOut { get; set; }
        public string ByPassReasonClockIn { get; set; }
        public string ByPassReasonClockOut { get; set; }
        public string PlaceOfService { get; set; }
        public string HHA_PCA_NP { get; set; }
        public string OtherActivity { get; set; }
        public long OtherActivityTime { get; set; }
        public bool IsSigned { get; set; }
        public bool IsPCACompleted { get; set; }
        public long EmployeeSignatureID { get; set; }
        public string PatientSignature { get; set; }
        public long IVRClockOut { get; set; }
        public string BeneficiaryID { get; set; }
        public string EarlyClockOutComment { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float PCACompletedLat { get; set; }
        public float PCACompletedLong { get; set; }
        public string PCACompletedIPAddress { get; set; }
        public float SignedLat { get; set; }
        public float SignedLong { get; set; }
        public string SignedIPAddress { get; set; }
        public int ActionTaken { get; set; }
        public string RejectReason { get; set; }
        public bool IsApprovalRequired { get; set; }

        public enum BypassActions
        {
            [Display(ResourceType = typeof(Resource), Name = "Status")]
            All = 0,
            [Display(ResourceType = typeof(Resource), Name = "Pending")]
            Pending = 1,
            [Display(ResourceType = typeof(Resource), Name = "Approved")]
            Approved,
            [Display(ResourceType = typeof(Resource), Name = "Rejected")]
            Rejected,
            [Display(ResourceType = typeof(Resource), Name = "InComplete")]
            InComplete = 4,
            [Display(ResourceType = typeof(Resource), Name = "Complete")]
            Complete = 5
        }
    }
}
