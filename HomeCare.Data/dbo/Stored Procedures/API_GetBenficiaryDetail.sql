CREATE PROCEDURE [dbo].[API_GetBenficiaryDetail]    
 @ScheduleID BIGINT                
AS                                    
BEGIN  
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
  SELECT PatientName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),HHA_PCA_Name=dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat),sm.StartDate,DATENAME(dw,sm.StartDate) as DayOfWeek,ev.EmployeeVisitID,          
  sm.StartDate as ScheduleDate,    
  CASE WHEN ev.PlaceOfService IS NOT NULL THEN ev.PlaceOfService ELSE c.Address+','+c.City+','+s.StateName+','+c.ZipCode END AS PlaceOfService,    
  CASE WHEN ev.HHA_PCA_NP IS NOT NULL THEN ev.HHA_PCA_NP ELSE e.HHA_NPI_ID END AS HHA_PCA_NP,    
  CASE WHEN ev.BeneficiaryID IS NOT NULL THEN ev.BeneficiaryID ELSE r.AHCCCSID END AS AHCCCSID,    
  CONVERT(varchar(15),  CAST(ev.ClockInTime AS TIME), 100) as ClockInTime,CONVERT(varchar(15),  CAST(ev.ClockOutTime AS TIME), 100) as ClockOutTime,        
  DATEDIFF(MINUTE, ev.ClockInTime,ev.ClockOutTime) as TotalTime        
  FROM EmployeeVisits ev            
  INNER JOIN ScheduleMasters sm ON sm.ScheduleID=ev.ScheduleID            
  INNER JOIN Referrals r ON r.ReferralID=sm.ReferralID    
  INNER JOIN ContactMappings cm ON cm.ReferralID=r.ReferralID AND cm.ContactTypeID=1    
  INNER JOIN Contacts c ON c.ContactID=cm.ContactID    
  INNER JOIN States s ON c.State=s.StateCode    
  INNER JOIN Employees e ON e.EmployeeID=sm.EmployeeID    
  WHERE ev.ScheduleID=@ScheduleID            
END  