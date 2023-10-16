CREATE PROCEDURE [dbo].[HC_DeleteDocument]
@ReferralDocumentID BIGINT,
@UserID BIGINT,
@DocumentTypeID INT
AS    
BEGIN    

DELETE FROM ReferralDocuments WHERE ReferralDocumentID=@ReferralDocumentID

UPDATE ReferralComplianceMappings SET Value=0 WHERE ReferralID=@UserID AND ComplianceID=@DocumentTypeID

SELECT 1 RETURN;
    
END