
CREATE PROCEDURE [dbo].[HC_DeleteModifier]      
 @ListOfIdsInCSV varchar(MAX)=null      
 --,@IsDeleted bit         
AS                  
BEGIN                  
 IF(LEN(@ListOfIdsInCSV)>0)              
 BEGIN            
   
  UPDATE Modifiers   
  SET   
  IsDeleted=CASE IsDeleted WHEN 0 THEN 1 ELSE 0 END  
  WHERE ModifierID IN (SELECT CAST(VAL AS BIGINT)   
  FROM GETCSVTABLE(@ListOfIdsInCSV));    
    
   SELECT 1 RETURN;                 
 END              
 ELSE    
 BEGIN      
  SELECT -1 RETURN;              
 END      
END 

