                    
-- EXEC HC_GetBatchRelatedAllData @BatchID = '80392', @FileType = 'CMS1500', @IsCaseManagement = 'False'                      
                      
                                                                                  
CREATE PROCEDURE [dbo].[HC_GetBatchRelatedAllData]                                                                                                                            
@BatchID as bigint ,                                                                            
@FileType as varchar(10)  ,                                          
@IsCaseManagement AS BIT                        
AS                      
                      
--select @BatchID = '1169', @FileType = 'CMS1500', @IsCaseManagement = 'False'                                                                                 
BEGIN                                                      
                                              
                                              
                                              
                                              
IF(@IsCaseManagement = 1)                                              
BEGIN                                               
 EXEC HC_CM_GetBatchRelatedAllData @BatchID,@FileType                                              
 PRINT 1;                                              
END                                              
ELSE                                               
BEGIN                                              
                                              
                    
Declare @TempNoteDXCodeMappings TABLE(                  
NoteID BIGINT,                  
BatchID BIGINT,                  
DxCodeType NVARCHAR(MAX),                  
DXCodeWithoutDot NVARCHAR(MAX),                  
Precedence int                  
)                  
INSERT INTO @TempNoteDXCodeMappings                  
SELECT DISTINCT NoteID,BatchID,DxCodeType,DXCodeWithoutDot,Precedence FROM NoteDXCodeMappings WHERE   (@BatchID IS NULL OR BatchID=@BatchID)                                         
                                              
                                                   
                        
              
DECLARE @SubmitterName varchar(max);                                                                      
DECLARE @SubmitterIdCode varchar(max);                                                                      
DECLARE @SubmitterContactName varchar(max);                                                                      
DECLARE @SubmitterContactPhone varchar(max);                                                                      
DECLARE @SubmitterContactEmail varchar(max);                                                          
DECLARE @SpecialProgramCode_CMS1500  VARCHAR(2);                                                        
SET @SpecialProgramCode_CMS1500='03'                                                        
DECLARE @MedicaidType VARCHAR(50); SET @MedicaidType = 'Medicaid'                                                                        
SELECT TOP 1                                                                      
@SubmitterName = Submitter_NM103_NameLastOrOrganizationName,                                                                      
@SubmitterIdCode = Submitter_NM109_IdCode,                                                                      
@SubmitterContactName = Submitter_EDIContact1_PER02_Name,                                                                      
@SubmitterContactPhone = Submitter_EDIContact1_PER04_CommunicationNumber,                                                                      
@SubmitterContactEmail = Submitter_EDIContact1_PER08_CommunicationNumber                                                                            
FROM OrganizationSettings                                                                      
                                                                       
                                                                          
DECLARE @PayorID bigint;                                                                       
SELECT @PayorID=PayorID FROM Batches where BatchID=@BatchID;                                                                                  
                                                                   
                                                                      
SELECT Claim_ServiceLine_Ref_REF01_ReferenceIdentificationQualifier_F8_02,*,                                                                       
Submitter_NM103_NameLastOrOrganizationName = @SubmitterName,                                                                      
Submitter_NM109_IdCodeQualifierEnum = @SubmitterIdCode,                                                                      
Submitter_EDIContact1_PER02_Name=@SubmitterContactName,                                                                      
Submitter_EDIContact1_PER04_CommunicationNumber=@SubmitterContactPhone,                                                                      
Submitter_EDIContact1_PER08_CommunicationNumber=@SubmitterContactEmail               
FROM PayorEdi837Settings Where PayorID=@PayorID                                                                                   
                                                                            
                                         
IF EXISTS (SELECT 1 FROM Batches WHERE BatchID=@BatchID AND IsSent=0)           
BEGIN                                                                                  
                                  
                                                                               
 SELECT                                                                              
 B.BatchID,B.BatchTypeID, BN.BatchNoteID, R.ReferralID,R.FirstName, R.LastName,Dob=CONVERT(VARCHAR(10), R.Dob, 112), R.Gender , RPM.BeneficiaryNumber as 'AHCCCSID',R.CISNumber,                                                                           
       
 MedicalRecordNumber=RPM.BeneficiaryNumber, R.PolicyNumber, AdmissionDate=CONVERT(VARCHAR(12), R.CreatedDate, 112)+REPLACE(CONVERT(VARCHAR(5), R.CreatedDate, 8),':',''),                                                                              
 SubscriberID=RPM.BeneficiaryNumber, ClaimSubmitterIdentifier=RPM.BeneficiaryNumber, PatientAccountNumber=RPM.BeneficiaryNumber,                
 StrBathNoteID='N'+CONVERT(VARCHAR,BN.BatchNoteID),                                                    
 StrNoteID=CONVERT(VARCHAR,BN.NoteID),                                                    
 -- Null Address,Null City,Null State,null ZipCode,                                                                            
 C.Address,C.City,C.State,C.ZipCode,                                                                            
 ModifierName = CASE WHEN SC.ModifierID IS NULL THEN '' ELSE                                                                            
      STUFF((SELECT TOP 4 ',' + convert(varchar(500),M.ModifierCode, 120)FROM Modifiers M                                                                                          
      WHERE M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID)) FOR XML PATH (''))  , 1, 1, '')                                                          
      END,                                                                                        
 ContinuedDX = (SELECT  STUFF((SELECT TOP 12 ',' +  F.DxCodeType+':'+F.DXCodeWithoutDot +':' + convert(varchar,F.Precedence) --+'|'+                                                                                   
       FROM @TempNoteDXCodeMappings F where F.NoteID=N.NoteID AND (F.BatchID = @BatchID) Order BY F.Precedence ASC                                                                                      
       FOR XML PATH('')),1,1,'')),                                                                            
 --RBS.SpecialProgramCode_CMS1500                                                        
 @SpecialProgramCode_CMS1500 as SpecialProgramCode_CMS1500                                                        
 ,RBM.AuthorizationCode,                                                                    
 POS_CMS1500=DDM_P_FC.Value,                        
 --POS_CMS1500=RBM.FacilityCode,                        
 POS_UB04=DDM_I_FC.Value,                               
 AdmissionTypeCode_UB04 = DDM_AT.Value,AdmissionSourceCode_UB04 = DDM_AS.Value,PatientStatusCode_UB04 = DDM_PS.Value,                                                                    
 PH.PhysicianID,PH.NPINumber as PhysicianNPINumber,PH.FirstName as PhysicianFirstName,PH.LastName as PhysicianLastName,                                                                            
 ServiceDateSpan=CONVERT(VARCHAR(10), N.ServiceDate, 112)+'-'+CONVERT(VARCHAR(10), N.ServiceDate, 112),    N.NoteID,N.ServiceDate,N.ServiceCodeID,N.ServiceCode,N.PosID,                                                                            
 CalculatedUnit=BN.CLM_UNIT,                                                              
 CalculatedAmount=BN.CLM_BilledAmount,                 
               
 -- Capture Amount in Batch - Should same as Initial Note Amount                              
