CREATE PROCEDURE [dbo].[HC_SaveServiceCode]                
@ServiceCodeID BIGINT,          
@ServiceCode VARCHAR(50),           
@ModifierID NVARCHAR(MAX),         
@ModifierIDCombinations NVARCHAR(MAX),         
@ServiceName VARCHAR(100),           
@Description VARCHAR(500),    
@AccountCode NVARCHAR(MAX),    
@IsBillable BIT,
@VisitTypeId BIGINT, 
@loggedInUserId BIGINT,      
@SystemID VARCHAR(100)    
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
    Declare @CareItemType INT  
  
 select @CareItemType = DDMasterTypeID from lu_DDMasterTypes lu  --inner join DDMaster dm on dm.ItemType=lu.DDMasterTypeID  where lu.Name='Care Type'  
  
 IF(@ServiceCodeID=0)                                  
 BEGIN                                  
  INSERT INTO ServiceCodes (ServiceCode,ModifierID,ServiceName,Description,IsBillable,IsDeleted,AccountCode,VisitTypeId)         
  VALUES(@ServiceCode,@ModifierID,@ServiceName,@Description,@IsBillable,0,@AccountCode,@VisitTypeId)      
    
  --Added On DDMaster  
  IF((SELECT COUNT(*) FROM DDMaster WHERE ItemType = @CareItemType AND Title = @Description) = 0 AND ISNULL(@Description,'') !='')  
  BEGIN  
   INSERT INTO DDMaster(ItemType,Title,Value,ParentID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)        
   VALUES (1,@Description,'',@VisitTypeId,GETUTCDATE(),@loggedInUserId,GETUTCDATE(),@loggedInUserId,@SystemID);  
   END  
  
 END             
 ELSE                                  
 BEGIN     
 --Update DDMaster  
 UPDATE DDMaster SET Title = @Description WHERE ItemType = @CareItemType AND Title IN (SELECT @Description FROM ServiceCodes WHERE ServiceCodeID=@ServiceCodeID)  
  
 --Update ServiceCode  
  UPDATE ServiceCodes                                   
  SET          
  ServiceCode=@ServiceCode,ModifierID=@ModifierID,ServiceName=@ServiceName,          
  Description=@Description,IsBillable=@IsBillable,AccountCode =@AccountCode ,VisitTypeId = @VisitTypeId   
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
GO