--   EXEC GetVisitTaskByCaretype 'Task', 41

CREATE PROCEDURE [dbo].[GetVisitTaskByCaretype]    
@VisitTaskType nvarchar(20),
@CareType bigint=0    
AS        
BEGIN        
        
 SELECT *, vc.VisitTaskCategoryName FROM VisitTasks vt    
 INNER JOIN VisitTaskCategories vc ON vt.VisitTaskCategoryID = vc.VisitTaskCategoryID
 WHERE VisitTaskType=@VisitTaskType AND CareType=@CareType AND IsDeleted=0        
END 