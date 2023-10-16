﻿CREATE PROCEDURE [dbo].[HC_PrivateDuty_OnHoldUnHoldAction]
@ReferralOnHoldDetailID BIGINT,        
@ReferralID BIGINT,        
@PatientOnHoldAction BIT=0,        
@PatientOnHoldReason NVARCHAR(1000),        
@StartDate DATE=NULL,        
@EndDate DATE=NULL,        
@loggedInId BIGINT,        
@SystemID NVARCHAR(MAX)        
AS        
BEGIN        
        
DECLARE @InfinateDate DATE='2099-12-31';        
        
IF(@PatientOnHoldAction=1)        
BEGIN        
 -- EXEC GenerateReferralTimeSlotDates @StartDate, @EndDate, @ReferralID         
        
 IF(@ReferralOnHoldDetailID=0)        
 BEGIN        
        
     IF EXISTS(        
  SELECT 1 FROM ReferralOnHoldDetails WHERE ReferralID=@ReferralID AND        
  ((StartDate BETWEEN @StartDate AND @EndDate OR ISNULL(EndDate,@InfinateDate) BETWEEN @StartDate AND @EndDate) OR         
  (@StartDate BETWEEN StartDate AND ISNULL(EndDate,@InfinateDate) OR @EndDate BETWEEN StartDate AND ISNULL(EndDate,@InfinateDate)))        
  )        
  BEGIN        
   SELECT -1 AS Result;     
   SELECT 0;    
   RETURN;        
  END        
        
        
  INSERT INTO ReferralOnHoldDetails SELECT @ReferralID, @StartDate, @EndDate, 0, GETDATE(), @loggedInId, GETDATE(), @loggedInId,@SystemID,@PatientOnHoldReason        
        
  --DECLARE @ReferralOnHoldDetailID BIGINT;        
  SET @ReferralOnHoldDetailID=@@IDENTITY;        
        
        
  --UPDATE SM SET SM.IsDeleted=1, SM.UpdatedBy=@loggedInId,SM.UpdatedDate=GETUTCDATE()    FROM ScheduleMasters SM        
  --INNER JOIN ReferralTimeSlotDates RTD ON RTD.ReferralTSDateID=SM.ReferralTSDateID        
  --WHERE SM.ReferralID=@ReferralID AND RTD.ReferralID=@ReferralID AND RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate        
        
  --WHERE SM.ReferralID=@ReferralID AND ( (SM.StartDate  BETWEEN @StartDate AND @EndDate) OR (SM.EndDate  BETWEEN @StartDate AND @EndDate) )        
         
  UPDATE RTD SET RTD.UsedInScheduling=0,RTD.Notes=@PatientOnHoldReason, RTD.OnHold=1, ReferralOnHoldDetailID=@ReferralOnHoldDetailID        
  FROM ReferralTimeSlotDates RTD        
  WHERE  RTD.ReferralID=@ReferralID AND RTD.ReferralTSDate BETWEEN @StartDate AND ISNULL(@EndDate,@InfinateDate) AND RTD.UsedInScheduling=1        
        
  SELECT 1 AS Result;  
  SELECT DISTINCT E.EmployeeID,E.FcmTokenId,E.DeviceType,SM.StartDate,SM.EndDate,PatientName=dbo.GetGeneralNameFormat(R.FirstName,R.LastName)  
  FROM ScheduleMasters SM    
  INNER JOIN ReferralTimeSlotDates RTD ON RTD.ReferralTSDateID=SM.ReferralTSDateID    
  INNER JOIN Employees E ON E.EmployeeID=SM.EmployeeID AND E.FcmTokenId IS NOT NULL  
  INNER JOIN Referrals R ON R.ReferralID=SM.ReferralID  
  WHERE SM.ReferralID=@ReferralID AND RTD.ReferralID=@ReferralID AND  RTD.ReferralTSDate BETWEEN @StartDate AND ISNULL(@EndDate,@InfinateDate) AND SM.IsDeleted=0;    
        
  UPDATE SM SET SM.IsDeleted=1, SM.UpdatedBy=@loggedInId,SM.UpdatedDate=GETUTCDATE()    FROM ScheduleMasters SM        
  INNER JOIN ReferralTimeSlotDates RTD ON RTD.ReferralTSDateID=SM.ReferralTSDateID        
  WHERE SM.ReferralID=@ReferralID AND RTD.ReferralID=@ReferralID AND  RTD.ReferralTSDate BETWEEN @StartDate AND ISNULL(@EndDate,@InfinateDate)    
      
      
  RETURN;    
    
 END        
 ELSE         
 BEGIN        
 -- EDIT FUNCATIO CODE WILL GO HERE    
   SELECT 1 AS Result;    
   SELECT 0;    
  RETURN;    
    
 END        
        
END        
ELSE IF(@PatientOnHoldAction=0 AND @ReferralOnHoldDetailID>0)        
BEGIN        
        
        
-- DELETE WILL WORK ONLY FOR CURRENT & FUTURE ON HOLD          
        
DECLARE @UnHoldDate DATE=GETDATE();        
        
DECLARE @CurrentActiveGroup BIT=0;           
SELECT   @CurrentActiveGroup= CASE WHEN @UnHoldDate BETWEEN RH.StartDate AND  ISNULL(RH.EndDate,@InfinateDate)  AND (@UnHoldDate!=RH.StartDate) THEN 1 ELSE 0 END        
FROM ReferralOnHoldDetails RH        
WHERE RH.ReferralOnHoldDetailID=@ReferralOnHoldDetailID        
        
IF(@CurrentActiveGroup=0)        
BEGIN        
        
   UPDATE RTD SET RTD.UsedInScheduling=1,RTD.Notes=NULL, RTD.OnHold=0,ReferralOnHoldDetailID=NULL         
   FROM ReferralTimeSlotDates RTD        
   WHERE  RTD.ReferralID=@ReferralID AND RTD.ReferralOnHoldDetailID=@ReferralOnHoldDetailID -- RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate        
        
   DELETE FROM ReferralOnHoldDetails WHERE ReferralOnHoldDetailID=@ReferralOnHoldDetailID        
        
   SELECT 1 AS Result;    
   SELECT 0;    
  RETURN;    
       
            
END        
ELSE        
BEGIN        
        
   UPDATE RTD SET RTD.UsedInScheduling=1,RTD.Notes=NULL, RTD.OnHold=0,ReferralOnHoldDetailID=NULL         
   FROM ReferralTimeSlotDates RTD        
   INNER JOIN ReferralOnHoldDetails RH ON RH.ReferralOnHoldDetailID=RTD.ReferralOnHoldDetailID        
   WHERE  RTD.ReferralID=@ReferralID AND RTD.ReferralOnHoldDetailID=@ReferralOnHoldDetailID AND RTD.ReferralTSDate BETWEEN @UnHoldDate AND  ISNULL(RH.EndDate,@InfinateDate)        
        
   UPDATE RH SET RH.EndDate= CASE WHEN  RH.StartDate = @UnHoldDate THEN RH.StartDate ELSE DATEADD(day, -1, @UnHoldDate) END , UpdatedDate= GETDATE(),UpdatedBy=@loggedInId FROM ReferralOnHoldDetails RH WHERE ReferralOnHoldDetailID=@ReferralOnHoldDetailID  
  
    
      
        
    SELECT 1 AS Result;    
   SELECT 0;    
  RETURN;    
END        
        
END         
ELSE         
 SELECT 1 AS Result;    
   SELECT 0;    
  RETURN;    
        
        
END
