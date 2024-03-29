
CREATE PROCEDURE [dbo].[AddBillingNote]        
@BillingNoteID bigint=0,    
 @BatchID bigint=0,    
 @BillingNote VARCHAR(MAX)=NULL,    
 @LoggedInID bigint=0  
         
AS        
BEGIN    
IF(@BillingNoteID>0)  
 BEGIN  
   UPDATE [BillingNote] SET  
   BillingNote=@BillingNote,  
   UpdatedDate=GETUTCDATE(),  
   UpdatedBy=@LoggedInID  
   WHERE BillingNoteID=@BillingNoteID
 --           
 END  
ELSE   
 BEGIN  
  INSERT INTO [BillingNote]  (BatchID, BillingNote,CreatedDate, CreatedBy, UpdatedDate, UpdatedBy,IsDeleted)    
            VALUES     (@BatchID,@BillingNote, GETUTCDATE(),  @LoggedInID,GETUTCDATE(),@LoggedInID,0)   
  END  
  
  EXEC GetBillingNote   @BatchID
  END