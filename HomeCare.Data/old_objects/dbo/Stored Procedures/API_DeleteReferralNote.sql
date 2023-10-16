CREATE PROCEDURE [dbo].[API_DeleteReferralNote] 
 @CommonNoteID bigint=NULL    
AS  
BEGIN  
 
BEGIN TRANSACTION trans                                                                            
BEGIN TRY                                                          
                                                 
    IF EXISTS (SELECT TOP 1 * FROM CommonNotes WHERE CommonNoteID=@CommonNoteID)  
	BEGIN  
		UPDATE CommonNotes
		SET 
		IsDeleted=1
		WHERE 
		CommonnoteID=@CommonNoteID  
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
