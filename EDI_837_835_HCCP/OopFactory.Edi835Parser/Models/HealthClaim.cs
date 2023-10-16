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
        public string EntityIdentifierCodeEnum { get; set; }    // NM101
        public string EntityTypeQualifier { get; set; }         // NM102
        public string NameLastOrOrganizationName { get; set; }  // NM103
        public string NameFirst { get; set; }                   // NM104
        public string NameMiddle { get; set; }                  // NM105
        public string NamePrefix { get; set; }                  // NM106
        public string NameSuffix { get; set; }                  // NM107
        public string IdCodeQualifier { get; set; }             // NM108
        public string IdCodeQualifierEnum { get; set; }         // NM109
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

        public string HeirarchicalLevelCode { get; set; } // HL03

        public string EntityIdentifierCode { get; set; } // NM101
        public string EntityTypeQualifier { get; set; } // NM102
        public string NameLastOrOrganizationName { get; set; } // NM103
        public string IdCodeQualifier { get; set; } // NM108
        public string IdCodeQualifierEnum { get; set; } // NM109

        public string AddressInformation { get; set; } // N301

        public string CityName { get; set; } // N401
        public string StateOrProvinceCode { get; set; } // N402
        public string PostalCode { get; set; } // N403

        public string ReferenceIdentificationQualifier { get; set; } // REF01
        public string ReferenceIdentification { get; set; } // REF02

        public List<Subscriber> Subscribers { get; set; } 
    }

    public class Subscriber // Loop2000B
    {
        public Subscriber()
        {
            Claims = new List<Claim>();
        }

        public string HeirarchicalLevelCode { get; set; } // HL03

        public string PayerResponsibilitySequenceNumber { get; set; } // SBR01
        public string IndividualRelationshipCode { get; set; } // SBR02
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

        public string PatientControlNumber { get; set; } // CLM01
        public string TotalClaimChargeAmount { get; set; } // CLM02
        public string FacilityCodeValue { get; set; } // CLM05-01
        public string FacilityCodeQualifier { get; set; } // CLM05-02
        public string ClaimFrequencyTypeCode { get; set; } // CLM05-03
        public string ProviderOrSupplierSignatureIndicator { get; set; } // CLM06
        public string ProviderAcceptAssignmentCode { get; set; } // CLM07
        public string BenefitsAssignmentCerficationIndicator { get; set; } // CLM08
        public string ReleaseOfInformationCode { get; set; } // CLM09
        public string PatientSignatureSourceCode { get; set; } // CLM10

        public string ReferenceIdentificationQualifier { get; set; } // REF01
        public string ReferenceIdentification { get; set; } // REF02

        public string HealthCareCodeInformation01 { get; set; } // HI01-01, HI01-02

        #region Provider Information

        #region Provider Information > Rendering Provider

        public string RenderingProviderEntityIdentifierCode { get; set; } // NM101
        public string RenderingProviderEntityTypeQualifier { get; set; } // NM102
        public string RenderingProviderNameLastOrOrganizationName { get; set; } // NM103
        public string RenderingProviderIdCodeQualifier { get; set; } // NM108
        public string RenderingProviderIdCodeQualifierEnum { get; set; } // NM109

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
    }

    public class ServiceLine // Loop2400
    {
        public string AssignedNumber { get; set; } // LX01

        public string CompositeMedicalProcedureIdentifier { get; set; } // SV101-01, SV101-02
        public string MonetaryAmount { get; set; } // SV102
        public string UnitOrBasisForMeasurementCode { get; set; } // SV103
        public string Quantity { get; set; } // SV104
        public string CompositeDiagnosisCodePointer { get; set; } // SV107

        public string DateTimeQualifier { get; set; } // DTP01
        public string DateTimePeriodFormatQualifier { get; set; } // DTP02
        public string DateTimePeriod { get; set; } // DTP03

        public string ReferenceIdentificationQualifier { get; set; } // REF01
        public string ReferenceIdentification { get; set; } // REF02
    }

    public class CreateModel
    {
        public CreateModel()
        {
            InterchangeControlHeader = new InterchangeControlHeader();
            FunctionalGroupHeader = new FunctionalGroupHeader();
            TransactionSetHeader = new TransactionSetHeader();
            BeginningOfHierarchicalTransaction = new BeginningOfHierarchicalTransaction();
            SubmitterName = new TypedLoopNM1();
            SubmitterEDIContact = new TypedLoopPER();
            ReceiverName = new TypedLoopNM1();
            BillingProviders = new List<BillingProvider>();
        }

        public InterchangeControlHeader InterchangeControlHeader { get; set; }
        public FunctionalGroupHeader FunctionalGroupHeader { get; set; }
        public TransactionSetHeader TransactionSetHeader { get; set; }
        public BeginningOfHierarchicalTransaction BeginningOfHierarchicalTransaction { get; set; }
        public TypedLoopNM1 SubmitterName { get; set; }
        public TypedLoopPER SubmitterEDIContact { get; set; }
        public TypedLoopNM1 ReceiverName { get; set; }
        public List<BillingProvider> BillingProviders { get; set; }
    }
}
