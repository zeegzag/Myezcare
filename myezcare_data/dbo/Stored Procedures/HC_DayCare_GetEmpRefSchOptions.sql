-- EXEC GetEmpRefSchOptions @ReferralID = '1951', @ScheduleID = '57837', @StartDate = '2018/03/05', @EndDate = '2018/03/25', @SortExpression = 'EmployeeDayOffID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'              
CREATE PROCEDURE [dbo].[HC_DayCare_GetEmpRefSchOptions]    
 @ReferralID BIGINT = 0,                                                 
 @ScheduleID BIGINT = 0,                                
 @StartDate DATETIME,              
 @EndDate DATETIME,                
 @SameDateWithTimeSlot BIT    
AS                                
BEGIN      

DECLARE @ReferralFacility BIGINT;
DECLARE @ReferralPayor BIGINT;

SELECT @ReferralFacility=R.DefaultFacilityID, @ReferralPayor=RPM.PayorID FROM Referrals R
LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID=R.ReferralID AND RPM.IsActive=1 AND RPM.IsDeleted=0 AND RPM.Precedence=1 
AND @StartDate BETWEEN RPM.PayorEffectiveDate AND RPM.PayorEffectiveEndDate 
WHERE R.ReferralID=@ReferralID




               
    
SELECT FacilityID,FacilityName FROM Facilities F WHERE IsDeleted=0  ORDER BY  F.FacilityName ASC          
                  
SELECT P.PayorName,P.ShortName, P.PayorID, RPM.Precedence FROM ReferralPayorMappings RPM          
INNER JOIN Payors P ON P.PayorID=RPM.PayorID          
WHERE RPM.ReferralID=@ReferralID AND  RPM.Precedence IS NOT NULL AND RPM.IsDeleted=0          
AND  CONVERT(DATE,@StartDate) BETWEEN RPM.PayorEffectiveDate AND RPM.PayorEffectiveEndDate      
ORDER BY RPM.Precedence ASC      
    
    

IF(@ScheduleID>0)
SELECT ReferralTSDateID,ScheduleID,PayorID,FacilityID FROM ScheduleMasters WHERE ScheduleID=@ScheduleID      
ELSE
SELECT ReferralTSDateID=0,ScheduleID=0,PayorID= @ReferralPayor,FacilityID=@ReferralFacility

          
                
SELECT RH.*, CreatedBy=dbo.GetGeneralNameFormat(EC.FirstName,EC.LastName), UpdatedBy=dbo.GetGeneralNameFormat(EU.FirstName,EU.LastName),             
CurrentActiveGroup = CASE WHEN GETDATE() BETWEEN RH.StartDate AND RH.EndDate THEN 1 ELSE 0 END,            
OldActiveGroup = CASE WHEN GETDATE() > RH.EndDate THEN 1 ELSE 0 END            
FROM ReferralOnHoldDetails RH            
INNER JOIN Employees EC ON EC.EmployeeID=RH.CreatedBy            
INNER JOIN Employees EU ON EU.EmployeeID=RH.UpdatedBy            
WHERE RH.IsDeleted=0  AND RH.ReferralID=@ReferralID-- AND (GETDATE() BETWEEN RH.StartDate AND RH.EndDate OR GETDATE() < RH.StartDate)            
ORDER BY StartDate DESC              
            
             
              
DECLARE @DayNumber INT;              
IF(@SameDateWithTimeSlot=1)              
  SELECT @DayNumber= [dbo].[GetWeekDayNumberFromDate](@StartDate);              
              
DECLARE @InfiniteEndDate DATE='2099-12-31';              
DECLARE @PatientPayorID BIGINT = 0;          
DECLARE @ReferralTSDateID BIGINT=0;          
        
SELECT @ReferralTSDateID=ReferralTSDateID, @PatientPayorID=PayorID FROM ScheduleMasters WHERE ScheduleID=@ScheduleID              
              
DECLARE @ReferralTimeSlotMasterID BIGINT=0;              
DECLARE @ReferralTimeSlotDetailID BIGINT=0;              
              
