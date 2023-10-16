
-- EXEC GetScheduleNotificationLogs 2179
CREATE PROCEDURE [dbo].[GetScheduleNotificationLogs]
@ScheduleID bigint
AS
BEGIN

	 SELECT NotificationType, E.FirstName+' '+E.LastName AS CreatedBy, SNL.CreatedDate,SNL.ToEmailAddress,SNL.ToPhone  FROM ScheduleNotificationLogs SNL
	 LEFT JOIN Employees E ON E.EmployeeID=SNL.CreatedBy
	 WHERE SNL.ScheduleID=@ScheduleID AND SNL.IsSent=1
	 GROUP BY SNL.NotificationType,E.FirstName, E.LastName, SNL.CreatedDate,SNL.ToEmailAddress,SNL.ToPhone
	 ORDER BY NotificationType

END
