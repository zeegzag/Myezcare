-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 07 Jul 2020
-- Description:	This SP is used to get event data PTO Created by employee.
-- =============================================
CREATE PROCEDURE [notif].[PTOCreatedByEmployee]
	@NotificationConfigurationID BIGINT, 
	@NotificationEventID BIGINT,
	@FromDateTime DATETIME,
	@ToDateTime DATETIME,
	@BaseURL NVARCHAR(100)
AS
BEGIN
	
	DECLARE @PTOCreatedByEmployee [notif].[EventDataTable];

	INSERT INTO @PTOCreatedByEmployee
		SELECT 
			REF.[RefID],
			REF.[RefTableName],
			TD.[Data] [TemplateData],
            NULL [DefaultRecipients]
		FROM
			[dbo].[EmployeeDayOffs] EDO
		CROSS APPLY ( SELECT EDO.[EmployeeDayOffID] [RefID], '[dbo].[EmployeeDayOffs]' [RefTableName] ) REF
		LEFT JOIN [notif].[Notifications] N
			ON N.RefID = REF.RefID
			   AND N.RefTableName = REF.RefTableName	
			   AND N.NotificationEventID = @NotificationEventID
			   AND N.NotificationConfigurationID = @NotificationConfigurationID
		CROSS APPLY (
			SELECT (	
				SELECT 
					[FirstName] [##FirstName##],
					[LastName] [##LastName##],
					CONVERT(VARCHAR, EDO.StartTime, 0) [##DayOffStartTime##],
					CONVERT(VARCHAR, EDO.EndTime, 0) [##DayOffEndTime##],
					CASE WHEN EDO.DayOffTypeID = 1 THEN 'Other' 
						WHEN EDO.DayOffTypeID = 2 THEN 'Sick'
						WHEN EDO.DayOffTypeID = 3 THEN 'Vacation'
					END [##DayOffType##],
					EDO.EmployeeComment [##Comment##],
					'PTO created' [##EventName##]
				FROM 
					[dbo].[Employees] E
				WHERE 
					E.[EmployeeID] = EDO.[EmployeeID] 
				FOR JSON PATH
			) [Data]
		) TD
		WHERE
			N.NotificationID IS NULL
			AND EDO.[IsDeleted] = 0
			AND EDO.[DayOffStatus] = 'InProgress'
			AND (EDO.[CreatedDate] BETWEEN @FromDateTime AND @ToDateTime 
				 OR EDO.[UpdatedDate] BETWEEN @FromDateTime AND @ToDateTime);

	SELECT * FROM @PTOCreatedByEmployee;

END