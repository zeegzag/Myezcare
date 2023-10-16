CREATE PROCEDURE [dbo].[UpdateScheduleDataEventProcessLog] 
  @TransactionID NVARCHAR(MAX),
  @FileName NVARCHAR(max),
  @IsSuccess BIT,
  @Messages NVARCHAR(MAX),
  @IsWaitingForResponse BIT = NULL,
  @UploadedFileName NVARCHAR(max) = NULL,
  @OrganizationID BIGINT OUTPUT
AS
BEGIN
  DECLARE @ScheduleDataEventProcessLogID BIGINT;

  SELECT TOP 1 
    @ScheduleDataEventProcessLogID = ScheduleDataEventProcessLogID,
    @OrganizationID = OrganizationID
  FROM [dbo].[ScheduleDataEventProcessLogs]
  WHERE (
      ISNULL(@TransactionID, '') = ''
      OR [TransactionID] = @TransactionID
      )
    AND [FileName] = @FileName

  UPDATE [dbo].[ScheduleDataEventProcessLogs]
  SET [IsSuccess] = @IsSuccess,
    [Messages] = @Messages,
    [IsWaitingForResponse] = ISNULL(@IsWaitingForResponse, 0),
    [UploadedFileName] = CASE WHEN [UploadedFileName] IS NULL THEN @UploadedFileName ELSE [UploadedFileName] END,
    [UpdatedDate] = GETUTCDATE()
  WHERE ScheduleDataEventProcessLogID = @ScheduleDataEventProcessLogID
END
