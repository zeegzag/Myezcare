-- EXEC [HC_CM_CreateBillingNotes_Temporary]  '3','','','2021-08-01','2021-08-02','1',''                  
-- SELECT @PayorID=1, @ClientName='Janet',@StartDate='2021-08-15',@EndDate='2021-08-15',@LoggedInID=1,@LoggedInID=''              
CREATE PROCEDURE [dbo].[HC_CM_CreateBillingNotes_Temporary]                                            
@PayorID BIGINT,                                        
@ServiceCodeIDs NVARCHAR(MAX)=NULL,                                            
@ClientName varchar(max)=null,                                              
@StartDate DATETIME,                                        
@EndDate DATETIME,                                        
@LoggedInID BIGINT,                                            
@SystemID NVARCHAR(MAX)                                            
AS                                            
BEGIN                                            
                                            
BEGIN TRANSACTION trans                                                                                                                
 BEGIN TRY                                              
                                            
                                                                        
DECLARE @BillingProviderName varchar(max);                                                                                
DECLARE @BillingProviderFirstName varchar(max)='';                                                                                
DECLARE @BillingProviderNPI varchar(max);                                                                                
DECLARE @BillingProviderAddress varchar(max);                                                                                
DECLARE @BillingProviderCity varchar(max);                                                                                
DECLARE @BillingProviderState varchar(max);                                                                                
DECLARE @BillingProviderZipcode varchar(max);                                                                                
DECLARE @BillingProviderEIN varchar(max);                                                                             
                                                                          
SELECT TOP 1                                                                          
@BillingProviderName=BillingProvider_NM103_NameLastOrOrganizationName,                                                                                
@BillingProviderNPI=BillingProvider_NM109_IdCode,                                                                                
@BillingProviderAddress=BillingProvider_N301_Address,                                                                                
@BillingProviderCity=BillingProvider_N401_City,                                                                                
@BillingProviderState=BillingProvider_N402_State,                                                                                
@BillingProviderZipcode=BillingProvider_N403_Zipcode,                                                                                
@BillingProviderEIN=BillingProvider_REF02_ReferenceIdentification                                                                                 
FROM OrganizationSettings                                                                          
                                                                                  
                                            
 DECLARE @Temp TABLE(                                             
   --RankOrder INT,                                                                                              
   ReferralID BIGINT,                                                                                              
   AHCCCSID NVARCHAR(100),                                                                                              
   CISNumber NVARCHAR(100),                  
   ServiceDate DATE,                                             
   ServiceStartTime DATETIME,                             
   ServiceEndTime DATETIME,                                                                 
   ServiceCodeID BIGINT,                                 
   ServiceCode NVARCHAR(200),                                                    
   ServiceName NVARCHAR(200),                                                                                              
   Description NVARCHAR(500),                                              
 ModifierID VARCHAR(500),                                              
   MaxUnit INT,                                                                  
   DailyUnitLimit INT,                                             
   UnitType INT,                                            
   PerUnitQuantity FLOAT,                                            
   ServiceCodeType INT,                                            
   ServiceCodeStartDate DATE,                                        
   ServiceCodeEndDate DATE,                                            
   IsBillable BIT,                                            
                                    
   RP_ID BIGINT,                                            
   RP_LastName NVARCHAR(200),                                            
   RP_FirstName NVARCHAR(200),                                          
   RP_Address NVARCHAR(200),                                            
   RP_City NVARCHAR(100),                                            
   RP_StateCode NVARCHAR(100),                                            
   RP_ZipCode NVARCHAR(100),                                            
   RP_HHA_NPI_ID NVARCHAR(100),                                             
                                            
 PayorID BIGINT,                                                                                              
   PayorName NVARCHAR(100),                                             
   PayorShortName NVARCHAR(100),                                                                                              
   PayorAddress NVARCHAR(200),                                                                                           
   PayorIdentificationNumber NVARCHAR(100),                                            
   PayorCity NVARCHAR(100),                                                                                              
   PayorState NVARCHAR(100),                                                                     
   PayorZipCode NVARCHAR(100),                                                                                              
                                                                              
   --PayorServiceCodeMappingID BIGINT,                                            
   --POSStartDate DATE,                                                                                              
   --POSEndDate DATE,                                                          
   Rate FLOAT,                                                
   CalculatedServiceTime BIGINT,                                                    
   CalculatedUnit FLOAT,                                            
   NoteID BIGINT,                                                                       
   AddNewNote BIGINT,                                            
   ReferralTSDateID BIGINT,                                            
   ReferralBillingAuthorizationID BIGINT ,                     
   ReferralTimeSlotDetailID BIGINT ,                   
   RenderingProvider_TaxonomyCode NVARCHAR(100),      
   CalculatedAmount FLOAT                     
  )                                                                                              
                                             
 INSERT INTO  @Temp                                           
