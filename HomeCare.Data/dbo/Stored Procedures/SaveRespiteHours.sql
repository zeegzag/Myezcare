-- EXEC SaveRespiteHours @StartDate = '2016/10/01', @EndDate = '2017/09/30', @ReferralID = '2290'
CREATE PROCEDURE [dbo].[SaveRespiteHours]
@IsAddHrsCall BIT,
@NoteID	 bigint,
@ReferralID	 bigint,
@StartDate DATE,
@EndDate DATE
AS
BEGIN
	DECLARE @Hrs decimal(14,2)
	DECLARE @IsDeleted decimal(14,2)

	SELECT @IsDeleted=IsDeleted FROM Notes Where NoteID=@NoteID

	SELECT @Hrs=SUM(HRS) FROM 
				(  SELECT HRS=
					(CASE WHEN ServiceCode='S5151' THEN 24 * COUNT(ServiceCode)
						  WHEN ServiceCode!='S5151' THEN CAST( (15 * SUM(CalculatedUnit)) / 60  as decimal(16,2) ) END)
						  --CAST(SUM(DATEDIFF(MINUTE,StartTime,EndTime))/60.0 as decimal(16,2)) END)
					From Notes where IsBillable=1 AND NoteID=@NoteID --AND IsDeleted=0 
					AND ReferralID=@ReferralID AND CheckRespiteHours=1 AND (ServiceDate>=@StartDate AND ServiceDate<=@EndDate) 
					Group By ServiceCode--, CalculatedUnit
				) AS Tab1

	DECLARE @ReferralRespiteUsageLimitID BIGINT;
	SELECT @ReferralRespiteUsageLimitID=ReferralRespiteUsageLimitID FROM ReferralRespiteUsageLimit WHERE IsActive=1 and ReferralID=@ReferralID and (CAST(GETDATE() AS DATE) >= StartDate and CAST(GETDATE() AS DATE) <= EndDate)

	IF(ISNULL(@ReferralRespiteUsageLimitID,0)=0)
	BEGIN
	  INSERT INTO ReferralRespiteUsageLimit(StartDate,EndDate,ReferralID,IsActive,UsedRespiteHours)
	  VALUES(@StartDate,@EndDate,@ReferralID,1,0);
	END

	--PRINT ISNULL(@Hrs,0)
	IF(@IsAddHrsCall=1)
		UPDATE ReferralRespiteUsageLimit set UsedRespiteHours=UsedRespiteHours + ISNULL(@Hrs,0) WHERE IsActive=1 AND ReferralID=@ReferralID AND
		(CAST(GETDATE() AS DATE) >= StartDate and CAST(GETDATE() AS DATE) <= EndDate)
    ELSE
	    UPDATE ReferralRespiteUsageLimit SET
		UsedRespiteHours=  CASE WHEN (UsedRespiteHours IS NULL OR UsedRespiteHours= 0 OR UsedRespiteHours < @Hrs) THEN 0 ELSE  UsedRespiteHours - ISNULL(@Hrs,0)  END 
		WHERE IsActive=1 AND ReferralID=@ReferralID AND
		(CAST(GETDATE() AS DATE) >= StartDate and CAST(GETDATE() AS DATE) <= EndDate)


END