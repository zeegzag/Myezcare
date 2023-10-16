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
  INSERT INTO EmployeeVisitNotes (EmployeeVisitID,ReferralTaskMappingID,Description,AlertComment,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)                
  VALUES (@EmployeeVisitID,@ReferralTaskMappingID,@Description,@AlertComment,GETUTCDATE(),@EmployeeID,GETUTCDATE(),@EmployeeID,@SystemID)              
            
  UPDATE EmployeeVisits SET SurveyCompleted=1,SurveyComment=@SurveyComment,UpdatedDate=GETUTCDATE(),UpdatedBy=@EmployeeID WHERE ScheduleID=@ScheduleID          
                
  SELECT 1; RETURN;              
END
