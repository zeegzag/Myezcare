-- HC_GenerateEdiFileModel 0,18,24234,'UB04',1,'2018-08-15','2019-09-15','',''              
-- HC_GenerateEdiFileModel 122,7,4256,'CMS1500'              
CREATE PROCEDURE [dbo].[HC_GenerateEdiFileModel]              
 @BatchID BIGINT=0,              
 @PayorID BIGINT,              
 @ReferralID BIGINT,              
 @FileType varchar(10),  
 @BatchTypeID INT = 1,  
 @StartDate Date=null,                            
 @EndDate Date=null ,  
 @ServiceCodeIDs VARCHAR(MAX)=null,    
 @ClientName VARCHAR(MAX)=null            
AS              
BEGIN              
              
 IF (@BatchID=0)              
 BEGIN      
       
 DECLARE @temp TABLE(      
  NoteID BIGINT,       
  CLM_UNIT BIGINT,    
  CLM_BilledAmount FLOAT,    
  Original_Unit BIGINT,    
  Original_Amount FLOAT,    
  IsUseInBilling BIT    
 )             
 INSERT INTO @temp           
 SELECT            
 N.NoteID,            
 TempCalculatedUnit=           
  CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)             
  THEN PSM.BillingUnitLimit     
  ELSE SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit)) END ,             
 TempCalculatedAmount=           
  CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)             
  THEN CONVERT( DECIMAL(10,2), (SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)) / SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))) * PSM.BillingUnitLimit )             
  ELSE CONVERT(DECIMAL(10,2), SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount))) END,              
 Original_Unit=N.CalculatedUnit,    
 Original_Amount=N.CalculatedAmount,     
 IsUseInBilling = CASE WHEN N.ParentID IS NULL THEN 1 ELSE 0 END            
 FROM Notes N             
 LEFT JOIN ChildNotes CN ON CN.ParentNoteID=N.NoteID    
 INNER JOIN Referrals R ON R.ReferralID=N.ReferralID            
 INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID            
 WHERE          
 N.IsBillable=1  AND N.MarkAsComplete=1 AND N.IsDeleted=0 AND N.GroupID IS NOT NULL          
 AND N.PayorID=@PayorID    
 AND N.ReferralID = @ReferralID   
 AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate)))   
 AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))    
 AND            
 ((@ClientName IS NULL OR LEN(R.LastName)=0)                 
 OR (                
    (R.FirstName LIKE '%'+@ClientName+'%' ) OR                  
    (R.LastName  LIKE '%'+@ClientName+'%')  OR                  
    (R.FirstName +' '+R.LastName like '%'+@ClientName+'%') OR                  
    (R.LastName +' '+R.FirstName like '%'+@ClientName+'%') OR                  
    (R.FirstName +', '+R.LastName like '%'+@ClientName+'%') OR                  
    (R.LastName +', '+R.FirstName like '%'+@ClientName+'%')))              
 AND N.NoteID NOT IN (SELECT NoteID FROM BatchNotes)    
 GROUP BY N.NoteID,N.ParentID,PSM.BillingUnitLimit,N.CalculatedAmount,N.CalculatedUnit            
 ORDER BY N.NoteID     
    
      
 SELECT R.ReferralID,R.FirstName, R.LastName,            
 dbo.GetGeneralNameFormat(R.FirstName, R.LastName) AS PatientName,             
 R.Dob,R.Gender,R.AHCCCSID,R.CISNumber,MedicalRecordNumber=R.AHCCCSID, R.PolicyNumber,               
 AdmissionDate=R.CreatedDate, StrAdmissionDate=CONVERT(varchar,R.CreatedDate, 1),                          
 SubscriberID=R.AHCCCSID, ClaimSubmitterIdentifier=R.AHCCCSID, PatientAccountNumber=R.AHCCCSID,              
 C.Address,C.City,C.State,C.ZipCode,C.Phone1,              
 ModifierName = CASE WHEN (SC.ModifierID IS NULL OR SC.ModifierID ='') THEN '' ELSE                        
 STUFF((SELECT TOP 4 ',' + convert(varchar(500),M.ModifierCode, 120)FROM Modifiers M                                      
 WHERE M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID)) FOR XML PATH (''))  , 1, 1, '')                                  
 END,            
 ContinuedDX = (SELECT  STUFF((SELECT TOP 12 ',' +  F.DxCodeType+':'+F.DXCodeWithoutDot +':' + convert(varchar,F.Precedence) --+'|'+                               
 FROM NoteDXCodeMappings F where F.NoteID=N.NoteID Order BY F.Precedence ASC                                  
 FOR XML PATH('')),1,1,'')),             
 RBS.SpecialProgramCode_CMS1500,RBM.AuthorizationCode,                
 POS_CMS1500=DDM_P_FC.Value,POS_UB04=DDM_I_FC.Value,                
 AdmissionTypeCode_UB04 = DDM_AT.Value,AdmissionSourceCode_UB04 = DDM_AS.Value,PatientStatusCode_UB04 = DDM_PS.Value,                
 PH.PhysicianID,PH.NPINumber as PhysicianNPINumber,PH.FirstName as PhysicianFirstName,PH.LastName as PhysicianLastName,dbo.GetGeneralNameFormat(PH.FirstName,PH.LastName) AS PhysicianFullName,              
 ServiceDateSpan=CONVERT(VARCHAR(10), N.ServiceDate, 112)+'-'+CONVERT(VARCHAR(10), N.ServiceDate, 112),          
 N.NoteID,N.ServiceDate,N.ServiceCodeID,N.ServiceCode,N.PosID,N.BillingProviderID,      
 N.BillingProviderName,N.BillingProviderFirstName,N.BillingProviderAddress,N.BillingProviderCity,      
 N.BillingProviderState,N.BillingProviderZipcode,N.BillingProviderEIN,N.BillingProviderNPI,                      
 N.BillingProviderGSA,N.BillingProviderAHCCCSID,N.RenderingProviderID,N.RenderingProviderName,N.RenderingProviderFirstName,                        
 N.RenderingProviderAddress,N.RenderingProviderCity,N.RenderingProviderState,N.RenderingProviderZipcode,N.RenderingProviderEIN,N.RenderingProviderNPI,                
 N.RenderingProviderGSA,N.RenderingProviderAHCCCSID,                        
 P.PayorID,P.PayorName,PayorShortName = P.ShortName,PayorAddress =P.Address,PayorCity=P.City,PayorState=P.StateCode,PayorZipcode=P.Zipcode,                    
 PayorIdentificationNumber=P.AgencyNPID,                             
 N.RandomGroupID,PSM.BillingUnitLimit,DDM_R.Value as RevenueCode,                        
 GroupIDForMileServices=CASE WHEN LEN(N.GroupIDForMileServices)=0 OR N.GroupIDForMileServices IS NULL THEN CONVERT(VARCHAR(100),NEWID()) ELSE N.GroupIDForMileServices END,    
 --TempCalculatedUnit=          CONVERT(VARCHAR(100),T.CLM_UNIT),   
 CalculatedUnit=CONVERT(float,T.CLM_UNIT),   
 CalculatedAmount=CONVERT(decimal(10,2),T.CLM_BilledAmount),      
 N.Rate,            
 CareType=DDM_C.Title,PS.Subscriber_SBR02_RelationshipCode              
 FROM @temp T    
 INNER JOIN Notes N ON T.NoteID=N.NoteID            
 INNER JOIN Referrals R ON R.ReferralID=N.ReferralID            
 INNER JOIN ChildNotes CN ON CN.NoteID=N.NoteID        
 INNER JOIN Payors P ON P.PayorID=N.PayorID           
 INNER JOIN PayorEdi837Settings PS ON PS.PayorID=P.PayorID                              
 INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID              
 LEFT JOIN ReferralBillingSettings RBS ON RBS.ReferralID=R.ReferralID                            
 LEFT JOIN Physicians PH on PH.PhysicianID = R.PhysicianID                            
 LEFT JOIN ContactMappings CM on CM.ReferralID=R.ReferralID AND CM.ContactTypeID=1 --AND (CM.IsDCSLegalGuardian=1 OR CM.IsPrimaryPlacementLegalGuardian=1)                              
 LEFT JOIN Contacts C on C.ContactID=CM.ContactID                            
 LEFT JOIN ServiceCodes SC ON SC.ServiceCodeID=N.ServiceCodeID                          
 LEFT JOIN ReferralBillingAuthorizations RBM on RBM.ReferralID=R.ReferralID AND RBM.Type='CMS1500' AND RBM.IsDeleted=0 and (CONVERT(date,GETDATE()) between RBM.StartDate and RBM.EndDate)              
 LEFT JOIN DDMaster DDM_C on DDM_C.DDMasterID=SC.CareType              
 LEFT JOIN DDMaster DDM_R on DDM_R.DDMasterID=PSM.RevenueCode                           
 LEFT JOIN DDMaster DDM_P_FC on DDM_P_FC.DDMasterID=RBS.POS_CMS1500                         
 LEFT JOIN DDMaster DDM_I_FC on DDM_I_FC.DDMasterID=RBS.POS_UB04                         
 LEFT JOIN DDMaster DDM_AT on DDM_AT.DDMasterID=RBS.AdmissionTypeCode_UB04                         
 LEFT JOIN DDMaster DDM_AS on DDM_AS.DDMasterID=RBS.AdmissionSourceCode_UB04                           
 LEFT JOIN DDMaster DDM_PS on DDM_PS.DDMasterID=RBS.PatientStatusCode_UB04    
 where    
 T.IsUseInBilling =1                        
 ORDER BY     
 N.ServiceDate    
        
