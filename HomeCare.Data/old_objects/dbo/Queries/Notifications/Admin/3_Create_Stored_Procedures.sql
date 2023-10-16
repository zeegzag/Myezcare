-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 02 Jul 2020
-- Description:	This SP is used to save notification event.
-- =============================================
CREATE PROCEDURE [notif].[SaveNotificationEvent]
	@NotificationEventID BIGINT,
	@EventName NVARCHAR(100),
	@Description NVARCHAR(500),
	@EventDefinitionSP NVARCHAR (200) NULL,
	@LoggedInUserID BIGINT,
	@SystemID VARCHAR(100)
AS
BEGIN

	IF EXISTS (SELECT TOP 1 [NotificationEventID] FROM [notif].[NotificationEvents] WHERE [EventName] = @EventName AND [NotificationEventID] != @NotificationEventID)
		BEGIN
			SELECT -1 As TransactionResultId; RETURN;
		END

	DECLARE @CurrDateTime DATETIME = GETUTCDATE();
	IF (@NotificationEventID = 0)
		BEGIN
			INSERT INTO [notif].[NotificationEvents]
			(
				[EventName],
				[Description],
				[EventDefinitionSP],
				[CreatedDate],
				[CreatedBy],
				[UpdatedDate],
				[UpdatedBy],
				[SystemID]
			)
			VALUES
			(
				@EventName,
				@Description,
				@EventDefinitionSP,
				@CurrDateTime,
				@LoggedInUserID,
				@CurrDateTime,
				@LoggedInUserID,
				@SystemID
			);
		END
	ELSE
		BEGIN
			UPDATE
				[notif].[NotificationEvents]
			SET
				[EventName] = @EventName,
				[Description] = @Description,
				[EventDefinitionSP] = @EventDefinitionSP,
				[UpdatedDate] = @CurrDateTime,
				[UpdatedBy] = @LoggedInUserID,
				[SystemID] = @SystemID
			WHERE
				[NotificationEventID] = @NotificationEventID;
		END

	SELECT 1 As TransactionResultId; RETURN;

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 02 Jul 2020
-- Description:	This SP is used to get notification event by id.
-- =============================================
CREATE PROCEDURE [notif].[GetNotificationEventById]
	@NotificationEventID BIGINT
AS
BEGIN

	SELECT 
		* 
	FROM 
		[notif].[NotificationEvents] 
	WHERE 
		[NotificationEventID] = @NotificationEventID;

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 02 Jul 2020
-- Description:	This SP is used to get notification event list.
-- =============================================
CREATE PROCEDURE [notif].[GetNotificationEventList]
	@EventName NVARCHAR(100) = NULL,
	@Description NVARCHAR(500) = NULL,
	@IsDeleted SMALLINT	 = -1,
	@SORTEXPRESSION NVARCHAR(100),
	@SORTTYPE NVARCHAR(10),
	@FROMINDEX INT,
	@PAGESIZE INT
AS
BEGIN

	;WITH CTEFilteredNotificationEvent AS
	(
		SELECT
			ROW_NUMBER() OVER (ORDER BY
				CASE WHEN @SortType = 'ASC' AND @SortExpression = 'EventName' THEN NE.[EventName] END ASC,
				CASE WHEN @SortType = 'DESC' AND @SortExpression = 'EventName' THEN NE.[EventName] END DESC,
				CASE WHEN @SortType = 'ASC' AND @SortExpression = 'Description' THEN NE.[Description] END ASC,
				CASE WHEN @SortType = 'DESC' AND @SortExpression = 'Description' THEN NE.[Description] END DESC
 			) AS [Row], 
			*,
			COUNT(NotificationEventID) OVER() [Count]
		FROM
			[notif].[NotificationEvents] NE
		WHERE
			(LEN(ISNULL(@EventName, '')) = 0 OR NE.[EventName] LIKE '%' + @EventName+ '%')
			AND (LEN(ISNULL(@Description, '')) = 0 OR NE.[Description] LIKE '%' + @Description+ '%')
			AND (@IsDeleted = -1 OR NE.[IsDeleted]= @IsDeleted)
	)
	SELECT
		*
	FROM
		CTEFilteredNotificationEvent FNE
	WHERE
		[Row] BETWEEN ((@PAGESIZE * (@FROMINDEX - 1)) + 1) AND (@PAGESIZE * @FROMINDEX)

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 02 Jul 2020
-- Description:	This SP is used to delete notification event by id.
-- =============================================
CREATE PROCEDURE [notif].[DeleteNotificationEvent]
	@NotificationEventIDs VARCHAR(4000),
	@IsDeleted SMALLINT,
	@LoggedInUserID BIGINT,
	@SystemID VARCHAR(100)
