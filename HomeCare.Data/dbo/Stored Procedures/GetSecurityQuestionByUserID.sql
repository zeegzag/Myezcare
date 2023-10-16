--20220711 RN
CREATE  PROCEDURE [dbo].[GetSecurityQuestionByUserID] 
	@UserName nvarchar(max)=NULL
AS                                                          
BEGIN                     
       
	   SELECT E.SecurityQuestionID,SQ.Question AS Question,E.SecurityAnswer AS SecurityAnswer
	   FROM EMPLOYEES E 
	   INNER JOIN SecurityQuestions SQ ON SQ.SecurityQuestionID=E.SecurityQuestionID	   
	   WHERE E.UserName=@UserName
                        
END 