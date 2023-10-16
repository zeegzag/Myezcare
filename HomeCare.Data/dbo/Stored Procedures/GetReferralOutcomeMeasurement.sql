CREATE PROCEDURE [dbo].[GetReferralOutcomeMeasurement]  
 @ReferralID bigint,
 @StartDate Date =null,
 @EndDate DAte=null
AS  
BEGIN  
 
 	SELECT * FROM ReferralOutcomeMeasurements ROM
 	WHERE
 		(ROM.ReferralID=@ReferralID) AND IsDeleted=0 AND
 		((@StartDate is null) OR ( ROM.OutcomeMeasurementDate >=@StartDate)) AND
 		((@EndDate is null) OR ( ROM.OutcomeMeasurementDate <=@EndDate))
		ORDER BY ROM.OutcomeMeasurementDate ASC
END