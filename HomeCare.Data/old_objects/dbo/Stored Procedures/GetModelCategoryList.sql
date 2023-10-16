
-- exec GetModelCategoryList 'mob',null,null,0  
CREATE PROCEDURE [dbo].[GetModelCategoryList]        
@CategoryName nvarchar(200),      
@SubCategoryName nvarchar(200),      
@Type nvarchar(20),      
@IsCategoryList bit      
AS            
BEGIN            
      
Declare @temp Table(      
 ParentCategoryId bigint,      
 VisitTaskType nvarchar(20),      
 ParentCategoryName nvarchar(200)      
)      
      
Insert Into @temp Select VisitTaskCategoryID,VisitTaskCategoryType,VisitTaskCategoryName from VisitTaskCategories Where ParentCategoryLevel is null      
      
IF(@IsCategoryList=1)            
 SELECT * FROM VisitTaskCategories         
 WHERE ((@CategoryName IS NULL OR LEN(@CategoryName)=0) OR VisitTaskCategoryName LIKE '%' + @CategoryName + '%')        
 AND ((@Type IS NULL OR LEN(@Type)=0) OR VisitTaskCategoryType LIKE '%' + @Type + '%') AND ParentCategoryLevel is null    
 ORDER BY VisitTaskCategoryName      
ELSE      
 Select vg.*,t.ParentCategoryName,t.VisitTaskType as ParentTaskType from VisitTaskCategories vg      
 Inner Join @temp t ON vg.ParentCategoryLevel=t.ParentCategoryId      
 Where ((@CategoryName IS NULL OR LEN(@CategoryName)=0) OR t.ParentCategoryName LIKE '%' + @CategoryName + '%')      
 AND ((@SubCategoryName IS NULL OR LEN(@SubCategoryName)=0) OR vg.VisitTaskCategoryName LIKE '%' + @SubCategoryName + '%')      
 AND vg.ParentCategoryLevel is not null    
 ORDER BY vg.VisitTaskCategoryName    
END

