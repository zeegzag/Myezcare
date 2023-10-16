-- EXEc [rpt].[GetEmployeeVisitsHours] NULL,NULL,0,41,0      
-- =============================================      
CREATE PROC [rpt].[GetEmployeeVisitsHours]      
 @FromDate DATE = NULL,      
 @ToDate DATE = NULL,      
 @ReferralID VARCHAR(MAX)='0',    
 @EmployeeID VARCHAR(MAX)='0',      
 @CareType VARCHAR(MAX)='0',      
 @Payor VARCHAR(MAX)='0'      
AS  
BEGIN      
 SELECT EmpID,EmployeeUniqueID,EmployeeName,COUNT(ScheduleID) AS Visits,COUNT(EmployeeVisitID) AS EmployeeVisits,      
 ServiceName,ServiceCode,FORMAT(TRY_CAST(ScheduleDate AS DATE),'MM/dd/yyyy') AS ScheduleDate,ReferralID,AccountNumber,PatientName,ClockInTime,ClockOutTime,      
 TravelTime,ScheduleID,EmployeeVisitID,Hours,BillHours,Diff,HasVisit,Amount,ROUND(DISTANCE,2) AS DISTANCE,      
 Supplies,CareType,InsCompany      
FROM      
(      
 select       
	sm.ScheduleID,
  e.EmployeeID AS EmpID,  
  e.EmployeeUniqueID AS EmployeeUniqueID,  
  EmployeeName=dbo.GetGeneralNameFormat(e.FirstName,e.LastName),      
  d.Title As ServiceName,      
  SC.ServiceCode,      
  sm.StartDate as ScheduleDate,      
  r.ReferralID AS ReferralID,   
  r.AHCCCSID as AccountNumber,  
  dbo.GetGeneralNameFormat(r.FirstName,r.LastName) AS PatientName,      
  CONVERT(varchar(15),  CAST(ev.ClockInTime AS TIME), 100) as ClockInTime,      
  CONVERT(varchar(15),  CAST(ev.ClockOutTime AS TIME), 100) as ClockOutTime,      
  0 AS TravelTime,      
  ev.EmployeeVisitID,  
  CAST(CASE WHEN ev.EmployeeVisitID IS NULL THEN 0 ELSE 1 END AS BIT) HasVisit,  
  DT.Hours,      
  DT.BillHours,      
  DT.Hours-DT.BillHours Diff,
  0 AS Amount,      
  (([dbo].GetGeoFromLatLng(E.Latitude,E.Longitude)).STDistance([dbo].GetGeoFromLatLng(C.Latitude,C.Longitude)) * 0.000621371) AS DISTANCE,      
  0 AS Supplies,      
  ISNULL(d.Title, 'NA') AS CareType, ISNULL(p.PayorName, 'NA') as InsCompany       
 from ScheduleMasters sm WITH(NOLOCK) 
 LEFT join EmployeeVisits ev WITH(NOLOCK) on sm.ScheduleID=ev.ScheduleID      
 inner join Employees e WITH(NOLOCK) on e.EmployeeID=sm.EmployeeID      
 inner join Referrals r WITH(NOLOCK) on r.ReferralID=sm.ReferralID      
 left join Payors p WITH(NOLOCK) on p.PayorID=sm.PayorID       
 LEFT JOIN ContactMappings CM WITH(NOLOCK) ON CM.ReferralID = r.ReferralID AND CM.ContactTypeID=1       
 LEFT JOIN Contacts C WITH(NOLOCK) ON C.ContactID= CM.ContactID       
 inner JOIN DDMaster d WITH(NOLOCK) on d.DDMasterID=sm.CareTypeId       
 inner join ReferralBillingAuthorizations RBA WITH(NOLOCK)  on RBA.ReferralBillingAuthorizationID = SM.ReferralBillingAuthorizationID      
 left JOIN ServiceCodes SC WITH(NOLOCK) on SC.ServiceCodeID=RBA.ServiceCodeID
 OUTER APPLY (
 SELECT 
	 DATEDIFF(minute,sm.StartDate,sm.EndDate) as Hours,      
	 ISNULL(DATEDIFF(minute,ev.ClockInTime,ev.ClockOutTime), 0) AS BillHours
 ) DT
 WHERE  
 sm.IsDeleted = 0
 AND
 ((@FromDate is null or @ToDate is null) or  ISNULL(ev.ClockInTime, sm.StartDate) BETWEEN @FromDate AND @Todate)      
  AND       
 (@ReferralID = '0' OR  TRY_CAst(R.ReferralID AS varchar(100)) in (select Item from dbo.SplitString(@ReferralID, ',')))      
 AND       
 (@EmployeeID = '0' OR  TRY_CAst(E.EmployeeID AS varchar(100)) in (select Item from dbo.SplitString(@EmployeeID, ',')))      
  AND       
 (@CareType = '0' OR  TRY_CAst(sm.CareTypeId AS varchar(100)) in (select Item from dbo.SplitString(@CareType, ',')))      
 AND       
 (@Payor = '0' OR  TRY_CAst(p.PayorID AS varchar(100)) in (select Item from dbo.SplitString(@Payor, ',')))      
) T      
GROUP BY EmpID,EmployeeUniqueID,EmployeeName,      
 ServiceName,ServiceCode,ScheduleDate,ReferralID,AccountNumber,PatientName,ClockInTime,ClockOutTime,      
 TravelTime,ScheduleID,EmployeeVisitID,Hours,BillHours,Diff,HasVisit,Amount,DISTANCE,      
 Supplies,CareType,InsCompany      
END 