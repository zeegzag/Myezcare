using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeCareApi.Models.ViewModel
{
    public class CMS485DataRequest
    {
        public long ReferralID { get; set; }
        public long OrganizationID { get; set; }
    }

    public class CMS485Data
    {
        public CMS485Data()
        {
            this.OrganizationSettings = new OrganizationSettings();
            this.Referral = new Referral();
            this.Payors = new List<Payor>();
            this.DXCodes = new List<DXCode>();
            this.Medications = new List<Medication>();
            this.Assignee = new Assignee();
        }
        public OrganizationSettings OrganizationSettings { get; set; }
        public Referral Referral { get; set; }
        public List<Payor> Payors { get; set; }
        public List<DXCode> DXCodes { get; set; }
        public List<Medication> Medications { get; set; }
        public Assignee Assignee { get; set; }
    }

    public class Referral
    {
        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string AccountNumber { get; set; }
        public string Email { get; set; }
        public string MainPhone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    public class Payor
    {
        public long PayorID { get; set; }
        public string PayorName { get; set; }
        public DateTime PayorEffectiveDate { get; set; }
        public DateTime PayorEffectiveEndDate { get; set; }
        public long BeneficiaryTypeID { get; set; }
        public string BeneficiaryType { get; set; }
        public string BeneficiaryNumber { get; set; }
    }

    public class DXCode
    {
        public long DXCodeID { get; set; }
        public string DXCodeName { get; set; }
        public string DxCodeTypeID { get; set; }
        public string DxCodeTypeName { get; set; }
        public string DxCodeShortName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    
    public class Medication
    {
        public long MedicationID { get; set; }
        public string MedicationName { get; set; }
        public long PhysicianID { get; set; }
        public string PhysicianFirstName { get; set; }
        public string PhysicianLastName { get; set; }
        public string Strength { get; set; }
        public string Unit { get; set; }
        public string Frequency { get; set; }
        public string Route { get; set; }
        public string Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class Assignee
    {
        public long EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public string SignaturePath { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
    }

    #region Referral Chart
    public class ReferralChartDataRequest
    {
        public long ReferralID { get; set; }
        public long OrganizationID { get; set; }
    }

    public class OrganizationDataRequest
    {
        public long OrganizationID { get; set; }
    }

    public class OrganizationData
    {
        public OrganizationData()
        {
            this.OrganizationSettings = new OrganizationSettings();
        }
        public OrganizationSettings OrganizationSettings { get; set; }
    }

    public class ReferralPersonalList
    {
        public ReferralPersonalList()
        {
            this.ReferralPersonalModel = new ReferralPersonalModel();
        }
        public ReferralPersonalModel ReferralPersonalModel { get; set; }
    }

    public class ReferralContactList
    {
        public ReferralContactList()
        {
            this.ReferralContactModel = new List<ReferralContactModel>();
        }
        public List<ReferralContactModel> ReferralContactModel { get; set; }
    }

    public class ReferralPhysicianList
    {
        public ReferralPhysicianList()
        {
            this.ReferralPhysicianModel = new List<ReferralPhysicianModel>();
        }
        public List<ReferralPhysicianModel> ReferralPhysicianModel { get; set; }
    }

    public class ReferralDXCodeList
    {
        public ReferralDXCodeList()
        {
            this.ReferralDXCodeModel = new List<ReferralDXCodeModel>();
        }
        public List<ReferralDXCodeModel> ReferralDXCodeModel { get; set; }
    }

    public class ReferralMedicationList
    {
        public ReferralMedicationList()
        {
            this.ReferralMedicationModel = new List<ReferralMedicationModel>();
        }
        public List<ReferralMedicationModel> ReferralMedicationModel { get; set; }
    }

    public class ReferralNotesList
    {
        public ReferralNotesList()
        {
            this.ReferralNotesModel = new List<ReferralNotesModel>();
        }
        public List<ReferralNotesModel> ReferralNotesModel { get; set; }
    }

    public class ReferralPreferencesList
    {
        public ReferralPreferencesList()
        {
            this.ReferralPreferencesModel = new List<ReferralPreferencesModel>();
        }
        public List<ReferralPreferencesModel> ReferralPreferencesModel { get; set; }
    }

    public class ReferralTaskMappingList
    {
        public ReferralTaskMappingList()
        {
            this.ReferralTaskMappingModel = new List<ReferralTaskMappingModel>();
        }
        public List<ReferralTaskMappingModel> ReferralTaskMappingModel { get; set; }
    }

    public class ReferralPayorList
    {
        public ReferralPayorList()
        {
            this.ReferralPayorModel = new List<ReferralPayorModel>();
        }
        public List<ReferralPayorModel> ReferralPayorModel { get; set; }
    }

    public class ReferralAllergyList
    {
        public ReferralAllergyList()
        {
            this.ReferralAllergyModel = new List<ReferralAllergyModel>();
        }
        public List<ReferralAllergyModel> ReferralAllergyModel { get; set; }
    }

    public class ReferralBillingAuthorizationList
    {
        public ReferralBillingAuthorizationList()
        {
            this.ReferralBillingAuthorizationModel = new List<ReferralBillingAuthorizationModel>();
        }
        public List<ReferralBillingAuthorizationModel> ReferralBillingAuthorizationModel { get; set; }
    }

    public class OrganizationSettings
    {
        public long OrganizationID { get; set; }
        public string SiteLogo { get; set; }
        public string SiteName { get; set; }
        public string SupportEmail { get; set; }
        public string Phone { get; set; }
        public string OrganizationAddress { get; set; }
        public string OrganizationCity { get; set; }
        public string OrganizationState { get; set; }
        public string OrganizationZipcode { get; set; }
        public string OrgAddress { get; set; }
        public string OrgSiteNameAndAddress { get; set; }
        public string TermsCondition { get; set; }
    }

    public class ReferralPersonalModel
    {
        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ReferralFullName { get; set; }
        public string ReferralAddress { get; set; }
        public string ReferralNameAndAddress { get; set; }
        public string ApartmentNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string DateOfBirth { get; set; }
        public string Age { get; set; }
        public string DobInDateFormat
        {
            get
            {
                if (DateOfBirth!=null)
                {
                    return DateOfBirth = Convert.ToDateTime(DateOfBirth).ToString("yyyy-MM-dd");

                }
                return string.Empty;
            }
        }
        public string Gender { get; set; }
        public string Language { get; set; }
        public string SSN { get; set; }
        public string AccountNumber { get; set; }
        public string Status { get; set; }
        public string CaseManagerName { get; set; }
        public string ProfileImagePath { get; set; }
        public string Assignee { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string BloodGroup { get; set; }
        public string Race { get; set; }
        public string Ethnicity { get; set; }
        public string BMI { get; set; }
        public string CreatedDate { get; set; }
    }

    public class ReferralContactModel
    {
        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MainPhone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string FullAddress { get; set; }
        public string ContactTypeName { get; set; }
        public string IsEmergencyContact { get; set; }
    }

    public class ReferralPhysicianModel
    {
        public long ReferralID { get; set; }
        public long PhysicianID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhysicianName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string PhysicianAddress { get; set; }
        public string PhysicianTypeID { get; set; }
    }

    public class ReferralDXCodeModel
    {
        public long ReferralID { get; set; }
        public long DXCodeID { get; set; }
        public string DXCodeName { get; set; }
        public string DxCodeTypeID { get; set; }
        public string DxCodeTypeName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CS_DXCodeName { get; set; }
    }

    public class ReferralMedicationModel
    {
        public long ReferralID { get; set; }
        public long MedicationID { get; set; }
        public string MedicationName { get; set; }
        public long PhysicianID { get; set; }
        public string PhysicianName { get; set; }
        public string Strength { get; set; }
        public string Unit { get; set; }
        public string Frequency { get; set; }
        public string Route { get; set; }
        public string Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class ReferralNotesModel
    {
        public long ReferralID { get; set; }
        public long CommonNoteID { get; set; }
        public string Note { get; set; }
        public string CreatedDate { get; set; }
        public string NotesAddedBy { get; set; }
    }

    public class ReferralPreferencesModel
    {
        public long ReferralID { get; set; }
        public string PreferenceName { get; set; }
        public string KeyType { get; set; }
    }

    public class ReferralTaskMappingModel
    {
        public long ReferralID { get; set; }
        public string VisitTaskType { get; set; }
        public string VisitTaskDetail { get; set; }
        public string VisitTaskCategoryName { get; set; }
        public string CarePlan { get; set; }
    }

    public class ReferralPayorModel
    {
        public long PayorID { get; set; }
        public string PayorName { get; set; }
        public string PayorEffectiveDate { get; set; }
        public string PayorEffectiveEndDate { get; set; }
        public long BeneficiaryTypeID { get; set; }
        public string BeneficiaryType { get; set; }
        public string BeneficiaryNumber { get; set; }
    }

    public class ReferralAllergyModel
    {
        public long Id { get; set; }
        public string Allergy { get; set; }
        public string Reaction { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public string CS_Allergy { get; set; }
        public string CS_Reaction { get; set; }
    }

    public class ReferralBillingAuthorizationModel
    {
        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AuthorizationCode { get; set; }
        public string Type { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    #endregion

    #region Employee Details 
    public class DataRequest
    {
        public long EmployeeID { get; set; }
        public long OrganizationID { get; set; }
    }

    public class EmployeePersonalList
    {
        public EmployeePersonalList()
        {
            this.EmployeePersonalModel = new List<EmployeePersonalModel>();
        }
        public List<EmployeePersonalModel> EmployeePersonalModel { get; set; }
    }

    public class EmployeePersonalModel
    {
        public long EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string EmployeeUniqueID { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }
        public string Designation { get; set; }
        public string Age { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneHome { get; set; }
        public string PhoneWork { get; set; }
        public string AssociateWith { get; set; }
        public string RoleID { get; set; }
        public string EmployeeAddress { get; set; }
        public string EmployeeNameAndAddress { get; set; }
        public string ApartmentNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string EmpSignature { get; set; }
        public string ProfileImagePath { get; set; }
    }

    #endregion

}