AS
BEGIN

	DECLARE @CurrDateTime DATETIME = GETUTCDATE();
	DECLARE @UpdateCount INT;

	UPDATE
		[notif].[NotificationEvents]
	SET
		[IsDeleted] = CASE WHEN @IsDeleted = -1 THEN [IsDeleted] ^ 1 ELSE @IsDeleted END,
		[UpdatedDate] = @CurrDateTime,
		[UpdatedBy] = @LoggedInUserID,
		[SystemID] = @SystemID
	WHERE 
		[NotificationEventID] IN (SELECT CAST([val] AS BIGINT) FROM [dbo].[GetCSVTable](@NotificationEventIDs));
	
	SELECT @UpdateCount = @@ROWCOUNT;

	IF (@UpdateCount > 0)
		BEGIN
			SELECT @UpdateCount; RETURN;
		END
	ELSE
		BEGIN
			SELECT -1; RETURN;
		END

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 02 Jul 2020
-- Description:	This SP is used to save notification configuration.
-- =============================================
CREATE PROCEDURE [notif].[SaveNotificationConfiguration]
	@NotificationConfigurationID BIGINT,
	@ConfigurationName NVARCHAR(100),
	@Description NVARCHAR(500),
	@NotificationEventID BIGINT,
	@EmailTemplateID BIGINT,
	@SMSTemplateID BIGINT,
	@LoggedInUserID BIGINT,
	@SystemID VARCHAR(100)
AS
BEGIN

	IF EXISTS (SELECT TOP 1 [NotificationConfigurationID] FROM [notif].[NotificationConfigurations] WHERE [ConfigurationName] = @ConfigurationName AND [NotificationConfigurationID] != @NotificationConfigurationID)
		BEGIN
			SELECT -1 As TransactionResultId; RETURN;
		END

	DECLARE @CurrDateTime DATETIME = GETUTCDATE();
	IF (@NotificationConfigurationID = 0)
		BEGIN
			INSERT INTO [notif].[NotificationConfigurations]
			(
				[ConfigurationName],
				[Description],
				[NotificationEventID],
				[EmailTemplateID],
				[SMSTemplateID],
				[CreatedDate],
				[CreatedBy],
				[UpdatedDate],
				[UpdatedBy],
				[SystemID]
			)
			VALUES
			(
				@ConfigurationName,
				@Description,
				@NotificationEventID,
				@EmailTemplateID,
				@SMSTemplateID,
				@CurrDateTime,
				@LoggedInUserID,
				@CurrDateTime,
				@LoggedInUserID,
				@SystemID
			);
		END
	ELSE
		BEGIN
			UPDATE
				[notif].[NotificationConfigurations]
			SET
				[ConfigurationName] = @ConfigurationName,
				[Description] = @Description,
				[NotificationEventID] = @NotificationEventID,
				[EmailTemplateID] = @EmailTemplateID,
				[SMSTemplateID] = @SMSTemplateID,
				[UpdatedDate] = @CurrDateTime,
				[UpdatedBy] = @LoggedInUserID,
				[SystemID] = @SystemID
			WHERE
				[NotificationConfigurationID] = @NotificationConfigurationID;
		END

	SELECT 1 As TransactionResultId; RETURN;

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 02 Jul 2020
-- Description:	This SP is used to get notification configuration by id.
-- =============================================
CREATE PROCEDURE [notif].[GetNotificationConfigurationById]
	@NotificationConfigurationID BIGINT
