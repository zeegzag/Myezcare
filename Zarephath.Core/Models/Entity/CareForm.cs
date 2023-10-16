using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    //[TableName("CareForms")]
    //[PrimaryKey("CareFormID")]
    //[Sort("CareFormID", "DESC")]
    public class CareForm : BaseEntity
    {
        public long CareFormID { get; set; }
        public long ReferralID { get; set; }
        public DateTime? CareFormDate { get; set; }

        public string LocationOfService { get; set; }
        public string Phone { get; set; }
        public string Cell { get; set; }
        public string Email { get; set; }

        public DateTime? PSI_StartDate { get; set; }
        public DateTime? PSI_EndDate { get; set; }

        //[Required(ErrorMessageResourceName = "ServiceRequestedRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ServiceRequested { get; set; } // VisitType DDMaster FK

        public bool IsMedicallyFrail { get; set; }
        public string SpecificFunctionalLimitations { get; set; }
        public int IsChargesForServicesRendered { get; set; }
        public bool OnRequest { get; set; }

        public string PlanOfSupervision { get; set; }
        public string DurationOfServices { get; set; }
        public string StatementsOfGoals { get; set; }
        public string ObjectivesOfServices { get; set; }
        public string DischargePlans { get; set; }
        public string DescriptionHowTheTasksArePerformed { get; set; }
        public string PertinentDiagnosis { get; set; }

        public bool IsAttachedMedicationForm { get; set; }
        public string Medications { get; set; }

        public string Treatments { get; set; }
        public string EquipmentNeeds { get; set; }
        public string Diet { get; set; }
        public string NutritionalNeeds { get; set; }

        public bool IsPhysiciansOrdersNeeded { get; set; }
        public string PhysicianOrdersDescription { get; set; }

        public string ClientSignature { get; set; }
        public DateTime? ClientSignatureDate { get; set; }

        public string NurseSignature { get; set; }
        public DateTime? NurseSignatureDate { get; set; }

        
        
        [Ignore]
        public string EncryptedReferralID { get; set; }
        [Ignore]
        public string TempClientSignaturePath { get; set; }
        [Ignore]
        public string EncryptedCareFormID { get; set; }

        [ResultColumn]
        public string Str_ServiceRequested { get; set; }
        [ResultColumn]
        public string EmployeeSignaturePath { get; set; }
        [ResultColumn]
        public string ClientName { get; set; }
        [ResultColumn]
        public string RecordID { get; set; }
    }
}
