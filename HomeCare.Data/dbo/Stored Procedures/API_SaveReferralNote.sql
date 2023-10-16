CREATE PROCEDURE [dbo].[API_SaveReferralNote]     
   @CommonNoteID bigint=NULL,   
 @ReferralID bigint=NULL,      
 @NoteDetail nvarchar(2000),      
 @LoggedInID bigint,
 @CategoryID bigint =null
AS      
BEGIN      
      
BEGIN TRANSACTION trans                                                                                
BEGIN TRY                                                              
                                                     
    IF EXISTS (SELECT TOP 1 * FROM CommonNotes WHERE CommonNoteID=@CommonNoteID)      
 BEGIN      
  UPDATE [dbo].[CommonNotes]      
  SET     
  Note=@NoteDetail,    
  UpdatedBy=@LoggedInID,    
  UpdatedDate=GETUTCDATE()    
  WHERE     
  CommonnoteID=@CommonNoteID      
 END      
 ELSE      
 BEGIN      
  INSERT INTO [dbo].[CommonNotes]    
  (EmployeeID,ReferralID,Note,IsDeleted,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate)      
  VALUES     
  (NULL,@ReferralID,@NoteDetail,0,@LoggedInID,GETUTCDATE(),@LoggedInID,GETUTCDATE())      
 END         
                                               
 SELECT 1 AS TransactionResultId;                                                              
                                                              
 IF @@TRANCOUNT > 0                                                                                
 BEGIN                                                                                 
  COMMIT TRANSACTION trans                                                                        
 END                                                                                
END TRY                                                                 
BEGIN CATCH                                                  
 SELECT -1 AS TransactionResultId,ERROR_MESSAGE() AS ErrorMessage;                                                                                
     
 IF @@TRANCOUNT > 0                                                                                
 BEGIN                                                                                 
  ROLLBACK TRANSACTION trans                                                                                 
 END                                                                  
END CATCH           
    
END