using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;
using System;

namespace Zarephath.Core.Models.Entity
{
    [TableName("EmployeeChecklists")]
    [PrimaryKey("EmployeeChecklistID")]
    [Sort("EmployeeChecklistID", "DESC")]
    public class EmployeeChecklist : BaseEntity
    {
        public long EmployeeChecklistID { get; set; }
        public long EmployeeID { get; set; }
        public bool DriverLicense { get; set; }
        public bool StateID { get; set; }
        public bool AlienCard { get; set; }
        public bool Passport { get; set; }
        public DateTime? IdentificationDate { get; set; }
        public DateTime? IdentificationCompletionDate { get; set; }
        public bool SSCardCopy { get; set; }
        public DateTime? SSCardDate { get; set; }
        public DateTime? SSCardCompletionDate { get; set; }
        public bool RN { get; set; }
        public bool LPN { get; set; }
        public bool LSW { get; set; }
        public bool CNA { get; set; }
        public bool Other { get; set; }
        public string OtherText { get; set; }
        public DateTime? CompetencyDate { get; set; }
        public DateTime? CompetencyCompletionDate { get; set; }
        public bool EducationContinuingCertificate { get; set; }
        public DateTime? EducationContinuingCertificateDate { get; set; }
        public DateTime? EducationContinuingCertificateCompletionDate { get; set; }
        public bool CurrentResume { get; set; }
        public DateTime? CurrentResumeDate { get; set; }
        public DateTime? CurrentResumeCompletionDate { get; set; }
        public bool FirstAid { get; set; }
        public DateTime? FirstAidDate { get; set; }
        public DateTime? FirstAidCompletionDate { get; set; }
        public bool CPR { get; set; }
        public DateTime? CPRDate { get; set; }
        public DateTime? CPRCompletionDate { get; set; }
        public bool BloodBornePathogens { get; set; }
        public DateTime? BloodBornePathogensDate { get; set; }
        public DateTime? BloodBornePathogensCompletionDate { get; set; }
        public bool DriverAbstract { get; set; }
        public DateTime? DriverAbstractDate { get; set; }
        public DateTime? DriverAbstractCompletionDate { get; set; }
        public bool TBClearance { get; set; }
        public DateTime? TBClearanceDate { get; set; }
        public DateTime? TBClearanceCompletionDate { get; set; }
        public bool HepatitisBVaccine { get; set; }
        public DateTime? HepatitisBVaccineDate { get; set; }
        public DateTime? HepatitisBVaccineCompletionDate { get; set; }
        public bool ProtectiveServicesClearance { get; set; }
        public DateTime? InitialDate { get; set; }
        public DateTime? InitialCompletionDate { get; set; }
        public bool FingerPrinting { get; set; }
        public DateTime? FingerPrintingDate { get; set; }
        public DateTime? FingerPrintingCompletionDate { get; set; }
        public int? YearOne { get; set; }
        public int? YearTwo { get; set; }
        public bool NameCheck { get; set; }
        public DateTime? NameCheckDate { get; set; }
        public DateTime? NameCheckCompletionDate { get; set; }
        public bool CarInsurance { get; set; }
        public DateTime? CarInsuranceDate { get; set; }
        public DateTime? CarInsuranceCompletionDate { get; set; }
        public bool LiabilityInsurance { get; set; }
        public DateTime? LiabilityInsuranceDate { get; set; }
        public DateTime? LiabilityInsuranceCompletionDate { get; set; }
        public bool EmployeeW4Form { get; set; }
        public bool ISCW9Form { get; set; }
        public DateTime? EmployeeFormDate { get; set; }
        public DateTime? EmployeeFormCompletionDate { get; set; }
        public bool ISCGELicense { get; set; }
        public DateTime? ISCGELicenseDate { get; set; }
        public DateTime? ISCGELicenseCompletionDate { get; set; }
        public bool DOHLicense { get; set; }
        public DateTime? DOHLicenseDate { get; set; }
        public DateTime? DOHLicenseCompletionDate { get; set; }
        public bool IndependentAgreement { get; set; }
        public bool EmployeeAgreement { get; set; }
        public DateTime? AgreementDate { get; set; }
        public DateTime? AgreementCompletionDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? SubmittedDate { get; set; }
    }
}
