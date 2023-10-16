CREATE PROCEDURE [dbo].[ChangeSaveNewSchedule]        
@ScheduleID BIGINT,              
 @StartTime TIME,                          
 @EndTime TIME,            
 @UpdatedBy BIGINT,
 @EmployeeID BIGINT=0        
AS                          
BEGIN        
       
  -- =============================================
-- Author:		<Akhilesh Kamal>
-- Create date: <24/12/2020>
-- Description:	<Update EmployeeID when we change schedule>
-- =============================================           
        
          
 UPDATE ScheduleMasters            
   SET StartDate=CONVERT(DATETIME, CONVERT(CHAR(8), (Select COALESCE(StartDate,GETDATE()) From ScheduleMasters Where ScheduleID=@ScheduleID), 112) + ' ' + CONVERT(CHAR(8), @StartTime, 108)),          
    EndDate=CONVERT(DATETIME, CONVERT(CHAR(8), (Select COALESCE(EndDate,GETDATE()) From ScheduleMasters Where ScheduleID=@ScheduleID), 112) + ' ' + CONVERT(CHAR(8), @EndTime, 108)),          
 UpdatedDate=GETDATE(), UpdatedBy=@UpdatedBy ,EmployeeID=@EmployeeID         
   WHERE ScheduleID = @ScheduleID;            
      EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @ScheduleID,'',''   
   SELECT 1 RETURN;                 
   --SELECT CONVERT(NVARCHAR(20),CONVERT(TIME(3),ClockInTime)) AS ClockInTime,           
   --CONVERT(NVARCHAR(20),CONVERT(TIME(3),ClockOutTime)) AS ClockOutTime FROM EmployeeVisits WHERE EmployeeVisitID=@EmployeeVisitID          
END