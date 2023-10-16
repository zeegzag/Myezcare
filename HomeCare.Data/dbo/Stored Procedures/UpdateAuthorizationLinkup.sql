CREATE PROCEDURE [dbo].[UpdateAuthorizationLinkup]
  @ReferralBillingAuthorizationID bigint,
  @ScheduleIDs nvarchar(max)
AS
BEGIN

  UPDATE SM
  SET
    SM.CareTypeId = RBA.CareType,
    SM.ReferralBillingAuthorizationID = RBA.ReferralBillingAuthorizationID,
    SM.PayorID = RBA.PayorID
  FROM ReferralBillingAuthorizations RBA
  INNER JOIN ScheduleMasters SM
    ON SM.IsDeleted = 0
    AND SM.ScheduleID IN
    (
      SELECT
        [val]
      FROM GetCSVTable(@ScheduleIDs)
    )
    AND (SM.PayorID <> RBA.PayorID
    OR SM.ReferralBillingAuthorizationID <> RBA.ReferralBillingAuthorizationID
    OR SM.CareTypeId <> RBA.CareType)
  WHERE
    RBA.ReferralBillingAuthorizationID = @ReferralBillingAuthorizationID
    AND RBA.IsDeleted = 0

  DECLARE @CurScheduleID bigint;
  DECLARE eventCursor CURSOR FORWARD_ONLY FOR
       SELECT
          [val]
        FROM GetCSVTable(@ScheduleIDs)
  OPEN eventCursor;
  FETCH NEXT FROM eventCursor INTO @CurScheduleID;
  WHILE @@FETCH_STATUS = 0 BEGIN
      EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @CurScheduleID,'',''
      FETCH NEXT FROM eventCursor INTO @CurScheduleID;
  END;
  CLOSE eventCursor;
  DEALLOCATE eventCursor;

END