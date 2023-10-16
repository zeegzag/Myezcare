using System.Collections.Generic;

namespace OopFactory.Edi835Parser.Models
{
    public class InterchangeControlHeader
    {
        public string AuthorizationInformationQualifier { get; set; } // ISA01
        public string AuthorizationInformation { get; set; }          // ISA02
        public string SecurityInformationQualifier { get; set; }      // ISA03
        public string SecurityInformation { get; set; }               // ISA04
        public string InterchangeSenderIdQualifier { get; set; }      // ISA05
        public string InterchangeSenderId { get; set; }               // ISA06
        public string InterchangeReceiverIdQualifier { get; set; }    // ISA07
        public string InterchangeReceiverId { get; set; }             // ISA08
        public string InterchangeDate { get; set; }                   // ISA09
        public string InterchangeTime { get; set; }                   // ISA10
        public string RepetitionSeparator { get; set; }               // ISA11
        public string InterchangeControlVersionNumber { get; set; }   // ISA12
        public string InterchangeControlNumber { get; set; }          // ISA13
        public string AcknowledgementRequired { get; set; }           // ISA14
        public string UsageIndicator { get; set; }                    // ISA15
        public string ComponentElementSeparator { get; set; }         // ISA16

        public string SegmentTerminator { get; set; }
        public string ElementSeparator { get; set; }
    }

    public class FunctionalGroupHeader
    {
        public string FunctionalIdentifierCode { get; set; }    // GS01
        public string ApplicationSenderCode { get; set; }       // GS02
        public string ApplicationReceiverCode { get; set; }     // GS03
        public string Date { get; set; }                        // GS04
        public string Time { get; set; }                        // GS05
        public string GroupControlNumber { get; set; }          // GS06
        public string ResponsibleAgencyCode { get; set; }       // GS07
        public string VersionOrReleaseOrNo { get; set; }        // GS08
        public string I_VersionOrReleaseOrNo { get; set; }      // GS08-ST03
    }

    public class TransactionSetHeader
    {
        public string TransactionSetIdentifier { get; set; }             // ST01
        public string TransactionSetControlNumber { get; set; }          // ST02
        public string ImplementationConventionReference { get; set; }    // ST03
    }

    public class BeginningOfHierarchicalTransaction
    {
        public string HierarchicalStructureCode { get; set; }  // BHT01
        public string TransactionSetPurposeCode { get; set; }  // BHT02
        public string ReferenceIdentification { get; set; }    // BHT03
        public string Date { get; set; }                       // BHT04
        public string InterchangeIdQualifier { get; set; }     // BHT05
        public string TransactionTypeCode { get; set; }        // BHT06
    }

    public class TypedLoopNM1 // 1000A
    {
        public string EntityIdentifierCodeEnum { get; set; }            // NM101
        public string EntityTypeQualifier { get; set; }                 // NM102
        public string NameLastOrOrganizationName { get; set; }          // NM103
        public string NameFirst { get; set; }                           // NM104
        public string NameMiddle { get; set; }                          // NM105
        public string NamePrefix { get; set; }                          // NM106
        public string NameSuffix { get; set; }                          // NM107
        public string IdCodeQualifier { get; set; }                     // NM108
        public string IdCodeQualifierEnum { get; set; }                 // NM109
        public string EntityRelationshipCode { get; set; }              // NM110
        public string EntityIdentifierCode { get; set; }                // NM111
        public string NameLastOrOrganizationName112 { get; set; }          // NM112
    }

    public class TypedLoopPER // 1000A
    {
        public string ContactFunctionCode { get; set; }                // PER01
        public string Name { get; set; }                               // PER02
        public string CommunicationNumberQualifier1 { get; set; }      // PER03
        public string CommunicationNumber1 { get; set; }               // PER04
        public string CommunicationNumberQualifier2 { get; set; }      // PER05
        public string CommunicationNumber2 { get; set; }               // PER06
        public string CommunicationNumberQualifier3 { get; set; }      // PER07
        public string CommunicationNumber3 { get; set; }               // PER08
        public string ContactInquiryReference { get; set; }            // PER09
    }

    public class BillingProvider // Loop2000A
    {
        public BillingProvider()
        {
            Subscribers = new List<Subscriber>();
        }

