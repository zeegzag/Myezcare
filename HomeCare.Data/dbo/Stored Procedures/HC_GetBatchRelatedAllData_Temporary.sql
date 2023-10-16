-- EXEC HC_GetBatchRelatedAllData_Temporary @LoggedInUserId = '47', @IsCaseManagement = 'False', @ReferralsIds = '41,84', @BatchTypeID = '3', @PayorID=12        
        
CREATE Procedure [dbo].[HC_GetBatchRelatedAllData_Temporary]                                                         
@LoggedInUserId BIGINT,                        
@IsCaseManagement BIT,                  
@ReferralsIds NVARCHAR(MAX)   ,                
@BatchTypeID BIGINT,        
@PayorID BIGINT        
as--select @LoggedInUserId = '47', @IsCaseManagement = 'False', @ReferralsIds = '165', @PayorID = '12', @BatchTypeID = '1'                                                                
BEGIN                                    
                            
                            
IF(@IsCaseManagement = 1)                            
BEGIN                             
 EXEC HC_CM_GetBatchRelatedAllData_Temporary @LoggedInUserId   , @ReferralsIds ,@BatchTypeID,@PayorID                    
END                            
ELSE                             
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
                                                     
                                                            
--DECLARE @PayorID bigint;                                                                
--SELECT TOP 1 @PayorID=PayorID FROM Notes_Temporary WHERE CreatedBy=@LoggedInUserId;                                                                
                                                    
                                                    
SELECT *,                                                     
Submitter_NM103_NameLastOrOrganizationName = @SubmitterName,                                                    
Submitter_NM109_IdCodeQualifierEnum = @SubmitterIdCode,                                                    
Submitter_EDIContact1_PER02_Name=@SubmitterContactName,                                                    
Submitter_EDIContact1_PER04_CommunicationNumber=@SubmitterContactPhone,                                                    
Submitter_EDIContact1_PER08_CommunicationNumber=@SubmitterContactEmail                                                    
FROM PayorEdi837Settings Where PayorID=@PayorID                                                                 
                        
                    
                    
                    
                    
                    
                    
                        
-------------- Temporary DX CODE Generation                    
DECLARE @Table_ReferralDX TABLE(ID INT IDENTITY(1, 1) primary key ,ReferralID BIGINT,DXDetails VARCHAR(MAX))                    
INSERT INTO @Table_ReferralDX                    
SELECT RDM.ReferralID, DXDetails = DC.DxCodeType+':'+DC.DXCodeWithoutDot +':' + convert(varchar,RDM.Precedence)                     
FROM ReferralDXCodeMappings RDM            
INNER JOIN DXCodes DC  ON DC.DXCodeID = RDM.DXCodeID                          
INNER JOIN DxCodeTypes DCT ON DCT.DxCodeTypeID = DC.DxCodeType                     
ORDER BY RDM.ReferralID,RDM.Precedence                  
                  
                    
DECLARE @Table_ReferralDX_01 TABLE(ReferralID BIGINT,ContinuedDX VARCHAR(MAX))                    
INSERT INTO @Table_ReferralDX_01                    
SELECT R.ReferralID,                    
ContinuedDX = (SELECT  STUFF((SELECT TOP 12 ',' +  TR.DXDetails                                               
       FROM @Table_ReferralDX TR where TR.ReferralID=R.ReferralID Order BY TR.ID ASC                                                          
       FOR XML PATH('')),1,1,''))                    
FROM Referrals R                    
                  
-------------- Temporary DX CODE Generation                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
                    
IF(@BatchTypeID = 1)                    
BEGIN                        
        
