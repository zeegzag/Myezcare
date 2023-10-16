 -- EXEC [rpt].[RptMemberServiceAppendixB]  '108','02','2022'
CREATE PROCEDURE [rpt].[RptMemberServiceAppendixB]        
 -- Add the parameters for the stored procedure here          
 @ReferralID bigint =0,   
 @Month nvarchar(max)=NULL,  
 @Year nvarchar(max)=NULL   
  as      
Begin
 If @month is null Begin select @month=CAST(datepart(month,getdate()) as nvarchar(50)) End

If @Year is null Begin select @year=CAST(datepart(YEar,getdate()) as nvarchar(50)) End

DECLARE @cols AS NVARCHAR(MAX),          
@query AS NVARCHAR(MAX),          
@FirstDateOfMonth AS nvarchar(8),          
@LastDateOfMonth AS nvarchar(8),
@LastDayOfMonth AS nvarchar(8);
set @LastDayOfMonth= FORMAT(DATEADD(d,-(day(DATEADD(m,1,GETDATE()))),DATEADD(m,1,GETDATE())),N'yyyyMMdd')

select @FirstDateOfMonth = FORMAT(DATEADD(d,-(DAY(GETDATE()-1)),GETDATE()),N'yyyyMMdd')          
select @LastDateOfMonth = FORMAT(DATEADD(d,-(day(DATEADD(m,1,GETDATE()))),DATEADD(m,1,GETDATE())),N'yyyyMMdd')    
         
SET @cols = STUFF((SELECT ',' + QUOTENAME(MonthDay)          
FROM (SELECT TOP (DATEDIFF(DAY, @FirstDateOfMonth, @LastDateOfMonth) +  1)          
MonthDay = DAY(DATEADD(DAY, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @FirstDateOfMonth))          
FROM sys.all_objects a          
CROSS JOIN sys.all_objects b) c          
FOR XML PATH(''), TYPE          
).value('.', 'NVARCHAR(MAX)')          
,1,1,'')          
          
 
 if @referralid=0 
 begin
 SET @query = 'SELECT PvtTbl.*        
        
FROM    (        
       select DISTINCT vt.VisitTaskDetail TaskDetail, DAY(sm.StartDate) [MonthDay],  
CASE WHEN ActivityStatus = ''Active'' THEN ''A'' WHEN ActivityStatus = ''PASSIVE'' THEN ''P'' WHEN ActivityStatus = ''REFUSED'' THEN ''R'' WHEN ActivityStatus = ''UNABLE'' THEN ''U''  
ELSE '''' END AS NoteStatus  
from VisitTasks vt left join ReferralTaskMappings rtm on vt.VisitTaskID=rtm.VisitTaskID and rtm.ReferralID='+CAST(@referralID AS VARCHAR(MAX))+' and rtm.IsDeleted=0  
left join EmployeeVisitNotes evn on evn.ReferralTaskMappingID=rtm.ReferralTaskMappingID  
left join employeevisits ev on ev.EmployeeVisitID=evn.EmployeeVisitID  
inner join ScheduleMasters sm on sm.ScheduleID=ev.ScheduleID and  DATEPART(MONTH, sm.StartDate)='+@Month+' and DATEPART(YEAR, sm.StartDate)='+@Year+' and vt.CareType=sm.CareTypeId  
 where 1=2
    
        ) AS Data        
        
PIVOT   (        
    MAX(NoteStatus)         
    FOR [MonthDay]  IN(' + @cols + ')        
    ) as PvtTbl;'        
          

End
 
else  
Begin         
          
SET @query = 'SELECT PvtTbl.*        
        
FROM    (        
       select DISTINCT vt.VisitTaskDetail TaskDetail, DAY(sm.StartDate) [MonthDay],  
CASE WHEN ActivityStatus = ''Active'' THEN ''A'' WHEN ActivityStatus = ''PASSIVE'' THEN ''P'' WHEN ActivityStatus = ''REFUSED'' THEN ''R'' WHEN ActivityStatus = ''UNABLE'' THEN ''U''  
ELSE '''' END AS NoteStatus  
from VisitTasks vt left join ReferralTaskMappings rtm on vt.VisitTaskID=rtm.VisitTaskID and rtm.ReferralID='+CAST(@referralID AS VARCHAR(MAX))+' and rtm.IsDeleted=0  
left join EmployeeVisitNotes evn on evn.ReferralTaskMappingID=rtm.ReferralTaskMappingID  
left join employeevisits ev on ev.EmployeeVisitID=evn.EmployeeVisitID  
inner join ScheduleMasters sm on sm.ScheduleID=ev.ScheduleID and  DATEPART(MONTH, sm.StartDate)='+@Month+' and DATEPART(YEAR, sm.StartDate)='+@Year+' and vt.CareType=sm.CareTypeId  
 
    
        ) AS Data        
        
PIVOT   (        
    MAX(NoteStatus)         
    FOR [MonthDay]  IN(' + @cols + ')        
    ) as PvtTbl;'        
          
       print @query    
 END   
 
execute(@query) 
End