        public string HierarchicalIDNumber { get; set; } // HL01  //ADD
        public string HierarchicalParentIDNumber { get; set; } // HL02
        public string HeirarchicalLevelCode { get; set; } // HL03
        public string HierarchicalChildCode { get; set; } // HL03  //ADD

        public string EntityIdentifierCode { get; set; } // NM101
        public string EntityTypeQualifier { get; set; } // NM102
        public string NameLastOrOrganizationName { get; set; } // NM103
        public string BillingProviderFirstName { get; set; } // NM104
        public string IdCodeQualifier { get; set; } // NM108
        public string IdCodeQualifierEnum { get; set; } // NM109

        public string AddressInformation { get; set; } // N301

        public string CityName { get; set; } // N401
        public string StateOrProvinceCode { get; set; } // N402
        public string PostalCode { get; set; } // N403

        public string ReferenceIdentificationQualifier { get; set; } // REF01
        public string ReferenceIdentification { get; set; } // REF02

        public List<Subscriber> Subscribers { get; set; }

        public string PVR01_ProviderCode { get; set; } // PVR01
        public string PVR02_ReferenceIdentificationQualifier { get; set; } // PVR02
        public string PVR03_ReferenceIdentification { get; set; } // PVR03

        public string I_PVR03_ReferenceIdentification { get; set; } // PVR03
    }

    public class Subscriber // Loop2000B
    {
        public Subscriber()
        {
            Claims = new List<Claim>();
        }

        //public string HeirarchicalLevelCode { get; set; } // HL01 ADD
        //public string HeirarchicalLevelCode { get; set; }// HL02 ADD
        public string HeirarchicalLevelCode { get; set; }// HL03 
        //public string HeirarchicalLevelCode { get; set; } // HL04 ADD

        public string PayerResponsibilitySequenceNumber { get; set; } // SBR01
        public string IndividualRelationshipCode { get; set; } // SBR02
        public string PolicyNumber { get; set; } // SBR03
        public string ClaimFilingIndicatorCode { get; set; } // SBR09

        #region Subscriber Name

        public string SubmitterEntityIdentifierCode { get; set; } // NM101
        public string SubmitterEntityTypeQualifier { get; set; } // NM102
        public string SubmitterNameLastOrOrganizationName { get; set; } // NM103
        public string SubmitterNameFirst { get; set; } // NM104
        public string SubmitterIdCodeQualifier { get; set; } // NM108
        public string SubmitterIdCodeQualifierEnum { get; set; } // NM109

        public string SubmitterAddressInformation { get; set; } // N301

        public string SubmitterCityName { get; set; } // N401
        public string SubmitterStateOrProvinceCode { get; set; } // N402
        public string SubmitterPostalCode { get; set; } // N403

        public string SubmitterDateTimePeriodFormatQualifier { get; set; } // DMG01
        public string SubmitterDateTimePeriod { get; set; } // DMG02
        public string SubmitterGenderCode { get; set; } // DMG03

        #endregion Subscriber Name

        #region Payer Name

        public string PayerEntityIdentifierCode { get; set; } // NM101
        public string PayerEntityTypeQualifier { get; set; } // NM102
        public string PayerNameLastOrOrganizationName { get; set; } // NM103
        public string PayerIdCodeQualifier { get; set; } // NM108
        public string PayerIdCodeQualifierEnum { get; set; } // NM109

        public string PayerAddressInformation { get; set; } // N301

        public string PayerCityName { get; set; } // N401
        public string PayerStateOrProvinceCode { get; set; } // N402
        public string PayerPostalCode { get; set; } // N403

        #endregion Payer Name

        public List<Claim> Claims { get; set; }
    }

    public class Claim // Loop2300
    {
        public Claim()
        {
            ServiceLines = new List<ServiceLine>();
        }

        public string ClaimSubmitterIdentifier { get; set; } // CLM01

