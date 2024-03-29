/*
Created by : Neeraj Sharma
Created Date: 02 August 2020
Updated by :
Updated Date :

Purpose: This stored procedure is used to update invoice table in Admin datable for the acknowledgement of payment
 from authorised.net.

*/


CREATE PROCEDURE [dbo].[UpdateInvoicefromAuthorizeNetDetails]
	@NINJAInvoiceNumber BIGINT,
	@TransactionIdAuthNet nvarchar(max),
	@ResponseCodeAuthNet nvarchar(max),
	@MessageCodeAuthNet nvarchar(max),
    @DescriptionAuthNet nvarchar(max),
	@AuthCodeAuthNet nvarchar(max),
	@OrganizationId BIGINT,
	@Statuscode nvarchar(MAX),
    @ErrorCode nvarchar(MAX),
    @ErrorText nvarchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
  
	BEGIN TRY    
    BEGIN TRANSACTION  
 
	if exists(select NinjaInvoiceNumber from dbo.Invoice where NinjaInvoiceNumber=@NINJAInvoiceNumber)
	Begin
	   UPDATE dbo.Invoice SET 	TransactionIdAuthNet=@TransactionIdAuthNet,ResponseCodeAuthNet=@ResponseCodeAuthNet,
	MessageCodeAuthNet=@MessageCodeAuthNet,DescriptionAuthNet=@DescriptionAuthNet,AuthCodeAuthNet=@AuthCodeAuthNet
	,OrganizationId=@OrganizationId,
	UpdatedDate =GETDATE()
	,Statuscode=@Statuscode,ErrorCode=@ErrorCode, ErrorText=@ErrorText
	WHERE NinjaInvoiceNumber=@NINJAInvoiceNumber

	End 
	else
	Begin

	INSERT INTO [dbo].[Invoice]
           ([OrganizationId]
           ,[InvoiceDate]
           ,[PlanName]
           ,[DueDate]
           ,[ActivePatientQuantity]
           ,[ActivePatientUnit]
           ,[ActivePatientAmount]
           ,[NumberOfTimeSheetQuantity]
           ,[NumberOfTimeSheetUnit]
           ,[NumberOfTimeSheetAmount]
           ,[IVRQuantity]
           ,[IVRUnit]
           ,[IVRAmount]
           ,[MessageQuantity]
           ,[MessageUnit]
           ,[MessageAmount]
           ,[ClaimsQuantity]
           ,[ClaimsUnit]
           ,[ClaimsAmount]
           ,[FormsQuantity]
           ,[FormsUnit]
           ,[FormsAmount]
           ,[InvoiceStatus]
           ,[Status]
           ,[IsPaid]
           ,[PaymentDate]
           ,[CreatedDate]
           ,[UpdatedDate]
           ,[MonthId]
           ,[PaidAmount]
           ,[InvoiceAmount]
           ,[FilePath]
           ,[OrginalFileName]
           ,[TrancationId]
           ,[TransactionId]
           ,[TransactionIdAuthNet]
           ,[ResponseCodeAuthNet]
           ,[MessageCodeAuthNet]
           ,[DescriptionAuthNet]
           ,[AuthCodeAuthNet]
		   ,[NinjaInvoiceNumber]
		     ,[Statuscode]
		   ,[ErrorCode]
		   ,[ErrorText]
		   )
     VALUES
           (@OrganizationId,
           getdate(),
            null,
           getdate(),
            1,        
            null,
            null,
            null,
             null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
			null,
            1,
           getdate(),
           getdate(),
           getdate(),
		   null,
            null,
            null,
            null,
            null,
            null,
            null,
           @TransactionIdAuthNet,
	       @ResponseCodeAuthNet,
	       @MessageCodeAuthNet,
           @DescriptionAuthNet,
	       @AuthCodeAuthNet,
		   @NINJAInvoiceNumber,
		   @Statuscode,
			@ErrorCode,
			@ErrorText
		   )
 
	End

	
    COMMIT TRANSACTION
	 SELECT 1;
	END TRY    
	BEGIN CATCH   
	    IF @@TRANCOUNT > 0  
	    ROLLBACK TRANSACTION  
		SELECT 2  --RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)  
END CATCH  

END
