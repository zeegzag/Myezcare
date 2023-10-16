    
-- EXEC [ADC].[API_AddNoteByCareType] 50192, 0, null, 0                  
CREATE PROCEDURE [adc].[API_AddNoteByCareType]         
--@EmployeeVisitNoteID BIGINT=0,                                                                      
 --@ReferralTaskMappingID BIGINT=0,                                                                                        
 @EmployeeVisitID BIGINT=0,                                                                              
 @EmployeeID BIGINT=0,                                                                
 --@Description NVARCHAR(1000)=null,                                                                
 --@ServiceTime BIGINT=0,                                                                              
 @SystemID VARCHAR(100)=null,                        
 @ResultRequired bit=1                                                        
AS                                                                                        
BEGIN 
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
DECLARE @MedicaidType VARCHAR(50); SET @MedicaidType = 'Medicaid'                                              
DECLARE @PayorID BIGINT;                                                                    
SET @PayorID = (Select SM.PayorID FROM EmployeeVisits EV INNER JOIN ScheduleMasters SM ON SM.ScheduleID=EV.ScheduleID WHERE EV.EmployeeVisitID=@EmployeeVisitID)                                                                    
IF (@PayorID IS NULL OR LEN(@PayorID)=0 OR @PayorID=0)                                                                  
BEGIN                                                                    
IF(@ResultRequired = 1) BEGIN                        
  SELECT 1 AS TransactionResultId;                             
  END                                                                        
Return;                                                                   
END                                                                    
                                                            
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
BEGIN TRANSACTION trans                   
 BEGIN TRY                    
--Declare @ScheduleID int                   
-- -- Kunda Rai: 16-06-2020             
-- -- Updating ReferralBillingAuthorizationID into ScheduleMasters is does not exists.                
-- IF((SELECT SM.ReferralBillingAuthorizationID FROM EmployeeVisits EV INNER JOIN ScheduleMasters SM ON SM.ScheduleID = EV.ScheduleID WHERE EV.EmployeeVisitID = @EmployeeVisitID) IS NULL)                  
-- BEGIN                  
-- select @ScheduleID= sm.scheduleID from ScheduleMasters sm inner join employeevisits ev on sm.ScheduleID=ev.ScheduleID where ev.EmployeeVisitID=@EmployeeVisitID                  
                  
