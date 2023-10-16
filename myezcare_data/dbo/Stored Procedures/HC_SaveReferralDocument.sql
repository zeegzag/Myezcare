CREATE PROCEDURE [dbo].[HC_SaveReferralDocument]      
@FileName VARCHAR(MAX),        
@FilePath VARCHAR(MAX),      
@KindOfDocument VARCHAR(20),      
--@DocumentTypeID BIGINT=0,      
@ReferralID BIGINT,
@ComplianceID BIGINT,
@UserType INT,      
@LoggedInUserID BIGINT,        
@SystemID VARCHAR(30)      
AS        
BEGIN        
      
INSERT INTO ReferralDocuments (FileName,FilePath,KindOfDocument,DocumentTypeID,UserID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,UserType,ComplianceID)
VALUES(@FileName,@FilePath,@KindOfDocument,0,@ReferralID,getdate(),@LoggedInUserID,getdate(),@LoggedInUserID,@SystemID,@UserType,@ComplianceID)
        
END
