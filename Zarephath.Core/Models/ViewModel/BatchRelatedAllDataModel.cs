using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;
using System.Linq;

namespace Zarephath.Core.Models.ViewModel
{
    public class ParentBatchRelatedAllDataModel
    {
        public PayorEdi837Setting PayorEdi837Setting { get; set; }
        public List<BatchRelatedAllDataModel> BatchRelatedAllDataModel { get; set; }

        public string FileName{ get; set; }
    }




    public class ParentBatchRelatedAllDataModel_Temporary
    {
        public PayorEdi837Setting PayorEdi837Setting { get; set; }
        public List<BatchRelatedAllDataModel_Temporary> BatchRelatedAllDataModel_Temporary { get; set; }

    }

    public class BatchRelatedAllDataModel_Temporary 
    {
        public string ErrorMessage { get; set; }
        public int ErrorCount { get; set; }
        public long NoteID { get; set; }
        public long ReferralID { get; set; }

        public BatchRelatedAllDataModel BatchRelatedDataModel { get; set; }
    }

    //public class BatchRelatedAllDataModel
    //{

    //    // Batch
    //    public long BatchID { get; set; }
    //    public long BatchTypeID { get; set; }
    //    public DateTime StartDate { get; set; }
    //    public DateTime EndDate { get; set; }
    //    public bool IsDeleted { get; set; }
    //    public bool IsSent { get; set; }
    //    public DateTime IsSentDate { get; set; }


    //    // Batch Notes
    //    public long BatchNoteID { get; set; }
    //    public long NoteID { get; set; }
    //    public long BatchNoteStatusID { get; set; }
    //    public string Reason { get; set; }
    //    public long ReasonCode { get; set; }


    //    // Notes

    //    // Referral
    //    public long ReferralID { get; set; }
    //    public string AHCCCSID { get; set; }
    //    public string MedicalRecordNumber { get; set; }
    //    public string PolicyNumber { get; set; }

    //    public string CISNumber { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string Dob { get; set; }
    //    public string Gender { get; set; }
    //    public string SubscriberID { get; set; }
    //    public string ClaimSubmitterIdentifier { get; set; }
    //    public string PatientAccountNumber { get; set; }
    //    public string Address { get; set; }
    //    public string City { get; set; }
    //    public string State { get; set; }
    //    public string ZipCode { get; set; }
    //    public string AdmissionDate { get; set; } // Created Date Of Patient


    //    //Referral billing setting
    //    public string AuthorizationCode { get; set; }
    //    public string POS_CMS1500 { get; set; }
    //    public int SpecialProgramCode_CMS1500 { get; set; }
    //    public string POS_UB04 { get; set; }
    //    public string AdmissionTypeCode_UB04 { get; set; }
    //    public string AdmissionSourceCode_UB04 { get; set; }
    //    public string PatientStatusCode_UB04 { get; set; }



    //    // DX Cods
    //    public string ContinuedDX { get; set; }


    //    //Physician
    //    public long PhysicianID { get; set; }
    //    public string PhysicianNPINumber { get; set; }
    //    public string PhysicianFirstName { get; set; }
    //    public string PhysicianLastName { get; set; }



    //    public DateTime ServiceDate { get; set; }
    //    public string ServiceDateSpan { get; set; }


    //    // Service Codes
    //    public long? ServiceCodeID { get; set; }
    //    public string ServiceCode { get; set; }
    //    public string ServiceName { get; set; }
    //    public string Description { get; set; }
    //    public int MaxUnit { get; set; }
    //    public int DailyUnitLimit { get; set; }
    //    public int UnitType { get; set; }
    //    public float PerUnitQuantity { get; set; }
    //    public int ServiceCodeType { get; set; }
    //    public bool IsBillable { get; set; }

    //    public long? ModifierID { get; set; }
    //    public string ModifierName { get; set; }

    //    public long PosID { get; set; }
    //    public string PosName { get; set; }


    //    public float Rate { get; set; }
    //    public float CalculatedUnit { get; set; }
    //    public float CalculatedAmount { get; set; }
    //    public string NoteDetails { get; set; }
    //    public string Assessment { get; set; }
    //    public string ActionPlan { get; set; }
    //    public bool MarkAsComplete { get; set; }
    //    public string POSDetail { get; set; }

    //    //BillingProviderModel
    //    public long BillingProviderID { get; set; }
    //    public string BillingProviderName { get; set; }
    //    public string BillingProviderFirstName { get; set; }
    //    public string BillingProviderAddress { get; set; }
    //    public string BillingProviderCity { get; set; }
    //    public string BillingProviderState { get; set; }
    //    public string BillingProviderZipcode { get; set; }
    //    public string BillingProviderEIN { get; set; }
    //    public string BillingProviderNPI { get; set; }
    //    public int? BillingProviderGSA { get; set; }

