CREATE PROCEDURE [dbo].[SaveEmployeeVisit]    
@EmployeeVisitID BIGINT,          
 @ClockInTime TIME,                      
 @ClockOutTime TIME,    
 @BeforeClockIn INT,    
 @UpdatedBy BIGINT    
AS                      
BEGIN    
 DECLARE @ScheduleID BIGINT;    
 SET @ScheduleID = (SELECT ScheduleID FROM EmployeeVisits WHERE EmployeeVisitID=@EmployeeVisitID)    
     
    
 IF((SELECT DATEDIFF(MINUTE,(SELECT StartDate FROM ScheduleMasters WHERE ScheduleID=@ScheduleID),CONVERT(DATETIME, CONVERT(CHAR(8), (Select COALESCE(ClockInTime,GETDATE()) From EmployeeVisits Where EmployeeVisitID=@EmployeeVisitID), 112) + ' ' + CONVERT(CHAR(8), @ClockInTime, 108)))) < @BeforeClockIn)    
 BEGIN    
 SELECT -1 AS Result RETURN;    
 END    
    
      
 UPDATE EmployeeVisits        
   SET ClockInTime=CONVERT(DATETIME, CONVERT(CHAR(8), (Select COALESCE(ClockInTime,GETDATE()) From EmployeeVisits Where EmployeeVisitID=@EmployeeVisitID), 112) + ' ' + CONVERT(CHAR(8), @ClockInTime, 108)),      
    ClockOutTime=CONVERT(DATETIME, CONVERT(CHAR(8), (Select COALESCE(ClockOutTime,GETDATE()) From EmployeeVisits Where EmployeeVisitID=@EmployeeVisitID), 112) + ' ' + CONVERT(CHAR(8), @ClockOutTime, 108)),      
 UpdatedDate=GETDATE(), UpdatedBy=@UpdatedBy      
   WHERE EmployeeVisitID=@EmployeeVisitID;        
        
   SELECT CONVERT(NVARCHAR(20),CONVERT(TIME(3),ClockInTime)) AS ClockInTime,       
   CONVERT(NVARCHAR(20),CONVERT(TIME(3),ClockOutTime)) AS ClockOutTime FROM EmployeeVisits WHERE EmployeeVisitID=@EmployeeVisitID      
END 
