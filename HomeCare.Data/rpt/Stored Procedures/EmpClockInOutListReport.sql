                
                  
--  EXEC [rpt].[EmpClockInOutListReport] @StartDate = '2021/04/26', @EndDate = '2021/06/18', @EmployeeName = '', @CareTypeID=0, @Status = '', @ScheduleID='96830', @dbname = 'DEVHomecare'                   
CREATE procedure [rpt].[EmpClockInOutListReport]  
--DECLARE  
@StartDate DATE=NULL,                                              
@EndDate DATE=NULL,                             
@EmployeeName VARCHAR(200)=NULL,                  
@CareTypeID BIGINT = 0,                   
@Status varchar(50) = NULL,                   
@ScheduleID varchar(MAX) = '0',                   
@dbname varchar(50) = NULL,                
@RegionID Nvarchar(max) = NULL,  
@TimeSlots Nvarchar(500) =NULL  
  as                                   
--select  @StartDate = '2023-04-08', @EndDate = '2023-04-08', @EmployeeName = '', @CareTypeID=0, @Status = '', @ScheduleID='0', @TimeSlots = '', @dbname = 'Live_Emoha_05042023'    
BEGIN  
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
SELECT                    
  t.ScheduleID, t.EmployeeName, t.ReferralName, t.CareType, t.ClockIn, t.ClockOut,                  
  CONCAT(t.StartDate,' ',t.StartTime) AS ScheduleStartTime,                  
  CONCAT(t.EndDate,' ',t.EndTime) AS ScheduleEndTime,                  
  t.IsPCACompleted, t.Address, t.lat, t.lng, t.Phone, t.StartDate,                  
  t.EndDate, t.StartTime, t.EndTime, t.MobileNumber, Status,RegionID,RegionName                  
FROM                  
(                  
SELECT DISTINCT SM.ScheduleID, dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS EmployeeName,                  
dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) AS ReferralName,dm.Title As CareType,                  
ClockIn=CASE WHEN EV.ClockInTime IS NULL THEN 'No' ELSE 'Yes' END,                  
ClockOut=CASE WHEN EV.ClockOutTime IS NULL THEN 'No' ELSE 'Yes' END,EV.IsPCACompleted,                                              
CONCAT(c.Address,c.ApartmentNo,c.City,c.ZipCode) As Address,c.Latitude as lat,c.Longitude as lng,c.Phone1 as Phone,                  
--FORMAT(sm.StartDate, Admin_Myezcare_Live.dbo.fn_getDateFormat(@dbname)) as StartDate,                  
--FORMAT(sm.EndDate, Admin_Myezcare_Live.dbo.fn_getDateFormat(@dbname)) as EndDate,               
(sm.StartDate) as StartDate,                  
(sm.EndDate) as EndDate,            
FORMAT(sm.StartDate,'hh:mm tt') as StartTime,FORMAT(sm.EndDate,'hh:mm tt') as EndTime, ISNULL(E.MobileNumber, 'NA') AS MobileNumber,                  
case when EV.ClockInTime IS NOT NULL AND EV.ClockOutTime IS NULL then 'Inprogress' when EV.EmployeeVisitid IS NULL AND SM.ScheduleID IS NOT NULL then 'Missed'                        
when EV.IsPCACompleted=1 then 'Complete' ELSE 'OnHold' end as Status,r.RegionID,rg.RegionName ,  
(case when convert(char(10), SM.startdate, 108) >= '00:00:00'  and convert(char(10), SM.startdate, 108) < '12:00:00' then 'Morning'                
      when convert(char(10), SM.startdate, 108) >= '12:00:01'  and convert(char(10), SM.startdate, 108) < '17:00:00' then 'Afternoon'                
      when convert(char(10), SM.startdate, 108) >= '17:00:01'  and convert(char(10), SM.startdate, 108) < '18:59:00' then 'Evening'                
       else 'Night' end) as Timing   
FROM ScheduleMasters SM                                              
INNER JOIN Referrals R ON R.ReferralID=SM.ReferralID                                              
INNER JOIN Employees E ON E.EmployeeID=SM.EmployeeID                                    
--INNER JOIN ReferralTimeSlotDates RTSD ON RTSD.ReferralTSDateID=SM.ReferralTSDateID AND RTSD.IsDenied=0                                    
Left JOIN EmployeeVisits EV ON EV.ScheduleID=SM.ScheduleID AND EV.IsDeleted=0                        
inner join ContactMappings cm on cm.referralid=R.referralid AND cm.IsEmergencyContact=0                       
inner join Contacts c on c.ContactID=cm.ContactID AND c.IsDeleted=0                     
LEFT JOIN DDMaster dm on dm.DDMasterID=sm.CareTypeId                 
left join Regions rg on rg.RegionID=r.RegionID                
WHERE SM.IsDeleted=0 AND R.ReferralStatusID=1                     
AND ((@CareTypeID=0 OR LEN(@CareTypeID)=0) OR (dm.DDMasterID in (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@CareTypeID))))                
AND ((@RegionID is null or @RegionID='' ) or (R.RegionID IN (SELECT VAL FROM GetCSVTable(@RegionID))))                
AND (@ScheduleID = '0' OR  TRY_CAst(sm.ScheduleID AS varchar(100)) in (select Item from dbo.SplitString(@ScheduleID, ',')))                  
 AND ((@StartDate IS NOT NULL AND @EndDate IS NULL AND CONVERT(DATE, SM.StartDate) = @StartDate)              
OR  (@EndDate IS NOT NULL AND  @StartDate IS NULL AND   CONVERT(DATE, SM.EndDate) = @EndDate)              
OR  (@StartDate IS NOT NULL AND @EndDate IS NOT NULL  AND  ((CONVERT(DATE, SM.StartDate) >= @StartDate  AND  CONVERT(DATE, SM.EndDate)   <= @EndDate) OR CONVERT(DATE, SM.StartDate) = @EndDate))              
OR  (@StartDate IS NULL AND   @EndDate IS NULL))      
  AND                                           
   ((@EmployeeName IS NULL OR LEN(e.LastName)=0)                                             
   OR (                                            
       (E.FirstName LIKE '%'+@EmployeeName+'%' )OR                                 
    (E.LastName  LIKE '%'+@EmployeeName+'%') OR                                              
    (E.FirstName +' '+E.LastName like '%'+@EmployeeName+'%') OR                                              
    (E.LastName +' '+E.FirstName like '%'+@EmployeeName+'%') OR                                              
    (E.FirstName +', '+E.LastName like '%'+@EmployeeName+'%') OR                                              
    (E.LastName +', '+E.FirstName like '%'+@EmployeeName+'%')))                   
) t  WHERE ((@Status is null or @Status='' ) or (t.Status = @Status))  
AND ((@TimeSlots is null or @TimeSlots='' ) or (t.Timing IN (SELECT VAL FROM GetCSVTable(@TimeSlots))))               
             
END 