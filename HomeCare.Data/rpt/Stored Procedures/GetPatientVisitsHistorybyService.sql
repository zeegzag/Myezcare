-- EXEC [RPT].[GetPatientVisitsHistorybyService] '2020-11-01 00:00:00','2021-01-03 00:00:00', 0,0,0,0,2
-- =============================================
CREATE PROC [rpt].[GetPatientVisitsHistorybyService]
	@FromDate DATE = NULL,
	@ToDate DATE = NULL,
	@Employee VARCHAR(4000)='0',
	@Patient VARCHAR(4000)='0',
	@Payor VARCHAR(4000)='0',
	@CareType VARCHAR(4000)='0',
	@ActionTaken VARCHAR(4000)='0'
AS
BEGIN

 SELECT ReferralID,PatientName,
	 FORMAT(TRY_CAST(ScheduleDate AS DATE),'MM/dd/yyyy') AS ScheduleDate,
	 ClockInTime,ClockOutTime,ServiceName,ServiceCode,
	 EmpID,EmployeeName,
	 Hours,BillHours,Amount,ROUND(DISTANCE,2) AS DISTANCE,
	 Supplies,InsCompany,
	 COUNT(EmployeeVisitID) AS EmployeeVisits,
	 EmployeeVisitID
FROM
(
	select 
		e.EmployeeID AS EmpID,
		EmployeeName=dbo.GetGeneralNameFormat(e.FirstName,e.LastName),
		d.Title As ServiceName,
		SC.ServiceCode,
		sm.StartDate as ScheduleDate,
		r.ReferralID AS ReferralID,
		dbo.GetGeneralNameFormat(r.FirstName,r.LastName) AS PatientName,
		CONVERT(varchar(15),  CAST(ev.ClockInTime AS TIME), 100) as ClockInTime,
		CONVERT(varchar(15),  CAST(ev.ClockOutTime AS TIME), 100) as ClockOutTime,
		0 AS TravelTime,
		ev.EmployeeVisitID,
		DATEDIFF(HOUR,sm.StartDate,sm.EndDate) as Hours,
		DATEDIFF(HOUR,sm.StartDate,sm.EndDate) AS BillHours,
		0 AS Amount,
		(([dbo].GetGeoFromLatLng(E.Latitude,E.Longitude)).STDistance([dbo].GetGeoFromLatLng(C.Latitude,C.Longitude)) * 0.000621371) AS DISTANCE,
		0 AS Supplies,
		p.PayorName as InsCompany
	from EmployeeVisits ev WITH(NOLOCK)
	inner join ScheduleMasters sm WITH(NOLOCK) on sm.ScheduleID=ev.ScheduleID
	inner join Employees e WITH(NOLOCK) on e.EmployeeID=sm.EmployeeID
	inner join Referrals r WITH(NOLOCK) on r.ReferralID=sm.ReferralID
	left join Payors p WITH(NOLOCK) on p.PayorID=sm.PayorID 
	LEFT JOIN ContactMappings CM WITH(NOLOCK) ON CM.ReferralID = r.ReferralID AND CM.ContactTypeID=1 
	LEFT JOIN Contacts C WITH(NOLOCK) ON C.ContactID= CM.ContactID 
	inner JOIN DDMaster d WITH(NOLOCK) on d.DDMasterID=sm.CareTypeId 
	left join PayorServiceCodeMapping PSM WITH(NOLOCK)  on PSM.PayorID=p.PayorID
	left JOIN ServiceCodes SC WITH(NOLOCK) on SC.ServiceCodeID=PSM.ServiceCodeID     
	WHERE 
	((@FromDate is null or @ToDate is null) or  CONVERT(DATE, ev.CreatedDate) BETWEEN @FromDate AND @Todate)
	AND 
	(@Employee = '0' OR  TRY_CAst(E.EmployeeID AS varchar(100)) in (select Item from dbo.SplitString(@Employee, ',')))
		AND 
	(@Patient = '0' OR  TRY_CAst(r.ReferralID AS varchar(100)) in (select Item from dbo.SplitString(@Patient, ',')))
	AND 
	(@Payor = '0' OR  TRY_CAst(p.PayorID AS varchar(100)) in (select Item from dbo.SplitString(@Payor, ',')))
		AND 
	(@CareType = '0' OR  TRY_CAst(sm.CareTypeId AS varchar(100)) in (select Item from dbo.SplitString(@CareType, ',')))
	AND 
	(@ActionTaken = '0' OR  TRY_CAst(ev.ActionTaken AS varchar(100)) in (select Item from dbo.SplitString(@ActionTaken, ',')))
) T
GROUP BY EmpID,EmployeeName,
 ServiceName,ServiceCode,ScheduleDate,ReferralID,PatientName,ClockInTime,ClockOutTime,
 TravelTime, EmployeeVisitID,Hours,BillHours,Amount,DISTANCE,
 Supplies,InsCompany
END