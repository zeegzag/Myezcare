-- EXEC UpdateRespiteHours @ReferralID = '987', @NoteID = '54260'
CREATE PROCEDURE [dbo].[UpdateRespiteHours]
@NoteID	 bigint,
@ReferralID	 bigint
AS
BEGIN

DECLARE @ImportHours DECIMAL(18,2);
DECLARE @ServiceDate DATE;
DECLARE @ServiceCode VARCHAR(MAX);
DECLARE @StartDate DATE;
DECLARE @EndDate DATE;
DECLARE @CheckRespiteHours BIT;


--SELECT ServiceDate,ServiceCode,CheckRespiteHours FROM Notes WHERE ReferralID=@ReferralID AND NoteID=@NoteID
SELECT @ServiceDate=ServiceDate,@ServiceCode=ServiceCode,@CheckRespiteHours=CheckRespiteHours FROM Notes WHERE ReferralID=@ReferralID AND NoteID=@NoteID

IF(LEN(@ServiceCode)>0)
	BEGIN
  
	  IF(DATEPART(mm,@ServiceDate) >= 10)  -- 10 STAND FOR MOTH OCTOMBER
       BEGIN
			SET @StartDate=DATEFROMPARTS( DATEPART(yyyy,@ServiceDate), 10, 1)
	   END
	  ELSE
	   BEGIN
		    DECLARE @LastYear DATE;
			SET @LastYear=DATEADD(yyyy,-1,@ServiceDate)
			SET @StartDate=DATEFROMPARTS( DATEPART(yyyy,@LastYear), 10, 1)
	   END
 
	   SET @EndDate=DATEADD(yyyy,1,@StartDate);
	   SET @EndDate=DATEADD(day,-1,@EndDate);

	   IF(DATEPART(yyyy,@StartDate)=2016)
	    BEGIN
			SELECT @ImportHours=CONVERT(DECIMAL(18,2),ImportHours) FROM Referrals WHERE ReferralID=@ReferralID
		END





		DECLARE @Hrs decimal(14,2)
		SELECT @Hrs=SUM(HRS) FROM 
					(  SELECT HRS=
						(CASE WHEN ServiceCode='S5151' THEN 24 * COUNT(ServiceCode)
							  WHEN ServiceCode!='S5151' THEN CAST( (15 * SUM(CalculatedUnit)) / 60  as decimal(16,2) ) END)
							  --CAST(SUM(DATEDIFF(MINUTE,StartTime,EndTime))/60.0 as decimal(16,2)) END)
						From Notes where IsBillable=1 AND IsDeleted=0 
						AND ReferralID=@ReferralID AND CheckRespiteHours=1 AND (ServiceDate>=@StartDate AND ServiceDate<=@EndDate) 
						Group By ServiceCode--, CalculatedUnit
					) AS Tab1


		DECLARE @ReferralRespiteUsageLimitID BIGINT;
		SELECT TOP 1 @ReferralRespiteUsageLimitID=ReferralRespiteUsageLimitID FROM ReferralRespiteUsageLimit 
		WHERE ReferralID=@ReferralID and (CAST(@ServiceDate  AS DATE) >= StartDate and CAST(@ServiceDate  AS DATE) <= EndDate) ORDER BY ReferralRespiteUsageLimitID DESC 

		IF(ISNULL(@ReferralRespiteUsageLimitID,0)=0)
		BEGIN
		  
		  UPDATE ReferralRespiteUsageLimit SET IsActive=0 WHERE ReferralID=@ReferralID

		  INSERT INTO ReferralRespiteUsageLimit(StartDate,EndDate,ReferralID,IsActive,UsedRespiteHours)
		   VALUES(@StartDate,@EndDate,@ReferralID,1,0);

		  SET @ReferralRespiteUsageLimitID=@@IDENTITY;
		END


		

		UPDATE ReferralRespiteUsageLimit set UsedRespiteHours=ISNULL(@ImportHours,0) + ISNULL(@Hrs,0)
		WHERE ReferralRespiteUsageLimitID=@ReferralRespiteUsageLimitID AND ReferralID=@ReferralID --AND (CAST(GETDATE() AS DATE) >= StartDate and CAST(GETDATE() AS DATE) <= EndDate)
    

		--PRINT ('@ImportHours =' + CONVERT(VARCHAR(MAX), ISNULL(@ImportHours,0)));
		--PRINT ('@Hrs =' + CONVERT(VARCHAR(MAX), ISNULL(@Hrs,0)));
		--PRINT ('@ServiceDate =' + CONVERT(VARCHAR(MAX), @ServiceDate) );
		--PRINT ('@ServiceCode =' +  CONVERT(VARCHAR(MAX), @ServiceCode)) ;
		--PRINT ('@StartDate =' +  CONVERT(VARCHAR(MAX), @StartDate)) ;
		--PRINT ('@EndDate =' +  CONVERT(VARCHAR(MAX), @EndDate));
		--PRINT ('@CheckRespiteHours =' +  CONVERT(VARCHAR(MAX), @CheckRespiteHours));

	END
ELSE
 PRINT @CheckRespiteHours
	--PRINT ('ASD'+ @ServiceCode)


END

-- EXEC UpdateRespiteHours @ReferralID = '987', @NoteID = '54260'
-- EXEC UpdateRespiteHours @ReferralID = '2824', @NoteID = '119250'
-- EXEC UpdateRespiteHours @ReferralID = '2892', @NoteID = '59867'