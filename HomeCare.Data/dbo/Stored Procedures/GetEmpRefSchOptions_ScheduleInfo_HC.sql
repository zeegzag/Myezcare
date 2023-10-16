CREATE PROCEDURE [dbo].[GetEmpRefSchOptions_ScheduleInfo_HC]                   
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
      
    
  --DECLARE @temp TABLE (CareTypeIds VARCHAR(MAX));              
  --DECLARE @IDs VARCHAR(MAX);              
              
  --INSERT INTO @temp              
  --SELECT STUFF(ISNULL(',' + T.CareTypeIds, '') + ISNULL(',' + T.SlotCareTypeIds, ''), 1, 1,               
  --    '') AS CareTypeIds              
  --FROM (              
  --  SELECT CareTypeIds,              
  --    STUFF((              
  --        SELECT DISTINCT ', ' + convert(VARCHAR(max), RTD.CareTypeId, 120)              
  --        FROM ReferralTimeslotdetails RTD WITH (NOLOCK)             
  --        INNER JOIN ReferralTimeSlotMaster RTM WITH (NOLOCK)             
  --          ON RTM.ReferralTimeSlotMasterID = RTD.ReferralTimeSlotMasterID              
  --        INNER JOIN ReferralTimeSlotDates RTDS WITH (NOLOCK)             
  --          ON RTDS.ReferralTimeSlotDetailID = RTD.ReferralTimeSlotDetailID              
  --        WHERE RTM.ReferralID = @ReferralID              
  --          AND RTDS.ReferralTSDate BETWEEN Convert(DATE, @StartDate)              
  --            AND Convert(DATE, @EndDate)              
  --        FOR XML PATH('')              
  --        ), 1, 1, '') AS SlotCareTypeIds              
  --  FROM Referrals WITH (NOLOCK)             
  --  WHERE ReferralId = @ReferralID              
  --  ) AS T              
              
  --SELECT @IDs = CareTypeIds              
  --FROM @temp              
              
  --SELECT *              
  --FROM DDMaster WITH (NOLOCK)             
  --WHERE ItemType = @DDType_CareType              
  --  AND IsDeleted = 0              
  --  AND DDMasterID IN (              
  --    SELECT val              
  --    FROM GetCSVTable(@IDs)              
  --    )              
              
  ----SELECT * FROm Referrals WHERE AHCCCSID='A47130732'                                 
  --SELECT P.PayorName,              
  --  P.ShortName,              
  --  P.PayorID,              
  --  RPM.Precedence              
  --FROM ReferralPayorMappings RPM WITH (NOLOCK)             
  --INNER JOIN Payors P WITH (NOLOCK)             
  --  ON P.PayorID = RPM.PayorID              
  --WHERE RPM.ReferralID = @ReferralID              
  --  AND RPM.Precedence IS NOT NULL              
  --  AND RPM.IsDeleted = 0              
  --  AND CONVERT(DATE, @StartDate) BETWEEN RPM.PayorEffectiveDate              
  --    AND RPM.PayorEffectiveEndDate              
  --ORDER BY RPM.Precedence ASC              
              
  --SELECT RH.*,              
  --  CreatedBy = dbo.GetGeneralNameFormat(EC.FirstName, EC.LastName),              
  --  UpdatedBy = dbo.GetGeneralNameFormat(EU.FirstName, EU.LastName),              
  --  CurrentActiveGroup = CASE               
  --    WHEN GETDATE() BETWEEN RH.StartDate              
  --        AND RH.EndDate              
  --      THEN 1              
  --    ELSE 0              
  --    END,              
  --  OldActiveGroup = CASE               
  --    WHEN GETDATE() > RH.EndDate              
  --      THEN 1              
  --    ELSE 0              
  --    END              
  --FROM ReferralOnHoldDetails RH  WITH (NOLOCK)            
  --INNER JOIN Employees EC WITH (NOLOCK)             
  --  ON EC.EmployeeID = RH.CreatedBy              
  --INNER JOIN Employees EU WITH (NOLOCK)      
  --  ON EU.EmployeeID = RH.UpdatedBy              
  --WHERE RH.IsDeleted = 0              
  --  AND RH.ReferralID = @ReferralID               
  --  -- AND (GETDATE() BETWEEN RH.StartDate AND RH.EndDate OR GETDATE() < RH.StartDate)                                            
  --ORDER BY StartDate DESC              
              
  ----SELECT MIN(RTD.ReferralTSDate), MAX(RTD.ReferralTSDate) FROM ReferralTimeSlotDates RTD                                            
  ----WHERE RTD.ReferralID=@ReferralID AND RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate AND RTD.OnHold=1                                            
  IF (              
      @StrSkillList IS NULL              
      OR LEN(@StrSkillList) = 0              
      )              
    SET @StrSkillList = NULL;              
              
  IF (              
      @StrPreferenceList IS NULL              
      OR LEN(@StrPreferenceList) = 0              
      )              
    SET @StrPreferenceList = NULL;              
              
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
              
  --IF (              
  --    @ReferralTSDateID IS NULL              
  --    OR @ReferralTSDateID = 0              
  --    )              
  --BEGIN              
  --  SELECT TOP 1 @ReferralTimeSlotMasterID = ReferralTimeSlotMasterID              
  --  FROM ReferralTimeSlotMaster WITH (NOLOCK)             
  --  WHERE ReferralID = @ReferralID              
  --    AND (              
  --      Convert(DATE, @StartDate) BETWEEN StartDate              
  --        AND ISNULL(EndDate, @InfiniteEndDate)              
  --      OR StartDate BETWEEN Convert(DATE, @StartDate)              
  --        AND ISNULL(EndDate, @InfiniteEndDate)              
  --      )              
              
  --  DECLARE @DateTimeDiffrence BIGINT = 0;              
              
  --  SET @DateTimeDiffrence = ISNULL(DATEDIFF(DAY, @StartDate, @EndDate), 0)              
              
  --  SELECT r.ReferralID,              
  --    --ddm.Title,rba.AllowedTimeType,ISNULL((rba.AllowedTime),0) as AllowedTime,                 
  --    r.FirstName,              
  --    r.LastName,              
  --    PatientPayorID = @PatientPayorID              
  --  --,ISNULL(((CASE                           
  --  -- when ddm.Title = 'Daily' Then (rba.AllowedTime)                          
  --  -- when ddm.Title = 'Weekly' Then (rba.AllowedTime/(7))                           
  --  -- when ddm.Title = 'Monthly' Then (rba.AllowedTime/(30))                           
  --  -- when ddm.Title = 'Yearly' Then (rba.AllowedTime/(365))                           
  --  -- Else                          
  --  -- (rba.AllowedTime)                           
  --  -- end)                            
  --  --    * (CASE WHEN @DateTimeDiffrence = 0 THEN 1 else @DateTimeDiffrence end))/(case when rba.AllowedTimeType = 'Minutes' then 60 else 1 end),0) AS ScheduledHours,                            
  --  -- SUM(ISNULL(DATEDIFF(HOUR,ev.ClockInTime, ev.ClockOutTime),0)) AS UsedHours,                            
  --  -- ISNULL((((CASE                           
  --  -- when ddm.Title = 'Daily' Then (rba.AllowedTime)                          
  --  -- when ddm.Title = 'Weekly' Then (rba.AllowedTime/(7))                           
  --  -- when ddm.Title = 'Monthly' Then (rba.AllowedTime/(30))                           
  --  -- when ddm.Title = 'Yearly' Then (rba.AllowedTime/(365))                           
  --  -- Else                          
  --  -- (rba.AllowedTime)                           
  --  -- end)                            
  --  --    * (CASE WHEN @DateTimeDiffrence = 0 THEN 1 else @DateTimeDiffrence end))/(case when rba.AllowedTimeType = 'Minutes' then 60 else 1 end)                            
  --  --   - (SUM(ISNULL(DATEDIFF(HOUR,ev.ClockInTime, ev.ClockOutTime),0)))),0) AS PendingHours              
  --  FROM [dbo].[Referrals] AS r WITH (NOLOCK)             
  --  --LEFT JOIN [dbo].[ScheduleMasters] AS sm                            
  --  --ON sm.ReferralID = r.ReferralID AND (@StartDate between sm.StartDate  AND sm.EndDate) And (@EndDate  between sm.StartDate  AND sm.EndDate)                      
  --  --LEFT JOIN [dbo].[EmployeeVisits] AS ev                            
  --  --ON ev.ScheduleID = sm.ScheduleID AND ev.IsDeleted = 0                             
  --  --LEFT JOIN [dbo].[ReferralBillingAuthorizations] AS rba                            
  --  --ON rba.ReferralID = r.ReferralID  AND  (@StartDate between rba.StartDate  AND rba.EndDate) AND (@EndDate between rba.StartDate  AND rba.EndDate)                           
  --  --LEFT JOIN [dbo].[DDMaster] ddm                             
  --  --ON ddm.DDMasterID = rba.PriorAuthorizationFrequencyType                            
  --  WHERE r.ReferralID = @ReferralID              
              
  --  --GROUP BY r.ReferralID,r.FirstName,r.LastName,rba.AllowedTime,ddm.Title,rba.AllowedTimeType                            
  --  --SELECT ReferralID, FirstName, LastName, PatientPayorID=@PatientPayorID FROM Referrals WHERE ReferralID=@ReferralID                                              
  --  SELECT TOP 1 *              
  --  FROM ReferralTimeSlotMaster RTS WITH (NOLOCK)             
  --  WHERE RTS.ReferralTimeSlotMasterID = @ReferralTimeSlotMasterID              
                                                     
  --  PRINT @DayNumber              
              
  --  -- Written by Pallav to show the timeslots                  
  --  SELECT DISTINCT RTSD.*,              
  --    RemainingSlotCount = SUM(CASE               
  --        WHEN SM.ScheduleID IS NULL              
  --          THEN 1              
  --        ELSE 0              
  --        END) OVER (              
  --      PARTITION BY RTSD.ReferralTimeSlotMasterID,              
  --      RTSD.ReferralTimeSlotDetailID,              
  --      RTSD.DAY              
  --      )              
  --  FROM ReferralTimeSlotDetails RTSD WITH (NOLOCK)              
  --  INNER JOIN ReferralTimeSlotDates RTDA WITH (NOLOCK)             
  --    ON RTDA.ReferralTimeSlotDetailID = RTSD.ReferralTimeSlotDetailID              
  --  INNER JOIN ReferralTimeSlotMaster RTM WITH (NOLOCK)              
  --    ON RTM.ReferralTimeSlotMasterID = RTSD.ReferralTimeSlotMasterID              
  --      AND RTM.referralID = @ReferralID              
  --  LEFT JOIN ScheduleMasters SM WITH (NOLOCK)             
  --    ON SM.ReferralTSDateID = RTDA.ReferralTSDateID              
  --      AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted = 0               
  --      -- and SM.referralID=RTM.ReferralID  and SM.ReferralID=RTDA.ReferralID                
  --  WHERE RTDA.referralID = @referralid              
  --    AND ReferralTSStartTime >= @StartDate              
  --    AND ReferralTSEndTime <= @EndDate              
  --    AND RTSD.CaretypeID IN (              
  --      SELECT CONVERT(BIGINT, VAL)              
  --      FROM GetCSVTable(@caretypeid)              
  --      )              
 --    AND RTSD.IsDeleted = 0              
  --    AND --RTM.ReferralID=@ReferralID and                                            
  --    (              
  --      @DayNumber IS NULL              
  --      OR @DayNumber = 0              
  --      OR (              
  --        @DayNumber = RTSD.Day              
  --  AND  @StartDate BETWEEN RTDA.ReferralTSStartTime                
  --            AND RTDA.ReferralTSEndTime            
  --        )              
--      )              
  --    -- Finished editng by Pallav                 
  --    /*                
  --Commented by Pallav , as the timeslots were not showing up for scheduling.                
  ----SELECT DISTINCT RTSD.* ,                                            
  ----RemainingSlotCount= SUM(CASE WHEN SM.ScheduleID IS NULL THEN 1 ELSE 0 END) OVER                                 
  ----(PARTITION BY RTSD.ReferralTimeSlotMasterID,RTSD.ReferralTimeSlotDetailID, RTSD.DAY)                                            
  ----FROM ReferralTimeSlotDetails RTSD                                            
  ----Inner JOIN ReferralTimeSlotDates RTD ON RTD.ReferralTimeSlotDetailID=RTSD.ReferralTimeSlotDetailID   and RTSD.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID                                         
  ------AND (@DayNumber IS NULL OR @DayNumber=0 OR (@DayNumber=RTSD.Day                 
  ----AND CONVERT(TIME,@StartDate) BETWEEN RTSD.StartTime AND RTSD.EndTime                                         
  ----AND RTD.ReferralTSDate BETWEEN CONVERT(DATE,@StartDate) AND CONVERT(DATE,@EndDate) AND RTD.UsedInScheduling=1                     
  ----LEFT JOIN ScheduleMasters SM ON SM.ReferralTSDateID=RTD.ReferralTSDateID AND SM.IsDeleted=0                                            
  ------WHERE RTSD.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID AND RTSD.IsDeleted=0 AND                                             
  ------(@DayNumber IS NULL OR @DayNumber=0 OR (@DayNumber=RTSD.Day AND CONVERT(TIME,@StartDate) BETWEEN RTSD.StartTime AND RTSD.EndTime))                                            
  --ORDER BY DAY ASC,StartTime ASC                           
  --*/              
  --    --SELECT ReferralBillingAuthorizationID, (ra.AuthorizationCode + ' - ' + s.ServiceCode) AS ReferralBillingAuthorizationName FROM ReferralBillingAuthorizations ra                 
  --    --        INNER JOIN ServiceCodes s ON ra.ServiceCodeID = s.ServiceCodeID                
  --    --     WHERE ra.ReferralID = @ReferralID AND ra.IsDeleted = 0 AND (@CareTypeID IS NULL OR @CareTypeID = 0 OR ra.CareType = @CareTypeID)                
  --END              
  --ELSE              
  --BEGIN              
  --  --SELECT * FROM ScheduleMasters WHERE ScheduleID=57829                                              
  --  --SELECT * FROM ReferralTimeSlotDates WHERE ReferralTSDateID=61                                              
  --  -- SELECT * FROM  ReferralTimeSlotMaster WHERE ReferralTimeSlotMasterID=1                                              
  --  SELECT @ReferralTimeSlotMasterID = ReferralTimeSlotMasterID,              
  --    @ReferralTimeSlotDetailID = ReferralTimeSlotDetailID              
  --  FROM ReferralTimeSlotDates WITH (NOLOCK)             
  --  WHERE ReferralTSDateID = @ReferralTSDateID              
              
  --  SELECT ReferralID,              
  --    FirstName,              
  --    LastName,              
  --    PatientPayorID = @PatientPayorID              
  --  FROM Referrals WITH (NOLOCK)             
  --  WHERE ReferralID = @ReferralID              
              
  --  SELECT TOP 1 *              
  --  FROM ReferralTimeSlotMaster RTS  WITH (NOLOCK)            
  --  WHERE RTS.ReferralTimeSlotMasterID = @ReferralTimeSlotMasterID              
              
  --  --SELECT * FROM ReferralTimeSlotDates                                              
  --  SELECT RTSD.*,              
  --    RemainingSlotCount = SUM(CASE               
  --        WHEN SM.ScheduleID IS NULL              
  --  THEN 1              
  --        ELSE 0              
  --        END) OVER (              
  --      PARTITION BY RTSD.ReferralTimeSlotMasterID,              
  --      RTSD.ReferralTimeSlotDetailID,              
  --      RTSD.DAY              
  --      )              
  --  FROM ReferralTimeSlotDetails RTSD WITH (NOLOCK)             
  --  LEFT JOIN ScheduleMasters SM WITH (NOLOCK)             
  --    ON SM.ScheduleID = @ScheduleID              
  --      AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted = 0              
  --  WHERE ReferralTimeSlotDetailID = @ReferralTimeSlotDetailID              
  --  ORDER BY DAY ASC,              
  --    StartTime ASC              
              
  --  IF (@ReferralTimeSlotDetailID IS NULL)              
  --    SET @ReferralTimeSlotDetailID = 0;              
  --END              
            
  DECLARE @TotalPrefeCount BIGINT = 0;              
  DECLARE @TotalSkillCount BIGINT = 0;              
              
  SELECT @TotalPrefeCount = SUM(CASE               
        WHEN PR.KeyType = @PreferenceType_Prefernce              
          THEN 1              
        ELSE 0              
   END) OVER (PARTITION BY RP.ReferralID),              
    @TotalSkillCount = SUM(CASE               
        WHEN PR.KeyType = @PreferenceType_Skill              
          THEN 1              
        ELSE 0              
        END) OVER (PARTITION BY RP.ReferralID)              
  FROM ReferralPreferences RP WITH (NOLOCK)             
  INNER JOIN Preferences PR  WITH (NOLOCK)            
    ON PR.PreferenceID = RP.PreferenceID              
  WHERE ReferralID = @ReferralID              
              
  IF (@TotalPrefeCount = 0)              
    SET @TotalPrefeCount = 1;              
              
  IF (@TotalSkillCount = 0)              
