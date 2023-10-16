  
--  EXEC API_GetEmployeeSurveyList  80086,'2020-10-02 23:02:21.003'       
        
CREATE  PROCEDURE [dbo].[API_GetEmployeeSurveyList]          
@EmployeeID bigint = 0,        
@CreatedDate datetime = Null        
AS              
BEGIN              
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()        
SELECT e.CovidSurveyID,dbo.GetGenericNameFormat(emp.FirstName,emp.MiddleName, emp.LastName,@NameFormat) as EmployeeName, q.Question, e.AnswersID FROM EmpCovidSurvey e    
INNER JOIN CovidSurveyQuestions q ON e.QuestionID = q.QuestionID    
INNER JOIN Employees emp ON e.EmployeeID = emp.EmployeeID    
  WHERE e.EmployeeID=@EmployeeID       
  AND Cast(e.CreatedDate as Date)=cast(@CreatedDate as date)      
  AND e.IsDeleted = 0;      
         
              
END 