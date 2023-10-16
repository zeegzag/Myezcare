-- EXEC SaveVisitTask @VisitTaskID = '8', @VisitTaskType = 'Task', @VisitTaskDetail = 'Task 002', @ServiceCodeID = '0', @IsRequired = 'False', @MinimumTimeRequired = NULL, @loggedInUserId = '1', @IsEditMode = 'True', @SystemID = '::1'      
      
CREATE PROCEDURE [dbo].[SaveCategory]  
@VisitTaskCategoryID bigint,  
 @VisitTaskType VARCHAR(100),              
 @CategoryName NVARCHAR(200)         
AS              
BEGIN              
           
 IF EXISTS (SELECT TOP 1 VisitTaskCategoryID FROM VisitTaskCategories WHERE VisitTaskCategoryName=@CategoryName AND VisitTaskCategoryType=@VisitTaskType AND VisitTaskCategoryID != @VisitTaskCategoryID)                
 BEGIN        
 SELECT -1 RETURN;          
 END        
            
              
    -- If edit mode              
 IF(@VisitTaskCategoryID=0)              
 BEGIN              
   INSERT INTO VisitTaskCategories  
   (VisitTaskCategoryName,VisitTaskCategoryType)  
   VALUES              
   (@CategoryName,@VisitTaskType);               
                 
 END              
 ELSE              
 BEGIN              
   UPDATE VisitTaskCategories               
   SET                    
     VisitTaskCategoryName=@CategoryName,  
  VisitTaskCategoryType=@VisitTaskType  
   WHERE VisitTaskCategoryID=@VisitTaskCategoryID;              
 END              
          
      
 SELECT 1; RETURN;          
          
          
END