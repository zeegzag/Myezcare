-- exec GetScheduleEmail 30,7                  
CREATE Procedure [dbo].[GetScheduleBatchServiceForProcess]                	
@GETScheduleBatchServiceStatus  varchar(50),  -- Initiated
@SETScheduleBatchServiceStatus  varchar(50)   -- InProgress
as 
BEGIN
select * from ScheduleBatchServices where ScheduleBatchServiceStatus = @GETScheduleBatchServiceStatus 
UPDATE ScheduleBatchServices SET ScheduleBatchServiceStatus=@SETScheduleBatchServiceStatus WHERE ScheduleBatchServiceStatus = @GETScheduleBatchServiceStatus
END

