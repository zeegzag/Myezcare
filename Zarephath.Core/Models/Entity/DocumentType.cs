using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("DocumentTypes")]
    [PrimaryKey("ReferralDocumentID")]
    [Sort("ReferralDocumentID", "DESC")]
    public class DocumentType
    {
        public int DocumentTypeID { get; set; }
        public string DocumentTypeName { get; set; }
        public string KindOfDocument { get; set; }
        public enum DocumentKind
        {
            Internal = 1,
            External
        }

        public enum DocumentTypes
        {
            Care_Consent = 1,
            Self_Administration_of_Medication,
            Health_Information_Disclosure,
            Admission_Requirements,
            Admission_Orientation,
            Zarephath_Crisis_Plan,
            Network_Crisis_Plan,
            Zarephath_Service_Plan_Respite,
            Zarephath_Service_Plan_Life_Skills,
            Zarephath_Service_Plan_Counseling,
            PHI_Referring_Agency,
            Agency_ROI,
            Agency_Network_Service_Plan,
            BX_Assessment,
            Demographic,
            SNCD,
            Ansell_Casey_Assessment,
            CAZ_Only_Referral_Checklist,
            Other,
            Permission_For_Phone,
            Permission_For_SMS,
            Permission_For_Email,
            Monthly_Summary,
            RecordRequest,
            SignaturePage,
            SZarephath_Service_Plan_Connecting_Families,
            OtherExternal,
            GuardianshipPaperwork,
            BX_Contract
        }
    }


}
