CREATE PROCEDURE [dbo].[GetVisitReasonModalDetail]
  @ScheduleID bigint,
  @HHAXAggregator NVARCHAR(MAX),
  @CareBridgeAggregator NVARCHAR(MAX),
  @TellusAggregator NVARCHAR(MAX),
  @SandataAggregator NVARCHAR(MAX)
AS
BEGIN
	SELECT
		P.ClaimProcessor,
		CASE WHEN ISNULL(P.ClaimProcessor,'') NOT IN (@HHAXAggregator, @TellusAggregator) THEN 1 ELSE 0 END ByPassModal,
		0 HideReasonType,
		CASE WHEN P.ClaimProcessor IN (@TellusAggregator) THEN 1 ELSE 0 END HideActionType
	FROM ScheduleMasters SM
	LEFT JOIN ReferralBillingAuthorizations RBA
		ON RBA.ReferralBillingAuthorizationID = SM.ReferralBillingAuthorizationID
	LEFT JOIN Payors P 
		ON P.PayorID = RBA.PayorID
	WHERE SM.ScheduleID = @ScheduleID
END