-- EXEC HC_ProcessPendingSchedule @PendingScheduleID = '21', @RoleID = '1', @ApprovalRequiredIVRBypassPermission = 'Mobile_ApprovalRequired_IVR_Bypass_ClockInOut', @BypassAction = 'Pending', @loggedInID = '1', @SystemID = '74D435D13E71'
CREATE PROCEDURE [dbo].[HC_ProcessPendingSchedule]  
 @PendingScheduleID BIGINT=0,
 @RoleID BIGINT,
 @ApprovalRequiredIVRBypassPermission VARCHAR(100),
 @BypassAction INT,
 @loggedInID BIGINT,  
 @SystemID VARCHAR(100)  
AS                  
BEGIN                  
  
BEGIN TRY  
  BEGIN TRAN PS_Tran  
    
  
DECLARE @EffectiveEndDate DATE='2099-12-31';  
  
  
IF(@PendingScheduleID=0)  
BEGIN  
SELECT -1   
ROLLBACK TRAN PS_Tran;  
RETURN;  
END  
  
  
DECLARE @ReferralID BIGINT;  
DECLARE @EmployeeID BIGINT;  
DECLARE @DayNumber INT;  
DECLARE @ClockInTime DATETIME;  
DECLARE @ClockOutTime DATETIME;  
DECLARE @TempRTDTable TABLE (ReferralTSDateID BIGINT, ReferralID BIGINT,ReferralTSDate DATE, ReferralTSStartTime DATETIME, ReferralTSEndTime DATETIME)  
DECLARE @ExistingInBoundReferralTSDateID BIGINT;  
DECLARE @ExistingOutBoundReferralTSDateID BIGINT;
DECLARE @IsApprovalRequired BIT = 0;  
  
  
DECLARE @TempETDTable TABLE (EmployeeTSDateID BIGINT, EmployeeID BIGINT, EmployeeTSDate DATE, EmployeeTSStartTime DATETIME, EmployeeTSEndTime DATETIME)  
DECLARE @ExistingInBoundEmployeeTSDateID BIGINT;  
DECLARE @ExistingOutBoundEmployeeTSDateID BIGINT;  
  
  
SELECT @ReferralID=ReferralID, @EmployeeID=EmployeeID,@DayNumber=DayNumber, @ClockInTime=ClockInTime, @ClockOutTime=ClockOutTime FROM PendingSchedules WHERE PendingScheduleID=@PendingScheduleID AND ScheduleID IS NULL  
  
IF(@ReferralID IS NULL)  
BEGIN  
SELECT -4;  
ROLLBACK TRAN PS_Tran;  
RETURN;  
END  
  
-- CHECK FOR Patient TImeSlots  
INSERT INTO @TempRTDTable  
SELECT RTD.ReferralTSDateID, RTD.ReferralID , RTD.ReferralTSDate, RTD.ReferralTSStartTime, RTD.ReferralTSEndTime  
FROM ReferralTimeSlotDates RTD  
INNER JOIN ReferralTimeSlotDetails RTSD ON RTSD.ReferralTimeSlotDetailID=RTD.ReferralTimeSlotDetailID  
INNER JOIN ReferralTimeSlotMaster RTM ON RTM.ReferralTimeSlotMasterID=RTSD.ReferralTimeSlotMasterID  
WHERE RTSD.IsDeleted=0 AND RTM.IsDeleted=0 AND RTD.ReferralID=@ReferralID AND RTD.ReferralTSDate BETWEEN CONVERT(DATE,@ClockInTime) AND CONVERT(DATE,@ClockOutTime)  
  
SELECT TOP 1 @ExistingInBoundReferralTSDateID=T.ReferralTSDateID FROM @TempRTDTable T  
WHERE @ClockInTime BETWEEN T.ReferralTSStartTime AND T.ReferralTSEndTime  
AND @ClockOutTime BETWEEN T.ReferralTSStartTime AND T.ReferralTSEndTime  
  
