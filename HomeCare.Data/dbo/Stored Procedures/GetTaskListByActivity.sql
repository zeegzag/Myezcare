
--   EXEC GetTaskListByActivity 'Task', 41, 1
  
CREATE PROCEDURE [dbo].[GetTaskListByActivity]      
@VisitTaskType nvarchar(max) = '',  
@CareType bigint=0,
@VisitTaskCategoryID bigint=0  
AS          
BEGIN          
          
 SELECT * FROM VisitTasks WHERE VisitTaskType=@VisitTaskType AND CareType=@CareType AND VisitTaskCategoryID=@VisitTaskCategoryID AND IsDeleted=0   
          
END