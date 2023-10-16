using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Controllers;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralCheckLists")]
    [PrimaryKey("ReferralCheckListID")]
    [Sort("ReferralCheckListID", "DESC")]
    public class ReferralCheckList : BaseEntity
    {
        public long ReferralCheckListID { get; set; }

        [Display(Name = "ServicRequestText", ResourceType = typeof(Resource))]
        public bool ServiceRequested { get; set; }
        public bool RespiteService { get; set; }
        public bool LifeSkillsService { get; set; }

        public bool CounselingService { get; set; }

        public bool ConnectingFamiliesService { get; set; }

        [Display(Name = "FacilitatorContactText", ResourceType = typeof(Resource))]
        public bool FacilitatorContacted { get; set; }
        [StringLength(100, ErrorMessageResourceName = "FacilatorContactedLength", ErrorMessageResourceType = typeof(Resource))]
        public string FacilitatorContactedText { get; set; }

        [Display(Name = "AHCCCSVerificationText", ResourceType = typeof(Resource))]
        public bool AHCCCSVerification { get; set; }
        //[StringLength(100, ErrorMessageResourceName = "AHCCCSVerificationLength", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? AHCCCSVerificationText { get; set; }

        [Display(Name = "AgencyServiceText", ResourceType = typeof(Resource))]
        public string ASPRespiteIntervention { get; set; }

        [StringLength(100, ErrorMessageResourceName = "ASPRespiteInterventionLength", ErrorMessageResourceType = typeof(Resource))]
        public string ASPRespiteInterventionText { get; set; }

        [Display(Name = "ServicePlanExpireText", ResourceType = typeof(Resource))]
        public bool SPExpireAndGuardianBHPSigned { get; set; }
        //[StringLength(100, ErrorMessageResourceName = "SPExpireAndGuardianBHPSignedLength", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? SPExpireAndGuardianBHPSignedText { get; set; }

        [Display(Name = "SPSignedByBhpText", ResourceType = typeof(Resource))]
        public bool SPSignedByBHP { get; set; }
        [StringLength(100, ErrorMessageResourceName = "SPSignedByBHPLength", ErrorMessageResourceType = typeof(Resource))]
        public string SPSignedByBHPText { get; set; }

        [Display(Name = "SPSignedByGuardianText", ResourceType = typeof(Resource))]
        public bool SPSignedByGuardian { get; set; }
        [StringLength(100, ErrorMessageResourceName = "SPSignedByGuardianLength", ErrorMessageResourceType = typeof(Resource))]
        public string SPSignedByGuardianText { get; set; }

        [Display(Name = "CoreAssessmentOrAnnualUpdateText", ResourceType = typeof(Resource))]
        public bool CoreAssessmentORAnnualUpdateCompleted { get; set; }
        //[StringLength(100, ErrorMessageResourceName = "CoreAssessmentORAnnualUpdateCompletedLength", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? CoreAssessmentORAnnualUpdateCompletedText { get; set; }

        [Display(Name = "DiagnosisCodeText", ResourceType = typeof(Resource))]
        public bool DiagnosisCodes { get; set; }
        [StringLength(100, ErrorMessageResourceName = "DiagnosisCodesLength", ErrorMessageResourceType = typeof(Resource))]
        public string DiagnosisCodesText { get; set; }

        [Display(Name = "AssessmentBHPSignedText", ResourceType = typeof(Resource))]
        public bool AssessmentBHPSigned { get; set; }
        public string AssessmentBHPSignedChecked { get; set; }

        [Display(Name = "SNCDText", ResourceType = typeof(Resource))]
        public string SNCD { get; set; }
        //[StringLength(100, ErrorMessageResourceName = "SNCDLength", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? SNCDText { get; set; }

        [Display(Name = "DemographicsText", ResourceType = typeof(Resource))]
        public string Demographics { get; set; }
        //[StringLength(100, ErrorMessageResourceName = "DemographicsLength", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? DemographicsText { get; set; }

        [Display(Name = "ROIText", ResourceType = typeof(Resource))]
        public string ROI { get; set; }
        [StringLength(100, ErrorMessageResourceName = "ROILength", ErrorMessageResourceType = typeof(Resource))]
        public string ROIText { get; set; }

        public bool ReferralChecklist { get; set; }
        [StringLength(100, ErrorMessageResourceName = "ReferralChecklistLength", ErrorMessageResourceType = typeof(Resource))]
        public string ReferralChecklistText { get; set; }

        [Display(Name = "DateRequestUpdatedPaperworkText", ResourceType = typeof(Resource))]
        public bool RequestPaperWork { get; set; }
        [StringLength(100, ErrorMessageResourceName = "RequestPaperWorkLength", ErrorMessageResourceType = typeof(Resource))]
        public string RequestPaperWorkText { get; set; }

        [Display(Name = "DatePaperworkReceivedText", ResourceType = typeof(Resource))]
        public bool PaperWorkReceived { get; set; }
        [StringLength(100, ErrorMessageResourceName = "PaperWorkReceivedLength", ErrorMessageResourceType = typeof(Resource))]
        public string PaperWorkReceivedText { get; set; }

        [Display(Name = "NotesLabel", ResourceType = typeof(Resource))]
        [StringLength(500, ErrorMessageResourceName = "NotesLength", ErrorMessageResourceType = typeof(Resource))]
        public string Notes { get; set; }
        public bool IsCheckListCompleted { get; set; }
        public bool IsCheckListOffline { get; set; }


        public long? ChecklistCompletedBy { get; set; }

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? ChecklistCompletedDate { get; set; }

        public long ReferralID { get; set; }


        [Ignore]
        public long? ROIFromAgencyID { get; set; }

        [Ignore]
        public string Payor { get; set; }

        [Ignore]
        public string ROIFromAgencyName { get; set; }
        [Ignore]
        public DateTime? ROIFromExpirationDate { get; set; }
        [Ignore]
        [Required(ErrorMessageResourceName = "CaseManagerAgencyRequired", ErrorMessageResourceType = typeof(Resource))]
        public long CaseManagerID { get; set; }
        [Ignore]
        public long AgencyID { get; set; }

        [Ignore]
        public bool CASIIScore { get; set; }

        [Ignore]
        public string CASIIScoreText { get; set; }

        [Ignore]
        public string ReferringAgency { get; set; }
        [Ignore]
        public string FacilatorInformation { get; set; }

        

        

    }
}
