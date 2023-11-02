CREATE PROCEDURE [dbo].[HC_DeleteReferralDocument]    
@ReferralDocumentID BIGINT,      
@EbriggsFormMppingID BIGINT,    
@CurrentDateTime DATETIME,    
@LoggedInID BIGINT,  
@IsDeleted BIT  
AS          
BEGIN          
    
IF(@ReferralDocumentID>0)   
IF(@IsDeleted='False')  
--DELETE FROM ReferralDocuments WHERE ReferralDocumentID=@ReferralDocumentID      
  update ReferralDocuments set IsDeleted=1 WHERE ReferralDocumentID=@ReferralDocumentID    
  ELSE  
  update ReferralDocuments set IsDeleted=0 WHERE ReferralDocumentID=@ReferralDocumentID    
IF(@EbriggsFormMppingID>0)   
 IF(@IsDeleted='True')  
 UPDATE EbriggsFormMppings SET IsDeleted=1,UpdatedDate=@CurrentDateTime,UpdatedBy=@LoggedInID    
 WHERE EbriggsFormMppingID=@EbriggsFormMppingID   
 ELSE  
 UPDATE EbriggsFormMppings SET IsDeleted=0,UpdatedDate=@CurrentDateTime,UpdatedBy=@LoggedInID    
 WHERE EbriggsFormMppingID=@EbriggsFormMppingID   
      
SELECT 1 RETURN;      
          
END    
  