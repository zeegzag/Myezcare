CREATE PROCEDURE [dbo].[AddOrgFormTag]
@OrganizationFormID BIGINT,
@FormTagID BIGINT,
@FormTagName NVARCHAR(200)
AS                              
BEGIN                              
--DECLARE @TablePrimaryId bigint;
                          
  BEGIN TRANSACTION trans                          
 BEGIN TRY
  
  IF(@FormTagID=0)
	BEGIN
		INSERT INTO FormTags VALUES (@FormTagName)
		SET @FormTagID=@@IDENTITY
	END

	INSERT INTO OrganizationFormTags VALUES (@OrganizationFormID,@FormTagID)
                          
 SELECT 1 AS TransactionResultId;
      IF @@TRANCOUNT > 0                          
     BEGIN                           
      COMMIT TRANSACTION trans                           
     END                         END TRY                          
   BEGIN CATCH                          
    SELECT -1 AS TransactionResultId,ERROR_MESSAGE() AS ErrorMessage;                          
    IF @@TRANCOUNT > 0                          
     BEGIN                           
      ROLLBACK TRANSACTION trans                           
     END                          
  END CATCH                         
                          
END