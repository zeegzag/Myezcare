-- EXEC GetNotePagePlaceOfService @ReferralID = '2892', @ServiceDate = '2017/08/01', @ServiceCodeID = '1', @PayorServiceCodeMappingID = '10066', @PosID = '53', @NoteID = '119251'
CREATE PROCEDURE [dbo].[GetNotePagePlaceOfService]
@ReferralID bigint,
@ServiceDate date,
@ServiceCodeID int,
@PayorServiceCodeMappingID bigint=0,
@PosID BIGINT=0,
@NoteID bigint=0,
@IsDeleted bigint=0,
@PayorID bigint=0
AS
BEGIN
	DECLARE @TotalUsedUnit float
	DECLARE @TodayUsedUnit float
	-- SET @TotalUsedUnit= CASE WHEN (select SUM(CalculatedUnit) from Notes where ReferralID=@ReferralID) IS NULL THEN 0 ELSE (select SUM(CalculatedUnit) from Notes where ReferralID=@ReferralID) END
	-- SET @TodayUsedUnit= CASE WHEN (select SUM(CalculatedUnit) from Notes where ReferralID=@ReferralID and ServiceDate=@ServiceDate) IS NULL then 0 else (select SUM(CalculatedUnit) from Notes where ReferralID=@ReferralID and ServiceDate=@ServiceDate) END
	
	SELECT @TotalUsedUnit= SUM(CalculatedUnit) from Notes where ReferralID=@ReferralID AND ServiceCodeID=@ServiceCodeID and NoteID!=@NoteID AND IsDeleted=0;
	IF(@TotalUsedUnit IS NULL)SET @TotalUsedUnit=0;

	SELECT  @TodayUsedUnit=  SUM(CalculatedUnit) from Notes where ReferralID=@ReferralID AND ServiceDate=@ServiceDate AND ServiceCodeID=@ServiceCodeID and NoteID!=@NoteID  AND IsDeleted=0;
	IF(@TodayUsedUnit IS NULL)SET @TodayUsedUnit=0;
	
	
	SELECT psm.PayorServiceCodeMappingID,psm.PayorID,psm.ServiceCodeID,psm.PosID,psm.Rate,sc.UnitType,sc.ServiceCodeType,
	sc.PerUnitQuantity,pos.PosName,sc.CheckRespiteHours,sc.MaxUnit, sc.DailyUnitLimit, sc.DefaultUnitIgnoreCalculation,
	CASE WHEN @TotalUsedUnit is null then 0 else @TotalUsedUnit END  as TotalUsedUnit,
	CASE WHEN @TodayUsedUnit is null then 0 else @TodayUsedUnit END as TodayUsedUnit,
	
	CASE WHEN sc.MaxUnit=0 THEN 0 ELSE (sc.MaxUnit-@TotalUsedUnit) END as AvailableMaxUnit,
	CASE WHEN (sc.MaxUnit = 0 AND sc.DailyUnitLimit = 0)   THEN 0
		 WHEN (sc.MaxUnit = 0 AND sc.DailyUnitLimit > 0 )  THEN sc.DailyUnitLimit - @TodayUsedUnit
		 WHEN (sc.MaxUnit > 0 AND sc.DailyUnitLimit = 0 )  THEN sc.MaxUnit - @TotalUsedUnit -- @TodayUsedUnit
		 WHEN (sc.MaxUnit > 0 AND sc.DailyUnitLimit > 0 )  THEN 
	CASE WHEN sc.DailyUnitLimit > (sc.MaxUnit-@TotalUsedUnit) THEN sc.MaxUnit-@TotalUsedUnit ELSE sc.DailyUnitLimit -@TodayUsedUnit END
	END AS AvailableDailyUnit
	from PayorServiceCodeMapping psm
	inner join ServiceCodes sc on sc.ServiceCodeID=psm.ServiceCodeID
	inner join PlaceOfServices pos on pos.PosID=psm.PosID	
	Where 
	(   (@NoteID>0 AND psm.PayorID IN (@PayorID))  OR 
	(@NoteID=0 AND psm.PayorID IN (select PayorID from ReferralPayorMappings where ReferralID=@ReferralID and IsActive=1 and IsDeleted=0)    )  ) 
	 -- in (select PayorID from ReferralPayorMappings where ReferralID=@ReferralID and IsActive=1 and IsDeleted=0) 
	AND psm.ServiceCodeID=@ServiceCodeID and (@IsDeleted =-1 OR  psm.IsDeleted=@IsDeleted) 
	AND (@ServiceDate >= psm.POSStartDate and @ServiceDate<= psm.POSEndDate)
	AND (( CAST(@PayorServiceCodeMappingID AS BIGINT)=0) OR psm.PayorServiceCodeMappingID = CAST(@PayorServiceCodeMappingID AS BIGINT))
	AND (( CAST(@PosID AS BIGINT)=0) OR psm.PosID = CAST(@PosID AS BIGINT))	

END
