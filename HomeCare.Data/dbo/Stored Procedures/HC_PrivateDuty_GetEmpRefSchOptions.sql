CREATE PROCEDURE [dbo].[HC_PrivateDuty_GetEmpRefSchOptions]    
 @ReferralID BIGINT = 0,                                                 
 @ScheduleID BIGINT = 0,                                
 @StartDate DATETIME,              
 @EndDate DATETIME,              
 @EmployeeName VARCHAR(MAX)='',                    
 @MileRadius BIGINT=NULL,                    
 --@SkillId BIGINT=0,                    
 --@PreferenceId BIGINT=0,                  
 @StrSkillList VARCHAR(MAX) = NULL,                                    
 @StrPreferenceList VARCHAR(MAX) = NULL,                
 @PreferenceType_Prefernce VARCHAR(100)='Preference',              
 @PreferenceType_Skill VARCHAR(100)='Skill',                 
 @SameDateWithTimeSlot BIT,              
 @SortExpression NVARCHAR(100),                                  
 @SortType NVARCHAR(10),                                
 @FromIndex INT,                                
 @PageSize INT,              
 @SortIndexArray VARCHAR(MAX)                                 
AS                                
BEGIN                     
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()           
          
--SELECT * FROm Referrals WHERE AHCCCSID='A47130732'          
          
SELECT P.PayorName,P.ShortName, P.PayorID, RPM.Precedence FROM ReferralPayorMappings RPM          
INNER JOIN Payors P ON P.PayorID=RPM.PayorID          
WHERE RPM.ReferralID=@ReferralID AND  RPM.Precedence IS NOT NULL AND RPM.IsDeleted=0      
AND  CONVERT(DATE,@StartDate) BETWEEN RPM.PayorEffectiveDate AND RPM.PayorEffectiveEndDate          
ORDER BY RPM.Precedence ASC          
          
          
          
            
SELECT RH.*, CreatedBy =dbo.GetGenericNameFormat(EC.FirstName,EC.MiddleName, EC.LastName,@NameFormat),            
    UpdatedBy =dbo.GetGenericNameFormat(EU.FirstName,EU.MiddleName, EU.LastName,@NameFormat),             
CurrentActiveGroup = CASE WHEN GETDATE() BETWEEN RH.StartDate AND RH.EndDate THEN 1 ELSE 0 END,            
OldActiveGroup = CASE WHEN GETDATE() > RH.EndDate THEN 1 ELSE 0 END            
FROM ReferralOnHoldDetails RH            
INNER JOIN Employees EC ON EC.EmployeeID=RH.CreatedBy            
INNER JOIN Employees EU ON EU.EmployeeID=RH.UpdatedBy            
WHERE RH.IsDeleted=0  AND RH.ReferralID=@ReferralID-- AND (GETDATE() BETWEEN RH.StartDate AND RH.EndDate OR GETDATE() < RH.StartDate)            
ORDER BY StartDate DESC              
            
            
--SELECT MIN(RTD.ReferralTSDate), MAX(RTD.ReferralTSDate) FROM ReferralTimeSlotDates RTD            
--WHERE RTD.ReferralID=@ReferralID AND RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate AND RTD.OnHold=1            
            
            
            
              
              
              
IF(@StrSkillList IS NULL OR LEN(@StrSkillList)=0)              
SET @StrSkillList=NULL;              
              
              
IF(@StrPreferenceList IS NULL OR LEN(@StrPreferenceList)=0)              
SET @StrPreferenceList=NULL;              
              
              
              
DECLARE @DayNumber INT;              
IF(@SameDateWithTimeSlot=1)              
  SELECT @DayNumber= [dbo].[GetWeekDayNumberFromDate](@StartDate);              
              
              
              
DECLARE @InfiniteEndDate DATE='2099-12-31';              
DECLARE @PatientPayorID BIGINT = 0;          
DECLARE @EmployeeID BIGINT = 0;              
DECLARE @EmployeeTSDateID BIGINT=0;              
DECLARE @ReferralTSDateID BIGINT=0;              
SELECT @EmployeeID=EmployeeID,@EmployeeTSDateID=EmployeeTSDateID,@ReferralTSDateID=ReferralTSDateID, @PatientPayorID=PayorID FROM ScheduleMasters WHERE ScheduleID=@ScheduleID              
              
DECLARE @ReferralTimeSlotMasterID BIGINT=0;              
DECLARE @ReferralTimeSlotDetailID BIGINT=0;              
              
