CREATE PROCEDURE [dbo].[GetAutoCreateServiceInformation]
@ReferralID bigint,
@ServiceDate date,
@ServiceCodeID int,
@PosID int,
@PayorServiceCodeMappingID bigint=0,
@NoteID bigint=0
AS
BEGIN
 
   
	SELECT psm.PayorServiceCodeMappingID,psm.PayorID,psm.ServiceCodeID,psm.PosID,psm.Rate,sc.UnitType,sc.ServiceCodeType,sc.ServiceCode,sc.Description,
	sc.PerUnitQuantity,pos.PosName,sc.CheckRespiteHours,sc.MaxUnit, sc.DailyUnitLimit
	from PayorServiceCodeMapping psm
	inner join ServiceCodes sc on sc.ServiceCodeID=psm.ServiceCodeID
	inner join PlaceOfServices pos on pos.PosID=psm.PosID	
	Where 
	psm.PayorID in (select PayorID from ReferralPayorMappings where ReferralID=@ReferralID and IsActive=1 and IsDeleted=0) 
	AND psm.ServiceCodeID IN (Select ServiceCodeID from ServiceCodes Where RandomGroupID=(Select RandomGroupID from ServiceCodes Where ServiceCodeID=@ServiceCodeID AND RandomGroupID IS NOT NULL) AND  ServiceCodeID!=@ServiceCodeID )
	and psm.PosID=@PosID and psm.IsDeleted=0 
	AND (@ServiceDate >= psm.POSStartDate and @ServiceDate<= psm.POSEndDate)
	AND (( CAST(@PayorServiceCodeMappingID AS BIGINT)=0) OR psm.PayorServiceCodeMappingID = CAST(@PayorServiceCodeMappingID AS BIGINT));

END


--select * from PayorServiceCodeMapping

