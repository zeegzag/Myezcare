using System;
using System.Collections.Generic;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class GetCms485Model
    {
        public GetCms485Model()
        {
            Cms485Model = new Cms485Model();
            OrganizationalModel = new OrganizationalModel();
            //DmasPayorList = new List<DmasPayorList>();
            CmsReferrals = new CmsReferrals();
        }
        public Cms485Model Cms485Model { get; set; }
        public OrganizationalModel OrganizationalModel { get; set; }
        //public List<DmasPayorList> DmasPayorList { get; set; }
        public CmsReferrals CmsReferrals { get; set; }

    }

    public class CmsReferrals
    {
        public long ReferralID { get; set; }
        public string ReferralAddress { get; set; }
        public string Dob { get; set; }
        public string Gender { get; set; }
    }

    public class Cms485Model
    {
        public long Cms485ID { get; set; }
        public string PatientClaimNo { get; set; }
        public string StartOfCareDate { get; set; }
        public string CertificationPeriodFrom { get; set; }
        public string CertificationPeriodTo { get; set; }
        public string MedicalRecordNo { get; set; }
        public string ProviderNo { get; set; }
        public string PatientName { get; set; }
        public string PatientAddress { get; set; }
        public string ProviderName { get; set; }
        public string ProviderAddress { get; set; }
        public string ProviderTelephone { get; set; }
        public string DOB { get; set; }
        public string Sex { get; set; }
        public string Medications { get; set; }
        public string ICD9CM1 { get; set; }
        public string PrincipalDiagnosis  { get; set; }
        public string PrincipalDiagnosisDate { get; set; }
        public string ICD9CM2 { get; set; }
        public string SurgicalProcedure { get; set; }
        public string SurgicalProcedureDate { get; set; }
        public string ICD9CM3 { get; set; }
        public string PertinentDiagnoses { get; set; }
        public string PertinentDiagnosesDate { get; set; }
        public string DmeAndSupplie { get; set; }
        public string SafetyMeasures { get; set; }
        public string NutritionalReq { get; set; }
        public string Allergies { get; set; }

        public bool Amputation { get; set; }
        public bool BowelBladder { get; set; }
        public bool Contracture { get; set; }
        public bool Hearing { get; set; }
        public bool Paralysis { get; set; }
        public bool Endurance { get; set; }
        public bool Ambulation { get; set; }
        public bool Speech { get; set; }
        public bool LegallyBlind { get; set; }
        public bool DyspneaWithMinimalExertion { get; set; }
        public bool FunctionalLimitationsOther { get; set; }

        public bool CompleteBedrest  { get; set; }
        public bool BedrestBRP { get; set; }
        public bool UpAsTolerated { get; set; }
        public bool TransferBedChair { get; set; }
        public bool ExercisePrescribed { get; set; }
        public bool PartialWeightBearing { get; set; }
        public bool IndependentAtHome { get; set; }
        public bool Crutches { get; set; }
        public bool Cane { get; set; }
        public bool Wheelchair { get; set; }
        public bool Walker { get; set; }
        public bool NoRestrictions { get; set; }
        public bool ActivitiesPermittedOther { get; set; }

        public bool Oriented { get; set; }
        public bool Comatose { get; set; }
        public bool Forgetful { get; set; }
        public bool Depressed { get; set; }
        public bool Disoriented { get; set; }
        public bool Lethargic { get; set; }
        public bool Agitated { get; set; }
        public bool MentalStatusOther { get; set; }

        public bool Poor { get; set; }
        public bool Guarded { get; set; }
        public bool Fair { get; set; }
        public bool Good { get; set; }
        public bool Excellent { get; set; }

        public string OrdersForDisciplineTreatments { get; set; }
        public string GoalsRehabilitationPotentialDischargePlans { get; set; }
        public string NurseSignOfVerbalSOC { get; set; }
        public string NurseDateOfVerbalSOC { get; set; }
        public string DateHHAReceivedSignedPOT { get; set; }
        public string PhysicianName { get; set; }
        public string PhysicianAddress { get; set; }
        public string AttendingPhysicianSign { get; set; }
        public string AttendingPhysicianDate { get; set; }
        
        public string JsonData { get; set; }
        public long EmployeeID { get; set; }
        public long ReferralID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
        public string EncryptedCms485ID { get { return Crypto.Encrypt(Convert.ToString(Cms485ID)); } }
        public string EncryptedReferralID { get; set; }
    }

    public class Cms485AddModel
    {
        public long Cms485ID { get; set; }
        public string EmployeeName { get; set; }
        public string JsonData { get; set; }
        public string CreatedDate { get; set; }
        public string EncryptedReferralID { get; set; }
    }

    public class Cms485CloneModel
    {
        public long Cms485ID { get; set; }
        public string EmployeeName { get; set; }
        public string JsonData { get; set; }
        public string CreatedDate { get; set; }
        public string EncryptedReferralID { get; set; }
    }

    public class Cms485ListModel
    {
        public Cms485ListModel()
        {
            Cms485FormList = new List<Cms485ModelList>();
        }
        public List<Cms485ModelList> Cms485FormList { get; set; }
    }

    public class Cms485ModelList
    {
        public long Cms485ID { get; set; }
        public string EmployeeName { get; set; }
        public string JsonData { get; set; }
        public string CreatedDate { get; set; }

    }

 

   

}
