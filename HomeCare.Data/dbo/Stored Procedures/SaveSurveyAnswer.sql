--  EXEC SaveSurveyAnswer '5', '80086', '4', '1', '2020-10-03',0   
--   select * from EmpCovidSurvey            
CREATE PROCEDURE [dbo].[SaveSurveyAnswer]                                
 @CovidSurveyID bigint = 0,                    
 @EmployeeID bigint = 0,                     
 @QuestionID bigint = 0,                  
 @AnswersID bigint = 0,
 @CreatedDate datetime = NULL,                            
 @IsDeleted bit = 0          
AS                    
BEGIN                    
                    
IF(@CovidSurveyID=0)
 BEGIN                  
  Insert into EmpCovidSurvey (EmployeeID,QuestionID,AnswersID,CreatedDate,IsDeleted,CreatedBy,UpdatedDate,UpdatedBy)         
  values (@EmployeeID,@QuestionID,@AnswersID,@CreatedDate,0,1,GETDATE(),1)                             
                  
   SELECT 1; RETURN;                  
 END                  
ELSE                    
 BEGIN                  
  UPDATE EmpCovidSurvey SET                         
  EmployeeID=@EmployeeID,                  
  QuestionID=@QuestionID,                  
  AnswersID=@AnswersID,                    
  IsDeleted= @IsDeleted,      
  UpdatedDate=GETDATE(),                
  UpdatedBy=1                 
  where CovidSurveyID = @CovidSurveyID-- and CreatedDate=Cast(@CreatedDate as datetime)                 
                  
   SELECT 2; RETURN;                  
 END                  
END