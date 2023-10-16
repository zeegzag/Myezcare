using System;
using System.Collections.Generic;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class Dmas99Model
    {
        public long Dmas99ID { get; set; }
        public string Services { get; set; }
        public string AssessmentDate { get; set; }
        public string Visit { get; set; }
        public string ReciName { get; set; }
        public string DOB { get; set; }
        public string MedicaidID { get; set; }
        public string StartOfCare { get; set; }
        public string ReciCurrAddress1 { get; set; }
        public string ReciCurrAddress2{ get; set; }
        public string AgencyName { get; set; }
        public int ProviderID { get; set; }
        public string ReciPhone { get; set; }
        public string ReciSSN { get; set; }

        public string BathNNH { get; set; }
        public string BathMHO { get; set; }
        public string BathHuSup { get; set; }
        public string BathHuPhy { get; set; }
        public string BathMHSup { get; set; }
        public string BathMHPhy { get; set; }
        public string BathAPO { get; set; }
        public string BathINP { get; set; }

        public string DresNNH { get; set; }
        public string DresMHO { get; set; }
        public string DresHuSup { get; set; }
        public string DresHuPhy { get; set; }
        public string DresMHSup { get; set; }
        public string DresMHPhy { get; set; }
        public string DresAPO { get; set; }
        public string DresINP { get; set; }

        public string ToilNNH { get; set; }
        public string ToilMHO { get; set; }
        public string ToilHuSup { get; set; }
        public string ToilHuPhy { get; set; }
        public string ToilMHSup { get; set; }
        public string ToilMHPhy { get; set; }
        public string ToilAPO { get; set; }
        public string ToilINP { get; set; }

        public string TranNNH { get; set; }
        public string TranMHO { get; set; }
        public string TranHuSup { get; set; }
        public string TranHuPhy { get; set; }
        public string TranMHSup { get; set; }
        public string TranMHPhy { get; set; }
        public string TranAPO { get; set; }
        public string TranINP { get; set; }

        public string EatNNH { get; set; }
        public string EatMHO { get; set; }
        public string EatHuSup { get; set; }
        public string EatHuPhy { get; set; }
        public string EatMHSup { get; set; }
        public string EatMHPhy { get; set; }
        public string EatAPO { get; set; }
        public string EatINP { get; set; }

        public string BowCon { get; set; }
        public string BowWeek { get; set; }
        public string BowSelf { get; set; }
        public string BowInConWeek { get; set; }
        public string BowExDe { get; set; }
        public string BowInCa { get; set; }
        public string BowOsNo { get; set; }

        public string BladCon { get; set; }
        public string BladWeek { get; set; }
        public string BladSelf { get; set; }
        public string BladInConWeek { get; set; }
        public string BladExDe { get; set; }
        public string BladInCa { get; set; }
        public string BladOsNo { get; set; }

        public string MobNNH { get; set; }
        public string MobMHO { get; set; }
        public string MobHuSup { get; set; }
        public string MobHuPhy { get; set; }
        public string MobMHSup { get; set; }
        public string MobMHPhy { get; set; }
        public string MobCMA { get; set; }
        public string MobCDN { get; set; }

        public string Oriented { get; set; }
        public string OrieDsSt { get; set; }
        public string OrieDsAt { get; set; }
        public string OrieDaSt { get; set; }
        public string OrieDaAt { get; set; }
        public string OrieSeCo { get; set; }
        public string SpheresAffected { get; set; }
        public string SourceInfo { get; set; }

        public string Appropriate { get; set; }
        public string BehWpLe { get; set; }
        public string BehWpGr { get; set; }
        public string BehAdLe { get; set; }
        public string BehAdGr { get; set; }
        public string BehSeCo { get; set; }
        public string DesInappropriateBehavior { get; set; }
        public string SourceInfo2 { get; set; }

        public bool ChkWithinNormalLimit { get; set; }
        public bool ChkWithoutAssistance { get; set; }
        public bool ChkLimitedMotion { get; set; }
        public bool ChkMonitoredByLayPerson { get; set; }
        public bool ChkInstabilityUncorrected { get; set; }
        public bool ChkMonitoredByNursing { get; set; }


        public string Diagnoses { get; set; }
        public string Medications { get; set; }
        public string CurrHealthStatus { get; set; }
        public string CurrMedicalNeeds { get; set; }
        public string TherapiesMedProc { get; set; }
        public string HospitalizationDate { get; set; }
        public string Reason { get; set; }

        public bool ChktyAgencyPersonalCare { get; set; }
        public string tyAgencyPersonalCare { get; set; }
        public bool ChkCDPersonalCare { get; set; }
        public string CDPersonalCare { get; set; }
        public bool ChkAgencyRespite { get; set; }
        public string AgencyRespite { get; set; }
        public bool ChkCDRespite { get; set; }
        public string CDRespite { get; set; }
        public bool ChkADHC { get; set; }
        public string ADHC { get; set; }
        public bool ChkPERS { get; set; }
        public string PERS { get; set; }
        public string WhWaSerIsThPatPayToBeDed { get; set; }
        public string TotalWeeklyHours { get; set; }
        public string DaysPerWeek { get; set; }

        public string SpecHoursAidIsReciHome { get; set; }
        public string OtMedNonMedFundSerRcvd1 { get; set; }
        public string OtMedNonMedFundSerRcvd2 { get; set; }
        public string WhoIsThePrimaryCareGiver { get; set; }
        public string NoRelationshipReci { get; set; }
        public string PrimCareGvrLiveWithReci { get; set; }
        public string PCGProvidesToTheReci1 { get; set; }
        public string PCGProvidesToTheReci2 { get; set; }
        public string RdBtHowOfDoesPCGSeeTheReci { get; set; }
        public string HowOfDoesPCGSeeTheReci { get; set; }
        public string WhOtThReciAuthorzdToSignAidRecord { get; set; }
        public string IsReciReceivingPERS { get; set; }
        public string HeRecevingMedMonitor { get; set; }

        public string IsReci14YrsOfAgeOrOld { get; set; }
        public string IsPERSAdequateMeetReciNeed { get; set; }
        public string TimeWhenPhoneServiceDiscontd { get; set; }
        public string ReciPleasedWithServicePERSPrvdr { get; set; }
        public string PersonDirectingCare { get; set; }
        public string DirectingCareRelationshipToReci { get; set; }
        public string PersonProvidingCare { get; set; }
        public string ProvidingCareRelationshipToReci { get; set; }
        public string ReciNeedOfPERSAtAllTimesToMaSafely { get; set; }
        public string ReciNeedOfSupVisionAtAllTimesToMaSafely { get; set; }
        public string ReciReceivingSupVision { get; set; }
        public string IfYesHasHeBeenInformedOfPERS { get; set; }
        public string DatesOfRNSupervisory { get; set; }
        public string ReciAgreeToFreqOfVisit { get; set; }
        public string FreqOfSupervisoryVisit  { get; set; }
        public string SupVisitForPersonalCare { get; set; }
        public string SupVisitForRespiteCare { get; set; }
        public string DoesAideDocAccurateCareProvided { get; set; }
        public string DoesServicePlanReflectNeedOfReci { get; set; }
        public string PleaseDescribeFollowUp1 { get; set; }
        public string PleaseDescribeFollowUp2 { get; set; }

        public string NumOfDayNoService6Month { get; set; }
        public string RegularAides { get; set; }
        public string SubAides { get; set; }
        public string ReciOrCareGiverHadPrblmWithCareProvided { get; set; }
        public string FollowUpTaken1 { get; set; }
        public string FollowUpTaken2 { get; set; }
        public string ReciSatisfyWithServiceReceiveByProAgency { get; set; }
        public string ProAgencyFollowUpTaken1 { get; set; }
        public string ProAgencyFollowUpTaken2 { get; set; }
        public string DateOfMostRecentDMAS225 { get; set; }
        public int PatientPayAmount { get; set; }
        public string AidePresentDuringVisit { get; set; }
        public string NameOfAide { get; set; }
        public string SfNursingNotes1 { get; set; }
        public string SfNursingNotes2 { get; set; }
        public string SfNursingNotes3 { get; set; }
        public string PersonalCareAideRelatedReci { get; set; }
        public string Sign { get; set; }
        public string SignDate { get; set; }
        public string RnSign { get; set; }
        public string RnSignDate { get; set; }

        public string JsonData { get; set; }
        
        public long EmployeeID { get; set; }
        public long ReferralID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
        public string EncryptedReferralID { get; set; }

    }

    public class Dmas99ListPage
    {
        public Dmas99ListPage()
        {
            Dmas99FormList = new List<Dmas99ModelList>();
        }
        public List<Dmas99ModelList> Dmas99FormList { get; set; }
    }


    public class Dmas99ModelList
    {
        public long Dmas99ID { get; set; }
        public string EmployeeName { get; set; }
        public string JsonData { get; set; }
        public string CreatedDate { get; set; }

    }



    public class Dmas99Models
    {
        public long Dmas99ID { get; set; }
        public string EmployeeName { get; set; }
        public string JsonData { get; set; }
        public string CreatedDate { get; set; }
        public string EncryptedReferralID { get; set; }
    }

    public class Dmas99CloneModel
    {
        public long Dmas99ID { get; set; }
        public string EmployeeName { get; set; }
        public string JsonData { get; set; }
        public string CreatedDate { get; set; }
        public string EncryptedReferralID { get; set; }

    }
    public class DmasReferrals
    {
        public long ReferralID { get; set; }
        public string PatientName { get; set; }
        public string Dob { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string Phone1 { get; set; }
        public string NPI { get; set; }
        public string NickName { get; set; }
        public string EncryptedReferralID { get; set; }
    }

    public class GetDmas99Model
    {
        public GetDmas99Model()
        {
            Dmas99Model = new Dmas99Model();
            OrganizationalModel = new OrganizationalModel();
            DmasPayorList = new List<DmasPayorList>();
            DmasReferrals = new DmasReferrals();
        }
        public Dmas99Model Dmas99Model { get; set; }
        public OrganizationalModel OrganizationalModel { get; set; }
        public List<DmasPayorList> DmasPayorList { get; set; }
        public DmasReferrals DmasReferrals { get; set; }

    }

}