    //    //RenderingProviderModel
    //    public long RenderingProviderID { get; set; }
    //    public string RenderingProviderFirstName { get; set; }
    //    public string RenderingProviderName { get; set; }
    //    public string RenderingProviderAddress { get; set; }
    //    public string RenderingProviderCity { get; set; }
    //    public string RenderingProviderState { get; set; }
    //    public string RenderingProviderZipcode { get; set; }
    //    public string RenderingProviderEIN { get; set; }
    //    public string RenderingProviderNPI { get; set; }
    //    public int? RenderingProviderGSA { get; set; }


    //    // Payor Informations
    //    public long PayorID { get; set; }
    //    public string PayorName { get; set; }
    //    public string PayorAddress { get; set; }
    //    public string PayorIdentificationNumber { get; set; }
    //    public string PayorCity { get; set; }
    //    public string PayorState { get; set; }
    //    public string PayorZipcode { get; set; }



    //    public string Submitted_ClaimSubmitterIdentifier { get; set; }
    //    public string Submitted_ClaimAdjustmentTypeID { get; set; }
    //    public string Original_ClaimSubmitterIdentifier { get; set; }
    //    public string Original_PayerClaimControlNumber { get; set; }
    //    public string ClaimAdjustmentReason { get; set; }

    //    public int? BillingUnitLimit { get; set; }
    //    public string RevenueCode { get; set; }

    //    public bool IsUseInBilling { get; set; }

    //    public string RandomGroupID { get; set; }
    //    public string GroupIDForMileServices { get; set; }
    //    public string StrBathNoteID { get; set; }
    //}



    public class EDIFileSearchModel
    {
        public long BatchTypeID { get; set; }
        public long PayorID { get; set; }
        public long ReferralID { get; set; }
        public long BatchID { get; set; }
        public string FileType { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ServiceCodeIDs { get; set; }
        public string ClientName { get; set; }
        public string StrNoteIds { get; set; }

        
    }

    public class BatchRelatedAllDataModel
    {

        // Batch
        public long BatchID { get; set; }
        public long BatchTypeID { get; set; }

        // Batch Notes
        public long BatchNoteID { get; set; }
        public long NoteID { get; set; }

        // Referral
        public long ReferralID { get; set; }
        public string AHCCCSID { get; set; }
        public string MedicalRecordNumber { get; set; }
        public string PolicyNumber { get; set; }

        public string CISNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Dob { get; set; }

