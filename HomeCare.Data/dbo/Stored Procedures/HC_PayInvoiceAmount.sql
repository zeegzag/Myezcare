-- EXEC HC_PayInvoiceAmount 2,1,4258,0,2,3,4,'2018-10-23',1  
CREATE PROCEDURE [dbo].[HC_PayInvoiceAmount]  
 @PaymentType INT,  
 @InvoiceId BIGINT,  
 @ReferralId BIGINT,  
 @Amount DECIMAL(18,2),  
 @InvoiceStatus_Paid INT,  
 @InvoiceStatus_PartialPaid INT,  
 @InvoiceStatus_Void INT,  
 @ServerDateTime DATETIME,  
 @LoggedInID BIGINT  
AS  
BEGIN  
 BEGIN TRANSACTION trans  
 BEGIN TRY  
    
  IF (@PaymentType=@InvoiceStatus_Paid)  
   BEGIN  
    INSERT INTO ReferralPaymentHistories (ReferralInvoiceID,PaymentDate,PaidAmount,IsDeleted,CreatedDate,CreatedBy)  
    SELECT ReferralInvoiceID,@ServerDateTime,PayAmount=PayAmount-ISNULL(PaidAmount,0),0,@ServerDateTime,@LoggedInID   
    FROM ReferralInvoices   
    WHERE ReferralID=@ReferralId AND ReferralInvoiceID=@InvoiceId  
  
    UPDATE ReferralInvoices 
	SET PaidAmount=PayAmount,InvoiceStatus=@InvoiceStatus_Paid 
	WHERE ReferralID=@ReferralId AND ReferralInvoiceID=@InvoiceId  

   END  
  ELSE IF (@PaymentType=@InvoiceStatus_PartialPaid)  
   BEGIN  
    INSERT INTO ReferralPaymentHistories (ReferralInvoiceID,PaymentDate,PaidAmount,IsDeleted,CreatedDate,CreatedBy)  
    SELECT @InvoiceId,@ServerDateTime,@Amount,0,@ServerDateTime,@LoggedInID  
      
    DECLARE @TotalPaidAmount DECIMAL(18,2),@PayAmount DECIMAL(18,2),@InvoiceStatus INT  
  
    SELECT @TotalPaidAmount = ISNULL(PaidAmount,0) + @Amount,@PayAmount = PayAmount  
    FROM ReferralInvoices   
    WHERE ReferralID=@ReferralId AND ReferralInvoiceID=@InvoiceId  
  
    IF (@TotalPaidAmount >= @PayAmount)  
     BEGIN  
      SET @InvoiceStatus = @InvoiceStatus_Paid  
     END  
    ELSE  
     BEGIN  
      SET @InvoiceStatus = @InvoiceStatus_PartialPaid  
     END  
  
    UPDATE ReferralInvoices SET PaidAmount=@TotalPaidAmount,InvoiceStatus=@InvoiceStatus WHERE ReferralID=@ReferralId AND ReferralInvoiceID=@InvoiceId  
   END  
  ELSE IF (@PaymentType=@InvoiceStatus_Void)  
   BEGIN  
    UPDATE ReferralInvoices SET InvoiceStatus=@InvoiceStatus_Void WHERE ReferralID=@ReferralId AND ReferralInvoiceID=@InvoiceId  
   END  
  SELECT 1 AS TransactionResultId;  
  EXEC HC_GetInvoiceDetail @InvoiceId  
  IF @@TRANCOUNT > 0  
   BEGIN  
    COMMIT TRANSACTION trans  
   END  
 END TRY  
 BEGIN CATCH  
  SELECT -1 AS TransactionResultId;  
  EXEC HC_GetInvoiceDetail @InvoiceId  
  IF @@TRANCOUNT > 0  
   BEGIN  
    ROLLBACK TRANSACTION trans  
   END  
 END CATCH  
END