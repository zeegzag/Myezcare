  
--  exec  HC_GenerateEdiFileModel01 138,18,24234,'UB04'  
CREATE PROCEDURE [dbo].[HC_GenerateEdiFileModel01]   
 @BatchID BIGINT=138,   
 @PayorID BIGINT,    
 @ReferralID BIGINT,    
 @FileType varchar(10)    
AS    
BEGIN    
   DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()      
DECLARE @BillingProviderName varchar(max);        
DECLARE @BillingProviderFirstName varchar(max)='';        
DECLARE @BillingProviderNPI varchar(max);        
DECLARE @BillingProviderAddress varchar(max);        
DECLARE @BillingProviderCity varchar(max);        
DECLARE @BillingProviderState varchar(max);        
DECLARE @BillingProviderZipcode varchar(max);        
DECLARE @BillingProviderEIN varchar(max);        
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
@SubmitterContactEmail = Submitter_EDIContact1_PER08_CommunicationNumber,        
@BillingProviderName=BillingProvider_NM103_NameLastOrOrganizationName,        
@BillingProviderNPI=BillingProvider_NM109_IdCode,        
@BillingProviderAddress=BillingProvider_N301_Address,        
@BillingProviderCity=BillingProvider_N401_City,        
@BillingProviderState=BillingProvider_N402_State,        
@BillingProviderZipcode=BillingProvider_N403_Zipcode,        
@BillingProviderEIN=BillingProvider_REF02_ReferenceIdentification         
FROM OrganizationSettings        
   
IF(@BATCHID = 0)  
BEGIN  
 SELECT R.ReferralID,R.FirstName, R.LastName,MiddleName = CASE WHEN LEN(R.MiddleName) > 0 THEN LEFT(R.MiddleName,1) ELSE '' END,  
dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) as PatientName ,   
 PatientDOB =CONVERT(varchar,R.Dob, 101),  
 Dob=CONVERT(VARCHAR(10), R.Dob, 112),  
 DobMM=CONVERT(VARCHAR(2),DATEPART(mm,R.Dob)),  
 DobDD=CONVERT(VARCHAR(2),DATEPART(dd,R.Dob)),    
 DobYYYY=RIGHT(YEAR(R.Dob),2),    
 R.Gender , R.AHCCCSID,R.CISNumber,MedicalRecordNumber=R.AHCCCSID, R.PolicyNumber,     
 StrAdmissionDate=CONVERT(varchar,R.Dob, 1),                
 SubscriberID=R.AHCCCSID, ClaimSubmitterIdentifier=R.AHCCCSID, PatientAccountNumber=R.AHCCCSID,    
 C.Address,C.City,C.State,C.ZipCode,C.Phone1,    
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
 N.BillingProviderID,BillingProviderName=@BillingProviderName, BillingProviderFirstName=@BillingProviderFirstName,        
 BillingProviderAddress=@BillingProviderAddress, BillingProviderCity=@BillingProviderCity,        
 BillingProviderState=@BillingProviderState, BillingProviderZipcode=@BillingProviderZipcode,        
 BillingProviderEIN=@BillingProviderEIN, BillingProviderNPI=@BillingProviderNPI,        
 N.BillingProviderGSA,N.BillingProviderAHCCCSID,N.RenderingProviderID,N.RenderingProviderName,N.RenderingProviderFirstName,              
 N.RenderingProviderAddress,N.RenderingProviderCity,N.RenderingProviderState,N.RenderingProviderZipcode,N.RenderingProviderEIN,N.RenderingProviderNPI,      
 N.RenderingProviderGSA,N.RenderingProviderAHCCCSID,              
 P.PayorID,P.PayorName,PayorShortName = P.ShortName,PayorAddress =P.Address,PayorCity=P.City,PayorState=P.StateCode,PayorZipcode=P.Zipcode,          
 PayorIdentificationNumber=P.PayorIdentificationNumber,                   
 --N.PayorID,N.PayorName,N.PayorShortName,N.PayorAddress,N.PayorIdentificationNumber,N.PayorCity,N.PayorState,N.PayorZipcode,      
 N.RandomGroupID,PSM.BillingUnitLimit,DDM_R.Value as RevenueCode,              
 GroupIDForMileServices=CASE WHEN LEN(N.GroupIDForMileServices)=0 OR N.GroupIDForMileServices IS NULL THEN CONVERT(VARCHAR(100),NEWID()) ELSE N.GroupIDForMileServices END    
 ,N.CalculatedUnit,CalculatedAmount=CONVERT(decimal(8,2),N.CalculatedAmount),N.Rate  
 FROM Referrals R    
 INNER JOIN Notes N ON R.ReferralID=N.ReferralID    
 INNER JOIN Payors P ON P.PayorID=N.PayorID                    
 INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID    
 LEFT JOIN ReferralBillingSettings RBS ON RBS.ReferralID=R.ReferralID                  
 LEFT JOIN Physicians PH on PH.PhysicianID = R.PhysicianID                  
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
 WHERE R.ReferralID=@ReferralID AND P.PayorID=@PayorID  AND  
 N.NoteID NOT IN (SELECT NoteID FROM BatchNotes)  
