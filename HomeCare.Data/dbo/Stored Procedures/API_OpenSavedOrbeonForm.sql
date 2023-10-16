CREATE PROCEDURE [dbo].[API_OpenSavedOrbeonForm]        
@ReferralDocumentID BIGINT   
AS        
BEGIN      
 declare @StoreType varchar(max)
select @StoreType=StoreType from ReferralDocuments RD  WHERE RD.ReferralDocumentID =@ReferralDocumentID     

IF(@StoreType='Orbeon')  
 BEGIN     
SELECT 1 [Version], FilePath NameForUrl,0 IsInternalForm, 1 IsOrbeonForm, RD.GoogleFileId [OrbeonFormID],  
    RD.ComplianceID,RD.StoreType   
  FROM ReferralDocuments RD  
 WHERE RD.ReferralDocumentID =@ReferralDocumentID      
    AND RD.StoreType = 'Orbeon'  
   END
   ELSE
   BEGIN
   SELECT 1 [Version], FilePath NameForUrl,0 IsInternalForm, 1 IsOrbeonForm, RD.GoogleFileId [OrbeonFormID],  
    RD.ComplianceID,RD.StoreType   
  FROM ReferralDocuments RD  
 WHERE RD.ReferralDocumentID =@ReferralDocumentID      
    --AND RD.StoreType = 'Orbeon'
   END
END
GO

