CREATE PROCEDURE [dbo].[API_OpenSavedOrbeonForm]      
@ReferralDocumentID BIGINT 
AS      
BEGIN    
    

    
SELECT 1 [Version], FilePath NameForUrl,0 IsInternalForm, 1 IsOrbeonForm, RD.GoogleFileId [OrbeonFormID],
    RD.ComplianceID 
  FROM ReferralDocuments RD
   
 WHERE RD.ReferralDocumentID =@ReferralDocumentID    
    AND RD.StoreType = 'Orbeon'
   
END 