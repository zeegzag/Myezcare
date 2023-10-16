CREATE PROCEDURE [dbo].[InsertScheduleDataEventProcessLog]
  @OrganizationID BIGINT,
  @TransactionID NVARCHAR(max),
  @ScheduleID BIGINT,
  @Aggregator NVARCHAR(max),
  @FileName NVARCHAR(max)
AS
BEGIN
  INSERT INTO [dbo].[ScheduleDataEventProcessLogs] (
    [OrganizationID],
	[TransactionID],
	[ScheduleID],
	[Aggregator],
	[FileName],
	[CreatedDate]
    )
  VALUES (
    @OrganizationID,
	@TransactionID,
	@ScheduleID,
	@Aggregator,
	@FileName,
    GETUTCDATE()
    )
END
