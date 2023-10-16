CREATE PROCEDURE [dbo].[GetEmailTemplateList]                        
@EmailTemplateID BIGINT=0,                        
@EmailTemplateName VARCHAR(100)=null,                        
@EmailTemplateSubject VARCHAR(100)=null,      
@Token varchar(100)=null,              
@IsDeleted BIGINT = -1,                       
@SortExpression NVARCHAR(100),                        
@SortType NVARCHAR(10),                        
@FromIndex INT,                        
@PageSize INT,  
@module nvarchar(150)=null,  
@EmailType nvarchar(150)=null                          
AS                        
BEGIN 
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
;WITH CTEEmailTemplate AS                    
 (                         
  SELECT *,COUNT(T1.EmailTemplateID) OVER() AS COUNT FROM                    
  (                        
   SELECT ROW_NUMBER() OVER (ORDER BY                    
                    
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EmailTemplateID' THEN EmailTemplateID END END ASC,                    
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EmailTemplateID' THEN EmailTemplateID END END DESC,    
     
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'OrderNumber' THEN OrderNumber END END ASC,                    
 CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'OrderNumber' THEN OrderNumber END END DESC,                    
                    
    CASE WHEN @SORTTYPE = 'ASC' THEN                        
      CASE                         
      WHEN @SORTEXPRESSION = 'EmailTemplateName' THEN EmailTemplateName                        
      WHEN @SORTEXPRESSION = 'EmailTemplateSubject' THEN EmailTemplateSubject                        
      WHEN @SORTEXPRESSION = 'Token' THEN Token    
    END                         
    END ASC,                        
    CASE WHEN @SORTTYPE = 'DESC' THEN                        
      CASE                         
      WHEN @SORTEXPRESSION = 'EmailTemplateName' THEN EmailTemplateName      
      WHEN @SORTEXPRESSION = 'EmailTemplateSubject' THEN EmailTemplateSubject      
      WHEN @SORTEXPRESSION = 'Token' THEN Token    
    END      
    END DESC      
    ) AS ROW,      
   EmailTemplateID,EmailTemplateName,EmailTemplateSubject,E.EmailTemplateTypeID,      
   E.EmailTemplateBody,E.CreatedDate,dbo.GetGenericNameFormat(EMP.FirstName,EMP.MiddleName, EMP.LastName,@NameFormat) as AddedBy ,E.Token, E.OrderNumber,e.IsEdit,e.IsHide,e.Module,e.EmailType      
   FROM EmailTemplates E      
   LEFT JOIN Employees  EMP on EMP.EmployeeID = E.CreatedBy      
   WHERE     
    ((@EmailTemplateName  IS NULL OR LEN(@EmailTemplateName )=0) OR (E.EmailTemplateName LIKE '%' + @EmailTemplateName + '%'))               
   AND ((@EmailTemplateSubject IS NULL OR LEN(@EmailTemplateSubject)=0) OR (E.EmailTemplateSubject LIKE '%' + @EmailTemplateSubject+ '%'))               
   AND ((@Token IS NULL OR LEN(@Token )=0) OR (Token LIKE '%' + @Token + '%'))  AND ((@EmailType  IS NULL OR LEN(@EmailType )=0) OR (E.EmailType LIKE '%' + @EmailType + '%'))   
   AND ((@module  IS NULL OR LEN(@module )=0) OR (E.Module LIKE '%' + @module + '%')) and e.Module in('Patient','Employee','Scheduling') and e.IsDeleted=0           
  ) AS T1                            
 )                        
 SELECT * FROM CTEEmailTemplate WHERE ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)                       
                      
END  