CREATE PROCEDURE [dbo].[HC_SaveEmpDocument]      
@ReferralDocumentID BIGINT,    
@FileName VARCHAR(100),      
@KindOfDocument VARCHAR(50),    
@UserID BIGINT,    
@DocumentTypeID INT,      
@UpdatedDate DATETIME,    
@LoggedInUserID BIGINT,        
@SystemID VARCHAR(30)      
AS        
BEGIN        
   
 DECLARE @ReferralID BIGINT;  
 DECLARE @ComplianceID BIGINT;  
  
 SELECT @ReferralID=UserID,@ComplianceID=DocumentTypeID FROM ReferralDocuments WHERE ReferralDocumentID=@ReferralDocumentID  
  
    
 IF EXISTS (SELECT TOP 1 ReferralDocumentID FROM ReferralDocuments     
 WHERE KindOfDocument=@KindOfDocument AND DocumentTypeID=@DocumentTypeID AND UserID=@UserID AND ReferralDocumentID!=@ReferralDocumentID)    
BEGIN                    
 SELECT -1 RETURN;    
END    
      
UPDATE ReferralDocuments SET FileName=@FileName,KindOfDocument=@KindOfDocument,DocumentTypeID=@DocumentTypeID,UpdatedDate=@UpdatedDate,UpdatedBy=@LoggedInUserID,    
SystemID=@SystemID    
WHERE ReferralDocumentID=@ReferralDocumentID
  
SELECT 1 RETURN;    
        
END