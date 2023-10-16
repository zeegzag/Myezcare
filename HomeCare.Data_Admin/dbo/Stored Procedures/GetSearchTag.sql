CREATE PROCEDURE [dbo].[GetSearchTag]      
@SearchText varchar(20)=null,        
@PageSize int
AS        
BEGIN      
      
SELECT DISTINCT Top(@PageSize) FormTagName,FormTagID FROM  FormTags    
WHERE (@SearchText IS NULL) OR (FormTagName LIKE '%'+@SearchText+'%' )
         
END