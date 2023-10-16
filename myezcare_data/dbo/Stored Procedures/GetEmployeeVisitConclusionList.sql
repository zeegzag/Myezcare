--EXEC GetEmployeeVisitConclusionList @VisitTaskType = 'Conclusion', @EmployeeVisitID = '1'      
CREATE PROCEDURE [dbo].[GetEmployeeVisitConclusionList]  
 @VisitTaskType NVARCHAR(30),        
 @EmployeeVisitID BIGINT        
AS                                  
BEGIN                                      
 SELECT evn.Description,v.VisitTaskDetail,evn.EmployeeVisitNoteID,rtm.ReferralTaskMappingID  
 FROM EmployeeVisitNotes evn    
 INNER JOIN ReferralTaskMappings rtm ON rtm.ReferralTaskMappingID=evn.ReferralTaskMappingID      
 INNER JOIN VisitTasks v ON v.VisitTaskID=rtm.VisitTaskID      
 WHERE v.VisitTaskType=@VisitTaskType AND evn.EmployeeVisitID=@EmployeeVisitID    
 ORDER BY v.VisitTaskID    
END
