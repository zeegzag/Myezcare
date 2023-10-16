    
/*      
Modified By : Pallav Saxena      
Modified Date : 11/08/2019      
Purpose: 500 ERROR on Claimss generation      
      
Modified By : Pallav Saxena      
Modified Date : 11/14/2019      
Purpose: Duplicate Claims generated due to authorization code , changed the left join to inner join on ReferralBillingAuthorizationCode      
      
*/          
-- EXEC HC_GetBatchRelatedAllData @BatchID = '29',@FileType = 'CMS1500'                                  
-- EXEC HC_GetBatchRelatedAllData @BatchID = '142',@FileType = 'UB04'                                  
CREATE Procedure [dbo].[HC_GetBatchRelatedAllData]                                
@BatchID as bigint ,                            
@FileType as varchar(10)                            
AS                                  
BEGIN                                  
                     
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
                      
                      
SELECT *,                       
Submitter_NM103_NameLastOrOrganizationName = @SubmitterName,                      
Submitter_NM109_IdCodeQualifierEnum = @SubmitterIdCode,                      
Submitter_EDIContact1_PER02_Name=@SubmitterContactName,                      
Submitter_EDIContact1_PER04_CommunicationNumber=@SubmitterContactPhone,                      
Submitter_EDIContact1_PER08_CommunicationNumber=@SubmitterContactEmail                      
FROM PayorEdi837Settings Where PayorID=@PayorID                                   
                            
                            
IF EXISTS (SELECT 1 FROM Batches WHERE BatchID=@BatchID AND IsSent=0)                                  
BEGIN                                  
                                  
 SELECT                              
 B.BatchID,B.BatchTypeID, BN.BatchNoteID, BN.NoteID,R.ReferralID,R.FirstName, R.LastName,Dob=CONVERT(VARCHAR(10), R.Dob, 112), R.Gender , RPM.BeneficiaryNumber as 'AHCCCSID',R.CISNumber,                            
 MedicalRecordNumber=RPM.BeneficiaryNumber, R.PolicyNumber, AdmissionDate=CONVERT(VARCHAR(12), R.CreatedDate, 112)+REPLACE(CONVERT(VARCHAR(5), R.CreatedDate, 8),':',''),                              
 SubscriberID=RPM.BeneficiaryNumber, ClaimSubmitterIdentifier=RPM.BeneficiaryNumber, PatientAccountNumber=RPM.BeneficiaryNumber,StrBathNoteID='N'+CONVERT(VARCHAR,BN.BatchNoteID),                             
 -- Null Address,Null City,Null State,null ZipCode,                            
 C.Address,C.City,C.State,C.ZipCode,                            
 ModifierName = CASE WHEN SC.ModifierID IS NULL THEN '' ELSE                            
      STUFF((SELECT TOP 4 ',' + convert(varchar(500),M.ModifierCode, 120)FROM Modifiers M                                          
      WHERE M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID)) FOR XML PATH (''))  , 1, 1, '')          
      END,                                        
 ContinuedDX = (SELECT  STUFF((SELECT TOP 12 ',' +  F.DxCodeType+':'+F.DXCodeWithoutDot +':' + convert(varchar,F.Precedence) --+'|'+                                   
       FROM NoteDXCodeMappings F where F.NoteID=N.NoteID Order BY F.Precedence ASC                                      
       FOR XML PATH('')),1,1,'')),                            
 --RBS.SpecialProgramCode_CMS1500        
 @SpecialProgramCode_CMS1500 as SpecialProgramCode_CMS1500        
 ,RBM.AuthorizationCode,                    
 POS_CMS1500=DDM_P_FC.Value,POS_UB04=DDM_I_FC.Value,                    
 AdmissionTypeCode_UB04 = DDM_AT.Value,AdmissionSourceCode_UB04 = DDM_AS.Value,PatientStatusCode_UB04 = DDM_PS.Value,                    
 PH.PhysicianID,PH.NPINumber as PhysicianNPINumber,PH.FirstName as PhysicianFirstName,PH.LastName as PhysicianLastName,                            
 ServiceDateSpan=CONVERT(VARCHAR(10), N.ServiceDate, 112)+'-'+CONVERT(VARCHAR(10), N.ServiceDate, 112),    N.NoteID,N.ServiceDate,N.ServiceCodeID,N.ServiceCode,N.PosID,                            
 CalculatedUnit=BN.CLM_UNIT,              
 CalculatedAmount=BN.CLM_BilledAmount,              
 N.BillingProviderID,                      
 N.BillingProviderName,N.BillingProviderFirstName,N.BillingProviderAddress,N.BillingProviderCity,                
 N.BillingProviderState,N.BillingProviderZipcode,N.BillingProviderEIN,N.BillingProviderNPI,                      
 N.BillingProviderGSA,N.BillingProviderAHCCCSID,N.RenderingProviderID,N.RenderingProviderName,N.RenderingProviderFirstName,                            
 N.RenderingProviderAddress,N.RenderingProviderCity,N.RenderingProviderState,N.RenderingProviderZipcode,N.RenderingProviderEIN,N.RenderingProviderNPI,                    
 N.RenderingProviderGSA,N.RenderingProviderAHCCCSID,                            
 P.PayorID,P.PayorName,PayorShortName = P.ShortName,PayorAddress =P.Address,PayorCity=P.City,PayorState=P.StateCode,PayorZipcode=P.Zipcode,                        
 PayorIdentificationNumber=P.PayorIdentificationNumber,                                 
 --N.PayorID,N.PayorName,N.PayorShortName,N.PayorAddress,N.PayorIdentificationNumber,N.PayorCity,N.PayorState,N.PayorZipcode,                    
 N.RandomGroupID,      
 RBM.BillingUnitLimit      
 ,DDM_R.Value as RevenueCode,                            
 GroupIDForMileServices=CASE WHEN LEN(N.GroupIDForMileServices)=0 OR N.GroupIDForMileServices IS NULL THEN CONVERT(VARCHAR(100),NEWID()) ELSE N.GroupIDForMileServices END,                                 
 BN.Submitted_ClaimSubmitterIdentifier, BN.Submitted_ClaimAdjustmentTypeID, BN.Original_ClaimSubmitterIdentifier, BN.Original_PayerClaimControlNumber,                            
 BN.ClaimAdjustmentReason, BN.IsUseInBilling, N.StartTime,N.EndTime                            
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
 LEFT JOIN DDMaster DDM_P_FC on DDM_P_FC.DDMasterID=RBS.POS_CMS1500                             
 LEFT JOIN DDMaster DDM_I_FC on DDM_I_FC.DDMasterID=RBS.POS_UB04                             
 LEFT JOIN DDMaster DDM_AT on DDM_AT.DDMasterID=RBS.AdmissionTypeCode_UB04                             
 LEFT JOIN DDMaster DDM_AS on DDM_AS.DDMasterID=RBS.AdmissionSourceCode_UB04                               
 LEFT JOIN DDMaster DDM_PS on DDM_PS.DDMasterID=RBS.PatientStatusCode_UB04          
 LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID  and RPM.isDeleted=0  and RPM.PayorID=@PayorID      
 LEFT JOIN DDMaster DM ON DM.DDMasterID = RPM.BeneficiaryTypeID AND DM.Title = @MedicaidType          
                             
 WHERE B.BatchID=@BatchID AND  (BN.IsFirstTimeClaimInBatch IS NULL OR BN.IsFirstTimeClaimInBatch=1) AND BN.IsUseInBilling=1  and (RBM.isDeleted=0 or RBM.IsDeleted is Null)      
                
