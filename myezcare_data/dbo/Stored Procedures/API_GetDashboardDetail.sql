
--EXEC API_GetDashboardDetail @ServerCurrentDate = N'2018-11-15', @EmployeeId = N'171'                                        
                                          
CREATE PROCEDURE [dbo].[API_GetDashboardDetail]  
 @ServerCurrentDate DATE,                                                        
 @EmployeeId bigint                                                        
AS                                                        
BEGIN                                                        
 -- SET NOCOUNT ON added to prevent extra result sets from                                                        
 -- interfering with SELECT statements.

 DECLARE @OrganizationTwilioNumber NVARCHAR(20)

 SET NOCOUNT ON;                                                        

 SELECT TOP 1 @OrganizationTwilioNumber=TwilioFromNo FROM OrganizationSettings

    SELECT e.EmployeeID, EmployeeName=dbo.GetGeneralNameFormat(e.FirstName,e.LastName),e.FirstName, e.LastName, e.Email, e.PhoneWork, e.PhoneHome, e.MobileNumber,e.IVRPin,
	@OrganizationTwilioNumber AS OrganizationTwilioNumber
	FROM dbo.Employees e WHERE e.EmployeeID=@EmployeeId                                                        
                              
                              
  --Get Today's Visit                                                      
 SELECT * FROM (SELECT TOP 10 RankOrder=ROW_NUMBER() OVER (PARTITION BY SM.EmployeeTSDateID,SM.ReferralTSDateID ORDER BY SM.CreatedDate DESC),              
 r.ReferralID,sm.ScheduleID,sm.EmployeeID,FullName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName),r.FirstName,sm.StartDate,sm.EndDate,          
 ImageURL=r.ProfileImagePath,rtd.IsDenied      
 --CASE WHEN UsedInScheduling=1 THEN 0 ELSE 1 END AS IsDenied                        
 FROM dbo.ScheduleMasters sm                                                        
 INNER JOIN dbo.Referrals r ON sm.ReferralID = r.ReferralID AND r.IsDeleted=0        
 INNER JOIN dbo.Employees e ON sm.EmployeeID = e.EmployeeID                          
 INNER JOIN ReferralTimeSlotDates rtd ON rtd.ReferralTSDateID=sm.ReferralTSDateID                          
 LEFT JOIN EmployeeVisits ev ON ev.ScheduleID=sm.ScheduleID                              
 WHERE --sm.IsDeleted=0 AND                    
 ((sm.IsDeleted = 0 AND rtd.IsDenied=0) OR (sm.IsDeleted=1 AND rtd.IsDenied=1)) AND                     
 sm.EmployeeID = @EmployeeId AND CONVERT(DATE,sm.StartDate)=@ServerCurrentDate AND sm.ScheduleStatusID=2                                               
 AND (ev.IsSigned=0 OR ev.IsSigned is null)              
 ORDER BY sm.StartDate DESC) T WHERE T.RankOrder=1            
                                              
--Get Tomorrows's Visit                            
 SELECT r.ReferralID,sm.ScheduleID,sm.EmployeeID,FullName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName),r.FirstName,sm.StartDate, sm.EndDate,          
 ImageURL=r.ProfileImagePath                       
 FROM dbo.ScheduleMasters sm                                                        
 INNER JOIN dbo.Referrals r ON sm.ReferralID = r.ReferralID AND r.IsDeleted=0        
 INNER JOIN dbo.Employees e ON sm.EmployeeID = e.EmployeeID                                                        
 LEFT JOIN EmployeeVisits ev ON ev.ScheduleID=sm.ScheduleID                              
 WHERE sm.IsDeleted = 0 AND sm.EmployeeID = @EmployeeId     
 AND CONVERT(DATE,sm.StartDate) = DATEADD(day, 1,(convert(date, @ServerCurrentDate)))     
 --AND sm.ScheduleStatusID=2          --AND (ev.SurveyCompleted=0 OR ev.SurveyCompleted is null) AND (ev.IsSigned=0 OR ev.IsSigned is null) AND (ev.IsPCACompleted=0 OR ev.IsPCACompleted is null)           
 ORDER BY sm.StartDate desc
END
