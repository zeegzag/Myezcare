CREATE PROCEDURE [dbo].[MapSelectedForms]      
@VisitTaskID BIGINT,      
@EBFormIDs NVARCHAR(MAX),      
@CurrentDateTime DATETIME,              
@loggedInUserId BIGINT,      
@SystemID VARCHAR(100)      
AS                
BEGIN                
BEGIN TRANSACTION trans      
 BEGIN TRY      
     
 DECLARE @TempTable TABLE (VisitTaskID BIGINT,EBFormID NVARCHAR(MAX),CurrentDateTime DATETIME,LoggedInID BIGINT,SystemID VARCHAR(100))      
 INSERT INTO @TempTable      
 SELECT @VisitTaskID,val,@CurrentDateTime,@loggedInUserId,@SystemID FROM dbo.GetCSVTable(@EBFormIDs)      
      
 INSERT INTO TaskFormMappings (VisitTaskID,EBFormID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)      
        SELECT T.VisitTaskID,T.EBFormID,CurrentDateTime,LoggedInID,CurrentDateTime,LoggedInID,T.SystemID FROM @TempTable T    
  LEFT JOIN TaskFormMappings TFM ON TFM.EBFormID=T.EBFormID AND TFM.VisitTaskID=T.VisitTaskID  
  WHERE TFM.TaskFormMappingID IS NULL  
  
  UPDATE TFM 
        SET 
        TFM.IsDeleted = 0,
        TFM.UpdatedDate = T.CurrentDateTime,
        TFM.UpdatedBy = T.LoggedInID,
        TFM.SystemID = T.SystemID      
    FROM @TempTable T    
  LEFT JOIN TaskFormMappings TFM ON TFM.EBFormID=T.EBFormID AND TFM.VisitTaskID=T.VisitTaskID  
  WHERE TFM.TaskFormMappingID IS NOT NULL    
      
   IF @@TRANCOUNT > 0                                
    BEGIN                                 
     COMMIT TRANSACTION trans                                 
    END                                
 SELECT 1;      
      
 END TRY                                
 BEGIN CATCH                                
                
  IF @@TRANCOUNT > 0                        
   BEGIN                                 
    ROLLBACK TRANSACTION trans                                 
 SELECT -1;      
   END                                
 END CATCH                               
                           
END