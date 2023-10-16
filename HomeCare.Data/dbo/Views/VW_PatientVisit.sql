CREATE View [dbo].[VW_PatientVisit]
AS
SELECT        p.ShortName, CONVERT(TIME(3), ev.ClockInTime) AS ClockInTime, CONVERT(TIME(3), ev.ClockOutTime) AS ClockOutTime,
                         e.LastName + ',' + e.FirstName AS EmployeeName, e.EmployeeUniqueID, r.LastName + ',' + r.FirstName AS ReferralName, 
                         r.ReferralID, e.PayPerHour,CONVERT(varchar(5), DATEADD(minute, DATEDIFF(MINUTE, ClockInTime, ClockOutTime), 0), 114) AS BillableHrs, CONVERT(DATE, sm.StartDate) AS StartDate, 
                         CONVERT(DATE, sm.EndDate) AS EndDate,dbo.DDMaster.Title as CareType,p.PayorName,ro.RoleName,(CONVERT(decimal, DATEADD(minute, DATEDIFF(MINUTE, ev.ClockInTime, ev.ClockOutTime), 0), 114)  * scp.Rate)as Amount,s.ServiceCode
FROM            dbo.ScheduleMasters sm
                         INNER JOIN dbo.Referrals  r ON r.ReferralID = sm.ReferralID
						 INNER JOIN dbo.EmployeeVisits ev ON sm.ScheduleID = ev.ScheduleID 
						 INNER JOIN dbo.Employees e ON e.EmployeeID = sm.EmployeeID 
						 INNER JOIN dbo.Payors p ON sm.PayorID = p.PayorID 
						 INNER JOIN dbo.DDMaster ON sm.CareTypeId = dbo.DDMaster.DDMasterID
						 INNER JOIN dbo.Roles ro ON e.RoleID=ro.RoleID
						 INNER JOIN dbo.PayorServiceCodeMapping scp on p.PayorID=scp.PayorID
						 INNER JOIN dbo.ServiceCodes s on scp.ServiceCodeID=s.ServiceCodeID