        public string Gender { get; set; }
        public string SubscriberID { get; set; }
        public string ClaimSubmitterIdentifier { get; set; }
        public string PatientAccountNumber { get; set; }
        public long ContactID { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string AdmissionDate { get; set; } // Created Date Of Patient

        //Referral billing setting
        public long ReferralBillingAuthorizationID { get; set; }
        public string AuthorizationCode { get; set; }
        public int POS_CMS1500_ID { get; set; }
        public string POS_CMS1500 { get; set; }
        public int SpecialProgramCode_CMS1500 { get; set; }
        public string POS_UB04 { get; set; }
        public string AdmissionTypeCode_UB04 { get; set; }
        public string AdmissionSourceCode_UB04 { get; set; }
        public string PatientStatusCode_UB04 { get; set; }



        // DX Cods
        public string ContinuedDX { get; set; }


        //Physician
        public bool IsCaseManagement { get; set; }
        public bool IsHomeCare { get; set; }

        public bool IsDayCare { get; set; }
        public long PhysicianID { get; set; }
        public string PhysicianNPINumber { get; set; }
        public string PhysicianFirstName { get; set; }
        public string PhysicianLastName { get; set; }
        public string PhysicianFullName { get; set; }


        public DateTime ServiceDate { get; set; }
        public string ServiceDateSpan { get; set; }


        // Service Codes
        public long? ServiceCodeID { get; set; }
        public string ServiceCode { get; set; }


        public long? ModifierID { get; set; }
        public string ModifierIDs { get; set; } //24a
        public string ModifierName { get; set; }

        public long PosID { get; set; }
        public string PosName { get; set; }


        public float Rate { get; set; }
        public float CalculatedUnit { get; set; }
        public float CalculatedAmount { get; set; }

        public float AMT01_ServiceLineAllowedAmount_AllowedAmount { get; set; }
        public float SVC03_LineItemProviderPaymentAmoun_PaidAmount { get; set; }


        public float MPP_AdjustmentAmount { get; set; }


        //BillingProviderModel
        public long BillingProviderID { get; set; }
        public string BillingProviderName { get; set; }
        public string BillingProviderFirstName { get; set; }
        public string BillingProviderAddress { get; set; }
        public string BillingProviderCity { get; set; }
        public string BillingProviderState { get; set; }
        public string BillingProviderZipcode { get; set; }
        public string BillingProviderEIN { get; set; }
        public string BillingProviderNPI { get; set; }
        public int? BillingProviderGSA { get; set; }

        //RenderingProviderModel
        public long RenderingProviderID { get; set; }
        public string RenderingProviderFirstName { get; set; }
        public string RenderingProviderName { get; set; }
        public string RenderingProviderAddress { get; set; }
        public string RenderingProviderCity { get; set; }
        public string RenderingProviderState { get; set; }
        public string RenderingProviderZipcode { get; set; }
        public string RenderingProviderEIN { get; set; }
        public string RenderingProviderNPI { get; set; }
        public string RenderingProvider_TaxonomyCode { get; set; }
        public int? RenderingProviderGSA { get; set; }



        public string SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName { get; set; }
        public string SupervisingProvidername2420DLoop_NM104_NameFirst { get; set; }
        public string SupervisingProvidername2420DLoop_REF02_ReferenceId { get; set; }
        





        // Payor Informations
        public long PayorID { get; set; }
        public string PayorName { get; set; }
        public string PayorAddress { get; set; }
        public string PayorIdentificationNumber { get; set; }
        public string PayorCity { get; set; }
        public string PayorState { get; set; }
        public string PayorZipcode { get; set; }

        public string PayorBillingType { get; set; }



        public string Submitted_ClaimSubmitterIdentifier { get; set; }
        public string Submitted_ClaimAdjustmentTypeID { get; set; }
        public string Original_ClaimSubmitterIdentifier { get; set; }
        public string Original_PayerClaimControlNumber { get; set; }
        public string ClaimAdjustmentReason { get; set; }

        public int? BillingUnitLimit { get; set; }
        public string RevenueCode { get; set; }

        public bool IsUseInBilling { get; set; }

        public string RandomGroupID { get; set; }
        public string GroupIDForMileServices { get; set; }
        public string StrBathNoteID { get; set; }

        //new property add for generate CMS1500 / UB04
        public string PatientDOB { get; set; }
        public string DobDD { get; set; }
        public string DobMM { get; set; }
        public string DobYYYY { get; set; }
        public string StrAdmissionDate { get; set; } // Created Date Of Patient

        public string PatientName { get; set; }
        public string PayorShortName { get; set; }

        public string CareType { get; set; }

        public string Subscriber_SBR02_RelationshipCode { get; set; }

        public string Phone1 { get; set; }
        public string PatientAddress
        {
            get
            {
                string fulladdress = string.Format("{0} {1}, {2} {3}", Address, City, State, ZipCode);
                return fulladdress;
            }
        }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string ServiceDescription
        {
            get
            {
                string StrStartTime = "0001";
                string StrEndTime = "2400";
                if (StartTime.HasValue)
                    StrStartTime = StartTime.Value.ToString("HHmm");
                if (EndTime.HasValue)
                    StrEndTime = EndTime.Value.ToString("HHmm");

                return String.Format("{0}-{1}", StrStartTime, StrEndTime);

            }
        }

    }

    #region EDI Related Model Grouping

    public class GroupedModelFor837
    {
        public GroupedModelFor837()
        {
            BillingProviders = new List<BillingProviderModel>();
        }

        public List<BillingProviderModel> BillingProviders { get; set; }
    }

    public class BillingProviderModel
    {
        public BillingProviderModel()
        {
            Subscribers = new List<SubscriberModel>();
            Subscribers_Updated = new List<SubscriberModel_Updated>();
        }
        public long BillingProviderID { get; set; }
        public string BillingProviderName { get; set; }
        public string BillingProviderFirstName { get; set; }
        public string BillingProviderAddress { get; set; }
        public string BillingProviderCity { get; set; }
        public string BillingProviderState { get; set; }
        public string BillingProviderZipcode { get; set; }
        public string BillingProviderEIN { get; set; }
        public string BillingProviderNPI { get; set; }
        public int? BillingProviderGSA { get; set; }

        public List<SubscriberModel> Subscribers { get; set; }
        public List<SubscriberModel_Updated> Subscribers_Updated { get; set; }
    }


    public class SubscriberModel_Updated
    {
        public SubscriberModel_Updated()
        {
            //Claims = new List<ClaimModel>();
        }


        #region Subscriber Details

