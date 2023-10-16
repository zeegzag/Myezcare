-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 26 Jun 2020
-- Description:	This SP is used to process notification and return details. (called by CronJob)
-- =============================================
CREATE PROCEDURE [notif].[GetConfiguredNotifications]
	@JobID UNIQUEIDENTIFIER,
	@BaseURL NVARCHAR(MAX)
AS
BEGIN
	DECLARE @CurrDateTime DATETIME = GETDATE();
	DECLARE @LastSuccessRunDateTime DATETIME = NULL;	
	SELECT TOP 1
		@LastSuccessRunDateTime = CONVERT(DATETIME, RTRIM([run_date]) + ' ' + STUFF(STUFF(REPLACE(STR(RTRIM([run_time]),6,0), ' ', '0'), 3, 0, ':'), 6, 0, ':'))
	FROM
		[msdb].[dbo].[sysjobhistory]
	WHERE
		[run_status] = 1
		AND [job_id] = @JobID
	ORDER BY 
		[instance_id] DESC;

    SELECT @LastSuccessRunDateTime = DATEADD(MINUTE, -5, ISNULL(@LastSuccessRunDateTime, @CurrDateTime))

	IF (@LastSuccessRunDateTime IS NOT NULL)
		BEGIN

			-- Get notifications event data.
			EXEC [notif].[ProcessAndGetNotificationEventData] @FromDateTime = @LastSuccessRunDateTime,
															  @ToDateTime = @CurrDateTime,
															  @BaseURL = @BaseURL;

		END;

END