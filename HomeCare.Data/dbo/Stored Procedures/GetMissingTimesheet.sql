-- =============================================  
-- Author:  <Author,Satya>  
-- Create date: <Create Date,05 Jan 2020,>  
-- Description: Retrieving the  Missing Timesheets for the schedules for which there is no clock in happened.  
-- =============================================  
CREATE PROCEDURE [dbo].[GetMissingTimesheet]  
@StartDate DATE,   
@EndDate DATE,  
@EmployeeIDs NVARCHAR(500) = NULL, -- optional parameter                                                 
@ReferralIDs NVARCHAR(500) = NULL  --optional Parameter  
AS  
BEGIN  
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
select sm.scheduleid,ev.scheduleid,sm.employeeid,sm.referralid,sm.CareTypeId,  
ev.ClockInTime,ev.ClockOutTime,sm.StartDate,sm.EndDate, dm.Title as CareType,  
dbo.GetGenericNameFormat(em.FirstName,em.MiddleName, em.LastName,@NameFormat) As EmployeeName,  
 dbo.GetGenericNameFormat(rf.FirstName,rf.MiddleName, rf.LastName,@NameFormat) As PatientName  
from ScheduleMasters sm   
left join EmployeeVisits ev on sm.scheduleid=ev.scheduleid   
left join DDMaster dm on dm.DDMasterID=sm.CareTypeId  
left join Employees em on em.EmployeeID = sm.employeeid  
left join Referrals rf on rf.ReferralID = sm.referralid  
where ev.employeevisitid is null and sm.scheduleid is not null and dm.Title is not null and ISNULL(SM.OnHold, 0) = 0 AND sm.isdeleted=0  
and Cast(sm.startdate as Date)>=@StartDate and Cast(sm.enddate as Date)<=@EndDate  
AND ((@EmployeeIDs IS NULL OR LEN(@EmployeeIDs)=0) OR (em.EmployeeID in (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@EmployeeIDs))))                                        
AND ((@ReferralIDs IS NULL OR LEN(@ReferralIDs)=0) OR (rf.ReferralID in (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@ReferralIDs))))                  
order by(sm.EndDate) desc  
END  