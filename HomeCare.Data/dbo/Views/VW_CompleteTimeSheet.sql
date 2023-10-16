

  CREATE View [dbo].[VW_CompleteTimeSheet]
  AS
  select EmployeeName=dbo.GetGeneralNameFormat(e.FirstName,e.LastName)                                      
 ,PatientName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName)
 ,CONVERT(TIME(3),ev.ClockInTime) AS ClockInTime,CONVERT(TIME(3),ev.ClockOutTime) AS ClockOutTime,CONVERT(TIME(3),(ev.ClockOutTime-ev.ClockInTime)) as TotalHours 
 ,sm.StartDate,sm.EndDate,ev.IsPCACompleted      
 ,p.PayorName,d.Title As CareType                          
   FROM  EmployeeVisits ev                                                                   
   Inner JOIN ScheduleMasters sm on sm.ScheduleID=ev.ScheduleID                                                 
   LEFT JOIN Employees e on e.EmployeeID=sm.EmployeeID                                                 
   INNER JOIN Referrals r on r.ReferralID=sm.ReferralID                 
   LEFT JOIN Payors p on p.PayorID=sm.PayorID        
   LEFT JOIN DDMaster d on d.DDMasterID=sm.CareTypeId