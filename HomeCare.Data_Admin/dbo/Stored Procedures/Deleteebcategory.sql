CREATE PROCEDURE [dbo].[Deleteebcategory] @Name           NVARCHAR(100) = NULL, 
                                          @Id             NVARCHAR(50) = NULL, 
                                          @EBCategoryID   NVARCHAR(100) = NULL, 
                                          @IsDeleted      BIGINT = -1, 
                                          @SortExpression NVARCHAR(100), 
                                          @SortType       NVARCHAR(10), 
                                          @FromIndex      INT, 
                                          @PageSize       INT, 
                                          @ListOfIdsInCsv VARCHAR(300), 
                                          @IsShowList     BIT, 
                                          @loggedInID     BIGINT 
AS 
  BEGIN 
      IF( Len(@ListOfIdsInCsv) > 0 ) 
        BEGIN 
            -- UPDATE ebcategories SET IsDeleted= CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END  ,UpdatedDate=GETUTCDATE() WHERE EBCategoryID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv))                       
            UPDATE ebcategories 
            SET    isdeleted = CASE isdeleted 
                                 WHEN 0 THEN 1 
                                 ELSE 0 
                               END, 
                   updateddate = Getutcdate() 
            WHERE  ebcategoryid = @EBCategoryID 
        END 

      IF( @IsShowList = 1 ) 
        BEGIN 
            EXEC Getebcategorylist 
              @Name, 
              @Id, 
              @EBCategoryID, 
              @IsDeleted, 
              @SortExpression, 
              @SortType, 
              @FromIndex, 
              @PageSize 
        END 
  END