CLM_BilledAmount =  CONVERT(DECIMAL(10,2),    CASE WHEN BN.CLM_BilledAmount IS NULL OR LEN(BN.CLM_BilledAmount)=0 THEN '0' ELSE BN.CLM_BilledAmount END ) ,                               
-- Payor Allowed Amount Per Service Line Item                              
AMT01_ServiceLineAllowedAmount_AllowedAmount =  CONVERT(DECIMAL(10,2), CASE WHEN BN.AMT01_ServiceLineAllowedAmount_AllowedAmount IS NULL             
OR LEN(BN.AMT01_ServiceLineAllowedAmount_AllowedAmount)=0  THEN '0' ELSE BN.AMT01_ServiceLineAllowedAmount_AllowedAmount END ),                              
-- Payor Paid Amount Per Service Line Item                              
SVC03_LineItemProviderPaymentAmoun_PaidAmount = CONVERT(DECIMAL(10,2),    CASE WHEN BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount IS NULL OR             
LEN(BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount)=0                               
THEN '0' ELSE BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount END),    
  
  
MPP_AdjustmentAmount =  CONVERT(DECIMAL(10,2),    CASE WHEN BN.MPP_AdjustmentAmount IS NULL OR LEN(BN.MPP_AdjustmentAmount)=0 THEN '0' ELSE BN.MPP_AdjustmentAmount END),    
              
 N.BillingProviderID,                                                                      
 N.BillingProviderName,N.BillingProviderFirstName,N.BillingProviderAddress,N.BillingProviderCity,                                                                