        public long ReferralID { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public string PolicyNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dob { get; set; }
        public string Gender { get; set; }
        public string SubscriberID { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        #endregion Subscriber Name

        #region Payer Name

        public string PayorIdentificationNumber { get; set; }
        public string PayorName { get; set; }
        public string PayorAddress { get; set; }
        public string PayorCity { get; set; }
        public string PayorState { get; set; }
        public string PayorZipcode { get; set; }
        public string PayorBillingType { get; set; }
        public long PayorID { get; set; }
        public long BatchID{ get; set; }

        #endregion Payer Name


        public ClaimModel Claim { get; set; }
    }

    public class SubscriberModel
    {
        public SubscriberModel()
        {
            Claims = new List<ClaimModel>();
        }


        #region Subscriber Details

        public long ReferralID { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public string PolicyNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dob { get; set; }
        public string Gender { get; set; }
        public string SubscriberID { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        #endregion Subscriber Name

        #region Payer Name

        public string PayorIdentificationNumber { get; set; }
        public string PayorName { get; set; }
        public string PayorAddress { get; set; }
        public string PayorCity { get; set; }
        public string PayorState { get; set; }
        public string PayorZipcode { get; set; }
        public string PayorBillingType { get; set; }
        public long PayorID { get; set; }

        #endregion Payer Name


        public long TotalClaimCount { get; set; }

        public float TotalAmount { get; set; }

        public float TotalAllowedAmount { get; set; }
        public float TotalPaidAmount { get; set; }
        public float TotalMPP_AdjustmentAmount { get; set; }
        public List<ClaimModel> Claims { get; set; }
    }

    public class ClaimModel
    {
        public ClaimModel()
        {
            ServiceLines = new List<ServiceLineModel>();
            ClaimMessages = new List<ClaimMessageModel>();
        }


        //ClaimSubmitterIdentifier==PatientAccountNumber
        public long BatchId { get; set; }
        public long ReferralID { get; set; }
        public string ClaimSubmitterIdentifier { get; set; }
        public string MedicalRecordNumber { get; set; } // REF02
        public string Prior_ReferenceIdentification { get; set; } // Prior REF02
        //public string PatientAccountNumber { get; set; }
        public float CalculatedAmount { get; set; }

        public float TotalAmount { get; set; }
        
        public float TotalAllowedAmount { get; set; }
        public float TotalPaidAmount { get; set; }

        public float TotalMPP_AdjustmentAmount { get; set; }

        

        public string StrNoteIds { get; set; }
        public long PosID { get; set; }
        public string PosName { get; set; }
        //public string POSDetail { get; set; }
        public string ContinuedDX { get; set; }
        public long? ModifierID { get; set; }
        public string ModifierName { get; set; }

        public string Submitted_ClaimAdjustmentTypeID { get; set; }
        public string Original_PayerClaimControlNumber { get; set; }
        public string ClaimAdjustmentReason { get; set; }

        public int SpecialProgramCode { get; set; }
        //public DateTime ServiceDate { get; set; }
        //// Service Codes
        //public long? ServiceCodeID { get; set; }
        //public string ServiceCode { get; set; }
        //public string ServiceName { get; set; }
        //public string Description { get; set; }
        //public int MaxUnit { get; set; }
        //public int DailyUnitLimit { get; set; }
        //public int UnitType { get; set; }
        //public float PerUnitQuantity { get; set; }
        //public int ServiceCodeType { get; set; }
        //public bool IsBillable { get; set; }
        //public float Rate { get; set; }
        //public float CalculatedUnit { get; set; }
        //public string NoteDetails { get; set; }
        //public string Assessment { get; set; }
        //public string ActionPlan { get; set; }
        //public bool MarkAsComplete { get; set; }

        #region Provider, Service Facility Information

        public string ClaimUniqueTraceID{ get; set; }

        public bool IsCaseManagement { get; set; }
        public bool IsHomeCare { get; set; }
        public bool IsDayCare { get; set; }
        public long RenderingProviderID { get; set; }
        public string RenderingProviderFirstName { get; set; }
        public string RenderingProviderName { get; set; }
        public string RenderingProviderEIN { get; set; }
        public string RenderingProviderNPI { get; set; }
        public int? RenderingProviderGSA { get; set; }

        public string RenderingProviderAddress { get; set; }
        public string RenderingProviderCity { get; set; }
        public string RenderingProviderState { get; set; }
        public string RenderingProviderZipcode { get; set; }

        public string RenderingProvider_TaxonomyCode { get; set; }

        public string SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName { get; set; }
        public string SupervisingProvidername2420DLoop_NM104_NameFirst { get; set; }
        public string SupervisingProvidername2420DLoop_REF02_ReferenceId { get; set; }

        public DateTime ServiceDate { get; set; }
        public string GroupIDForMileServices { get; set; }

        #endregion

        public List<ServiceLineModel> ServiceLines { get; set; }

        public List<ClaimMessageModel> ClaimMessages { get; set; }
        public string ClaimLevelStatus { get; set; }


        public string PhysicianNPINumber { get; set; }
        public string PhysicianFirstName { get; set; }
        public string PhysicianLastName { get; set; }

        public string POS_CMS1500 { get; set; }
        public string AdmissionTypeCode_UB04 { get; set; }
        public string AdmissionSourceCode_UB04 { get; set; }
        public string PatientStatusCode_UB04 { get; set; }

        public string AdmissionDate { get; set; }
    }

    public class ServiceLineModel
    {
        public string ModifierName { get; set; }
        public string ServiceCode { get; set; }
        public float CalculatedAmount { get; set; }
        public float AMT01_ServiceLineAllowedAmount_AllowedAmount { get; set; }
        public float SVC03_LineItemProviderPaymentAmoun_PaidAmount { get; set; }

        public float MPP_AdjustmentAmount { get; set; }

        public float CalculatedUnit { get; set; }
        public DateTime ServiceDate { get; set; }
        public string ServiceDateSpan { get; set; }
        public long PosID { get; set; }
        public long RenderingProviderID { get; set; }
        public string StrBathNoteID { get; set; }
        public long BatchID { get; set; }
        public long NoteID { get; set; }
        public long BatchNoteID { get; set; }
        public string FacilityCode { get; set; }
        public string RevenueCode { get; set; }
        public string ServiceDescription { get; set; }

    }



    #region Group By Models

    public class BillingGroupClass
    {
        public BillingProviderModel BillingProviderModel { get; set; }
        public List<BatchRelatedAllDataModel> ListModel { get; set; }
    }

    public class SubscriberPayorGroupClass
    {
        public SubscriberModel SubscriberModel { get; set; }
        public List<BatchRelatedAllDataModel> ListModel { get; set; }
    }

    public class ClaimGroupClass
    {
        public ClaimModel ClaimModel { get; set; }
        public List<BatchRelatedAllDataModel> ListModel { get; set; }
    }


    public class ServiceLineGroupClass
    {
        public ServiceLineModel ServiceLineModel { get; set; }
        public List<BatchRelatedAllDataModel> ListModel { get; set; }
    }

    #endregion

    #endregion

    #region 270/271

    #region Generate 270 Related Models
    public class InformationLevelModel
    {
        public string HeirarchicalLevelCode { get; set; } // HL03
        public string EntityIdentifierCode { get; set; } // NM101
        public string EntityTypeQualifier { get; set; } // NM102
        public string NameLastOrOrganizationName { get; set; } // NM103
        public string IdCodeQualifier { get; set; } // NM108
        public string IdCodeQualifierEnum { get; set; } // NM109
    }

    public class ClientDetailsfor270Model
    {

        public string HeirarchicalLevelCode { get; set; }// HL03 

        public string TRN01_TraceTypeCode { get; set; } // TRN01
        public string TRN02_ReferenceIdentification02 { get; set; } // TRN02
        public string TRN03_CompanyIdentifier { get; set; } // TRN03
        public string TRN04_ReferenceIdentification04 { get; set; } // TRN04


        #region Subscriber

        public string SubmitterEntityIdentifierCode { get; set; } // NM101
        public string SubmitterEntityTypeQualifier { get; set; } // NM102
        public string LastName { get; set; } // NM103
        public string FirstName { get; set; } // NM104
        public string SubmitterIdCodeQualifier { get; set; } // NM108
        public string SubmitterIdCodeQualifierEnum { get; set; } // NM109

        public string SubmitterDateTimePeriodFormatQualifier { get; set; } // DMG01
        public string Dob { get; set; } // DMG02
        public string Gender { get; set; } // DMG03


        public string SubmitterDTPQualifier { get; set; } // DTP01
        public string SubmitterDTPFormatQualifier { get; set; } // DTP02
        public string SubmitterDTPDateTimePeriod { get; set; } // DTP03


        public string SubmitterEligibility01 { get; set; } // EQ01

        #endregion Subscriber


    }

    public class TransactionHeaderFor270Model
    {
        public string ISA01_AuthorizationInformationQualifier { get; set; }
        public string ISA02_AuthorizationInformation { get; set; } // NM101
        public string ISA03_SecurityInformationQualifier { get; set; } // NM102
        public string ISA04_SecurityInformation { get; set; } // NM103
        public string ISA05_InterchangeSenderIdQualifier { get; set; } // NM108
        public string ISA06_InterchangeSenderId { get; set; } // NM109

        public string ISA07_InterchangeReceiverIdQualifier { get; set; }
        public string ISA08_InterchangeReceiverId { get; set; }
        public string ISA09_InterchangeDate { get; set; }
        public string ISA10_InterchangeTime { get; set; }
        public string ISA11_RepetitionSeparator { get; set; }
        public string ISA12_InterchangeControlVersionNumber { get; set; }
        public long ISA13_InterchangeControlNumber { get; set; }
        public DateTime? ISA13_UpdatedDate { get; set; }
        public string ISA14_AcknowledgementRequired { get; set; }
        public string ISA15_UsageIndicator { get; set; }
        public string ISA16_ComponentElementSeparator { get; set; }
        public string SegmentTerminator { get; set; }
        public string ElementSeparator { get; set; }


        public string GS01_FunctionalIdentifierCode { get; set; }
        public string GS02_ApplicationSenderCode { get; set; }
        public string GS03_ApplicationReceiverCode { get; set; }
        public string GS04_Date { get; set; }
        public string GS05_Time { get; set; }
        public string GS06_GroupControlNumber { get; set; }

        public string GS07_ResponsibleAgencyCode { get; set; }
        public string GS08_VersionOrReleaseOrNo { get; set; }


        public string ST01_TransactionSetIdentifier { get; set; }
        public string ST02_TransactionSetControlNumber { get; set; }
        public string ST03_ImplementationConventionReference { get; set; }


        public string BHT01_HierarchicalStructureCode { get; set; }
        public string BHT02_TransactionSetPurposeCode { get; set; }
        public string BHT04_Date { get; set; }
        public string BHT05_Time { get; set; }
        public string BHT06_TransactionTypeCode { get; set; }

    }


    public class Parent270DataModel
    {
        public PayorEdi270Setting PayorEdi270Setting { get; set; }
        public InformationLevelModel InformationSource { get; set; }
        public InformationLevelModel InformationReceiver { get; set; }
        public List<ClientDetailsfor270Model> ListClientDetailsfor270Model { get; set; }

        public AddProcess270Model AddProcess270Model { get; set; }
    }
    #endregion

    #region 270 Page Related Models
    public class ListEdi270FileLogModel : Edi270271File
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Payors { get; set; }
        public string ReferralStatuses { get; set; }

        public int Count { get; set; }
        public string StrDisplayName { get; set; }
        //public string StrDisplayName { get { return Common.GetGeneralNameFormat(FirstName, LastName); } }
        public string StrFileSize { get { return Common.GetFileSize(Convert.ToDecimal(FileSize), Constants.SizeIn_KB); } }
        [Ignore]
        public string AWSSignedFilePath
        {
            get
            {
                AmazonFileUpload az = new AmazonFileUpload();
                return az.GetPreSignedUrl(ConfigSettings.ZarephathBucket, FilePath);
            }
        }
    }
    public class ListEdi271FileLogModel : Edi270271File
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Count { get; set; }
        public string StrDisplayName { get; set; }
       // public string StrDisplayName { get { return Common.GetGeneralNameFormat(FirstName, LastName); } }
        public string StrFileSize { get { return Common.GetFileSize(Convert.ToDecimal(FileSize), Constants.SizeIn_KB); } }


        public string AWSSignedFilePath
        {
            get
            {
                AmazonFileUpload az = new AmazonFileUpload();
                return az.GetPreSignedUrl(ConfigSettings.ZarephathBucket, FilePath);
            }
        }

        public string AWSReadableFilePath
        {
            get
            {
                AmazonFileUpload az = new AmazonFileUpload();
                return az.GetPreSignedUrl(ConfigSettings.ZarephathBucket, ReadableFilePath);
            }
        }
    }


    public class ListEdi270FileLogModel01 : Edi270271File
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Payors { get; set; }
        public string ReferralStatuses { get; set; }

        public int Count { get; set; }
       // public string StrDisplayName { get { return Common.GetGeneralNameFormat(FirstName, LastName); } }
        public string StrDisplayName { get; set; }
        public string StrFileSize { get { return Common.GetFileSize(Convert.ToDecimal(FileSize), Constants.SizeIn_KB); } }

        [Ignore]
        public string AWSSignedFilePath
        {
            get
            {
                return FilePath;
                //AmazonFileUpload az = new AmazonFileUpload();
                //return az.GetPreSignedUrl(ConfigSettings.ZarephathBucket, FilePath);
            }
        }
    }
    public class ListEdi271FileLogModel01 : Edi270271File
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Count { get; set; }
       // public string StrDisplayName { get { return Common.GetGeneralNameFormat(FirstName, LastName); } }
        public string StrDisplayName { get; set; }
        public string StrFileSize { get { return Common.GetFileSize(Convert.ToDecimal(FileSize), Constants.SizeIn_KB); } }


