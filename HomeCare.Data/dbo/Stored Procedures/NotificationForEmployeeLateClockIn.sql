-- exec NotificationForEmployeeLateClockIn     
      
CREATE PROCEDURE [dbo].[NotificationForEmployeeLateClockIn]        
 -- @RoleID nvarchar(200)      
AS        
BEGIN      
 DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
select sm.ScheduleID,e.EmployeeID,e.LastName,e.FirstName ,Employee = dbo.GetGenericNameFormat(E.FirstName,E.MiddleName, E.LastName,@NameFormat),
sm.StartDate as ScheduleDate,sm.EndDate as ScheduleEndDate,sm.StartDate as StartTime,ev.ClockInTime as ClockInTime,ev.ClockOutTime as ClockOutTime from ScheduleMasters sm  
left join EmployeeVisits ev on ev.ScheduleID=sm.ScheduleID  
inner join Employees e on e.EmployeeID = sm.EmployeeID   
where ev.ClockInTime>sm.StartDate and sm.startdate >getdate()-1  
END  