using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Controllers;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Notes")]
    [PrimaryKey("NoteID")]
    [Sort("NoteID", "DESC")]
    public class Note : BaseEntity
    {
        public long NoteID { get; set; }
        public long ReferralID { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public string ContinuedDX { get; set; }
        [Required(ErrorMessageResourceName = "ServiceDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime ServiceDate { get; set; }

        [RequiredIf("ServiceCodeType == 2 || ServiceCodeType == 1", ErrorMessageResourceName = "ServiceCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? ServiceCodeID { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public int MaxUnit { get; set; }
        public int DailyUnitLimit { get; set; }
        public int UnitType { get; set; }
        public float PerUnitQuantity { get; set; }

        [Required(ErrorMessageResourceName = "ServiceCodeTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int ServiceCodeType { get; set; }
        public DateTime? ServiceCodeStartDate { get; set; }
        public DateTime? ServiceCodeEndDate { get; set; }
        public bool IsBillable { get; set; }
        public bool HasGroupOption { get; set; }
        public long? ModifierID { get; set; }
        public bool CheckRespiteHours { get; set; }

        [RequiredIf("ServiceCodeID > 0 && ServiceCodeID != 3", ErrorMessageResourceName = "POSRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? PosID { get; set; }

        [RequiredIf("ServiceCodeType == 2 || ServiceCodeType == 1", ErrorMessageResourceName = "RenderingProviderRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? RenderingProviderID { get; set; }

        [RequiredIf("ServiceCodeType == 2 || ServiceCodeType == 1", ErrorMessageResourceName = "BillingProviderRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? BillingProviderID { get; set; }
        public float Rate { get; set; }
        public DateTime? POSStartDate { get; set; }
        public DateTime? POSEndDate { get; set; }
        [Required(ErrorMessageResourceName = "ZarephathServiceRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ZarephathService { get; set; }

        [Required(ErrorMessageResourceName = "StartMileRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? StartMile { get; set; }

        [Required(ErrorMessageResourceName = "EndMileRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? EndMile { get; set; }

        public DateTime? StartTime { get; set; }

        [Ignore]
        [RequiredIf("ServiceCodeID > 0 && ServiceCodeID != 3", ErrorMessageResourceName = "StartTimeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string StrStartTime
        {
            get
            {
                return StartTime.HasValue ? string.Format("{0:hh:mm tt}", StartTime.Value) : null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    DateTime timeOnly = DateTime.ParseExact(value, "hh:mm tt",
                                                            System.Globalization.CultureInfo.CurrentCulture);
                    StartTime = ServiceDate.Add(timeOnly.TimeOfDay);
                }
                else
                {
                    StartTime = ServiceDate;
                }
            }
        }

        public DateTime? EndTime { get; set; }

        [Ignore]
        [RequiredIf("ServiceCodeID > 0 && ServiceCodeID != 3", ErrorMessageResourceName = "EndTimeRequired", ErrorMessageResourceType = typeof(Resource), Priority = 1)]
        public string StrEndTime
        {
            get
            {
                return EndTime.HasValue ? string.Format("{0:hh:mm tt}", EndTime.Value) : null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    DateTime timeOnly = DateTime.ParseExact(value, "hh:mm tt",
                                                            System.Globalization.CultureInfo.CurrentCulture);
                    EndTime = ServiceDate.Add(timeOnly.TimeOfDay);
                }
                else
                {
                    EndTime = ServiceDate;
                }
            }
        }

        public int NoOfStops { get; set; }
        public float CalculatedUnit { get; set; }

        [Required(ErrorMessageResourceName = "InterventionRequired", ErrorMessageResourceType = typeof(Resource))]
        public string NoteDetails { get; set; }
        [Required(ErrorMessageResourceName = "AssessmentRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Assessment { get; set; }
        public string ActionPlan { get; set; }
        public string SpokeTo { get; set; }
        public string Relation { get; set; }
        public string OtherNoteType { get; set; }
        public bool MarkAsComplete { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? SignatureDate { get; set; }

        [RequiredIf("PosID == 99", ErrorMessageResourceName = "POSDetailsRequired", ErrorMessageResourceType = typeof(Resource))]
        public string POSDetail { get; set; }
        public long PayorServiceCodeMappingID { get; set; }
        public long PayorID { get; set; }
        public bool IsDeleted { get; set; }
        public string Source { get; set; }

        public string BillingProviderName { get; set; }
        public string BillingProviderAddress { get; set; }
        public string BillingProviderCity { get; set; }
        public string BillingProviderState { get; set; }
        public string BillingProviderZipcode { get; set; }
        public string BillingProviderEIN { get; set; }
        public string BillingProviderNPI { get; set; }
        public string BillingProviderAHCCCSID { get; set; }
        public int? BillingProviderGSA { get; set; }

        public string RenderingProviderName { get; set; }
        public string RenderingProviderAddress { get; set; }
        public string RenderingProviderCity { get; set; }
        public string RenderingProviderState { get; set; }
        public string RenderingProviderZipcode { get; set; }
        public string RenderingProviderEIN { get; set; }
        public string RenderingProviderNPI { get; set; }
        public string RenderingProviderAHCCCSID { get; set; }
        public int? RenderingProviderGSA { get; set; }

        public string PayorName { get; set; }
        public string PayorShortName { get; set; }
        public string PayorAddress { get; set; }
        public string PayorIdentificationNumber { get; set; }
        public string PayorCity { get; set; }
        public string PayorState { get; set; }
        public string PayorZipcode { get; set; }

        public float CalculatedAmount { get; set; }

        [RequiredIf("ServiceCodeType == 1", ErrorMessageResourceName = "DriverNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? DriverID { get; set; }

        //|| ServiceCodeType=2
        [RequiredIf("(DriverID != null && DriverID!=0) || (ServiceCodeType == 1)", ErrorMessageResourceName = "VehicleNumberRequired", ErrorMessageResourceType = typeof(Resource))]
        public string VehicleNumber { get; set; }

        [RequiredIf("(DriverID != null && DriverID!=0) || (ServiceCodeType == 1)", ErrorMessageResourceName = "VehicleTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string VehicleType { get; set; }

        [RequiredIf("(DriverID != null && DriverID!=0) || (ServiceCodeType == 1)", ErrorMessageResourceName = "PickUpAddressRequired", ErrorMessageResourceType = typeof(Resource))]
        public string PickUpAddress { get; set; }

        [RequiredIf("(DriverID != null && DriverID!=0) || (ServiceCodeType == 1)", ErrorMessageResourceName = "DropOffAddressRequired", ErrorMessageResourceType = typeof(Resource))]
        public string DropOffAddress { get; set; }

        public bool RoundTrip { get; set; }
        public bool OneWay { get; set; }
        public bool MultiStops { get; set; }

        public string EscortName { get; set; }
        public string Relationship { get; set; }

        public long IssueID { get; set; }

        [RequiredIf("IsIssue == true", ErrorMessageResourceName = "AssigneeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? IssueAssignID { get; set; }

        //return NoteID > 0 && IssueAssignID.HasValue && IssueAssignID > 0;
        [Ignore]
        public bool IsIssue
        {
            get
            {
                return IssueAssignID.HasValue && IssueAssignID > 0;
            }
            set { value = value; }
        }

        [Ignore]
        public string Signature { get; set; }

        public string RandomGroupID { get; set; }
        public string GroupIDForMileServices { get; set; }

        public bool DTRIsOnline { get; set; }

        [Ignore]
        public PosDropdownModel SelectedServiceCodeForPayor { get; set; }

        [Ignore]
        public bool AssigneeNeeded { get; set; }

        //[Required(ErrorMessageResourceName = "NoteAssigneeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? NoteAssignee { get; set; }

        //[Required(ErrorMessageResourceName = "NoteCommentRequired", ErrorMessageResourceType = typeof(Resource))]
        public string NoteComments { get; set; }
        


        public long? NoteAssignedBy { get; set; }
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? NoteAssignedDate { get; set; }


        [Ignore]
        public EmpSignatureDetails EmpSignatureDetails { get; set; }
        
    }
}
