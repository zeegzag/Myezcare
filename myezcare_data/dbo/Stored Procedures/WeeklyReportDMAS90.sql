--updatedBy      UpdatedDate      Description    
--Akhilesh       07-Aug-2019      For Employee Signature    
	

-- exec [WeeklyReportDMAS90] 13,42,'6/17/2019 12:00:00','6/23/2019 12:00:00'
  
----  exec [WeeklyReportDMAS90] 13,41,'2019-06-01 08:58:43.037','2019-07-30 08:59:08.143'  
CREATE PROCEDURE [dbo].[WeeklyReportDMAS90]   
@ReferralID bigint=null,   
@Caretype bigint=NULL,  
@startDate DateTime=null,  
@EndDate DateTime =null  
  
AS    
--declare @ReferralID int = '4', @CareType int = '41', @StartDate datetime = '07/14/2019 12:00:00 AM', @EndDate datetime = '07/20/2019 12:00:00 AM'  
begin   
if(@startDate is null)  
begin  
set @startdate=CONVERT(varchar, getdate(), 101)  

end    
if(@EndDate is null)  
begin  
set @EndDate=CONVERT(varchar, getdate(), 101)  
--select CONVERT(varchar, getdate(), 101)  
END  
  
IF  OBJECT_ID('tempdb.dbo.#WeeklyTimesheet') IS NOT null  
BEGIN  
   DROP TABLE  #WeeklyTimesheet;  
END  
CREATE table #WeeklyTimesheet (  
  
    VisitTaskDetail  varchar(100),  
    VisitTaskID int,  
    CareType int,  
 CareTypeName nvarchar(100),  
 ReferralID int,  
    ScheduleID int,  
    VisitTaskCategoryName nvarchar(1000),  
    ClockInTime time,  
    ClockOutTime time,  
    IVRClockOut bit,   
    IVRClockIn bit,   
    ScheduleDate date,  
 ScheduleEndDate date,  
    --sirEndDate  datetime,  
    isDone  varchar(10),  
    ReferralName Varchar(100),  
    EmployeeName Varchar(100),  
    --sirNotes nVarchar(max),  
    AlertComment  nvarchar(1000),  
 CreatedDay varchar(20),  
 Notes Varchar(max),  
 survey Varchar(max),  
 Phone1 bigInt,  
    ServiceTime bigInt,  
 PatientSignature Varchar(max),  
 PatientSignatureDate datetime,
 EmployeeSignatureID bigInt, 
 SignaturePath  Varchar(max),
 EmployeeSignatureDate datetime
 )  
DECLARE @dt datetime;  
SET DATEFIRST 1; -- First day of the week is set to monday  
DECLARE @DateFrom DateTime =@StartDate, @DateTo DateTime = @EndDate ;  
DECLARE Cur CURSOR Local for   
WITH CTE (dt)  
AS  
(  
      SELECT @DateFrom  
      UNION ALL  
      SELECT DATEADD(d, 1, dt) FROM CTE  
      WHERE dt < @DateTo  
)  
SELECT dt FROM CTE  where datepart ("dw", dt) = 1 OPTION (MAXRECURSION 0);  
   
OPEN cur  
FETCH NEXT from cur INTO @dt  
WHILE @@FETCH_STATUS=0   
 Begin  
  
    Insert into #WeeklyTimesheet(CareType,CareTypeName,ReferralID,ScheduleID, VisitTaskDetail,VisitTaskID,VisitTaskCategoryName,ClockInTime,ClockOutTime,IVRClockIn,IVRClockOut,ScheduleDate,ScheduleEndDate,isDone,ReferralName,EmployeeName,ServiceTime,AlertComment,CreatedDay,Notes,survey,Phone1,PatientSignature,PatientSignatureDate,EmployeeSignatureID,SignaturePath,EmployeeSignatureDate)  
 SELECT DISTINCT   