        public string StrClaimId { get; set; } // REFD9
        public string MyEzCare_BatchNoteID { get; set; } // REFD9
        public string REF_D9_Key { get; set; } // Claim Identifier for Transmission Intermediaries D9
        //public string PatientControlNumber { get; set; } // CLM01
        public string TotalClaimChargeAmount { get; set; } // CLM02
        public string FacilityCodeValue { get; set; } // CLM05-01
        public string FacilityCodeQualifier { get; set; } // CLM05-02
        public string ClaimFrequencyTypeCode { get; set; } // CLM05-03
        public string ProviderOrSupplierSignatureIndicator { get; set; } // CLM06
        public string ProviderAcceptAssignmentCode { get; set; } // CLM07
        public string BenefitsAssignmentCerficationIndicator { get; set; } // CLM08
        public string ReleaseOfInformationCode { get; set; } // CLM09
        public string PatientSignatureSourceCode { get; set; } // CLM10
        public int SpecialProgramCode { get; set; } // CLM12
        public string Prior_ReferenceIdentificationQualifier { get; set; } // Prior_REF01
        public string Prior_ReferenceIdentification { get; set; } // Prior_REF02
        public string ReferenceIdentificationQualifier { get; set; } // REF01
        public string ReferenceIdentification { get; set; } // REF02
        public string HealthCareCodeInformation01 { get; set; } // HI01-01, HI01-02

        public string HealthCareCodeInformation01_01 { get; set; } // HI01-01
        public string HealthCareCodeInformation01_02 { get; set; } // HI01-02

        public string ReferenceIdentificationQualifier_F8_02 { get; set; } // REF01_02
        public string ReferenceIdentification_F8_02 { get; set; } // REF02_02


        public string HealthCareCodeInformation02 { get; set; }
        public string HealthCareCodeInformation03 { get; set; }
        public string HealthCareCodeInformation04 { get; set; }
        public string HealthCareCodeInformation05 { get; set; }
        public string HealthCareCodeInformation06 { get; set; }
        public string HealthCareCodeInformation07 { get; set; }
        public string HealthCareCodeInformation08 { get; set; }
        public string HealthCareCodeInformation09 { get; set; }
        public string HealthCareCodeInformation10 { get; set; }
        public string HealthCareCodeInformation11 { get; set; }
        public string HealthCareCodeInformation12 { get; set; }


        #region Provider Information

        #region Provider Information > Rendering Provider

        public string RenderingProviderEntityIdentifierCode { get; set; } // NM101
        public string RenderingProviderEntityTypeQualifier { get; set; } // NM102
        public string RenderingProviderNameLastOrOrganizationName { get; set; } // NM103
        public string RenderingProviderIdCodeQualifier { get; set; } // NM108
        public string RenderingProviderIdCodeQualifierEnum { get; set; } // NM109

        #endregion Provider Information > Rendering Provider

        #region Provider Information > Supervising Provider

        public string SupervisingProviderEntityIdentifierCode { get; set; } // NM101
        public string SupervisingProviderEntityTypeQualifier { get; set; } // NM102
        public string SupervisingProviderNameLastOrOrganizationName { get; set; } // NM103
        public string SupervisingProviderFirstName { get; set; } // NM104

        
        public string SupervisingProvider_SecondaryIdentification_ReferenceIdentificationQualifier { get; set; } // REF_01 - Default Value = 0B
        public string SupervisingProvider_SecondaryIdentification_ReferenceIdentification { get; set; } // REF 02
        public string SupervisingProvider_SecondaryIdentification_PayorReferenceIdentificationQualifier { get; set; } // REF04_01 DEFAULT Value 2U
        public string SupervisingProvider_SecondaryIdentification_PayorReferenceIdentification { get; set; } // NM104

        #endregion Provider Information > Rendering Provider

        #region Provider Information > Service Facility Location

        public string ServiceFacilityLocationEntityIdentifierCode { get; set; } // NM101
        public string ServiceFacilityLocationEntityTypeQualifier { get; set; } // NM102
        public string ServiceFacilityLocationNameLastOrOrganizationName { get; set; } // NM103
        public string ServiceFacilityLocationIdCodeQualifier { get; set; } // NM108
        public string ServiceFacilityLocationIdCodeQualifierEnum { get; set; } // NM109

        public string ServiceFacilityLocationAddressInformation { get; set; } // N301