N.BillingProviderState,N.BillingProviderZipcode,N.BillingProviderEIN,N.BillingProviderNPI,                                                                      
 N.BillingProviderGSA,N.BillingProviderAHCCCSID,N.RenderingProviderID,N.RenderingProviderName,N.RenderingProviderFirstName,                                                                            
 N.RenderingProviderAddress,N.RenderingProviderCity,N.RenderingProviderState,N.RenderingProviderZipcode,N.RenderingProviderEIN,N.RenderingProviderNPI,                                                        
 N.RenderingProviderGSA,N.RenderingProviderAHCCCSID,                                                                            
 P.PayorID,P.PayorName,PayorShortName = P.ShortName,PayorAddress =P.Address,PayorCity=P.City,PayorState=P.StateCode,PayorZipcode=P.Zipcode,                                                                        
 PayorIdentificationNumber=P.PayorIdentificationNumber,   P.PayorBillingType ,                                     
 --N.PayorID,N.PayorName,N.PayorShortName,N.PayorAddress,N.PayorIdentificationNumber,N.PayorCity,N.PayorState,N.PayorZipcode,                                                                    
 N.RandomGroupID,                                      
 RBM.BillingUnitLimit                                                      
 ,DDM_R.Value as RevenueCode,                                                                            
 GroupIDForMileServices=CASE WHEN LEN(N.GroupIDForMileServices)=0 OR N.GroupIDForMileServices IS NULL THEN CONVERT(VARCHAR(100),NEWID()) ELSE N.GroupIDForMileServices END,                                                                                 
 BN.Submitted_ClaimSubmitterIdentifier, BN.Submitted_ClaimAdjustmentTypeID, BN.Original_ClaimSubmitterIdentifier, BN.Original_PayerClaimControlNumber,                                                                
 BN.ClaimAdjustmentReason, CAST(1 as bit) IsUseInBilling, N.StartTime,N.EndTime , N.RenderingProvider_TaxonomyCode,                             
 N.SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName,N.SupervisingProvidername2420DLoop_NM104_NameFirst,N.SupervisingProvidername2420DLoop_REF02_ReferenceId                                     
                                  
 FROM Batches B                                      
 INNER JOIN BatchNotes BN ON BN.BatchID=B.BatchID                                                                                  
 INNER JOIN Notes N ON N.NoteID=BN.NoteID                                                                   
 INNER JOIN Payors P ON P.PayorID=N.PayorID                                                                                  
                                                       
 INNER JOIN Referrals R on R.ReferralID=N.ReferralID                   
 LEFT JOIN ReferralBillingSettings RBS ON RBS.ReferralID=R.ReferralID                                        
 LEFT JOIN (SELECT DISTINCT  ReferralID, PhysicianID FROM ReferralPhysicians) As RP on RP.ReferralID = R.ReferralID                                                                       
                                           
                                              
 LEFT JOIN Physicians PH on PH.PhysicianID = RP.PhysicianID                                                                                
 LEFT JOIN ContactMappings CM on CM.ReferralID=R.ReferralID AND CM.ContactTypeID=1 --AND (CM.IsDCSLegalGuardian=1 OR CM.IsPrimaryPlacementLegalGuardian=1)                                                                                  
 LEFT JOIN Contacts C on C.ContactID=CM.ContactID                                                                                
 LEFT JOIN ServiceCodes SC ON SC.ServiceCodeID=N.ServiceCodeID                                                         
 --INNER JOIN                                                      
 --(                                                      
 --SELECT BillingUnitLimit, PayorServiceCodeMappingID AS ID,NULL ReferralID,'' Type,POSStartDate StartDate,POSEndDate EndDate,IsDeleted,RevenueCode,'' AuthorizationCode                                                       
 --FROM PayorServiceCodeMapping WHERE PayorServiceCodeMapping.PayorID=@PayorID                                                      
 -- UNION ALL                                                      
 --SELECT BillingUnitLimit, ReferralBillingAuthorizationID AS ID,ReferralID,Type,StartDate,EndDate,IsDeleted,RevenueCode,AuthorizationCode FROM ReferralBillingAuthorizations WHERE ReferralBillingAuthorizations.PayorID=@PayorID                             
 
    
      
        
          
            
              
                 
                 
                     
                      
                        
                         
--) PSM ON PSM.ID = N.PayorServiceCodeMappingID OR (                                                      
 --PSM.ReferralID=R.ReferralID AND PSM.Type='CMS1500' AND PSM.IsDeleted=0 and (CONVERT(date,N.ServiceDate) between PSM.StartDate and PSM.EndDate))                                                      
                                                      
 Inner JOIN ReferralBillingAuthorizations RBM on RBM.ReferralID=R.ReferralID AND RBM.Type='CMS1500' AND RBM.IsDeleted=0 and (CONVERT(date,N.ServiceDate) between RBM.StartDate and RBM.EndDate) and RBM.PayorID=@PayorID                                       
  
    
      
        
          
            
              
                
 Inner Join ScheduleMasters SM on SM.ReferralBillingAuthorizationID=RBM.ReferralBillingAuthorizationID                                                       
 Inner Join EmployeeVisits EV on EV.ScheduleID=sm.ScheduleID and EV.EmployeeVisitID=N.EmployeeVisitID                                                       
 --INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID                                                         
                                                      
