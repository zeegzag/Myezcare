CREATE PROCEDURE [dbo].[HHAXUpdateVisitTransaction]
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
      HHAXEvvmsID = @TransactionID
    WHERE
      ScheduleID = @ScheduleID
  END
  ELSE
  BEGIN

    INSERT INTO dbo.ScheduleDetails
    (
      ScheduleID,
      HHAXEvvmsID
    )
    VALUES
    (
      @ScheduleID,
      @TransactionID
    )
  END
END