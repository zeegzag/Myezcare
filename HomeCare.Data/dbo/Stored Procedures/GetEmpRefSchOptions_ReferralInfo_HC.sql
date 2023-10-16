  
CREATE PROCEDURE [dbo].[GetEmpRefSchOptions_ReferralInfo_HC]                 
  @ReferralID BIGINT = 0,            
  @ScheduleID BIGINT = 0,            
  @StartDate DATETIME,            
  @EndDate DATETIME,            
  @EmployeeName VARCHAR(MAX) = '',            
  @MileRadius BIGINT = NULL,                                                
  @StrSkillList VARCHAR(MAX) = NULL,            
  @StrPreferenceList VARCHAR(MAX) = NULL,            
  @PreferenceType_Prefernce VARCHAR(100) = 'Preference',            
  @PreferenceType_Skill VARCHAR(100) = 'Skill',            
  @SameDateWithTimeSlot BIT,            
  @SortExpression NVARCHAR(100),            
  @SortType NVARCHAR(10),            
  @FromIndex INT,            
  @PageSize INT,            
  @SortIndexArray VARCHAR(MAX),            
  @DDType_CareType INT,            
  @CareTypeID VARCHAR(MAX) = NULL   
AS    
BEGIN    
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
   DECLARE @DayNumber INT;                        
   IF (@SameDateWithTimeSlot = 1)              
  SELECT @DayNumber = [dbo].[GetWeekDayNumberFromDate](@StartDate);              
              
   DECLARE @InfiniteEndDate DATE = '2099-12-31';              
   DECLARE @PatientPayorID BIGINT = 0;              
   DECLARE @EmployeeID BIGINT = 0;              
   DECLARE @EmployeeTSDateID BIGINT = 0;              
   DECLARE @ReferralTSDateID BIGINT = 0;              
              
   SELECT @EmployeeID = EmployeeID,              
  @EmployeeTSDateID = EmployeeTSDateID,              
  @ReferralTSDateID = ReferralTSDateID,              
  @PatientPayorID = PayorID              
   FROM ScheduleMasters WITH (NOLOCK)             
   WHERE ScheduleID = @ScheduleID              
              
   DECLARE @ReferralTimeSlotMasterID BIGINT = 0;              
   DECLARE @ReferralTimeSlotDetailID BIGINT = 0;              
              
   IF (              
    @ReferralTSDateID IS NULL              
    OR @ReferralTSDateID = 0              
    )              
   BEGIN              
  SELECT TOP 1 @ReferralTimeSlotMasterID = ReferralTimeSlotMasterID              
  FROM ReferralTimeSlotMaster WITH (NOLOCK)             
  WHERE ReferralID = @ReferralID              
    AND (              
   Convert(DATE, @StartDate) BETWEEN StartDate              
     AND ISNULL(EndDate, @InfiniteEndDate)              
   OR StartDate BETWEEN Convert(DATE, @StartDate)              
     AND ISNULL(EndDate, @InfiniteEndDate)              
   )              
              
  DECLARE @DateTimeDiffrence BIGINT = 0;              
              
  SET @DateTimeDiffrence = ISNULL(DATEDIFF(DAY, @StartDate, @EndDate), 0)              
              
  SELECT r.ReferralID,r.FirstName, r.LastName, 
	ReferralName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),
    PatientPayorID = @PatientPayorID              
    FROM [dbo].[Referrals] AS r WITH (NOLOCK)             
   WHERE r.ReferralID = @ReferralID              
              
   SELECT TOP 1 *              
  FROM ReferralTimeSlotMaster RTS WITH (NOLOCK)             
  WHERE RTS.ReferralTimeSlotMasterID = @ReferralTimeSlotMasterID              
              
   PRINT @DayNumber              
              
  -- Written by Pallav to show the timeslots                  
  SELECT DISTINCT RTSD.*,              
    RemainingSlotCount = SUM(CASE               
     WHEN SM.ScheduleID IS NULL              
    THEN 1              
     ELSE 0              
     END) OVER (              
   PARTITION BY RTSD.ReferralTimeSlotMasterID,              
   RTSD.ReferralTimeSlotDetailID,              
   RTSD.DAY              
   )              
  FROM ReferralTimeSlotDetails RTSD WITH (NOLOCK)              
  INNER JOIN ReferralTimeSlotDates RTDA WITH (NOLOCK)             
    ON RTDA.ReferralTimeSlotDetailID = RTSD.ReferralTimeSlotDetailID              
  INNER JOIN ReferralTimeSlotMaster RTM WITH (NOLOCK)              
    ON RTM.ReferralTimeSlotMasterID = RTSD.ReferralTimeSlotMasterID              
   AND RTM.referralID = @ReferralID              
  LEFT JOIN ScheduleMasters SM WITH (NOLOCK)             
    ON SM.ReferralTSDateID = RTDA.ReferralTSDateID              
   AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted = 0               
   WHERE RTDA.referralID = @referralid              
    AND ReferralTSStartTime >= @StartDate              
    AND ReferralTSEndTime <= @EndDate              
    AND RTSD.CaretypeID IN (              
   SELECT CONVERT(BIGINT, VAL)              
   FROM GetCSVTable(@caretypeid)              
   )              
    AND RTSD.IsDeleted = 0              
    AND --RTM.ReferralID=@ReferralID and                                            
    (              
   @DayNumber IS NULL              
   OR @DayNumber = 0              
   OR (              
    @DayNumber = RTSD.Day              
  AND  @StartDate BETWEEN RTDA.ReferralTSStartTime                
      AND RTDA.ReferralTSEndTime            
     )              
   )              
    -- Finished editng by Pallav                 
   END              
   ELSE              
   BEGIN                                                    
  SELECT @ReferralTimeSlotMasterID = ReferralTimeSlotMasterID,              
    @ReferralTimeSlotDetailID = ReferralTimeSlotDetailID              
  FROM ReferralTimeSlotDates WITH (NOLOCK)             
  WHERE ReferralTSDateID = @ReferralTSDateID              
              
  SELECT ReferralID, FirstName, LastName,        
	ReferralName = dbo.GetGenericNameFormat(FirstName,MiddleName,LastName,@NameFormat),
    PatientPayorID = @PatientPayorID              
  FROM Referrals WITH (NOLOCK)             
  WHERE ReferralID = @ReferralID              
              
  SELECT TOP 1 *              
  FROM ReferralTimeSlotMaster RTS  WITH (NOLOCK)            
  WHERE RTS.ReferralTimeSlotMasterID = @ReferralTimeSlotMasterID              
              
  SELECT RTSD.*,              
    RemainingSlotCount = SUM(CASE               
     WHEN SM.ScheduleID IS NULL              
    THEN 1              
     ELSE 0              
     END) OVER (              
   PARTITION BY RTSD.ReferralTimeSlotMasterID,              
   RTSD.ReferralTimeSlotDetailID,              
   RTSD.DAY              
   )              
  FROM ReferralTimeSlotDetails RTSD WITH (NOLOCK)             
  LEFT JOIN ScheduleMasters SM WITH (NOLOCK)             
    ON SM.ScheduleID = @ScheduleID              
   AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted = 0              
  WHERE ReferralTimeSlotDetailID = @ReferralTimeSlotDetailID              
  ORDER BY DAY ASC,              
    StartTime ASC              
              
  IF (@ReferralTimeSlotDetailID IS NULL)              
    SET @ReferralTimeSlotDetailID = 0;              
   END              
  
  
END           