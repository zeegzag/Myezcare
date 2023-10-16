-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 07 Jul 2020
-- Description:	This SP is used to get event data Patient on Hold.
-- =============================================
CREATE PROCEDURE [notif].[PatientOnHold]
	@NotificationConfigurationID BIGINT, 
	@NotificationEventID BIGINT,
	@FromDateTime DATETIME,
	@ToDateTime DATETIME,
	@BaseURL NVARCHAR(100)
AS
BEGIN
	
	DECLARE @PatientOnHold [notif].[EventDataTable];

	INSERT INTO @PatientOnHold
		SELECT 
			REF.[RefID],
			REF.[RefTableName],
			TD.[Data] [TemplateData],
            NULL [DefaultRecipients]
		FROM
			[dbo].[ReferralOnHoldDetails] ROHD
		CROSS APPLY ( SELECT ROHD.[ReferralOnHoldDetailID] [RefID], '[dbo].[ReferralOnHoldDetails]' [RefTableName] ) REF
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
				E.[EmployeeID] = ROHD.[CreatedBy]
		) ED
		CROSS APPLY (
			SELECT 
				R.[FirstName] + ' ' + R.[LastName] [PatientName]
			FROM
				[dbo].[Referrals] R
			WHERE
				R.[ReferralID] = ROHD.[ReferralID]
		) PD
		CROSS APPLY (
			SELECT (	
				SELECT 
					ED.[EmployeeName] [##EmployeeName##],
					PD.[PatientName] [##PatientName##],
					ROHD.PatientOnHoldReason [##OnHoldReason##],
					'Patient On Hold' [##EventName##]
				FOR JSON PATH
			) [Data]
		) TD
		WHERE
			N.NotificationID IS NULL
			AND ROHD.[IsDeleted] = 0
			AND (ROHD.[CreatedDate] BETWEEN @FromDateTime AND @ToDateTime 
				 OR ROHD.[UpdatedDate] BETWEEN @FromDateTime AND @ToDateTime);

	SELECT * FROM @PatientOnHold;

END