SELECT *, CalculatedAmount= CalculatedUnit * Rate FROM            
(                                            
SELECT R.ReferralID,R.AHCCCSID, R.CISNumber,ServiceDate = CONVERT(DATE,RTD.ReferralTSDate),                                                                                              
RTD.ReferralTSStartTime,RTD.ReferralTSEndTime,S.ServiceCodeID,S.ServiceCode,S.ServiceName,S.Description,S.ModifierID,RBA.MaxUnit,RBA.DailyUnitLimit,RBA.UnitType,RBA.PerUnitQuantity,                                            
ServiceCodeType= ISNULL(S.ServiceCodeType,0),S.ServiceCodeStartDate,S.ServiceCodeEndDate,S.IsBillable,                                            
--FacilityID=-1,FacilityLastName=@BillingProviderName,FacilityName=NULL,Address=@BillingProviderAddress,City=@BillingProviderCity,                                        
--State=@BillingProviderState,ZipCode=@BillingProviderZipcode,NPI=@BillingProviderNPI,                                          
FacilityID=-1,FacilityLastName=E.LastName,FacilityName=E.FirstName,Address=E.Address,City=E.City,                                
State=E.StateCode,ZipCode=E.ZipCode, NPI=e.EmployeeUniqueID,            
            
P.PayorID,P.PayorName,P.ShortName,PayorAddress=P.Address,P.PayorIdentificationNumber,PayorCity=P.City,P.StateCode,PayorZipCode=P.ZipCode,                                            
--PSM.PayorServiceCodeMappingID,PSM.POSStartDate,PSM.POSEndDate,                                  
RBA.Rate,                                            
CalculatedServiceTime= 1, --DATEDIFF(MINUTE, SM.StartDate, SM.EndDate),                                            
CalculatedUnit= 1,                              
N.NoteID,AddNewNote= CASE WHEN N.NoteID IS NULL THEN 1 ELSE 0 END,RTD.ReferralTSDateID ,RBA.ReferralBillingAuthorizationID ,                      
                      
RTDS.ReferralTimeSlotDetailID ,  RenderingProvider_TaxonomyCode = DT.Value                  
                                            
FROM ReferralTimeSlotDetails RTDS                                          
 INNER JOIN ReferralTimeSlotDates RTD ON RTDS.ReferralTimeSlotDetailID=RTD.ReferralTimeSlotDetailID AND RTDS.IsDeleted=0                                                     
 INNER JOIN Referrals R ON R.ReferralID=RTD.ReferralID AND RTD.UsedInScheduling=1 AND RTD.OnHold=0                                   
 INNER JOIn Employees E ON E.EmployeeID = R.Assignee                 
            
 INNER JOIN ReferralPayorMappings RPM ON RPM.ReferralID=R.ReferralID AND RPM.Precedence=1 AND RPM.IsDeleted=0                                  
 AND  RTD.ReferralTSDate BETWEEN RPM.PayorEffectiveDate AND RPM.PayorEffectiveEndDate                                        
                            
 INNER JOIN Payors P ON P.PayorID=RPM.PayorID                                          
                                   
 INNER JOIN ReferralBillingAuthorizations RBA ON RBA.PayorID=P.PayorID AND RBA.ReferralID=R.ReferralID                                    
 AND  RTD.ReferralTSDate BETWEEN RBA.StartDate AND RBA.EndDate
 AND RBA.Type='CMS1500' AND RBA.IsDeleted=0
                                   
 --LEFT JOIN ReferralBillingAuthorizationServiceCodes RBAS ON RBAS.ReferralBillingAuthorizationID=RBA.ReferralBillingAuthorizationID                                        
 INNER JOIN ServiceCodes S on S.ServiceCodeID = RBA.ServiceCodeID                                                                 
 LEFT JOIN Modifiers M ON M.ModifierID=S.ModifierID                                                                    
 --LEFT JOIN PayorServiceCodeMapping PSM ON PSM.PayorID=P.PayorID AND PSM.ServiceCodeID=S.ServiceCodeID                                        
                                        
 --INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorID=P.PayorID                                            
 --INNER JOIN ServiceCodes S ON S.ServiceCodeID= PSM.ServiceCodeID AND S.CareType=R.CareTypeIds                                             
 --INNER JOIN Facilities F ON F.FacilityID= SM.FacilityID AND S.CareType=R.CareTypeIds                                             
 LEFT JOIN Notes N ON N.ReferralTSDateID=RTD.ReferralTSDateID  AND N.IsDeleted=0                                
                          
 LEFT JOIN ScheduleMasters SM  ON RTD.ReferralTSDateID=SM.ReferralTSDateID                               
     
 LEFT JOIN DDMaster DT  ON DT.DDMasterID = RBA.TaxonomyID    
                              
WHERE --SM.IsDeleted=0  AND SM.IsPatientAttendedSchedule=1                                           
 1=1                                    
 AND SM.ScheduleID IS NULL                              
 AND (((@StartDate is null OR RTD.ReferralTSDate>= @StartDate) AND (@EndDate is null OR RTD.ReferralTSDate <= @EndDate)))                                                               
 AND RPM.PayorID=@PayorID                        
 -- AND RTD.ReferralID in (SELECT val FROM GETCSVTABLE(@ReferralsIds))                                             
 AND ((@ClientName IS NULL OR LEN(R.LastName)=0)                                       
 OR (                           
    (R.FirstName LIKE '%'+@ClientName+'%' ) OR                                                                
    (R.LastName  LIKE '%'+@ClientName+'%')  OR                                                                
    (R.FirstName +' '+R.LastName like '%'+@ClientName+'%') OR                                                                
    (R.LastName +' '+R.FirstName like '%'+@ClientName+'%') OR                                                                
    (R.FirstName +', '+R.LastName like '%'+@ClientName+'%') OR                                                                
    (R.LastName +', '+R.FirstName like '%'+@ClientName+'%')))                                                            
                                         
                                         
 ) AS T                                 
                                             
                                        
                                        
                                        
                                        
                                        
                                        
                                        
