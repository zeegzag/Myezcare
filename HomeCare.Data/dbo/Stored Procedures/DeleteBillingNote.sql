
CREATE PROCEDURE [dbo].[DeleteBillingNote] 
 (  
 @BillingNoteID bigint = 0,
@BatchID bigint = 0  
 ) 
AS
BEGIN
   
 if @BillingNoteID>0  

 BEGIN  
      update BillingNote set IsDeleted=1 where BillingNoteID=@BillingNoteID  
 END
 EXEC GetBillingNote   @BatchID
 END 