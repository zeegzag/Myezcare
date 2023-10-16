using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Zarephath.Core.Controllers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class ReferralList
    {
        public long ReferralID { get; set; }
        public string Name { get; set; }
        public long ClientID { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }

        public int ReferralStatusID { get; set; }
        public string Status { get; set; }
        public long Assignee { get; set; }
        public bool IsSaveAsDraft { get; set; }
        public string AssigneeName { get; set; }
        public long PayorID { get; set; }
        public string ContractName { get; set; }
        public long CaseManagerID { get; set; }
        public string FaciliatorName { get; set; }
        public bool ZSPLifeSkills { get; set; }
        public bool ZSPRespite { get; set; }
        public bool ZSPCounselling { get; set; }
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        public DateTime CreatedDate { get; set; }

        public long CreatedBy { get; set; }


        public DateTime UpdatedDate { get; set; }
        public string UpdatedName { get; set; }
        public string CreatedName { get; set; }
        public string CompanyName { get; set; }
        public string LocationName { get; set; }
        public bool IsChecklistCompleted { get; set; }
        public long ChecklistCompletedBy { get; set; }
        public string ChecklistName { get; set; }
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? ChecklistCompletedDate { get; set; }
        public bool IsSparFormCompleted { get; set; }

        public long SparFormCompletedBy { get; set; }
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? SparFormCompletedDate { get; set; }
        public string ClinicalReviewName { get; set; }
        public string PlacementRequirement { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }
        public bool NotifyCaseManager { get; set; }
        public long AgencyID { get; set; }
        public long AgencyLocationID { get; set; }
        public bool CounselingService { get; set; }
        public bool LifeSkillsService { get; set; }
        public bool RespiteService { get; set; }
        public string ClientNickName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string FullAddress
        {
            get
            {
                if (!string.IsNullOrEmpty(Address))
                    return String.Format("{0} {1} - {2} {3}", Address, City, ZipCode, StateCode);

                return "";
            }
        }
        public string Services
        {
            get
            {
                List<string> str = new List<string>();
                if (RespiteService)
                {
                    str.Add(Constants.Respite);
                }
                if (LifeSkillsService)
                {
                    str.Add(Constants.Life_Skills);
                }
                if (CounselingService)
                {
                    str.Add(Constants.Counselling);
                }
                if (str.Count > 0)
                {
                    return string.Join(",", str.ToArray());
                }
                return string.Empty;
            }


        }

        public bool IsDeleted { get; set; }

        public string Age { get; set; }
        public string Gender { get; set; }

        public long Row { get; set; }

        public int LastReferralStatusID { get { return ReferralStatusID; } }


        public int Count { get; set; }
        public string GroupNames { get; set; }
        public string PayorName { get; set; }
        public string GoogleFileId { get; set; }
        public long ReferralDocumentID { get; set; }


    }

    public class ReferralDetailsModel
    {
        public ReferralDetailsModel()
        {
            ReferralBasicDetails = new ReferralBasicDetails();
            EmergencyContactDetails = new Contact();
            Authorizations = new List<ReferralBillingAuthorizationList>();
            DxCodeMappings = new List<DXCodeMappingList>();
            Payors = new List<ListReferralPayorMapping>();
            BeneficiaryTypes = new List<ReferralBeneficiaryType>();
        }

        public ReferralBasicDetails ReferralBasicDetails { get; set; }
        public Contact EmergencyContactDetails { get; set; } 
        public List<ReferralBillingAuthorizationList> Authorizations { get; set; }
        public List<DXCodeMappingList> DxCodeMappings { get; set; }
        public List<ListReferralPayorMapping> Payors { get; set; }
        public List<ReferralBeneficiaryType> BeneficiaryTypes { get; set; }
    }

    public class ReferralBasicDetails
    {
        public string AHCCCSID { get; set; }
        public string HealthPlan { get; set; }
        public string CareGiver { get; set; }
        public long AgencyID { get; set; }
        public string AgencyPhone { get; set; }
        public string AgencyAddress { get; set; }
        public string AgencyCity { get; set; }
        public string AgencyState { get; set; }
        public string AgencyZipCode { get; set; }
        public string AgencyTIN { get; set; }
        public string AgencyEIN { get; set; }
        public string AgencyMobile { get; set; }
        public string CaseManagerName { get; set; }
    }
}
