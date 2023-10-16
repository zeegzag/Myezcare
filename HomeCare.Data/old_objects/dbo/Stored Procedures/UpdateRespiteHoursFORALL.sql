-- EXEC UpdateRespiteHoursFORALL @ServiceDate = '2016-01-1'
CREATE PROCEDURE [dbo].[UpdateRespiteHoursFORALL]
@ServiceDate DATE
AS
BEGIN

			-- EXECUTE CURSOR TO RUN AND UPDATE LAST ATTANDACE DATE
				DECLARE @TempReferralID BIGINT
				DECLARE CUR CURSOR FOR SELECT ReferralID FROM Referrals
				OPEN CUR
				FETCH NEXT FROM CUR INTO @TempReferralID
				WHILE @@FETCH_STATUS = 0 BEGIN




							DECLARE @ImportHours DECIMAL(18,2);
							DECLARE @ServiceCode VARCHAR(MAX);
							DECLARE @StartDate DATE;
							DECLARE @EndDate DATE;



							IF(LEN(@ServiceDate)>0)
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
										SELECT @ImportHours=CONVERT(DECIMAL(18,2),ImportHours) FROM Referrals WHERE ReferralID=@TempReferralID
									END





									DECLARE @Hrs DECIMAL(18,2)
									SELECT @Hrs=SUM(HRS) FROM 
												(  SELECT HRS=
													(CASE WHEN ServiceCode='S5151' THEN 24 * COUNT(ServiceCode)
														  WHEN ServiceCode!='S5151' THEN CAST( (15 * SUM(CalculatedUnit)) / 60  as decimal(16,2) ) END)
														  --CAST(SUM(DATEDIFF(MINUTE,StartTime,EndTime))/60.0 as decimal(16,2)) END)
													From Notes where IsBillable=1 AND IsDeleted=0 
													AND ReferralID=@TempReferralID AND CheckRespiteHours=1 AND (ServiceDate>=@StartDate AND ServiceDate<=@EndDate) 
													Group By ServiceCode--, CalculatedUnit
												) AS Tab1


									DECLARE @ReferralRespiteUsageLimitID BIGINT;
									SELECT @ReferralRespiteUsageLimitID=ReferralRespiteUsageLimitID FROM ReferralRespiteUsageLimit WHERE IsActive=1 and ReferralID=@TempReferralID and (CAST(GETDATE() AS DATE) >= StartDate and CAST(GETDATE() AS DATE) <= EndDate)

									IF(ISNULL(@ReferralRespiteUsageLimitID,0)=0)
									BEGIN
									  INSERT INTO ReferralRespiteUsageLimit(StartDate,EndDate,ReferralID,IsActive,UsedRespiteHours) 	  VALUES(@StartDate,@EndDate,@TempReferralID,1,0);
									END


		

									UPDATE ReferralRespiteUsageLimit set UsedRespiteHours=ISNULL(@ImportHours,0) + ISNULL(@Hrs,0)
									WHERE IsActive=1 AND ReferralID=@TempReferralID AND (CAST(GETDATE() AS DATE) >= StartDate and CAST(GETDATE() AS DATE) <= EndDate)
									

									--PRINT ('============================================================');
									--PRINT ('@TempReferralID =' + CONVERT(VARCHAR(MAX), @TempReferralID));
									--PRINT ('@ImportHours =' + CONVERT(VARCHAR(MAX), ISNULL(@ImportHours,0)));
									--PRINT ('@Hrs =' + CONVERT(VARCHAR(MAX), ISNULL(@Hrs,0)));
									--PRINT ('@ServiceDate =' + CONVERT(VARCHAR(MAX), @ServiceDate) );
									--PRINT ('@StartDate =' +  CONVERT(VARCHAR(MAX), @StartDate)) ;
									--PRINT ('@EndDate =' +  CONVERT(VARCHAR(MAX), @EndDate));


									--DECLARE @Name VARCHAR(MAX);
									--DECLARE @AHCCCSID VARCHAR(MAX);
									--SELECT @Name=LastName+' '+FirstName,@AHCCCSID=AHCCCSID FROM Referrals WHERE ReferralID=@TempReferralID

									--SELECT Name=@Name, AHCCCSID=@AHCCCSID, ImportHours=ISNULL(@ImportHours,0), Used=ISNULL(@ImportHours,0) + ISNULL(@Hrs,0),
									--Remain=600 - (ISNULL(@ImportHours,0) + ISNULL(@Hrs,0))


								END



                FETCH NEXT FROM CUR INTO @TempReferralID 

					
				END
				CLOSE CUR    
				DEALLOCATE CUR
			 -- CLOSE CURSOR TO RUN AND UPDATE LAST ATTANDACE DATE

END

-- EXEC UpdateRespiteHoursFORALL @ServiceDate = '2017-01-1'



--SELECT * FROM Referrals WHERE ReferralID=1
--SELECT CONVERT(DECIMAL(18,2),ImportHours) FROM Referrals WHERE ReferralID=1