IF(@ReferralTSDateID IS NULL OR @ReferralTSDateID=0)              
BEGIN              
              
 SELECT TOP 1 @ReferralTimeSlotMasterID=ReferralTimeSlotMasterID FROM ReferralTimeSlotMaster WHERE ReferralID=@ReferralID AND @StartDate BETWEEN StartDate AND ISNULL (EndDate,@InfiniteEndDate)         
              
 SELECT ReferralID, FirstName, LastName, PatientPayorID=@PatientPayorID FROM Referrals WHERE ReferralID=@ReferralID              
 SELECT TOP 1 * FROM ReferralTimeSlotMaster  RTS WHERE RTS.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID              
              
 SELECT DISTINCT RTSD.* ,              
 RemainingSlotCount= SUM(CASE WHEN SM.ScheduleID IS NULL THEN 1 ELSE 0 END) OVER(PARTITION BY RTSD.ReferralTimeSlotMasterID,RTSD.ReferralTimeSlotDetailID, RTSD.DAY)              
 FROM ReferralTimeSlotDetails RTSD              
 INNER JOIN ReferralTimeSlotDates RTD ON RTD.ReferralTimeSlotDetailID=RTSD.ReferralTimeSlotDetailID              
 AND (@DayNumber IS NULL OR @DayNumber=0 OR (@DayNumber=RTSD.Day AND CONVERT(TIME,@StartDate) BETWEEN RTSD.StartTime AND RTSD.EndTime))              
 AND RTD.ReferralTSDate BETWEEN CONVERT(DATE,@StartDate) AND CONVERT(DATE,@EndDate) AND RTD.UsedInScheduling=1              
 LEFT JOIN ScheduleMasters SM ON SM.ReferralTSDateID=RTD.ReferralTSDateID AND SM.IsDeleted=0              
 WHERE RTSD.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID AND RTSD.IsDeleted=0 AND               
 (@DayNumber IS NULL OR @DayNumber=0 OR (@DayNumber=RTSD.Day AND CONVERT(TIME,@StartDate) BETWEEN RTSD.StartTime AND RTSD.EndTime))        
ORDER BY DAY ASC,StartTime ASC              
              
              
              
              
END              
ELSE              
BEGIN              
              
  --SELECT * FROM ScheduleMasters WHERE ScheduleID=57829              
  --SELECT * FROM ReferralTimeSlotDates WHERE ReferralTSDateID=61              
  -- SELECT * FROM  ReferralTimeSlotMaster WHERE ReferralTimeSlotMasterID=1              
  SELECT @ReferralTimeSlotMasterID=ReferralTimeSlotMasterID,@ReferralTimeSlotDetailID=ReferralTimeSlotDetailID FROM ReferralTimeSlotDates WHERE ReferralTSDateID=@ReferralTSDateID              
              
  SELECT ReferralID, FirstName, LastName, PatientPayorID=@PatientPayorID FROM Referrals WHERE ReferralID=@ReferralID              
    
  SELECT TOP 1 * FROM ReferralTimeSlotMaster  RTS WHERE RTS.ReferralTimeSlotMasterID=@ReferralTimeSlotMasterID              
  --SELECT * FROM ReferralTimeSlotDates              
  SELECT DISTINCT RTSD.*,    
  RemainingSlotCount= SUM(CASE WHEN SM.ScheduleID IS NULL THEN 1 ELSE 0 END) OVER(PARTITION BY RTSD.ReferralTimeSlotMasterID,RTSD.ReferralTimeSlotDetailID, RTSD.DAY)    
  FROM ReferralTimeSlotDetails  RTSD     
  INNER JOIN ReferralTimeSlotDates RTD ON RTD.ReferralTimeSlotDetailID=RTSD.ReferralTimeSlotDetailID     
  AND (CONVERT(TIME,@StartDate) BETWEEN RTSD.StartTime AND RTSD.EndTime)              
 AND RTD.ReferralTSDate BETWEEN CONVERT(DATE,@StartDate) AND CONVERT(DATE,@EndDate) AND RTD.UsedInScheduling=1     
  LEFT JOIN ScheduleMasters SM ON SM.ReferralTSDateID=RTD.ReferralTSDateID AND SM.IsDeleted=0     
  WHERE RTSD.ReferralTimeSlotDetailID=@ReferralTimeSlotDetailID              
  ORDER BY DAY ASC,StartTime ASC              
              
  IF(@ReferralTimeSlotDetailID IS NULL)              
  SET @ReferralTimeSlotDetailID=0;              
               
               
END              
              
                    
                                
END