        public string AWSSignedFilePath
        {
            get
            {
                return FilePath;
                //AmazonFileUpload az = new AmazonFileUpload();
                //return az.GetPreSignedUrl(ConfigSettings.ZarephathBucket, FilePath);
            }
        }

        public string AWSReadableFilePath
        {
            get
            {
                return ReadableFilePath;
                //AmazonFileUpload az = new AmazonFileUpload();
                //return az.GetPreSignedUrl(ConfigSettings.ZarephathBucket, ReadableFilePath);
            }
        }
    }

    #endregion


    public class SearchProcess271ListPage
    {
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string Comment { get; set; }
        public long PayorID { get; set; }
        public string ServiceID { get; set; }
        public string Name { get; set; }
        public DateTime? EligibilityCheckDate { get; set; }
        public int Upload271FileProcessStatus { get; set; }
        public string ListOfIdsInCSV { get; set; }
        public int IsDeleted { get; set; }

    }

    public class AddProcess270Model
    {
        public string Name { get; set; }
        public string PayorIDs { get; set; }
        public string ServiceIDs { get; set; }
        public DateTime EligibilityCheckDate
        {
            get { return DateTime.Now; }
        }

        public List<string> ReferralStatusIDs { get; set; }
    }
    public class SearchProcess270ListPage
    {
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string Comment { get; set; }
        public long PayorID { get; set; }
        public string ServiceID { get; set; }
        public string Name { get; set; }
        public DateTime? EligibilityCheckDate { get; set; }
        public int Upload271FileProcessStatus { get; set; }
        public string ListOfIdsInCSV { get; set; }
        public int IsDeleted { get; set; }
        public List<string> ReferralStatusIDs { get; set; }

    }

