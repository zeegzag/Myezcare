CREATE PROCEDURE [dbo].[HC_SavePayorBillingSetting]                  
 @PayorEdi837SettingId BIGINT=0,                  
 @PayorID BIGINT,                                      
         
 @ISA06_InterchangeSenderId VARCHAR(15)=NULL,                                      
 @ISA08_InterchangeReceiverId VARCHAR(15)=NULL,                  
 @ISA11_RepetitionSeparator VARCHAR(1)=NULL,                  
 @ISA16_ComponentElementSeparator VARCHAR(1)=NULL,                  
         
 @CMS1500HourRounding INT=0,                  
 @UB04HourRounding INT=0,              
        
-- @BillingProvider_PRV01_ProviderCode VARCHAR(10)=NULL,              
-- @BillingProvider_PRV02_ReferenceIdentificationQualifier VARCHAR(20)=NULL,              
 @BillingProvider_PRV03_ProviderTaxonomyCode VARCHAR(50)=NULL              
AS                                      
BEGIN                     
        
DECLARE @ISA01_AuthorizationInformationQualifier VARCHAR(2)='00';        
DECLARE @ISA03_SecurityInformationQualifier VARCHAR(2)='00';        
DECLARE @ISA05_InterchangeSenderIdQualifier VARCHAR(2)='ZZ';        
DECLARE @ISA07_InterchangeReceiverIdQualifier VARCHAR(2)='ZZ';        
DECLARE @ISA12_InterchangeControlVersionNumber VARCHAR(5)='00001';        
DECLARE @ISA13_InterchangeControlNumber VARCHAR(10)='00000001';        
DECLARE @ISA14_AcknowledgementRequired VARCHAR(1)= '1';         
DECLARE @ISA15_UsageIndicator VARCHAR(1)= 'P';         
DECLARE @GS01_FunctionalIdentifierCode VARCHAR(2)= 'HC' ;        
DECLARE @GS06_GroupControlNumber VARCHAR(1) = '1';        
DECLARE @GS07_ResponsibleAgencyCode VARCHAR(1)= 'X';        
DECLARE @GS08_VersionOrReleaseOrNo VARCHAR(20)= '005010X222A1';         
DECLARE @ST01_TransactionSetIdentifier VARCHAR(4)= '837';         
DECLARE @ST02_TransactionSetControlNumber VARCHAR(4)= '0001';        
DECLARE @ST03_ImplementationConventionReference VARCHAR(20)= '005010X222A1';         
DECLARE @BHT01_HierarchicalStructureCode VARCHAR(4)= '0019';         
DECLARE @BHT02_TransactionSetPurposeCode VARCHAR(2)= '00';         
DECLARE @BHT06_TransactionTypeCode VARCHAR(2)= 'CH';         
DECLARE @Submitter_NM101_EntityIdentifierCodeEnum VARCHAR(2)= '41';         
DECLARE @Submitter_NM102_EntityTypeQualifier VARCHAR(1)= '2';         
DECLARE @Submitter_NM108_IdCodeQualifier VARCHAR(2)= '46';         
DECLARE @Submitter_EDIContact1_PER01_ContactFunctionCode VARCHAR(2)= 'IC';        
DECLARE @Submitter_EDIContact1_PER03_CommunicationNumberQualifier VARCHAR(2)= 'TE';        
DECLARE @Submitter_EDIContact1_PER05_CommunicationNumberQualifier VARCHAR(2)= 'EM';       
DECLARE @Submitter_EDIContact1_PER06_CommunicationNumber VARCHAR(2)= '1';       
DECLARE @Submitter_EDIContact1_PER07_CommunicationNumberQualifier VARCHAR(2)= 'EM';       
      
DECLARE @Receiver_NM101_EntityIdentifierCodeEnum VARCHAR(2)= '40';        
DECLARE @Receiver_NM102_EntityTypeQualifier VARCHAR(1)= '2';        
DECLARE @Receiver_NM108_IdCodeQualifier  VARCHAR(2)= '46';   
       
DECLARE @BillingProvider_HL01_HierarchicalIDNumber VARCHAR(1)= '1';        
DECLARE @BillingProvider_HL03_HierarchicalLevelCode VARCHAR(2)= '20';        
DECLARE @BillingProvider_HL04_HierarchicalChildCode VARCHAR(1)= '1';  
  
DECLARE @BillingProvider_PRV01_ProviderCode VARCHAR(2)= 'BI';         
DECLARE @BillingProvider_PRV02_ReferenceIdentificationQualifier VARCHAR(3)= 'PXC';  
        