IF(@ExistingInBoundReferralTSDateID IS NOT NULL)  
BEGIN  
  
  IF EXISTS (SELECT * FROM ScheduleMasters SM   
  INNER JOIN EmployeeVisits EV ON EV.ScheduleID=SM.ScheduleID  
  WHERE SM.ReferralTSDateID=@ExistingInBoundReferralTSDateID)  
  BEGIN  
  SELECT -2;   
  ROLLBACK TRAN PS_Tran;  
  RETURN;  
  END  
  DELETE SM FROM ScheduleMasters SM WHERE SM.ReferralTSDateID=@ExistingInBoundReferralTSDateID   
  --UPDATE ScheduleMasters SET IsDeleted=1 WHERE ReferralTSDateID=@ExistingInBoundReferralTSDateID   
  
END  
ELSE  
BEGIN   
  
  SELECT TOP 1 @ExistingOutBoundReferralTSDateID=T.ReferralTSDateID FROM @TempRTDTable T  
WHERE T.ReferralTSStartTime BETWEEN @ClockInTime  AND @ClockOutTime  
AND T.ReferralTSEndTime BETWEEN @ClockInTime  AND @ClockOutTime  
  IF EXISTS (SELECT * FROM ScheduleMasters SM   
  INNER JOIN EmployeeVisits EV ON EV.ScheduleID=SM.ScheduleID  
  WHERE SM.ReferralTSDateID=@ExistingOutBoundReferralTSDateID)  
  BEGIN  
  SELECT -2;   
  ROLLBACK TRAN PS_Tran;  
  RETURN;  
  END  
  DELETE SM FROM ScheduleMasters SM WHERE SM.ReferralTSDateID=@ExistingOutBoundReferralTSDateID   
  --UPDATE ScheduleMasters SET IsDeleted=1 WHERE ReferralTSDateID=@ExistingOutBoundReferralTSDateID   
  
END  
  
IF NOT EXISTS(  
SELECT TOP 1 RPM.PayorID FROM Referrals R   
INNER JOIN ReferralPayorMappings RPM ON RPM.ReferralID=R.ReferralID AND RPM.Precedence=1   
AND CONVERT(DATE,@ClockInTime) BETWEEN RPM.PayorEffectiveDate AND ISNULL(PayorEffectiveEndDate,@EffectiveEndDate)  
WHERE R.ReferralID=@ReferralID  
)  
BEGIN  
  SELECT -3;  
  ROLLBACK TRAN PS_Tran;  
  RETURN;  
END  
  
-- EXEC HC_ProcessPendingSchedule 1,1,'systemid'  
  
  
  
  
  
  
  
  
-- CHECK FOR Patient TImeSlots  
INSERT INTO @TempETDTable  
SELECT ETD.EmployeeTSDateID, ETD.EmployeeID , ETD.EmployeeTSDate, ETD.EmployeeTSStartTime, ETD.EmployeeTSEndTime  
FROM EmployeeTimeSlotDates ETD  
INNER JOIN EmployeeTimeSlotDetails ETSD ON ETSD.EmployeeTimeSlotDetailID=ETD.EmployeeTimeSlotDetailID  
INNER JOIN EmployeeTimeSlotMaster ETM ON ETM.EmployeeTimeSlotMasterID=ETSD.EmployeeTimeSlotMasterID  
WHERE ETSD.IsDeleted=0 AND ETM.IsDeleted=0 AND ETD.EmployeeID=@EmployeeID AND ETD.EmployeeTSDate BETWEEN CONVERT(DATE,@ClockInTime) AND CONVERT(DATE,@ClockOutTime)  
  
SELECT TOP 1 @ExistingInBoundEmployeeTSDateID=T.EmployeeTSDateID FROM @TempETDTable T  
WHERE @ClockInTime BETWEEN T.EmployeeTSStartTime AND T.EmployeeTSEndTime  
AND @ClockOutTime BETWEEN T.EmployeeTSStartTime AND T.EmployeeTSEndTime  
  
