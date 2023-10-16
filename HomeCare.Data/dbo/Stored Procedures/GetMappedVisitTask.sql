--EXEC GetMappedVisitTask @VisitTaskType = 'Task', @EmployeeVisitID = '45'
--EXEC [GetMappedVisitTask] @VisitTaskType = 'Task', @EmployeeVisitID = '30214'        
/*
Purpose: Get the list of all the task related to the schedule for a visit.
Changes: Added the caretype clause to get only the caretype related to a schedule
MOdified by: Pallav Saxena
Modified Date : 09/23/2019
*/


CREATE PROCEDURE [dbo].[GetMappedVisitTask]      
 @VisitTaskType NVARCHAR(30),          
 @EmployeeVisitID BIGINT          
AS                                    
BEGIN                                        
if (@VisitTaskType='Task') 
begin
 SELECT rm.ReferralTaskMappingID,v.VisitTaskDetail   
 FROM EmployeeVisits ev      
 INNER JOIN ScheduleMasters sm ON sm.ScheduleID=ev.ScheduleID      
 INNER JOIN ReferralTaskMappings rm ON rm.ReferralID=sm.ReferralID AND rm.IsDeleted=0   
 INNER JOIN VisitTasks v ON v.VisitTaskID=rm.VisitTaskID AND v.VisitTaskType=@VisitTaskType      and v.caretype=sm.caretypeid
 WHERE ev.EmployeeVisitID=@EmployeeVisitID  Order BY v.VisitTaskDetail ASC    
 end
 else
 begin
  SELECT rm.ReferralTaskMappingID,v.VisitTaskDetail   
 FROM EmployeeVisits ev      
 INNER JOIN ScheduleMasters sm ON sm.ScheduleID=ev.ScheduleID      
 INNER JOIN ReferralTaskMappings rm ON rm.ReferralID=sm.ReferralID AND rm.IsDeleted=0   
 INNER JOIN VisitTasks v ON v.VisitTaskID=rm.VisitTaskID AND v.VisitTaskType=@VisitTaskType      --and v.caretype=sm.caretypeid
 WHERE ev.EmployeeVisitID=@EmployeeVisitID  Order BY v.VisitTaskDetail ASC    
 end
END