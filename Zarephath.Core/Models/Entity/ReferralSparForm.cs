using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Controllers;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralSparForms")]
    [PrimaryKey("ReferralSparFormID")]
    [Sort("ReferralSparFormID", "DESC")]
    public class ReferralSparForm : BaseEntity
    {
        public long ReferralSparFormID { get; set; }

        public DateTime? ReviewDate { get; set; }
        public DateTime? AdmissionDate { get; set; }

        [StringLength(100, ErrorMessageResourceName = "AssessmentDateLength", ErrorMessageResourceType = typeof(Resource))]
        public string AssessmentDate { get; set; }
        public bool? AssessmentCompletedAndSignedByBHP { get; set; }
        public bool? IdentifyDTSDTOBehavior { get; set; }
        
        //[Required(ErrorMessageResourceName = "DemographicRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? DemographicDate { get; set; }
        public bool? IsROI { get; set; }
        public bool? IsSNCD { get; set; }

        [StringLength(500, ErrorMessageResourceName = "UpdateIfApplicableLength", ErrorMessageResourceType = typeof(Resource))]
        public string DTSDTOUpdateText { get; set; }

        [StringLength(500, ErrorMessageResourceName = "AdditionalInfoLength", ErrorMessageResourceType = typeof(Resource))]
        public string AdditionInformation { get; set; }
        public bool? ServicePlanCompleted { get; set; }
        public bool? ServicePlanSignedDatedByBHP { get; set; }
        public bool? ServicePlanIdentified { get; set; }
        public bool? ServicePlanHasFrequency { get; set; }

        [StringLength(500, ErrorMessageResourceName = "AdditionalInfoLength", ErrorMessageResourceType = typeof(Resource))]
        public string ServicePlanAdditionalInfo { get; set; }
        public bool IsSparFormCompleted { get; set; }
        public bool IsSparFormOffline { get; set; }
        public long? SparFormCompletedBy { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? SparFormCompletedDate { get; set; }
        public string BHPReviewSignature { get; set; }
        public DateTime Date { get; set; }
        public long ReferralID { get; set; }


        public string CASIIScore { get; set; }
    }
}
