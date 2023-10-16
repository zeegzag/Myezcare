CREATE PROCEDURE [dbo].[API_GetByPassDetail]    
 @ScheduleID BIGINT            
AS                                  
BEGIN      
  SELECT ev.EarlyClockOutComment AS AlertComment,EmployeeName = dbo.GetGeneralNameFormat(e.FirstName,e.LastName),PatientName = dbo.GetGeneralNameFormat(r.FirstName,r.LastName),e1.MobileNumber,ev.ByPassReasonClockIn,ev.ByPassReasonClockOut,e.FcmTokenId,e.DeviceType
 FROM EmployeeVisits ev      
 INNER JOIN ScheduleMasters sm ON sm.ScheduleID=ev.ScheduleID      
 INNER JOIN Employees e ON e.EmployeeID=sm.EmployeeID      
 INNER JOIN Referrals r ON r.ReferralID=sm.ReferralID      
 CROSS JOIN Employees e1 WHERE e1.RoleID=1 AND e1.MobileNumber IS NOT NULL AND e1.IsDeleted=0  
 AND ev.ScheduleID=@ScheduleID  
END
