CREATE PROCEDURE [dbo].[HC_PrivateDuty_GetAssignedEmployees]  
 @ReferralTimeSlotDetailID BIGINT,        
 @ReferralTimeSlotMasterID BIGINT,    
 @Day INT,      
 @StartDate DATE,        
 @EndDate DATE      
AS                                          
BEGIN                              
    DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
SELECT DISTINCT E.EmployeeID,EmployeeName=dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat),E.LastName, E.MobileNumber    
--,SM.StartDate,SM.EndDate, StrDayName= DATENAME(dw,SM.StartDate)     
FROM ScheduleMasters SM    
INNER JOIN Employees E ON E.EmployeeID=SM.EmployeeID      
INNER JOIN ReferralTimeSlotDates RSD ON RSD.ReferralTSDateID=SM.ReferralTSDateID    
WHERE RSD.ReferralTimeSlotDetailID=@ReferralTimeSlotDetailID AND RSD.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID AND DayNumber=@Day    
AND RSD.ReferralTSDate BETWEEN @StartDate AND @EndDate AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted=0     
ORDER BY E.LastName ASC --, SM.StartDate ASC    
 --WHERE sm.ReferralTSDateID IN       
    
    
    
 --(SELECT ReferralTSDateID FROM ReferralTimeSlotDates       
 --WHERE ReferralTimeSlotDetailID=@ReferralTimeSlotDetailID AND ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID      
 --AND CONVERT(DATE,ReferralTSStartTime) BETWEEN @StartDate AND @EndDate OR CONVERT(DATE,ReferralTSEndTime) BETWEEN @StartDate AND @EndDate      
 --AND DayNumber=@Day)      
 --AND sm.IsDeleted=0 AND sm.ReferralID=@ReferralID    
END  