        public string ServiceFacilityLocationCityName { get; set; } // N401
        public string ServiceFacilityLocationStateOrProvinceCode { get; set; } // N402
        public string ServiceFacilityLocationPostalCode { get; set; } // N403

        #endregion Provider Information > Service Facility Location

        #endregion Provider Information

        public List<ServiceLine> ServiceLines { get; set; }

        public bool IsCaseManagement { get; set; }
        public bool IsHomeCare { get; set; }
        public bool IsDayCare { get; set; }
        public string ReferringProvider_EntityIdentifierCode { get; set; } // NM101
        public int ReferringProvider_EntityTypeQualifier { get; set; } // NM102
        public string ReferringProvider_NameLastOrOrganizationName { get; set; } // NM103
        public string ReferringProvider_NameFirst { get; set; } // NM104
        public string ReferringProvider_IDCodeQualifier { get; set; } // NM108
        public string ReferringProvider_IDCode { get; set; } // NM109

        public string RenderingProvider_EntityIdentifierCode { get; set; }// NM101
        public string RenderingProvider_NameLastOrOrganizationName { get; set; } // NM103
        public string RenderingProvider_NameFirst { get; set; } // NM104
        public string RenderingProvider_EntityTypeQualifier { get; set; }// NM102
        public string RenderingProvider_IDCodeQualifier { get; set; }// NM108
        public string RenderingProvider_IDCode { get; set; } // NM109

        public string RenderingProvider_ProviderCode { get; set; } // PRV01
        public string RenderingProvider_ReferenceIdentificationQualifier { get; set; } // PRV02

        public string RenderingProvider_TaxonomyCode { get; set; } // PRV03



        public string SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName { get; set; }
        public string SupervisingProvidername2420DLoop_NM104_NameFirst { get; set; }
        public string SupervisingProvidername2420DLoop_REF02_ReferenceId { get; set; }



        public string I_AttendingProvider_EntityIdentifierCode { get; set; }
        public string I_AttendingProvider_EntityTypeQualifier { get; set; }
        public string I_AttendingProvider_IDCodeQualifier { get; set; }

        public string I_Claim_DTP01_DateTimeQualifier { get; set; }
        public string I_Claim_DTP01_DateTimePeriodFormatQualifier { get; set; }
        public string I_Claim_DTP01_DateTimePeriod { get; set; }

        public string I_Claim_DTP02_DateTimeQualifier { get; set; }
        public string I_Claim_DTP02_DateTimePeriodFormatQualifier { get; set; }
        public string I_Claim_DTP02_DateTimePeriod { get; set; }

        public string AdmissionTypeCode_UB04 { get; set; }
        public string AdmissionSourceCode_UB04 { get; set; }
        public string PatientStatusCode_UB04 { get; set; }
    }

    public class ServiceLine // Loop2400
    {
        public string AssignedNumber { get; set; } // LX01

        public string Product_ServiceID { get; set; } // SV201 //RevenueCode
        public string CompositeMedicalProcedureIdentifier { get; set; } // SV101-01, SV101-0
        //public string CompositeMedicalProcedureIdentifier_01 { get; set; } // SV101-01, SV101-02
        // public string CompositeMedicalProcedureIdentifier_02 { get; set; } // SV101-01, SV101-02
        public string ServiceDescription { get; set; } // SV10-07
        public string MonetaryAmount { get; set; } // SV102
        public string UnitOrBasisForMeasurementCode { get; set; } // SV103
        public string Quantity { get; set; } // SV104
        public string FacilityCode { get; set; } // SV105
        public string CompositeDiagnosisCodePointer { get; set; } // SV107
        public string CompositeDiagnosisCodePointer_01 { get; set; } // SV107_01

        public string DateTimeQualifier { get; set; } // DTP01
        public string DateTimePeriodFormatQualifier { get; set; } // DTP02
        public string DateTimePeriod { get; set; } // DTP03

        public string ReferenceIdentificationQualifier { get; set; } // REF01_01
        public string ReferenceIdentification { get; set; } // REF02_01


        public string NTE01_NoteReferenceCode { get; set; } // REF01_01
        public string NTE02_Description { get; set; } // REF01_01


    }

