-- =============================================  
-- Author:  Fenil Gandhi  
-- Create date: 07 Jul 2020  
-- Description: This SP is used to get event data early clock in.  
-- =============================================  
CREATE PROCEDURE [notif].[EarlyClockIn]  
 @NotificationConfigurationID BIGINT,   
 @NotificationEventID BIGINT,  
 @FromDateTime DATETIME,  
 @ToDateTime DATETIME,  
 @BaseURL NVARCHAR(100)  
AS  
BEGIN  
   
 DECLARE @EarlyClockIn [notif].[EventDataTable];  
  
 INSERT INTO @EarlyClockIn  
  SELECT   
   REF.[RefID],  
   REF.[RefTableName],  
   TD.[Data] [TemplateData],  
            NULL [DefaultRecipients]  
  FROM  
   [dbo].[ScheduleMasters] SM  
  INNER JOIN [dbo].[EmployeeVisits] EV  
   ON EV.[ScheduleID] = SM.[ScheduleID]  
  CROSS APPLY ( SELECT EV.[EmployeeVisitID] [RefID], '[dbo].[EmployeeVisits]' [RefTableName] ) REF  
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
    E.[EmployeeID] = SM.[EmployeeID]  
  ) ED  
  CROSS APPLY (  
   SELECT   
    R.[FirstName] + ' ' + R.[LastName] [PatientName]  
   FROM  
    [dbo].[Referrals] R  
   WHERE  
    R.[ReferralID] = SM.[ReferralID]  
  ) PD  
  CROSS APPLY (  
   SELECT (   
    SELECT   
     ED.[EmployeeName] [##EmployeeName##],  
     PD.[PatientName] [##PatientName##],  
     'Early Clock-in' [##EventName##]  
    FOR JSON PATH  
   ) [Data]  
  ) TD  
  WHERE  
   N.NotificationID IS NULL  
   AND SM.[IsDeleted] = 0  
   AND EV.[IsDeleted] = 0  
   AND EV.[ClockInTime] < SM.[StartDate]  
   AND (EV.[CreatedDate] BETWEEN @FromDateTime AND @ToDateTime   
     OR EV.[UpdatedDate] BETWEEN @FromDateTime AND @ToDateTime);   
  
 SELECT * FROM @EarlyClockIn;  
  
END