--  UPDATE SM SET SM.ReferralBillingAuthorizationID = RBA.ReferralBillingAuthorizationID                  
--  --select ev.employeevisitid,rbmax.AuthorizationCode, sm.ReferralBillingAuthorizationID,rba.ReferralBillingAuthorizationID,rba.CareType,sm.CareTypeId,sm.StartDate                  
--  FROM EmployeeVisits EV INNER JOIN ScheduleMasters SM ON SM.ScheduleID = EV.ScheduleID                  
--          INNER JOIN ReferralBillingAuthorizations RBA ON RBA.CareType = SM.CareTypeId and rba.ReferralID=sm.ReferralID and (sm.StartDate between rba.StartDate and rba.enddate) and (sm.EndDate between rba.startdate and rba.EndDate)                  
--          Inner join PayorServiceCodeMapping psm on psm.ServiceCodeID=RBA.ServiceCodeID and sm.CareTypeId=psm.CareType and psm.CareType=rba.CareType and psm.PayorID=sm.PayorID                  
--          inner join                   
--          (select rba1.ReferralBillingAuthorizationID,rba1.AuthorizationCode,max(enddate) MaxEndDate from ReferralBillingAuthorizations rba1 group by rba1.ReferralBillingAuthorizationID,rba1.AuthorizationCode) as rbmax                   
--             on rbmax.ReferralBillingAuthorizationID=rba.ReferralBillingAuthorizationID and rba.EndDate=rbmax.MaxEndDate                   
--  WHERE RBA.IsDeleted = 0 AND SM.IsDeleted = 0 AND EV.IsDeleted = 0 and sm.ScheduleID=@ScheduleID                  
                  
                   
 --END                  
                  
  DECLARE @Temp TABLE(                                                                                
   RankOrder INT,                                                                                
   ReferralID BIGINT,                                                                                
   AHCCCSID NVARCHAR(100),                                                                                
   CISNumber NVARCHAR(100),                                                                                
   ServiceDate DATE,                                                                                
   ClockInTime DATETIME,                                                                                
   ClockOutTime DATETIME,                                                                        
   ServiceCodeID BIGINT,                                                                                
   ServiceCode NVARCHAR(200),                                                                           
   ServiceName NVARCHAR(200),                                                                                
   Description NVARCHAR(500),                                                                               
   MaxUnit INT,                                              
   DailyUnitLimit INT,                                                                                
   UnitType INT,                                                                                
   PerUnitQuantity FLOAT,                                           
   ServiceCodeType INT,                                                                                
   ServiceCodeStartDate DATE,                                                                                
   ServiceCodeEndDate DATE,                                        
   IsBillable BIT,                                                                                
   EmployeeID BIGINT,                                                   
   EmployeeFirstName NVARCHAR(200),                                                                                
   EmployeeLastName NVARCHAR(200),                   
   EmployeeUniqueID varchar(200),              
   Address NVARCHAR(200),                                                       
   City NVARCHAR(100),                                 
   StateCode NVARCHAR(100),                                                                                
   ZipCode NVARCHAR(100),                                        
   PayorID BIGINT,                                                                                
   PayorName NVARCHAR(100),                                                           
PayorShortName NVARCHAR(100),                                                                                
   PayorAddress NVARCHAR(200),                                                                                
PayorIdentificationNumber NVARCHAR(100),                                                                                
   PayorCity NVARCHAR(100),                                                                                
   PayorState NVARCHAR(100),                                                                                
   PayorZipCode NVARCHAR(100),                                                                                
   EmployeeVisitNoteID BIGINT,                                                                                
   NoteID BIGINT,                                                                                
   POSStartDate DATE,                                                                                
   POSEndDate DATE,                                                                                
   Rate FLOAT,                                    
   SumOfServiceTime BIGINT,                                                                                 
   ModifierID VARCHAR(500),                                                                    
                    
   --PayorServiceCodeMappingID BIGINT,                                        
   AddNewNote BIT,                                    
   RoundUpUnit INT,                                    
   SumOfUnit FLOAT      ,                   
   ReferralBillingAuthorizationID int,                
   RenderingProvider_TaxonomyCode NVARCHAR(100)                
  )                                                                                
                                                                                
  --Insert into Temp Table - Loads All Visit Task And Related Notes                                                                                
  INSERT INTO @Temp    (RankOrder ,ReferralID ,AHCCCSID ,CISNumber ,ServiceDate ,ClockInTime ,ClockOutTime , ServiceCodeID ,ServiceCode , ServiceName ,Description ,  MaxUnit ,DailyUnitLimit ,UnitType ,PerUnitQuantity ,ServiceCodeType ,                
  ServiceCodeStartDate,  ServiceCodeEndDate ,IsBillable ,EmployeeID ,  EmployeeFirstName ,EmployeeLastName ,EmployeeUniqueID ,Address ,City ,StateCode ,ZipCode ,PayorID ,PayorName , PayorShortName ,PayorAddress ,                
  PayorIdentificationNumber ,PayorCity ,PayorState ,PayorZipCode ,EmployeeVisitNoteID ,                 
  NoteID ,POSStartDate ,POSEndDate ,Rate ,SumOfServiceTime , ModifierID , AddNewNote ,RoundUpUnit ,ReferralBillingAuthorizationID,RenderingProvider_TaxonomyCode,SumOfUnit)                                                                              
  SELECT *,                                    
  SumOfUnit=CASE WHEN UnitType=1 THEN                                      
  CONVERT(INT,(SumOfServiceTime/PerUnitQuantity + (CASE WHEN ((SumOfServiceTime % PerUnitQuantity)>= RoundUpUnit) THEN 1 ELSE 0 END )))                                    
   ELSE                                                                                
    1 END                                    
   FROM (                               
  SELECT top 1      RankOrder=ROW_NUMBER() OVER (PARTITION BY EV.EmployeeVisitID, R.ReferralID,SC.ServiceCodeID ORDER BY SC.ServiceCodeID),                                                                       
R.ReferralID, RPM.BeneficiaryNumber as 'AHCCCSID', --R.AHCCCSID,                        
   R.CISNumber,ServiceDate=CONVERT(DATE,EV.ClockInTime),EV.ClockInTime,EV.ClockOutTime,                                                                                
   SC.ServiceCodeID,SC.ServiceCode,SC.ServiceName,SC.Description, RBA.MaxUnit,RBA.DailyUnitLimit,RBA.UnitType,RBA.PerUnitQuantity,ISNULL(SC.ServiceCodeType,0) AS ServiceCodeType,                          
   SC.ServiceCodeStartDate,SC.ServiceCodeEndDate,SC.IsBillable,                                                                                
-- E.EmployeeID,EmployeeFirstName=E.FirstName,EmployeeLastName=E.LastName,E.HHA_NPI_ID,E.Address,E.City,E.StateCode,E.ZipCode,                                                                                
   F.FacilityID, EmployeeFirstName='',   EmployeeLastName= F.FacilityName, F.NPI, F.Address, F.city, F.State,   F.zipcode,                   
   SM.PayorID,P.PayorName,PayorShortName=P.ShortName,PayorAddress=P.Address,PayorIdentificationNumber=P.PayorIdentificationNumber,                                                                    
   PayorCity=P.City,PayorState=P.StateCode,PayorZipCode=P.ZipCode,                                                   
   NULL EmployeeVisitNoteID,                    
   N.NoteID,                                                                                
   --RBA.POSStartDate,RBA.POSEndDate,                         
   RBA.StartDate,RBA.EndDate,RBA.Rate,                         
   SumOfServiceTime=datediff(mi,ev.clockinTime,ev.ClockOutTime),SC.ModifierID,                  
   --RBA.PayorServiceCodeMappingID,                                        
   AddNewNote =CASE WHEN  ISNULL(SUM(N.NoteID) OVER (PARTITION BY R.ReferralID,SC.ServiceCodeID),0) > 0 THEN 0 ELSE 1 END,                                    
   RBA.RoundUpUnit,RBA.ReferralBillingAuthorizationID,  RenderingProvider_TaxonomyCode = DT.Value                 
FROM                              
  Referrals AS R  INNER JOIN                     
  ScheduleMasters AS SM ON R.ReferralID = SM.ReferralID   INNER JOIN                    
        --ReferralTaskMappings AS RMT ON RMT.ReferralID = R.ReferralID INNER JOIN                     
  EmployeeVisits AS EV ON SM.ScheduleID = EV.ScheduleID INNER JOIN                    
  --Employees AS E ON SM.EMPLOYEEID=E.EMPLOYEEID INNER JOIN                    
  Facilities AS F ON SM.FacilityID=F.FacilityID INNER JOIN                    
    --    VisitTasks AS VT ON RMT.VisitTaskID = VT.VisitTaskID INNER JOIN                    
  --      ServiceCodes AS SC ON VT.ServiceCodeID = SC.ServiceCodeID left JOIN  -- Changed by Pallav as the notes were not getting added due to missing Auth Numbers                   
        ReferralPayorMappings AS RPM ON SM.ReferralID = RPM.ReferralID Inner JOIN                    
        ReferralBillingAuthorizations AS RBA  ON RBA.ReferralID = RPM.ReferralID AND RBA.ReferralBillingAuthorizationID = SM.ReferralBillingAuthorizationID                   
  AND RBA.Type='CMS1500' AND RBA.IsDeleted=0 and (CONVERT(date,EV.ClockInTime) between RBA.StartDate and RBA.EndDate) and RBA.PayorID=RPM.PayorID        
  INNER JOIN                   
  ServiceCodes SC on SC.servicecodeid=rba.servicecodeid inner join                  
  Payors AS P ON RPM.PayorID = P.PayorID  left JOIN                    
        --PayorServiceCodeMapping AS PSC ON SC.ServiceCodeID = PSC.ServiceCodeID AND PSC.PAYORID=P.PAYORID and PSC.POSEndDate>=getdate() and sm.caretypeID=psc.caretype inner join                   
  --ReferralBillingAuthorizationServiceCodes AS RB ON RB.ServiceCodeID = PSC.ServiceCodeID and RBA.ReferralBillingAuthorizationID = RB.ReferralBillingAuthorizationID AND PSC.PayorID = RBA.PayorID left  JOIN                      
  Notes N ON N.ServiceCodeID=SC.ServiceCodeID AND N.ReferralID=R.ReferralID  AND  N.EmployeeVisitID=EV.EmployeeVisitID and (N.IsDeleted=0 OR N.IsDeleted IS NULL)                 
  LEFT JOIN DDMaster DM ON DM.DDMasterID = RPM.BeneficiaryTypeID AND DM.Title = @MedicaidType                     
  LEFT JOIN DDMaster DT  ON DT.DDMasterID = RBA.TaxonomyID              
WHERE EV.EmployeeVisitID =@EMPLOYEEVISITID  and ev.isDeleted=0 and ev.isPCACompleted=1 --AND (N.IsDeleted=0 OR N.IsDeleted IS NULL)                      
) AS T                              
--Insert into Note Table - Only New Notes                                              
  INSERT INTO Notes (ReferralID,AHCCCSID,CISNumber,ServiceDate,StartTime,EndTime,                                                                                
  ServiceCodeID,ServiceCode,ServiceName,Description,MaxUnit,DailyUnitLimit,UnitType,PerUnitQuantity,                              
  ServiceCodeType,ServiceCodeStartDate,ServiceCodeEndDate,IsBillable,                                                               
  RenderingProviderID,RenderingProviderName,RenderingProviderFirstName,RenderingProviderAddress,RenderingProviderCity,                                                                                
  RenderingProviderState,RenderingProviderZipcode,RenderingProviderNPI,                                                                  
  PayorID,PayorName,PayorShortName,PayorAddress,PayorIdentificationNumber,PayorCity,PayorZipcode,                                                                                
  POSStartDate,POSEndDate,Rate,CalculatedUnit,CalculatedAmount,CalculatedServiceTime,ModifierID,MarkAsComplete,                  
  --PayorServiceCodeMappingID,                  
  BillingProviderID,                                                            
  BillingProviderName,BillingProviderEIN,BillingProviderNPI,                                                            
  BillingProviderAddress,BillingProviderCity,BillingProviderState,BillingProviderZipcode,EmployeeVisitID,                                                    
  --EmployeeVisitNoteIDs                                               
  CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,ReferralBillingAuthorizationID,RenderingProvider_TaxonomyCode,          
  SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName,SupervisingProvidername2420DLoop_NM104_NameFirst,SupervisingProvidername2420DLoop_REF02_ReferenceId)          
          
  SELECT ReferralID,AHCCCSID,CISNumber,ServiceDate,ClockInTime,ClockOutTime,                                                  
  ServiceCodeID,ServiceCode,ServiceName,Description,MaxUnit,DailyUnitLimit,UnitType,PerUnitQuantity,                                                                                
  ServiceCodeType,ServiceCodeStartDate,ServiceCodeEndDate,IsBillable,                                                            
  -- EmployeeID,    ,       EmployeeFirstName,       Address,City,StateCode,ZipCode,                    
            
  NULL, @BillingProviderName,@BillingProviderFirstName, @BillingProviderAddress,@BillingProviderCity, @BillingProviderState,@BillingProviderZipcode,@BillingProviderNPI,            
            
  --EmployeeID,EmployeeLastName,EmployeeFirstName,@BillingProviderAddress,@BillingProviderCity,@BillingProviderState,@BillingProviderZipcode,                    
  --  CASE  WHEN HHA_NPI_ID is NULL or len(HHA_NPI_ID)=0 then @BillingProviderNPI else HHA_NPI_ID END AS 'HHA_NPI_ID',                                                                                  
  PayorID,PayorName,PayorShortName,PayorAddress,PayorIdentificationNumber,PayorCity,PayorZipcode,       
  POSStartDate,POSEndDate,Rate,SumOfUnit,T1.SumOfUnit*Rate,SumOfServiceTime,ModifierID,1,                  
  --PayorServiceCodeMappingID,                  
  -1,                                                            
  @BillingProviderName,@BillingProviderEIN,@BillingProviderNPI,       
  @BillingProviderAddress,@BillingProviderCity,@BillingProviderState,@BillingProviderZipcode,@EmployeeVisitID,                                                    
  --STUFF(                                                                                
  --       (SELECT ', ' + convert(varchar(10), T2.EmployeeVisitNoteID, 120)                                                                                
  --        FROM @Temp T2                                                                                
  --    where T1.ServiceCodeID = T2.ServiceCodeID                                                                                
  --        FOR XML PATH (''))                                                                                
  --        , 1, 1, ''),                                                                                
