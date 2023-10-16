  
CREATE PROCEDURE [dbo].[GetEmpRefSchOptions_PatientVisitFrequency_HC]                             
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
  
       DECLARE @temp TABLE (CareTypeIds VARCHAR(MAX));              
    DECLARE @IDs VARCHAR(MAX);              
              
    INSERT INTO @temp              
    SELECT STUFF(ISNULL(',' + T.CareTypeIds, '') + ISNULL(',' + T.SlotCareTypeIds, ''), 1, 1,               
     '') AS CareTypeIds              
    FROM (              
   SELECT CareTypeIds,              
     STUFF((              
      SELECT DISTINCT CONCAT(', ',RTD.CareTypeId)    
      FROM ReferralTimeslotdetails RTD WITH (NOLOCK)             
      INNER JOIN ReferralTimeSlotMaster RTM WITH (NOLOCK)             
     ON RTM.ReferralTimeSlotMasterID = RTD.ReferralTimeSlotMasterID              
      INNER JOIN ReferralTimeSlotDates RTDS WITH (NOLOCK)             
     ON RTDS.ReferralTimeSlotDetailID = RTD.ReferralTimeSlotDetailID              
      WHERE RTM.ReferralID = @ReferralID              
     AND RTDS.ReferralTSDate BETWEEN Convert(DATE, @StartDate)              
       AND Convert(DATE, @EndDate)              
      FOR XML PATH('')              
      ), 1, 1, '') AS SlotCareTypeIds              
   FROM Referrals WITH (NOLOCK)             
   WHERE ReferralId = @ReferralID              
   ) AS T              
              
    SELECT @IDs = CareTypeIds              
    FROM @temp              
              
    SELECT *              
    FROM DDMaster WITH (NOLOCK)             
    WHERE ItemType = @DDType_CareType              
   AND IsDeleted = 0              
   AND DDMasterID IN (              
     SELECT val              
     FROM GetCSVTable(@IDs)              
     )              
              
     SELECT P.PayorName,              
   P.ShortName,              
   P.PayorID,              
   RPM.Precedence              
    FROM ReferralPayorMappings RPM WITH (NOLOCK)             
    INNER JOIN Payors P WITH (NOLOCK)             
   ON P.PayorID = RPM.PayorID              
    WHERE RPM.ReferralID = @ReferralID              
   AND RPM.Precedence IS NOT NULL              
   AND RPM.IsDeleted = 0              
   --AND CONVERT(DATE, @StartDate) BETWEEN RPM.PayorEffectiveDate              
   --  AND RPM.PayorEffectiveEndDate              
    ORDER BY RPM.Precedence ASC              
              
  
  
END
GO


