--  exec [WeeklyReportDMAS90] 13,41,'2019-06-12 08:58:43.037','2019-06-24 08:59:08.143'
CREATE PROCEDURE [dbo].[WeeklyReportDMAS90_Pallav] 
@ReferralID bigint=null, 
@Caretype bigint=NULL,
@startDate DateTime=null,
@EndDate DateTime =null

AS

begin 
if(@startDate is null)
begin
set @startdate=CONVERT(varchar, getdate(), 101)


end

if(@EndDate is null)
begin
set @EndDate=CONVERT(varchar, getdate(), 101)
--select CONVERT(varchar, getdate(), 101)
end

Declare @WeeklyTimesheet Table(

    VisitTaskDetail  varchar(100),
    VisitTaskID int,
    CareType int,
	CareTypeName nvarchar(100),
	ReferralID int,
    ScheduleID int,
    VisitTaskCategoryName nvarchar(1000),
    ClockInTime datetime,
    ClockOutTime datetime,
    IVRClockOut bit, 
    IVRClockIn bit, 
    ScheduleDate datetime,
	ScheduleEndDate datetime,
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
	PatientSignatureDate datetime
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

    Insert into @WeeklyTimesheet(CareType,CareTypeName,ReferralID,ScheduleID, VisitTaskDetail,VisitTaskID,VisitTaskCategoryName,ClockInTime,ClockOutTime,IVRClockIn,IVRClockOut,ScheduleDate,ScheduleEndDate,isDone,ReferralName,EmployeeName,ServiceTime,AlertComment,CreatedDay,Notes,survey,Phone1,PatientSignature,PatientSignatureDate)
SELECT DISTINCT
vt.CareType,dm.Title,R.ReferralID,  sm.ScheduleID,vt.VisitTaskDetail, vt.VisitTaskID, VisitTaskCategories.VisitTaskCategoryName,CONVERT(TIME(3),EmployeeVisits.ClockInTime),CONVERT(TIME(3),EmployeeVisits.ClockOutTime), EmployeeVisits.IVRClockOut,
EmployeeVisits.IVRClockIn,sm.StartDate as ScheduleDate,sm.EndDate as ScheduleEndDate, CASE WHEN evn.ServiceTime > 0 THEN 'YES' ELSE Null END AS isDone,
R.LastName + ',' + R.FirstName AS PatientName, emp.LastName + ',' + emp.FirstName AS EmpName,evn.ServiceTime, evn.AlertComment,DateName(WEEKDAY,evn.CreatedDate) as [CreatedDay], CASE WHEN EmployeeVisits.surveyCompleted > 0 THEN EmployeeVisits.SurveyComment ELSE '' END AS Notes,CASE WHEN EmployeeVisits.surveyCompleted =1 THEN 'Yes' ELSE 'NO' END AS survey,c.Phone1,EmployeeVisits.PatientSignature,EmployeeVisits.CreatedDate as PatientSignatureDate
FROM            VisitTasks AS vt
                          left OUTER JOIN ReferralTaskMappings AS rtm ON vt.VisitTaskID = rtm.VisitTaskID
                         INNER JOIN ScheduleMasters AS sm ON sm.referralID = rtm.referralID
                         left outer JOIN VisitTaskCategories ON vt.VisitTaskCategoryID = VisitTaskCategories.VisitTaskCategoryID
                         left outer JOIN EmployeeVisitNotes AS evn ON rtm.ReferralTaskMappingID = evn.ReferralTaskMappingID
                         inner JOIN EmployeeVisits ON  sm.ScheduleID = EmployeeVisits.ScheduleID
                         left JOIN dbo.Referrals r ON rtm.ReferralID = r.ReferralID
                        left JOIN employees emp ON emp.EmployeeID = sm.EmployeeID
						inner join DDMaster dm on dm.DDMasterID=vt.CareType
						left join ContactMappings cm on cm.ReferralID=sm.ReferralID
						inner join Contacts c on c.ContactID=cm.ContactID
WHERE        
(rtm.ReferralID = @ReferralID) AND vt.VisitTaskType='Task'  AND (vt.CareType = @Caretype)
AND
 (sm.StartDate BETWEEN @dt AND DATEADD(DAY, 8 - DATEPART(WEEKDAY, @dt), CAST(@dt AS DATE)))


FETCH NEXT from cur INTO @dt
End -- End of Fetch
CLOSE cur  
DEALLOCATE cur
  --select * from @WeeklyTimesheet

  select ScheduleID as ScheduleID,
  ScheduleDate as ScheduleDate,ScheduleEndDate as ScheduleEndDate,ReferralName as ReferralName,MAX(VisitTaskID) as VisitTaskID  from @WeeklyTimesheet
   Group by ScheduleID,ScheduleDate,ScheduleEndDate,ReferralName


  Declare @PrintSheet table (
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

  
  insert into @printsheet (activityname, activityID) select distinct VisitTaskDetail,VisitTaskID from @WeeklyTimesheet
   insert into @printsheet (activityname, activityID) values ('ClockinTime',701)
   insert into @printsheet (activityname, activityID) values ('ClockOutTime',702)
  declare @Day varchar(15)
  Declare @sDate datetime
  Declare rptCur cursor local for select distinct datename(weekday,scheduledate) [day],scheduledate sdate from @WeeklyTimesheet
   OPEN rptcur
 FETCH NEXT from rptcur INTO @day,@sDate
WHILE @@FETCH_STATUS=0
 BEGIN
	
		if(@day='Monday')  
			update @PrintSheet set Monday=(select isdone from @WeeklyTimesheet where activityname=visittaskdetail AND [@WeeklyTimesheet].ScheduleDate=@sdate )
		else if(@day='Tuesday')  
			update @PrintSheet set Tuesday=(select isdone from @WeeklyTimesheet where activityname=visittaskdetail AND [@WeeklyTimesheet].ScheduleDate=@sdate)
		else if(@day='Wednesday')  
			update @PrintSheet set Wednesday=(select isdone from @WeeklyTimesheet where activityname=visittaskdetail AND [@WeeklyTimesheet].ScheduleDate=@sdate)
		else if(@day='Thursday')  
			update @PrintSheet set Thursday=(select isdone from @WeeklyTimesheet where activityname=visittaskdetail AND [@WeeklyTimesheet].ScheduleDate=@sdate)
		else if(@day='Friday')  
			update @PrintSheet set Friday=(select isdone from @WeeklyTimesheet where activityname=visittaskdetail AND [@WeeklyTimesheet].ScheduleDate=@sdate)
		else if(@day='Saturday')  
			update @PrintSheet set Saturday=(select isdone from @WeeklyTimesheet where activityname=visittaskdetail AND [@WeeklyTimesheet].ScheduleDate=@sdate)
		else if(@day='Sunday')  
			update @PrintSheet set Sunday=(select isdone from @WeeklyTimesheet where activityname=visittaskdetail AND [@WeeklyTimesheet].ScheduleDate=@sdate)
  FETCH NEXT from rptcur INTO @day,@sDate
 
  END --End of Fetch rptCur
  
 close rptcur
 deallocate rptcur
 Select * from @PrintSheet
 END
 SELECT * FROM @WeeklyTimesheet wt
 --EXEC WeeklyReportDMAS90_Pallav @ReferralID = '13', @CareType = '41', @StartDate = '7/14/2019 12:00:00 AM', @EndDate = '7/17/2019 12:00:00 AM'


--SELECT VisitTaskDetail, 
--       [Monday], 
--       [Tuesday], 
--       [Wednesday], 
--       [Thursday], 
--       [Friday] 
--       [Saturday], 
--       [Sunday]
--FROM   (SELECT VisitTaskDetail, 
--               isDone, 
--               ClockInTime,
--               ClockOutTime,
--               Datename(weekday, ScheduleDate ) DAY from @WeeklyTimesheet
--			 ) p 
--      PIVOT ( 
--        count(isDone)
--        FOR DAY IN ([Monday], [Tuesday], [Wednesday], [Thursday], [Friday], [Saturday], [Sunday])) pvt

--end Try
--Begin Catch
----CLOSE cur  
----DEALLOCATE cur
--SELECT
-- ERROR_NUMBER() AS ErrorNumber,
-- ERROR_STATE() AS ErrorState,
-- ERROR_SEVERITY() AS ErrorSeverity,
-- ERROR_PROCEDURE() AS ErrorProcedure,
-- ERROR_LINE() AS ErrorLine,
-- ERROR_MESSAGE() AS ErrorMessage;
--End Catch

----  exec [WeeklyReportDMAS90] 13,41,'2019-01-25 08:58:43.037','2019-06-24 08:59:08.143'