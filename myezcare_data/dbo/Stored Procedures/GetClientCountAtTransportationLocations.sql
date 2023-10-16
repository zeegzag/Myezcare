--EXEC GetClientCountAtTransportationLocations @WeekMasterID = '10'
--EXEC GetClientCountAtTransportationLocations 11
CREATE PROCEDURE [dbo].[GetClientCountAtTransportationLocations]
@WeekMasterID bigint,
@RegionID bigint
AS
BEGIN
	 DECLARE @StratDate Datetime;
	 DECLARE @EndDate Datetime;
	 SELECT @EndDate=EndDate,@StratDate=StartDate FROM WeekMasters WHERE WeekMasterID=@WeekMasterID

	 print @EndDate
	 print @StratDate

	 Select Count(ReferralID) As ClientCount,Location,SchStartDate = (CONVERT(VARCHAR(10),CONVERT(datetime,StartDate ,1),101)) 
	 From ScheduleMasters SM
	 INNER JOIN TransportLocations TL ON TL.TransportLocationID=SM.DropOffLocation
	 LEFT JOIN Regions R ON R.RegionID=TL.RegionID
	 WHERE StartDate>=@StratDate AND StartDate<=@EndDate AND TL.RegionID=@RegionID AND SM.IsDeleted=0
	 GROUP BY Location,StartDate 
	 Order BY StartDate,Location

 	 Select Count(ReferralID) As ClientCount,Location,SchEndDate= (CONVERT(VARCHAR(10),CONVERT(datetime,EndDate ,1),101)) 
	 FROM ScheduleMasters SM
	 INNER JOIN TransportLocations TL ON TL.TransportLocationID=SM.PickUpLocation
	 LEFT JOIN Regions R ON R.RegionID=TL.RegionID
	 WHERE EndDate>=@StratDate AND EndDate<=@EndDate AND TL.RegionID=@RegionID AND SM.IsDeleted=0
	 GROUP BY Location,EndDate 
	 Order BY EndDate,Location


END
