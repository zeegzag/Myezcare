using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Models.ViewModel
{
    public class AddManageClaimModel
    {
        public AddManageClaimModel()
        {
            PayorList = new List<PayorList>();
            PatientList = new List<ReferralListModel>();
            ListManageClaimsModel = new ListManageClaimsModel();
        }
        public List<PayorList> PayorList { get; set; }
        public List<ReferralListModel> PatientList { get; set; }
        [Ignore]
        public long BatchId { get; set; }
        [Ignore]
        public long InsuredID { get; set; }
        [Ignore]
        public string FDOS { get; set; }
        [Ignore]
        public ListManageClaimsModel ListManageClaimsModel { get; set; }
    }
    public class ManageClaimModel
    {



    }

    public class SearchClaimListModel
    {

    }

    public class ListManageClaimsModel
    {
        public long BatchUploadedClaimID { get; set; }
        public string PatientName { get; set; }
        public string BatchID { get; set; } // Acct
        public long BatchTypeID { get; set; }
        public long PayorID { get; set; }
        public long ReferralID { get; set; }
        public string Payer { get; set; }
        public string INS_Number { get; set; } //InsuredID
        public string FDOS { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public double Charges { get; set; }
        public string BillingProvider { get; set; }
        public string Status { get; set; }
        public DateTime AddedDate { get; set; }
        public double Total_Charge { get; set; }
        public string Count { get; set; }
    }

    public class ListManageERAsModel
    {

    }
    public class HC_ListClaimErrors
    {
        public long BatchUpClaimErrorID { get; set; }
        public long BatchUploadedClaimID { get; set; }
        public string Field { get; set; }
        public string MsgID { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
    public class ClaimErrorsAndCMS1500
    {
        public List<HC_ListClaimErrors> ClaimErrors { get; set; }
        public List<ListBatchModel> BatchList { get; set; }
        public object CMS1500 { get; set; }
    }
    public class SaveCMS1500Modal
    {
        public long BatchID { get; set; }
        public long NoteID { get; set; }
        public long BatchUploadedClaimID { get; set; }

        public long PayorID { get; set; }
        public string PayorIdentificationNumber { get; set; }
        public string PayorName { get; set; }
        public long ReferralID { get; set; }
        public string AHCCCSID { get; set; }

        public long ContactID { get; set; }
        public string PatientAddress { get; set; }
        public string PatientCity { get; set; }
        public string PatientState { get; set; }
        public string PatientZipCode { get; set; }
        //public string PatientPhone { get; set; }

        public string PatientDOB { get; set; }

        public long PhysicianID { get; set; }
        public string Ref_NPI { get; set; }

        public long ReferralBillingAuthorizationID { get; set; }
        public string AuthorizationCode { get; set; }

        public string ServiceDate { get; set; }
        public int PlaceOfServiceID { get; set; }
        public string PlaceOfService { get; set; }
        public int Mod1_1_ID { get; set; }
        public string Mod1_1_Code { get; set; }
        public int Mod2_1_ID { get; set; }
        public string Mod2_1_Code { get; set; }
        public int Mod3_1_ID { get; set; }
        public string Mod3_1_Code { get; set; }
        public int Mod4_1_ID { get; set; }
        public string Mod4_1_Code { get; set; }

        public string BillingProviderEIN { get; set; }

        public string BillingProviderNPI { get; set; }
    }
}