DECLARE @BillingProvider_NM101_EntityIdentifierCode VARCHAR(2)= '85';        
DECLARE @BillingProvider_NM102_EntityTypeQualifier VARCHAR(2)= '2';        
DECLARE @BillingProvider_NM108_IdentificationCodeQualifier VARCHAR(2)= 'XX';        
DECLARE @BillingProvider_REF01_ReferenceIdentificationQualifier VARCHAR(2)= 'EI';         
DECLARE @Subscriber_HL02_HierarchicalParentIDNumber  VARCHAR(1)= '1';         
DECLARE @Subscriber_HL03_HierarchicalLevelCode  VARCHAR(2)= '22';         
DECLARE @Subscriber_HL04_HierarchicalChildCode  VARCHAR(1)= '0';         
DECLARE @Subscriber_SBR01_PayerResponsibilitySequenceNumberCode  VARCHAR(1)= 'P';         
DECLARE @Subscriber_SBR02_RelationshipCode  VARCHAR(2)= '18';         
DECLARE @Subscriber_SBR09_ClaimFilingIndicatorCode  VARCHAR(2)= 'MC';         
DECLARE @Subscriber_NM101_EntityIdentifierCode  VARCHAR(2)= 'IL';         
DECLARE @Subscriber_NM102_EntityIdentifierCode  VARCHAR(1)= '1';         
DECLARE @Subscriber_NM108_IdentificationCodeQualifier VARCHAR(2)= 'MI';         
DECLARE @Subscriber_DMG01_DateTimePeriodFormatQualifier VARCHAR(2)= 'D8';         
DECLARE @Subscriber_Payer_NM101_EntityIdentifierCode VARCHAR(2)= 'PR';         
DECLARE @Subscriber_Payer_NM102_EntityTypeQualifier VARCHAR(1)= '2';         
DECLARE @Subscriber_Payer_NM108_IdentificationCodeQualifier VARCHAR(2)= 'PI';         
DECLARE @Claim_CLM01_PatientAccountNumber VARCHAR(10)= '@@@@@@@';         
DECLARE @Claim_CLM05_02_FacilityCodeQualifier VARCHAR(1)= 'B';         
DECLARE @Claim_CLM05_03_ClaimFrequencyCode VARCHAR(1)= '1';         
DECLARE @Claim_CLM06_ProviderSignatureOnFile VARCHAR(1)= 'Y';         
DECLARE @Claim_CLM07_ProviderAcceptAssignment VARCHAR(1)= 'A';         
DECLARE @Claim_CLM08_AssignmentOfBenefitsIndicator VARCHAR(1)= 'Y';         
DECLARE @Claim_CLM09_ReleaseOfInformationCode VARCHAR(1)= 'Y';         
DECLARE @Claim_ServiecLine_LX01_AssignedNumber  VARCHAR(1)= '1';         
      
DECLARE @Claim_ServiecLine_SV101_01_ProductServiceIDQualifier VARCHAR(2)= 'HC';         
DECLARE @Claim_ServiecLine_SV103_BasisOfMeasurement VARCHAR(2)= 'UN';         
DECLARE @Claim_ServiceLine_Date_DTP01_DateTimeQualifier VARCHAR(3)= '472';         
DECLARE @Claim_ServiceLine_Date_DTP02_DateTimePeriodFormatQualifier VARCHAR(2)= 'D8';          
      
      
DECLARE @SegmentTerminator VARCHAR(1)= '~';         
DECLARE @ElementSeparator VARCHAR(1)= '^';         
DECLARE @SkipSituationalContent BIT = 1;         
DECLARE @ISA13_InterchangeControlNo BIGINT= 81;         
DECLARE @RequiredRenderingProvider BIT = 0;         
DECLARE @CheckForPolicyNumber BIT  = 0;         
      
DECLARE @Claim_Prior_REF01_ReferenceIdentificationQualifier VARCHAR(2)= 'G1';       
DECLARE @Claim_REF01_ReferenceIdentificationQualifier VARCHAR(2)='EA';        
      
DECLARE @ReferringProvider_NM101_EntityIdentifierCode VARCHAR(2)= 'DN';         
DECLARE @ReferringProvider_NM102_EntityTypeQualifier INT = 1 ;        
DECLARE @ReferringProvider_NM108_IDCodeQualifier VARCHAR(2)= 'XX';        
DECLARE @RenderingProvider_NM101_EntityIdentifierCode  VARCHAR(2)= '82';         
DECLARE @RenderingProvider_NM102_EntityTypeQualifier INT = 1;        
DECLARE @RenderingProvider_NM108_IDCodeQualifier VARCHAR(2)= 'XX';   
        
