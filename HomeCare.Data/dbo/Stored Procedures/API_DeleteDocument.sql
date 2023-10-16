CREATE PROCEDURE [dbo].[API_DeleteDocument]    
@ReferralDocumentID BIGINT,      
@EbriggsFormMppingID BIGINT,    
@ServerCurrentDateTime DATETIME,    
@LoggedInID BIGINT,  
@SystemID NVARCHAR(100)  
AS          
BEGIN          
    
IF(@ReferralDocumentID>0)    
DELETE FROM ReferralDocuments WHERE ReferralDocumentID=@ReferralDocumentID      
    
IF(@EbriggsFormMppingID>0)    
UPDATE EbriggsFormMppings SET IsDeleted=1,UpdatedDate=@ServerCurrentDateTime,UpdatedBy=@LoggedInID,SystemID=@SystemID  
WHERE EbriggsFormMppingID=@EbriggsFormMppingID    
      
SELECT 1 RETURN;      
          
END