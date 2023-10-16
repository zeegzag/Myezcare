CREATE PROCEDURE [dbo].[GetUpload835FilesForProcess]
@GetStatus  VARCHAR(MAX),  -- InProcess
@SetStatus  VARCHAR(MAX)  -- Running
AS
BEGIN

	SELECT * FROM Upload835Files WHERE Upload835FileProcessStatus = @GetStatus;
	UPDATE Upload835Files SET Upload835FileProcessStatus=@SetStatus WHERE Upload835FileProcessStatus = @GetStatus

END
