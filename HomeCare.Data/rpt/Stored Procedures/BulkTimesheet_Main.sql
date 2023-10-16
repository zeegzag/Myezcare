  
--   exec [rpt].[BulkTimesheet_Main] '60247,50251','DEVHomecare'  
CREATE PROCEDURE [rpt].[BulkTimesheet_Main]   
--@ReferralID BIGINT = NULL,  
--@EmployeeID BIGINT = NULL,  
--@StartDate DATE = NULL,  
--@EndDate DATE = NULL,      
@EmployeeVisitID varchar(MAX)='0',   
@dbname varchar(50) = NULL    
AS                                            
BEGIN            
       
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
DECLARE @DateFormat VARCHAR(MAX)= dbo.GetOrgDateTimeFormat()  
  DECLARE @OtherActivity VARCHAR(MAX);            
  DECLARE @OtherActivityTime BIGINT;            
            
  SELECT BeneficiaryName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),EmployeeName=dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat),e.EmployeeUniqueID,              
  r.AHCCCSID AS BeneficiaryID,FORMAT(sm.StartDate, @DateFormat) as Date,DATENAME(dw,sm.StartDate) as DayOfWeek,                
  PlaceOfService= CASE WHEN PlaceOfService IS NULL THEN 'NA' ELSE ev.PlaceOfService END,              
  HHA_PCA_Name= dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat),              
  HHA_PCA_NP= CASE WHEN HHA_PCA_NP IS NULL THEN 'NA' ELSE ev.HHA_PCA_NP END,              
  ev.PatientSignature, EmployeeSignature=es.SignaturePath,              
  CONVERT(varchar(15),  CAST(ev.ClockInTime AS TIME), 100) as ClockInTime,CONVERT(varchar(15),  CAST(ev.ClockOutTime AS TIME), 100) as ClockOutTime,ev.EmployeeVisitID,                 
  DATEDIFF(MINUTE,(CAST(ev.ClockInTime AS TIME)),(CAST(ev.ClockOutTime AS TIME)))  as TotalTime,           
  OtherActivity=@OtherActivity,OtherActivityTime=@OtherActivityTime                 
                  
  FROM EmployeeVisits ev                    
  INNER JOIN ScheduleMasters sm ON sm.ScheduleID=ev.ScheduleID                    
  INNER JOIN Referrals r ON sm.ReferralID=r.ReferralID                    
  INNER JOIN Employees e ON e.EmployeeID=sm.EmployeeID              
  LEFT JOIN EmployeeSignatures es ON es.EmployeeSignatureID=e.EmployeeSignatureID           
  WHERE   
  (@EmployeeVisitID = '0' OR  TRY_CAst(ev.EmployeeVisitID AS varchar(MAX)) in (select Item from dbo.SplitString(@EmployeeVisitID, ',')))  
 --(@EmployeeID = 0 OR e.EmployeeID = @EmployeeID)  
-- AND (@ReferralID = 0 OR r.ReferralID = @ReferralID)  
-- AND ((@StartDate IS NOT NULL AND @EndDate IS NULL AND CONVERT(DATE,SM.StartDate) >= @StartDate) OR                     
--(@EndDate IS NOT NULL AND @StartDate IS NULL AND CONVERT(DATE,SM.EndDate) <= @EndDate) OR                    
--(@StartDate IS NOT NULL AND @EndDate IS NOT NULL AND (CONVERT(DATE,SM.StartDate) >= @StartDate AND CONVERT(DATE,SM.EndDate) <= @EndDate)) OR                    
--(@StartDate IS NULL AND @EndDate IS NULL))   
           
END 