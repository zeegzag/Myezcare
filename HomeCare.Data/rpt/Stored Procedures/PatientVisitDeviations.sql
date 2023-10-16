-- EXEc [rpt].[PatientVisitDeviations] NULL, NULL, 0, 0
-- =============================================
CREATE PROC [rpt].[PatientVisitDeviations]
	@FromDate DATE = NULL,
	@ToDate DATE = NULL,
	@Payor VARCHAR(4000)='0',
	@CareType VARCHAR(4000)='0'
AS
BEGIN
 SELECT ReferralID,PatientName,
 FORMAT(TRY_CAST(ScheduleDate AS DATE),'MM/dd/yyyy') as ScheduleDate,
 Service,Insurance,BillHours,0 AS BillUnit,
 VisitHours,DevHours,Comments,DeviationType,EmployeeID,EmployeeName
FROM
(
	select
	r.ReferralID,PatientName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName),
	CONVERT(VARCHAR(10), sm.startDate, 111) as ScheduleDate,
	d.Title as Service,CASE WHEN p.PayorID IS NULL THEN 'NO BILL'  ELSE p.PayorName  END AS Insurance,
	DATEDIFF(minute,ev.ClockInTime,ev.ClockOutTime) AS BillHours,
	DATEDIFF(minute,ev.ClockInTime,ev.ClockOutTime) AS VisitHours,dn.DeviationTime as DevHours,
	dn.DeviationNotes as Comments,dm.Title as DeviationType,
	e.EmployeeID as EmployeeID,EmployeeName=dbo.GetGeneralNameFormat(e.FirstName,e.LastName)
	from Referrals r
	left join ScheduleMasters sm on sm.ReferralID=r.ReferralID
	left join EmployeeVisits ev on ev.ScheduleID=sm.ScheduleID
	left join SaveDeviationNote dn on dn.EmployeeVisitID=ev.EmployeeVisitID
	left join DDMaster dm on dm.DDMasterID=dn.DeviationID
	left JOIN DDMaster d on d.DDMasterID=sm.CareTypeId 
	left join Employees e on e.EmployeeID=sm.EmployeeID
	left join Payors p on p.PayorID=sm.PayorID 
	where r.isdeleted=0                                                                        
	AND  ((@FromDate is null or @ToDate is null) or  ev.CreatedDate BETWEEN @FromDate AND @Todate)
	AND 
	(@Payor = '0' OR  TRY_CAst(P.PayorID AS varchar(100)) in (select Item from dbo.SplitString(@Payor, ',')))
	AND 
	(@CareType = '0' OR  TRY_CAst(sm.CareTypeId AS varchar(100)) in (select Item from dbo.SplitString(@CareType, ',')))
	--AND  (@EmployeeID = '0' OR  TRY_CAst(E.EmployeeID AS varchar(100)) in (select Item from dbo.SplitString(@EmployeeID, ',')))
) T
--GROUP BY ReferralID,PatientName
END