AS
BEGIN

	SELECT 
		* 
	FROM 
		[notif].[NotificationConfigurations] 
	WHERE 
		[NotificationConfigurationID] = @NotificationConfigurationID;

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 02 Jul 2020
-- Description:	This SP is used to get notification configuration list.
-- =============================================
CREATE PROCEDURE [notif].[GetNotificationConfigurationList]
	@ConfigurationName NVARCHAR(100) = NULL,
	@Description NVARCHAR(500) = NULL,
	@NotificationEventID BIGINT = NULL,
	@EmailTemplateID BIGINT = NULL,
	@SMSTemplateID BIGINT = NULL,
	@IsDeleted SMALLINT	 = -1,
	@SORTEXPRESSION NVARCHAR(100),
	@SORTTYPE NVARCHAR(10),
	@FROMINDEX INT,
	@PAGESIZE INT
AS
BEGIN

	;WITH CTEFilteredNotificationConfiguration AS
	(
		SELECT
			ROW_NUMBER() OVER (ORDER BY
				CASE WHEN @SortType = 'ASC' AND @SortExpression = 'ConfigurationName' THEN NC.[ConfigurationName] END ASC,
				CASE WHEN @SortType = 'DESC' AND @SortExpression = 'ConfigurationName' THEN NC.[ConfigurationName] END DESC,
				CASE WHEN @SortType = 'ASC' AND @SortExpression = 'Description' THEN NC.[Description] END ASC,
				CASE WHEN @SortType = 'DESC' AND @SortExpression = 'Description' THEN NC.[Description] END DESC,
				CASE WHEN @SortType = 'ASC' AND @SortExpression = 'EventName' THEN NE.[EventName] END ASC,
				CASE WHEN @SortType = 'DESC' AND @SortExpression = 'EventName' THEN NE.[EventName] END DESC,
				CASE WHEN @SortType = 'ASC' AND @SortExpression = 'EmailTemplateName' THEN ET.[EmailTemplateName] END ASC,
				CASE WHEN @SortType = 'DESC' AND @SortExpression = 'EmailTemplateName' THEN ET.[EmailTemplateName] END DESC,
				CASE WHEN @SortType = 'ASC' AND @SortExpression = 'SMSTemplateName' THEN ST.[EmailTemplateName] END ASC,
				CASE WHEN @SortType = 'DESC' AND @SortExpression = 'SMSTemplateName' THEN ST.[EmailTemplateName] END DESC
 			) AS [Row], 
			NC.*,
			NE.[EventName],
			ET.[EmailTemplateName],
			ST.[EmailTemplateName] [SMSTemplateName],
			COUNT(NotificationConfigurationID) OVER() [Count]
		FROM
			[notif].[NotificationConfigurations] NC
		LEFT JOIN [notif].[NotificationEvents] NE
			ON NC.[NotificationEventID] = NE.[NotificationEventID]
		LEFT JOIN [dbo].[EmailTemplates] ET
			ON NC.[EmailTemplateID] = ET.[EmailTemplateID]
		LEFT JOIN [dbo].[EmailTemplates] ST
			ON NC.[SMSTemplateID] = ST.[EmailTemplateID]
		WHERE
			(LEN(ISNULL(@ConfigurationName, '')) = 0 OR NC.[ConfigurationName] LIKE '%' + @ConfigurationName+ '%')
			AND (LEN(ISNULL(@Description, '')) = 0 OR NC.[Description] LIKE '%' + @Description+ '%')
			AND (ISNULL(@NotificationEventID, 0) = 0 OR NC.[NotificationEventID] = @NotificationEventID)
			AND (ISNULL(@EmailTemplateID, 0) = 0 OR NC.[EmailTemplateID] = @EmailTemplateID)
			AND (ISNULL(@SMSTemplateID, 0) = 0 OR NC.[SMSTemplateID] = @SMSTemplateID)
			AND (@IsDeleted = -1 OR NC.[IsDeleted]= @IsDeleted)
	)
	SELECT
		*
	FROM
		CTEFilteredNotificationConfiguration FNE
	WHERE
		[Row] BETWEEN ((@PAGESIZE * (@FROMINDEX - 1)) + 1) AND (@PAGESIZE * @FROMINDEX)

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 02 Jul 2020
-- Description:	This SP is used to delete notification configuration by id.
-- =============================================
CREATE PROCEDURE [notif].[DeleteNotificationConfiguration]
	@NotificationConfigurationIDs VARCHAR(4000),
	@IsDeleted SMALLINT,
	@LoggedInUserID BIGINT,
	@SystemID VARCHAR(100)
