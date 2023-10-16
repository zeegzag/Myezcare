CREATE PROCEDURE [dbo].[SaveSubCategory]  
@VisitTaskCategoryID bigint,    
 @ParentCategoryLevel bigint,  
 @VisitTaskType NVARCHAR(20),  
 @SubCategoryName NVARCHAR(200)  
AS                
BEGIN                
             
 IF EXISTS (SELECT TOP 1 VisitTaskCategoryID FROM VisitTaskCategories WHERE ParentCategoryLevel=@ParentCategoryLevel AND VisitTaskCategoryName=@SubCategoryName AND VisitTaskCategoryID != @VisitTaskCategoryID)  
 BEGIN          
 SELECT -1 RETURN;            
 END          
              
                
    -- If edit mode                
 IF(@VisitTaskCategoryID=0)                
 BEGIN                
   INSERT INTO VisitTaskCategories    
   (VisitTaskCategoryName,VisitTaskCategoryType,ParentCategoryLevel)    
   VALUES                
   (@SubCategoryName,@VisitTaskType,@ParentCategoryLevel);  
                   
 END                
 ELSE                
 BEGIN                
   UPDATE VisitTaskCategories                 
   SET                      
     VisitTaskCategoryName=@SubCategoryName,    
 ParentCategoryLevel=@ParentCategoryLevel    
   WHERE VisitTaskCategoryID=@VisitTaskCategoryID;                
 END                
            
        
 SELECT 1; RETURN;            
            
            
END