vt.CareType,dm.Title,R.ReferralID,  sm.ScheduleID,vt.VisitTaskDetail, vt.VisitTaskID, VisitTaskCategories.VisitTaskCategoryName,CONVERT(TIME(3),EmployeeVisits.ClockInTime),CONVERT(TIME(3),EmployeeVisits.ClockOutTime), EmployeeVisits.IVRClockOut,  
EmployeeVisits.IVRClockIn,sm.StartDate as ScheduleDate,sm.EndDate as ScheduleEndDate, CASE WHEN (evn.ServiceTime > 0 AND evn.servicetime IS NOT null) THEN 'YES' ELSE Null END AS isDone,  
R.LastName + ',' + R.FirstName AS PatientName, emp.LastName + ',' + emp.FirstName AS EmpName,evn.ServiceTime, evn.AlertComment,DateName(WEEKDAY,sm.StartDate) as [CreatedDay], CASE WHEN EmployeeVisits.surveyCompleted > 0 THEN EmployeeVisits.SurveyComment ELSE '' END AS Notes,CASE WHEN EmployeeVisits.surveyCompleted =1 THEN 'Yes' ELSE 'NO' END AS survey,c.Phone1,EmployeeVisits.PatientSignature,EmployeeVisits.CreatedDate as PatientSignatureDate,Emp.EmployeeSignatureID,es.SignaturePath,EmployeeVisits.CreatedDate As EmployeeSignatureDate 
FROM            VisitTasks AS vt  
     inner JOIN ReferralTaskMappings AS rtm ON vt.VisitTaskID = rtm.VisitTaskID AND rtm.ReferralID=@ReferralID   
     LEFT outer JOIN VisitTaskCategories ON vt.VisitTaskCategoryID = VisitTaskCategories.VisitTaskCategoryID  
                         
          
         Inner JOIN dbo.Referrals r ON rtm.ReferralID = r.ReferralID --AND r.ReferralID=@ReferralID  
      inner JOIN ScheduleMasters AS sm ON  sm.referralID = r.ReferralID  
                          
                        inner JOIN EmployeeVisits ON  sm.ScheduleID = EmployeeVisits.ScheduleID  
      inner  JOIN EmployeeVisitNotes AS evn ON evn.employeeVisitID=EmployeeVisits.EmployeeVisitID AND evn.ReferralTaskMappingID = rtm.ReferralTaskMappingID  
  
                           
                        LEFT OUTER JOIN  employees emp ON emp.EmployeeID = sm.EmployeeID  
      inner join DDMaster dm on dm.DDMasterID=vt.CareType  
      Inner join ContactMappings cm on cm.ReferralID=sm.ReferralID  
      inner join Contacts c on c.ContactID=cm.ContactID  
	  LEFT JOIN EmployeeSignatures es ON es.EmployeeSignatureID=emp.EmployeeSignatureID 
WHERE          
(r.ReferralID = @ReferralID) AND vt.VisitTaskType='Task'  AND dbo.EmployeeVisits.IsPCACompleted=1 AND EmployeeVisits.IsDeleted=0 AND vt.CareType=@CareType  
AND  
  
 (cast(sm.StartDate as date) >= @dt AND cast(sm.StartDate as date) <=DATEADD(DAY, 8 - DATEPART(WEEKDAY, @dt), CAST(@dt AS DATE)))  
  
  
FETCH NEXT from cur INTO @dt  
End -- End of Fetch  
CLOSE cur    
DEALLOCATE cur  
  --Update @PrintSheet set Thursday=(select top 1 isdone from #temp where convert(varchar(15),scheduledate,101)=convert(varchar(15),'2019-07-18',101))  
  --SELECT * FROM #temp where scheduledate='2019-07-18'  
  
  --select ScheduleID as ScheduleID,  
  --ScheduleDate as ScheduleDate,ScheduleEndDate as ScheduleEndDate,ReferralName as ReferralName,MAX(VisitTaskID) as VisitTaskID  from @WeeklyTimesheet  
  -- Group by ScheduleID,ScheduleDate,ScheduleEndDate,ReferralName  
IF OBJECT_ID('tempdb.dbo.#PrintSheet') IS not NULL  
BEGIN  
  DROP TABLE #PrintSheet;  
END
  
SELECT  VisitTaskDetail, VisitTaskID, CareType ,CareTypeName,ReferralID ,ScheduleID ,VisitTaskCategoryName,IVRClockOut,IVRClockIn,ScheduleDate ,
ScheduleEndDate ,isDone  ,ReferralName ,EmployeeName ,  AlertComment  ,  CreatedDay ,  Notes ,survey,  Phone1,ServiceTime ,  PatientSignature ,  PatientSignatureDate,EmployeeSignatureID,SignaturePath,EmployeeSignatureDate FROM #WeeklyTimesheet wt  
 
  CREATE TABLE  #PrintSheet  (  
  ActivityName varchar(100),  
  ActivityId int,  
  monday varchar(100),  
  Tuesday varchar(100),  
  Wednesday varchar(100),  
  Thursday varchar(100),  
  Friday varchar(100),  
  Saturday varchar(100),  
  Sunday varchar(100))  
  --Clockin varchar(50),  
  --ClockOut varchar(50))  
 --select distinct datename(weekday,scheduledate) [day],convert(varchar(15),scheduledate,101) sdate from @WeeklyTimesheet   
  --insert into #PrintSheet (activityname, activityID) values ('WDate',700)  
  insert into #PrintSheet (activityname, activityID) select distinct VisitTaskDetail,VisitTaskID from dbo.VisitTasks WHERE dbo.VisitTasks.CareType=@careType AND dbo.VisitTasks.IsDeleted=0   
    
   insert into #PrintSheet (activityname, activityID) values ('DAILY TIME IN',701)  
   insert into #PrintSheet (activityname, activityID) values ('DAILY TIME OUT',702)  
    insert into #PrintSheet (activityname, activityID) values ('NUMBER OF HOURS',703)  
  
 DECLARE @SQL Nvarchar(max)  
  declare @Day varchar(15)  
  Declare @sDate date  
  Declare rptCur cursor local for select distinct datename(weekday,scheduledate) [day],convert(varchar(15),scheduledate,101) sdate from #WeeklyTimesheet  
   OPEN rptcur  
 FETCH NEXT from rptcur INTO @day,@sDate  
