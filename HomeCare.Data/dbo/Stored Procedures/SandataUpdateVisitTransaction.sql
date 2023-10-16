CREATE PROCEDURE [dbo].[SandataUpdateVisitTransaction]
	@ScheduleID bigint,
  @TransactionID nvarchar(max)
AS
BEGIN

  IF EXISTS
    (
      SELECT
        1
      FROM dbo.ScheduleDetails
      WHERE
        ScheduleID = @ScheduleID
    )
  BEGIN
    UPDATE dbo.ScheduleDetails
    SET
      SandataID = @TransactionID
    WHERE
      ScheduleID = @ScheduleID
  END
  ELSE
  BEGIN

    INSERT INTO dbo.ScheduleDetails
    (
      ScheduleID,
      SandataID
    )
    VALUES
    (
      @ScheduleID,
      @TransactionID
    )
  END
END
