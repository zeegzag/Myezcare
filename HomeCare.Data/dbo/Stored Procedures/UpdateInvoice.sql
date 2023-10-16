CREATE PROCEDURE [dbo].[UpdateInvoice] 
(
@ReferralInvoiceID BIGINT = NULL ,
@InvoiceDueDate NVARCHAR(max) = NULL 
)
AS  
BEGIN  
  BEGIN TRANSACTION trans;  
  
  BEGIN TRY  
    UPDATE ReferralInvoices  
    SET InvoiceDueDate = @InvoiceDueDate  
    WHERE ReferralInvoiceID = @ReferralInvoiceID
  
    SELECT 1 AS TransactionResultId;  
  
    IF @@TRANCOUNT > 0  
    BEGIN  
      COMMIT TRANSACTION trans;  
    END  
  END TRY  
  
  BEGIN CATCH  
    SELECT - 1 AS TransactionResultId,  
      ERROR_MESSAGE() AS ErrorMessage;  
  
    IF @@TRANCOUNT > 0  
    BEGIN  
      ROLLBACK TRANSACTION trans;  
    END  
  END CATCH  
END  
  