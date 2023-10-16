      
CREATE PROCEDURE [dbo].[GetEmployeeVisitConclusionList]    
 @VisitTaskType NVARCHAR(30),          
 @EmployeeVisitID BIGINT          
AS                                    
BEGIN
--UpdatedBy:Akhilesh kamal
   --UpdateDate:17/01/2020
   --Description:add value of comment in select statement                                        
 SELECT evn.Description,evn.AlertComment,v.VisitTaskDetail,evn.EmployeeVisitNoteID,rtm.ReferralTaskMappingID    
 FROM EmployeeVisitNotes evn      
 INNER JOIN ReferralTaskMappings rtm ON rtm.ReferralTaskMappingID=evn.ReferralTaskMappingID        
 INNER JOIN VisitTasks v ON v.VisitTaskID=rtm.VisitTaskID        
 WHERE v.VisitTaskType=@VisitTaskType AND evn.EmployeeVisitID=@EmployeeVisitID      
 ORDER BY v.VisitTaskID      
END  
GO
