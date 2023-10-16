--EXEC API_CreatePendingScheduleClockInOut @EmployeeID = N'944', @ReferralID = N'3930', @Time = N'2018-11-02 03:37:52 PM', @IsClockIN = N'True'
CREATE PROCEDURE [dbo].[API_CreatePendingScheduleClockInOut]  
@EmployeeID BIGINT,  
@ReferralID BIGINT,  
@Time DATETIME,  
@IsClockIN BIT,  
@PendingScheduleID BIGINT=0,  
@loggedInID BIGINT=1  
AS  
BEGIN  
  
IF(@IsClockIN=1)  
BEGIN  
   
   
 SELECT @PendingScheduleID=PS.PendingScheduleID FROM PendingSchedules PS  
 WHERE PS.EmployeeID=@EmployeeID AND PS.ReferralID=@ReferralID AND  PS.ClockOutTime IS NULL AND PS.ClockInTime IS NOT NULL AND  
 CONVERT(DATE,@Time)= CONVERT(DATE,PS.ClockInTime)  
  
  
 IF(@PendingScheduleID=0)  
 INSERT INTO PendingSchedules(ReferralID,EmployeeID,ClockInTime,ClockOutTime,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID)  
 SELECT @ReferralID, @EmployeeID, @Time, NULL, @loggedInID, GETUTCDATE(), @loggedInID, GETUTCDATE(), NULL   
 ELSE   
 UPDATE PendingSchedules SET ClockInTime=@Time, UpdatedBy=@loggedInID, UpdatedDate=GETUTCDATE() WHERE PendingScheduleID=@PendingScheduleID  
  
END  
ELSE  
BEGIN  
   
 UPDATE PendingSchedules SET ClockOutTime=@Time, UpdatedBy=@loggedInID, UpdatedDate=GETUTCDATE()   
 WHERE EmployeeID=@EmployeeID AND ReferralID=@ReferralID AND  ClockInTime IS NOT NULL AND CONVERT(DATE,@Time)= CONVERT(DATE,ClockInTime)  
  
END  
  
  
END