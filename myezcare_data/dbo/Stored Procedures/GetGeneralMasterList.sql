-- SELECT * FROM  DDMaster      
-- EXEC GetGeneralMasterList '18'      
      
      
CREATE PROCEDURE [dbo].[GetGeneralMasterList]          
 @ItemType INT,      
 @KeyName NVARCHAR(100)='',      
 @ValueName NVARCHAR(100)=''       
AS        
BEGIN        
      
IF(@KeyName='') SET @KeyName='Value';      
IF(@ValueName='') SET @ValueName='Name';      
      
      
DECLARE  @SqlQuery NVARCHAR(MAX)='';      
      
      
    SET @SqlQuery='SELECT * FROM (SELECT '+@ValueName+'=Title,'+@KeyName+'=DDMasterID from DDMaster where IsDeleted=0 and ItemType='+CONVERT(VARCHAR(100),@ItemType)+'     
               UNION  SELECT '+@ValueName+'=''Add/Edit/Item'','+@KeyName+'=9999999900001'+CONVERT(VARCHAR(100),@ItemType)+') AS T ORDER BY '+@KeyName+' DESC';
      
-- PRINT @SqlQuery;      
    
-- EXEC GetGeneralMasterList '6'      
      
EXECUTE sp_executesql @SqlQuery;      
       
      
        
END
