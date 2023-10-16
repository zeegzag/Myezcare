CREATE PROCEDURE [dbo].[API_AddConclusion]        
 @ReferralTaskMappingID BIGINT,                
 @Description NVARCHAR(1000),    
 @AlertComment NVARCHAR(1000),    
 @EmployeeVisitID BIGINT,              
 @ScheduleID BIGINT,              
 @SurveyComment NVARCHAR(1000),              
 @EmployeeID BIGINT,                
 @SystemID VARCHAR(100)                   
AS                              
BEGIN                  
 Declare @EmployeeVisitNoteID bigint
 select @EmployeeVisitNoteID= evn.EmployeeVisitNoteID from  EmployeeVisitNotes EVN where evn.EmployeeVisitID=@EmployeeVisitID and evn.ReferralTaskMappingID=@ReferralTaskMappingID and IsDeleted=0
 
 if (@EmployeeVisitNoteID is not null)
 begin
 update EmployeeVisitNotes set 	Description=@Description,AlertComment=@AlertComment,UpdatedDate=GetUTCDate(),UpdatedBy=@EmployeeID,SystemID=@SystemID where EmployeeVisitNoteID=@EmployeeVisitNoteID 
 end
 else
 begin
 INSERT INTO EmployeeVisitNotes (EmployeeVisitID,ReferralTaskMappingID,Description,AlertComment,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)                    
  VALUES (@EmployeeVisitID,@ReferralTaskMappingID,@Description,@AlertComment,GETUTCDATE(),@EmployeeID,GETUTCDATE(),@EmployeeID,@SystemID)                  
end                
  UPDATE EmployeeVisits SET SurveyCompleted=1,SurveyComment=@SurveyComment,UpdatedDate=GETUTCDATE(),UpdatedBy=@EmployeeID WHERE ScheduleID=@ScheduleID              
  EXEC [dbo].[ScheduleEventBroadcast] 'EditSchedule', @ScheduleID,'222','25'                
  SELECT 1; RETURN;                  

END 