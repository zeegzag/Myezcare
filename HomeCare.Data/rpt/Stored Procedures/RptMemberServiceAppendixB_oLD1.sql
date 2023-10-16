
-- exec [rpt].[RptMemberServiceAppendixB]   37    
-- =============================================        
-- Author:  Ashar A        
-- Create date: 20 Oct 2021        
-- Description: <Description,,>        
-- =============================================        
CREATE PROCEDURE [rpt].[RptMemberServiceAppendixB_oLD1]        
 -- Add the parameters for the stored procedure here          
 @ReferralID bigint =0   
 --@StartDate date='',  
 --@EndDate date=''  
    AS     
          
DECLARE @cols AS NVARCHAR(MAX),          
@query AS NVARCHAR(MAX),          
@FirstDateOfMonth AS nvarchar(8),          
@LastDateOfMonth AS nvarchar(8);          
          
select @FirstDateOfMonth = FORMAT(DATEADD(d,-(DAY(GETDATE()-1)),GETDATE()),N'yyyyMMdd')          
select @LastDateOfMonth = FORMAT(DATEADD(d,-(day(DATEADD(m,1,GETDATE()))),DATEADD(m,1,GETDATE())),N'yyyyMMdd')    
--select @FirstDateOfMonth = FORMAT(@StartDate,N'yyyyMMdd')          
--select @LastDateOfMonth = FORMAT(@EndDate,N'yyyyMMdd')   
          
SET @cols = STUFF((SELECT ',' + QUOTENAME(MonthDay)          
FROM (SELECT TOP (DATEDIFF(DAY, @FirstDateOfMonth, @LastDateOfMonth) + 1)          
MonthDay = DAY(DATEADD(DAY, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @FirstDateOfMonth))          
FROM sys.all_objects a          
CROSS JOIN sys.all_objects b) c          
FOR XML PATH(''), TYPE          
).value('.', 'NVARCHAR(MAX)')          
,1,1,'')          
          
          
          
SET @query = 'SELECT PvtTbl.*        
        
FROM    (        
       select DISTINCT vt.VisitTaskDetail TaskDetail, DAY(evn.CreatedDate) [MonthDay],  
CASE WHEN ActivityStatus = ''Active'' THEN ''A'' WHEN ActivityStatus = ''PASSIVE'' THEN ''P'' WHEN ActivityStatus = ''REFUSED'' THEN ''R'' WHEN ActivityStatus = ''UNABLE'' THEN ''U''  
ELSE '''' END AS NoteStatus  
from VisitTasks vt left join ReferralTaskMappings rtm on vt.VisitTaskID=rtm.VisitTaskID and rtm.ReferralID='+CAST(@referralID AS VARCHAR(MAX))+' and rtm.IsDeleted=0  
left join EmployeeVisitNotes evn on evn.ReferralTaskMappingID=rtm.ReferralTaskMappingID  
left join employeevisits ev on ev.EmployeeVisitID=evn.EmployeeVisitID  
left join ScheduleMasters sm on sm.ScheduleID=ev.ScheduleID and sm.StartDate >'+@FirstDateOfMonth+' and EndDate<'+@LastDateOfMonth+' and vt.CareType=sm.CareTypeId  
    
        ) AS Data        
        
PIVOT   (        
    MAX(NoteStatus)         
    FOR [MonthDay]  IN(' + @cols + ')        
    ) as PvtTbl;'        
          
       print @query    
          
execute(@query)