IF(@ReferralTSDateID IS NULL OR @ReferralTSDateID=0)              
BEGIN              
              
 SELECT TOP 1 @ReferralTimeSlotMasterID=ReferralTimeSlotMasterID FROM ReferralTimeSlotMaster WHERE ReferralID=@ReferralID AND @StartDate BETWEEN StartDate AND ISNULL (EndDate,@InfiniteEndDate)              
              
 SELECT ReferralID, FirstName, LastName,dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat) AS ReferralName, PatientPayorID=@PatientPayorID FROM Referrals WHERE ReferralID=@ReferralID              
 SELECT TOP 1 * FROM ReferralTimeSlotMaster  RTS WHERE RTS.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID              
              
                
 --SELECT * FROM ReferralTimeSlotDetails RTSD              
 --WHERE ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID AND IsDeleted=0 AND               
 --(@DayNumber IS NULL OR @DayNumber=0 OR (@DayNumber=RTSD.Day AND CONVERT(TIME,@StartDate) BETWEEN RTSD.StartTime AND RTSD.EndTime))              
 --ORDER BY DAY ASC,StartTime ASC              
              
 SELECT DISTINCT RTSD.* ,              
 RemainingSlotCount= SUM(CASE WHEN SM.ScheduleID IS NULL THEN 1 ELSE 0 END) OVER(PARTITION BY RTSD.ReferralTimeSlotMasterID,RTSD.ReferralTimeSlotDetailID, RTSD.DAY)              
 FROM ReferralTimeSlotDetails RTSD              
 INNER JOIN ReferralTimeSlotDates RTD ON RTD.ReferralTimeSlotDetailID=RTSD.ReferralTimeSlotDetailID              
 AND (@DayNumber IS NULL OR @DayNumber=0 OR (@DayNumber=RTSD.Day AND CONVERT(TIME,@StartDate) BETWEEN RTSD.StartTime AND RTSD.EndTime))              
 AND RTD.ReferralTSDate BETWEEN CONVERT(DATE,@StartDate) AND CONVERT(DATE,@EndDate) AND RTD.UsedInScheduling=1              
 LEFT JOIN ScheduleMasters SM ON SM.ReferralTSDateID=RTD.ReferralTSDateID AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted=0              
              
 WHERE RTSD.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID AND RTSD.IsDeleted=0 AND               
 (@DayNumber IS NULL OR @DayNumber=0 OR (@DayNumber=RTSD.Day AND CONVERT(TIME,@StartDate) BETWEEN RTSD.StartTime AND RTSD.EndTime))              
 ORDER BY DAY ASC,StartTime ASC              
              
              
              
              
END              
ELSE              
BEGIN              
              
  --SELECT * FROM ScheduleMasters WHERE ScheduleID=57829              
  --SELECT * FROM ReferralTimeSlotDates WHERE ReferralTSDateID=61              
  -- SELECT * FROM  ReferralTimeSlotMaster WHERE ReferralTimeSlotMasterID=1              
  SELECT @ReferralTimeSlotMasterID=ReferralTimeSlotMasterID,@ReferralTimeSlotDetailID=ReferralTimeSlotDetailID FROM ReferralTimeSlotDates WHERE ReferralTSDateID=@ReferralTSDateID              
              
  SELECT ReferralID, FirstName, LastName,dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat) AS ReferralName, PatientPayorID=@PatientPayorID FROM Referrals WHERE ReferralID=@ReferralID              
  SELECT TOP 1 * FROM ReferralTimeSlotMaster  RTS WHERE RTS.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID              
  --SELECT * FROM ReferralTimeSlotDates              
  SELECT RTSD.* FROM ReferralTimeSlotDetails  RTSD WHERE ReferralTimeSlotDetailID=@ReferralTimeSlotDetailID              
  ORDER BY DAY ASC,StartTime ASC              
              
  IF(@ReferralTimeSlotDetailID IS NULL)              
  SET @ReferralTimeSlotDetailID=0;              
               
               
END              
              
              
               
DECLARE @TotalPrefeCount BIGINT=0;              
DECLARE @TotalSkillCount BIGINT=0;              
              
SELECT                
@TotalPrefeCount=SUM(CASE WHEN PR.KeyType=@PreferenceType_Prefernce THEN 1 ELSE 0 END) OVER (PARTITION BY RP.ReferralID),              
@TotalSkillCount=SUM(CASE WHEN PR.KeyType=@PreferenceType_Skill THEN 1 ELSE 0 END) OVER (PARTITION BY RP.ReferralID)              
FROM ReferralPreferences  RP              
INNER JOIN Preferences PR ON PR.PreferenceID=RP.PreferenceID              
WHERE ReferralID=@ReferralID              
              
