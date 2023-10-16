-- EXEC HC_GetBatchRelatedAllData @BatchID = '142',@FileType = 'CMS1500'                        
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
 B.BatchID,B.BatchTypeID, BN.BatchNoteID, BN.NoteID,R.ReferralID,R.FirstName, R.LastName,Dob=CONVERT(VARCHAR(10), R.Dob, 112), R.Gender , R.AHCCCSID,R.CISNumber,                  
 MedicalRecordNumber=R.AHCCCSID, R.PolicyNumber, AdmissionDate=CONVERT(VARCHAR(12), R.CreatedDate, 112)+REPLACE(CONVERT(VARCHAR(5), R.CreatedDate, 8),':',''),                    
 SubscriberID=R.AHCCCSID, ClaimSubmitterIdentifier=R.AHCCCSID, PatientAccountNumber=R.AHCCCSID,StrBathNoteID='N'+CONVERT(VARCHAR,BN.BatchNoteID),                   
 C.Address,C.City,C.State,C.ZipCode,                  
 ModifierName = CASE WHEN SC.ModifierID IS NULL THEN '' ELSE                  
      STUFF((SELECT TOP 4 ',' + convert(varchar(500),M.ModifierCode, 120)FROM Modifiers M                                
      WHERE M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID)) FOR XML PATH (''))  , 1, 1, '')                            
      END,                              
 ContinuedDX = (SELECT  STUFF((SELECT TOP 12 ',' +  F.DxCodeType+':'+F.DXCodeWithoutDot +':' + convert(varchar,F.Precedence) --+'|'+                         
       FROM NoteDXCodeMappings F where F.NoteID=N.NoteID Order BY F.Precedence ASC                            
       FOR XML PATH('')),1,1,'')),                  
 RBS.SpecialProgramCode_CMS1500,RBM.AuthorizationCode,          
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
 PayorIdentificationNumber=P.AgencyNPID,                       
 --N.PayorID,N.PayorName,N.PayorShortName,N.PayorAddress,N.PayorIdentificationNumber,N.PayorCity,N.PayorState,N.PayorZipcode,          
 N.RandomGroupID,PSM.BillingUnitLimit,DDM_R.Value as RevenueCode,                  
 GroupIDForMileServices=CASE WHEN LEN(N.GroupIDForMileServices)=0 OR N.GroupIDForMileServices IS NULL THEN CONVERT(VARCHAR(100),NEWID()) ELSE N.GroupIDForMileServices END,                       
 BN.Submitted_ClaimSubmitterIdentifier, BN.Submitted_ClaimAdjustmentTypeID, BN.Original_ClaimSubmitterIdentifier, BN.Original_PayerClaimControlNumber,                  
 BN.ClaimAdjustmentReason, BN.IsUseInBilling                  
 FROM Batches B                           
 INNER JOIN BatchNotes BN ON BN.BatchID=B.BatchID                        
 INNER JOIN Notes N ON N.NoteID=BN.NoteID         
 INNER JOIN Payors P ON P.PayorID=N.PayorID                        
 INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID                                                 
 INNER JOIN Referrals R on R.ReferralID=N.ReferralID                    
 LEFT JOIN ReferralBillingSettings RBS ON RBS.ReferralID=R.ReferralID                      
 LEFT JOIN (SELECT DISTINCT  ReferralID, PhysicianID FROM ReferralPhysicians) As RP on RP.ReferralID = R.ReferralID                      
 LEFT JOIN Physicians PH on PH.PhysicianID = RP.PhysicianID                      
 LEFT JOIN ContactMappings CM on CM.ReferralID=R.ReferralID AND CM.ContactTypeID=1 --AND (CM.IsDCSLegalGuardian=1 OR CM.IsPrimaryPlacementLegalGuardian=1)                        
 LEFT JOIN Contacts C on C.ContactID=CM.ContactID                      
 LEFT JOIN ServiceCodes SC ON SC.ServiceCodeID=N.ServiceCodeID                    
 LEFT JOIN ReferralBillingAuthorizations RBM on RBM.ReferralID=R.ReferralID AND RBM.Type=@FileType AND RBM.IsDeleted=0 and (CONVERT(date,GETDATE()) between RBM.StartDate and RBM.EndDate)                  
 LEFT JOIN DDMaster DDM_R on DDM_R.DDMasterID=PSM.RevenueCode                     
 LEFT JOIN DDMaster DDM_P_FC on DDM_P_FC.DDMasterID=RBS.POS_CMS1500                   
 LEFT JOIN DDMaster DDM_I_FC on DDM_I_FC.DDMasterID=RBS.POS_UB04                   
 LEFT JOIN DDMaster DDM_AT on DDM_AT.DDMasterID=RBS.AdmissionTypeCode_UB04                   
 LEFT JOIN DDMaster DDM_AS on DDM_AS.DDMasterID=RBS.AdmissionSourceCode_UB04                     
 LEFT JOIN DDMaster DDM_PS on DDM_PS.DDMasterID=RBS.PatientStatusCode_UB04                     
 WHERE B.BatchID=@BatchID AND  (BN.IsFirstTimeClaimInBatch IS NULL OR BN.IsFirstTimeClaimInBatch=1) AND BN.IsUseInBilling=1            
      
