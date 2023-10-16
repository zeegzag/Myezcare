CREATE PROCEDURE [dbo].[GetEBCategoryDetail] @EBCategoryID NVARCHAR(max) 
AS 
  BEGIN 
      SELECT * 
      FROM   ebcategories 
      WHERE  id = @EBCategoryID 
  END