DECLARE @RenderingProvider_PRV01_ProviderCode VARCHAR(2)= 'PE';         
DECLARE @RenderingProvider_PRV02_ReferenceIdentificationQualifier VARCHAR(3)= 'PXC';  
        
DECLARE @I_GS08_ST03_VersionOrReleaseOrNo VARCHAR(20)= '005010X223A2';         
DECLARE @I_PVR03_ReferenceIdentification VARCHAR(20)= '251E00000X';        
DECLARE @I_AttendingProvider_NM101_EntityIdentifierCode VARCHAR(2)= '71';         
DECLARE @I_AttendingProvider_NM102_EntityTypeQualifier VARCHAR(1)= '1';         
DECLARE @I_AttendingProvider_NM108_IDCodeQualifier VARCHAR(2)= 'XX';         
DECLARE @I_Claim_DTP01_01_DateTimeQualifier VARCHAR(3)= '434';         
DECLARE @I_Claim_DTP01_02_DateTimePeriodFormatQualifier VARCHAR(3)= 'RD8';         
DECLARE @I_Claim_DTP02_01_DateTimeQualifier VARCHAR(3)= '435';         
DECLARE @I_Claim_DTP02_02_DateTimePeriodFormatQualifier  VARCHAR(2)= 'DT';                         
         
        
         
IF EXISTS (SELECT TOP 1 * FROM PayorEdi837Settings WHERE PayorID=@PayorID)        
BEGIN -- update        
        
UPDATE PayorEdi837Settings        
SET        
    ISA06_InterchangeSenderId = @ISA06_InterchangeSenderId,        
    --BillingProvider_PRV01_ProviderCode =  @BillingProvider_PRV01_ProviderCode,        
    --BillingProvider_PRV02_ReferenceIdentificationQualifier = @BillingProvider_PRV02_ReferenceIdentificationQualifier,        
    BillingProvider_PRV03_ProviderTaxonomyCode = @BillingProvider_PRV03_ProviderTaxonomyCode,        
    ISA08_InterchangeReceiverId = @ISA08_InterchangeReceiverId,        
    ISA11_RepetitionSeparator = @ISA11_RepetitionSeparator,        
    ISA16_ComponentElementSeparator =@ISA16_ComponentElementSeparator,        
    ISA13_UpdatedDate = GETDATE()      
  where PayorID= @PayorID        
