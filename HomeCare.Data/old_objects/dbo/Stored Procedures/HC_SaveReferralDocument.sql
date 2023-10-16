CREATE PROCEDURE [dbo].[HC_SaveReferralDocument]      
	@FileName			VARCHAR(MAX),        
	@FilePath			VARCHAR(MAX),      
	@KindOfDocument		VARCHAR(20),      
	--@DocumentTypeID BIGINT=0,      
	@ReferralID			BIGINT,
	@ComplianceID		BIGINT,
	@UserType			INT,      
	@LoggedInUserID		BIGINT,        
	@SystemID			VARCHAR(30),
	@StoreType			VARCHAR(50),
	@GoogleFileId		varchar(255),
	@GoogleDetails		varchar(max)
AS        
BEGIN        
     
	IF(@StoreType = 'Orbeon')
	BEGIN
		DECLARE @count INT = 0
		SELECT @count= COUNT(*) FROM ReferralDocuments WHERE StoreType = 'Orbeon' AND GoogleFileId = @GoogleFileId

		IF(@count > 0) BEGIN
			RETURN
		END
	END
	INSERT INTO ReferralDocuments (FileName,FilePath,KindOfDocument,DocumentTypeID,UserID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,UserType,ComplianceID,StoreType,GoogleFileId,GoogleDetails)
	VALUES(@FileName,@FilePath,@KindOfDocument,0,@ReferralID,getdate(),@LoggedInUserID,getdate(),@LoggedInUserID,@SystemID,@UserType,@ComplianceID,@StoreType,@GoogleFileId,@GoogleDetails)
        
END
