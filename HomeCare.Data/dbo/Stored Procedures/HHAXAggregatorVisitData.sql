CREATE PROCEDURE [dbo].[HHAXAggregatorVisitData]
  @EventName nvarchar(max),
  @ScheduleID bigint,
  @ReasonCode nvarchar(max),
  @ActionCode nvarchar(max),
  @OrganizationID bigint,
  @HHAXAggregator nvarchar(max)
AS
BEGIN

  DECLARE @IsDelete bit = (SELECT CASE WHEN @EventName = 'DeleteSchedule' THEN 1 ELSE 0 END)	
  DECLARE @Result int = 0,
          @EmployeeID bigint,
          @Payor nvarchar(max),
          @Aggregator nvarchar(max),
          @IsBillVisit BIT,
          @NA varchar(5) = '-NA-'

  SELECT
    @EmployeeID = AGI.EmployeeID,
    @Payor = ISNULL(AGI.PayorName, @NA),
    @Aggregator = ISNULL(AGI.ClaimProcessor, @NA),
    @IsBillVisit = CAST(CASE WHEN AGI.VisitBilledBy = @HHAXAggregator THEN 1 ELSE 0 END AS BIT)
  FROM [dbo].[GetAggregatorInfo](@ScheduleID) AGI
  LEFT JOIN EmployeeVisits EV
	ON EV.ScheduleID = AGI.ScheduleID
  WHERE
    (ISNULL(AGI.AnyTimeClockIn, 0) = 0 OR EV.EmployeeVisitID IS NOT NULL)

  DECLARE @ProviderTaxID nvarchar(max),
          @LenProviderTaxID int,
          @LenHHAXAuth int,
		  @LenHHAXEvvmsID int
  
    SELECT
      @ProviderTaxID = BillingProvider_REF02_ReferenceIdentification,
      @LenProviderTaxID = LEN(ISNULL(BillingProvider_REF02_ReferenceIdentification, '')),
      @LenHHAXAuth = LEN(ISNULL(HHAXClientId, '') + ISNULL(HHAXClientId, ''))
    FROM OrganizationSettings

  IF (@IsDelete = 1)
  BEGIN
    SELECT
      @LenHHAXEvvmsID = LEN(ISNULL([HHAXEvvmsID], ''))
    FROM ScheduleDetails
  END

  IF ((@Aggregator = @HHAXAggregator AND ISNULL(@EmployeeID, 0) > 0 AND @LenProviderTaxID > 0 AND @LenHHAXAuth > 0) OR (@LenHHAXEvvmsID > 0))
  BEGIN
    EXEC [dbo].[CreateAndSaveBatchForSchedule] @ScheduleID = @ScheduleID

    SELECT
      CASE WHEN @IsDelete = 1 THEN 'delete' ELSE @EventName END [EventName],
      @ScheduleID [ScheduleID],
      @OrganizationID [OrganizationID],
      dbo.udf_HHAXCaregiver(@ProviderTaxID, @EmployeeID) [Caregiver],
      CASE WHEN @IsDelete = 1 THEN dbo.udf_HHAXDeletedVisit(@ScheduleID) ELSE dbo.udf_HHAXVisit(@EventName, @ProviderTaxID, @ScheduleID, @ReasonCode, @ActionCode, @IsBillVisit) END [Visit];
    SET @Result = 1;
  END
  ELSE
  BEGIN
    SELECT
      NULL;
  END

  SELECT
      @Payor [Payor], 
      @Aggregator [Aggregator], 
      ISNULL(@EmployeeID, 0) [EmployeeID], 
      @LenProviderTaxID [ProviderTaxIDLength],
      @LenHHAXAuth [HHAXAuthLength];
    SELECT
      @Result [ResultID];
END