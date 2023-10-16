-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 07 Jul 2020
-- Description:	This SP is used to get event data Messaging.
-- =============================================
CREATE PROCEDURE [notif].[Messaging]
	@NotificationConfigurationID BIGINT, 
	@NotificationEventID BIGINT,
	@FromDateTime DATETIME,
	@ToDateTime DATETIME,
	@BaseURL NVARCHAR(100)
AS
BEGIN
	
	DECLARE @Messaging [notif].[EventDataTable];

	INSERT INTO @Messaging
		SELECT 
			REF.[RefID],
			REF.[RefTableName],
			TD.[Data] [TemplateData],
            NULL [DefaultRecipients]
		FROM
			[dbo].[ReferralInternalMessages] RIM
		CROSS APPLY ( SELECT RIM.[ReferralInternalMessageID] [RefID], '[dbo].[ReferralInternalMessages]' [RefTableName] ) REF
		LEFT JOIN [notif].[Notifications] N
			ON N.RefID = REF.RefID
			   AND N.RefTableName = REF.RefTableName	
			   AND N.NotificationEventID = @NotificationEventID
			   AND N.NotificationConfigurationID = @NotificationConfigurationID
		CROSS APPLY (
			SELECT 
				E.[FirstName] + ' ' + E.[LastName] [EmployeeName]
			FROM
				[dbo].[Employees] E
			WHERE
				E.[EmployeeID] = RIM.CreatedBy
		) MB
		CROSS APPLY (
			SELECT 
				E.[FirstName] + ' ' + E.[LastName] [EmployeeName]
			FROM
				[dbo].[Employees] E
			WHERE
				E.[EmployeeID] = RIM.Assignee
		) A
		CROSS APPLY (
			SELECT (	
				SELECT 
					RIM.Note [##Message##],
					MB.EmployeeName [##MessageBy##],
					A.EmployeeName [##Assignee##],
					'Internal Message' [##EventName##]
				FOR JSON PATH
			) [Data]
		) TD
		WHERE
			N.NotificationID IS NULL
			AND RIM.[IsDeleted] = 0
			AND RIM.[IsResolved] = 0
			AND (RIM.[CreatedDate] BETWEEN @FromDateTime AND @ToDateTime 
				 OR RIM.[UpdatedDate] BETWEEN @FromDateTime AND @ToDateTime);

	SELECT * FROM @Messaging;

END