WHILE @@FETCH_STATUS=0  
 BEGIN  
-- SELECT distinct ps.*,isdone, createdDay FROM #PrintSheet ps INNER JOIN #WeeklyTimesheet wt ON ps.ActivityId=wt.VisitTaskID WHERE scheduledate='2019-07-18'  
--select isdone,createdday from #WeeklyTimesheet inner join #PrintSheet ON visitTaskID=activityID  WHERE createdDay='Thursday' AND scheduledate='2019-07-18'  
 SET @SQL='update #PrintSheet set '+@Day+'=convert(varchar(20),scheduledate,101) from #WeeklyTimesheet WHERE createdDay='''+@DAY+''' AND scheduledate='''+CAST(@SDATE AS VARCHAR(20))+''' and ActivityID=700'  
 PRINT @SQL  
 EXEC SP_EXECUTESQL @SQL   
 SET @SQL='update #PrintSheet set '+@Day+'=isdone from #WeeklyTimesheet inner join #PrintSheet ON visitTaskID=activityID  WHERE createdDay='''+@DAY+''''-- AND scheduledate='''+CAST(@SDATE AS VARCHAR(20))+''''  
 PRINT @SQL  
 EXEC SP_EXECUTESQL @SQL   
 SET @SQL='update #PrintSheet set '+@Day+'=convert(varchar,ClockInTime,100) from #WeeklyTimesheet WHERE createdDay='''+@DAY+''' AND scheduledate='''+CAST(@SDATE AS VARCHAR(20))+''' and ActivityID=701'  
 PRINT @SQL  
 EXEC SP_EXECUTESQL @SQL   
 SET @SQL='update #PrintSheet set '+@Day+'=convert(varchar,ClockOutTime,100) from #WeeklyTimesheet WHERE createdDay='''+@DAY+''' AND scheduledate='''+CAST(@SDATE AS VARCHAR(20))+''' and ActivityID=702'  
 PRINT @SQL  
 EXEC SP_EXECUTESQL @SQL   
 SET @SQL='update #PrintSheet set '+@Day+'=cast(Convert(Time,cast(clockOutTime as datetime) - cast(ClockInTime as dateTime)) as varchar(5))+'' Hrs'' from #WeeklyTimesheet WHERE createdDay='''+@DAY+''' AND scheduledate='''+CAST(@SDATE AS VARCHAR(20))+''' and ActivityID=703'  
 --PRINT @SQL  
 --EXEC SP_EXECUTESQL @SQL   
  FETCH NEXT from rptcur INTO @day,@sDate  
   
  END --End of Fetch rptCur  
    
 close rptcur  
 deallocate rptcur  
-- SELECT * FROM #WeeklyTimesheet wt  
 Select * from #PrintSheet  
    DROP TABLE  #WeeklyTimesheet;  
   DROP TABLE  #PrintSheet;  
 END  
  
 --SELECT convert(varchar(20),getdate(),101)
