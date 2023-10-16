using System;
using System.Collections.Generic;
using ExpressiveAnnotations.Attributes;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Controllers;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class AttendanceMasterPageModel
    {
        public AttendanceMasterPageModel()
        {
            ScheduleStatuses = new List<ScheduleStatus>();
            RegionList = new List<Region>();
            Facilities = new List<FacilityModel>();
            AttendanceMasterSearchModel = new AttendanceMasterSearchModel();
            ScheduleMaster = new ScheduleMaster();
        }
        public List<ScheduleStatus> ScheduleStatuses { get; set; }
        public List<Region> RegionList { get; set; }
        public List<FacilityModel> Facilities { get; set; }

        [Ignore]
        public AttendanceMasterSearchModel AttendanceMasterSearchModel { get; set; }

        [Ignore]
        public ScheduleMaster ScheduleMaster { get; set; }

        [Ignore]
        public List<NameValueDataInString> CancellationReasons { get; set; }


    }

    public class AttendanceMasterSearchModel
    {
        public long FacilityID { get; set; }
        public string ClientName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class AttendanceDetail
    {
        public long AttendanceMasterID { get; set; }
        public long ScheduleMasterID { get; set; }
        public long ReferralID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comment { get; set; }
        public int? AttendanceStatus { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime UpdatedDate { get; set; }
        public string UpdatedByName { get; set; }



        #region Schedule Master Details
        public long PickUpLocation { get; set; }
        public long DropOffLocation { get; set; }
        public long ScheduleStatusID { get; set; }
        public string Comments { get; set; }
        public bool IsAssignedToTransportationGroupUp { get; set; }
        public bool IsAssignedToTransportationGroupDown { get; set; }
        [RequiredIf(@"ScheduleStatus.ScheduleStatuses.Cancelled==ScheduleStatusID", ErrorMessageResourceName = "WhoCancelledRequired", ErrorMessageResourceType = typeof(Resource))]
        public string WhoCancelled { get; set; }
        [RequiredIf(@"ScheduleStatus.ScheduleStatuses.Cancelled==ScheduleStatusID", ErrorMessageResourceName = "WhenCancelledRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? WhenCancelled { get; set; }
        [RequiredIf(@"ScheduleStatus.ScheduleStatuses.Cancelled==ScheduleStatusID", ErrorMessageResourceName = "CancelleReasonRequired", ErrorMessageResourceType = typeof(Resource))]
        public string CancelReason { get; set; }
        public bool IsReschedule { get; set; }

        #endregion
    }

    public class FacilityAttendanceDetails
    {
        public List<AttendanceDetail> AttendanceDetails { get; set; }
        public Facility FacilityDetail { get; set; }
        
    }
}
