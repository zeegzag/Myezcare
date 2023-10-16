CREATE PROCEDURE [dbo].[SavePCACompeleted]    
@EmployeeVisitID BIGINT,          
-- @SurveyCompleted BIGINT,                      
 --@IsPCACompleted BIGINT,    
 @UpdatedBy BIGINT 
 --@loggedInID BIGINT   
AS                      
BEGIN    
       
 --UPDATE EmployeeVisits        
 --  SET IsPCACompleted=CAST(@IsPCACompleted AS BIGINT),      
 --   SurveyCompleted=CAST(@SurveyCompleted AS BIGINT),
 --UpdatedDate=GETDATE(), UpdatedBy=CAST(@UpdatedBy AS BIGINT)      
 --  WHERE EmployeeVisitID=@EmployeeVisitID;        


 --UpdatedBy:Akhilesh kamal
--UpdateDate:17/01/2020
--Description:For IsPCACompleted,SurveyCompleted save true value

UPDATE EmployeeVisits        
   SET IsPCACompleted=1,      
    SurveyCompleted=1,
 UpdatedBy=CAST(@UpdatedBy as bigint)      
   WHERE EmployeeVisitID=@EmployeeVisitID;         

   SELECT IsPCACompleted,SurveyCompleted FROM EmployeeVisits WHERE EmployeeVisitID=@EmployeeVisitID      
END