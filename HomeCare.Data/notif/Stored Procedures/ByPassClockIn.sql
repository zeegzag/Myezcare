-- =============================================    
-- Author:  Fenil Gandhi    
-- Create date: 09 Dec 2020    
-- Description: This SP is used to get event data By-pass clock in.    
-- =============================================    
CREATE PROCEDURE [notif].[ByPassClockIn]    
 @NotificationConfigurationID BIGINT,     
 @NotificationEventID BIGINT,    
 @FromDateTime DATETIME,    
 @ToDateTime DATETIME,    
 @BaseURL NVARCHAR(100)    
AS    
BEGIN    
     
 DECLARE @ByPassClockIn [notif].[EventDataTable];  
 DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
    
 INSERT INTO @ByPassClockIn    
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
    dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) [EmployeeName]    
   FROM    
    [dbo].[Employees] E    
   WHERE    
    E.[EmployeeID] = SM.[EmployeeID]    
  ) ED    
  CROSS APPLY (    
   SELECT     
    dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) [PatientName]    
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
     EV.ByPassReasonClockIn [##Message##],  
     'By Pass Clock In' [##AlertFor##],  
     'By-pass Clock-in' [##EventName##]    
    FOR JSON PATH    
   ) [Data]    
  ) TD    
  WHERE    
   N.NotificationID IS NULL    
   AND SM.[IsDeleted] = 0    
   AND EV.[IsByPassClockIn] = 1  
   AND EV.[IsDeleted] = 0    
   AND (EV.[CreatedDate] BETWEEN @FromDateTime AND @ToDateTime     
     OR EV.[UpdatedDate] BETWEEN @FromDateTime AND @ToDateTime);     
    
 SELECT * FROM @ByPassClockIn;    
    
END 