END                        
ELSE IF EXISTS (SELECT 1 FROM Batches WHERE BatchID=@BatchID AND IsSent=1)                        
BEGIN                     
        
 SELECT                   
 B.BatchID,B.BatchTypeID, BN.BatchNoteID, BN.NoteID,R.ReferralID,R.FirstName, R.LastName,Dob=CONVERT(VARCHAR(10), R.Dob, 112), R.Gender , R.AHCCCSID,R.CISNumber,                  
 MedicalRecordNumber=R.AHCCCSID, R.PolicyNumber, AdmissionDate=CONVERT(VARCHAR(12), R.CreatedDate, 112)+REPLACE(CONVERT(VARCHAR(5), R.CreatedDate, 8),':',''),                    
 SubscriberID=R.AHCCCSID, ClaimSubmitterIdentifier=R.AHCCCSID, PatientAccountNumber=R.AHCCCSID,StrBathNoteID='N'+CONVERT(VARCHAR,BN.BatchNoteID),                   
 C.Address,C.City,C.State,C.ZipCode,                  
 ModifierName = CASE WHEN SC.ModifierID IS NULL THEN '' ELSE                  
    STUFF((SELECT TOP 4 ',' + convert(varchar(500),M.ModifierCode, 120)FROM Modifiers M                                
    WHERE M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID)) FOR XML PATH (''))  , 1, 1, '')                            
    END,                              
 ContinuedDX = (SELECT  STUFF((SELECT TOP 12 ',' +  F.DxCodeType+':'+F.DXCodeWithoutDot +':' + convert(varchar,F.Precedence) --+'|'+                         
       FROM NoteDXCodeMappings F where F.NoteID=N.NoteID Order BY F.Precedence ASC                            
       FOR XML PATH('')),1,1,'')),                  
 RBS.SpecialProgramCode_CMS1500,RBM.AuthorizationCode,          
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
 PayorIdentificationNumber=P.AgencyNPID,          
 --N.PayorID,N.PayorName,N.PayorShortName,N.PayorAddress,N.PayorIdentificationNumber,N.PayorCity,N.PayorState,N.PayorZipcode,                
 N.RandomGroupID,PSM.BillingUnitLimit,DDM_R.Value as RevenueCode,          
 GroupIDForMileServices=CASE WHEN LEN(N.GroupIDForMileServices)=0 OR N.GroupIDForMileServices IS NULL THEN CONVERT(VARCHAR(100),NEWID()) ELSE N.GroupIDForMileServices END,                       
 BN.Submitted_ClaimSubmitterIdentifier, BN.Submitted_ClaimAdjustmentTypeID, BN.Original_ClaimSubmitterIdentifier, BN.Original_PayerClaimControlNumber,                  
 BN.ClaimAdjustmentReason, BN.IsUseInBilling                  
 FROM Batches B                         
 INNER JOIN BatchNotes BN ON BN.BatchID=B.BatchID                        
 INNER JOIN BatchNoteDetails N ON N.NoteID=BN.NoteID AND N.BatchID=BN.BatchID                  
 INNER JOIN Payors P ON P.PayorID=N.PayorID                        
 INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID           
 INNER JOIN Referrals R on R.ReferralID=N.ReferralID                    
 LEFT JOIN ReferralBillingSettings RBS ON RBS.ReferralID=R.ReferralID                        
 LEFT JOIN Physicians PH on PH.PhysicianID = R.PhysicianID                    
 LEFT JOIN ContactMappings CM on CM.ReferralID=R.ReferralID AND CM.ContactTypeID=1 --AND (CM.IsDCSLegalGuardian=1 OR CM.IsPrimaryPlacementLegalGuardian=1)                        
 LEFT JOIN Contacts C on C.ContactID=CM.ContactID                        
 LEFT JOIN ServiceCodes SC ON SC.ServiceCodeID=N.ServiceCodeID           
 LEFT JOIN ReferralBillingAuthorizations RBM on RBM.ReferralID=R.ReferralID and RBM.Type=@FileType AND RBM.IsDeleted=0 and (CONVERT(date,GETDATE()) between RBM.StartDate and RBM.EndDate)           
 LEFT JOIN DDMaster DDM_R on DDM_R.DDMasterID=PSM.RevenueCode           
 LEFT JOIN DDMaster DDM_P_FC on DDM_P_FC.DDMasterID=RBS.POS_CMS1500                   
 LEFT JOIN DDMaster DDM_I_FC on DDM_I_FC.DDMasterID=RBS.POS_UB04                      
 LEFT JOIN DDMaster DDM_AT on DDM_AT.DDMasterID=RBS.AdmissionTypeCode_UB04                     
 LEFT JOIN DDMaster DDM_AS on DDM_AS.DDMasterID=RBS.AdmissionSourceCode_UB04                     
 LEFT JOIN DDMaster DDM_PS on DDM_PS.DDMasterID=RBS.PatientStatusCode_UB04             
 WHERE B.BatchID=@BatchID AND  (BN.IsFirstTimeClaimInBatch IS NULL OR BN.IsFirstTimeClaimInBatch=1) AND BN.IsUseInBilling=1            
           
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
                        
END  