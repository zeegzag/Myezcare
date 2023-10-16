CREATE PROCEDURE [dbo].[HC_PrivateDuty_GetSchEmployeeDetailForPopup]  
@EmployeeID BIGINT,                  
@StartDate DATE,                  
@EndDate DATE,                
@Preference_Skill nvarchar(10),                
@Preference_Preference nvarchar(10)                
                
AS                          
BEGIN                          
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()   
 DECLARE @StartDatePTO DATE=@StartDate    
 DECLARE @EndDatePTO DATE=@EndDate    
     
                   
 IF(@StartDate=@EndDate)                
 BEGIN                 
  SET @EndDate= DATEADD(DAY, 1,@EndDate);                
 END                
                
 DECLARE @InfinateDate DATE='2099-12-31';                  
                  
 SELECT *,EmployeeName=dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat) FROM Employees WHERE EmployeeID=@EmployeeID                
 --SELECT *, STUFF((select ', ' + p.PreferenceName FROM EmployeePreferences ep                
 --LEFT JOIN Preferences p ON ep.PreferenceID=p.PreferenceID                
 --Where ep.EmployeeID=e.EmployeeID AND p.KeyType='Skill'                
 --FOR XML PATH('')                
 --), 1, 1, '') AS Preferences                
 -- FROM Employees e WHERE e.EmployeeID=@EmployeeID                
                  
  DECLARE @EmployeeTimeSlotMasterID BIGINT;                  
 SELECT TOP 1 @EmployeeTimeSlotMasterID=EmployeeTimeSlotMasterID FROM EmployeeTimeSlotMaster WHERE EmployeeID=@EmployeeID AND @StartDate BETWEEN StartDate AND ISNULL(EndDate,@InfinateDate)            
                  
 SELECT * FROM EmployeeTimeSlotMaster WHERE EmployeeTimeSlotMasterID =@EmployeeTimeSlotMasterID                   
                  
                  
                  
                  
 SELECT DISTINCT            
 RemainingSlotCount= SUM(CASE WHEN SM.ScheduleID IS NULL THEN 1 ELSE 0 END) OVER(PARTITION BY ets.EmployeeTimeSlotMasterID,ets.EmployeeTimeSlotDetailID,  ets.DAY)    ,            
 ets.*,StrDayName=DATENAME(dw, Day)--, ISFull= CASE WHEN SM.ScheduleID IS NOT NULL THEN 1 ELSE 0 END ,            
             
             
 FROM EmployeeTimeSlotDetails ets            
 INNER JOIN EmployeeTimeSlotDates etsd ON etsd.EmployeeTimeSlotMasterID=ets.EmployeeTimeSlotMasterID             
    AND etsd.EmployeeTimeSlotDetailID=ets.EmployeeTimeSlotDetailID AND ets.EmployeeTimeSlotMasterID=@EmployeeTimeSlotMasterID AND ets.IsDeleted=0            
 AND (etsd.EmployeeTSDate BETWEEN @StartDate AND @EndDate)            
 LEFT JOIN ScheduleMasters sm ON sm.EmployeeTSDateID=etsd.EmployeeTSDateID AND ISNULL(SM.OnHold, 0) = 0 AND sm.IsDeleted=0            
    ORDER BY Day ASC          
 --SELECT *,StrDayName=DATENAME(dw, Day) FROM EmployeeTimeSlotDetails WHERE EmployeeTimeSlotMasterID=@EmployeeTimeSlotMasterID AND IsDeleted=0            
                  
                   
                  
 SELECT DISTINCT EDO.*,ScheduleCount= SUM(CASE WHEN SM.ScheduleID IS NULL THEN 0 ELSE 1 END) OVER(PARTITION BY EDO.EmployeeDayOffID)                   
 FROM EmployeeDayOffs  EDO                  
 LEFT JOIN EmployeeTimeSlotDates ETD ON  ETD.EmployeeID=EDO.EmployeeID AND (ETD.EmployeeTSDate BETWEEN @StartDatePTO AND @EndDate)                  
 LEFT JOIN ScheduleMasters SM ON SM.EmployeeTSDateID=ETD.EmployeeTSDateID AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted=0                  
 WHERE EDO.EmployeeID=@EmployeeID And EDO.IsDeleted=0    
 AND      
 (    
 ((CONVERT(DATE,EDO.StartTime) BETWEEN @StartDatePTO AND @EndDatePTO) OR (CONVERT(DATE,EDO.EndTime) BETWEEN @StartDatePTO AND @EndDatePTO))    
 or            
 ((@StartDatePTO BETWEEN CONVERT(DATE,EDO.StartTime) AND CONVERT(DATE,EDO.EndTime)) OR (@EndDatePTO BETWEEN CONVERT(DATE,EDO.StartTime) AND CONVERT(DATE,EDO.EndTime)))    
 )    
      
                 
 SELECT p.PreferenceName,p.PreferenceID FROM EmployeePreferences ep                
 INNER JOIN Preferences p ON ep.PreferenceID=p.PreferenceID                
 WHERE ep.EmployeeID=@EmployeeID AND p.KeyType=@Preference_Skill                
                
 SELECT p.PreferenceName,p.PreferenceID FROM EmployeePreferences ep                
 INNER JOIN Preferences p ON ep.PreferenceID=p.PreferenceID                
 WHERE ep.EmployeeID=@EmployeeID AND p.KeyType=@Preference_Preference                
                
 SELECT DISTINCT sm.ReferralID,PatientName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) FROM ScheduleMasters sm                
INNER JOIN Referrals r ON r.ReferralID=sm.ReferralID                
WHERE EmployeeID=@EmployeeID AND r.IsDeleted=0       
AND (CONVERT(DATE,sm.StartDate) BETWEEN @StartDate AND @EndDate OR CONVERT(DATE,sm.EndDate) BETWEEN @StartDate AND @EndDate) AND ISNULL(SM.OnHold, 0) = 0 AND sm.IsDeleted=0      
                   
END  