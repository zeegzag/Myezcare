using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Infrastructure;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("PayorEdi837Settings")]
    [PrimaryKey("PayorEdi837SettingId")]
    [Sort("PayorEdi837SettingId", "DESC")]
    public class PayorEdi837Setting
    {
        public long PayorEdi837SettingId { get; set; }
        public long PayorID { get; set; }
        public string ISA01_AuthorizationInformationQualifier { get; set; }  // 00 :Common Value
        public string ISA02_AuthorizationInformation { get; set; } // 10 spaces :Default value
        public string ISA03_SecurityInformationQualifier { get; set; } //00 Common
        public string ISA04_SecurityInformation { get; set; } // 10 spaces :Default value
        public string ISA05_InterchangeSenderIdQualifier { get; set; } //@@

        [Required(ErrorMessageResourceName = "SenderIdRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ISA06_InterchangeSenderId { get; set; }  //@@@@@@@@@@@@@@@
        public string ISA07_InterchangeReceiverIdQualifier { get; set; } //@@

        [Required(ErrorMessageResourceName = "ReceiverIdRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ISA08_InterchangeReceiverId { get; set; } //@@@@@@@@@@@@@@@
        [Ignore]
        public string ISA09_InterchangeDate { get { return String.Format("{0:yyMMdd}", Common.GetOrgCurrentDateTime()  ); } } //Created Date  Format: YYMMDD
        [Ignore]
        public string ISA10_InterchangeTime { get { return String.Format("{0:hhmm}", Common.GetOrgCurrentDateTime() ); } } //Created Time Format: HHMM

        [Required(ErrorMessageResourceName = "RepetitionSeparatorRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ISA11_RepetitionSeparator { get; set; } // ^ :Common Value
        public string ISA12_InterchangeControlVersionNumber { get; set; } // 00501 :Default value
        public string ISA13_InterchangeControlNumber { get; set; } //00000001 :Begins with 00000001 and increments by +1 for each subsequent file create each day. Resets each day.
        public string ISA14_AcknowledgementRequired { get; set; } // 0 :Common Value
        public string ISA15_UsageIndicator { get; set; } //P  :Common Value

        [Required(ErrorMessageResourceName = "ElementSeparatorRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ISA16_ComponentElementSeparator { get; set; } //@
        public string GS01_FunctionalIdentifierCode { get; set; } //HC :Default value
        public string GS02_ApplicationSenderCode { get; set; }  // @@@@@@@@@@@@@@@
        public string GS03_ApplicationReceiverCode { get; set; } // @@@@@@@@@@@@@@
        [Ignore]
        public string GS04_Date { get { return String.Format("{0:yyyyMMdd}", Common.GetOrgCurrentDateTime() ); } } //Created Date  Format: YYYYMMDD
        [Ignore]
        public string GS05_Time { get { return String.Format("{0:hhmm}", Common.GetOrgCurrentDateTime() ); } } //Created Time Format: HHMM
        public string GS06_GroupControlNumber { get; set; } // Begins with 1 and increments +1 for each subsequent GS within the file. Resets back to 1 with each new file.
        public string GS07_ResponsibleAgencyCode { get; set; } // X :Default value
        public string GS08_VersionOrReleaseOrNo { get; set; } // 005010X222A1
        public string ST01_TransactionSetIdentifier { get; set; } // 837 :Default value
        public string ST02_TransactionSetControlNumber { get; set; }
        public string ST03_ImplementationConventionReference { get; set; } // 005010X222A1 : Default Values
        public string BHT01_HierarchicalStructureCode { get; set; }
        public string BHT02_TransactionSetPurposeCode { get; set; }
        public string BHT03_ReferenceIdentification { get; set; }
        [Ignore]
        public string BHT04_Date { get { return String.Format("{0:yyyyMMdd}", Common.GetOrgCurrentDateTime() ); } }
        [Ignore]
        public string BHT05_Time { get { return String.Format("{0:hhmmss}", Common.GetOrgCurrentDateTime() ); } }
        public string BHT06_TransactionTypeCode { get; set; }
        public string Submitter_NM101_EntityIdentifierCodeEnum { get; set; }
        public string Submitter_NM102_EntityTypeQualifier { get; set; }

        [Required(ErrorMessageResourceName = "SubmitterNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Submitter_NM103_NameLastOrOrganizationName { get; set; }
        public string Submitter_NM104_NameFirst { get; set; }
        public string Submitter_NM105_NameMiddle { get; set; }
        public string Submitter_NM106_NamePrefix { get; set; }
        public string Submitter_NM107_NameSuffix { get; set; }
        public string Submitter_NM108_IdCodeQualifier { get; set; }

        [Required(ErrorMessageResourceName = "SubmitterCodeQualifierRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Submitter_NM109_IdCodeQualifierEnum { get; set; }
        public string Submitter_NM110_EntityRelationshipCode { get; set; }
        public string Submitter_NM111_EntityIdentifierCode { get; set; }
        public string Submitter_NM112_NameLastOrOrganizationName { get; set; }
        public string Submitter_EDIContact1_PER01_ContactFunctionCode { get; set; }

        [Required(ErrorMessageResourceName = "SubmitterContactNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Submitter_EDIContact1_PER02_Name { get; set; }
        public string Submitter_EDIContact1_PER03_CommunicationNumberQualifier { get; set; }

        [Required(ErrorMessageResourceName = "SubmitterPhoneRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "PhoneInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Submitter_EDIContact1_PER04_CommunicationNumber { get; set; }
        public string Submitter_EDIContact1_PER05_CommunicationNumberQualifier { get; set; }
        public string Submitter_EDIContact1_PER06_CommunicationNumber { get; set; }
        public string Submitter_EDIContact1_PER07_CommunicationNumberQualifier { get; set; }

        [Required(ErrorMessageResourceName = "SubmitterEmailRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Submitter_EDIContact1_PER08_CommunicationNumber { get; set; }
        public string Submitter_EDIContact1_PER09_ContactInquiryReference { get; set; }
        public string Submitter_EDIContact2_PER01_ContactFunctionCode { get; set; }
        public string Submitter_EDIContact2_PER02_Name { get; set; }
        public string Submitter_EDIContact2_PER03_CommunicationNumberQualifier { get; set; }
        public string Submitter_EDIContact2_PER04_CommunicationNumber { get; set; }
        public string Submitter_EDIContact2_PER05_CommunicationNumberQualifier { get; set; }
        public string Submitter_EDIContact2_PER06_CommunicationNumber { get; set; }
        public string Submitter_EDIContact2_PER07_CommunicationNumberQualifier { get; set; }
        public string Submitter_EDIContact2_PER08_CommunicationNumber { get; set; }
        public string Submitter_EDIContact2_PER09_ContactInquiryReference { get; set; }
        public string Receiver_NM101_EntityIdentifierCodeEnum { get; set; }
        public string Receiver_NM102_EntityTypeQualifier { get; set; }
        public string Receiver_NM103_NameLastOrOrganizationName { get; set; }
        public string Receiver_NM104_NameFirst { get; set; }
        public string Receiver_NM105_NameMiddle { get; set; }
        public string Receiver_NM106_NamePrefix { get; set; }
        public string Receiver_NM107_NameSuffix { get; set; }
        public string Receiver_NM108_IdCodeQualifier { get; set; }
        public string Receiver_NM109_IdCodeQualifierEnum { get; set; }
        public string Receiver_NM110_EntityRelationshipCode { get; set; }
        public string Receiver_NM111_EntityIdentifierCode { get; set; }
        public string Receiver_NM112_NameLastOrOrganizationName { get; set; }

        // Trailing Header
        public string SE01_NumberOfIncludedSegments { get; set; } // AUTO MAPPED FROM EDI SET UP
        public string SE02_TransactionSetControlNumber { get; set; } // AUTO MAPPED FROM EDI SET UP
        public string GE01_NumberOfTransactionSetsIncluded { get; set; } // AUTO MAPPED FROM EDI SET UP
        public string GE02_GroupControlNumber { get; set; } // AUTO MAPPED FROM EDI SET UP
        public string IEA01_NumberOfIncludedFunctionalGroups { get; set; } // AUTO MAPPED FROM EDI SET UP
        public string IEA02_InterchangeControlNumber { get; set; } // AUTO MAPPED FROM EDI SET UP




        public string BillingProvider_HL01_HierarchicalIDNumber { get; set; }
        public string BillingProvider_HL02_HierarchicalParentIDNumber { get; set; }
        public string BillingProvider_HL03_HierarchicalLevelCode { get; set; }
        public string BillingProvider_HL04_HierarchicalChildCode { get; set; }

        [Required(ErrorMessageResourceName = "ProviderCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string BillingProvider_PRV01_ProviderCode { get; set; }
        [Required(ErrorMessageResourceName = "ReferenceIdentificationQualifierRequired", ErrorMessageResourceType = typeof(Resource))]
        public string BillingProvider_PRV02_ReferenceIdentificationQualifier { get; set; }
        [Required(ErrorMessageResourceName = "ProviderTaxonomyCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string BillingProvider_PRV03_ProviderTaxonomyCode { get; set; }

        public string BillingProvider_NM101_EntityIdentifierCode { get; set; }
        public string BillingProvider_NM102_EntityTypeQualifier { get; set; }
        public string BillingProvider_NM108_IdentificationCodeQualifier { get; set; }
        public string BillingProvider_REF01_ReferenceIdentificationQualifier { get; set; }
        //Subscriber                                                                 
        public string Subscriber_HL02_HierarchicalParentIDNumber { get; set; }   // AUTO MAPPED FROM EDI SET UP
        public string Subscriber_HL03_HierarchicalLevelCode { get; set; }
        public string Subscriber_HL04_HierarchicalChildCode { get; set; }   // AUTO MAPPED FROM EDI SET UP

        public string Subscriber_SBR01_PayerResponsibilitySequenceNumberCode { get; set; }
        public string Subscriber_SBR02_RelationshipCode { get; set; }
        public string Subscriber_SBR09_ClaimFilingIndicatorCode { get; set; }
        public string Subscriber_NM101_EntityIdentifierCode { get; set; }
        public string Subscriber_NM102_EntityIdentifierCode { get; set; }
        public string Subscriber_NM108_IdentificationCodeQualifier { get; set; }
        public string Subscriber_NM109_SubscriberPrimaryIdentifier { get; set; } // CLIENT AHCCCSID

        public string Subscriber_DMG01_DateTimePeriodFormatQualifier { get; set; }

        public string Subscriber_Payer_NM101_EntityIdentifierCode { get; set; }
        public string Subscriber_Payer_NM102_EntityTypeQualifier { get; set; }
        public string Subscriber_Payer_NM108_IdentificationCodeQualifier { get; set; }
        public string Subscriber_Payer_NM109_IdentificationCode { get; set; }  // COMING FROM PAYOR TABLE

        //Claim Information                                                         

        public string Claim_CLM01_PatientAccountNumber { get; set; }   // CLAIM SUBMITTER ID, We are taking this from Stored Procesdure(Combination of Batch, Batch Note, Note)
        public string Claim_CLM05_02_FacilityCodeQualifier { get; set; }
        public string Claim_CLM05_03_ClaimFrequencyCode { get; set; }  // ORIGINAL

        public string Claim_CLM05_03_ClaimFrequencyCode_Replcement { get; set; }
        public string Claim_CLM05_03_ClaimFrequencyCode_Void { get; set; }

        public string Claim_CLM06_ProviderSignatureOnFile { get; set; }
        public string Claim_CLM07_ProviderAcceptAssignment { get; set; }
        public string Claim_CLM08_AssignmentOfBenefitsIndicator { get; set; }
        public string Claim_CLM09_ReleaseOfInformationCode { get; set; }
        public string Claim_CLM010_PatientSignatureSource { get; set; }

        public string Claim_REF01_ReferenceIdentificationQualifier { get; set; }
        public string Claim_REF02_MedicalRecordNumber { get; set; }

        public string Claim_HI01_01_PrincipalDiagnosisQualifier { get; set; }

        //Provider Information                                                       

        public string Claim_RenderringProvider_NM01_EntityIdentifierCode { get; set; }
        public string Claim_RenderringProvider_NM02_EntityTypeQualifier { get; set; }
        public string Claim_RenderringProvider_NM108_IdentificationCodeQualifier { get; set; }

        public string Claim_ServiceFacility_NM101_EntityIdentifierCode { get; set; }
        public string Claim_ServiceFacility_NM102_EntityTypeQualifier { get; set; }
        public string Claim_ServiceFacility_NM108_IdentificationCodeQualifier { get; set; }

        //Service Line Number                                                       

        public string Claim_ServiecLine_LX01_AssignedNumber { get; set; }  // AUTO MAPPED FROM EDI SET UP
        public string Claim_ServiecLine_SV101_01_ProductServiceIDQualifier { get; set; }
        public string Claim_ServiecLine_SV103_BasisOfMeasurement { get; set; }
        public string Claim_ServiceLine_SV107_01_DiagnosisCodePointer { get; set; }

        public string Claim_ServiceLine_Date_DTP01_DateTimeQualifier { get; set; }
        public string Claim_ServiceLine_Date_DTP02_DateTimePeriodFormatQualifier { get; set; }

        public string Claim_ServiceLine_Ref_REF01_ReferenceIdentificationQualifier { get; set; }
        public string Claim_ServiceLine_Ref_REF02_ReferenceIdentification { get; set; }

        public string Claim_ServiceLine_Ref_REF01_ReferenceIdentificationQualifier_F8_02 { get; set; }
        public string Claim_ServiceLine_Ref_REF02_ReferenceIdentification_F8_02 { get; set; }


        public string SegmentTerminator { get; set; }
        public string ElementSeparator { get; set; }


        public bool SkipSituationalContent { get; set; }

        //Compulsory to include Renderring Provider either each match w/ Billing or Not. Like FOR MMIC it is required
        public bool RequiredRenderingProvider { get; set; }


        public long ISA13_InterchangeControlNo { get; set; }
        public DateTime? ISA13_UpdatedDate { get; set; }

        public bool CheckForPolicyNumber { get; set; }

        [RegularExpression(Constants.RegxNumericSixDigit, ErrorMessageResourceName = "RoundUpUnitInvalid", ErrorMessageResourceType = typeof(Resource))]
        [Range(0,15, ErrorMessageResourceName = "ValueValidation", ErrorMessageResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        public int CMS1500HourRounding { get; set; }

        [RegularExpression(Constants.RegxNumericSixDigit, ErrorMessageResourceName = "RoundUpUnitInvalid", ErrorMessageResourceType = typeof(Resource))]
        [Range(0, 15, ErrorMessageResourceName = "ValueValidation", ErrorMessageResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        public int UB04HourRounding { get; set; }

        public string Claim_Prior_REF01_ReferenceIdentificationQualifier { get; set; }

        public string ReferringProvider_NM101_EntityIdentifierCode { get; set; }
        public int ReferringProvider_NM102_EntityTypeQualifier { get; set; }
        public string ReferringProvider_NM108_IDCodeQualifier { get; set; }

        public string RenderingProvider_NM101_EntityIdentifierCode { get; set; }
        public string RenderingProvider_NM102_EntityTypeQualifier { get; set; }
        public string RenderingProvider_NM108_IDCodeQualifier { get; set; }

        public string RenderingProvider_PRV01_ProviderCode { get; set; }
        public string RenderingProvider_PRV02_ReferenceIdentificationQualifier { get; set; }

        public string I_GS08_ST03_VersionOrReleaseOrNo { get; set; }
        public string I_PVR03_ReferenceIdentification { get; set; }

        public string I_AttendingProvider_NM101_EntityIdentifierCode { get; set; }
        public string I_AttendingProvider_NM102_EntityTypeQualifier { get; set; }
        public string I_AttendingProvider_NM108_IDCodeQualifier { get; set; }

        public string I_Claim_DTP01_01_DateTimeQualifier { get; set; }
        public string I_Claim_DTP01_02_DateTimePeriodFormatQualifier { get; set; }
        public string I_Claim_DTP02_01_DateTimeQualifier { get; set; }
        public string I_Claim_DTP02_02_DateTimePeriodFormatQualifier { get; set; }



        public string SupervisingProvidername2420DLoop_NM101_EntityIdentifierCode { get; set; }  // 'DQ'
        public string SupervisingProvidername2420DLoop_NM102_EntityTypeQualifier { get; set; }   //'1'
        public string SupervisingProvidername2420DLoop_REF01_ReferenceIdQualifier { get; set; }  //'LU'

    }

    public class BillingSettingModel
    {
        public long PayorID { get; set; }
        public string searchtext { get; set; }
        public string Key { get; set; }
        public string Val { get; set; }
        public int? CharLength { get; set; }
    }
}
