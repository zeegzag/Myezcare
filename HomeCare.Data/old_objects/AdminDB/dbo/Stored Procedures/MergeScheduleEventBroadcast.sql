CREATE PROCEDURE [dbo].[MergeScheduleEventBroadcast]
  @OrganizationID bigint,
  @EventName nvarchar(max),
  @ScheduleID bigint,
  @ReasonCode nvarchar(max),
  @ActionCode nvarchar(max)
AS
BEGIN

  MERGE [dbo].[ScheduleEventBroadcast] T
  USING
  (
    SELECT
      @OrganizationID [OrganizationID],
      @ScheduleID [ScheduleID],
      @EventName [EventName],
      @ReasonCode [ReasonCode],
      @ActionCode [ActionCode],
      GETDATE() [CreatedDate],
      0 [IsProcessed]
  ) S
  ON
  T.[OrganizationID] = S.[OrganizationID]
  AND T.[ScheduleID] = S.[ScheduleID]
  AND T.[IsProcessed] = S.[IsProcessed]
  WHEN NOT MATCHED BY TARGET THEN
  INSERT
  (
	  [OrganizationID],
	  [ScheduleID],
	  [EventName],
	  [ReasonCode],
	  [ActionCode],
	  [CreatedDate],
	  [IsProcessed]
  )
  VALUES
  (
	  S.[OrganizationID],
	  S.[ScheduleID],
	  S.[EventName],
	  S.[ReasonCode],
	  S.[ActionCode],
	  S.[CreatedDate],
	  S.[IsProcessed]
  )
  WHEN MATCHED THEN
  UPDATE SET
	  [OrganizationID] = S.[OrganizationID],
	  [ScheduleID] = S.[ScheduleID],
	  [EventName] = S.[EventName],
	  [ReasonCode] = S.[ReasonCode],
	  [ActionCode] = S.[ActionCode],
	  [CreatedDate] = S.[CreatedDate],
	  [IsProcessed] = S.[IsProcessed];

END