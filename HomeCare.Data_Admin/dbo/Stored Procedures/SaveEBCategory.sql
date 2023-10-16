CREATE PROCEDURE [dbo].[SaveEBCategory]-- 'SDSD','SDSD','SSS','FALSE',0
  -- Add the parameters for the stored procedure here                             
  @EBCategoryID NVARCHAR(max)=NULL, 
  @Id           NVARCHAR(max)=NULL, 
  @Name         NVARCHAR(max), 
  @IsDeleted    BIT, 
  @IsINSUp      INT 
AS 
  BEGIN 
      

      IF( @IsINSUp = 0 ) 
        BEGIN 
            INSERT INTO ebcategories 
                        ([name], 
                         ebcategoryid, 
                         id, 
                         createddate, 
                         isdeleted) 
            VALUES      (@Name, 
                         @EBCategoryID, 
                         @Id, 
                         Getutcdate(), 
                         @IsDeleted); 
        END 
      ELSE 
        BEGIN 
            UPDATE ebcategories 
            SET    [name] = @Name, 
                   ebcategoryid = @EBCategoryID, 
                   id = @Id, 
                   isdeleted = @IsDeleted, 
                   updateddate = Getutcdate() 
            WHERE  ebcategoryid = @EBCategoryID; 
        END 

      SELECT 1; 

      RETURN; 
  END