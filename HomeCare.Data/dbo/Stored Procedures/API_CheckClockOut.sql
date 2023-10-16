-- exec API_CheckClockOut 54457,'',''    
-- exec API_CheckClockOut 74512,'',''    
    
CREATE PROCEDURE [dbo].[API_CheckClockOut]        
 @ScheduleID BIGINT,        
 @ClockOutTime DateTime,        
 @ClockOutBeforeTime INT    
AS                                  
BEGIN        
 --DECLARE @Diff BIGINT;    
 --Select @Diff=datediff(mi, @ClockOutTime, (SELECT EndDate FROM ScheduleMasters WHERE ScheduleID=@ScheduleID AND IsDeleted=0))        
        
 --IF(@Diff >= @ClockOutBeforeTime)        
 -- Select 1        
 --ELSE        
 -- Select 0       
    
 --19/05/2020     
 --Akhilesh     
 -- Get VisitTaskLength and check length is greater then 0 then we can able to checkout    
 DECLARE @Diff BIGINT;    
 DECLARE @VisitTaskLength BIGINT;    
    
 Select @Diff=datediff(mi, @ClockOutTime, (SELECT EndDate FROM ScheduleMasters WHERE ScheduleID=@ScheduleID AND IsDeleted=0))     
     
 SELECT @VisitTaskLength= COUNT(evn.EmployeeVisitNoteID)         
 FROM EmployeeVisitNotes evn                              
 INNER JOIN EmployeeVisits s ON evn.EmployeeVisitID = s.EmployeeVisitID                  
 INNER JOIN ReferralTaskMappings rtm ON rtm.ReferralTaskMappingID=evn.ReferralTaskMappingID                              
 INNER JOIN VisitTasks v ON v.VisitTaskID=rtm.VisitTaskID                      
 LEFT JOIN VisitTaskCategories vg ON vg.VisitTaskCategoryID=v.VisitTaskCategoryID                      
 LEFT JOIN VisitTaskCategories vg1 ON v.VisitTaskSubCategoryID=vg1.VisitTaskCategoryID    
 WHERE s.ScheduleID=@ScheduleID    
 IF(@VisitTaskLength>0)    
    IF(@Diff >= @ClockOutBeforeTime)        
    Select 1        
    ELSE        
    Select 0;    
 ELSE    
 Select 0;    
END
GO