    public class AddProcess271Model
    {
        [Ignore]
        public string Comment { get; set; }

        [Ignore]
        [Required(ErrorMessageResourceName = "Upload271FileRequired", ErrorMessageResourceType = typeof(Resource))]
        public string TempFilePath { get; set; }
    }
    public class AddProcess270271Model
    {
        public AddProcess270271Model()
        {
            PayorList = new List<NameValueData>();
            ReferralStatuses = new List<ReferralStatus>();
            FileProcessStatus = new List<NameValueData>();
            ServiceList = new List<NameValueDataInString>();
            DeleteFilter = new List<NameValueData>();
            AddProcess270Model = new AddProcess270Model();
            SearchProcess270ListPage = new SearchProcess270ListPage();
            AddProcess271Model = new AddProcess271Model();
            SearchProcess271ListPage = new SearchProcess271ListPage();
        }

        public List<NameValueData> PayorList { get; set; }
        public List<ReferralStatus> ReferralStatuses { get; set; }

        [Ignore]
        public List<NameValueData> FileProcessStatus { get; set; }
        [Ignore]
        public List<NameValueDataInString> ServiceList { get; set; }
        [Ignore]
        public List<NameValueData> DeleteFilter { get; set; }

        [Ignore]
        public AddProcess270Model AddProcess270Model { get; set; }
        [Ignore]
        public SearchProcess270ListPage SearchProcess270ListPage { get; set; }
        [Ignore]
        public AddProcess271Model AddProcess271Model { get; set; }
        [Ignore]
        public SearchProcess271ListPage SearchProcess271ListPage { get; set; }
    }