GETDATE(),@EmployeeID,GETDATE(),@EmployeeID,@SystemID,ReferralBillingAuthorizationID, RenderingProvider_TaxonomyCode,          
EmployeelastName,EmployeeFirstName,EmployeeUniqueID          
          
  FROM @Temp T1 WHERE T1.RankOrder=1 AND   T1.AddNewNote=1 -- T1.NoteID IS NULL                                                                                
                                                                                
  --Update Notes Table                                                                                
  UPDATE N SET N.ServiceCodeID=T.ServiceCodeID,                                                                                
  N.ServiceCode=T.ServiceCode,                                                                                
  N.ServiceName=T.ServiceName,                                                                                
  N.Description=T.Description,                                                                                
  N.MaxUnit=T.MaxUnit,                                                                                
  N.DailyUnitLimit=T.DailyUnitLimit,                                                                                
  N.UnitType=T.UnitType,                           
  N.PerUnitQuantity=T.PerUnitQuantity,                                                                     
--N.ServiceCodeType=T.ServiceCodeType,                                                                                
  N.ServiceCodeStartDate=T.ServiceCodeStartDate,                                                                                
  N.ServiceCodeEndDate=T.ServiceCodeEndDate,                                                                                
  N.IsBillable=T.IsBillable,                                      
  N.Rate=T.Rate,                                                                   
