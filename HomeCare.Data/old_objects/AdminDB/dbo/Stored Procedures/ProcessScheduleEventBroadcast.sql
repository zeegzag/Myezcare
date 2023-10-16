CREATE PROCEDURE [dbo].[ProcessScheduleEventBroadcast]
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @Cnt BIGINT = 0, @FailedCnt BIGINT = 0

	DECLARE 
	    @ScheduleEventBroadcastID bigint,
	    @BaseURL nvarchar(max),
		@EventName nvarchar(max),
	    @ScheduleID bigint,
	    @ReasonCode nvarchar(max),
	    @ActionCode nvarchar(max)

    DECLARE eventCursor CURSOR FORWARD_ONLY FOR
	SELECT
	  SB.ScheduleEventBroadcastID,
	  'https://' + O.[DomainName] + '.myezcare.com' BaseURL,
	  SB.EventName,
	  SB.ScheduleID,
	  SB.ReasonCode,
	  SB.ActionCode
	FROM [dbo].[ScheduleEventBroadcast] SB
	INNER JOIN [dbo].[Organizations] O
	  ON O.OrganizationID = SB.OrganizationID
	WHERE
	  SB.IsProcessed = 0;

    OPEN eventCursor;
    FETCH NEXT FROM eventCursor INTO @ScheduleEventBroadcastID, @BaseURL, @EventName, @ScheduleID, @ReasonCode, @ActionCode;

	
    WHILE @@FETCH_STATUS = 0 BEGIN
	    SET @Cnt = @Cnt + 1
		BEGIN TRY  
            DECLARE @hkey nvarchar(max) = 'Content-Type: application/json';

			DECLARE @body nvarchar(max) = '{"EventName":"' + @EventName + '","ScheduleID":' + CONVERT(nvarchar(max), @ScheduleID) + ',"ReasonCode":"' + @ReasonCode + '","ActionCode":"' + @ActionCode + '"}';

			DECLARE @endpoint nvarchar(max) = @BaseURL + N'/hc/schedule/event';

			EXEC curl.XPOST @H = @hkey,
							@d = @body,
							@url = @endpoint;

			UPDATE
				[dbo].[ScheduleEventBroadcast]
			SET
				IsProcessed = 1
			WHERE
				ScheduleEventBroadcastID = @ScheduleEventBroadcastID
        END TRY  
        BEGIN CATCH  
            -- do nothing
			SET @FailedCnt = @FailedCnt + 1
        END CATCH  

        FETCH NEXT FROM eventCursor INTO @ScheduleEventBroadcastID, @BaseURL, @EventName, @ScheduleID, @ReasonCode, @ActionCode;
    END;

	PRINT 'Total Processed Count: ' + CONVERT(VARCHAR(MAX), @Cnt) + ', Failed Count: ' + CONVERT(VARCHAR(MAX), @FailedCnt)+ ', Success Count: ' + CONVERT(VARCHAR(MAX), @Cnt - @FailedCnt)
    CLOSE eventCursor;
    DEALLOCATE eventCursor;  

END