
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateClockInOutSmsUserLog]
	-- Add the parameters for the stored procedure here
	@RecordList NVARCHAR(MAX),
	@LeftArrowChar NVARCHAR(20),
	@RightArrowChar NVARCHAR(20)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	UPDATE L
	SET 
			L.SmsStatusId=R.SmsStatusId,
			L.AttemptCount=ISNULL(L.AttemptCount,0)+1
	FROM EVV_SmsLogs L
	INNER JOIN 
	(
			SELECT 
				(SELECT Result FROM [dbo].[CSVtoTableWithIdentity](Result,@RightArrowChar) WHERE ReturnId = 1) AS ScheduleID,
				(SELECT Result FROM [dbo].[CSVtoTableWithIdentity](Result,@RightArrowChar) WHERE ReturnId = 2) AS EmployeeID,
				(SELECT Result FROM [dbo].[CSVtoTableWithIdentity](Result,@RightArrowChar) WHERE ReturnId = 3) AS SmsStatusId,
				(SELECT Result FROM [dbo].[CSVtoTableWithIdentity](Result,@RightArrowChar) WHERE ReturnId = 4) AS SmsType
			FROM [dbo].[CSVtoTableWithIdentity](@RecordList,@LeftArrowChar)
	) AS R ON R.EmployeeID=L.EmployeeID AND R.ScheduleID=L.ScheduleID AND R.SmsType=L.[Type]

END