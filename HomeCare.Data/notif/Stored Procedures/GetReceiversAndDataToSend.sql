-- =============================================
-- Author:      Fenil Gandhi
-- Create date: 26 Jun 2020
-- Description:  This SP is used to generate List of receivers and data to send notifications.
-- =============================================
CREATE PROCEDURE [notif].[GetReceiversAndDataToSend]
AS
BEGIN
  DECLARE @NotificationReceiversData [notif].[NotificationReceiversDataTable];

  -- Generate List of receivers and data to send notifications.
  WITH [RoleMappings]
  AS
  (
    SELECT
      NCRM.NotificationConfigurationID,
      STRING_AGG(E.EmployeeID, ',') EmployeeIDs
    FROM [notif].[NotificationConfigurationRoleMapping] NCRM
    INNER JOIN [dbo].[Employees] E
      ON E.RoleID = NCRM.RoleID
    GROUP BY NCRM.NotificationConfigurationID
  ),
  [NotificationEventData]
  AS
  (
    SELECT
      N.[NotificationID],
      NC.[NotificationConfigurationID],
      R.[Recipients],
      N.[IsEmailSent],
      N.[IsSMSSent],
      N.[IsWebNotificationSent],
      N.[IsMobileAppNotificationSent],
      ET.*,
      ST.*
    FROM [notif].[Notifications] N
    INNER JOIN [notif].[NotificationConfigurations] NC
      ON NC.[NotificationConfigurationID] = N.[NotificationConfigurationID]
    CROSS APPLY
    (
      SELECT
        ISNULL(
        (
          SELECT
            RM.EmployeeIDs
          FROM [RoleMappings] RM
          WHERE
            RM.[NotificationConfigurationID] = NC.[NotificationConfigurationID]
        )
        , '') [EmployeeIDs]
    ) RM
    CROSS APPLY
    (
      SELECT
        ISNULL(N.[DefaultRecipients], '') + CASE
          WHEN LEN(N.[DefaultRecipients]) > 0 AND
            LEN(RM.EmployeeIDs) > 0 THEN ','
          ELSE ''
        END + RM.EmployeeIDs [Recipients]
    ) R
    OUTER APPLY
    (
      SELECT
        [Subject] [EmailSubject],
        [Body] [EmailBody]
      FROM [notif].[GetMessage](NC.[EmailTemplateID], N.[TemplateData])
    ) ET
    OUTER APPLY
    (
      SELECT
        [Body] [SMSText]
      FROM [notif].[GetMessage](NC.[SMSTemplateID], N.[TemplateData])
    ) ST
    WHERE
      N.[IsProcessed] = 0
  ),
  [EmpWithPref]
  AS
  (
    SELECT
      E.EmployeeID,
      E.RoleID,
      E.Email,
      E.MobileNumber,
      ENP.SendEmail,
      ENP.SendSMS,
      ENP.WebNotification,
      ENP.MobileAppNotification
    FROM [dbo].[Employees] E
    INNER JOIN [notif].[EmployeeNotificationPreferences] ENP
      ON E.EmployeeID = ENP.EmployeeID
    WHERE
      E.IsActive = 1
      AND E.IsDeleted = 0
  ),
  [Receivers]
  AS
  (
    SELECT
      NED.NotificationID,
      ER.EmailRecipients,
      SR.SMSRecipients,
      WR.WebNotificationEmployeeIds,
      MR.MobileAppNotificationEmployeeIds
    FROM [NotificationEventData] NED
    CROSS APPLY
    (
      SELECT
        STRING_AGG(EWP.Email, ';') [EmailRecipients]
      FROM [EmpWithPref] EWP
      WHERE
        EWP.EmployeeID IN
        (
          SELECT
            [val]
          FROM [dbo].[GetCSVTable](NED.[Recipients])
        )
        AND EWP.SendEmail = 1
        AND EWP.Email IS NOT NULL
    ) ER
    CROSS APPLY
    (
      SELECT
        STRING_AGG(EWP.MobileNumber, ';') [SMSRecipients]
      FROM [EmpWithPref] EWP
      WHERE
        EWP.EmployeeID IN
        (
          SELECT
            [val]
          FROM [dbo].[GetCSVTable](NED.[Recipients])
        )
        AND EWP.SendSMS = 1
        AND EWP.MobileNumber IS NOT NULL
    ) SR
    CROSS APPLY
    (
      SELECT
        STRING_AGG(EWP.EmployeeID, ',') WebNotificationEmployeeIds
      FROM [EmpWithPref] EWP
      WHERE
        EWP.EmployeeID IN
        (
          SELECT
            [val]
          FROM [dbo].[GetCSVTable](NED.[Recipients])
        )
        AND EWP.WebNotification = 1
    ) WR
    CROSS APPLY
    (
      SELECT
        STRING_AGG(EWP.EmployeeID, ',') MobileAppNotificationEmployeeIds
      FROM [EmpWithPref] EWP
      WHERE
        EWP.EmployeeID IN
        (
          SELECT
            [val]
          FROM [dbo].[GetCSVTable](NED.[Recipients])
        )
        AND EWP.MobileAppNotification = 1
    ) MR
  )
  INSERT INTO @NotificationReceiversData
    SELECT
      N.[NotificationConfigurationID],
      N.[NotificationID],
      CASE
        WHEN N.[IsEmailSent] = 0 THEN R.[EmailRecipients]
        ELSE NULL
      END,
      N.[EmailSubject],
      N.[EmailBody],
      CASE
        WHEN N.[IsWebNotificationSent] = 0 THEN R.[WebNotificationEmployeeIds]
        ELSE NULL
      END,
      CASE
        WHEN N.[IsMobileAppNotificationSent] = 0 THEN R.[MobileAppNotificationEmployeeIds]
        ELSE NULL
      END,
      CASE
        WHEN N.[IsSMSSent] = 0 THEN R.[SMSRecipients]
        ELSE NULL
      END,
      N.[SMSText]
    FROM [NotificationEventData] N
    INNER JOIN [Receivers] R
      ON N.NotificationID = R.NotificationID;

  SELECT
    *
  FROM @NotificationReceiversData;

END