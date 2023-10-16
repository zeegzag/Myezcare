CREATE PROCEDURE [dbo].[API_PCASigned]          
 @EmployeeVisitID BIGINT,                      
 @ScheduleID BIGINT,    
 @SignedLat DECIMAL(10,7),    
 @SignedLong DECIMAL(10,7),    
 @SignedIPAddress VARCHAR(100)    
AS                                
BEGIN                    
     DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()          
  Update EmployeeVisits          
  SET IsSigned=1,IsPCACompleted=1,SignedLat=@SignedLat,SignedLong=@SignedLong,SignedIPAddress=@SignedIPAddress,    
  UpdatedDate=GETUTCDATE()    
  WHERE EmployeeVisitID=@EmployeeVisitID AND ScheduleID=@ScheduleID      
      
  SELECT evn.AlertComment,e.MobileNumber,EmployeeName=dbo.GetGenericNameFormat(es.FirstName,es.MiddleName, es.LastName,@NameFormat),PatientName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat)      
 FROM EmployeeVisitNotes evn      
 INNER JOIN ReferralTaskMappings rm ON rm.ReferralTaskMappingID=evn.ReferralTaskMappingID      
 INNER JOIN VisitTasks v ON v.VisitTaskID=rm.VisitTaskID AND v.SendAlert=1      
 INNER JOIN EmployeeVisits ev ON ev.EmployeeVisitID=evn.EmployeeVisitID      
 INNER JOIN ScheduleMasters s ON s.ScheduleID=ev.ScheduleID      
 INNER JOIN Referrals r ON r.ReferralID=s.ReferralID      
 INNER JOIN Employees es ON es.EmployeeID=s.EmployeeID      
 CROSS JOIN Employees e       
 WHERE e.RoleID=1 AND e.MobileNumber Is not null    and e.IsDeleted=0  
 AND evn.AlertComment is not null AND evn.Description like 'yes' AND evn.EmployeeVisitID=@EmployeeVisitID      
      
END  