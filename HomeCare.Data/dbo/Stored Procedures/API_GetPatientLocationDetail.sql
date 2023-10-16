--Exec [API_GetPatientLocationDetail] @ScheduleID=57739        
CREATE PROCEDURE [dbo].[API_GetPatientLocationDetail]      
 @ScheduleID BIGINT          
AS                              
BEGIN                  
  SELECT c.Latitude,c.Longitude,ScheduleCode=sm.ScheduleID FROM ScheduleMasters sm        
  INNER JOIN Employees e ON e.EmployeeID=sm.EmployeeID        
  INNER JOIN ContactMappings cm ON cm.ReferralID=sm.ReferralID AND cm.ContactTypeID=1        
  LEFT JOIN Contacts c ON c.ContactID=cm.ContactID        
  WHERE sm.ScheduleID=@ScheduleID                  
END