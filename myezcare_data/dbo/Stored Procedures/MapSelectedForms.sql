--EXEC MapSelectedForms @VisitTaskID = '82', @EBFormIDs = '5bee5a9d8a063bc765ef1e96,5bee5abc8a063bc765ef2033', @CurrentDateTime = '2018/12/26 16:16:47', @loggedInUserId = '1', @SystemID = '::1'    
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
