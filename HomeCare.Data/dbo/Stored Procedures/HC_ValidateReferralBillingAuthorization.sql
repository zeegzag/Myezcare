--EXEC [dbo].[HC_ValidateReferralBillingAuthorization]  40
CREATE PROCEDURE [dbo].[HC_ValidateReferralBillingAuthorization]   
(@ReferralID bigint,            
@AuthType nvarchar(20)='',
@scheduleIDs nvarchar(max) = ''
)
AS
BEGIN

	DECLARE @ReferalTbl TABLE (ReferralID BIGINT) 

	INSERT INTO @ReferalTbl(ReferralID)
	SELECT @ReferralID

	IF ISNULL(@scheduleIDs,'') != ''
	BEGIN
		INSERT INTO @ReferalTbl(ReferralID)
		SELECT DISTINCT ReferralID FROM ScheduleMasters WHERE ScheduleID IN (SELECT val FROM [dbo].[f_split](@scheduleIDs,',')) 
	END
	DELETE FROM @ReferalTbl WHERE ReferralID NOT IN
	(
	SELECT T.ReferralID FROM @ReferalTbl t
	JOIN Referrals R ON R.ReferralID = T.ReferralID
	WHERE ISNULL(R.IsBillable,0) = 1
	)


	IF EXISTS (
		SELECT * FROM @ReferalTbl WHERE ReferralID > 0 AND ReferralID NOT IN(
		SELECT RB.ReferralID  FROM ReferralBillingAuthorizations RB
		JOIN @ReferalTbl R ON R.ReferralID = RB.ReferralID
		WHERE 
		(ISNULL(@AuthType,'') = '' OR [Type] = @AuthType) AND  
		RB.IsDeleted = 0 AND RB.Enddate >= getdate())
		)
	BEGIN
		SELECT CAST(0 AS BIT) IsSuccess,'ER0001' ErrorCode, 'Not having valid Billing Authorizations.' [Message]
		RETURN;
	END
	SELECT CAST(1 AS BIT) IsSuccess,NULL ErrorCode, 'Valid Billing Authorizations.' [Message]
END
