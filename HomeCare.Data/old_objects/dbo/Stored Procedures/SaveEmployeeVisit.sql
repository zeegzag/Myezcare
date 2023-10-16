  
 -- EXEC SaveEmployeeVisit    10177,    
CREATE PROCEDURE [dbo].[SaveEmployeeVisit]            
  @EmployeeVisitID BIGINT,                  
  @ClockInTime DateTime,                              
  @ClockOutTime DateTime,            
  @BeforeClockIn INT,            
  @UpdatedBy BIGINT            
AS                              
BEGIN            
 DECLARE @ScheduleID BIGINT;           
 Declare @StartDate Date, @EndDate Date;         
 SET @ScheduleID = (SELECT ScheduleID FROM EmployeeVisits WHERE EmployeeVisitID=@EmployeeVisitID)        
   --Updated by:Akhilesh        
 --Updated date: 02-06-2020        
 --Desc: to Remove validation check for early clockIn and Invalid ClockIn   
  
  -- Comment Start Akhilesh     
 IF((SELECT DATEDIFF(MINUTE,(SELECT StartDate FROM ScheduleMasters WHERE ScheduleID=@ScheduleID),CONVERT(DATETIME, CONVERT(CHAR(8), (Select COALESCE(ClockInTime,GETDATE()) From EmployeeVisits Where EmployeeVisitID=@EmployeeVisitID), 112) + ' ' + CONVERT(CHAR(8), @ClockInTime, 108)))) < @BeforeClockIn)            
 BEGIN            
 SELECT -1 AS Result RETURN;            
 END            
        
 --Updated by:Kundan        
 --Updated date: 24-01-2020        
 --Desc: to check is clock in and clock out is whithin schedule date and time.    
     
   -- =============================================  
-- Author:  Akhilesh Kamal  
-- Create date: 22 nov 2020  
-- Description: Comment on to check is clock in and clock out is whithin schedule date and time  
-- =============================================    
 --IF((SELECT COUNT(*) FROM ScheduleMasters         
 --WHERE         
 -- --@ClockInTime BETWEEN StartDate AND EndDate AND        
 -- @ClockOutTime >= StartDate AND @ClockOutTime <=  DATEADD(MINUTE,29,EndDate)  
 -- AND ScheduleID=@ScheduleID) = 0)        
 -- BEGIN        
 -- SELECT -2 AS Result RETURN;            
 -- END        
    -- Comment End Akhilesh  
         
 --Updated By:Pallav Saxena        
 --Updated Date: 03/16/2020        
 --Description: Fixed the issue of the date getting updated to current date instead of Start and EndDate of the Schedule.        
        
--UPDATE EMPLOYEEVISITS SET CLOCKOUTTIME=Convert(varchar,CONVERT(CHAR(10),EndDate,23) +' '+ CONVERT(CHAR(12),CLOCKOUTTIME,114), 13),        
--CLOCKINTIME=Convert(varchar,CONVERT(CHAR(10),STARTDate,23) +' '+ CONVERT(CHAR(12),CLOCKINTIME,114), 13),UpdatedBy=@UpdatedBy, UpdatedDate=getdate()              
--FROM EMPLOYEEVISITS EV INNER JOIN SCHEDULEMASTERS SM ON EV.SCHEDULEID=SM.SCHEDULEID WHERE  SM.ScheduleID=@ScheduleID        
 DECLARE @TotalDeviationTime INT;    
DECLARE @TotalClocinTime INT;    
DECLARE @TotalSheduleTime int;    
DECLARE @PendingServiceTime int;    
  SELECT @TotalClocinTime=DATEDIFF(MINUTE, ClockInTime, ClockOutTime) FROM EmployeeVisits  WHERE EmployeeVisitID=@EmployeeVisitID    
     SELECT @TotalSheduleTime=DATEDIFF(MINUTE, StartDate, EndDate) FROM ScheduleMasters                   
 WHERE ScheduleID=(SELECT ScheduleID FROM EmployeeVisits WHERE EmployeeVisitID=@EmployeeVisitID)     
     SELECT @TotalDeviationTime=SUM(DeviationTime) FROM SaveDeviationNote                   
  WHERE EmployeeVisitID=@EmployeeVisitID AND IsDeleted=0    
 select @PendingServiceTime = @TotalSheduleTime-@TotalClocinTime    
 IF(@TotalDeviationTime<=@PendingServiceTime)     
 BEGIN    
  SELECT -3 AS Result RETURN;     
 END    
    
 -- Pallav/Kundan/Akhilesh: 08-04-2020    
 -- Fixed clock in clock out date reset issue, now inserting schedule master dates if clock in/out is null    
  UPDATE EMPLOYEEVISITS       
 SET ClockInTime=CONVERT(DATETIME, CONVERT(CHAR(8), (    
 Select COALESCE(EV.ClockInTime,SM.StartDate) From EmployeeVisits EV inner join ScheduleMasters SM on SM.ScheduleID = EV.ScheduleID Where EV.EmployeeVisitID=@EmployeeVisitID    
 ), 112) + ' ' + CONVERT(CHAR(8), @ClockInTime, 108)),            
    ClockOutTime=CONVERT(DATETIME, CONVERT(CHAR(8), (    
 Select COALESCE(EV.ClockOutTime,SM.EndDate) From EmployeeVisits EV inner join ScheduleMasters SM on SM.ScheduleID = EV.ScheduleID Where EV.EmployeeVisitID=@EmployeeVisitID    
 ), 112) + ' ' + CONVERT(CHAR(8), @ClockOutTime, 108)),UpdatedBy=@UpdatedBy, UpdatedDate=getdate()           
       
FROM EMPLOYEEVISITS EV INNER JOIN SCHEDULEMASTERS SM ON EV.SCHEDULEID=SM.SCHEDULEID WHERE  EV.EmployeeVisitID=@EmployeeVisitID       
      
        EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @ScheduleID,'222','25'
                
   SELECT CONVERT(datetime,ClockInTime) AS ClockInTime,               
   CONVERT(datetime,ClockOutTime) AS ClockOutTime FROM EmployeeVisits WHERE EmployeeVisitID=@EmployeeVisitID             
END 