END                                  
ELSE IF EXISTS (SELECT 1 FROM Batches WHERE BatchID=@BatchID AND IsSent=1)                                  
BEGIN                               
                  
 SELECT                             
 B.BatchID,B.BatchTypeID, BN.BatchNoteID, BN.NoteID,R.ReferralID,R.FirstName, R.LastName,Dob=CONVERT(VARCHAR(10), R.Dob, 112), R.Gender , RPM.BeneficiaryNumber as 'AHCCCSID'        
 --R.AHCCCSID        
 ,R.CISNumber,                            
 MedicalRecordNumber=RPM.BeneficiaryNumber,        
 --MedicalRecordNumber=R.AHCCCSID,        
  R.PolicyNumber, AdmissionDate=CONVERT(VARCHAR(12), R.CreatedDate, 112)+REPLACE(CONVERT(VARCHAR(5), R.CreatedDate, 8),':',''),                              
 SubscriberID=RPM.BeneficiaryNumber, ClaimSubmitterIdentifier=RPM.BeneficiaryNumber, PatientAccountNumber=RPM.BeneficiaryNumber,StrBathNoteID='N'+CONVERT(VARCHAR,BN.BatchNoteID),                             
 C.Address,C.City,C.State,C.ZipCode,                            
 ModifierName = CASE WHEN SC.ModifierID IS NULL THEN '' ELSE                            
    STUFF((SELECT TOP 4 ',' + convert(varchar(500),M.ModifierCode, 120)FROM Modifiers M                                          
    WHERE M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID)) FOR XML PATH (''))  , 1, 1, '')                                      
    END,                                        
 ContinuedDX = (SELECT  STUFF((SELECT TOP 12 ',' +  F.DxCodeType+':'+F.DXCodeWithoutDot +':' + convert(varchar,F.Precedence) --+'|'+                                   
       FROM NoteDXCodeMappings F where F.NoteID=N.NoteID Order BY F.Precedence ASC                                      
       FOR XML PATH('')),1,1,'')),                            
 --RBS.SpecialProgramCode_CMS1500        
 @SpecialProgramCode_CMS1500 AS SpecialProgramCode_CMS1500        
 ,RBM.AuthorizationCode,          
 POS_CMS1500=DDM_P_FC.Value,POS_UB04=DDM_I_FC.Value,                    
 AdmissionTypeCode_UB04 = DDM_AT.Value,AdmissionSourceCode_UB04 = DDM_AS.Value,PatientStatusCode_UB04 = DDM_PS.Value,                    
 PH.PhysicianID,PH.NPINumber as PhysicianNPINumber,PH.FirstName as PhysicianFirstName,PH.LastName as PhysicianLastName,                            
 ServiceDateSpan=CONVERT(VARCHAR(10), N.ServiceDate, 112)+'-'+CONVERT(VARCHAR(10), N.ServiceDate, 112),    N.NoteID,N.ServiceDate,N.ServiceCodeID,N.ServiceCode,N.PosID,                            
 CalculatedUnit=BN.CLM_UNIT,              
 CalculatedAmount=BN.CLM_BilledAmount,                    
 N.BillingProviderID,                      
 N.BillingProviderName,N.BillingProviderFirstName,N.BillingProviderAddress,N.BillingProviderCity,                
 N.BillingProviderState,N.BillingProviderZipcode,N.BillingProviderEIN,N.BillingProviderNPI,                        
 N.BillingProviderGSA,N.BillingProviderAHCCCSID,N.RenderingProviderID,N.RenderingProviderName,N.RenderingProviderFirstName,                            
 N.RenderingProviderAddress,N.RenderingProviderCity,N.RenderingProviderState,N.RenderingProviderZipcode,N.RenderingProviderEIN,N.RenderingProviderNPI,                    
 N.RenderingProviderGSA,N.RenderingProviderAHCCCSID,                            
 P.PayorID,P.PayorName,PayorShortName = P.ShortName,PayorAddress =P.Address,PayorCity=P.City,PayorState=P.StateCode,PayorZipcode=P.Zipcode,                        
 PayorIdentificationNumber=P.PayorIdentificationNumber,                    
 --N.PayorID,N.PayorName,N.PayorShortName,N.PayorAddress,N.PayorIdentificationNumber,N.PayorCity,N.PayorState,N.PayorZipcode,                          
 N.RandomGroupID,      
 RBM.BillingUnitLimit,      
 DDM_R.Value as RevenueCode,                    
 GroupIDForMileServices=CASE WHEN LEN(N.GroupIDForMileServices)=0 OR N.GroupIDForMileServices IS NULL THEN CONVERT(VARCHAR(100),NEWID()) ELSE N.GroupIDForMileServices END,                                 
 BN.Submitted_ClaimSubmitterIdentifier, BN.Submitted_ClaimAdjustmentTypeID, BN.Original_ClaimSubmitterIdentifier, BN.Original_PayerClaimControlNumber,                            
 BN.ClaimAdjustmentReason, BN.IsUseInBilling, N.StartTime,N.EndTime                            
 FROM Batches B                                   
 INNER JOIN BatchNotes BN ON BN.BatchID=B.BatchID                                  
 INNER JOIN Notes N ON N.NoteID=BN.NoteID                   
 INNER JOIN Payors P ON P.PayorID=N.PayorID                                  
                
 INNER JOIN Referrals R on R.ReferralID=N.ReferralID                              
 LEFT JOIN ReferralBillingSettings RBS ON RBS.ReferralID=R.ReferralID                                  
 LEFT JOIN Physicians PH on PH.PhysicianID = R.PhysicianID                              
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
      
