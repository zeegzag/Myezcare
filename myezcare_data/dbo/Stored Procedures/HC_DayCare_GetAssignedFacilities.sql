--EXEC GetAssignedEmployees @ReferralTimeSlotDetailID = '30052', @ReferralTimeSlotMasterID = '1966', @StartDate = '5/21/2018 12:00:00 AM', @EndDate = '5/21/2019 12:00:00 AM'  
 CREATE PROCEDURE [dbo].[HC_DayCare_GetAssignedFacilities]
 @ReferralTimeSlotDetailID BIGINT,    
 @ReferralTimeSlotMasterID BIGINT,
 @Day INT,  
 @StartDate DATE,    
 @EndDate DATE  
AS                                      
BEGIN                          

SELECT DISTINCT F.FacilityID,F.FacilityName,FacilityNPI=F.NPI
FROM ScheduleMasters SM
INNER JOIN Facilities F ON F.FacilityID=SM.FacilityID 
INNER JOIN ReferralTimeSlotDates RSD ON RSD.ReferralTSDateID=SM.ReferralTSDateID
WHERE RSD.ReferralTimeSlotDetailID=@ReferralTimeSlotDetailID AND RSD.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID AND DayNumber=@Day
AND RSD.ReferralTSDate BETWEEN @StartDate AND @EndDate AND SM.IsDeleted=0 
ORDER BY F.FacilityName ASC --, SM.StartDate ASC
 --WHERE sm.ReferralTSDateID IN   


END