    #endregion

    #region 277CA

    public class ListEdi277FileLogModel : Edi277File
    {

        public string Payor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Count { get; set; }
        public string StrDisplayName { get; set; }
        // public string StrDisplayName { get { return Common.GetGeneralNameFormat(FirstName, LastName); } }
        public string StrFileSize { get { return Common.GetFileSize(Convert.ToDecimal(FileSize), Constants.SizeIn_KB); } }


        public string AWSSignedFilePath
        {
            get
            {
                AmazonFileUpload az = new AmazonFileUpload();
                return az.GetPreSignedUrl(ConfigSettings.ZarephathBucket, FilePath);
            }
        }

        public string AWSReadableFilePath
        {
            get
            {
                AmazonFileUpload az = new AmazonFileUpload();
                return az.GetPreSignedUrl(ConfigSettings.ZarephathBucket, ReadableFilePath);
            }
        }
    }

    public class SearchProcess277ListPage
    {
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string Comment { get; set; }
        public long PayorID { get; set; }
        public int Upload277FileProcessStatus { get; set; }
        public string ListOfIdsInCSV { get; set; }
        public int IsDeleted { get; set; }
    }

    public class AddProcess277Model
    {
        public string Comment { get; set; }
        [Required(ErrorMessageResourceName = "PayorRequired", ErrorMessageResourceType = typeof(Resource))]
        public long PayorID { get; set; }
        [Required(ErrorMessageResourceName = "Upload277CAFileRequired", ErrorMessageResourceType = typeof(Resource))]
        public string TempFilePath { get; set; }
    }

    public class AddProcess277PageModel
    {

        public AddProcess277PageModel()
        {
            PayorList = new List<NameValueData>();
            FileProcessStatus = new List<NameValueData>();
            DeleteFilter = new List<NameValueData>();
            AddProcess277Model = new AddProcess277Model();
            SearchProcess277ListPage = new SearchProcess277ListPage();
        }

        public List<NameValueData> PayorList { get; set; }
        [Ignore]
        public List<NameValueData> FileProcessStatus { get; set; }
        [Ignore]
        public List<NameValueData> DeleteFilter { get; set; }
        [Ignore]
        public AddProcess277Model AddProcess277Model { get; set; }
        [Ignore]
        public SearchProcess277ListPage SearchProcess277ListPage { get; set; }
    }

    #endregion

    #region GenerateEdiFileModel

