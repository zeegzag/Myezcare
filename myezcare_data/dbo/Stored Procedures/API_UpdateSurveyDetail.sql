 CREATE PROCEDURE [dbo].[API_UpdateSurveyDetail]
 @EmployeeVisitID BIGINT,        
 @ScheduleID BIGINT,        
 @SurveyComment NVARCHAR(1000),        
 @EmployeeID BIGINT             
AS                        
BEGIN            
UPDATE EmployeeVisits SET SurveyCompleted=1,SurveyComment=@SurveyComment,UpdatedDate=GETUTCDATE(),UpdatedBy=@EmployeeID WHERE ScheduleID=@ScheduleID        
              
  SELECT 1; RETURN;            
END 