AS
BEGIN

	DECLARE @CurrDateTime DATETIME = GETUTCDATE();
	DECLARE @UpdateCount INT;

	UPDATE
		[notif].[NotificationConfigurations]
	SET
		[IsDeleted] = CASE WHEN @IsDeleted = -1 THEN [IsDeleted] ^ 1 ELSE @IsDeleted END,
		[UpdatedDate] = @CurrDateTime,
		[UpdatedBy] = @LoggedInUserID,
		[SystemID] = @SystemID
	WHERE 
		[NotificationConfigurationID] IN (SELECT CAST([val] AS BIGINT) FROM [dbo].[GetCSVTable](@NotificationConfigurationIDs));
	
	SELECT @UpdateCount = @@ROWCOUNT;

	IF (@UpdateCount > 0)
		BEGIN
			SELECT @UpdateCount; RETURN;
		END
	ELSE
		BEGIN
			SELECT -1; RETURN;
		END

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 06 Jul 2020
-- Description:	This SP is used to add/update notification configurations from admin DB to particular organization DB.
-- =============================================
CREATE PROCEDURE [notif].[AddUpdateOrganizationNotificationConfigurations]
	@OrganizationID BIGINT,
	@NotificationConfigurationIDs NVARCHAR(MAX),
	@LoggedInUserID BIGINT,
	@SystemID VARCHAR(100)
