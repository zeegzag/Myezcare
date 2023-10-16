-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 07 Jul 2020
-- Description:	This SP is used to get event data expired documents.
-- =============================================
CREATE PROCEDURE [notif].[ExpiredDocuments]
	@NotificationConfigurationID BIGINT, 
	@NotificationEventID BIGINT,
	@FromDateTime DATETIME,
	@ToDateTime DATETIME,
	@BaseURL NVARCHAR(100)
AS
BEGIN
	
	DECLARE @ExpiredDocuments [notif].[EventDataTable];

	INSERT INTO @ExpiredDocuments
		SELECT 
			REF.[RefID],
			REF.[RefTableName],
			TD.[Data] [TemplateData],
            NULL [DefaultRecipients]
		FROM
			[dbo].[ReferralDocuments] RD
		CROSS APPLY ( SELECT RD.[ReferralDocumentID] [RefID], '[dbo].[ReferralDocuments]' [RefTableName] ) REF
		LEFT JOIN [notif].[Notifications] N
			ON N.RefID = REF.RefID
			   AND N.RefTableName = REF.RefTableName	
			   AND N.NotificationEventID = @NotificationEventID
			   AND N.NotificationConfigurationID = @NotificationConfigurationID
		OUTER APPLY (
			SELECT 
				E.[FirstName] + ' ' + E.[LastName] [EmployeeName]
			FROM
				[dbo].[Employees] E
			WHERE
				E.[EmployeeID] = RD.UserID
				AND RD.UserType = 2
		) ED
		OUTER APPLY (
			SELECT 
				R.[FirstName] + ' ' + R.[LastName] [PatientName]
			FROM
				[dbo].[Referrals] R
			WHERE
				R.[ReferralID] = RD.UserID
				AND RD.UserType = 1
		) PD
		CROSS APPLY (
			SELECT (	
				SELECT 
					RD.[FileName] [##FileName##],
					ISNULL(ED.[EmployeeName], PD.[PatientName]) [##FullName##],
					'Expired Documents' [##EventName##]
				FOR JSON PATH
			) [Data]
		) TD
		WHERE
			N.NotificationID IS NULL
			AND CONVERT(DATETIME, RD.[ExpirationDate]) BETWEEN @FromDateTime AND @ToDateTime;

	SELECT * FROM @ExpiredDocuments;

END