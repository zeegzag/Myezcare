--  EXEC [rpt].[CovidSurveyReport] '2020-10-03'

CREATE PROCEDURE [rpt].[CovidSurveyReport] 
	@Date datetime=''
AS                                                        
BEGIN                   
               
 SELECT 
	'Survey#' + cast(ECS.QuestionID as varchar(10)) AS QuestionID,                        
   CSQ.Question,
   case when ECS.AnswersID = 0 then 'No' when ECS.AnswersID = 1 then 'Yes' else 'NA' end AS AnswersID,
   CONCAT(E.FirstName,' ',E.LastName) AS [FullName],
   ECS.CreatedDate,
   case when ECS.AnswersID = 1 then 'Yes, Survey participant is suspected to have corona virus symptoms.'  else 'No, Survey participant is not suspected to have corona virus symptoms.' end AS Result,
    case when ECS.AnswersID = 1 then '1'  else '0' end AS ResultCode
 FROM [dbo].[EmpCovidSurvey] ECS
  INNER JOIN [dbo].[CovidSurveyQuestions] CSQ  ON CSQ.QuestionID = ECS.QuestionID
  INNER JOIN [dbo].[Employees] E  ON E.EmployeeID = ECS.EmployeeID 
  WHERE                         
		convert(varchar(10), ECS.[CreatedDate],110) = convert(varchar, @Date,110)                     
        AND ECS.[IsDeleted] = 0 
		ORDER BY ECS.AnswersID DESC
END