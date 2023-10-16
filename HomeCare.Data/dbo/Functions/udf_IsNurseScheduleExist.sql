CREATE FUNCTION dbo.udf_IsNurseScheduleExist (
  @Date DATE,
  @StartTime TIME,
  @EndTime TIME,
  @EmployeeId BIGINT,
  @ReferralId BIGINT,
  @CareTypeId BIGINT,
  @PayorID BIGINT,
  @ReferralBillingAuthorizationID BIGINT,
  @AnyTimeClockIn BIT
  )
RETURNS BIT
AS
BEGIN
  DECLARE @IsExist BIT;

  IF EXISTS (
      SELECT 1
      FROM ScheduleMasters SM
      WHERE @Date BETWEEN CONVERT(DATE, SM.StartDate)
          AND CONVERT(DATE, SM.EndDate)
        AND @StartTime = CONVERT(TIME, SM.StartDate)
        AND @EndTime = CONVERT(TIME, SM.EndDate)
        AND ISNULL(SM.EmployeeID, 0) = ISNULL(@EmployeeId, 0)
        AND ISNULL(SM.ReferralID, 0) = ISNULL(@ReferralId, 0)
        AND ISNULL(SM.CareTypeId, 0) = ISNULL(@CareTypeId, 0)
        AND ISNULL(SM.PayorID, 0) = ISNULL(@PayorID, 0)
        AND ISNULL(SM.ReferralBillingAuthorizationID, 0) = ISNULL(
          @ReferralBillingAuthorizationID, 0)
        AND ISNULL(SM.AnyTimeClockIn, 0) = ISNULL(@AnyTimeClockIn, 0)
        AND SM.IsDeleted = 0
      )
    SET @IsExist = 1;
  ELSE
    SET @IsExist = 0;

  RETURN @IsExist;
END;