    public class GenerateEdiFileModel
    {
        public GenerateEdiFileModel()
        {
            DxCodes = new List<string[]>();
            ServiceLines = new List<ServiceLineForEdiFileModel>();
        }
        public long BatchID { get; set; }
        public long NoteID { get; set; }
        public long ReferralID { get; set; }
        public string PatientName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PatientDOB { get; set; }
        public string Dob { get; set; }
        public string Gender { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public string MedicalRecordNumber { get; set; }
        public string PolicyNumber { get; set; }
        public string AdmissionDate { get; set; }
        public string SubscriberID { get; set; }
        public string ClaimSubmitterIdentifier { get; set; }
        public string PatientAccountNumber { get; set; }
        public string DobDD { get; set; }
        public string DobMM { get; set; }
        public string DobYYYY { get; set; }

        public long PayorID { get; set; }
        public string PayorName { get; set; }
        public string PayorAddress { get; set; }
        public string PayorCity { get; set; }
        public string PayorState { get; set; }
        public string PayorZipcode { get; set; }
        public string PayorIdentificationNumber { get; set; }

        public long ContactID { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PatientAddress { get; set; }

        public string ModifierName { get; set; }

        public string Phone1 { get; set; }
        public string CareType { get; set; }

        public long PhysicianID { get; set; }
        public string PhysicianFirstName { get; set; }
        public string PhysicianLastName { get; set; }
        public string PhysicianFullName { get; set; }
        public string PhysicianNPINumber { get; set; }

        public string BillingProviderName { get; set; }
        public string BillingProviderFirstName { get; set; }
        public string BillingProviderAddress { get; set; }
        public string BillingProviderCity { get; set; }
        public string BillingProviderState { get; set; }
        public string BillingProviderZipcode { get; set; }
        public string BillingProviderNPI { get; set; }
        public string BillingProviderInfo { get; set; }
        public string BillingProviderEIN { get; set; }


        public string RenderingProviderInfo { get; set; }
        public string RenderingProviderFirstName { get; set; }
        public string RenderingProviderName { get; set; }
        public string RenderingProviderNPI { get; set; }

        public long ReferralBillingAuthorizationID { get; set; }
        public string AuthorizationCode { get; set; }
        public string AdmissionTypeCode_UB04 { get; set; }
        public string AdmissionSourceCode_UB04 { get; set; }
        public string PatientStatusCode_UB04 { get; set; }

        public float TotalAmount { get; set; }
        public string TotalAmountInDollars { get; set; }
        public string TotalAmountInCents { get; set; }

        public string ServiceStartDate { get; set; }
        public string ServiceEndDate { get; set; }

        public string CreatedDate
        {
            get
            {
                return DateTime.Now.ToString("MM/dd/yy");
            }
        }
        public string Subscriber_SBR02_RelationshipCode { get; set; }

        public float Rate { get; set; }
        public float CalculatedUnit { get; set; }
        public float CalculatedAmount { get; set; }

        public List<string[]> DxCodes { get; set; }
        public List<ServiceLineForEdiFileModel> ServiceLines { get; set; }
        public List<MultiServiceLineForEdiFileModel> MultiServiceLineForEdiFileModelList { get; set; }
        public string DiagnosisPointer { get; set; }
        public string TotalCharge1 { get; set; }
        public string TotalCharge2 { get; set; }
    }

    public class MultiServiceLineForEdiFileModel
    {
        public MultiServiceLineForEdiFileModel()
        {
            MultiServiceLines = new List<ServiceLineForEdiFileModel>();
        }
        public string UniqueId { get; set; }
        public float FinalAmount
        {
            get
            {
                return MultiServiceLines.Sum(q => q.CalAmount);
            }
        }

        public string FinalAmountInDollars { get; set; }
        public string FinalAmountInCents { get; set; }

        public List<ServiceLineForEdiFileModel> MultiServiceLines { get; set; }
    }


    public class ServiceLineForEdiFileModel
    {
        public ServiceLineForEdiFileModel()
        {
            ServiceCode = string.Empty;
            POS_CMS1500 = string.Empty;
            RenderingProviderNPI = string.Empty;
            ModifierNames = new string[0];
            ServiceDateDD = string.Empty;
            ServiceDateMM = string.Empty;
            ServiceDateYYYY = string.Empty;
            DiagnosisPointer = string.Empty;
            CalculatedUnit = string.Empty;
            CalculatedAmountInDollars = string.Empty;
            CalculatedAmountInCents = string.Empty;
            ModifierName = string.Empty;
            RevenueCode = string.Empty;
            StrServiceDate = string.Empty;
            CareType = string.Empty;
            POS_CMS1500_ID = 0;
        }

        public DateTime? ServiceDate { get; set; }
        public string ServiceDateDD { get; set; }
        public string ServiceDateMM { get; set; }
        public string ServiceDateYYYY { get; set; }
        public string StrServiceDate { get; set; }

        public string RevenueCode { get; set; }
        public string CareType { get; set; }
        public string ServiceCode { get; set; }
        public int POS_CMS1500_ID { get; set; }
        public string POS_CMS1500 { get; set; }
        public string RenderingProviderNPI { get; set; }

        public string[] ModifierIDs { get; set; }
        public string ModifierID { get; set; }
        public string[] ModifierNames { get; set; }
        public string ModifierName { get; set; }

        public float? Rate { get; set; }
        public string RateInDollars { get; set; }
        public string RateInCents { get; set; }

        public string CalculatedUnit { get; set; }
        public float? CalculatedUnitInFloat { get; set; }

        public float CalAmount { get; set; }
        public float? CalculatedAmount { get; set; }
        public string CalculatedAmountInDollars { get; set; }
        public string CalculatedAmountInCents { get; set; }

        public string DiagnosisPointer { get; set; }
    }

    #endregion
}