SELECT                                                            
 BatchID=0,BatchTypeID=0, BatchNoteID=0,                        
                         
 N.NoteID,R.ReferralID,R.FirstName, R.LastName,Dob=CONVERT(VARCHAR(10), R.Dob, 112), R.Gender , RPM.BeneficiaryNumber as 'AHCCCSID',R.CISNumber,                                                          
 MedicalRecordNumber=RPM.BeneficiaryNumber, R.PolicyNumber, AdmissionDate=CONVERT(VARCHAR(12), R.CreatedDate, 112)+REPLACE(CONVERT(VARCHAR(5), R.CreatedDate, 8),':',''),                                                            
 SubscriberID=RPM.BeneficiaryNumber, ClaimSubmitterIdentifier=RPM.BeneficiaryNumber, PatientAccountNumber=RPM.BeneficiaryNumber,StrBathNoteID='N',                                  
 -- Null Address,Null City,Null State,null ZipCode,                                                          
 C.Address,C.City,C.State,C.ZipCode,                                                          
 ModifierName = CASE WHEN SC.ModifierID IS NULL THEN '' ELSE                                                          
      STUFF((SELECT TOP 4 ',' + convert(varchar(500),M.ModifierCode, 120)FROM Modifiers M                                                                        
      WHERE M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID)) FOR XML PATH (''))  , 1, 1, '')                                        
      END,                                                                      
   TR.ContinuedDX,                    
                       
 --ContinuedDX = (SELECT  STUFF((SELECT TOP 12 ',' +  F.DxCodeType+':'+F.DXCodeWithoutDot +':' + convert(varchar,F.Precedence) --+'|'+                                                                 
 --      FROM NoteDXCodeMappings F where F.NoteID=N.NoteID Order BY F.Precedence ASC                                                                    
 --      FOR XML PATH('')),1,1,'')),                                                 
 --RBS.SpecialProgramCode_CMS1500                                      
 @SpecialProgramCode_CMS1500 as SpecialProgramCode_CMS1500                                      
 ,RBM.AuthorizationCode,                                                  
 POS_CMS1500=DDM_P_FC.Value    
 --POS_CMS1500=RBM.FacilityCode    
     
 ,POS_UB04=DDM_I_FC.Value,                                                  
 AdmissionTypeCode_UB04 = DDM_AT.Value,AdmissionSourceCode_UB04 = DDM_AS.Value,PatientStatusCode_UB04 = DDM_PS.Value,                                                  
 PH.PhysicianID,PH.NPINumber as PhysicianNPINumber,PH.FirstName as PhysicianFirstName,PH.LastName as PhysicianLastName,                                                          
 ServiceDateSpan=CONVERT(VARCHAR(10), N.ServiceDate, 112)+'-'+CONVERT(VARCHAR(10), N.ServiceDate, 112),    N.NoteID,N.ServiceDate,N.ServiceCodeID,N.ServiceCode,N.PosID,                                                          
 CalculatedUnit=N.CalculatedUnit,                                            
 CalculatedAmount=N.CalculatedAmount,                                            
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
 Submitted_ClaimSubmitterIdentifier='',                         
 Submitted_ClaimAdjustmentTypeID='',                         
 Original_ClaimSubmitterIdentifier='',                         
 Original_PayerClaimControlNumber='',                                                          
 ClaimAdjustmentReason='',                         
 IsUseInBilling=1,                         
                         
 N.StartTime,                        
 N.EndTime, N.RenderingProvider_TaxonomyCode,          
 N.SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName,N.SupervisingProvidername2420DLoop_NM104_NameFirst,N.SupervisingProvidername2420DLoop_REF02_ReferenceId                                                  
 FROM                         
 --Batches B                                                                   
 --INNER JOIN BatchNotes BN ON BN.BatchID=B.BatchID                                                                
 Notes_Temporary N --ON N.NoteID=BN.NoteID                                                 
 INNER JOIN Payors P ON P.PayorID=N.PayorID                                                                
 INNER JOIN Referrals R on R.ReferralID=N.ReferralID                                                            
 LEFT JOIN @Table_ReferralDX_01 TR ON TR.ReferralID=R.ReferralID                                                             
 LEFT JOIN ReferralBillingSettings RBS ON RBS.ReferralID=R.ReferralID                                                             
 LEFT JOIN (SELECT DISTINCT  ReferralID, PhysicianID FROM ReferralPhysicians) As RP on RP.ReferralID = R.ReferralID                                                           
 LEFT JOIN Physicians PH on PH.PhysicianID = RP.PhysicianID                     
 LEFT JOIN ContactMappings CM on CM.ReferralID=R.ReferralID AND CM.ContactTypeID=1                        
 LEFT JOIN Contacts C on C.ContactID=CM.ContactID                                                              
 LEFT JOIN ServiceCodes SC ON SC.ServiceCodeID=N.ServiceCodeID                                 
 Inner JOIN ReferralBillingAuthorizations RBM on RBM.ReferralID=R.ReferralID AND RBM.Type='CMS1500' AND RBM.IsDeleted=0 and (CONVERT(date,N.ServiceDate) between RBM.StartDate and RBM.EndDate) and RBM.PayorID=@PayorID                                     
 Inner Join ScheduleMasters SM on SM.ReferralBillingAuthorizationID=RBM.ReferralBillingAuthorizationID                                     
 Inner Join EmployeeVisits EV on EV.ScheduleID=sm.ScheduleID and EV.EmployeeVisitID=N.EmployeeVisitID                                     
 LEFT JOIN DDMaster DDM_R on DDM_R.DDMasterID=RBM.RevenueCode                            
 LEFT JOIN DDMaster DDM_P_FC on DDM_P_FC.DDMasterID=RBM.FacilityCode--RBS.POS_CMS1500                                                           
 LEFT JOIN DDMaster DDM_I_FC on DDM_I_FC.DDMasterID=RBS.POS_UB04                                                           
 LEFT JOIN DDMaster DDM_AT on DDM_AT.DDMasterID=RBS.AdmissionTypeCode_UB04                                                           
 LEFT JOIN DDMaster DDM_AS on DDM_AS.DDMasterID=RBS.AdmissionSourceCode_UB04                      
 LEFT JOIN DDMaster DDM_PS on DDM_PS.DDMasterID=RBS.PatientStatusCode_UB04                                        
 LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID  and RPM.isDeleted=0  and RPM.PayorID=@PayorID                                    
 LEFT JOIN DDMaster DM ON DM.DDMasterID = RPM.BeneficiaryTypeID AND DM.Title = @MedicaidType                                                                          
 WHERE N.IsDeleted=0 AND N.CreatedBy=@LoggedInUserId  AND  (RBM.isDeleted=0 or RBM.IsDeleted is Null)                          
 AND N.ReferralID in (SELECT val FROM GETCSVTABLE(@ReferralsIds))                   
        