--inner JOIN ReferralBillingAuthorizationServiceCodes RBAS ON RBM.ReferralBillingAuthorizationID = RBAS.ReferralBillingAuthorizationID  and RBAS.ServiceCodeID=SC.ServiceCodeID and RBAS.isDeleted=0                                                      
 LEFT JOIN DDMaster DDM_R on DDM_R.DDMasterID=RBM.RevenueCode                                                                               
-- LEFT JOIN DDMaster DDM_P_FC on DDM_P_FC.DDMasterID=RBS.POS_CMS1500                      
LEFT JOIN DDMaster DDM_P_FC on DDM_P_FC.DDMasterID=RBM.FacilityCode                    
 LEFT JOIN DDMaster DDM_I_FC on DDM_I_FC.DDMasterID=RBS.POS_UB04                                                        
 LEFT JOIN DDMaster DDM_AT on DDM_AT.DDMasterID=RBS.AdmissionTypeCode_UB04                                                                             
 LEFT JOIN DDMaster DDM_AS on DDM_AS.DDMasterID=RBS.AdmissionSourceCode_UB04                                                                               
 LEFT JOIN DDMaster DDM_PS on DDM_PS.DDMasterID=RBS.PatientStatusCode_UB04                                                          
 LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID  and RPM.isDeleted=0  and RPM.PayorID=@PayorID                                                      
 LEFT JOIN DDMaster DM ON DM.DDMasterID = RPM.BeneficiaryTypeID AND DM.Title = @MedicaidType                                                          
                                       
 WHERE B.BatchID=@BatchID AND  (BN.IsFirstTimeClaimInBatch IS NULL OR BN.IsFirstTimeClaimInBatch=1)-- AND BN.IsUseInBilling=1                    
 and (RBM.isDeleted=0 or RBM.IsDeleted is Null)                                                      
                                                                
END                                                                                  
ELSE IF EXISTS (SELECT 1 FROM Batches WHERE BatchID=@BatchID AND IsSent=1)                                                                                  
BEGIN                      
              
              
              
