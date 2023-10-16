CREATE PROCEDURE [dbo].[GetVisitTaskSubCategory]  
@VisitTaskCategoryID bigint  
AS      
BEGIN      
      
 SELECT * FROM VisitTaskCategories WHERE ParentCategoryLevel=@VisitTaskCategoryID  
      
END
