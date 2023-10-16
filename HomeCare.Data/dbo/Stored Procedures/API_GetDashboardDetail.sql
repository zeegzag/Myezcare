--  EXEC API_GetDashboardDetail @ServerCurrentDate = N'2021-09-25', @EmployeeId = N'50'  
                                  
CREATE PROCEDURE [dbo].[API_GetDashboardDetail]              
 @ServerCurrentDate DATE,                                                                    
 @EmployeeId bigint                                                                    
AS                                                                    
BEGIN  
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
 -- SET NOCOUNT ON added to prevent extra result sets from                                                                      
 -- interfering with SELECT statements.              
     --select  @ServerCurrentDate = N'2021-09-25', @EmployeeId = N'50'         
 DECLARE @OrganizationTwilioNumber NVARCHAR(20)              
              
 SET NOCOUNT ON;                                                                      
              
 SELECT TOP 1 @OrganizationTwilioNumber=TwilioFromNo FROM OrganizationSettings              
              
    SELECT e.EmployeeID, EmployeeName=dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat),e.FirstName, e.LastName, e.Email, e.PhoneWork, e.PhoneHome, e.MobileNumber,e.IVRPin,              
 @OrganizationTwilioNumber AS OrganizationTwilioNumber              
 FROM dbo.Employees e WHERE e.EmployeeID=@EmployeeId                                                                      
                                            
                                            
  --Get Today's Visit                                                                    
 SELECT * FROM (        
 SELECT TOP 25        
 RankOrder=ROW_NUMBER() OVER (PARTITION BY SM.EmployeeTSDateID,SM.ReferralTSDateID ORDER BY SM.CreatedDate DESC),                            
 r.ReferralID,sm.ScheduleID,sm.EmployeeID,FullName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),r.FirstName,sm.StartDate,sm.EndDate,ev.ClockInTime,ev.ClockOutTime,              
 ImageURL=r.ProfileImagePath,rtd.IsDenied,ISNULL(sm.AnyTimeClockIn, 0) AnyTimeClockIn, dm.Title as CareType,sm.VisitType --CareType=dbo.Fn_GetCareTypes(r.CareTypeIds)                    
 --CASE WHEN UsedInScheduling=1 THEN 0 ELSE 1 END AS IsDenied                                      
 FROM dbo.ScheduleMasters sm                                                                      
 INNER JOIN dbo.Referrals r ON sm.ReferralID = r.ReferralID AND r.IsDeleted=0                      
 INNER JOIN dbo.Employees e ON sm.EmployeeID = e.EmployeeID                                        
 INNER JOIN ReferralTimeSlotDates rtd ON rtd.ReferralTSDateID=sm.ReferralTSDateID                                        
 LEFT JOIN EmployeeVisits ev ON ev.ScheduleID=sm.ScheduleID     
 INNER JOIN DDMaster dm on dm.DDMasterID=sm.CareTypeId    
 WHERE --sm.IsDeleted=0 AND                                  
 ISNULL(SM.OnHold, 0) = 0 AND ((sm.IsDeleted = 0 AND rtd.IsDenied=0) OR (sm.IsDeleted=1 AND rtd.IsDenied=1)) AND                                   
 sm.EmployeeID = @EmployeeId   
 AND @ServerCurrentDate BETWEEN CONVERT(DATE,sm.StartDate) AND CONVERT(DATE,sm.EndDate)  
 AND sm.ScheduleStatusID=2                                                             
 AND (ev.IsSigned=0 OR ev.IsSigned is null)  AND (ev.IsPCACompleted=0 OR ev.IsPCACompleted is null)                        
 ORDER BY sm.StartDate DESC        
  UNION         
  SELECT TOP 25        
 RankOrder=ROW_NUMBER() OVER (PARTITION BY sm.ScheduleID ORDER BY SM.CreatedDate DESC),                            
 r.ReferralID,sm.ScheduleID,sm.EmployeeID,FullName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),r.FirstName,sm.StartDate,sm.EndDate,ev.ClockInTime,ev.ClockOutTime,              
 ImageURL=r.ProfileImagePath, CAST(0 AS BIT) IsDenied,ISNULL(sm.AnyTimeClockIn, 0) AnyTimeClockIn, dm.Title as CareType,sm.VisitType --CareType=dbo.Fn_GetCareTypes(r.CareTypeIds)                   
 --CASE WHEN UsedInScheduling=1 THEN 0 ELSE 1 END AS IsDenied                                      
 FROM dbo.ScheduleMasters sm                                                                      
 INNER JOIN dbo.Referrals r ON sm.ReferralID = r.ReferralID AND r.IsDeleted=0                      
 INNER JOIN dbo.Employees e ON sm.EmployeeID = e.EmployeeID                                                                     
 LEFT JOIN EmployeeVisits ev ON ev.ScheduleID=sm.ScheduleID      
 INNER JOIN DDMaster dm on dm.DDMasterID=sm.CareTypeId    
 WHERE         
 ISNULL(SM.OnHold, 0) = 0 AND sm.IsDeleted=0 AND                                                             
 sm.EmployeeID = @EmployeeId         
 AND @ServerCurrentDate BETWEEN CONVERT(DATE,sm.StartDate) AND CONVERT(DATE,sm.EndDate)     
 AND sm.ScheduleStatusID=2                  
 AND Sm.EmployeeTSDateID IS NULL AND SM.ReferralTSDateID IS NULL        
 AND ISNULL(ev.IsSigned,0)=0        
 AND ISNULL(ev.IsPCACompleted,0)=0        
 ORDER BY sm.StartDate DESC        
 ) T WHERE T.RankOrder=1                          
                                                            
--Get Tomorrows's Visit                                          
 SELECT r.ReferralID,sm.ScheduleID,sm.EmployeeID,FullName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),r.FirstName,sm.StartDate, sm.EndDate,ev.ClockInTime,ev.ClockOutTime,                      
 ImageURL=r.ProfileImagePath,sm.AnyTimeClockIn,dm.Title as CareType,sm.VisitType--CareType=dbo.Fn_GetCareTypes(r.CareTypeIds)                                      
 FROM dbo.ScheduleMasters sm                                                                      
 INNER JOIN dbo.Referrals r ON sm.ReferralID = r.ReferralID AND r.IsDeleted=0                      
 INNER JOIN dbo.Employees e ON sm.EmployeeID = e.EmployeeID                                                                      
 LEFT JOIN EmployeeVisits ev ON ev.ScheduleID=sm.ScheduleID     
 INNER JOIN DDMaster dm on dm.DDMasterID=sm.CareTypeId      
 WHERE ISNULL(SM.OnHold, 0) = 0 AND sm.IsDeleted = 0 AND sm.EmployeeID = @EmployeeId                   
 AND CONVERT(DATE,sm.StartDate) <= DATEADD(day, 7,(convert(date, @ServerCurrentDate)))              
  AND CONVERT(DATE,sm.StartDate) >= DATEADD(day,1,(convert(date, @ServerCurrentDate)))     AND (ev.IsPCACompleted=0 OR ev.IsPCACompleted is null)                  
 --AND sm.ScheduleStatusID=2          --AND (ev.SurveyCompleted=0 OR ev.SurveyCompleted is null) AND (ev.IsSigned=0 OR ev.IsSigned is null)                      
 ORDER BY sm.StartDate asc              
END  