CREATE PROCEDURE [dbo].[API_CheckClockOut]  
 @ScheduleID BIGINT,  
 @ClockOutTime DateTime,  
 @ClockOutBeforeTime INT  
AS                            
BEGIN  
 DECLARE @Diff BIGINT;  
  
 Select @Diff=datediff(mi, @ClockOutTime, (SELECT EndDate FROM ScheduleMasters WHERE ScheduleID=@ScheduleID AND IsDeleted=0))  
  
 IF(@Diff >= @ClockOutBeforeTime)  
  Select 1  
 ELSE  
  Select 0  
END
