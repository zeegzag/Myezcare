CREATE PROCEDURE [dbo].[GetGroupVisitTask]    
 @VisitTaskType NVARCHAR(30) ,   
 @CareType BIGINT=0    
AS   
BEGIN    
 IF (@VisitTaskType = 'Task')    
 BEGIN   
 print 'ji'
  SELECT v.VisitTaskID    
   ,v.VisitTaskDetail    
  FROM VisitTasks v    
  WHERE v.VisitTaskType = @VisitTaskType    
   AND v.caretype = @CareType    
   AND v.IsDeleted = 0    
  ORDER BY v.VisitTaskDetail ASC    
 END    
 ELSE    
 BEGIN  
  SELECT v.VisitTaskID    
   ,v.VisitTaskDetail    
  FROM VisitTasks v    
  WHERE v.VisitTaskType = @VisitTaskType    
  --and v.caretype=@CareType    
   AND v.IsDeleted = 0    
  ORDER BY v.VisitTaskDetail ASC    
 END    
END