    public class Edi837Model
    {
        public Edi837Model()
        {
            InterchangeControlHeader = new InterchangeControlHeader();
            FunctionalGroupHeader = new FunctionalGroupHeader();
            TransactionSetHeader = new TransactionSetHeader();
            BeginningOfHierarchicalTransaction = new BeginningOfHierarchicalTransaction();
            SubmitterName = new TypedLoopNM1();
            SubmitterEDIContact = new List<TypedLoopPER>();
            ReceiverName = new TypedLoopNM1();
            BillingProviders = new List<BillingProvider>();
        }

        public InterchangeControlHeader InterchangeControlHeader { get; set; }
        public FunctionalGroupHeader FunctionalGroupHeader { get; set; }
        public TransactionSetHeader TransactionSetHeader { get; set; }
        public BeginningOfHierarchicalTransaction BeginningOfHierarchicalTransaction { get; set; }
        public TypedLoopNM1 SubmitterName { get; set; }
        public List<TypedLoopPER> SubmitterEDIContact { get; set; } //MAXIMUM ALLOWED 2
        public TypedLoopNM1 ReceiverName { get; set; }

        public List<BillingProvider> BillingProviders { get; set; }
    }




    public class Subscriber270
    {

        public string HeirarchicalLevelCode { get; set; }// HL03 

        public string TRN01_TraceTypeCode { get; set; } // TRN01
        public string TRN02_ReferenceIdentification02 { get; set; } // TRN02
        public string TRN03_CompanyIdentifier { get; set; } // TRN03
        public string TRN04_ReferenceIdentification04 { get; set; } // TRN04


        #region Subscriber

        public string SubmitterEntityIdentifierCode { get; set; } // NM101
        public string SubmitterEntityTypeQualifier { get; set; } // NM102
        public string SubmitterNameLastOrOrganizationName { get; set; } // NM103
        public string SubmitterNameFirst { get; set; } // NM104
        public string SubmitterIdCodeQualifier { get; set; } // NM108
        public string SubmitterIdCodeQualifierEnum { get; set; } // NM109

        public string SubmitterDateTimePeriodFormatQualifier { get; set; } // DMG01
        public string SubmitterDateTimePeriod { get; set; } // DMG02
        public string SubmitterGenderCode { get; set; } // DMG03


        public string SubmitterDTPQualifier { get; set; } // DTP01
        public string SubmitterDTPFormatQualifier { get; set; } // DTP02
        public string SubmitterDTPDateTimePeriod { get; set; } // DTP03


        public string SubmitterEligibility01 { get; set; } // EQ01

        #endregion Subscriber


    }


    public class InformationLevelModel
    {
        public string HeirarchicalLevelCode { get; set; } // HL03

        public string EntityIdentifierCode { get; set; } // NM101
        public string EntityTypeQualifier { get; set; } // NM102
        public string NameLastOrOrganizationName { get; set; } // NM103
        public string IdCodeQualifier { get; set; } // NM108
        public string IdCodeQualifierEnum { get; set; } // NM109
    }

    public class HlLevel270
    {
        public HlLevel270()
        {
            InformationSource = new InformationLevelModel();
            InformationReceiver = new InformationLevelModel();
            Subscribers = new List<Subscriber270>();
        }
        public InformationLevelModel InformationSource { get; set; }
        public InformationLevelModel InformationReceiver { get; set; }
        public List<Subscriber270> Subscribers { get; set; }
    }

    public class Edi270Model
    {
        public Edi270Model()
        {
            InterchangeControlHeader = new InterchangeControlHeader();
            FunctionalGroupHeader = new FunctionalGroupHeader();
            TransactionSetHeader = new TransactionSetHeader();
            BeginningOfHierarchicalTransaction = new BeginningOfHierarchicalTransaction();
            HlLevel270Model = new List<HlLevel270>();
        }

        public InterchangeControlHeader InterchangeControlHeader { get; set; }
        public FunctionalGroupHeader FunctionalGroupHeader { get; set; }
        public TransactionSetHeader TransactionSetHeader { get; set; }
        public BeginningOfHierarchicalTransaction BeginningOfHierarchicalTransaction { get; set; }
        public List<HlLevel270> HlLevel270Model { get; set; }
    }
}
