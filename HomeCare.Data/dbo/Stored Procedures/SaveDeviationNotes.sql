
--  Exec SaveDeviationNotes 10177,30086,'AFFDDFD',2156,40                
 -- select * from SaveDeviationNote where EmployeeVisitID =10177                
-- select * from SaveDeviationNote order by DeviationNoteID desc                  
--CreatedBy: Akhilesh                
--CreatedDate:17/3/2020                
--Description: for Save deviation note                
                  
          
--UpdaredBy:Akhilesh          
--UpdaredDate:22 march 2020          
--Description:For deny duplicate entry          
CREATE procedure [dbo].[SaveDeviationNotes]                    
                    
@EmployeeVisitID int,                    
@DeviationID int,                    
@DeviationNotes nvarchar(500),                    
@DeviationNoteID bigint=0,      
@DeviationTime bigint                   
--@CreatedBy date,                    
--@UpdatedBy date                    
                    
AS        
BEGIN              
 IF EXISTS (SELECT TOP 1 DeviationID FROM SaveDeviationNote WHERE EmployeeVisitID=@EmployeeVisitID AND DeviationNotes=@DeviationNotes AND DeviationID=@DeviationID AND DeviationTime=@DeviationTime)                      
 BEGIN                                  
 SELECT -1 RETURN;                                    
 END   
   DECLARE @TotalDeviationTime INT;                          
 DECLARE @TotalServiceTime INT;  
  DECLARE @Diff INT; 
  DECLARE @TotalClocinTime INT; 
  DECLARE @TotalSheduleTime INT;    
  IF(@DeviationNoteID>0)  
  BEGIN  
    SELECT @Diff=(@DeviationTime-DeviationTime) FROM SaveDeviationNote WHERE DeviationNoteID=@DeviationNoteID                        
  SELECT @TotalDeviationTime=COALESCE((SUM(DeviationTime)+@Diff),@Diff) FROM SaveDeviationNote             
  WHERE EmployeeVisitID=@EmployeeVisitID AND IsDeleted=0   
  END  
  ELSE  
  BEGIN  
     SELECT @TotalDeviationTime=COALESCE((SUM(DeviationTime)+@DeviationTime),@DeviationTime) FROM SaveDeviationNote               
  WHERE EmployeeVisitID=@EmployeeVisitID AND IsDeleted=0   
  END  
  
  SELECT @TotalClocinTime=DATEDIFF(MINUTE, ClockInTime, ClockOutTime) FROM EmployeeVisits  WHERE EmployeeVisitID=@EmployeeVisitID     
  SELECT @TotalSheduleTime=DATEDIFF(MINUTE, StartDate, EndDate) FROM ScheduleMasters               
 WHERE ScheduleID=(SELECT ScheduleID FROM EmployeeVisits WHERE EmployeeVisitID=@EmployeeVisitID)   
     select @TotalServiceTime = @TotalSheduleTime-@TotalClocinTime
 IF(@TotalDeviationTime>@TotalServiceTime)                            
 BEGIN                            
  SELECT -2; RETURN;                            
 END    
 IF(@DeviationNoteID=0)   
 BEGIN               
insert into SaveDeviationNote (DeviationID,EmployeeId,DeviationNotes,DeviationType,IsDeleted,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,EmployeeVisitID,DeviationTime)                     
values(@DeviationID,1,@DeviationNotes,'',0,1,GETDATE(),'','',@EmployeeVisitID,@DeviationTime)                    
                  
 SELECT 1; RETURN;                   
end                
 IF(@DeviationNoteID>0)              
BEGIN                
UPDATE SaveDeviationNote SET DeviationNotes=@DeviationNotes,DeviationID=@DeviationID,DeviationTime=@DeviationTime WHERE DeviationNoteID=@DeviationNoteID  
 SELECT 1; RETURN;                 
END                
END                    
                    
-- select * from SaveDeviationNote order by  DeviationNoteID desc