CREATE PROCEDURE [dbo].[API_UpdateDocument]
@ReferralDocumentID BIGINT,        
@FileName VARCHAR(100),          
@DocumentationType VARCHAR(50),  
@ExpirationDate DATE = NULL,  
@ServerCurrentDateTime DATETIME,        
@LoggedInID BIGINT,            
@SystemID VARCHAR(30)          
AS            
BEGIN            
       
     
-- IF EXISTS (SELECT TOP 1 ReferralDocumentID FROM ReferralDocuments         
-- WHERE KindOfDocument=@KindOfDocument AND DocumentTypeID=@DocumentTypeID AND UserID=@UserID AND ReferralDocumentID!=@ReferralDocumentID)        
--BEGIN                        
-- SELECT -1 RETURN;        
--END        
          
UPDATE ReferralDocuments SET FileName=@FileName,KindOfDocument=@DocumentationType,ExpirationDate=@ExpirationDate,  
UpdatedDate=@ServerCurrentDateTime,UpdatedBy=@LoggedInID,SystemID=@SystemID        
WHERE ReferralDocumentID=@ReferralDocumentID        
    
SELECT 1 RETURN;        
            
END