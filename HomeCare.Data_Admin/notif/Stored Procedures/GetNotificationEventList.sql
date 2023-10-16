
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