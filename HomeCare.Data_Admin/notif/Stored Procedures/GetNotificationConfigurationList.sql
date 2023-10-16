
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