------------------------------------------INSERT & UPDATE NOTES -------------------------------------------------------------------------------------------------                                    
                                              
  INSERT INTO Notes_Temporary (ReferralID,AHCCCSID,CISNumber,ServiceDate,StartTime,EndTime,                                             
  ServiceCodeID,ServiceCode,ServiceName,Description,ModifierID,MaxUnit,DailyUnitLimit,UnitType,PerUnitQuantity,                                                                     
  ServiceCodeType,ServiceCodeStartDate,ServiceCodeEndDate,IsBillable,                                                                             
  --RenderingProviderID,RenderingProviderName,RenderingProviderFirstName,RenderingProviderAddress,RenderingProviderCity,                                                                                              
  --RenderingProviderState,RenderingProviderZipcode,RenderingProviderNPI,                                                                                
  PayorID,PayorName,PayorShortName,PayorAddress,PayorIdentificationNumber,PayorCity,PayorZipcode,                                             
  Rate,CalculatedServiceTime,                                            
  CalculatedUnit,CalculatedAmount,MarkAsComplete,                                            
  BillingProviderID,BillingProviderName,BillingProviderEIN,BillingProviderNPI,                                                                          
  BillingProviderAddress,BillingProviderCity,BillingProviderState,BillingProviderZipcode,                                                                  
  ReferralTSDateID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,ReferralBillingAuthorizationID,ReferralTimeSlotDetailID,RenderingProvider_TaxonomyCode)                                                                         
                                            
  SELECT ReferralID,AHCCCSID,CISNumber,ServiceDate,ServiceStartTime,ServiceEndTime,                                                                                              
  ServiceCodeID,ServiceCode,ServiceName,Description,ModifierID,MaxUnit,DailyUnitLimit,UnitType,PerUnitQuantity,                                                                             
  ServiceCodeType,ServiceCodeStartDate,ServiceCodeEndDate,IsBillable,                                                                
  --RP_ID,RP_LastName,RP_FirstName,RP_Address,RP_City,RP_StateCode,RP_ZipCode,RP_HHA_NPI_ID,                                                                                              
  PayorID,PayorName,PayorShortName,PayorAddress,PayorIdentificationNumber,PayorCity,PayorZipcode,                                         
  Rate,CalculatedServiceTime,CalculatedUnit,CalculatedAmount,1,                                           
  -1,@BillingProviderName,@BillingProviderEIN,@BillingProviderNPI,                                                                          
  @BillingProviderAddress,@BillingProviderCity,@BillingProviderState,@BillingProviderZipcode,           
  T1.ReferralTSDateID,GETDATE(),@LoggedInID,GETDATE(),@LoggedInID,@SystemID,ReferralBillingAuthorizationID  ,                                       
  T1.ReferralTimeSlotDetailID  , T1.RenderingProvider_TaxonomyCode                      
  FROM @Temp T1 WHERE T1.AddNewNote=1                                             
                                                                                              
  --UPDATE DATA INTO NOTES TABLE                                            
  UPDATE N SET                                             
  N.ServiceCodeID=T.ServiceCodeID,                                                                                              
  N.ServiceCode=T.ServiceCode,                                                                                              
  N.ServiceName=T.ServiceName,                                                                                              
  N.Description=T.Description,                      
  N.MaxUnit=T.MaxUnit,                                                                                              
  N.DailyUnitLimit=T.DailyUnitLimit,                                            
  N.UnitType=T.UnitType,                                                                                              
  N.PerUnitQuantity=T.PerUnitQuantity,                                                                                   
  N.ServiceCodeType=T.ServiceCodeType,                                                
  N.ServiceCodeStartDate=T.ServiceCodeStartDate,                                                                         
  N.ServiceCodeEndDate=T.ServiceCodeEndDate,                                                                                              
  N.IsBillable=T.IsBillable,                            
  N.Rate=T.Rate,                                                                                 
  N.CalculatedUnit=T.CalculatedUnit,                                                                             
  N.CalculatedAmount=T.CalculatedAmount,                                                                                              
  N.CalculatedServiceTime=T.CalculatedServiceTime,                                                                                              
  N.ModifierID=T.ModifierID,                                                                                  
  --N.PayorServiceCodeMappingID=T.PayorServiceCodeMappingID,                                                                               
          
  --N.RenderingProviderName=T.RP_LastName,                                 
  --N.RenderingProviderFirstName=T.RP_FirstName,                                                                                  
  --N.RenderingProviderAddress=T.RP_Address,                                                                                  
  --N.RenderingProviderCity=T.RP_City,                                                              
  --N.RenderingProviderState=T.RP_StateCode,                                                                                  
  --N.RenderingProviderZipcode=T.RP_ZipCode,                                                                                  
  --N.RenderingProviderNPI=T.RP_HHA_NPI_ID,                                                                          
  N.BillingProviderName=@BillingProviderName,                       
  N.BillingProviderNPI=@BillingProviderNPI,                                                                          
  N.BillingProviderEIN=@BillingProviderEIN,                                       
  N.BillingProviderAddress=@BillingProviderAddress,                                                                          
  N.BillingProviderCity=@BillingProviderCity,                                                                          
  N.BillingProviderState=@BillingProviderState,                                                                          
  N.BillingProviderZipcode=@BillingProviderZipcode,                                                                  
  N.UpdatedDate=GETDATE(),                                           
  N.UpdatedBy=@LoggedInID   ,                              
  N.ReferralBillingAuthorizationID = T.ReferralBillingAuthorizationID,      
  N.RenderingProvider_TaxonomyCode = T.RenderingProvider_TaxonomyCode      
  FROM Notes_Temporary N                                                                                            
  INNER JOIN @Temp T ON T.NoteID=N.NoteID                                            
  WHERE T.AddNewNote=0                                              
                                            
                                            
                                            
                                                           
    --SELECT 1 AS TransactionResultId;                                                                                              
                                                                
  IF @@TRANCOUNT > 0                                                                                                                
  BEGIN                                                                                                                 
   COMMIT TRANSACTION trans                                                                                                        
  END                                                                      
 END TRY                                                                                                 
                                              
  BEGIN CATCH                                                                                  
       --SELECT -1 AS TransactionResultId,ERROR_MESSAGE() AS ErrorMessage;                                                                                                                
       IF @@TRANCOUNT > 0                                                        
     BEGIN                                                                                                                
      ROLLBACK TRANSACTION trans                                       
     END                                                                                                  
 END CATCH                    
END 