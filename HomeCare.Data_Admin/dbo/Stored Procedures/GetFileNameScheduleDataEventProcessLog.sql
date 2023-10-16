CREATE PROCEDURE [dbo].[GetFileNameScheduleDataEventProcessLog] 
  @TransactionID NVARCHAR(MAX),
  @UploadedFileName NVARCHAR(max)
AS
BEGIN
  SELECT TOP 1 
    [FileName]
  FROM [dbo].[ScheduleDataEventProcessLogs]
  WHERE (
      ISNULL(@TransactionID, '') = ''
      OR [TransactionID] = @TransactionID
      )
    AND [UploadedFileName] = @UploadedFileName
END