SELECT * FROM (                                                                  
 SELECT                                                                             
 B.BatchID,B.BatchTypeID, BN.BatchNoteID, R.ReferralID,R.FirstName, R.LastName,Dob=CONVERT(VARCHAR(10), R.Dob, 112), R.Gender , RPM.BeneficiaryNumber as 'AHCCCSID'                                                        
 --R.AHCCCSID                                                        
 ,R.CISNumber,                                                                            
 MedicalRecordNumber=RPM.BeneficiaryNumber,                                                        
 --MedicalRecordNumber=R.AHCCCSID,                                                        
  R.PolicyNumber, AdmissionDate=CONVERT(VARCHAR(12), R.CreatedDate, 112)+REPLACE(CONVERT(VARCHAR(5), R.CreatedDate, 8),':',''),                                                         
 SubscriberID=RPM.BeneficiaryNumber, ClaimSubmitterIdentifier=RPM.BeneficiaryNumber, PatientAccountNumber=RPM.BeneficiaryNumber,                
 StrBathNoteID='N'+CONVERT(VARCHAR,BN.BatchNoteID),                                                                   
 StrNoteID=CONVERT(VARCHAR,BN.NoteID),                                                    
 C.Address,C.City,C.State,C.ZipCode,                                                                            
 ModifierName = CASE WHEN SC.ModifierID IS NULL THEN '' ELSE                                                                            
    STUFF((SELECT TOP 4 ',' + convert(varchar(500),M.ModifierCode, 120)FROM Modifiers M                                                                   
    WHERE M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID)) FOR XML PATH (''))  , 1, 1, '')                                                                                      
    END,                                                                                        
 ContinuedDX = (SELECT  STUFF((SELECT TOP 12 ',' +  F.DxCodeType+':'+F.DXCodeWithoutDot +':' + convert(varchar,F.Precedence) --+'|'+                                                                                   
       FROM @TempNoteDXCodeMappings F where F.NoteID=N.NoteID Order BY F.Precedence ASC                                                                                      
       FOR XML PATH('')),1,1,'')),                                                                            
 --RBS.SpecialProgramCode_CMS1500                                                        
 @SpecialProgramCode_CMS1500 AS SpecialProgramCode_CMS1500                                                        
 ,RBM.AuthorizationCode,                                                          
 POS_CMS1500=DDM_P_FC.Value,                        
 --POS_CMS1500=RBM.FacilityCode,                        
 POS_UB04=DDM_I_FC.Value,                                                                    
 AdmissionTypeCode_UB04 = DDM_AT.Value,AdmissionSourceCode_UB04 = DDM_AS.Value,PatientStatusCode_UB04 = DDM_PS.Value,                                                                    
 PH.PhysicianID,PH.NPINumber as PhysicianNPINumber,PH.FirstName as PhysicianFirstName,PH.LastName as PhysicianLastName,                                                           
 ServiceDateSpan=CONVERT(VARCHAR(10), N.ServiceDate, 112)+'-'+CONVERT(VARCHAR(10), N.ServiceDate, 112), N.NoteID,    N.ServiceDate,N.ServiceCodeID,N.ServiceCode,N.PosID,                                                                            
 CalculatedUnit=BN.CLM_UNIT,                                                              
 CalculatedAmount=BN.CLM_BilledAmount,                
               
-- Capture Amount in Batch - Should same as Initial Note Amount                              
CLM_BilledAmount =  CONVERT(DECIMAL(10,2),    CASE WHEN BN.CLM_BilledAmount IS NULL OR LEN(BN.CLM_BilledAmount)=0 THEN '0' ELSE BN.CLM_BilledAmount END ) ,                               
-- Payor Allowed Amount Per Service Line Item                              
AMT01_ServiceLineAllowedAmount_AllowedAmount =  CONVERT(DECIMAL(10,2), CASE WHEN BN.AMT01_ServiceLineAllowedAmount_AllowedAmount IS NULL             
OR LEN(BN.AMT01_ServiceLineAllowedAmount_AllowedAmount)=0  THEN '0' ELSE BN.AMT01_ServiceLineAllowedAmount_AllowedAmount END ),                              
-- Payor Paid Amount Per Service Line Item                              
SVC03_LineItemProviderPaymentAmoun_PaidAmount = CONVERT(DECIMAL(10,2),    CASE WHEN BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount IS NULL OR LEN(BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount)=0                               
THEN '0' ELSE BN.SVC03_LineItemProviderPaymentAmoun_PaidAmount END),     
  
