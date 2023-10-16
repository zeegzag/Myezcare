
CREATE PROCEDURE [dbo].[API_CheckRequiredTask]      
 @ScheduleID BIGINT,              
 @ReferralID BIGINT,              
 @VisitTaskType NVARCHAR(10)              
AS                                      
BEGIN                  
                                      
 If (Exists(      
 Select RM.ReferralTaskMappingID from ReferralTaskMappings RM      
 INNER JOIN VisitTasks V ON V.VisitTaskID=RM.VisitTaskID      
 Where ReferralID=@ReferralID AND V.VisitTaskType=@VisitTaskType AND RM.IsRequired=1 AND RM.IsDeleted=0    
 AND RM.ReferralTaskMappingID NOT IN      
 (      
 Select evn.ReferralTaskMappingID from EmployeeVisits ev      
 INNER JOIN EmployeeVisitNotes evn ON evn.EmployeeVisitID=ev.EmployeeVisitID      
 WHERE ev.ScheduleID=@ScheduleID      
 )))      
 BEGIN      
  SELECT 0 RETURN;      
 END      
ELSE      
 BEGIN      
  SELECT 1 RETURN;      
 END      
END 

