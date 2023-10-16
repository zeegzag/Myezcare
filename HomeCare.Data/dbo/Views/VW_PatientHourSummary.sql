 CREATE View [dbo].[VW_PatientHourSummary]
 As
 SELECT RefId, PatientName, COUNT(EmployeeVisitId) AS PatientVisits, SUM(TotalHours) as VisitHours,  SUM(Rate) as Amount
FROM
(
    SELECT
	--IIF (ed.DayOffStatus='Approved' or ed.EmployeeComment='Sick','Personal','Vacation') as k,

     e.EmployeeID AS EmpId,r.ReferralID as RefId  ,(CONVERT(decimal, DATEADD(minute, DATEDIFF(MINUTE, ev.ClockInTime, ev.ClockOutTime), 0), 114)  * scp.Rate) 'Rate',
        ev.EmployeeVisitId, CONVERT(decimal, DATEADD(minute, DATEDIFF(MINUTE, ev.ClockInTime, ev.ClockOutTime), 0), 114) AS TotalHours,PatientName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName)
    FROM  EmployeeVisits ev                                                                   
     INNER JOIN ScheduleMasters sm
            ON sm.ScheduleID=ev.ScheduleID                                                 
     LEFT JOIN Employees e
            ON e.EmployeeID=sm.EmployeeID
			inner join Referrals r on r.ReferralID=sm.ReferralID
			left join EmployeeDayOffs ed on ed.EmployeeID=e.EmployeeID
	 left JOIN dbo.PayorServiceCodeMapping scp on sm.PayorID=scp.PayorID
	 left JOIN dbo.ServiceCodes s on scp.ServiceCodeID=s.ServiceCodeID 			
) T
GROUP BY RefId,PatientName