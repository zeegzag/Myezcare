CREATE PROCEDURE [dbo].[SaveVisitReason]
  @ScheduleID bigint,
  @ReasonType nvarchar(max),
  @ReasonCode nvarchar(max),
  @ActionCode nvarchar(max),
  @CompanyName nvarchar(max),
  @LoggedInUserID bigint
AS
BEGIN
  IF (@ScheduleID > 0)
  BEGIN
    SELECT @ReasonCode = TRIM(@ReasonCode)
    SELECT @ActionCode = TRIM(@ActionCode)

    INSERT INTO [dbo].[VisitReasons]
    (
      [Type],
      ReasonCode,
      ActionCode,
      ScheduleID,
      CompanyName,
      IsDeleted,
      CreatedDate,
      CreatedBy,
      UpdatedDate,
      UpdatedBy
    )
    VALUES
    (
      @ReasonType,
      @ReasonCode,
      @ActionCode,
      @ScheduleID,
      @CompanyName,
      0,
      GETDATE(),
      @LoggedInUserID,
      GETDATE(),
      @LoggedInUserID
    )

	EXEC [dbo].[ScheduleEventBroadcast] @ReasonType,
                                        @ScheduleID,
                                        @ReasonCode,
                                        @ActionCode

    SELECT
      1;
    RETURN;
  END
  SELECT
    2;
  RETURN;
END