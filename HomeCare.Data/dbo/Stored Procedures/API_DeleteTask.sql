CREATE PROCEDURE [dbo].[API_DeleteTask]
  @EmployeeVisitNoteID bigint = 0
AS
BEGIN

  BEGIN TRANSACTION trans
    BEGIN TRY

      IF EXISTS
        (
          SELECT TOP 1
            *
          FROM EmployeeVisitNotes
          WHERE
            EmployeeVisitNoteID = @EmployeeVisitNoteID
        )
      BEGIN
        DELETE FROM EmployeeVisitNotes
        WHERE EmployeeVisitNoteID = @EmployeeVisitNoteID
      END

      SELECT
        1 AS TransactionResultId;

      IF @@TRANCOUNT > 0
      BEGIN
      COMMIT TRANSACTION trans
    END
  END TRY
  BEGIN CATCH
    SELECT
      -1 AS TransactionResultId,
      ERROR_MESSAGE() AS ErrorMessage;

    IF @@TRANCOUNT > 0
    BEGIN
      ROLLBACK TRANSACTION trans
    END
  END CATCH

END