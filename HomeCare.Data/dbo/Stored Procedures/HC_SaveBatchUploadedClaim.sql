-- SP_HELPTEXT HC_SaveBatchUploadedClaim  
  
-- =============================================      
-- Author:  <Author,,Name>      
-- Create date: <Create Date,,>      
-- Description: <Description,,>      
-- =============================================      
      
-- exec [dbo].[HC_SaveBatchUploadedClaim] 0,'17','1111111112','7555555555','ABCD1','246366718','2011-05-01','000428594','myfile.x12','507883714','60054','0000017262','','','CLAIM.MD','CLMMD','A','30.00'      
      
CREATE  PROCEDURE [dbo].[HC_SaveBatchUploadedClaim]      
@BatchUploadedClaimID bigint,      
@BatchID nvarchar(100),        
@Bill_NPI nvarchar(100),        
@Bill_TaxID nvarchar(100),        
@ClaimID nvarchar(100),        
@ClaimMD_ID nvarchar(100),        
--@MessageID nvarchar(MAX),        
@Message nvarchar(MAX),        
--@MessageStatus nvarchar(MAX),        
@FDOS nvarchar(100),        
@FileID nvarchar(100),        
@FileName nvarchar(100),        
@INS_Number nvarchar(100),        
@PayerID nvarchar(100),        
@PCN nvarchar(100),        
@Remote_ClaimID nvarchar(100),        
@Sender_ICN nvarchar(100),        
@Sender_Name nvarchar(100),        
@SenderID nvarchar(100),        
@Status nvarchar(100),        
@Total_Charge nvarchar(100),      
@PatientName nvarchar(100) null,      
@ReferralID bigint,      
@Payer nvarchar(100),      
@BillingProvider nvarchar(100),      
@LoggedInUserId bigint,              
@CurrentDate DATETIME,              
@SystemID  NVARCHAR(MAX)        
        
AS      
BEGIN       
      
declare @BatchTypeID as bigint      
declare @PayorID as bigint        
set @BatchTypeID = (select BatchTypeID FROM Batches WHERE BatchID=@BatchID)      
set @PayorID = (select PayorID FROM Batches WHERE BatchID=@BatchID)      
  
  
  
SELECT TOP 1 @ReferralID=ReferralID FROM ReferralBillingAuthorizations WHERE @ClaimID     LIKE  AuthorizationCode+'%'
      
IF EXISTS(SELECT 1 FROM BatchUploadedClaims WHERE ClaimID = @ClaimID AND BatchID=@BatchID)      
BEGIN       
      
UPDATE BatchUploadedClaims              
 SET       
 BatchTypeID=@BatchTypeID,[Bill_NPI]=@Bill_NPI,[Bill_TaxID]=@Bill_TaxID,[ClaimID]=@ClaimID,[ClaimMD_ID]=@ClaimMD_ID, [FDOS]=@FDOS, [FileID]=@FileID,       
 [FileName]=@FileName,[INS_Number]=@INS_Number,[PayerID]=@PayerID,[PCN]=@PCN, [Remote_ClaimID]=@Remote_ClaimID, [Sender_ICN]=@Sender_ICN,       
 [Sender_Name]=@Sender_Name,[SenderID]=@SenderID,[Status]=@Status,[Total_Charge]=@Total_Charge, [ReferralID]=@ReferralID, [PatientName]=@PatientName, [Payer]=@Payer,[BillingProvider]=@BillingProvider,      
 UpdatedBy=@LoggedInUserId,UpdatedDate=@CurrentDate,SystemID=@SystemID      
-- ,MessageID=@MessageID,  
 ,MessageStr=@Message  
 --MessageStatus=@MessageStatus  
 WHERE ClaimID=@ClaimID        
      
 DELETE FROM BatchUploadedClaimErrors WHERE BatchUploadedClaimID IN (SELECT BatchUploadedClaimID FROM BatchUploadedClaims WHERE ClaimID = @ClaimID)      
      
 SELECT * FROM BatchUploadedClaims WHERE ClaimID=@ClaimID AND BatchID=@BatchID    
   
PRINT '1'  
          
END              
ELSE              
BEGIN         
      
 INSERT INTO BatchUploadedClaims               
 (BatchID,BatchTypeID,PayorID,Bill_NPI,Bill_TaxID,ClaimID,ClaimMD_ID,FDOS,FileID,[FileName],INS_Number,PayerID,              
 PCN,Remote_ClaimID,Sender_ICN,Sender_Name,SenderID,[Status],Total_Charge,[ReferralID],PatientName,Payer,BillingProvider,      
 CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,MessageStr)              
 VALUES              
 (@BatchID,@BatchTypeID,@PayorID,@Bill_NPI,@Bill_TaxID,@ClaimID,@ClaimMD_ID,@FDOS,@FileID,@FileName,@INS_Number,@PayerID,              
 @PCN,@Remote_ClaimID,@Sender_ICN,@Sender_Name,@SenderID,@Status,@Total_Charge,@ReferralID,@PatientName,@Payer,@BillingProvider,      
 @LoggedInUserId,@CurrentDate,@LoggedInUserId,@CurrentDate,@SystemID,@Message)              
             
 SELECT * FROM BatchUploadedClaims WHERE BatchUploadedClaimID=SCOPE_IDENTITY()        
    PRINT '2'  
END       
      
END 