MPP_AdjustmentAmount =  CONVERT(DECIMAL(10,2),    CASE WHEN BN.MPP_AdjustmentAmount IS NULL OR LEN(BN.MPP_AdjustmentAmount)=0 THEN '0' ELSE BN.MPP_AdjustmentAmount END),    
              
 N.BillingProviderID,                                                                      
 N.BillingProviderName,N.BillingProviderFirstName,N.BillingProviderAddress,N.BillingProviderCity,                                                                
 N.BillingProviderState,N.BillingProviderZipcode,N.BillingProviderEIN,N.BillingProviderNPI,                                                                        
 N.BillingProviderGSA,N.BillingProviderAHCCCSID,N.RenderingProviderID,N.RenderingProviderName,N.RenderingProviderFirstName,                                                                            
 N.RenderingProviderAddress,N.RenderingProviderCity,N.RenderingProviderState,N.RenderingProviderZipcode,N.RenderingProviderEIN,N.RenderingProviderNPI,                                                                    
 N.RenderingProviderGSA,N.RenderingProviderAHCCCSID,                                                        
 P.PayorID,P.PayorName,PayorShortName = P.ShortName,PayorAddress =P.Address,PayorCity=P.City,PayorState=P.StateCode,PayorZipcode=P.Zipcode,                                                  
 PayorIdentificationNumber=P.PayorIdentificationNumber, P.PayorBillingType  ,                                       
 --N.PayorID,N.PayorName,N.PayorShortName,N.PayorAddress,N.PayorIdentificationNumber,N.PayorCity,N.PayorState,N.PayorZipcode,                                                                          
 N.RandomGroupID,                                                      
 RBM.BillingUnitLimit,                                                      
 DDM_R.Value as RevenueCode,                                                                    
 GroupIDForMileServices=CASE WHEN LEN(N.GroupIDForMileServices)=0 OR N.GroupIDForMileServices IS NULL THEN CONVERT(VARCHAR(100),NEWID()) ELSE N.GroupIDForMileServices END,                                                                                 
 BN.Submitted_ClaimSubmitterIdentifier, BN.Submitted_ClaimAdjustmentTypeID, BN.Original_ClaimSubmitterIdentifier, BN.Original_PayerClaimControlNumber,                                                                            
 BN.ClaimAdjustmentReason, CAST(1 as bit) IsUseInBilling, N.StartTime,N.EndTime  , N.RenderingProvider_TaxonomyCode,                                  
 N.SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName,N.SupervisingProvidername2420DLoop_NM104_NameFirst,N.SupervisingProvidername2420DLoop_REF02_ReferenceId       ,              