END   
ELSE  
BEGIN  
 SELECT BN.BatchID,R.ReferralID,R.FirstName, R.LastName,MiddleName = CASE WHEN LEN(R.MiddleName) > 0 THEN LEFT(R.MiddleName,1) ELSE '' END,  
 dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) as PatientName ,   
 PatientDOB =CONVERT(varchar,R.Dob, 101),  
 Dob=CONVERT(VARCHAR(10), R.Dob, 112),  
 DobMM=CONVERT(VARCHAR(2),DATEPART(mm,R.Dob)),  
 DobDD=CONVERT(VARCHAR(2),DATEPART(dd,R.Dob)),    
 DobYYYY=RIGHT(YEAR(R.Dob),2),    
 R.Gender , R.AHCCCSID,R.CISNumber,MedicalRecordNumber=R.AHCCCSID, R.PolicyNumber,     
 StrAdmissionDate=CONVERT(varchar,R.Dob, 1),                
 SubscriberID=R.AHCCCSID, ClaimSubmitterIdentifier=R.AHCCCSID, PatientAccountNumber=R.AHCCCSID,    
 C.Address,C.City,C.State,C.ZipCode,C.Phone1,    
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
 N.BillingProviderID,BillingProviderName=@BillingProviderName, BillingProviderFirstName=@BillingProviderFirstName,        
 BillingProviderAddress=@BillingProviderAddress, BillingProviderCity=@BillingProviderCity,        
 BillingProviderState=@BillingProviderState, BillingProviderZipcode=@BillingProviderZipcode,        
 BillingProviderEIN=@BillingProviderEIN, BillingProviderNPI=@BillingProviderNPI,        
 N.BillingProviderGSA,N.BillingProviderAHCCCSID,N.RenderingProviderID,N.RenderingProviderName,N.RenderingProviderFirstName,              
 N.RenderingProviderAddress,N.RenderingProviderCity,N.RenderingProviderState,N.RenderingProviderZipcode,N.RenderingProviderEIN,N.RenderingProviderNPI,      
 N.RenderingProviderGSA,N.RenderingProviderAHCCCSID,              
 P.PayorID,P.PayorName,PayorShortName = P.ShortName,PayorAddress =P.Address,PayorCity=P.City,PayorState=P.StateCode,PayorZipcode=P.Zipcode,          
 PayorIdentificationNumber=P.PayorIdentificationNumber,                   
 --N.PayorID,N.PayorName,N.PayorShortName,N.PayorAddress,N.PayorIdentificationNumber,N.PayorCity,N.PayorState,N.PayorZipcode,      
 N.RandomGroupID,PSM.BillingUnitLimit,DDM_R.Value as RevenueCode,              
 GroupIDForMileServices=CASE WHEN LEN(N.GroupIDForMileServices)=0 OR N.GroupIDForMileServices IS NULL THEN CONVERT(VARCHAR(100),NEWID()) ELSE N.GroupIDForMileServices END    
 ,N.CalculatedUnit,CalculatedAmount=CONVERT(decimal(8,2),N.CalculatedAmount),N.Rate,  
 CareType=DDM_C.Title  
 FROM Referrals R    
 INNER JOIN Notes N ON R.ReferralID=N.ReferralID    
 INNER JOIN BatchNotes BN ON BN.NoteID=N.NoteID  
 INNER JOIN Payors P ON P.PayorID=N.PayorID                    
 INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID    
 LEFT JOIN ReferralBillingSettings RBS ON RBS.ReferralID=R.ReferralID                  
 LEFT JOIN Physicians PH on PH.PhysicianID = R.PhysicianID                  
 LEFT JOIN ContactMappings CM on CM.ReferralID=R.ReferralID AND CM.ContactTypeID=1 --AND (CM.IsDCSLegalGuardian=1 OR CM.IsPrimaryPlacementLegalGuardian=1)                    
 LEFT JOIN Contacts C on C.ContactID=CM.ContactID                  
 LEFT JOIN ServiceCodes SC ON SC.ServiceCodeID=N.ServiceCodeID                
 LEFT JOIN ReferralBillingAuthorizations RBM on RBM.ReferralID=R.ReferralID AND RBM.Type=@FileType AND RBM.IsDeleted=0 and (CONVERT(date,GETDATE()) between RBM.StartDate and RBM.EndDate)              
 LEFT JOIN DDMaster DDM_C on DDM_C.DDMasterID=SC.CareType    
 LEFT JOIN DDMaster DDM_R on DDM_R.DDMasterID=PSM.RevenueCode                 
 LEFT JOIN DDMaster DDM_P_FC on DDM_P_FC.DDMasterID=RBS.POS_CMS1500               
 LEFT JOIN DDMaster DDM_I_FC on DDM_I_FC.DDMasterID=RBS.POS_UB04               
 LEFT JOIN DDMaster DDM_AT on DDM_AT.DDMasterID=RBS.AdmissionTypeCode_UB04               
 LEFT JOIN DDMaster DDM_AS on DDM_AS.DDMasterID=RBS.AdmissionSourceCode_UB04                 
 LEFT JOIN DDMaster DDM_PS on DDM_PS.DDMasterID=RBS.PatientStatusCode_UB04                 
 WHERE R.ReferralID=@ReferralID AND P.PayorID=@PayorID   
 And BN.BatchID = @BatchID   
END   
    
    
END  