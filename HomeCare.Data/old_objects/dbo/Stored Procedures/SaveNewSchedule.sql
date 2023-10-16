CREATE PROCEDURE [dbo].[SaveNewSchedule]        
@ScheduleID BIGINT,              
 @StartTime TIME,                          
 @EndTime TIME,            
 @UpdatedBy BIGINT        
AS                          
BEGIN        
       
             
        
          
 UPDATE ScheduleMasters            
   SET StartDate=CONVERT(DATETIME, CONVERT(CHAR(8), (Select COALESCE(StartDate,GETDATE()) From ScheduleMasters Where ScheduleID=@ScheduleID), 112) + ' ' + CONVERT(CHAR(8), @StartTime, 108)),          
    EndDate=CONVERT(DATETIME, CONVERT(CHAR(8), (Select COALESCE(EndDate,GETDATE()) From ScheduleMasters Where ScheduleID=@ScheduleID), 112) + ' ' + CONVERT(CHAR(8), @EndTime, 108)),          
 UpdatedDate=GETDATE(), UpdatedBy=@UpdatedBy          
   WHERE ScheduleID = @ScheduleID;            
      EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @ScheduleID,'222','25'   
   SELECT 1 RETURN;                 
   --SELECT CONVERT(NVARCHAR(20),CONVERT(TIME(3),ClockInTime)) AS ClockInTime,           
   --CONVERT(NVARCHAR(20),CONVERT(TIME(3),ClockOutTime)) AS ClockOutTime FROM EmployeeVisits WHERE EmployeeVisitID=@EmployeeVisitID          
END 