RowNumber = ROW_NUMBER() OVER ( PARTITION BY BN.NoteID Order BY BN.BatchID DESC, BN.MarkAsLatest DESC, BN.BatchNoteID DESC)                                         
 FROM Batches B                                                    
 INNER JOIN BatchNotes BN ON BN.BatchID=B.BatchID                                                                                  
 INNER JOIN BatchNoteDetails N ON N.NoteID=BN.NoteID                                                                   
 INNER JOIN Payors P ON P.PayorID=N.PayorID                                                                                  
                                                                
 INNER JOIN Referrals R on R.ReferralID=N.ReferralID                                              
 LEFT JOIN ReferralBillingSettings RBS ON RBS.ReferralID=R.ReferralID                                                                                  
 LEFT JOIN Physicians PH on PH.PhysicianID = R.PhysicianID                                                                              
 LEFT JOIN ContactMappings CM on CM.ReferralID=R.ReferralID AND CM.ContactTypeID=1 --AND (CM.IsDCSLegalGuardian=1 OR CM.IsPrimaryPlacementLegalGuardian=1)                                                                                 
 LEFT JOIN Contacts C on C.ContactID=CM.ContactID                                                                                  
 LEFT JOIN ServiceCodes SC ON SC.ServiceCodeID=N.ServiceCodeID                                                          
                                                 
 Inner JOIN ReferralBillingAuthorizations RBM on RBM.ReferralID=R.ReferralID AND RBM.Type='CMS1500' AND RBM.IsDeleted=0 and (CONVERT(date,N.ServiceDate) between RBM.StartDate and RBM.EndDate) and RBM.PayorID=@PayorID                                      
  
     
      
       
          
            
               
                
  Inner Join ScheduleMasters SM on SM.ReferralBillingAuthorizationID=RBM.ReferralBillingAuthorizationID                                                       
 Inner Join EmployeeVisits EV on EV.ScheduleID=sm.ScheduleID and EV.EmployeeVisitID=N.EmployeeVisitID                                                       
                                                
 LEFT JOIN DDMaster DDM_R on DDM_R.DDMasterID=RBM.RevenueCode                                                      
 LEFT JOIN DDMaster DDM_P_FC on DDM_P_FC.DDMasterID=RBM.FacilityCode                                                                             
 LEFT JOIN DDMaster DDM_I_FC on DDM_I_FC.DDMasterID=RBS.POS_UB04                                                                                
 LEFT JOIN DDMaster DDM_AT on DDM_AT.DDMasterID=RBS.AdmissionTypeCode_UB04                                                                              
 LEFT JOIN DDMaster DDM_AS on DDM_AS.DDMasterID=RBS.AdmissionSourceCode_UB04                                                                               
 LEFT JOIN DDMaster DDM_PS on DDM_PS.DDMasterID=RBS.PatientStatusCode_UB04                                                                       
 LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID  and RPM.isDeleted=0  and rpm.PayorID=@PayorID                                                      
 LEFT JOIN DDMaster DM ON DM.DDMasterID = RPM.BeneficiaryTypeID AND DM.Title = @MedicaidType                                                          
                           
 WHERE B.BatchID=@BatchID  --AND BN.IsUseInBilling=1         --AND  (BN.IsFirstTimeClaimInBatch IS NULL OR BN.IsFirstTimeClaimInBatch=1) -- AND BN.IsUseInBilling=1                     
 and (RBM.isDeleted=0 or RBM.IsDeleted is Null)                   
               
 ) AS TEMP  WHERE RowNumber = 1                 
 -- EXEC HC_GetBatchRelatedAllData @BatchID = '80336', @FileType = 'CMS1500', @IsCaseManagement = 'False'                      
                                                                     
END                        
                                                                   
SELECT FileName= REPLACE(P.ShortName,' ','_')                                                         
+'_'+ REPLACE(CONVERT (CHAR(10),B.StartDate, 101),'/','')                                                                                  
+'_'+ REPLACE(CONVERT (CHAR(10),B.EndDate, 101),'/','')                                                                                      
+'_'+ BatchTypeShort                                                                                  
+'_'+ CONVERT(VARCHAR(MAX),B.BatchID)                                                                                  
FROM Batches B                                                        INNER JOIN Payors P ON P.PayorID=B.PayorID                                                                                  
INNER JOIN BatchTypes BT ON BT.BatchTypeID=B.BatchTypeID                                                                                  
WHERE B.BatchID=@BatchID                                                        
                                                    
                                             
                                                        
DECLARE @CurScheduleID bigint;                                                       
DECLARE eventCursor CURSOR FORWARD_ONLY FOR                                                    
 SELECT DISTINCT                                                    
   EV.ScheduleID                                                    
 FROM BatchNoteDetails BND                                                    
 INNER JOIN Notes N                                             
   ON N.NoteID = BND.NoteID                                                    
   AND N.IsDeleted = 0                                                    
 INNER JOIN EmployeeVisits EV                                                    
   ON EV.IsDeleted = 0                                                    
   AND EV.EmployeeVisitID = N.EmployeeVisitID                                                    
 WHERE                                                    
   BND.BatchID = @BatchID                                                    
   AND BND.IsDeleted = 0;                                                    
                                                    
OPEN eventCursor;                                         
FETCH NEXT FROM eventCursor INTO @CurScheduleID;            
WHILE @@FETCH_STATUS = 0 BEGIN                                                    
    EXEC [dbo].[ScheduleEventBroadcast] 'BillingSchedule', @CurScheduleID,'',''                                                    
    FETCH NEXT FROM eventCursor INTO @CurScheduleID;                                                    
END;                                                    
CLOSE eventCursor;                                                    
DEALLOCATE eventCursor;                                                    
                                              
                                              
END                 
                                                   
END 