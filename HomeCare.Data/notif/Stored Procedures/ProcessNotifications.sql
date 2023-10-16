
-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 26 Jun 2020
-- Description:	This SP is used to process notification. (called by SQL Job)
-- =============================================
CREATE PROCEDURE [notif].[ProcessNotifications]
	@JobID UNIQUEIDENTIFIER,
	@BaseURL NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Endpoint NVARCHAR(MAX) = @BaseURL + N'/CronJob/SendConfiguredNotifications/' + CONVERT(NVARCHAR(50), @JobID);
	DECLARE @JsonResponse NVARCHAR(MAX) = NULL;
	DECLARE @NL NVARCHAR(2) = CHAR(13);

	SELECT @JsonResponse = CURL.XGET(NULL, @Endpoint);

	DECLARE @IsSuccess BIT, @Message NVARCHAR(MAX), @Data NVARCHAR(MAX);
	SELECT
		@IsSuccess = R.[IsSuccess],
		@Message = R.[Message],
		@Data = R.[Data]
	FROM 
		OPENJSON(@JsonResponse)
		WITH (   
			[IsSuccess] BIT '$.IsSuccess',  
			[Message] NVARCHAR(MAX) '$.Message',  
			[Data] NVARCHAR(MAX) '$.Data'
		) R

	IF (@IsSuccess = 1)
		BEGIN
			PRINT @Message + @NL + @Data;
		END
	ELSE
		BEGIN
			RAISERROR (N'Some errors occurred!%sDetails: %s.', 11, 1, @NL, @Message);
		END
	
END