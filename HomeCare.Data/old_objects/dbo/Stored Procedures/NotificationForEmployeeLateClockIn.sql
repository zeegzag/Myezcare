--CreatedBy              CreatedDate      UpdatedDate    description    
--Akhilesh Kamal         19-7-2019            ""         [NotificationForEmployeeLateClockIn]  
    
-- exec NotificationForEmployeeLateClockIn   
    
CREATE PROCEDURE [dbo].[NotificationForEmployeeLateClockIn]      
 -- @RoleID nvarchar(200)    
AS      
BEGIN      
select sm.ScheduleID,e.EmployeeID,e.LastName,e.FirstName ,sm.StartDate as ScheduleDate,sm.EndDate as ScheduleEndDate,sm.StartDate as StartTime,ev.ClockInTime as ClockInTime,ev.ClockOutTime as ClockOutTime from ScheduleMasters sm
left join EmployeeVisits ev on ev.ScheduleID=sm.ScheduleID
inner join Employees e on e.EmployeeID = sm.EmployeeID 
where ev.ClockInTime>sm.StartDate
END
