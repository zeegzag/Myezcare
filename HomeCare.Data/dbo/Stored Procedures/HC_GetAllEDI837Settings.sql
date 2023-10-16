                 
CREATE PROCEDURE [dbo].[HC_GetAllEDI837Settings]                                  
@PayorID bigint ,                    
@searchtext varchar(100) = ''                     
AS                                                                
BEGIN                      
                     
DECLARE @colsUnpivot AS NVARCHAR(MAX), @query  AS NVARCHAR(MAX), @colsPivot as  NVARCHAR(MAX);                    
                    
select @colsUnpivot = stuff((select ','+  C.name +'= CASE WHEN '+C.name+' IS NULL THEN '''' ELSE CAST('+C.name+' as NVARCHAR(MAX)) END' --quotename(C.name)                    
        from sys.columns as C                    
        where C.object_id = object_id('PayorEdi837Settings')                  
  AND  C.name!='PayorEdi837SettingId' AND C.name!='Submitter_NM103_NameLastOrOrganizationName' AND C.name!='Submitter_NM109_IdCodeQualifierEnum'                 
  AND C.name!='Submitter_EDIContact1_PER02_Name' AND C.name!='Submitter_EDIContact1_PER04_CommunicationNumber'               
  AND C.name!='Submitter_EDIContact1_PER08_CommunicationNumber' AND C.name!='Submitter_EDIContact1_PER06_CommunicationNumber'                   
  AND C.name!='BHT04_Date' AND C.name!='BHT05_Time' AND C.name!='Claim_CLM01_PatientAccountNumber' AND C.name!='Claim_REF02_MedicalRecordNumber'                
  AND C.name!='Claim_ServiceLine_Ref_REF02_ReferenceIdentification' AND C.name!='Claim_ServiceLine_Ref_REF02_ReferenceIdentification_F8_02'                
  AND C.name!='Claim_ServiceLine_SV107_01_DiagnosisCodePointer' AND C.name!='GS04_Date' AND C.name!='GS05_Time' AND C.name!='GS06_GroupControlNumber'                
  AND C.name!='ISA09_InterchangeDate' AND C.name!='ISA10_InterchangeTime' AND C.name!='ST02_TransactionSetControlNumber'                
  AND C.name!='Subscriber_HL02_HierarchicalParentIDNumber' AND C.name!='Subscriber_NM109_SubscriberPrimaryIdentifier'                
  AND C.name!='UB04HourRounding' AND C.name!='CMS1500HourRounding' AND C.name!='ISA13_InterchangeControlNumber' AND C.name!='ISA13_UpdatedDate'                
  AND C.name!='Submitter_EDIContact2_PER01_ContactFunctionCode' AND C.name!='Submitter_EDIContact2_PER02_Name'               
  AND C.name!='Submitter_EDIContact2_PER03_CommunicationNumberQualifier'AND C.name!='Submitter_EDIContact2_PER04_CommunicationNumber'               
  AND C.name!='Submitter_EDIContact2_PER05_CommunicationNumberQualifier' AND C.name!='Submitter_EDIContact2_PER06_CommunicationNumber'               
  AND C.name!='Submitter_EDIContact2_PER07_CommunicationNumberQualifier' AND C.name!='Submitter_EDIContact2_PER08_CommunicationNumber'               
  AND C.name!='Submitter_EDIContact2_PER09_ContactInquiryReference' AND C.name!='Subscriber_Payer_NM109_IdentificationCode'         
  AND C.name!='Claim_ServiceFacility_NM101_EntityIdentifierCode' AND C.name!='Claim_ServiceFacility_NM102_EntityTypeQualifier'         
  AND C.name!='Claim_ServiceFacility_NM108_IdentificationCodeQualifier'         
  AND C.name!='SE01_NumberOfIncludedSegments' AND C.name!='SE02_TransactionSetControlNumber'        
  AND C.name!='GE01_NumberOfTransactionSetsIncluded' AND C.name!='GE02_GroupControlNumber'        
  AND C.name!='IEA01_NumberOfIncludedFunctionalGroups' AND C.name!='IEA02_InterchangeControlNumber'        
  AND C.name!='Claim_ServiceLine_Ref_REF01_ReferenceIdentificationQualifier'AND C.name!='Claim_ServiceLine_Ref_REF01_ReferenceIdentificationQualifier_F8_02'        
  AND C.name!='SkipSituationalContent' AND C.name!='CheckForPolicyNumber'         
  AND C.name!='Claim_RenderringProvider_NM01_EntityIdentifierCode' AND C.name!='Claim_RenderringProvider_NM02_EntityTypeQualifier'        
  AND C.name!='Claim_RenderringProvider_NM108_IdentificationCodeQualifier'         
  AND C.name!='Submitter_NM104_NameFirst' AND C.name!='Submitter_NM105_NameMiddle' AND C.name!='Submitter_NM106_NamePrefix'          
  AND C.name!='Submitter_NM107_NameSuffix' AND C.name!='Submitter_NM110_EntityRelationshipCode' AND C.name!='Submitter_NM111_EntityIdentifierCode'          
  AND C.name!='Submitter_NM112_NameLastOrOrganizationName' AND C.name!='Submitter_EDIContact1_PER09_ContactInquiryReference'        
  AND C.name!='Receiver_NM103_NameLastOrOrganizationName' AND C.name!='Receiver_NM104_NameFirst' AND C.name!='Receiver_NM105_NameMiddle'          
  AND C.name!='Receiver_NM106_NamePrefix' AND C.name!='Receiver_NM107_NameSuffix' AND C.name!='Receiver_NM109_IdCodeQualifierEnum'          
  AND C.name!='Receiver_NM110_EntityRelationshipCode' AND C.name!='Receiver_NM111_EntityIdentifierCode' AND C.name!='Receiver_NM112_NameLastOrOrganizationName'         
  AND C.name!='ST03_ImplementationConventionReference' AND C.name!='BHT03_ReferenceIdentification'        
  AND C.name!='I_PVR03_ReferenceIdentification' AND C.name!='Claim_CLM010_PatientSignatureSource'   --AND C.name !='RequiredRenderingProvider'       
        
        
   --and C.name <> 'PayorName'                    
    for xml path('')), 1, 1, '')                    
                    
select @colsPivot = stuff((select ','+   C.name --quotename(C.name)                    
        from sys.columns as C                    
        where C.object_id = object_id('PayorEdi837Settings')                 
  AND  C.name!='PayorID' AND C.name!='PayorEdi837SettingId'  AND C.name!='Submitter_NM103_NameLastOrOrganizationName' AND C.name!='Submitter_NM109_IdCodeQualifierEnum'                 
  AND C.name!='Submitter_EDIContact1_PER02_Name' AND C.name!='Submitter_EDIContact1_PER04_CommunicationNumber'               
  AND C.name!='Submitter_EDIContact1_PER08_CommunicationNumber'  AND C.name!='Submitter_EDIContact1_PER06_CommunicationNumber'                 
  AND C.name!='BHT04_Date' AND C.name!='BHT05_Time' AND C.name!='Claim_CLM01_PatientAccountNumber' AND C.name!='Claim_REF02_MedicalRecordNumber'                
  AND C.name!='Claim_ServiceLine_Ref_REF02_ReferenceIdentification' AND C.name!='Claim_ServiceLine_Ref_REF02_ReferenceIdentification_F8_02'                
  AND C.name!='Claim_ServiceLine_SV107_01_DiagnosisCodePointer' AND C.name!='GS04_Date' AND C.name!='GS05_Time' AND C.name!='GS06_GroupControlNumber'                
  AND C.name!='ISA09_InterchangeDate' AND C.name!='ISA10_InterchangeTime' AND C.name!='ST02_TransactionSetControlNumber'                
  AND C.name!='Subscriber_HL02_HierarchicalParentIDNumber' AND C.name!='Subscriber_NM109_SubscriberPrimaryIdentifier'                
  AND C.name!='UB04HourRounding' AND C.name!='CMS1500HourRounding' AND C.name!='ISA13_InterchangeControlNumber' AND C.name!='ISA13_UpdatedDate'                 
  AND C.name!='Submitter_EDIContact2_PER01_ContactFunctionCode' AND C.name!='Submitter_EDIContact2_PER02_Name'               
  AND C.name!='Submitter_EDIContact2_PER03_CommunicationNumberQualifier'AND C.name!='Submitter_EDIContact2_PER04_CommunicationNumber'               
  AND C.name!='Submitter_EDIContact2_PER05_CommunicationNumberQualifier' AND C.name!='Submitter_EDIContact2_PER06_CommunicationNumber'               
  AND C.name!='Submitter_EDIContact2_PER07_CommunicationNumberQualifier' AND C.name!='Submitter_EDIContact2_PER08_CommunicationNumber'               
  AND C.name!='Submitter_EDIContact2_PER09_ContactInquiryReference' AND C.name!='Subscriber_Payer_NM109_IdentificationCode'        
  AND C.name!='Claim_ServiceFacility_NM101_EntityIdentifierCode' AND C.name!='Claim_ServiceFacility_NM102_EntityTypeQualifier'         
  AND C.name!='Claim_ServiceFacility_NM108_IdentificationCodeQualifier'          
  AND C.name!='SE01_NumberOfIncludedSegments' AND C.name!='SE02_TransactionSetControlNumber'        
  AND C.name!='GE01_NumberOfTransactionSetsIncluded' AND C.name!='GE02_GroupControlNumber'        
  AND C.name!='IEA01_NumberOfIncludedFunctionalGroups' AND C.name!='IEA02_InterchangeControlNumber'        
  AND C.name!='Claim_ServiceLine_Ref_REF01_ReferenceIdentificationQualifier'AND C.name!='Claim_ServiceLine_Ref_REF01_ReferenceIdentificationQualifier_F8_02'        
  --AND C.name!='RequiredRenderingProvider'   
  AND C.name!='SkipSituationalContent' AND C.name!='CheckForPolicyNumber'         
  AND C.name!='Claim_RenderringProvider_NM01_EntityIdentifierCode' AND C.name!='Claim_RenderringProvider_NM02_EntityTypeQualifier'      AND C.name!='Claim_RenderringProvider_NM108_IdentificationCodeQualifier'         
  AND C.name!='Submitter_NM104_NameFirst' AND C.name!='Submitter_NM105_NameMiddle' AND C.name!='Submitter_NM106_NamePrefix'          
  AND C.name!='Submitter_NM107_NameSuffix' AND C.name!='Submitter_NM110_EntityRelationshipCode' AND C.name!='Submitter_NM111_EntityIdentifierCode'          
  AND C.name!='Submitter_NM112_NameLastOrOrganizationName' AND C.name!='Submitter_EDIContact1_PER09_ContactInquiryReference'        
  AND C.name!='Receiver_NM103_NameLastOrOrganizationName' AND C.name!='Receiver_NM104_NameFirst' AND C.name!='Receiver_NM105_NameMiddle'          
  AND C.name!='Receiver_NM106_NamePrefix' AND C.name!='Receiver_NM107_NameSuffix' AND C.name!='Receiver_NM109_IdCodeQualifierEnum'          
  AND C.name!='Receiver_NM110_EntityRelationshipCode' AND C.name!='Receiver_NM111_EntityIdentifierCode' AND C.name!='Receiver_NM112_NameLastOrOrganizationName'         
  AND C.name!='ST03_ImplementationConventionReference' AND C.name!='BHT03_ReferenceIdentification'        
  AND C.name!='I_PVR03_ReferenceIdentification' AND C.name!='Claim_CLM010_PatientSignatureSource'  --AND C.name !='RequiredRenderingProvider'       
   --and C.name <> 'PayorName'                    
     for xml path('')), 1, 1, '')                    
                    
                    
set @query                     
  = 'select  [Key], Val                    
from (                    
                    
SELECT '+@colsUnpivot+'                    
FROM PayorEdi837Settings                     
) as a                    
unpivot                    
(                    
 Val for [Key] in ('+@colsPivot+')                    
) as p                    
where payorid='+CONVERT(VARCHAR(100),@PayorID)                    
                    
--SET @query=                   
--'SELECT Distinct                   
--T.[Key],T.Val,T1.character_maximum_length AS CharLength                   
--FROM ('+@query+') AS T                   
--INNER JOIN information_schema.columns T1 on T1.column_name = T.[Key]                  
--WHERE T.[Key] LIKE ''%'+@searchtext+'%''  ';                    
                  
--SELECT  @query                    
--exec(@query)                  
                  
DECLARE @Temp TABLE(                                                                  
   ColumnName NVARCHAR(Max),                                                                  
   Value NVARCHAR(Max)                                                                
  )                    
                  
  
--   exec [HC_GetAllEDI837Settings] 1  ,''                  
PRINT @query  
  
insert into @Temp (ColumnName,Value)   
exec(@query)                  
        
                 
----- Declare For Getting length of Columns                
DECLARE @Temp_ForLength TABLE(                                                                  
   ColumnName NVARCHAR(Max),                                                                  
   CharLength INT                                                                  
  )                                                                  
INSERT INTO @Temp_ForLength (ColumnName,CharLength)                      
select column_name,character_maximum_length                      
from information_schema.columns                    
where table_name = 'PayorEdi837Settings'                  
              
          
----- Declare For Getting Description/ Comments of Columns                  
--DECLARE @Temp_ForComments TABLE(                                                                  
--   ColumnName NVARCHAR(Max),                                                                  
--   Description NVARCHAR(Max)                                                                
--  )         
                     
--insert into @Temp_ForComments (ColumnName,Description)        
--Select        
-- Convert(nvarchar(MAX),sc.name) as 'ColumnName',        
-- Convert(nvarchar(MAX),sep.value) as 'Description'        
--From         
--   sys.extended_properties sep        
--      Inner join        
--         sys.objects so        
--         On        
--            sep.major_id = so.object_id        
--      Left outer join        
--         sys.columns sc        
--         On        
--            so.object_id = sc.object_id and        
--            sep.minor_id = sc.column_id                       
--Where        
--   sep.name = 'MS_Description' and        
--   so.type = 'U';        
        
        
        
select T1.ColumnName AS [Key] ,T1.Value AS Val ,T2.CharLength --,T3.Description        
FROM @Temp T1                  
INNER JOIN @Temp_ForLength T2 ON T2.ColumnName =T1.ColumnName          
--LEFT JOIN @Temp_ForComments T3 ON T3.ColumnName =T1.ColumnName          
where T1.ColumnName like '%'+@searchtext+'%'         
                 
--ORDER BY T1.ColumnName                  
                                
END 