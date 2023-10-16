 
 CREATE PROCEDURE [dbo].[HC_SaveModifier]      
 @ModifierID bigint,      
 @ModifierCode VARCHAR(100),                  
 @ModifierName NVARCHAR(200),    
 @CreatedBy bigint,    
 @UpdatedBy bigint,    
 @SystemID NVARCHAR(MAX)            
AS                  
BEGIN                  
               
 IF EXISTS (SELECT TOP 1 ModifierID FROM Modifiers WHERE ModifierCode=@ModifierCode AND ModifierID != @ModifierID)                    
 BEGIN            
 SELECT -1 RETURN;              
 END            
                
 IF(@ModifierID=0)                  
 BEGIN                  
   INSERT INTO Modifiers      
   (ModifierCode,ModifierName,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)      
   VALUES                  
   (@ModifierCode,@ModifierName,0,GETDATE(),@CreatedBy,GETDATE(),@UpdatedBy,@SystemID);                   
                     
 END                  
 ELSE                  
 BEGIN                  
   UPDATE Modifiers                   
   SET                        
 ModifierCode=@ModifierCode,      
 ModifierName=@ModifierName,    
 UpdatedDate=GETDATE(),    
 UpdatedBy= @UpdatedBy    
   WHERE ModifierID=@ModifierID;     
                    
 END                  
              
          
 SELECT 1; RETURN;              
              
              
END