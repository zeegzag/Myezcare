CREATE PROCEDURE [dbo].[API_GetPendingScheduleClockInDetails]
@EmployeeID BIGINT,
@ReferralID BIGINT,
@Time DATETIME
AS
BEGIN

SELECT * FROM PendingScheduleID PS
 WHERE PS.EmployeeID=@EmployeeID AND PS.ReferralID=@ReferralID AND  PS.ClockInTime IS NULL AND CONVERT(DATE,@Time)= CONVERT(DATE,PS.ClockInTime)

END
