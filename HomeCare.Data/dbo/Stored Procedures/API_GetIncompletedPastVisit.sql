CREATE PROCEDURE [dbo].[API_GetIncompletedPastVisit]          
 @ReferralID BIGINT,                      
 @EmployeeID BIGINT,                
 @CurrentDateTime NVARCHAR(100)                
AS                                        
BEGIN              
        DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()                                   
   SELECT DISTINCT sm.ScheduleID,EmployeeName = dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat),e.FirstName,ss.ScheduleStatusName,ev.ClockInTime,ev.ClockOutTime,sm.StartDate,sm.EndDate,sm.ReferralID,sm.EmployeeID,            
   ImageURL=r.ProfileImagePath,            
  CASE WHEN UsedInScheduling=1 THEN 0 ELSE 1 END AS IsDenied                      
    FROM ScheduleMasters sm                                      
 INNER JOIN Employees e ON e.EmployeeID=sm.EmployeeID                                      
 INNER JOIN ScheduleStatuses ss ON ss.ScheduleStatusID=sm.ScheduleStatusID            
 INNER JOIN Referrals r ON r.ReferralID=sm.ReferralID            
 INNER JOIN ReferralTimeSlotDates rtd ON rtd.ReferralTSDateID=sm.ReferralTSDateID                
 LEFT JOIN EmployeeVisits ev ON ev.ScheduleID=sm.ScheduleID                      
 Where sm.ReferralID=@ReferralID --AND ((ev.IsSigned=1 AND ev.IsPCACompleted=1) OR (rtd.UsedInScheduling=0 AND sm.IsDeleted=1))                      
 AND sm.EmployeeID=@EmployeeID AND ev.ClockInTime IS NOT NULL AND ev.ClockOutTime IS NOT NULL AND ev.IsPCACompleted=0 AND ev.IsSigned=0 --AND ev.SurveyCompleted=1   
 AND CONVERT(DATETIME,sm.StartDate)<=CONVERT(DATETIME,@CurrentDateTime)          
END  