IF(@ExistingInBoundEmployeeTSDateID IS NOT NULL)  
BEGIN  
  
  IF EXISTS (SELECT * FROM ScheduleMasters SM   
  INNER JOIN EmployeeVisits EV ON EV.ScheduleID=SM.ScheduleID  
  WHERE SM.EmployeeTSDateID=@ExistingInBoundEmployeeTSDateID)  
  BEGIN  
  SELECT -5;   
  ROLLBACK TRAN PS_Tran;  
  RETURN;  
  END  
  DELETE SM FROM ScheduleMasters SM WHERE SM.EmployeeTSDateID=@ExistingInBoundEmployeeTSDateID   
  --UPDATE ScheduleMasters SET IsDeleted=1 WHERE ReferralTSDateID=@ExistingInBoundReferralTSDateID   
  
END  
ELSE  
BEGIN   
  
  
  SELECT TOP 1 @ExistingOutBoundEmployeeTSDateID=T.EmployeeTSDateID FROM @TempETDTable T  
  WHERE T.EmployeeTSStartTime BETWEEN @ClockInTime  AND @ClockOutTime  
  AND T.EmployeeTSEndTime BETWEEN @ClockInTime  AND @ClockOutTime  
    
  IF EXISTS (SELECT * FROM ScheduleMasters SM   
  INNER JOIN EmployeeVisits EV ON EV.ScheduleID=SM.ScheduleID  
  WHERE SM.EmployeeTSDateID=@ExistingOutBoundEmployeeTSDateID)  
  BEGIN  
  SELECT -5;   
  ROLLBACK TRAN PS_Tran;  
  RETURN;  
  END  
  
  DELETE SM FROM ScheduleMasters SM WHERE SM.EmployeeTSDateID=@ExistingOutBoundEmployeeTSDateID   
  --UPDATE ScheduleMasters SET IsDeleted=1 WHERE ReferralTSDateID=@ExistingOutBoundReferralTSDateID   
  
END  
  
  
INSERT INTO ScheduleMasters(ReferralID,StartDate,EndDate,ScheduleStatusID,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,IsDeleted,EmployeeID,PayorID)  
SELECT R.ReferralID,PS.ClockInTime,PS.ClockOutTime,2, @loggedInID,GETUTCDATE(), @loggedInID,GETUTCDATE(),@SystemID,0,PS.EmployeeID,RPM.PayorID FROM PendingSchedules PS  
INNER JOIN Referrals R ON R.ReferralID=PS.ReferralID AND PS.PendingScheduleID=@PendingScheduleID  
INNER JOIN ReferralPayorMappings RPM ON RPM.ReferralID=R.ReferralID AND RPM.Precedence=1   
AND CONVERT(DATE,@ClockInTime) BETWEEN RPM.PayorEffectiveDate AND ISNULL(PayorEffectiveEndDate,@EffectiveEndDate)  
WHERE PS.PendingScheduleID=@PendingScheduleID  
  
DECLARE @ScheduleID BIGINT=@@IDENTITY;  
UPDATE PendingSchedules SET ScheduleID=@ScheduleID WHERE PendingScheduleID=@PendingScheduleID  

--Code for set as approval required visit
 IF EXISTS(SELECT P.PermissionID FROM RolePermissionMapping RPM
 INNER JOIN Permissions P ON P.PermissionID=RPM.PermissionID  
 WHERE RPM.RoleID=@RoleID AND P.PermissionCode=@ApprovalRequiredIVRBypassPermission AND RPM.IsDeleted=0)
	SET @IsApprovalRequired=1;
 ELSE
	SET @BypassAction=NULL;

INSERT INTO EmployeeVisits(ScheduleID,ClockInTime,ClockOutTime,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,SurveyCompleted,IsApprovalRequired,ActionTaken)  
VALUES(@ScheduleID,@ClockInTime,@ClockOutTime,0,GETUTCDATE(),@loggedInID,GETUTCDATE(),@loggedInID,@SystemID,0,@IsApprovalRequired,@BypassAction)
  
SELECT 1   
COMMIT TRAN PS_Tran  
RETURN;  
  
  
END TRY  
BEGIN CATCH  
    IF(@@TRANCOUNT > 0)ROLLBACK TRAN PS_Tran;  
    SELECT -6 RETURN;  
END CATCH  
  
  
  
  
  
  
                     
END
