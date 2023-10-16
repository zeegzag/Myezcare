--EXEC Getebcategorylist @Name = mgoo'Activities', @IsDeleted = '0', @SortExpression = 'ID', @SortType = 'DESC', @FromIndex = '1', @PageSize = '10'      
CREATE PROCEDURE [dbo].[Getebcategorylist] @Name           NVARCHAR(100) = NULL, 
                                           @Id             NVARCHAR(50) = NULL, 
                                           @EBCategoryID   NVARCHAR(100) = NULL, 
                                           @IsDeleted      BIGINT = -1, 
                                           @SortExpression NVARCHAR(100), 
                                           @SortType       NVARCHAR(10), 
                                           @FromIndex      INT, 
                                           @PageSize       INT 
AS 
  BEGIN ; 
      WITH cteebcategorylist 
           AS (SELECT *, 
                      Count(T1.id) 
                        OVER() AS Count 
               FROM   (SELECT Row_number() 
                                OVER ( 
                                  ORDER BY CASE WHEN @SortType = 'ASC' THEN CASE 
                                WHEN 
                                @SortExpression 
                                = 'Name' 
                                THEN NAME END END ASC, CASE WHEN @SortType = 
                                'DESC' 
                                THEN 
                                CASE 
                                WHEN 
                                @SortExpression = 'Name' THEN NAME END END DESC 
                                ) 
                              AS 
                              Row, 
                              id, 
                              NAME, 
                              ebcategoryid, 
                              isdeleted 
                       FROM   ebcategories P 
                       WHERE  ( ( Cast(@IsDeleted AS BIGINT) = -1 ) 
                                 OR P.isdeleted = @IsDeleted ) 
                              AND ( ( @Name IS NULL 
                                       OR Len(@Name) = 0 ) 
                                     OR (( P.NAME LIKE '%' + @Name + '%' )) )) 
                      AS 
                      T1) 
      SELECT * 
      FROM   cteebcategorylist 
      WHERE  row BETWEEN ( ( @PageSize * ( @FromIndex - 1 ) ) + 1 ) AND ( 
                         @PageSize * @FromIndex ) 
  END