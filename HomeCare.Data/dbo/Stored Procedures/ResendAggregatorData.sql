CREATE PROCEDURE [dbo].[ResendAggregatorData]
	@ListOfIdsInCsv NVARCHAR(MAX)
AS
BEGIN
  DECLARE @CurScheduleID BIGINT;

  DECLARE eventCursor CURSOR FORWARD_ONLY
  FOR
  SELECT val
  FROM [dbo].GetCSVTable(@ListOfIdsInCsv);

  OPEN eventCursor;

  FETCH NEXT
  FROM eventCursor
  INTO @CurScheduleID;

  WHILE @@FETCH_STATUS = 0
  BEGIN
    EXEC [dbo].[ScheduleEventBroadcast] 'ResendSchedule',
      @CurScheduleID,
      '',
      ''

    FETCH NEXT
    FROM eventCursor
    INTO @CurScheduleID;
  END;

  CLOSE eventCursor;

  DEALLOCATE eventCursor;
END