END                        
        
ELSE IF (@BatchTypeID =2)        
BEGIN        
SELECT NULL        
END        
        
ELSE IF (@BatchTypeID = 3 )        
BEGIN        
        
SELECT                                                            
 BatchID=0,BatchTypeID=0, BatchNoteID=0,                        
 N.NoteID,R.ReferralID,R.FirstName, R.LastName,Dob=CONVERT(VARCHAR(10), R.Dob, 112), R.Gender , RPM.BeneficiaryNumber as 'AHCCCSID',R.CISNumber,                                                    
 MedicalRecordNumber=RPM.BeneficiaryNumber, R.PolicyNumber, AdmissionDate=CONVERT(VARCHAR(12), R.CreatedDate, 112)+REPLACE(CONVERT(VARCHAR(5), R.CreatedDate, 8),':',''),         
 SubscriberID=RPM.BeneficiaryNumber, ClaimSubmitterIdentifier=RPM.BeneficiaryNumber, PatientAccountNumber=RPM.BeneficiaryNumber,StrBathNoteID='N',                                  
-- Null Address,Null City,Null State,null ZipCode,                                                          
 C.Address,C.City,C.State,C.ZipCode,                                                          
 ModifierName = CASE WHEN SC.ModifierID IS NULL THEN '' ELSE                                                          
      STUFF((SELECT TOP 4 ',' + convert(varchar(500),M.ModifierCode, 120)FROM Modifiers M                                                                        
      WHERE M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID)) FOR XML PATH (''))  , 1, 1, '')                                        
      END,                                                                      
   TR.ContinuedDX,                    
 @SpecialProgramCode_CMS1500 as SpecialProgramCode_CMS1500                                      
 ,RBM.AuthorizationCode,                                                  
 POS_CMS1500=DDM_P_FC.Value,    
 --POS_CMS1500=RBM.FacilityCode,    
 POS_UB04=DDM_I_FC.Value,                                                  
 AdmissionTypeCode_UB04 = DDM_AT.Value,AdmissionSourceCode_UB04 = DDM_AS.Value,PatientStatusCode_UB04 = DDM_PS.Value,                                                  
 PH.PhysicianID,PH.NPINumber as PhysicianNPINumber,PH.FirstName as PhysicianFirstName,PH.LastName as PhysicianLastName,                                                          
 ServiceDateSpan=CONVERT(VARCHAR(10), N.ServiceDate, 112)+'-'+CONVERT(VARCHAR(10), N.ServiceDate, 112),    N.NoteID,N.ServiceDate,N.ServiceCodeID,N.ServiceCode,N.PosID,                                                          
 CalculatedUnit=N.CalculatedUnit,                                            
 CalculatedAmount=N.CalculatedAmount,                                            
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
 --GroupIDForMileServices=CASE WHEN LEN(N.GroupIDForMileServices)=0 OR N.GroupIDForMileServices IS NULL THEN CONVERT(VARCHAR(100),NEWID()) ELSE N.GroupIDForMileServices END,                        
 Submitted_ClaimSubmitterIdentifier='',                         
 Submitted_ClaimAdjustmentTypeID='',                         
 Original_ClaimSubmitterIdentifier='',                         
 Original_PayerClaimControlNumber='',                                                          
 ClaimAdjustmentReason='',                         
 IsUseInBilling=1,                         
                         
 N.StartTime,                        
 N.EndTime, N.RenderingProvider_TaxonomyCode,          
 N.SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName,N.SupervisingProvidername2420DLoop_NM104_NameFirst,N.SupervisingProvidername2420DLoop_REF02_ReferenceId                                                  
 FROM                         
 --Batches B                                                                   
 --INNER JOIN BatchNotes BN ON BN.BatchID=B.BatchID                                                                
 Notes N --ON N.NoteID=BN.NoteID                                                 
 INNER JOIN (                  
                       
     SELECT t.NoteID,t.CLP01_ClaimSubmitterIdentifier AS Original_ClaimSubmitterIdentifier, t.CLP07_PayerClaimControlNumber AS Original_PayerClaimControlNumber,t.ClaimAdjustmentTypeID AS Submitted_ClaimAdjustmentTypeID ,t.ClaimAdjustmentReason           
  
    
      
 FROM                  
       (SELECT DISTINCT ROW_NUMBER() OVER                   
       ( PARTITION BY BN.NoteID Order BY BN.BatchID DESC,MarkAsLatest DESC,  BN.BatchNoteID Desc)  AS RowNumber,                   
       -- ( PARTITION BY BN.BatchID, BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber,                   
       NoteID,CLP02_ClaimStatusCode,CLP01_ClaimSubmitterIdentifier,                   
        CLP07_PayerClaimControlNumber, ClaimAdjustmentTypeID , ClaimAdjustmentReason FROM BatchNotes BN                   
       ) AS t -- WHERE RowNumber=1 AND ClaimAdjustmentTypeID IS NOT NULL                  
       LEFT JOIN  BatchNotes BN1 ON   t.CLP07_PayerClaimControlNumber=BN1.Original_PayerClaimControlNumber AND t.NoteID = BN1.NoteID        
       WHERE RowNumber=1 AND (t.ClaimAdjustmentTypeID IS NOT NULL AND                   
       t.ClaimAdjustmentTypeID NOT IN ('Write-Off','Denial','Resend','Data-Validation','Payor-Change','Other')                  
       )                   
       AND BN1.Original_PayerClaimControlNumber IS NULL                  
                  
    ) BND ON BND.NoteID=N.NoteID         
 INNER JOIN Payors P ON P.PayorID=N.PayorID                                                                
 INNER JOIN Referrals R on R.ReferralID=N.ReferralID                                                            
 LEFT JOIN @Table_ReferralDX_01 TR ON TR.ReferralID=R.ReferralID                                                             
 LEFT JOIN ReferralBillingSettings RBS ON RBS.ReferralID=R.ReferralID                                                             
 LEFT JOIN (SELECT DISTINCT  ReferralID, PhysicianID FROM ReferralPhysicians) As RP on RP.ReferralID = R.ReferralID                                                           
 LEFT JOIN Physicians PH on PH.PhysicianID = RP.PhysicianID                                                              
 LEFT JOIN ContactMappings CM on CM.ReferralID=R.ReferralID AND CM.ContactTypeID=1                        
 LEFT JOIN Contacts C on C.ContactID=CM.ContactID                                                              
 LEFT JOIN ServiceCodes SC ON SC.ServiceCodeID=N.ServiceCodeID                                       
 Inner JOIN ReferralBillingAuthorizations RBM on RBM.ReferralID=R.ReferralID AND RBM.Type='CMS1500' AND RBM.IsDeleted=0 and (CONVERT(date,N.ServiceDate) between RBM.StartDate and RBM.EndDate) and RBM.PayorID=@PayorID                                     
 Inner Join ScheduleMasters SM on SM.ReferralBillingAuthorizationID=RBM.ReferralBillingAuthorizationID                                     
 Inner Join EmployeeVisits EV on EV.ScheduleID=sm.ScheduleID and EV.EmployeeVisitID=N.EmployeeVisitID                                     
 LEFT JOIN DDMaster DDM_R on DDM_R.DDMasterID=RBM.RevenueCode                                                             
 --LEFT JOIN DDMaster DDM_P_FC on DDM_P_FC.DDMasterID=RBS.POS_CMS1500                                                           
 LEFT JOIN DDMaster DDM_P_FC on DDM_P_FC.DDMasterID=RBM.FacilityCode  
  
 LEFT JOIN DDMaster DDM_I_FC on DDM_I_FC.DDMasterID=RBS.POS_UB04                                                           
 LEFT JOIN DDMaster DDM_AT on DDM_AT.DDMasterID=RBS.AdmissionTypeCode_UB04                                                           
 LEFT JOIN DDMaster DDM_AS on DDM_AS.DDMasterID=RBS.AdmissionSourceCode_UB04                      
 LEFT JOIN DDMaster DDM_PS on DDM_PS.DDMasterID=RBS.PatientStatusCode_UB04                                        
 LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID  and RPM.isDeleted=0  and RPM.PayorID=@PayorID                                    
 LEFT JOIN DDMaster DM ON DM.DDMasterID = RPM.BeneficiaryTypeID AND DM.Title = @MedicaidType                                                                          
 WHERE N.IsDeleted=0  AND  (RBM.isDeleted=0 or RBM.IsDeleted is Null)                          
 AND N.ReferralID in (SELECT val FROM GETCSVTABLE(@ReferralsIds))                   
        
END        
        
        
        
ELSE IF (@BatchTypeID = 4 )        
BEGIN        
        
SELECT                                                            
 BatchID=0,BatchTypeID=0, BatchNoteID=0,                        
 N.NoteID,R.ReferralID,R.FirstName, R.LastName,Dob=CONVERT(VARCHAR(10), R.Dob, 112), R.Gender , RPM.BeneficiaryNumber as 'AHCCCSID',R.CISNumber,                                                    
 MedicalRecordNumber=RPM.BeneficiaryNumber, R.PolicyNumber, AdmissionDate=CONVERT(VARCHAR(12), R.CreatedDate, 112)+REPLACE(CONVERT(VARCHAR(5), R.CreatedDate, 8),':',''),         
 SubscriberID=RPM.BeneficiaryNumber, ClaimSubmitterIdentifier=RPM.BeneficiaryNumber, PatientAccountNumber=RPM.BeneficiaryNumber,StrBathNoteID='N',                                  
-- Null Address,Null City,Null State,null ZipCode,                                                          
 C.Address,C.City,C.State,C.ZipCode,                                                          
 ModifierName = CASE WHEN SC.ModifierID IS NULL THEN '' ELSE                                                          
      STUFF((SELECT TOP 4 ',' + convert(varchar(500),M.ModifierCode, 120)FROM Modifiers M                                                                        
      WHERE M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID)) FOR XML PATH (''))  , 1, 1, '')                                        
      END,                                
   TR.ContinuedDX,                    
 @SpecialProgramCode_CMS1500 as SpecialProgramCode_CMS1500                                      
 ,RBM.AuthorizationCode,                                                  
 POS_CMS1500=DDM_P_FC.Value,    
 --POS_CMS1500=RBM.FacilityCode,    
 POS_UB04=DDM_I_FC.Value,                                                  
 AdmissionTypeCode_UB04 = DDM_AT.Value,AdmissionSourceCode_UB04 = DDM_AS.Value,PatientStatusCode_UB04 = DDM_PS.Value,                                                  
 PH.PhysicianID,PH.NPINumber as PhysicianNPINumber,PH.FirstName as PhysicianFirstName,PH.LastName as PhysicianLastName,                                                          
 ServiceDateSpan=CONVERT(VARCHAR(10), N.ServiceDate, 112)+'-'+CONVERT(VARCHAR(10), N.ServiceDate, 112),    N.NoteID,N.ServiceDate,N.ServiceCodeID,N.ServiceCode,N.PosID,                                                          
 CalculatedUnit=N.CalculatedUnit,                                            
 CalculatedAmount=N.CalculatedAmount,                                            
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
 --GroupIDForMileServices=CASE WHEN LEN(N.GroupIDForMileServices)=0 OR N.GroupIDForMileServices IS NULL THEN CONVERT(VARCHAR(100),NEWID()) ELSE N.GroupIDForMileServices END,                        
 Submitted_ClaimSubmitterIdentifier='',                         
 Submitted_ClaimAdjustmentTypeID='',                         
 Original_ClaimSubmitterIdentifier='',                         
 Original_PayerClaimControlNumber='',                                                          
 ClaimAdjustmentReason='',                         
 IsUseInBilling=1,                         
                         
 N.StartTime,                        
 N.EndTime, N.RenderingProvider_TaxonomyCode,          
 N.SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName,N.SupervisingProvidername2420DLoop_NM104_NameFirst,N.SupervisingProvidername2420DLoop_REF02_ReferenceId                                                  
 FROM                         
 --Batches B                                                                   
 --INNER JOIN BatchNotes BN ON BN.BatchID=B.BatchID                                                                
 Notes N --ON N.NoteID=BN.NoteID                                                 
 INNER JOIN (                  
                       
     SELECT t.NoteID,t.CLP01_ClaimSubmitterIdentifier AS Original_ClaimSubmitterIdentifier, t.CLP07_PayerClaimControlNumber AS Original_PayerClaimControlNumber,t.ClaimAdjustmentTypeID AS Submitted_ClaimAdjustmentTypeID ,t.ClaimAdjustmentReason            
  
   
      
 FROM                  
       (SELECT DISTINCT ROW_NUMBER() OVER                   
       ( PARTITION BY BN.NoteID Order BY BN.BatchID DESC,MarkAsLatest DESC,  BN.BatchNoteID Desc)  AS RowNumber,                   
       -- ( PARTITION BY BN.BatchID, BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber,                   
       NoteID,CLP02_ClaimStatusCode,CLP01_ClaimSubmitterIdentifier,                   
        CLP07_PayerClaimControlNumber, ClaimAdjustmentTypeID , ClaimAdjustmentReason FROM BatchNotes BN                   
       ) AS t -- WHERE RowNumber=1 AND ClaimAdjustmentTypeID IS NOT NULL                  
       LEFT JOIN  BatchNotes BN1 ON   t.CLP07_PayerClaimControlNumber=BN1.Original_PayerClaimControlNumber AND t.NoteID = BN1.NoteID        
       WHERE RowNumber=1 AND (t.ClaimAdjustmentTypeID IS NOT NULL AND                   
       t.ClaimAdjustmentTypeID IN ('Resend','Data-Validation')                  
       )                   
       AND BN1.Original_PayerClaimControlNumber IS NULL                  
                  
    ) BND ON BND.NoteID=N.NoteID         
 INNER JOIN Payors P ON P.PayorID=N.PayorID                                                                
 INNER JOIN Referrals R on R.ReferralID=N.ReferralID                                                            
 LEFT JOIN @Table_ReferralDX_01 TR ON TR.ReferralID=R.ReferralID                                                             
 LEFT JOIN ReferralBillingSettings RBS ON RBS.ReferralID=R.ReferralID                                                             
 LEFT JOIN (SELECT DISTINCT  ReferralID, PhysicianID FROM ReferralPhysicians) As RP on RP.ReferralID = R.ReferralID                                                           
 LEFT JOIN Physicians PH on PH.PhysicianID = RP.PhysicianID                                                              
 LEFT JOIN ContactMappings CM on CM.ReferralID=R.ReferralID AND CM.ContactTypeID=1                        
 LEFT JOIN Contacts C on C.ContactID=CM.ContactID                                                              
 LEFT JOIN ServiceCodes SC ON SC.ServiceCodeID=N.ServiceCodeID                                       
 Inner JOIN ReferralBillingAuthorizations RBM on RBM.ReferralID=R.ReferralID AND RBM.Type='CMS1500' AND RBM.IsDeleted=0 and (CONVERT(date,N.ServiceDate) between RBM.StartDate and RBM.EndDate) and RBM.PayorID=@PayorID                                     
 Inner Join ScheduleMasters SM on SM.ReferralBillingAuthorizationID=RBM.ReferralBillingAuthorizationID                                     
 Inner Join EmployeeVisits EV on EV.ScheduleID=sm.ScheduleID and EV.EmployeeVisitID=N.EmployeeVisitID                                     
 LEFT JOIN DDMaster DDM_R on DDM_R.DDMasterID=RBM.RevenueCode                                                             
 --LEFT JOIN DDMaster DDM_P_FC on DDM_P_FC.DDMasterID=RBS.POS_CMS1500    
 LEFT JOIN DDMaster DDM_P_FC on DDM_P_FC.DDMasterID=RBM.FacilityCode  
 LEFT JOIN DDMaster DDM_I_FC on DDM_I_FC.DDMasterID=RBM.FacilityCode--RBS.POS_UB04                                                           
 LEFT JOIN DDMaster DDM_AT on DDM_AT.DDMasterID=RBS.AdmissionTypeCode_UB04                                                           
 LEFT JOIN DDMaster DDM_AS on DDM_AS.DDMasterID=RBS.AdmissionSourceCode_UB04                      
 LEFT JOIN DDMaster DDM_PS on DDM_PS.DDMasterID=RBS.PatientStatusCode_UB04                                        
 LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID  and RPM.isDeleted=0  and RPM.PayorID=@PayorID                                    
 LEFT JOIN DDMaster DM ON DM.DDMasterID = RPM.BeneficiaryTypeID AND DM.Title = @MedicaidType                                                                          
 WHERE N.IsDeleted=0  AND  (RBM.isDeleted=0 or RBM.IsDeleted is Null)                          
 AND N.ReferralID in (SELECT val FROM GETCSVTABLE(@ReferralsIds))                   
        
END        
        
        
        
        
                        
                        
SELECT FileName=PayorBillingType FROM Payors Where PayorID=@PayorID                          
                                 
END                         
                        
                        
END 