AS
BEGIN
	DECLARE @CurrDateTime DATETIME = GETUTCDATE();
	
	DECLARE @EventTable NVARCHAR(600), @ConfigurationTable NVARCHAR(600), @IsError BIT, @ErrorMessage NVARCHAR(100);
	SET @EventTable = [notif].[GetOrganizationTableName](@OrganizationID, '[notif].[NotificationEvents]')
	SET @ConfigurationTable = [notif].[GetOrganizationTableName](@OrganizationID, '[notif].[NotificationConfigurations]')
	SET @IsError = IIF(LEFT(@ConfigurationTable, 7) = 'Error: ', 1, 0);
	IF (@IsError = 1)
		BEGIN
			SET @ErrorMessage = SUBSTRING(@ConfigurationTable, 8, LEN(@ConfigurationTable))
			RAISERROR (@ErrorMessage, 11, 1);
		END
	ELSE
		BEGIN
			DECLARE @cmd NVARCHAR(MAX)
			SET @cmd = 'MERGE 
							' + @EventTable + ' T
						USING (	SELECT
									[NotificationEventID],
									[EventName],
									[Description],
									[EventDefinitionSP],
									@CurrDateTime [CurrDateTime],
									@LoggedInUserID [LoggedInUserID],
									@SystemID [SystemID],
									[IsDeleted]
								FROM
									[notif].[NotificationEvents] NE
								WHERE
									[NotificationEventID] IN ( SELECT DISTINCT [NotificationEventID]
																FROM
																	[notif].[NotificationConfigurations] NC
																INNER JOIN [dbo].[GetCSVTable](@NotificationConfigurationIDs) CL
																	ON NC.NotificationConfigurationID = CL.val )
								 ) S
						ON 
							T.[NotificationEventID] = S.[NotificationEventID]
						WHEN MATCHED THEN
							UPDATE SET
								[EventName] = S.[EventName],
								[Description] = S.[Description],
								[EventDefinitionSP] = S.[EventDefinitionSP],
								[UpdatedDate] = S.[CurrDateTime],
								[UpdatedBy] = S.[LoggedInUserID],
								[SystemID] = S.[SystemID]
						WHEN NOT MATCHED BY TARGET THEN
							INSERT
							(
								[NotificationEventID],
								[EventName],
								[Description],
								[EventDefinitionSP],
								[CreatedDate],
								[CreatedBy],
								[UpdatedDate],
								[UpdatedBy],
								[SystemID]
							)
							VALUES
							(
								S.[NotificationEventID],
								S.[EventName],
								S.[Description],
								S.[EventDefinitionSP],
								S.[CurrDateTime],
								S.[LoggedInUserID],
								S.[CurrDateTime],
								S.[LoggedInUserID],
								S.[SystemID]
							);
				

						MERGE 
							' + @ConfigurationTable + ' T
						USING (	SELECT
									[NotificationConfigurationID],
									[ConfigurationName],
									[Description],
									[NotificationEventID],
									NULL [EmailTemplateID],
									NULL [SMSTemplateID],
									@CurrDateTime [CurrDateTime],
									@LoggedInUserID [LoggedInUserID],
									@SystemID [SystemID],
									[IsDeleted]
								FROM
									[notif].[NotificationConfigurations] NC
								INNER JOIN [dbo].[GetCSVTable](@NotificationConfigurationIDs) CL
									ON NC.NotificationConfigurationID = CL.val ) S
						ON 
							T.[NotificationConfigurationID] = S.[NotificationConfigurationID]
						WHEN MATCHED THEN
							UPDATE SET
								[ConfigurationName] = S.[ConfigurationName],
								[Description] = S.[Description],
								[NotificationEventID] = S.[NotificationEventID],
								[UpdatedDate] = S.[CurrDateTime],
								[UpdatedBy] = S.[LoggedInUserID],
								[SystemID] = S.[SystemID],
								[IsDeleted] = S.[IsDeleted]
						WHEN NOT MATCHED BY TARGET THEN
							INSERT
							(
								[NotificationConfigurationID],
								[ConfigurationName],
								[Description],
								[NotificationEventID],
								[EmailTemplateID],
								[SMSTemplateID],
								[CreatedDate],
								[CreatedBy],
								[UpdatedDate],
								[UpdatedBy],
								[SystemID]
							)
							VALUES
							(
								S.[NotificationConfigurationID],
								S.[ConfigurationName],
								S.[Description],
								S.[NotificationEventID],
								S.[EmailTemplateID],
								S.[SMSTemplateID],
								S.[CurrDateTime],
								S.[LoggedInUserID],
								S.[CurrDateTime],
								S.[LoggedInUserID],
								S.[SystemID]
							);';

			EXEC sp_executesql @cmd, 
							   N'@NotificationConfigurationIDs NVARCHAR(MAX),
								 @CurrDateTime DATETIME,
								 @LoggedInUserID BIGINT,
								 @SystemID VARCHAR(100)', 
							   @NotificationConfigurationIDs = @NotificationConfigurationIDs, 
							   @CurrDateTime = @CurrDateTime, 
							   @LoggedInUserID = @LoggedInUserID, 
							   @SystemID = @SystemID;
		END

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 06 Jul 2020
-- Description:	This SP is used to delete notification configurations from particular organization DB.
-- =============================================
CREATE PROCEDURE [notif].[DeleteOrganizationNotificationConfigurations]
	@OrganizationID BIGINT,
	@NotificationConfigurationIDs NVARCHAR(MAX),
	@LoggedInUserID BIGINT,
	@SystemID VARCHAR(100)
