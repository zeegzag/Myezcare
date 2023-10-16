--EXEC API_CheckForClockInClockOut @EmployeeID = N'97', @ReferralIds = N'24234', @Today = N'2018-11-15'
    
CREATE PROCEDURE [dbo].[API_CheckForClockInClockOut]      
 @EmployeeID bigint,              
 @ReferralIds nvarchar(max),      
 @Today Date    
AS              
BEGIN              
 -- SET NOCOUNT ON added to prevent extra result sets from              
 -- interfering with SELECT statements.              
 SET NOCOUNT ON;            
            
 DECLARE @tmpTable TABLE (ReferralId bigint)            
            
 INSERT INTO @tmpTable (ReferralId)            
 SELECT c.RESULT FROM dbo.CSVtoTableWithIdentity(@ReferralIds,',') c            
           
 DECLARE @tmpScheduleMasters TABLE (ScheduleID bigint)            
            
 INSERT INTO @tmpScheduleMasters (ScheduleID)            
 SELECT sm.ScheduleID            
 FROM dbo.ScheduleMasters sm             
 WHERE sm.EmployeeID=@EmployeeID AND CONVERT(DATE,sm.StartDate)=@Today AND sm.ReferralID IN (SELECT * FROM @tmpTable tt) AND sm.IsDeleted=0    
    
 IF ((NOT EXISTS (SELECT 1 FROM dbo.EmployeeVisits ev WHERE ev.ScheduleID IN (SELECT tsm.ScheduleID FROM @tmpScheduleMasters tsm) AND ClockOutTime IS NULL)) AND    
 (NOT EXISTS(SELECT PS.PendingScheduleID FROM dbo.PendingSchedules PS    
 WHERE PS.EmployeeID=@EmployeeID AND CONVERT(DATE,PS.ClockInTime)=@Today AND PS.ReferralID IN (SELECT * FROM @tmpTable tt) AND PS.IsDeleted=0 AND PS.ClockOutTime IS NULL)))    
 SELECT 0; --Say for ClockIn      
 ELSE  
 SELECT 1; --Say for ClockOut      
      
END
