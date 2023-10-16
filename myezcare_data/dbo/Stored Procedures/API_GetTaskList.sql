
--Exec API_GetTaskList @ReferralID=1953,@ScheduleID=88818, @VisitTaskType='Task'                
CREATE PROCEDURE [dbo].[API_GetTaskList]      
 @ScheduleID BIGINT,            
 @ReferralID BIGINT,            
 @VisitTaskType NVARCHAR(10)            
AS                                    
BEGIN                
                                    
 SELECT evn.ServiceTime,VisitTaskDetail=v.VisitTaskDetail  
 --+ CASE WHEN sc1.ServiceCode Is Not Null THEN '  (' + sc1.ServiceCode + ')' ELSE '' END    
 ,COALESCE(vg.VisitTaskCategoryName,'Other') as ParentCategoryName,vg1.VisitTaskCategoryName,evn.EmployeeVisitNoteID,      
 evn.ReferralTaskMappingID,evn.Description    
 FROM EmployeeVisitNotes evn                        
 INNER JOIN EmployeeVisits s ON evn.EmployeeVisitID = s.EmployeeVisitID  AND s.ScheduleID=@ScheduleID            
 INNER JOIN ReferralTaskMappings rtm ON rtm.ReferralTaskMappingID=evn.ReferralTaskMappingID                        
 INNER JOIN VisitTasks v ON v.VisitTaskID=rtm.VisitTaskID                
 LEFT JOIN VisitTaskCategories vg ON vg.VisitTaskCategoryID=v.VisitTaskCategoryID                
 LEFT JOIN VisitTaskCategories vg1 ON v.VisitTaskSubCategoryID=vg1.VisitTaskCategoryID    
 --LEFT JOIN ServiceCodes sc1 ON sc1.ServiceCodeID=v.ServiceCodeID    
 WHERE rtm.ReferralID=@ReferralID AND v.VisitTaskType=@VisitTaskType                      
                          
 --SELECT v.VisitTaskDetail,v.MinimumTimeRequired      
 --FROM ReferralTaskMappings rm                        
 --INNER JOIN VisitTasks v ON v.VisitTaskID=rm.VisitTaskID                        
 --WHERE rm.ReferralID=@ReferralID AND v.VisitTaskType=@VisitTaskType AND rm.IsDeleted=0                        
END 

