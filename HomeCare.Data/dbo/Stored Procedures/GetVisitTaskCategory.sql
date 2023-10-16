CREATE PROCEDURE [dbo].[GetVisitTaskCategory]  
@VisitTaskType nvarchar(20)  
AS      
BEGIN      
      
 SELECT * FROM VisitTaskCategories WHERE VisitTaskCategoryType=@VisitTaskType  
      
END