SET @TotalSkillCount = 1;              
  --print '@ReferralTimeSlotDetailID = ' + Convert(varchar(100), @ReferralTimeSlotDetailID)            
  -- PRINT @TotalPrefeCount                                              
  -- PRINT @TotalSkillCount      
        
  SELECT *      
 INTO #TempReferralTimeSlotMaster       
 FROM dbo.ReferralTimeSlotMaster RTSM WITH (NOLOCK)      
 WHERE       
  RTSM.ReferralID = @ReferralID                 
        AND (                
          Convert(DATE, @StartDate) BETWEEN RTSM.StartDate                
            AND ISNULL(RTSM.EndDate, @InfiniteEndDate)                
          OR RTSM.StartDate BETWEEN Convert(DATE, @StartDate)                
            AND ISNULL(RTSM.EndDate, @InfiniteEndDate)                
          );      
      
 SELECT *      
 INTO #TempReferralTimeSlotDetails      
 FROM dbo.ReferralTimeSlotDetails RTSD WITH (NOLOCK)      
 WHERE RTSD.IsDeleted = 0      
  AND RTSD.ReferralTimeSlotMasterID IN (      
            SELECT ReferralTimeSlotMasterID       
            FROM #TempReferralTimeSlotMaster RTSM WITH (NOLOCK)       
             )      
        AND (@ReferralTimeSlotDetailID = 0 OR RTSD.ReferralTimeSlotDetailID = @ReferralTimeSlotDetailID );      
      
 SELECT *      
 INTO #TempReferralTimeSlotDates      
 FROM ReferralTimeSlotDates RTSDT WITH (NOLOCK)        
 WHERE RTSDT.ReferralTimeSlotMasterID  IN (      
            SELECT ReferralTimeSlotMasterID       
            FROM #TempReferralTimeSlotMaster RTSM WITH (NOLOCK)       
             )      
  AND RTSDT.ReferralTimeSlotDetailID IN (      
            SELECT RTSD.ReferralTimeSlotDetailID       
            FROM #TempReferralTimeSlotDetails RTSD WITH (NOLOCK)      
             )         
            
      
  DECLARE @TempGetEmpTimeSlots TABLE (              
    FirstName VARCHAR(5000),              
    LastName VARCHAR(5000),
	EmployeeName VARCHAR(5000),
    EmployeeID BIGINT,              
    ETMStartDate DATE NULL,              
    ETMEndDate DATE NULL,                  
    IsDeleted BIGINT,              
    Latitude FLOAT,              
    Longitude FLOAT,              
    RankNumber BIGINT,              
    Frequency VARCHAR(MAX),              
    EmployeeTimeSlotDetailIDs VARCHAR(MAX)              
    );        
              
  WITH CTETempGetEmpTimeSlots              
  AS (                
    SELECT E.FirstName,                
      E.LastName, 
	  dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS EmployeeName,
      ETM.EmployeeID,                
      ETM.StartDate,                
      ETM.EndDate,                         
      E.IsDeleted,                
      E.Latitude,                
      E.Longitude,    
   RankNumber = ROW_NUMBER() OVER (PARTITION BY ETM.EmployeeTimeSlotMasterID ORDER BY ETM.StartDate),       
   ETM.Frequency,    
   ETM.EmployeeTimeSlotDetailIDs    
    FROM     
 (    
    
 SELECT     
  ETS.EmployeeTimeSlotMasterID,    
  ETS.EmployeeID,                
  ETS.StartDate,                
  ETS.EndDate,      
  Frequency = STRING_AGG(case when seqnum_Day = 1 then CONVERT(VARCHAR(MAX), dbo.GetWeekDay(ETS.[Day])) end, ','),    
  EmployeeTimeSlotDetailIDs = STRING_AGG(case when seqnum_EmployeeTimeSlotDetailID = 1 then CONVERT(VARCHAR(MAX), ETS.EmployeeTimeSlotDetailID) end, ',')    
 FROM (    
 SELECT     
 ETM.EmployeeTimeSlotMasterID,     
 ETM.EmployeeID,    
 ETM.StartDate,    
 ETM.EndDate,                
   ETSD.EmployeeTimeSlotDetailID,    
   ETSD.[Day],    
   row_number() over (partition by ETM.EmployeeTimeSlotMasterID,     
 ETM.EmployeeID,    
 ETM.StartDate,    
 ETM.EndDate, ETSD.[Day] order by ETSD.[Day]) as seqnum_Day,    
   row_number() over (partition by ETM.EmployeeTimeSlotMasterID,     
 ETM.EmployeeID,    
 ETM.StartDate,    
 ETM.EndDate, ETSD.EmployeeTimeSlotDetailID order by ETSD.EmployeeTimeSlotDetailID) as seqnum_EmployeeTimeSlotDetailID    
   --   Frequency = STUFF((                
   --       SELECT ',' + CONVERT(VARCHAR(MAX), dbo.GetWeekDay(ETSD1.Day))                
   --       FROM EmployeeTimeSlotDetails ETSD1                
   --       INNER JOIN EmployeeTimeSlotMaster ETM1                
   --         ON ETM1.EmployeeTimeSlotMasterID = ETSD1.EmployeeTimeSlotMasterID                
   --           AND ETSD1.IsDeleted = 0                
   --       WHERE ETM1.EmployeeID = ETM.EmployeeID                
   --         AND ISNULL(ETSD1.StartTime, '') = ISNULL(ETSD.StartTime, '')                  
   --         AND ISNULL(ETSD1.EndTime, '') = ISNULL(ETSD.EndTime, '')                 
   --       ORDER BY ETSD1.Day ASC                
   --       FOR XML PATH('')                
   --), 1, 1, ''),                
   --   EmployeeTimeSlotDetailIDs = STUFF((                
   --       SELECT ',' + CONVERT(VARCHAR(MAX), ETSD1.EmployeeTimeSlotDetailID)                
   --       FROM EmployeeTimeSlotDetails ETSD1                
   --       INNER JOIN EmployeeTimeSlotMaster ETM1                
   --         ON ETM1.EmployeeTimeSlotMasterID = ETSD1.EmployeeTimeSlotMasterID                
   --           AND ETSD1.IsDeleted = 0                
   --       WHERE ETM1.EmployeeID = ETM.EmployeeID                
   --         AND ISNULL(ETSD1.StartTime, '') = ISNULL(ETSD.StartTime, '')                  
   --         AND ISNULL(ETSD1.EndTime, '') = ISNULL(ETSD.EndTime, '')       
   --       ORDER BY ETSD1.EmployeeTimeSlotDetailID ASC                
   --       FOR XML PATH('')                
   --       ), 1, 1, '')    
 FROM    
 EmployeeTimeSlotDates ETSDT WITH (NOLOCK)              
  INNER JOIN EmployeeTimeSlotDetails ETSD WITH (NOLOCK) ON ETSD.IsDeleted = 0  AND ETSD.EmployeeTimeSlotDetailID = ETSDT.EmployeeTimeSlotDetailID              
  INNER JOIN EmployeeTimeSlotMaster ETM WITH (NOLOCK) ON ETM.IsDeleted = 0 AND ETM.EmployeeTimeSlotMasterID = ETSD.EmployeeTimeSlotMasterID                
        AND (                
          Convert(DATE, @StartDate) BETWEEN ETM.StartDate AND ISNULL(ETM.EndDate, @InfiniteEndDate)                
   OR ETM.StartDate BETWEEN Convert(DATE, @StartDate) AND ISNULL(ETM.EndDate, @InfiniteEndDate)                
          )                
    INNER JOIN #TempReferralTimeSlotMaster RTSM WITH (NOLOCK) ON RTSM.ReferralID = @ReferralID                 
    --INNER JOIN ReferralTimeSlotDetails RTSD ON RTSD.IsDeleted=0  AND RTSD.ReferralTimeSlotMasterID = RTSM.ReferralTimeSlotMasterID                                                
    INNER JOIN #TempReferralTimeSlotDetails RTSD WITH (NOLOCK) ON RTSD.ReferralTimeSlotMasterID = RTSM.ReferralTimeSlotMasterID AND RTSD.Day = ETSD.Day                
 INNER JOIN #TempReferralTimeSlotDates RTSDT WITH (NOLOCK)             
 ON RTSDT.ReferralTimeSlotMasterID = RTSM.ReferralTimeSlotMasterID              
 AND RTSDT.ReferralTimeSlotDetailID = RTSD.ReferralTimeSlotDetailID              
 AND RTSDT.ReferralTSStartTime BETWEEN ETSDT.EmployeeTSStartTime                
          AND ETSDT.EmployeeTSEndTime                
        AND RTSDT.ReferralTSEndTime BETWEEN ETSDT.EmployeeTSStartTime                
          AND ETSDT.EmployeeTSEndTime                
    AND (                  @DayNumber IS NULL                
          OR @DayNumber = 0                
          OR (                
            @DayNumber = RTSD.Day                
            AND  @StartDate BETWEEN RTSDT.ReferralTSStartTime                
              AND RTSDT.ReferralTSEndTime                
            )                
          )     
 WHERE 1 = 1    
  AND (                
          @SameDateWithTimeSlot = 0                
          OR (                
            @SameDateWithTimeSlot = 1                
            AND  @StartDate BETWEEN ETSDT.EmployeeTSStartTime                
              AND ETSDT.EmployeeTSEndTime                
            AND @EndDate BETWEEN ETSDT.EmployeeTSStartTime                
              AND ETSDT.EmployeeTSEndTime                
            )                
          )     
 ) ETS    
  GROUP BY     
   ETS.EmployeeTimeSlotMasterID,    
   ETS.EmployeeID,                
   ETS.StartDate,                
   ETS.EndDate    
 ) ETM    
    INNER JOIN Employees E WITH (NOLOCK)               
      ON E.EmployeeID = ETM.EmployeeID                
        AND E.IsDeleted = 0 -- AND E.RoleID!=1                                                
    WHERE 1 = 1                
      AND (                
        @CareTypeID IS NULL                
        OR @CareTypeID = ''                
        OR @CareTypeID = 0                
        OR E.CareTypeIds LIKE '%' + @CareTypeID + '%'                
        OR E.CareTypeIds LIKE '%,' + @CareTypeID + ',%'                
        OR E.CareTypeIds LIKE '%,' + @CareTypeID + '%'                
        OR E.CareTypeIds LIKE '%' + @CareTypeID + ',%'              
        )                
      AND (                
        (                
          @EmployeeName IS NULL                
          OR LEN(@EmployeeName) = 0                
          OR E.FirstName IS NULL                
          )                
        OR (                
          (E.FirstName LIKE '%' + @EmployeeName + '%')                
          OR (E.LastName LIKE '%' + @EmployeeName + '%')                
          OR (E.FirstName + ' ' + E.LastName LIKE '%' + @EmployeeName + '%')                
          OR (E.LastName + ' ' + E.FirstName LIKE '%' + @EmployeeName + '%')                
          OR (E.FirstName + ', ' + E.LastName LIKE '%' + @EmployeeName + '%')                
          OR (E.LastName + ', ' + E.FirstName LIKE '%' + @EmployeeName + '%')                
          )           
        )                
                 
    )                
  INSERT INTO @TempGetEmpTimeSlots              
  SELECT TGETS.*              
  FROM CTETempGetEmpTimeSlots TGETS WITH (NOLOCK)             
  LEFT JOIN ReferralBlockedEmployees RBE WITH (NOLOCK)             
    ON RBE.ReferralID = @ReferralID              
      AND RBE.EmployeeID = TGETS.EmployeeID              
      AND RBE.IsDeleted = 0              
  WHERE TGETS.RankNumber = 1              
    AND RBE.ReferralBlockedEmployeeID IS NULL              
              
  PRINT @SameDateWithTimeSlot;              
  PRINT @EmployeeID --@ReferralTimeSlotDetailID;                             
    -- EXEC GetEmpRefSchOptions @ReferralID = '1951', @ScheduleID = '57841', @StartDate = '2018/03/05', @EndDate = '2018/03/25', @SortExpression = 'EmployeeDayOffID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'                                  
  
    
      
        
          
  --SELECT * FROM @TempGetEmpTimeSlots                                            
  -- SELECT * FROM @TempGetEmpTimeSlots                                            
  DECLARE @TempGetEmpoyeePreferences TABLE (            
    ReferralID BIGINT,            
    RefPreferenceID BIGINT,            
    EmpPreferenceID BIGINT,            
    EmployeeID BIGINT,          
    FirstName VARCHAR(5000),            
    LastName VARCHAR(5000), 
	EmployeeName VARCHAR(5000),
    IsDeleted BIT,            
    OrderRank INT,            
    PreferencesMatchPercent INT,            
    SkillsMatchPercent INT,            
    EmpLatLong GEOGRAPHY,            
    KeyType VARCHAR(MAX),            
    EmployeeTimeSlotDetailIDs VARCHAR(MAX),            
    Frequency VARCHAR(MAX),            
    --StartTime TIME,            
    --EndTime TIME,            
    ETMStartDate DATE NULL,            
    ETMEndDate DATE NULL            
    );            
            
  WITH CTETempGetEmpoyeePreferences            
  AS (            
    SELECT ReferralID,            
      RefPreferenceID,            
      EmpPreferenceID,            
      EmployeeID,            
      FirstName,            
      LastName, 
	  EmployeeName,
      IsDeleted,            
      OrderRank,            
      PreferencesMatchPercent,            
      SkillsMatchPercent,            
      EmpLatLong,            
      KeyType,            
      EmployeeTimeSlotDetailIDs,            
      Frequency,            
      --StartTime,            
      --EndTime,            
      ETMStartDate,            
      ETMEndDate            
    FROM (            
      SELECT R.ReferralID,            
        PE.KeyType,            
        RefPreferenceID = RP.PreferenceID,            
        EmpPreferenceID = EP.PreferenceID,            
        E.EmployeeID,            
        E.FirstName,            
        E.LastName,  
		E.EmployeeName,
        E.IsDeleted,            
        EmpLatLong = [dbo].GetGeoFromLatLng(E.Latitude, E.Longitude),            
        PreferencesMatchPercent = SUM(CASE             
            WHEN PE.KeyType = @PreferenceType_Prefernce            
              AND RP.PreferenceID IS NOT NULL            
              THEN 1            
            ELSE 0            
            END) OVER (            
          PARTITION BY EP.EmployeeID,            
          EmployeeTimeSlotDetailIDs            
          ) * 100 / @TotalPrefeCount,            
        SkillsMatchPercent = SUM(CASE             
            WHEN PE.KeyType = @PreferenceType_Skill            
              AND RP.PreferenceID IS NOT NULL            
              THEN 1            
            ELSE 0            
            END) OVER (            
          PARTITION BY EP.EmployeeID,            
          EmployeeTimeSlotDetailIDs            
          ) * 100 / @TotalSkillCount,            
     E.EmployeeTimeSlotDetailIDs,            
        E.Frequency,            
        --E.StartTime,            
        --E.EndTime,            
        ETMStartDate,            
        E.ETMEndDate,            
        --SkillsMatchCount=COUNT(R.ReferralID) OVER( PARTITION BY R.ReferralID,EP.EmployeeID,PE.KeyType)  ,                                          
        OrderRank = DENSE_RANK() OVER (            
          PARTITION BY EP.EmployeeID ORDER BY R.ReferralID DESC,            
            EP.EmployeePreferenceID DESC            
          )            
      FROM @TempGetEmpTimeSlots E            
      LEFT JOIN EmployeePreferences EP WITH (NOLOCK)           
        ON E.EmployeeID = EP.EmployeeID            
      LEFT JOIN ReferralPreferences RP  WITH (NOLOCK)          
        ON RP.PreferenceID = EP.PreferenceID            
          AND RP.ReferralID = @ReferralID            
      LEFT JOIN Preferences PE WITH (NOLOCK)           
        ON PE.PreferenceID = EP.PreferenceID            
      LEFT JOIN (            
    SELECT ReferralID = @ReferralID            
        ) R            
        ON R.ReferralID = RP.ReferralID            
      WHERE 1 = 1            
        --AND (  (@SkillId=0 AND @PreferenceId=0) OR                                            
        --      ((@SkillId=0 AND @PreferenceId!=0) AND (EP.PreferenceID = @PreferenceId)) OR                                            
--      ((@SkillId!=0 AND @PreferenceId=0) AND (EP.PreferenceID = @SkillId)) OR                                            
        --      ((@SkillId!=0 AND @PreferenceId!=0) AND (EP.PreferenceID IN (@SkillId ,@PreferenceId)))  )                                            
        AND (            
          (            
            @StrSkillList IS NULL            
            AND @StrPreferenceList IS NULL            
            )            
          OR (            
            (            
              @StrSkillList IS NULL            
              AND @StrPreferenceList IS NOT NULL            
            )            
            AND (            
              EP.PreferenceID IN (            
                SELECT CONVERT(BIGINT, VAL)            
                FROM GetCSVTable(@StrPreferenceList)            
                )            
              )            
            )            
          OR (            
            (            
              @StrSkillList IS NOT NULL            
              AND @StrPreferenceList IS NULL            
              )            
            AND (            
              EP.PreferenceID IN (            
                SELECT CONVERT(BIGINT, VAL)            
                FROM GetCSVTable(@StrSkillList)            
                )            
              )            
            )            
          OR (            
            (            
              @StrSkillList IS NOT NULL            
              AND @StrPreferenceList IS NOT NULL            
              )            
            AND (            
              (            
                EP.PreferenceID IN (            
                  SELECT CONVERT(BIGINT, VAL)            
                  FROM GetCSVTable(@StrPreferenceList)            
                  )            
                )            
              OR (            
                EP.PreferenceID IN (            
                  SELECT CONVERT(BIGINT, VAL)            
                  FROM GetCSVTable(@StrSkillList)            
                  )            
                )            
              )            
            )            
          )            
        --R.ReferralID=3769  --E.EmployeeID=2                                            
        --ORDER BY ReferralID DESC,MatchCount DESC,FirstName ASC                                            
      ) AS TEMP            
    WHERE 1 = 1            
      AND TEMP.OrderRank = 1            
    )            
  INSERT INTO @TempGetEmpoyeePreferences            
  SELECT *            
  FROM CTETempGetEmpoyeePreferences  WITH (NOLOCK)          
            
  UPDATE @TempGetEmpoyeePreferences            
  SET ReferralID = @ReferralID            
            
  -- EXEC GetEmpRefSchOptions @ReferralID = '1951', @EmployeeID = '0', @StartDate = '2018/03/05', @EndDate = '2018/03/18', @SortExpression = 'EmployeeDayOffID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'                                         
  
   
  --SELECT * FROM @TempGetEmpoyeePreferences                                            
  DECLARE @Item1 VARCHAR(max),            
    @Item2 VARCHAR(max),            
    @Item3 VARCHAR(max),            
    @Item4 VARCHAR(max);            
            
  SELECT @Item1 = splitdata            
  FROM dbo.fnSplitString(@SortIndexArray, ',')            
  WHERE ROWID = 1            
            
  SELECT @Item2 = splitdata            
  FROM dbo.fnSplitString(@SortIndexArray, ',')            
  WHERE ROWID = 2            
            
  SELECT @Item3 = splitdata            
  FROM dbo.fnSplitString(@SortIndexArray, ',')            
  WHERE ROWID = 3            
            
  SELECT @Item4 = splitdata            
  FROM dbo.fnSplitString(@SortIndexArray, ',')            
  WHERE ROWID = 4            
            
  PRINT @StartDate;            
  PRINT @EndDate;;            
      
  SELECT DISTINCT EmployeeId INTO #TempEmpIds FROM @TempGetEmpoyeePreferences;    
    
  SELECT *    
  INTO #TempScheduleMasters     
  FROM dbo.ScheduleMasters SM WITH (NOLOCK)     
  WHERE SM.EmployeeID IN (SELECT NEmpId.EmployeeId FROM #TempEmpIds  NEmpId WITH (NOLOCK) )        
        AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted = 0            
        AND SM.ReferralID != @ReferralID    
  -- the below condtion is added to provide boundry for checking employee is already scheduled with other patient      
  -- 28 Feb 2021, issue found for client GEC, employee [IKUSEBIALA, TEMITOPE] is not available to assign for patient [MOROCHO, DANIEL]    
  -- 08 Apr 2021, issue found for client Pathmedic, employee [CAMPOS, MAYRA] is not available to assign for patient [CRUZ, JUAN]    
  -- START    
        AND Convert(DATE, SM.StartDate) BETWEEN Convert(DATE, @StartDate) AND Convert(DATE, @EndDate)    
  -- END    
        AND (            
        SM.EmployeeTSDateID IS NOT NULL            
        AND SM.ReferralTSDateID IS NOT NULL    
  );            
      
  SELECT *     
  INTO #TempEmployeeTimeSlotDates     
  FROM EmployeeTimeSlotDates ETD WITH (NOLOCK)            
  WHERE ETD.EmployeeTSDateID IN (SELECT SM.EmployeeTSDateID  FROM #TempScheduleMasters SM WITH (NOLOCK))         
              AND (            
                (            
                  @SameDateWithTimeSlot = 0            
                  AND ETD.EmployeeTSDate BETWEEN Convert(DATE, @StartDate)            
                    AND Convert(DATE, @EndDate)            
                  )            
                OR (            
                  @SameDateWithTimeSlot = 1            
                  AND (            
                    Convert(DATE, @StartDate) BETWEEN ETD.EmployeeTSStartTime            
                      AND ETD.EmployeeTSEndTime            
                    OR Convert(DATE, @EndDate) BETWEEN ETD.EmployeeTSStartTime            
                      AND ETD.EmployeeTSEndTime            
                    )            
                  )            
                ) ;           
    
  WITH CTEEmployeeTSList            
  AS (            
    SELECT *,            
      COUNT(t1.EmployeeID) OVER () AS Count            
    FROM (            
      SELECT ROW_NUMBER() OVER (            
          ORDER BY CASE             
              WHEN 'Skills ASC' = @Item1            
                THEN TBL1.SkillsMatchPercent            
              END ASC,            
            CASE             
              WHEN @Item1 = 'Skills DESC'            
                THEN TBL1.SkillsMatchPercent            
              END DESC,            
            CASE             
              WHEN 'Preferences ASC' = @Item1            
                THEN TBL1.PreferencesMatchPercent            
              END ASC,            
            CASE             
              WHEN 'Preferences DESC' = @Item1            
                THEN TBL1.PreferencesMatchPercent            
              END DESC,            
            CASE             
              WHEN 'Miles ASC' = @Item1            
                THEN TBL1.Distance            
              END ASC,            
            CASE             
              WHEN 'Miles DESC' = @Item1            
                THEN TBL1.Distance        
              END DESC,            
            CASE             
              WHEN 'Conflicts ASC' = @Item1            
                THEN TBL1.Conflicts            
              END ASC,            
      CASE             
              WHEN 'Conflicts DESC' = @Item1            
                THEN TBL1.Conflicts            
              END DESC,            
            CASE             
              WHEN 'Skills ASC' = @Item2            
                THEN TBL1.SkillsMatchPercent            
              END ASC,            
            CASE             
              WHEN @Item2 = 'Skills DESC'            
                THEN TBL1.SkillsMatchPercent            
              END DESC,            
            CASE             
              WHEN 'Preferences ASC' = @Item2            
                THEN TBL1.PreferencesMatchPercent            
              END ASC,            
            CASE             
              WHEN 'Preferences DESC' = @Item2            
                THEN TBL1.PreferencesMatchPercent            
              END DESC,            
            CASE             
              WHEN 'Miles ASC' = @Item2            
                THEN TBL1.Distance            
              END ASC,            
            CASE             
              WHEN 'Miles DESC' = @Item2            
                THEN TBL1.Distance            
              END DESC,            
            CASE             
              WHEN 'Conflicts ASC' = @Item2            
                THEN TBL1.Conflicts            
              END ASC,            
            CASE             
              WHEN 'Conflicts DESC' = @Item2            
                THEN TBL1.Conflicts            
              END DESC,            
            CASE             
              WHEN 'Skills ASC' = @Item3            
                THEN TBL1.SkillsMatchPercent            
              END ASC,            
            CASE             
              WHEN @Item3 = 'Skills DESC'            
                THEN TBL1.SkillsMatchPercent            
              END DESC,            
            CASE             
              WHEN 'Preferences ASC' = @Item3            
                THEN TBL1.PreferencesMatchPercent            
              END ASC,            
            CASE             
              WHEN 'Preferences DESC' = @Item3            
                THEN TBL1.PreferencesMatchPercent            
              END DESC,            
            CASE             
              WHEN 'Miles ASC' = @Item3            
                THEN TBL1.Distance            
              END ASC,            
            CASE             
              WHEN 'Miles DESC' = @Item3            
                THEN TBL1.Distance            
              END DESC,            
            CASE             
              WHEN 'Conflicts ASC' = @Item3            
                THEN TBL1.Conflicts            
              END ASC,            
            CASE             
              WHEN 'Conflicts DESC' = @Item3            
                THEN TBL1.Conflicts            
              END DESC,            
            CASE             
              WHEN 'Skills ASC' = @Item4            
                THEN TBL1.SkillsMatchPercent            
              END ASC,            
            CASE             
              WHEN @Item4 = 'Skills DESC'            
                THEN TBL1.SkillsMatchPercent            
              END DESC,            
            CASE             
              WHEN 'Preferences ASC' = @Item4            
                THEN TBL1.PreferencesMatchPercent            
              END ASC,            
            CASE             
              WHEN 'Preferences DESC' = @Item4            
                THEN TBL1.PreferencesMatchPercent            
              END DESC,            
            CASE             
              WHEN 'Miles ASC' = @Item4            
                THEN TBL1.Distance            
              END ASC,            
            CASE             
              WHEN 'Miles DESC' = @Item4            
                THEN TBL1.Distance            
              END DESC,            
            CASE             
              WHEN 'Conflicts ASC' = @Item4            
                THEN TBL1.Conflicts            
              END ASC,            
            CASE             
              WHEN 'Conflicts DESC' = @Item4            
                THEN TBL1.Conflicts            
             END DESC,            
            CASE             
       WHEN @SortType = 'ASC'            
                THEN CASE             
                    WHEN @SortExpression = 'Employee'            
                      THEN TBL1.FirstName            
                    END            
              END ASC,            
            CASE             
              WHEN @SortType = 'DESC'            
                THEN CASE             
                    WHEN @SortExpression = 'Employee'            
                      THEN TBL1.FirstName            
                    END            
              END DESC,            
            CASE             
              WHEN @SortType = 'ASC'            
                THEN CASE             
                    WHEN @SortExpression = 'Frequency'            
                      THEN TBL1.Frequency            
                    END            
              END ASC,            
            CASE             
              WHEN @SortType = 'DESC'            
                THEN CASE             
                    WHEN @SortExpression = 'Frequency'            
                      THEN TBL1.Frequency            
                    END            
              END DESC--,            
            --CASE             
            --  WHEN @SortType = 'ASC'            
            --    THEN CASE             
            --        WHEN @SortExpression = 'StartTime'            
            --          THEN TBL1.StartTime            
            --        END            
            --  END ASC,            
            --CASE             
            --  WHEN @SortType = 'DESC'            
            --    THEN CASE             
            --        WHEN @SortExpression = 'StartTime'            
            --          THEN TBL1.StartTime            
            --        END            
            --  END DESC,            
      --CASE             
      --        WHEN @SortType = 'ASC'            
      --          THEN CASE             
      --              WHEN @SortExpression = 'EndTime'            
      --                THEN TBL1.EndTime            
      --              END            
      --        END ASC,            
      --      CASE             
      --        WHEN @SortType = 'DESC'            
      --          THEN CASE             
      --              WHEN @SortExpression = 'EndTime'            
      --                THEN TBL1.EndTime            
      --              END            
      --        END DESC            
          ) AS Row,            
        *            
      FROM (            
        SELECT *            
        FROM (            
          SELECT E.*,            
            Distance = CASE             
              WHEN EmpLatLong IS NULL            
                OR C.Latitude IS NULL            
                OR C.Longitude IS NULL            
                THEN NULL            
              ELSE (            
                  EmpLatLong.STDistance([dbo].GetGeoFromLatLng(C.Latitude, C.            
                      Longitude)) * 0.000621371            
                  )            
              END,            
            --  RefLatLong=[dbo].GetGeoFromLatLng(C.Latitude,C.Longitude),                                            
            SUM(CASE             
                WHEN ETD.EmployeeTSDateID IS NOT NULL            
                  THEN 1            
                ELSE 0            
                END) OVER (            
              PARTITION BY E.EmployeeID,            
              EmployeeTimeSlotDetailIDs            
              ) AS Conflicts,            
            DENSE_RANK() OVER (            
              PARTITION BY E.EmployeeID ORDER BY SM.ScheduleID ASC            
              ) AS EmployeeRank,            
            SM.ScheduleID,            
            ETD.EmployeeTSDateID            
          FROM @TempGetEmpoyeePreferences E            
          LEFT JOIN #TempScheduleMasters SM WITH (NOLOCK)           
            ON SM.EmployeeID = E.EmployeeID            
   left join ReferralTimeSlotDates RTSD WITH (NOLOCK) on sm.ReferralTSDateID=rtsd.ReferralTSDateID          
   LEFT join ReferralTimeSlotDetails RTSDetail WITH (NOLOCK) on RTSDetail.ReferralTimeSlotDetailID=RTSD.ReferralTimeSlotDetailID           
    LEFT JOIN #TempEmployeeTimeSlotDates ETD WITH (NOLOCK) ON ETD.EmployeeTSDateID = SM.EmployeeTSDateID           
     AND ISNULL(SM.AnyTimeClockIn, 0) = 0            
              AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted = 0            
                          
          LEFT JOIN ContactMappings CM WITH (NOLOCK)           
            ON CM.ReferralID = E.ReferralID            
              AND CM.ContactTypeID = 1            
          LEFT JOIN Contacts C WITH (NOLOCK)           
            ON C.ContactID = CM.ContactID         
    WHERE      
      ISNULL(RTSDetail.IsDeleted, 0) = 0       
          ) AS TEMP            
        WHERE 1 = 1            
          AND EmployeeRank = 1            
          AND (            
            @MileRadius IS NULL            
            OR Distance < @MileRadius            
            )            
          --ORDER BY ReferralID DESC,Distance ASC,SkillsMatchCount ASC, PreferencesMatchCount DESC, FirstName ASC                                            
        ) AS TBL1            
      ) AS t1            
    )            
  SELECT *            
  FROM CTEEmployeeTSList WITH (NOLOCK)            
  WHERE ROW BETWEEN ((@PageSize * (@FromIndex - 1)) + 1            
        )            
      AND (@PageSize * @FromIndex)            
          
  /******Kundan Kumar Rai******              
Added new collection for ReferralBillingAuthorizations              
*******END******************/            
  --SELECT ReferralBillingAuthorizationID,            
  --  (ra.AuthorizationCode + ' - ' + s.ServiceCode) AS             
  --  ReferralBillingAuthorizationName            
  --FROM ReferralBillingAuthorizations ra WITH (NOLOCK)           
  --INNER JOIN ServiceCodes s WITH (NOLOCK)           
  --  ON ra.ServiceCodeID = s.ServiceCodeID            
  --WHERE ra.ReferralID = @ReferralID            
  --  AND ra.IsDeleted = 0             
  --  --AND (@CareTypeID IS NULL OR @CareTypeID = 0 OR ra.CareType = @CareTypeID)              
  --  AND ra.CareType = @CareTypeID            
  --  AND ra.EndDate >= GETDATE();    
    
--By Akhilesh 02/03/2022--    
  SELECT           
  DISTINCT              
  ReferralBillingAuthorizationID,              
  RBA.ReferralID,              
  AuthorizationCode AS ReferralBillingAuthorizationName,              
  StartDate,              
  EndDate,              
  AllowedTime,              
  RBA.UnitType,              
  RBA.MaxUnit,              
  RBA.DailyUnitLimit,              
  UnitLimitFrequency,            
  SC.ServiceCode,            
  CareType = DM.Title            
 FROM              
  ReferralBillingAuthorizations RBA              
  INNER JOIN ReferralPayorMappings RPM ON RBA.ReferralID = RPM.ReferralID AND RBA.PayorID = RPM.PayorID              
  INNER JOIN ServiceCodes SC ON SC.ServiceCodeID = RBA.ServiceCodeID            
  INNER JOIN DDMaster DM on RBA.CareType = DM.DDMasterID    
   WHERE RBA.ReferralID = @ReferralID              
    AND RBA.IsDeleted = 0               
    --AND (@CareTypeID IS NULL OR @CareTypeID = 0 OR ra.CareType = @CareTypeID)                
    AND RBA.CareType = @CareTypeID              
    AND RBA.EndDate >= GETDATE();     
        
 DROP TABLE #TempReferralTimeSlotMaster    
 DROP TABLE #TempReferralTimeSlotDetails    
 DROP TABLE #TempReferralTimeSlotDates    
 DROP TABLE #TempScheduleMasters    
 DROP TABLE #TempEmployeeTimeSlotDates    
 DROP TABLE #TempEmpIds    
     
  END 