END              
ELSE              
BEGIN                
              
 SELECT BN.BatchID,R.ReferralID,R.FirstName, R.LastName,            
 dbo.GetGeneralNameFormat(R.FirstName, R.LastName) AS PatientName,             
 R.Dob,R.Gender,R.AHCCCSID,R.CISNumber,MedicalRecordNumber=R.AHCCCSID, R.PolicyNumber,               
 AdmissionDate=R.CreatedDate, StrAdmissionDate=CONVERT(varchar,R.CreatedDate, 1),                          
 SubscriberID=R.AHCCCSID, ClaimSubmitterIdentifier=R.AHCCCSID, PatientAccountNumber=R.AHCCCSID,              
 C.Address,C.City,C.State,C.ZipCode,C.Phone1,              
 ModifierName = CASE WHEN (SC.ModifierID IS NULL OR SC.ModifierID ='') THEN '' ELSE                        
    STUFF((SELECT TOP 4 ',' + convert(varchar(500),M.ModifierCode, 120)FROM Modifiers M                                      
    WHERE M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID)) FOR XML PATH (''))  , 1, 1, '')                                  
   END,            
    ContinuedDX = (SELECT  STUFF((SELECT TOP 12 ',' +  F.DxCodeType+':'+F.DXCodeWithoutDot +':' + convert(varchar,F.Precedence) --+'|'+                               
    FROM NoteDXCodeMappings F where F.NoteID=N.NoteID Order BY F.Precedence ASC                                  
    FOR XML PATH('')),1,1,'')),             
 RBS.SpecialProgramCode_CMS1500,RBM.AuthorizationCode,                
 POS_CMS1500=DDM_P_FC.Value,POS_UB04=DDM_I_FC.Value,                
 AdmissionTypeCode_UB04 = DDM_AT.Value,AdmissionSourceCode_UB04 = DDM_AS.Value,PatientStatusCode_UB04 = DDM_PS.Value,                
 PH.PhysicianID,PH.NPINumber as PhysicianNPINumber,PH.FirstName as PhysicianFirstName,PH.LastName as PhysicianLastName,dbo.GetGeneralNameFormat(PH.FirstName,PH.LastName) AS PhysicianFullName,              
 ServiceDateSpan=CONVERT(VARCHAR(10), N.ServiceDate, 112)+'-'+CONVERT(VARCHAR(10), N.ServiceDate, 112),          
 N.NoteID,N.ServiceDate,N.ServiceCodeID,N.ServiceCode,N.PosID,N.BillingProviderID,       
 N.BillingProviderName,N.BillingProviderFirstName,N.BillingProviderAddress,N.BillingProviderCity,      
 N.BillingProviderState,N.BillingProviderZipcode,N.BillingProviderEIN,N.BillingProviderNPI,                    
 N.BillingProviderGSA,N.BillingProviderAHCCCSID,N.RenderingProviderID,N.RenderingProviderName,N.RenderingProviderFirstName,                        
 N.RenderingProviderAddress,N.RenderingProviderCity,N.RenderingProviderState,N.RenderingProviderZipcode,N.RenderingProviderEIN,N.RenderingProviderNPI,                
 N.RenderingProviderGSA,N.RenderingProviderAHCCCSID,                        
 P.PayorID,P.PayorName,PayorShortName = P.ShortName,PayorAddress =P.Address,PayorCity=P.City,PayorState=P.StateCode,PayorZipcode=P.Zipcode,                    
 PayorIdentificationNumber=P.AgencyNPID,                             
 N.RandomGroupID,PSM.BillingUnitLimit,DDM_R.Value as RevenueCode,                        
 GroupIDForMileServices=CASE WHEN LEN(N.GroupIDForMileServices)=0 OR N.GroupIDForMileServices IS NULL THEN CONVERT(VARCHAR(100),NEWID()) ELSE N.GroupIDForMileServices END,              
 CalculatedUnit= BN.CLM_UNIT,        
 CalculatedAmount=CONVERT(decimal(10,2),BN.CLM_BilledAmount),        
 N.Rate,            
 CareType=DDM_C.Title,PS.Subscriber_SBR02_RelationshipCode            
 FROM BatchNotes BN    
 INNER JOIN Notes N ON N.NoteID=BN.NoteID              
 INNER JOIN ChildNotes CN ON CN.NoteID = N.NoteID        
 INNER JOIN Referrals R ON R.ReferralID = N.ReferralID              
 INNER JOIN Payors P ON P.PayorID=N.PayorID                              
 INNER JOIN PayorEdi837Settings PS ON PS.PayorID=P.PayorID                              
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
 WHERE R.ReferralID=@ReferralID AND P.PayorID=@PayorID AND BN.BatchID=@BatchID AND BN.IsUseInBilling = 1     
 ORDER BY     
 N.ServiceDate         
         
 END              
        
END  
