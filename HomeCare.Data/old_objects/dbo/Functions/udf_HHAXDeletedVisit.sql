CREATE FUNCTION dbo.udf_HHAXDeletedVisit (
  @ScheduleID bigint)
RETURNS nvarchar(max)
AS
BEGIN
  RETURN
  (
    SELECT
      SD.HHAXEvvmsID [evvmsid]
    FROM ScheduleDetails SD
    WHERE
      SD.ScheduleID = @ScheduleID

    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER 
  )
END;