IF(@TotalPrefeCount=0) SET @TotalPrefeCount=1;              
IF(@TotalSkillCount=0) SET @TotalSkillCount=1;              
              
-- PRINT @TotalPrefeCount              
-- PRINT @TotalSkillCount              
              
          
               
 PRINT @SameDateWithTimeSlot;              
          
 PRINT @EmployeeID   
              
 --SELECT * FROM @TempGetEmpTimeSlots              
-- SELECT * FROM @TempGetEmpTimeSlots              
               
DECLARE @TempGetEmpoyeePreferences Table(              
   ReferralID BIGINT,              
   RefPreferenceID BIGINT,              
   EmpPreferenceID  BIGINT,              
   EmployeeID BIGINT,              
   FirstName VARCHAR(5000),            
   LastName VARCHAR(5000), 
   EmployeeName VARCHAR(5000),
   IsDeleted BIT,              
   OrderRank INT,              
   PreferencesMatchPercent INT,              
   SkillsMatchPercent INT,              
   EmpLatLong GeoGraphy,              
   KeyType VARCHAR(MAX),  
   Email VARCHAR(100),              
   PhoneWork VARCHAR(20)          
)               
;WITH CTETempGetEmpoyeePreferences AS               
 (               
              
SELECT ReferralID,RefPreferenceID,EmpPreferenceID,EmployeeID,FirstName,LastName,EmployeeName, IsDeleted,OrderRank,PreferencesMatchPercent,SkillsMatchPercent,EmpLatLong,KeyType,Email,PhoneWork     --EmployeeTimeSlotDetailIDs, Frequency, StartTime, EndTime,ETMStartDate,
ETMEndDate              
 FROM (              
              
SELECT R.ReferralID,PE.KeyType,              
RefPreferenceID=RP.PreferenceID,EmpPreferenceID=EP.PreferenceID,E.EmployeeID,E.FirstName,E.LastName,dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS EmployeeName,
E.IsDeleted, EmpLatLong=[dbo].GetGeoFromLatLng(E.Latitude,E.Longitude),              
PreferencesMatchPercent=SUM(CASE WHEN PE.KeyType=@PreferenceType_Prefernce AND RP.PreferenceID IS NOT NULL THEN 1 ELSE 0 END) OVER( PARTITION BY EP.EmployeeID) * 100/@TotalPrefeCount ,              
SkillsMatchPercent=SUM(CASE WHEN PE.KeyType=@PreferenceType_Skill AND RP.PreferenceID IS NOT NULL THEN 1 ELSE 0 END) OVER( PARTITION BY EP.EmployeeID) * 100/@TotalSkillCount , E.Email, E.PhoneWork,              
  
OrderRank= DENSE_RANK() OVER(PARTITION BY  EP.EmployeeID ORDER BY R.ReferralID DESC ,EP.EmployeePreferenceID DESC)                
FROM Employees E   
LEFT JOIN ReferralBlockedEmployees RBE ON RBE.ReferralID=@ReferralID AND RBE.EmployeeID= E.EmployeeID AND RBE.IsDeleted=0            
LEFT JOIN EmployeePreferences EP ON E.EmployeeID=EP.EmployeeID              
LEFT JOIN ReferralPreferences RP ON RP.PreferenceID=EP.PreferenceID AND RP.ReferralID=@ReferralID              
LEFT JOIN Preferences PE ON PE.PreferenceID=EP.PreferenceID              
LEFT JOIN (SELECT ReferralID=@ReferralID) R ON  R.ReferralID=RP.ReferralID                          
WHERE 1=1    AND RBE.ReferralBlockedEmployeeID IS NULL              
AND ((@EmployeeName IS NULL OR LEN(@EmployeeName)=0 OR E.FirstName IS NULL)             
    OR            
   ( (E.FirstName LIKE '%'+@EmployeeName+'%' )OR              
  (E.LastName  LIKE '%'+@EmployeeName+'%') OR              
  (E.FirstName +' '+E.LastName like '%'+@EmployeeName+'%') OR              
  (E.LastName +' '+E.FirstName like '%'+@EmployeeName+'%') OR              
  (E.FirstName +', '+E.LastName like '%'+@EmployeeName+'%') OR              
  (E.LastName +', '+E.FirstName like '%'+@EmployeeName+'%'))            
   )               
AND (  (@StrSkillList IS NULL  AND @StrPreferenceList IS NULL ) OR              
      ((@StrSkillList IS NULL  AND @StrPreferenceList IS NOT NULL ) AND (EP.PreferenceID IN (SELECT CONVERT(BIGINT, VAL) FROM GetCSVTable(@StrPreferenceList)) )) OR              
      ((@StrSkillList IS NOT NULL  AND @StrPreferenceList IS NULL) AND (EP.PreferenceID IN (SELECT CONVERT(BIGINT, VAL) FROM GetCSVTable(@StrSkillList)) )) OR              
      ((@StrSkillList IS NOT NULL AND @StrPreferenceList IS NOT NULL) AND               
      (              
       (EP.PreferenceID IN (SELECT CONVERT(BIGINT, VAL) FROM GetCSVTable(@StrPreferenceList)) ) OR              
       (EP.PreferenceID IN (SELECT CONVERT(BIGINT, VAL) FROM GetCSVTable(@StrSkillList)) )              
      )              
                    
 ))              
                  
) AS Temp WHERE 1=1 AND Temp.OrderRank=1               
              
              
)              
              
