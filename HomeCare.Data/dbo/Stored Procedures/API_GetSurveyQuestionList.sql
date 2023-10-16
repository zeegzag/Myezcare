--  EXEC API_GetSurveyQuestionList
  
CREATE PROCEDURE [dbo].[API_GetSurveyQuestionList]    
  
AS        
BEGIN        
        
 SELECT * FROM CovidSurveyQuestions WHERE IsDeleted = 0;  
        
END