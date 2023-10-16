CREATE PROCEDURE [dbo].[SaveParentChildMapping]
	@ParentTaskId BIGINT,
	@ChildTaskIds VARCHAR(MAX)
AS
BEGIN
	
	BEGIN TRANSACTION trans
	BEGIN TRY
		UPDATE DDMaster SET ParentID=0 WHERE ParentID=@ParentTaskId AND DDMasterID NOT IN (SELECT RESULT FROM dbo.CSVtoTableWithIdentity(@ChildTaskIds,','))
		UPDATE DDMaster SET ParentID=@ParentTaskId WHERE DDMasterID IN (SELECT RESULT FROM dbo.CSVtoTableWithIdentity(@ChildTaskIds,','))
		SELECT 1 AS TransactionResultId;
		IF @@TRANCOUNT > 0
			BEGIN
				COMMIT TRANSACTION trans
			END
	END TRY
	BEGIN CATCH
		SELECT -1 AS TransactionResultId;
		IF @@TRANCOUNT > 0
			BEGIN
				ROLLBACK TRANSACTION trans
			END		
	END CATCH
END