END        
ELSE -- insert        
BEGIN         
 Insert Into PayorEdi837Settings         
 (PayorID,        
  ISA01_AuthorizationInformationQualifier,ISA03_SecurityInformationQualifier,ISA05_InterchangeSenderIdQualifier,ISA06_InterchangeSenderId,        
  ISA07_InterchangeReceiverIdQualifier,ISA08_InterchangeReceiverId,ISA11_RepetitionSeparator,ISA12_InterchangeControlVersionNumber,    
  ISA14_AcknowledgementRequired,ISA15_UsageIndicator,ISA16_ComponentElementSeparator,        
  GS01_FunctionalIdentifierCode,      
  GS06_GroupControlNumber,GS07_ResponsibleAgencyCode,GS08_VersionOrReleaseOrNo,        
  ST01_TransactionSetIdentifier,ST02_TransactionSetControlNumber,ST03_ImplementationConventionReference,        
  BHT01_HierarchicalStructureCode,BHT02_TransactionSetPurposeCode,BHT06_TransactionTypeCode,        
  Submitter_NM101_EntityIdentifierCodeEnum,Submitter_NM102_EntityTypeQualifier,Submitter_NM108_IdCodeQualifier,        
  Submitter_EDIContact1_PER01_ContactFunctionCode,Submitter_EDIContact1_PER03_CommunicationNumberQualifier,Submitter_EDIContact1_PER05_CommunicationNumberQualifier,      
  Submitter_EDIContact1_PER06_CommunicationNumber  ,Submitter_EDIContact1_PER07_CommunicationNumberQualifier,      
  Receiver_NM101_EntityIdentifierCodeEnum,Receiver_NM102_EntityTypeQualifier,Receiver_NM108_IdCodeQualifier,        
  BillingProvider_HL01_HierarchicalIDNumber,BillingProvider_HL03_HierarchicalLevelCode,BillingProvider_HL04_HierarchicalChildCode,        
  BillingProvider_NM101_EntityIdentifierCode,BillingProvider_NM102_EntityTypeQualifier,BillingProvider_NM108_IdentificationCodeQualifier,        
  BillingProvider_REF01_ReferenceIdentificationQualifier,        
  Subscriber_HL02_HierarchicalParentIDNumber,Subscriber_HL03_HierarchicalLevelCode,Subscriber_HL04_HierarchicalChildCode,        
  Subscriber_SBR01_PayerResponsibilitySequenceNumberCode,Subscriber_SBR02_RelationshipCode,Subscriber_SBR09_ClaimFilingIndicatorCode,        
  Subscriber_NM101_EntityIdentifierCode,Subscriber_NM102_EntityIdentifierCode,Subscriber_NM108_IdentificationCodeQualifier,        
  Subscriber_DMG01_DateTimePeriodFormatQualifier,        
  Subscriber_Payer_NM101_EntityIdentifierCode,Subscriber_Payer_NM102_EntityTypeQualifier,Subscriber_Payer_NM108_IdentificationCodeQualifier,        
  Claim_CLM01_PatientAccountNumber,        
  Claim_CLM05_02_FacilityCodeQualifier,Claim_CLM05_03_ClaimFrequencyCode,        
  Claim_CLM06_ProviderSignatureOnFile,Claim_CLM07_ProviderAcceptAssignment,Claim_CLM08_AssignmentOfBenefitsIndicator,        
  Claim_CLM09_ReleaseOfInformationCode,        
  Claim_ServiecLine_LX01_AssignedNumber,        
  Claim_ServiecLine_SV101_01_ProductServiceIDQualifier,Claim_ServiecLine_SV103_BasisOfMeasurement,      
  Claim_ServiceLine_Date_DTP01_DateTimeQualifier,Claim_ServiceLine_Date_DTP02_DateTimePeriodFormatQualifier,        
  SegmentTerminator,ElementSeparator,SkipSituationalContent,        
  ISA13_InterchangeControlNo,RequiredRenderingProvider,        
  CheckForPolicyNumber,CMS1500HourRounding,UB04HourRounding,        
  BillingProvider_PRV01_ProviderCode,BillingProvider_PRV02_ReferenceIdentificationQualifier,BillingProvider_PRV03_ProviderTaxonomyCode,        
  Claim_Prior_REF01_ReferenceIdentificationQualifier,Claim_REF01_ReferenceIdentificationQualifier,      
  ReferringProvider_NM101_EntityIdentifierCode,ReferringProvider_NM102_EntityTypeQualifier,ReferringProvider_NM108_IDCodeQualifier,        
  RenderingProvider_NM101_EntityIdentifierCode,RenderingProvider_NM102_EntityTypeQualifier,RenderingProvider_NM108_IDCodeQualifier,        
  RenderingProvider_PRV01_ProviderCode,RenderingProvider_PRV02_ReferenceIdentificationQualifier,        
  I_GS08_ST03_VersionOrReleaseOrNo,I_PVR03_ReferenceIdentification,        
  I_AttendingProvider_NM101_EntityIdentifierCode,I_AttendingProvider_NM102_EntityTypeQualifier,I_AttendingProvider_NM108_IDCodeQualifier,        
  I_Claim_DTP01_01_DateTimeQualifier,I_Claim_DTP01_02_DateTimePeriodFormatQualifier,        
  I_Claim_DTP02_01_DateTimeQualifier,I_Claim_DTP02_02_DateTimePeriodFormatQualifier)        
 Values         
 (@PayorID,        
  @ISA01_AuthorizationInformationQualifier,@ISA03_SecurityInformationQualifier,@ISA05_InterchangeSenderIdQualifier,@ISA06_InterchangeSenderId,        
  @ISA07_InterchangeReceiverIdQualifier,@ISA08_InterchangeReceiverId,@ISA11_RepetitionSeparator,@ISA12_InterchangeControlVersionNumber,    
  @ISA14_AcknowledgementRequired,@ISA15_UsageIndicator,@ISA16_ComponentElementSeparator,        
  @GS01_FunctionalIdentifierCode,      
  @GS06_GroupControlNumber,@GS07_ResponsibleAgencyCode,@GS08_VersionOrReleaseOrNo,        
  @ST01_TransactionSetIdentifier,@ST02_TransactionSetControlNumber,@ST03_ImplementationConventionReference,        
  @BHT01_HierarchicalStructureCode,@BHT02_TransactionSetPurposeCode,@BHT06_TransactionTypeCode,        
  @Submitter_NM101_EntityIdentifierCodeEnum,@Submitter_NM102_EntityTypeQualifier,@Submitter_NM108_IdCodeQualifier,        
  @Submitter_EDIContact1_PER01_ContactFunctionCode,@Submitter_EDIContact1_PER03_CommunicationNumberQualifier,@Submitter_EDIContact1_PER05_CommunicationNumberQualifier,        
  @Submitter_EDIContact1_PER06_CommunicationNumber  ,@Submitter_EDIContact1_PER07_CommunicationNumberQualifier,      
  @Receiver_NM101_EntityIdentifierCodeEnum,@Receiver_NM102_EntityTypeQualifier,@Receiver_NM108_IdCodeQualifier,        
  @BillingProvider_HL01_HierarchicalIDNumber,@BillingProvider_HL03_HierarchicalLevelCode,@BillingProvider_HL04_HierarchicalChildCode,        
  @BillingProvider_NM101_EntityIdentifierCode,@BillingProvider_NM102_EntityTypeQualifier,@BillingProvider_NM108_IdentificationCodeQualifier,        
  @BillingProvider_REF01_ReferenceIdentificationQualifier,        
  @Subscriber_HL02_HierarchicalParentIDNumber,@Subscriber_HL03_HierarchicalLevelCode,@Subscriber_HL04_HierarchicalChildCode,        
  @Subscriber_SBR01_PayerResponsibilitySequenceNumberCode,@Subscriber_SBR02_RelationshipCode,@Subscriber_SBR09_ClaimFilingIndicatorCode,        
  @Subscriber_NM101_EntityIdentifierCode,@Subscriber_NM102_EntityIdentifierCode,@Subscriber_NM108_IdentificationCodeQualifier,        
  @Subscriber_DMG01_DateTimePeriodFormatQualifier,        
  @Subscriber_Payer_NM101_EntityIdentifierCode,@Subscriber_Payer_NM102_EntityTypeQualifier,@Subscriber_Payer_NM108_IdentificationCodeQualifier,        
  @Claim_CLM01_PatientAccountNumber,        
  @Claim_CLM05_02_FacilityCodeQualifier,@Claim_CLM05_03_ClaimFrequencyCode,        
  @Claim_CLM06_ProviderSignatureOnFile,@Claim_CLM07_ProviderAcceptAssignment,@Claim_CLM08_AssignmentOfBenefitsIndicator,        
  @Claim_CLM09_ReleaseOfInformationCode,      
  @Claim_ServiecLine_LX01_AssignedNumber,        
  @Claim_ServiecLine_SV101_01_ProductServiceIDQualifier,@Claim_ServiecLine_SV103_BasisOfMeasurement,      
  @Claim_ServiceLine_Date_DTP01_DateTimeQualifier,@Claim_ServiceLine_Date_DTP02_DateTimePeriodFormatQualifier,        
  @SegmentTerminator,@ElementSeparator,@SkipSituationalContent,        
  @ISA13_InterchangeControlNo,@RequiredRenderingProvider,        
  @CheckForPolicyNumber,@CMS1500HourRounding,@UB04HourRounding,        
  @BillingProvider_PRV01_ProviderCode,@BillingProvider_PRV02_ReferenceIdentificationQualifier,@BillingProvider_PRV03_ProviderTaxonomyCode,        
  @Claim_Prior_REF01_ReferenceIdentificationQualifier, @Claim_REF01_ReferenceIdentificationQualifier,      
  @ReferringProvider_NM101_EntityIdentifierCode,@ReferringProvider_NM102_EntityTypeQualifier,@ReferringProvider_NM108_IDCodeQualifier,        
  @RenderingProvider_NM101_EntityIdentifierCode,@RenderingProvider_NM102_EntityTypeQualifier,@RenderingProvider_NM108_IDCodeQualifier,        
  @RenderingProvider_PRV01_ProviderCode,@RenderingProvider_PRV02_ReferenceIdentificationQualifier,        
  @I_GS08_ST03_VersionOrReleaseOrNo,@I_PVR03_ReferenceIdentification,        
  @I_AttendingProvider_NM101_EntityIdentifierCode,@I_AttendingProvider_NM102_EntityTypeQualifier,@I_AttendingProvider_NM108_IDCodeQualifier,        
  @I_Claim_DTP01_01_DateTimeQualifier,@I_Claim_DTP01_02_DateTimePeriodFormatQualifier,        
  @I_Claim_DTP02_01_DateTimeQualifier,@I_Claim_DTP02_02_DateTimePeriodFormatQualifier)           
END        
        
SELECT 1; RETURN;         
      
END
