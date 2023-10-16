--EXEC [GetMappedVisitTask] @VisitTaskType = 'Task', @EmployeeVisitID = '30214'      
CREATE PROCEDURE [dbo].[GetMappedVisitTask]    
 @VisitTaskType NVARCHAR(30),        
 @EmployeeVisitID BIGINT        
AS                                  
BEGIN                                      
 SELECT rm.ReferralTaskMappingID,v.VisitTaskDetail 
 FROM EmployeeVisits ev    
 INNER JOIN ScheduleMasters sm ON sm.ScheduleID=ev.ScheduleID    
 INNER JOIN ReferralTaskMappings rm ON rm.ReferralID=sm.ReferralID AND rm.IsDeleted=0 
 INNER JOIN VisitTasks v ON v.VisitTaskID=rm.VisitTaskID AND v.VisitTaskType=@VisitTaskType    
 WHERE ev.EmployeeVisitID=@EmployeeVisitID  Order BY v.VisitTaskDetail ASC  
END