N.CalculatedUnit=T.SumOfUnit,                                                                                
  N.CalculatedAmount=T.SumOfUnit*T.Rate,                                                                                
  N.CalculatedServiceTime=T.SumOfServiceTime,                                                                                
  N.ModifierID=T.ModifierID,                                                                    
             
             
            
   N.RenderingProviderName = @BillingProviderName,                      
        N.RenderingProviderFirstName = @BillingProviderFirstName,                      
        N.RenderingProviderAddress = @BillingProviderAddress,                      
        N.RenderingProviderCity = @BillingProviderCity,                      
        N.RenderingProviderState = @BillingProviderState,                      
        N.RenderingProviderZipcode = @BillingProviderZipcode,            
        N.RenderingProviderNPI = @BillingProviderNPI,            
            
            
  --N.RenderingProviderName=T.EmployeeLastName,                                                            
  --N.RenderingProviderFirstName=T.EmployeeFirstName,                                                                    
  --N.RenderingProviderAddress=T.Address,                                                          
  --N.RenderingProviderCity=T.City,                                                                    
  --N.RenderingProviderState=T.StateCode,                                                                    
  --N.RenderingProviderZipcode=T.ZipCode,                                                                    
  --N.RenderingProviderNPI=    CASE  WHEN T.HHA_NPI_ID is NULL or len(T.HHA_NPI_ID)=0 then @BillingProviderNPI else T.HHA_NPI_ID END ,                                                                                                                        
 
  --N.RenderingProviderNPI=T.HHA_NPI_ID,                                                            
  N.BillingProviderName=@BillingProviderName,                                                            
  N.BillingProviderNPI=@BillingProviderNPI,                                                            
  N.BillingProviderEIN=@BillingProviderEIN,                                                            
  N.BillingProviderAddress=@BillingProviderAddress,                                                            
  N.BillingProviderCity=@BillingProviderCity,                                                            
  N.BillingProviderState=@BillingProviderState,                                     
  N.BillingProviderZipcode=@BillingProviderZipcode,                                                    
  N.EmployeeVisitID=@EmployeeVisitID,                                                    
  N.UpdatedDate=GETDATE(),                                                                                
  N.UpdatedBy=@EmployeeID   ,                  
  N.ReferralBillingAuthorizationID=T.ReferralBillingAuthorizationID,                  
  N.RenderingProvider_TaxonomyCode = T.RenderingProvider_TaxonomyCode,          
  N.SupervisingProvidername2420DLoop_NM103_NameLastOrOrganizationName  = T.EmployeelastName,          
  N.SupervisingProvidername2420DLoop_NM104_NameFirst      = T.EmployeeFirstName,          
  N.SupervisingProvidername2420DLoop_REF02_ReferenceId     = T.EmployeeUniqueID          
  FROM Notes N                 
  INNER JOIN @Temp T ON T.NoteID=N.NoteID                                                                                
                                                                                  
  ----Update NoteID in EmployeeVisitNotes Table                                                                                
  --UPDATE EVN                                                                                
  --SET EVN.NoteID=N.NoteID,EVN.ServiceCodeID=V.ServiceCodeID,UpdatedDate=GETDATE(),UpdatedBy=@LoggedInID                                                                                
  --FROM Notes N                                                                                
  --INNER JOIN ScheduleMasters SM ON SM.ReferralID=N.ReferralID AND SM.EmployeeID=N.RenderingProviderID AND                                                                                 
  --(N.ServiceDate BETWEEN CONVERT(DATE,SM.StartDate) AND CONVERT(DATE,SM.EndDate)) AND SM.IsDeleted=0                             
  --INNER JOIN EmployeeVisits EV ON EV.ScheduleID=SM.ScheduleID                                                                                
  --INNER JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID AND EVN.IsDeleted=0                                                                                
  --INNER JOIN ReferralTaskMappings RTM ON RTM.ReferralTaskMappingID=EVN.ReferralTaskMappingID                                                          
  --INNER JOIN VisitTasks V ON V.VisitTaskID=RTM.VisitTaskID AND V.VisitTaskType='Task' AND V.ServiceCodeID=N.ServiceCodeID                                                                    
                                                          
                                              
                                         
  ----Update NoteID in EmployeeVisitNotes Table (NEW)                                                                  
  UPDATE EVN                                                                 
  SET EVN.NoteID=N.NoteID,EVN.ServiceCodeID=V.ServiceCodeID,UpdatedDate=GETDATE(),UpdatedBy=@EmployeeID                                                                  
  FROM EmployeeVisitNotes EVN                                                                  
  LEFT JOIN ReferralTaskMappings RTM ON RTM.ReferralTaskMappingID=EVN.ReferralTaskMappingID                                                                  
  LEFT JOIN VisitTasks V ON V.VisitTaskID=RTM.VisitTaskID AND V.VisitTaskType='Task'                                                                  
  INNER JOIN EmployeeVisits EV ON EV.EmployeeVisitID=EVN.EmployeeVisitID                                                          
  INNER JOIN ScheduleMasters SM ON SM.ScheduleID=EV.ScheduleID                                                                  
  LEFT JOIN Notes N ON N.ReferralID=SM.ReferralID AND N.RenderingProviderID=SM.EmployeeID AND N.EmployeeVisitID=EV.EmployeeVisitID AND                                                              
  N.ServiceDate=CONVERT(DATE,EV.ClockInTime) AND N.PayorID=SM.PayorID AND (N.ServiceCodeID=V.ServiceCodeID OR                                 
  (N.ServiceCodeID=EVN.ServiceCodeID AND EVN.ReferralTaskMappingID IS NULL))                                
  WHERE EVN.EmployeeVisitID=@EmployeeVisitID AND EVN.IsDeleted=0                                                                  
                                                                  
                                                          
  --DELETE Notes                                                                
                                              
  DECLARE @DeletedTABLE TABLE (NoteID BIGINT,EmployeeVisitNoteID BIGINT)                                              
  INSERT INTO @DeletedTABLE                                              
  SELECT DISTINCT N.NoteID, EVN.EmployeeVisitNoteID FROM EmployeeVisitNotes EVN                                              
  INNER JOIN Notes N ON EVN.NoteID=N.NoteID AND EVN.IsDeleted=1                                             
  WHERE EVN.EmployeeVisitID=@EmployeeVisitID                                              
                                                
                                              
  UPDATE N SET N.IsDeleted = 1 FROM @DeletedTABLE DN                                          
  INNER JOIN Notes N ON N.NoteID=DN.NoteID                                              
  LEFT JOIN  EmployeeVisitNotes EVN ON DN.EmployeeVisitNoteID!=EVN.EmployeeVisitNoteID AND DN.NoteID=EVN.NoteID AND EVN.IsDeleted=0                                              
  WHERE EVN.NoteID IS NULL                                              
                                      
  UPDATE N SET N.IsDeleted= 0 FROM EmployeeVisitNotes EVN                                                
  INNER JOIN Notes N ON EVN.NoteID=N.NoteID AND EVN.IsDeleted=0                                               
  WHERE EVN.EmployeeVisitID=@EmployeeVisitID                                                      
                                                              
  --UPDATE N SET N.IsDeleted=CASE WHEN (EVN.NoteID IS Null) THEN 1 ELSE 0 END                                                                    
  --FROM Notes N                                                                  
  --LEFT JOIN EmployeeVisitNotes EVN ON EVN.NoteID=N.NoteID AND EVN.ServiceCodeID=N.ServiceCodeID                                                        
                                                                    
