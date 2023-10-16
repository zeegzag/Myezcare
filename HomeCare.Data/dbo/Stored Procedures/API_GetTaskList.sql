--Exec API_GetTaskList @ReferralID=1953,@ScheduleID=88818, @VisitTaskType='Task'                  
CREATE PROCEDURE [dbo].[API_GetTaskList]        
 @ScheduleID BIGINT,              
 @ReferralID BIGINT,              
 @VisitTaskType NVARCHAR(10)              
AS                                      
BEGIN 
/*
Changed by: Pallav
Reason: To remove the time taken for task entry in simple task entry
Date: 02/13/2020
*/                 
WITH CTE(SVC_Time,VisitTaskDetail, ParentCategoryName,VisitTaskCategoryName,EmployeeVisitNoteID,ReferralTaskMappingID,Description,SimpleTaskType) as                                   
 (SELECT  evn.ServiceTime ,VisitTaskDetail=v.VisitTaskDetail    
 --+ CASE WHEN sc1.ServiceCode Is Not Null THEN '  (' + sc1.ServiceCode + ')' ELSE '' END      
 ,COALESCE(vg.VisitTaskCategoryName,'Other') as ParentCategoryName,vg1.VisitTaskCategoryName,evn.EmployeeVisitNoteID,        
 evn.ReferralTaskMappingID,evn.Description,evn.SimpleTaskType      
 FROM EmployeeVisitNotes evn                          
 INNER JOIN EmployeeVisits s ON evn.EmployeeVisitID = s.EmployeeVisitID  AND s.ScheduleID=@ScheduleID              
 INNER JOIN ReferralTaskMappings rtm ON rtm.ReferralTaskMappingID=evn.ReferralTaskMappingID                          
 INNER JOIN VisitTasks v ON v.VisitTaskID=rtm.VisitTaskID                  
 LEFT JOIN VisitTaskCategories vg ON vg.VisitTaskCategoryID=v.VisitTaskCategoryID                  
 LEFT JOIN VisitTaskCategories vg1 ON v.VisitTaskSubCategoryID=vg1.VisitTaskCategoryID      
 --LEFT JOIN ServiceCodes sc1 ON sc1.ServiceCodeID=v.ServiceCodeID      
 WHERE rtm.ReferralID=@ReferralID AND v.VisitTaskType=@VisitTaskType)                

SELECT CASE simpleTaskType when 1 then 0 else SVC_Time end servicetime,VisitTaskDetail, ParentCategoryName,VisitTaskCategoryName,EmployeeVisitNoteID,ReferralTaskMappingID,Description,SimpleTaskType
from CTE
-- End of change by Pallav
END