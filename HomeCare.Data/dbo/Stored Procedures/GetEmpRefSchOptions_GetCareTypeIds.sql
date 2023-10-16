GO

-- Exec [GetEmpRefSchOptions_GetCareTypeIds] @ReferralID=4323,  @StartDate = '2021/09/27 00:00:00', @EndDAte = '2022/09/27 00:00:00',@DDType_CareType=1
CREATE PROCEDURE [dbo].[GetEmpRefSchOptions_GetCareTypeIds]           
  @ReferralID BIGINT = 0,        
  @StartDate DATETIME,        
  @EndDate DATETIME,        
  @DDType_CareType INT      
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
          SELECT DISTINCT ', ' + convert(VARCHAR(max), RTD.CareTypeId, 120)        
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

END