-- EXEC GetEmailTemplateList @IsDeleted = '0', @SortExpression = 'EmailTemplateName', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'  
CREATE PROCEDURE [dbo].[GetEmailTemplateList]                      
@EmailTemplateID BIGINT=0,                      
@EmailTemplateName VARCHAR(100)=null,                      
@EmailTemplateSubject VARCHAR(100)=null,    
@Token varchar(100)=null,            
@IsDeleted BIGINT = -1,                     
@SortExpression NVARCHAR(100),                      
@SortType NVARCHAR(10),                      
@FromIndex INT,                      
@PageSize INT                        
AS                      
BEGIN                        
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
   E.EmailTemplateBody,E.CreatedDate,EMP.FirstName+''+ EMP.LastName as AddedBy ,E.Token, E.OrderNumber    
   FROM EmailTemplates E    
   LEFT JOIN Employees  EMP on EMP.EmployeeID = E.CreatedBy    
   WHERE ((CAST(@IsDeleted AS BIGINT)=-1) OR E.IsDeleted=@IsDeleted)    
   AND ((@EmailTemplateName  IS NULL OR LEN(@EmailTemplateName )=0) OR (E.EmailTemplateName LIKE '%' + @EmailTemplateName + '%'))             
   AND ((@EmailTemplateSubject IS NULL OR LEN(@EmailTemplateSubject)=0) OR (E.EmailTemplateSubject LIKE '%' + @EmailTemplateSubject+ '%'))             
   AND ((@Token IS NULL OR LEN(@Token )=0) OR (Token LIKE '%' + @Token + '%'))             
  ) AS T1                          
 )                      
 SELECT * FROM CTEEmailTemplate WHERE ROW BETWEEN ((@PAGESIZE*(@FROMINDEX-1))+1) AND (@PAGESIZE*@FROMINDEX)                     
                    
END
