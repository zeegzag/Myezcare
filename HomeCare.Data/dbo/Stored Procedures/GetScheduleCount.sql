CREATE procedure [dbo].[GetScheduleCount]    
--declare
@StartDate date
as    
select  COUNT(ScheduleID)as Counts from ScheduleMasters where ScheduleStatusID=2     
and IsDeleted=0   and StartDate=@StartDate