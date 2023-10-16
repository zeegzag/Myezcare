/*  
Changed by sagar-26 Dec 2019(Line number 33): Added condition by- Sagar 26 Dec 2019 : added one condition 'AND IsDeleted=0'  
*/  
CREATE PROCEDURE [dbo].[API_AddTask]                  
 @EmployeeVisitNoteID BIGINT,                  
 @ReferralTaskMappingID BIGINT,                                    
 @EmployeeVisitID BIGINT,                          
 @EmployeeID BIGINT,            
 @Description NVARCHAR(1000),            
 @ServiceTime BIGINT,        
 @SetAsIncomplete BIT,      
 @PatientResignatureFlag BIT,      
 @SystemID VARCHAR(100),  
 @SimpleTaskType bit                         
AS                                    
BEGIN                  
 DECLARE @MinimumTimeRequired BIGINT;                  
 DECLARE @TotalTime INT;              
 DECLARE @TotalServiceTime INT;              
 DECLARE @Diff INT;     
 DECLARE @isTaskEntered int=0;             
              
 IF(@EmployeeVisitNoteID>0)              
 BEGIN              
  SELECT @Diff=(@ServiceTime-ServiceTime) FROM EmployeeVisitNotes WHERE EmployeeVisitNoteID=@EmployeeVisitNoteID              
  SELECT @TotalServiceTime=COALESCE((SUM(ServiceTime)+@Diff),@Diff) FROM EmployeeVisitNotes WHERE EmployeeVisitID=@EmployeeVisitID AND IsDeleted=0               
 END              
 ELSE              
 BEGIN              
  SELECT @TotalServiceTime=COALESCE((SUM(ServiceTime)+@ServiceTime),@ServiceTime) FROM EmployeeVisitNotes WHERE EmployeeVisitID=@EmployeeVisitID AND IsDeleted=0              
 END              
   
 --Added condition by- Sagar 26 Dec 2019 : added one condition 'AND IsDeleted=0'  
 SELECT @TotalTime=DATEDIFF(MINUTE, StartDate, EndDate) FROM ScheduleMasters WHERE ScheduleID=(SELECT ScheduleID FROM EmployeeVisits WHERE EmployeeVisitID=@EmployeeVisitID) AND IsDeleted=0                   
 --SELECT @TotalServiceTime=COALESCE((SUM(ServiceTime)+@ServiceTime),@ServiceTime) FROM EmployeeVisitNotes WHERE EmployeeVisitID=@EmployeeVisitID AND IsDeleted=0              
 /*  
Changed by: Pallav  
Reason: To remove validation for maximum service time in simple task entry  
Date: 02/13/2020  
*/                   
                 
 IF (@SimpleTaskType=0)  
  BEGIN  
   IF(@TotalServiceTime>@TotalTime)          
   BEGIN              
    SELECT -1; RETURN;              
   END   
  END     
                  
                  
 SET @MinimumTimeRequired = (SELECT v.MinimumTimeRequired FROM VisitTasks v                        
 INNER JOIN ReferralTaskMappings rtm ON v.VisitTaskID=rtm.VisitTaskID WHERE rtm.ReferralTaskMappingID=@ReferralTaskMappingID)                        
                                    
 IF(@ServiceTime>=@MinimumTimeRequired)                  
  BEGIN                  
   IF(@EmployeeVisitNoteID=0)    
   BEGIN    
    SELECT  @isTaskEntered= count(*) FROM EmployeeVisitNotes evn right outer JOIN dbo.EmployeeVisits ev ON  evn.EmployeeVisitID = ev.EmployeeVisitID     
    WHERE  ev.EmployeeVisitID=@EmployeeVisitID AND evn.ReferralTaskMappingID =@ReferralTaskMappingID     
  IF (@isTaskEntered<=0 )     
  BEGIN    
   INSERT INTO EmployeeVisitNotes (EmployeeVisitID,ReferralTaskMappingID,Description,ServiceTime,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,SimpleTaskType)                          
    VALUES (@EmployeeVisitID,@ReferralTaskMappingID,@Description,@ServiceTime,GETUTCDATE(),@EmployeeID,GETUTCDATE(),@EmployeeID,@SystemID,@SimpleTaskType)    
  END    
   --SET EmployeeVisitID to Filled form of related task    
   UPDATE EbriggsFormMppings SET EmployeeVisitNoteID=@@IDENTITY WHERE ReferralTaskMappingID=@ReferralTaskMappingID    
 END    
   ELSE                  
    UPDATE EmployeeVisitNotes SET Description=@Description,ServiceTime=@ServiceTime,ReferralTaskMappingID=@ReferralTaskMappingID,            
 UpdatedDate=GETUTCDATE(),UpdatedBy=@EmployeeID,SimpleTaskType=@SimpleTaskType                 
    WHERE EmployeeVisitNoteID=@EmployeeVisitNoteID        
        
 IF (@SetAsIncomplete = 1 AND @PatientResignatureFlag=1)      
  UPDATE EmployeeVisits SET IsPCACompleted=0,IsSigned=0 WHERE EmployeeVisitID=@EmployeeVisitID        
              DECLARE @ScheduleID bigint= (SELECT ScheduleID FROM EmployeeVisits WHERE EmployeeVisitID=@EmployeeVisitID);
             EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @ScheduleID,'',''     
   SELECT 1; RETURN;                        
  END                        
 ELSE                        
  BEGIN                        
   SELECT @MinimumTimeRequired; RETURN;                        
  END                        
END