CREATE PROCEDURE [dbo].[HC_GetPayorBillingSetting]    
@PayorID BIGINT    
AS                                          
BEGIN                                          
 SELECT PayorEdi837SettingId,PayorID,    
 ISA06_InterchangeSenderId,ISA06_InterchangeSenderId,ISA08_InterchangeReceiverId,ISA11_RepetitionSeparator,ISA16_ComponentElementSeparator,      
 Submitter_NM103_NameLastOrOrganizationName,Submitter_NM109_IdCodeQualifierEnum,Submitter_EDIContact1_PER02_Name,Submitter_EDIContact1_PER04_CommunicationNumber,      
 Submitter_EDIContact1_PER08_CommunicationNumber,CMS1500HourRounding,UB04HourRounding,BillingProvider_PRV01_ProviderCode,BillingProvider_PRV02_ReferenceIdentificationQualifier,  
 BillingProvider_PRV03_ProviderTaxonomyCode  
 FROM PayorEdi837Settings WHERE PayorID=@PayorID    
 END
