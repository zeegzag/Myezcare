-- EXEC [GetScheduleEmailDetail] 57737
CREATE PROCEDURE  [dbo].[HC_GetScheduleEmailDetail]           
@ScheduleID bigint        
AS          
BEGIN                
  SELECT sm.ScheduleID,PatientName=r.LastName+', '+r.FirstName,EmployeeName=e.FirstName+' '+e.LastName,sm.StartDate,c.Phone1 AS PatientPhone,c.Email AS PatientEmail,PatientAddress=c.Address+', '+c.City+', '+c.ZipCode,e.Email AS EmployeeEmail, e.PhoneWork 
AS EmployeePhone
FROM ScheduleMasters sm
INNER JOIN Referrals r ON r.ReferralID=sm.ReferralID
INNER JOIN Employees e ON e.EmployeeID=sm.EmployeeID
INNER JOIN ContactMappings cm ON cm.ReferralID=r.ReferralID AND cm.ContactTypeID=1
INNER JOIN Contacts c ON c.ContactID=cm.ContactID
INNER JOIN States s ON s.StateCode=c.State
 WHERE              
 ISNULL(SM.OnHold, 0) = 0 AND sm.IsDeleted =0 and sm.ScheduleID=@ScheduleID        
END