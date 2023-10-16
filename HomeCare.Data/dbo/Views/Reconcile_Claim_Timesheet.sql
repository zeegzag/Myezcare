/*and (datediff(minute,clockinTime,clockoutTime)/60>8 or datediff(minute,clockinTime,clockoutTime)<0)*/
CREATE View [dbo].[Reconcile_Claim_Timesheet] as
SELECT        E.FirstName E_FirstName, E.LastName E_LastName, 
                         R.FirstName AS P_FirstName, R.LastName AS P_LastName,p.PayorName, ev.EmployeeVisitID, sm.StartDate, ev.ClockInTime, sm.EndDate, ev.ClockOutTime, DATEDIFF(minute, sm.StartDate, ev.ClockInTime) AS StartDate_CIT, DATEDIFF(minute, ev.ClockInTime, ev.ClockOutTime) 
                         AS ShiftTime, DATEDIFF(minute, ev.ClockInTime, ev.ClockOutTime) - n.CalculatedServiceTime AS MinDifference, n.CalculatedServiceTime, n.CalculatedUnit
FROM            ScheduleMasters AS sm INNER JOIN
                         EmployeeVisits AS ev ON sm.ScheduleID = ev.ScheduleID INNER JOIN
                         Notes AS n ON n.EmployeeVisitID = ev.EmployeeVisitID INNER JOIN
                         Referrals AS R ON sm.ReferralID = R.ReferralID INNER JOIN
                         Employees AS E ON sm.EmployeeID = E.EmployeeID INNER JOIN
                         Payors AS p ON sm.PayorID = p.PayorID
--WHERE        (sm.StartDate > '2020-03-31')