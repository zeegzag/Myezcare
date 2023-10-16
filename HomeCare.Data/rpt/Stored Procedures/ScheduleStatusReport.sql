CREATE PROCEDURE [rpt].[ScheduleStatusReport]      
--Declare    
@ReferralID BIGINT = NULL,    
@EmployeeID BIGINT = NULL,    
@StartDate DATE = NULL,    
@EndDate DATE = NULL  ,  
@TimesheetStatus varchar(3)='ALL'  
--select  @ReferralID=0,@EmployeeID=0,@StartDate='2022-01-01 00:00:00',@EndDate='2022-01-04 00:00:00',@TimesheetStatus='ALL'  
  
as    
BEGIN      
  
Print @timesheetstatus  
declare @orgtype nvarchar(max)    
declare @DateFormat nvarchar(max)    
select @DateFormat= [Admin_Myezcare_Live].[dbo].[fn_getDateFormat](db_name())    
    
select @orgtype=[dbo].[GetOrgType]()    
--select CHARINDEX('DayCare', @orgtype, 0)    
    
  
select  sm.ScheduleID,ev.EmployeeVisitID,e.EmployeeID,r.ReferralID,    
FORMAT(StartDate,@DateFormat) as[StartDate],    
FORMAT(EndDate,@DateFormat) as[EndDate],    
CONVERT(varchar(15),  CAST(ev.ClockInTime AS TIME), 100) as ClockInTime,    
CONVERT(varchar(15),  CAST(ev.ClockOutTime AS TIME), 100) as ClockOutTime,    
    
CONCAT((DATEDIFF(Minute,startdate,enddate)/60),':',    
       (DATEDIFF(Minute,startdate,enddate)%60))     ScheduleHours,    
CONCAT((DATEDIFF(Minute,ClockInTime,ClockOutTime)/60),':',    
       (DATEDIFF(Minute,ClockInTime,ClockOutTime)%60))     ActualHours,    
sm.CareTypeId, d.title as CareType,    
case rtsd.OnHold when 'True' then 'Yes' ELSE 'No' end as OnHold,    
case rtsd.IsDenied when 'True' then 'Yes' ELSE 'No' end as IsDenied,    
case ev.IsPCACompleted when 'True' then 'Yes' ELSE 'No' end as IsPCACompleted,    
case when ev.IsPCACompleted='True' and rtsd.IsDenied='False' and rtsd.OnHold='False' and sm.IsDeleted=0 and ev.IsDeleted=0 then 'Completed'     
when  rtsd.IsDenied='True' and rtsd.OnHold='False' then 'Denied'    
when rtsd.IsDenied='False' and rtsd.OnHold='True' then 'Hold'    
when  rtsd.IsDenied='False' and rtsd.OnHold='False' and sm.ScheduleStatusID=2 then 'Scheduled'     
    
ELSE 'InComplete' end as Status,    
ISNULL(rtsd.Notes, 'NA') as Notes,    
dbo.GetGeneralNameFormat(e.FirstName, e.LastName) AS EmployeesName,    
dbo.GetGeneralNameFormat(r.FirstName, r.LastName) AS ReferralName,    
case  when CHARINDEX('DayCare', @orgtype, 0)>0 then f.FacilityName else 'abc' end facilityName,    
case   
 when ev.IsPCACompleted=1 then 'Complete'   
 when ev.EmployeeVisitID is  null and sm.AbsentReason is null then 'Not Started'  
 when ev.EmployeeVisitID is  null and sm.AbsentReason is not null then 'Absent'  
 when ev.EmployeeVisitID is not null and ev.IsPCACompleted=0 and  sm.AbsentReason is null and (ev.ClockInTime is not null) then 'Present and Incomplete'  
 --when ev.IsPCACompleted=0 and   ev.ClockOutTime is not null   then 'InComplete'   
 end Attendance  
   
--case when ev.EmployeeVisitID is not null then 'Present' else 'Absent' end Attendance    
from schedulemasters sm    
LEFT join EmployeeVisits ev on ev.ScheduleID=sm.ScheduleID    
--LEFT join EmployeeVisitNotes evn on evn.EmployeeVisitID=ev.EmployeeVisitID    
left join employees e on e.EmployeeID=sm.EmployeeID    
inner join Referrals r on r.ReferralID=sm.ReferralID    
inner join ReferralTimeSlotDates rtsd on rtsd.ReferralTSDateID=sm.ReferralTSDateID    
inner join DDMaster d on d.DDMasterID = sm.CareTypeId    
left join Facilities F on f.FacilityID=r.DefaultFacilityID    
    
WHERE    
  (@EmployeeID =0 or @EmployeeID is null OR e.EmployeeID = @EmployeeID)    
  AND  
  (@ReferralID =0 or @ReferralID is null OR r.ReferralID = @ReferralID)    
AND ((@StartDate is null or @EndDate is null) or  sm.StartDate BETWEEN @StartDate AND @EndDate)    
AND ((@StartDate IS NOT NULL AND @EndDate IS NULL AND CONVERT(DATE,SM.StartDate) >= @StartDate) OR                   
(@EndDate IS NOT NULL AND @StartDate IS NULL AND CONVERT(DATE,SM.EndDate) <= @EndDate) OR                  
(@StartDate IS NOT NULL AND @EndDate IS NOT NULL AND (CONVERT(DATE,SM.StartDate) >= @StartDate AND CONVERT(DATE,SM.EndDate) <= @EndDate)) OR                  
(@StartDate IS NULL AND @EndDate IS NULL)                  
) and  
(@TimesheetStatus='ALL' or   
  
 (@TimesheetStatus= 'C' and ev.IsPCACompleted=1 )   
 or  
  (@TimesheetStatus= 'NS' and  (ev.EmployeeVisitID is  null and sm.AbsentReason is null ))  
 or  
  (@TimesheetStatus= 'A' and  (ev.EmployeeVisitID is  null and sm.AbsentReason is not null))  
 or   
  (@TimesheetStatus= 'PIC' and  ev.EmployeeVisitID is not null and sm.AbsentReason is null )  
-- --or   
-- -- (@TimesheetStatus= 'IC' and ev.IsPCACompleted=0 and ev.ClockOutTime is not null )   
)  
   
  
  
order by startdate asc    
END