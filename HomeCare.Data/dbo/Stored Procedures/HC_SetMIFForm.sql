--EXEC HC_SetMIFForm @ReferralID = '1951', @TodayDate = '2018/11/22'
CREATE PROCEDURE [dbo].[HC_SetMIFForm]
@ReferralID BIGINT,
@TodayDate DATE
AS
BEGIN
	SELECT * FROM ReferralBillingAuthorizations WHERE Type='CMS1500' AND ReferralID=@ReferralID AND @TodayDate BETWEEN StartDate AND EndDate
END