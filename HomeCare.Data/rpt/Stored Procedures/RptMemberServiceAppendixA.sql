-- =============================================    
-- Author:  Ashar A    
-- Create date: 13 Oct 2021    
-- Description: <Description,,>    
-- =============================================    
CREATE PROCEDURE [rpt].[RptMemberServiceAppendixA]   
 -- Add the parameters for the stored procedure here    
 @ReferralID bigint  = 0  
AS    
    
DECLARE @cols AS NVARCHAR(MAX),    
@query AS NVARCHAR(MAX),    
@FirstDateOfMonth AS nvarchar(8),    
@LastDateOfMonth AS nvarchar(8);    
    
select @FirstDateOfMonth = FORMAT(DATEADD(d,-(DAY(GETDATE()-1)),GETDATE()),N'yyyyMMdd')    
select @LastDateOfMonth = FORMAT(DATEADD(d,-(day(DATEADD(m,1,GETDATE()))),DATEADD(m,1,GETDATE())),N'yyyyMMdd')    
  
SET @cols = STUFF((SELECT ',' + QUOTENAME(MonthDay)    
FROM (SELECT TOP (DATEDIFF(DAY, @FirstDateOfMonth, @LastDateOfMonth) + 1)    
MonthDay = DAY(DATEADD(DAY, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @FirstDateOfMonth))    
FROM sys.all_objects a    
CROSS JOIN sys.all_objects b) c    
FOR XML PATH(''), TYPE    
).value('.', 'NVARCHAR(MAX)')    
,1,1,'')    
    
    
    
SET @query = 'SELECT * FROM (  
SELECT vt.VisitTaskDetail, DAY(evn.CreatedDate) [MonthDay] ,  
EmployeeVisitNoteID  
FROM EmployeeVisitNotes evn  
inner join ReferralTaskMappings rtm on rtm.ReferralTaskMappingID=evn.ReferralTaskMappingID  
inner join VisitTasks vt on vt.VisitTaskID=rtm.VisitTaskID  
inner join VisitTaskCategories vtc on vtc.VisitTaskCategoryID=vt.VisitTaskCategoryID  
inner join EmployeeVisits ev on ev.EmployeeVisitID=evn.EmployeeVisitID  
inner join ScheduleMasters sm on sm.ScheduleID=ev.ScheduleID  
where vt.VisitTaskType=''task'' AND rtm.ReferralID  = '+ CAST(@ReferralID AS varchar) +'  AND (ev.CreatedDate BETWEEN '''+ @FirstDateOfMonth +''' AND '''+ @LastDateOfMonth +''')  
UNION  
SELECT Attendence ,DAY(EV.CreatedDate) [MonthDay], EmployeeVisitID  
FROM EmployeeVisits EV  
inner join ScheduleMasters SM ON  
EV.ScheduleID = SM.ScheduleID  
where  SM.ReferralID  = '+ CAST(@ReferralID AS varchar) +'  AND (EV.CreatedDate BETWEEN '''+ @FirstDateOfMonth +''' AND '''+ @LastDateOfMonth +''')  
)  
AS s PIVOT ( COUNT(EmployeeVisitNoteID)  
FOR [MonthDay] in (' + @cols + '))  
AS PIVOT1'  
    
    
    
execute(@query)