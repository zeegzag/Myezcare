
--EXEC API_UpdateEmployeeVisitTime 132114,20132,170,'04:30:00'    
CREATE PROCEDURE [dbo].[API_UpdateEmployeeVisitTime]          
 @ScheduleID BIGINT,          
 @EmployeeVisitID BIGINT,          
 @EmployeeID BIGINT,          
 @ClockInTime Time=null,          
 @ClockOutTime Time=null          
AS          
BEGIN          
 DECLARE @VisitTime INT          
 DECLARE @ServiceTime INT          
          
 SET @ClockInTime=(CASE WHEN @ClockInTime IS NULL THEN (SELECT CONVERT(Time,ClockInTime) FROM EmployeeVisits WHERE EmployeeVisitID=@EmployeeVisitID) ELSE @ClockInTime END)          
 SET @ClockOutTime=(CASE WHEN @ClockOutTime IS NULL THEN (SELECT CONVERT(Time,ClockOutTime) FROM EmployeeVisits WHERE EmployeeVisitID=@EmployeeVisitID) ELSE @ClockOutTime END)          
          
 SET @VisitTime=(SELECT DATEDIFF(MINUTE,@ClockInTime,@ClockOutTime))          
 SET @ServiceTime=(Select SUM(ServiceTime) From EmployeeVisitNotes Where EmployeeVisitID=@EmployeeVisitID)          
       
 IF(@ClockInTime > (SELECT CONVERT(TIME,EndDate) FROM ScheduleMasters WHERE ScheduleID=@ScheduleID))      
 BEGIN    
 SELECT -2; RETURN;      
 END    
      
 IF (@ClockInTime > @ClockOutTime)    
 BEGIN      
 SELECT -3; RETURN;      
 END    
          
          
 IF((ISNULL(@ServiceTime,0) <= @VisitTime ) OR (@ClockOutTime IS NULL))          
  BEGIN          
   UPDATE EmployeeVisits                  
   SET           
   ClockInTime=CONVERT(DATETIME, CONVERT(CHAR(8), (Select COALESCE(ClockInTime,GETDATE()) From EmployeeVisits Where EmployeeVisitID=@EmployeeVisitID), 112) + ' ' + CONVERT(CHAR(8), @ClockInTime, 108)),                
   ClockOutTime=CONVERT(DATETIME, CONVERT(CHAR(8), (Select COALESCE(ClockOutTime,GETDATE()) From EmployeeVisits Where EmployeeVisitID=@EmployeeVisitID), 112) + ' ' + CONVERT(CHAR(8), @ClockOutTime, 108)),                
   UpdatedDate=GETDATE(), UpdatedBy=@EmployeeID          
   WHERE EmployeeVisitID=@EmployeeVisitID;          
          
   SELECT 1;          
  END          
 ELSE          
  SELECT -1          
          
END

