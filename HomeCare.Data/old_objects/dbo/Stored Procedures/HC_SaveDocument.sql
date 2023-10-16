--EXEC HC_SaveDocument @ReferralDocumentID = '1', @FileName = 'ClaimsOutcomeStatus_20181002120651.xlsx', @KindOfDocument = 'Internal', @UserID = '24234', @DocumentTypeID = '2', @UpdatedDate = '2018/10/11 18:26:46', @LoggedInUserID = '1', @SystemID = '::1'
  
--EXEC HC_SaveDocument @ReferralDocumentID = '255', @FileName = 'PcaTimeSheet_10_08_2018_19_24_00.pdf', @KindOfDocument = 'Internal', @UserID = '3960', @DocumentTypeID = '2', @UpdatedDate = '2018/10/16 16:27:41', @LoggedInUserID = '1', @SystemID = '::1'
CREATE PROCEDURE [dbo].[HC_SaveDocument]    
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


		INSERT INTO ReferralComplianceMappings (ReferralID,ComplianceID,Value,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)
		SELECT RD.UserID,RD.DocumentTypeID,1,@UpdatedDate,@LoggedInUserID,@UpdatedDate,@LoggedInUserID,@SystemID FROM ReferralDocuments RD
		LEFT JOIN ReferralComplianceMappings RCM ON RCM.ReferralID=RD.UserID AND RCM.ComplianceID=RD.DocumentTypeID
		WHERE RD.UserID=@UserID AND RCM.ReferralComplianceID IS NULL AND DocumentTypeID>0

		UPDATE ReferralComplianceMappings SET Value=0 WHERE ReferralID=@ReferralID

		Update RCM SET RCM.Value=1
		FROM ReferralComplianceMappings RCM
		INNER JOIN ReferralDocuments RD ON RCM.ReferralID=RD.UserID AND RCM.ComplianceID=RD.DocumentTypeID
		WHERE RD.UserID=@UserID

		--UPDATE ReferralComplianceMappings SET Value=1 WHERE ReferralID=@UserID AND ComplianceID=@DocumentTypeID

		--UPDATE ReferralComplianceMappings SET Value=0 WHERE ReferralID=@UserID

		--UPDATE ReferralComplianceMappings SET Value=1 WHERE ReferralComplianceID IN (SELECT RCM.ReferralComplianceID FROM ReferralDocuments RD
		--INNER JOIN ReferralComplianceMappings RCM ON RCM.ReferralID=RD.UserID AND RCM.ComplianceID=RD.DocumentTypeID
		--WHERE RCM.ReferralID=@UserID)
		 --ReferralID=@UserID AND ComplianceID=@DocumentTypeID
  
SELECT 1 RETURN;  
      
END
