CREATE PROCEDURE [dbo].[HC_SaveServiceCode]          
@ServiceCodeID BIGINT,    
@ServiceCode VARCHAR(50),     
@ModifierID NVARCHAR(MAX),   
@ModifierIDCombinations NVARCHAR(MAX),   
@ServiceName VARCHAR(100),     
@Description VARCHAR(500),     
@IsBillable BIT  
AS                            
BEGIN                            
IF NOT EXISTS (SELECT TOP 1 ServiceCodeID FROM ServiceCodes   
WHERE   
(  
 ServiceCode=@ServiceCode  
 And (((@ModifierIDCombinations IS NULL OR LEN(@ModifierIDCombinations)=0) AND (ModifierId IS NULL OR LEN(ModifierId)=0))   
   OR   
  (@ModifierIDCombinations IS NOT NULL AND ModifierId in (select Result from CSVtoTableWithIdentity(@ModifierIDCombinations,'|'))))    
 AND   
 ServiceCodeID != @ServiceCodeID)  
)  
      
BEGIN                      
  
 IF(@ServiceCodeID=0)                            
 BEGIN                            
  INSERT INTO ServiceCodes                            
  (ServiceCode,ModifierID,ServiceName,Description,IsBillable,IsDeleted)   
  VALUES            
  (@ServiceCode,@ModifierID,@ServiceName,@Description,@IsBillable,0)   
 END       
 ELSE                            
 BEGIN                            
  UPDATE ServiceCodes                             
  SET    
  ServiceCode=@ServiceCode,ModifierID=@ModifierID,ServiceName=@ServiceName,    
  Description=@Description,IsBillable=@IsBillable  
  WHERE ServiceCodeID=@ServiceCodeID;                            
 END    
 SELECT 1 As TransactionResultId;
END    
ELSE    
BEGIN    
 SELECT -1 As TransactionResultId;
 RETURN;    
END    
                  
END
