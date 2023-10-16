CREATE PROCEDURE [dbo].[HC_DeleteReferralDocument]
@ReferralDocumentID BIGINT,  
@EbriggsFormMppingID BIGINT,
@CurrentDateTime DATETIME,
@LoggedInID BIGINT
AS      
BEGIN      

IF(@ReferralDocumentID>0)
DELETE FROM ReferralDocuments WHERE ReferralDocumentID=@ReferralDocumentID  

IF(@EbriggsFormMppingID>0)
UPDATE EbriggsFormMppings SET IsDeleted=1,UpdatedDate=@CurrentDateTime,UpdatedBy=@LoggedInID
WHERE EbriggsFormMppingID=@EbriggsFormMppingID
  
SELECT 1 RETURN;  
      
END