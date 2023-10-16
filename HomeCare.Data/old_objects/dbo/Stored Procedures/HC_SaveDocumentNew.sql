CREATE PROCEDURE [dbo].[HC_SaveDocumentNew]     
@ReferralDocumentID BIGINT,         
@FileName VARCHAR(100),           
@KindOfDocument VARCHAR(50),   
@ExpirationDate DATE = NULL,   
@UpdatedDate DATETIME,         
@LoggedInUserID BIGINT,             
@SystemID VARCHAR(30),
@ReferralID bigint      
AS             
BEGIN             
        
      
-- IF EXISTS (SELECT TOP 1 ReferralDocumentID FROM ReferralDocuments          
-- WHERE KindOfDocument=@KindOfDocument AND DocumentTypeID=@DocumentTypeID AND UserID=@UserID AND ReferralDocumentID!=@ReferralDocumentID)         
--BEGIN                         
-- SELECT -1 RETURN;         
--END         
           
UPDATE ReferralDocuments SET FileName=@FileName,KindOfDocument=@KindOfDocument,ExpirationDate=@ExpirationDate,   
UpdatedDate=@UpdatedDate,UpdatedBy=@LoggedInUserID,SystemID=@SystemID,ReferralID=@ReferralID     
WHERE ReferralDocumentID=@ReferralDocumentID         
     
SELECT 1 RETURN;         
             
END