--inner JOIN ReferralBillingAuthorizationServiceCodes RBAS ON PSM.Referr = RBAS.ReferralBillingAuthorizationID  and RBAS.ServiceCodeID=SC.ServiceCodeID and RBAS.isDeleted=0      
 LEFT JOIN DDMaster DDM_R on DDM_R.DDMasterID=RBM.RevenueCode      
 LEFT JOIN DDMaster DDM_P_FC on DDM_P_FC.DDMasterID=RBS.POS_CMS1500                             
 LEFT JOIN DDMaster DDM_I_FC on DDM_I_FC.DDMasterID=RBS.POS_UB04                                
 LEFT JOIN DDMaster DDM_AT on DDM_AT.DDMasterID=RBS.AdmissionTypeCode_UB04                               
 LEFT JOIN DDMaster DDM_AS on DDM_AS.DDMasterID=RBS.AdmissionSourceCode_UB04                               
 LEFT JOIN DDMaster DDM_PS on DDM_PS.DDMasterID=RBS.PatientStatusCode_UB04                       
 LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID  and RPM.isDeleted=0  and rpm.PayorID=@PayorID      
 LEFT JOIN DDMaster DM ON DM.DDMasterID = RPM.BeneficiaryTypeID AND DM.Title = @MedicaidType          
        
 WHERE B.BatchID=@BatchID AND  (BN.IsFirstTimeClaimInBatch IS NULL OR BN.IsFirstTimeClaimInBatch=1) AND BN.IsUseInBilling=1  and (RBM.isDeleted=0 or RBM.IsDeleted is Null)                  
                     
END                                  
                   
SELECT FileName= REPLACE(P.ShortName,' ','_')                                     
+'_'+ REPLACE(CONVERT (CHAR(10),B.StartDate, 101),'/','')                                  
+'_'+ REPLACE(CONVERT (CHAR(10),B.EndDate, 101),'/','')                                      
+'_'+ BatchTypeShort                                  
+'_'+ CONVERT(VARCHAR(MAX),B.BatchID)                                  
FROM Batches B                                  
INNER JOIN Payors P ON P.PayorID=B.PayorID                                  
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