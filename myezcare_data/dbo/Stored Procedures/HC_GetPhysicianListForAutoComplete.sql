
-- exec [HC_GetPhysicianListForAutoComplete] 'ya'              
CREATE PROCEDURE [dbo].[HC_GetPhysicianListForAutoComplete]                    
 @SearchText VARCHAR(MAX),                  
 @PageSize int=10             
AS                    
BEGIN                    
 select TOP (@PageSize)            
 *  
 from Physicians             
 WHERE                     
   (                    
    FirstName LIKE '%'+@SearchText+'%' OR                    
    MiddleName LIKE '%'+@SearchText+'%'  OR  
    LastName LIKE '%'+@SearchText+'%'                 
   ) and IsDeleted = 0          
END  