----------------------------------------NoteDXCodeMappings-----------------------------------------                                                                                
/*                                                                                
  DECLARE @NoteDxTemp TABLE(                                                                                
   ReferralDXCodeMappingID BIGINT,                                                                      
   ReferralID BIGINT,                                                                                
   NoteID BIGINT,                                                                     
   DXCodeID BIGINT,                                                                           
   DXCodeName VARCHAR(50),                                                                                
   DxCodeType VARCHAR(50),                                                   
   Precedence INT,                                                                                
   StartDate DATE,                                                                                
   EndDate DATE,                                              
   Description VARCHAR(500),                                                                                
   DXCodeWithoutDot VARCHAR(50),                                                                                
   DxCodeShortName VARCHAR(50)                                                                    
  )                      
                                                                                
  INSERT INTO @NoteDxTemp                             
            
    Select                                                                                
  RDM.ReferralDXCodeMappingID,T.ReferralID,T.Noteid,DC.DXCodeID,DC.DXCodeName,DC.DxCodeType,RDM.Precedence,RDM.StartDate,RDM.EndDate,DC.Description,                                                                                
  DC.DXCodeWithoutDot,DCT.DxCodeShortName                                                                                
   FROM ReferralDXCodeMappings RDM                                                                                
   left JOIN Notes T ON T.ReferralID=RDM.ReferralID --AND T.RankOrder=1 --AND T.NoteID IS NULL                                                                                
   Inner join schedulemasters SM on SM.ReferralID=T.ReferralID --and SM.employeeid=T.employeeid                  
   INNER JOIN EmployeeVisits EV on EV.SCheduleID=SM.ScheduleID                   
   INNER JOIN DXCodes DC ON DC.DXCodeID=RDM.DXCodeID                                                
   INNER JOIN DxCodeTypes DCT ON DCT.DxCodeTypeID=DC.DxCodeType                                                            
   WHERE EV.EmployeeVisitID=@EmployeeVisitID                  
                                                                          
 DELETE NDM FROM NoteDXCodeMappings NDM                                                             
 INNER JOIN @NoteDxTemp ndt ON ndt.NoteID=ndm.NoteID AND ndt.ReferralID=ndm.ReferralID AND ndt.DXCodeID!=ndm.DXCodeID                                                                  
                                                             
  INSERT INTO NoteDXCodeMappings                                                                                
  SELECT DISTINCT NDT.ReferralDXCodeMappingID,NDT.ReferralID,NDT.NoteID,NDT.DXCodeID,NDT.DXCodeName,NDT.DxCodeType,NDT.Precedence,NDT.StartDate,NDT.EndDate,                                                                                
  NDT.Description,NDT.DXCodeWithoutDot,NDT.DxCodeShortName                                                                                
  FROM @NoteDxTemp NDT                           
  LEFT JOIN NoteDXCodeMappings NDM ON NDM.ReferralDXCodeMappingID=NDT.ReferralDXCodeMappingID                                                                                
  LEFT JOIN NoteDXCodeMappings NDM1 ON NDM1.NoteID=NDT.NoteID                                                                                
  WHERE NDM.ReferralDXCodeMappingID IS NULL OR NDM1.NoteID IS NULL                                                            
                                                                                  
  --DELETE NoteDXCodeMappings                     
  --DELETE NDM FROM NoteDXCodeMappings NDM          
  --INNER JOIN @NoteDxTemp NDT ON NDT.ReferralDXCodeMappingID=NDM.ReferralDXCodeMappingID                                                                                
  --INNER JOIN @NoteDxTemp NDT1 ON NDT1.NoteID=NDM.NoteID                                                                                
  --WHERE NDT.ReferralDXCodeMappingID IS NULL OR NDT1.NoteID IS NULL                                                            
                                                                    
  --Update NoteDXCodeMappings Table                                            
  Update NDM                                                                          SET NDM.ReferralDXCodeMappingID=temp.ReferralDXCodeMappingID,                                                                                
  NDM.NoteID=temp.NoteID,                                                                                
  NDM.DXCodeID=temp.DXCodeID,                                                                                
  NDM.DXCodeName=temp.DXCodeName,                                                     
  NDM.DxCodeType=temp.DxCodeType,                          
  NDM.Precedence=temp.Precedence,                                                                                
  NDM.StartDate=temp.StartDate,                                                                                
  NDM.EndDate=temp.EndDate,                                                                 
  NDM.Description=temp.Description,                                                                                
  NDM.DXCodeWithoutDot=temp.DXCodeWithoutDot,                                                               
  NDM.DxCodeShortName=temp.DxCodeShortName                                      
  FROM NoteDXCodeMappings NDM                                                                              
  INNER JOIN @NoteDxTemp temp ON NDM.ReferralDXCodeMappingID=temp.ReferralDXCodeMappingID AND NDM.NoteID=temp.NoteID                                                                              
        
*/      
      
      
  INSERT INTO NoteDXCodeMappings        
    SELECT       
    ReferralDXCodeMappingID = NULL,                      
          N.ReferralID,                      
          N.NoteID,                      
          NDT.DXCodeID,                      
          NDT.DXCodeName,                      
          NDT.DxCodeType,                      
          Precedence =  1,                      
          StartDate = NULL,                      
          EndDate = NULL,                     
          NDT.Description,                      
          NDT.DXCodeWithoutDot,                      
          DxCodeShortName  = NULL  ,    
    BatchTypeID =  NULL,    
   BatchID = NULL    
    FROM Notes N       
    LEFT JOIN ReferralBillingAuthorizations RBA ON RBA.ReferralBillingAuthorizationID = N.ReferralBillingAuthorizationID      
    LEFT JOIN DXCodes NDT ON  CHARINDEX(',' + CONVERT(NVARCHAR(MAX),NDT.DXCodeID) + ',', ',' + RBA.DxCodeID + ',') > 0      
    WHERE N.EmployeeVisitID = @EmployeeVisitID      
                                                                                
                                                                                
  ----------------------------------------SignatureLogs-----------------------------------------                                                            
  -- INSERT into SignatureLogs Table                                                                                
  INSERT INTO SignatureLogs (NoteID,Signature,EmployeeSignatureID,SignatureBy,Name,Date,MacAddress,SystemID)                                                                         
  SELECT DISTINCT N.NoteID,ES.SignaturePath,ES.EmployeeSignatureID,ES.EmployeeID,dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat),                                     
  GETDATE(),NULL,@SystemID                                                                               
  FROM EmployeeVisits EV                                                                                
  INNER JOIN ScheduleMasters SM ON SM.ScheduleID=EV.ScheduleID                                                                                
  INNER JOIN Employees E ON E.EmployeeID=SM.EmployeeID                                                                                
  INNER JOIN EmployeeSignatures ES ON ES.EmployeeSignatureID=E.EmployeeSignatureID                                                                                
  INNER JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID                                                   
  INNER JOIN Notes N ON N.NoteID=EVN.NoteID                               
  LEFT JOIN SignatureLogs SL ON SL.NoteID=N.NoteID                                                                             
  WHERE EV.EmployeeVisitID=@EmployeeVisitID AND SL.NoteID IS NULL                                                                                
                                                                      
                                              
  -- DELETE SignatureLogs                                       
  --DELETE FROM SignatureLogs                                                                                
  --WHERE NoteID NOT IN (SELECT NoteID FROM EmployeeVisitNotes WHERE EmployeeVisitID=@EmployeeVisitID AND NoteID is not null)                                                                    
                                                                  
 DELETE FROM SignatureLogs                                                                  
  WHERE NoteID IN (SELECT NoteID FROM Notes WHERE IsDeleted=1)                                                                    
                                                                                
                  
                                                                                
  IF(@ResultRequired = 1) BEGIN                        
  SELECT 1 AS TransactionResultId;                          
 END                                                                              
                                                                                
 IF @@TRANCOUNT > 0                                                                                                  
  BEGIN                                                                                                   
   COMMIT TRANSACTION trans                                                                                          
  END                                                                                                  
 END TRY                                                                                   
                                                                           
 BEGIN CATCH                                
--Changed Select -1 to  1  due to deadlock error message that was coming up on the mobile application. API_AddNote Should not be triggered from the mobile application. -Pallav Date : 08/20/2019                                                            
  IF(@ResultRequired = 1) BEGIN                        
  SELECT 1 AS TransactionResultId;--,ERROR_MESSAGE() AS ErrorMessage;                        
 END                          
  IF @@TRANCOUNT > 0                                                                                                  
  BEGIN                          
   ROLLBACK TRANSACTION trans                                                                                                   
  END                                                                                    
 END CATCH                                                                                
END 