-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE HC_SaveBatchUploadedClaimErrors

@BatchUploadedClaimID bigint,  
@Field nvarchar(100),  
@MsgID nvarchar(100),  
@Message nvarchar(max),  
@Status nvarchar(50)  
	
AS
BEGIN

INSERT INTO BatchUploadedClaimErrors         
 (BatchUploadedClaimID,Field,MsgID,[Message],[Status])       
 VALUES        
 (@BatchUploadedClaimID,@Field,@MsgID,@Message,@Status)        
        
 SELECT * FROM BatchUploadedClaimErrors WHERE BatchUpClaimErrorID=SCOPE_IDENTITY()  
   
END