INSERT INTO @TempGetEmpoyeePreferences              
SELECT * FROM CTETempGetEmpoyeePreferences              
            
UPDATE @TempGetEmpoyeePreferences SET ReferralID=@ReferralID            
              
     
                 
                 
                 
Declare @Item1 varchar(max), @Item2 varchar(max), @Item3 varchar(max), @Item4 varchar(max);                
select @Item1=splitdata  from dbo.fnSplitString(@SortIndexArray,',')  WHERE ROWID=1                
select @Item2=splitdata  from dbo.fnSplitString(@SortIndexArray,',')  WHERE ROWID=2                
select @Item3=splitdata  from dbo.fnSplitString(@SortIndexArray,',')  WHERE ROWID=3                
select @Item4=splitdata  from dbo.fnSplitString(@SortIndexArray,',')  WHERE ROWID=4                
                 
                 
                 
PRINT  @StartDate;              
PRINT  @EndDate;               
          
                    
 ;WITH CTEEmployeeTSList AS                                
 (                                 
  SELECT *,COUNT(t1.EmployeeID) OVER() AS Count FROM                                 
  (                                
   SELECT ROW_NUMBER() OVER (ORDER BY                                 
                  
 CASE WHEN  'Skills ASC'= @Item1  THEN  TBL1.SkillsMatchPercent  END  ASC, CASE WHEN  @Item1='Skills DESC'  THEN  TBL1.SkillsMatchPercent  END DESC,                        
 CASE WHEN  'Preferences ASC'=@Item1 THEN  TBL1.PreferencesMatchPercent  END  ASC, CASE WHEN 'Preferences DESC'=@Item1  THEN  TBL1.PreferencesMatchPercent  END DESC,            
 CASE WHEN  'Miles ASC'=@Item1 THEN  TBL1.Distance END  ASC,CASE WHEN 'Miles DESC'=@Item1  THEN TBL1.Distance END DESC,                          
 CASE WHEN  'Conflicts ASC'= @Item1 THEN  TBL1.Conflicts END  ASC, CASE WHEN 'Conflicts DESC'= @Item1  THEN  TBL1.Conflicts   END DESC,               
               
 CASE WHEN  'Skills ASC'= @Item2  THEN  TBL1.SkillsMatchPercent  END  ASC, CASE WHEN  @Item2='Skills DESC'  THEN  TBL1.SkillsMatchPercent  END DESC,                          
 CASE WHEN  'Preferences ASC'=@Item2 THEN  TBL1.PreferencesMatchPercent  END  ASC, CASE WHEN 'Preferences DESC'=@Item2  THEN  TBL1.PreferencesMatchPercent  END DESC,            
 CASE WHEN  'Miles ASC'=@Item2 THEN  TBL1.Distance END  ASC,CASE WHEN 'Miles DESC'=@Item2  THEN TBL1.Distance END DESC,                          
 CASE WHEN  'Conflicts ASC'= @Item2 THEN  TBL1.Conflicts END  ASC, CASE WHEN 'Conflicts DESC'= @Item2  THEN  TBL1.Conflicts   END DESC,               
               
 CASE WHEN  'Skills ASC'= @Item3  THEN  TBL1.SkillsMatchPercent  END  ASC, CASE WHEN  @Item3='Skills DESC'  THEN  TBL1.SkillsMatchPercent  END DESC,                          
 CASE WHEN  'Preferences ASC'=@Item3 THEN  TBL1.PreferencesMatchPercent  END  ASC, CASE WHEN 'Preferences DESC'=@Item3  THEN  TBL1.PreferencesMatchPercent  END DESC,            
 CASE WHEN  'Miles ASC'=@Item3 THEN  TBL1.Distance END  ASC,CASE WHEN 'Miles DESC'=@Item3  THEN TBL1.Distance END DESC,                          
 CASE WHEN  'Conflicts ASC'= @Item3 THEN  TBL1.Conflicts END  ASC, CASE WHEN 'Conflicts DESC'= @Item3  THEN  TBL1.Conflicts   END DESC,               
               
               
 CASE WHEN  'Skills ASC'= @Item4  THEN  TBL1.SkillsMatchPercent  END  ASC, CASE WHEN  @Item4='Skills DESC'  THEN  TBL1.SkillsMatchPercent  END DESC,                          
 CASE WHEN  'Preferences ASC'=@Item4 THEN  TBL1.PreferencesMatchPercent  END  ASC, CASE WHEN 'Preferences DESC'=@Item4  THEN  TBL1.PreferencesMatchPercent  END DESC,                       
 CASE WHEN  'Miles ASC'=@Item4 THEN  TBL1.Distance END  ASC,CASE WHEN 'Miles DESC'=@Item4  THEN TBL1.Distance END DESC,                          
 CASE WHEN  'Conflicts ASC'= @Item4 THEN  TBL1.Conflicts END  ASC, CASE WHEN 'Conflicts DESC'= @Item4  THEN  TBL1.Conflicts   END DESC,                
                        
                         
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Employee' THEN TBL1.FirstName END END ASC,                                
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Employee' THEN TBL1.FirstName END END DESC,  
  
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Email' THEN TBL1.Email END END ASC,                                
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Email' THEN TBL1.Email END END DESC,              
              
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PhoneWork' THEN TBL1.PhoneWork END END ASC,                                
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PhoneWork' THEN TBL1.PhoneWork END END DESC  
               
 --CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Frequency' THEN TBL1.Frequency END END ASC,                                
 --   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Frequency' THEN TBL1.Frequency END END DESC,              
              
 --CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'StartTime' THEN TBL1.StartTime END END ASC,                                
 --   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'StartTime' THEN TBL1.StartTime END END DESC,              
              
 --CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EndTime' THEN TBL1.EndTime END END ASC,                                
 --   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EndTime' THEN TBL1.EndTime END END DESC              
              
                                 
                    
  ) AS Row,  * FROM (                      
                   
  SELECT * FROM (              
  SELECT  E.*,              
  Distance=              
  CASE WHEN EmpLatLong IS NULL OR C.Latitude IS NULL OR C.Longitude IS NULL THEN NULL ELSE               
  (EmpLatLong.STDistance([dbo].GetGeoFromLatLng(C.Latitude,C.Longitude)) * 0.000621371) END,--  RefLatLong=[dbo].GetGeoFromLatLng(C.Latitude,C.Longitude),              
              
  SUM(CASE WHEN SM.ScheduleID IS NOT NULL THEN 1 ELSE 0 END) OVER ( PARTITION BY E.EmployeeID) AS Conflicts,              
  DENSE_RANK() OVER ( PARTITION BY E.EmployeeID ORDER BY SM.ScheduleID ASC) AS EmployeeRank, SM.ScheduleID          
  FROM @TempGetEmpoyeePreferences E              
  LEFT JOIN ScheduleMasters SM ON SM.EmployeeID = E.EmployeeID AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted=0 AND SM.ReferralID!=@ReferralID               
  AND (SM.EmployeeID IS NOT NULL AND SM.ReferralTSDateID IS NOT NULL)              
  --LEFT JOIN EmployeeTimeSlotDates ETD ON ETD.EmployeeTSDateID = SM.EmployeeTSDateID            
         
  LEFT JOIN ContactMappings CM ON CM.ReferralID = E.ReferralID AND CM.ContactTypeID=1              
  LEFT JOIN Contacts C ON C.ContactID= CM.ContactID               
              
  ) AS TEMP WHERE 1=1 AND EmployeeRank=1 AND               
  (@MileRadius IS NULL OR Distance < @MileRadius)              
              
              
  --ORDER BY ReferralID DESC,Distance ASC,SkillsMatchCount ASC, PreferencesMatchCount DESC, FirstName ASC              
               
                        
  )   AS TBL1               
                         
                      
                         
                         
                    
  ) AS t1  )                                
  
  
                           
 SELECT * FROM CTEEmployeeTSList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                 
               
               
              
                               
                                
END  
GO