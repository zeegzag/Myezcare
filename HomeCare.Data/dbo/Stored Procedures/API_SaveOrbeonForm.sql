CREATE PROCEDURE [dbo].[API_SaveOrbeonForm]    
    @ReferralDocumentID BIGINT = NULL,      
    @FileName   VARCHAR(MAX),          
    @FilePath   VARCHAR(MAX),        
    @KindOfDocument  VARCHAR(20),        
    @ReferralID   BIGINT,  
    @ComplianceID  BIGINT,  
    @UserType   INT,        
    @LoggedInID  BIGINT,          
    @SystemID   VARCHAR(30),  
    @StoreType   VARCHAR(50)=null,  
    @GoogleFileId  varchar(255)=null,  
    @GoogleDetails  varchar(max)=null  
AS          
BEGIN          
    IF (ISNULL(@ReferralDocumentID, 0) > 0)  
        BEGIN  
            UPDATE ReferralDocuments  
                SET  
                    FilePath = @FilePath,  
                    KindOfDocument = @KindOfDocument,  
                    UserID = @ReferralID,  
                    UpdatedDate = getdate(),  
                    UpdatedBy = @LoggedInID,  
                    SystemID = @SystemID,  
                    UserType = @UserType,  
                    ComplianceID = @ComplianceID,  
                    StoreType = @StoreType,  
                    GoogleFileId = @GoogleFileId,  
                    GoogleDetails = @GoogleDetails  
                WHERE  
                    ReferralDocumentID = @ReferralDocumentID;  
        END  
    ELSE  
        BEGIN  
            INSERT INTO ReferralDocuments (FileName,FilePath,KindOfDocument,DocumentTypeID,UserID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,UserType,ComplianceID,StoreType,GoogleFileId,GoogleDetails)  
            VALUES(@FileName,@FilePath,@KindOfDocument,0,@ReferralID,getdate(),@LoggedInID,getdate(),@LoggedInID,@SystemID,@UserType,@ComplianceID,@StoreType,@GoogleFileId,@GoogleDetails)  
  
            SELECT @ReferralDocumentID = @@IDENTITY;  
        END  
      
    SELECT * FROM ReferralDocuments WHERE ReferralDocumentID = @ReferralDocumentID;  
END