AS
BEGIN

	DECLARE @CurrDateTime DATETIME = GETUTCDATE();

	DECLARE @ConfigurationTable NVARCHAR(600), @IsError BIT, @ErrorMessage NVARCHAR(100);
	SET @ConfigurationTable = [notif].[GetOrganizationTableName](@OrganizationID, '[notif].[NotificationConfigurations]')
	SET @IsError = IIF(LEFT(@ConfigurationTable, 7) = 'Error: ', 1, 0);
	IF (@IsError = 1)
		BEGIN
			SET @ErrorMessage = SUBSTRING(@ConfigurationTable, 8, LEN(@ConfigurationTable))
			RAISERROR (@ErrorMessage, 11, 1);
		END
	ELSE
		BEGIN
			DECLARE @cmd NVARCHAR(1000)
			SET @cmd = 'UPDATE NC
						SET
				 			[IsDeleted] = 1,
				 			[UpdatedDate] = @CurrDateTime,
				 			[UpdatedBy] = @LoggedInUserID,
				 			[SystemID] = @SystemID
						FROM
				 			' + @ConfigurationTable + ' NC
						INNER JOIN [dbo].[GetCSVTable](@NotificationConfigurationIDs) CL
				 			ON NC.NotificationConfigurationID = CL.val';

			EXEC sp_executesql @cmd, 
							   N'@NotificationConfigurationIDs NVARCHAR(MAX),
								 @CurrDateTime DATETIME,
								 @LoggedInUserID BIGINT,
								 @SystemID VARCHAR(100)', 
							   @NotificationConfigurationIDs = @NotificationConfigurationIDs, 
							   @CurrDateTime = @CurrDateTime, 
							   @LoggedInUserID = @LoggedInUserID, 
							   @SystemID = @SystemID;
		END

END
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 09 Jul 2020
-- Description:	This SP is used to process notifications for organizations.
-- =============================================
CREATE PROCEDURE [notif].[ProcessOrganizationNotifications]
	@JobID UNIQUEIDENTIFIER
AS
BEGIN

	-- Declare required fields to fetch for creating SQL jobs.
	DECLARE @OrganizationID BIGINT, @DBName NVARCHAR(MAX), @BaseURL NVARCHAR(MAX), @OrgSPName NVARCHAR(MAX);

	-- Declare cursor to fetch fields to create job for processing notifications.
	DECLARE CursorJobConfig CURSOR
	FOR SELECT
			[OrganizationID],
			[DBName], 
			'https://' + [DomainName] + '.myezcare.com'
		FROM 
			[dbo].[Organizations]
		WHERE
			[IsActive] = 1
			--AND [OrganizationID] IN (30023)
			AND [IsDeleted] = 0;

	-- Open cursor to fetch data.
	OPEN CursorJobConfig;

	-- Fetch fields.
	FETCH NEXT FROM CursorJobConfig INTO 
		@OrganizationID,
		@DBName, 
		@BaseURL;

	-- Loop the cursor.
	WHILE @@FETCH_STATUS = 0
		BEGIN

			SELECT @OrgSPName = '[' + @DBName + '].[notif].[ProcessNotifications]';
			IF (OBJECT_ID(@OrgSPName) IS NOT NULL)
				BEGIN
					PRINT '==== ORG ID: ' + CAST(@OrganizationID AS VARCHAR) + ', DB: ' + @DBName + ' ====';
					DECLARE @cmd NVARCHAR(MAX) = N'EXEC ' + @OrgSPName + ' @JobID, @BaseURL;';
					EXEC sp_executesql @cmd, 
										N'@JobID UNIQUEIDENTIFIER, 
										  @BaseURL NVARCHAR(MAX)', 
										@JobID = @JobID,
										@BaseURL = @BaseURL;
				END

			-- Fetch fields.
			FETCH NEXT FROM CursorJobConfig INTO 
				@OrganizationID,
				@DBName, 
				@BaseURL;
		END;
	
	-- Close cursor.
	CLOSE CursorJobConfig;

	-- Deallocate curser.
	DEALLOCATE CursorJobConfig;

END
GO