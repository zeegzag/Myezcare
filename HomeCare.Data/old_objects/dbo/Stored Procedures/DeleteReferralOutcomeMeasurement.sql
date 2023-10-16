CREATE PROCEDURE [dbo].[DeleteReferralOutcomeMeasurement]  
 @ReferralOutcomeMeasurementID bigint,
 @ReferralID bigint,
 @StartDate Date =null,
 @EndDate DAte=null
AS  
BEGIN  
	UPDATE ReferralOutcomeMeasurements SET IsDeleted=1 WHERE ReferralOutcomeMeasurementID=@ReferralOutcomeMeasurementID
	EXEC [GetReferralOutcomeMeasurement]  @ReferralID,@StartDate,@EndDate
END
