CREATE view [dbo].[EmployeeVisitRpt]
as
SELECT       scheduleMasters.ScheduleId, Payors.ShortName, EmployeeVisits.ClockInTime, EmployeeVisits.ClockOutTime, 
Employees.LastName+','+Employees.FirstName as EmployeeName, Employees.EmployeeUniqueID, DDMaster.Value, 
                         Referrals.LastName +','+Referrals.FirstName AS ReferralName, Referrals.ReferralID, Employees.PayPerHour,
						 datediff(mi,EmployeeVisits.ClockInTime,EmployeeVisits.ClockOutTime) as  BillableHrs,ScheduleMasters.StartDate,ScheduleMasters.EndDate
FROM            ScheduleMasters INNER JOIN
                         
                         Referrals ON Referrals.ReferralID = ScheduleMasters.ReferralID INNER JOIN
						 
                         EmployeeVisits ON ScheduleMasters.ScheduleID = EmployeeVisits.ScheduleID   INNER JOIN
						 Employees on employees.EmployeeID=ScheduleMasters.EmployeeID inner join
                         Payors ON ScheduleMasters.PayorID = Payors.PayorID INNER JOIN
                         DDMaster ON ScheduleMasters.CareTypeId = DDMaster.DDMasterID
						 where isPcaCompleted=1-- and datediff(mi,EmployeeVisits.ClockInTime,EmployeeVisits.ClockOutTime)>1