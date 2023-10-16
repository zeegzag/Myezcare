-- exec HC_GetModifierList '','',-1    
CREATE PROCEDURE [dbo].[HC_GetModifierList]            
@ModifierCode nvarchar(200),          
@ModifierName nvarchar(200),    
@IsDeleted int        
AS                
BEGIN                
       
 SELECT ModifierID,ModifierName,ModifierCode,IsDeleted
  FROM Modifiers  
 WHERE(       
 ((@ModifierCode IS NULL OR LEN(@ModifierCode)=0) OR ModifierCode LIKE '%' + @ModifierCode + '%') AND           
 ((@ModifierName IS NULL OR LEN(@ModifierName)=0) OR ModifierName LIKE '%' + @ModifierName + '%') AND    
 ((CAST(@IsDeleted AS BIGINT)= -1) OR IsDeleted=@IsDeleted))           
 END
