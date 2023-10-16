--Exec API_GetReferralTasks @ReferralID=1963, @ScheduleID=78806, @VisitTaskType='Task'  
  
CREATE PROCEDURE [dbo].[API_GetReferralTasks01]  
 @ReferralID BIGINT,              
 @ScheduleID BIGINT,              
 @VisitTaskType NVARCHAR(10)                  
AS                              
BEGIN        

    
--SELECT coalesce(vg.VisitTaskCategoryID,-1) as VisitTaskCategoryID        
--     , coalesce(vg.VisitTaskCategoryName,'Other') as VisitTaskCategoryName, vg1.VisitTaskCategoryID as ParentCategoryLevel, vg1.VisitTaskCategoryName as ParentCategoryName  
--  From VisitTasks as v    
--LEFT JOIN VisitTaskCategories as vg on vg.VisitTaskCategoryID = v.VisitTaskCategoryID    
--LEFT JOIN VisitTaskCategories vg1 ON v.VisitTaskSubCategoryID=vg1.VisitTaskCategoryID  
--INNER JOIN ReferralTaskMappings rm on v.VisitTaskID=rm.VisitTaskID        
--INNER JOIN ScheduleMasters sm ON sm.ReferralID=rm.ReferralID              
-- INNER JOIn EmployeeVisits ev ON ev.ScheduleID=sm.ScheduleID              
-- WHERE rm.ReferralID=@ReferralID AND v.VisitTaskType=@VisitTaskType AND rm.IsDeleted=0 AND sm.ScheduleID=@ScheduleID


SELECT v.VisitTaskDetail,
vg.VisitTaskCategoryName as CategoryName,vg.VisitTaskCategoryID as CategoryId, 
vg1.VisitTaskCategoryName as SubCategoryName, vg1.VisitTaskCategoryID as SubCategoryId
FROM VisitTasks v
LEFT JOIN VisitTaskCategories vg ON vg.VisitTaskCategoryID=v.VisitTaskCategoryID
LEFT JOIN VisitTaskCategories vg1 ON vg1.VisitTaskCategoryID=v.VisitTaskSubCategoryID
INNER JOIN ReferralTaskMappings rm on v.VisitTaskID=rm.VisitTaskID        
INNER JOIN ScheduleMasters sm ON sm.ReferralID=rm.ReferralID              
 INNER JOIn EmployeeVisits ev ON ev.ScheduleID=sm.ScheduleID              
 WHERE rm.ReferralID=@ReferralID AND v.VisitTaskType=@VisitTaskType AND rm.IsDeleted=0 AND sm.ScheduleID=@ScheduleID

        
        
 SELECT rm.ReferralTaskMappingID,rm.IsRequired,v.VisitTaskDetail,v.MinimumTimeRequired,ev.EmployeeVisitID,coalesce(vg1.VisitTaskCategoryID,vg.VisitTaskCategoryID) as VisitTaskCategoryID        
 FROM ReferralTaskMappings rm        
 INNER JOIN VisitTasks v ON v.VisitTaskID=rm.VisitTaskID        
 LEFT JOIN VisitTaskCategories vg on vg.VisitTaskCategoryID = v.VisitTaskCategoryID  
 LEFT JOIN VisitTaskCategories vg1 ON v.VisitTaskSubCategoryID=vg1.VisitTaskCategoryID        
 INNER JOIN ScheduleMasters sm ON sm.ReferralID=rm.ReferralID              
 INNER JOIn EmployeeVisits ev ON ev.ScheduleID=sm.ScheduleID              
 WHERE rm.ReferralID=@ReferralID AND v.VisitTaskType=@VisitTaskType AND rm.IsDeleted=0 AND sm.ScheduleID=@ScheduleID          
          
 SELECT * FROM EmployeeVisits WHERE ScheduleID=@ScheduleID          
END