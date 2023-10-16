CREATE PROCEDURE [dbo].[GetSearchRegion]            
@SearchText varchar(20)=null,              
@PageSize int      
AS              
BEGIN            
      
 SELECT Top(@PageSize) RegionID,RegionName FROM Regions      
 WHERE (@SearchText IS NULL) OR (RegionName LIKE '%'+@SearchText+'%' )      
          
END
