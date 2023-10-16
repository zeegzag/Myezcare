-- =============================================
-- Author:		<Author,,BALWINDER>
-- Create date: <Create Date,,09-04-2020>
-- Description:	<Description,,Update INVOICE BY MODEL ENTITY>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateInvoice]
	@InvoiceNumber BIGINT,
	@MonthId INT,
	@OrganizationId BIGINT,
	@InvoiceDate DATETIME,
	@PlanName NVARCHAR(50),
	@DueDate DATETIME,
	@ActivePatientQuantity INT,
	@ActivePatientUnit DECIMAL(18,2),
	@ActivePatientAmount DECIMAL(18,2),
	@NumberOfTimeSheetQuantity INT,
	@NumberOfTimeSheetUnit DECIMAL(18,2),
	@NumberOfTimeSheetAmount DECIMAL(18,2),
	@IVRQuantity INT,
	@IVRUnit DECIMAL(18,2),
	@IVRAmount DECIMAL(18,2),
	@MessageQuantity INT,
	@MessageUnit DECIMAL(18,2),
	@MessageAmount DECIMAL(18,2),
	@ClaimsQuantity INT,
	@ClaimsUnit	DECIMAL(18, 2),
	@ClaimsAmount	DECIMAL(18, 2),
	@FormsQuantity	INT,
	@FormsUnit	DECIMAL(18,2),
	@FormsAmount	DECIMAL(18,2),
	@InvoiceStatus	NVARCHAR(50),
	@Status	BIT=0,
	@InvoiceAmount DECIMAL(18,2)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--select * from dbo.Invoice


	BEGIN TRY    
    BEGIN TRANSACTION  
    UPDATE dbo.Invoice SET MonthId =@MonthId,	OrganizationId =@OrganizationId,	InvoiceDate=@InvoiceDate,	PlanName =@PlanName,	
	DueDate =@DueDate,	ActivePatientQuantity=@ActivePatientQuantity,	ActivePatientUnit=@ActivePatientUnit ,ActivePatientAmount=@ActivePatientAmount ,
	NumberOfTimeSheetQuantity=@NumberOfTimeSheetQuantity ,	NumberOfTimeSheetUnit=@NumberOfTimeSheetUnit ,	NumberOfTimeSheetAmount=@NumberOfTimeSheetAmount ,	
	IVRQuantity=@IVRQuantity ,	IVRUnit=@IVRUnit ,	IVRAmount=@IVRAmount,	
	MessageQuantity=@MessageQuantity ,	MessageUnit=@MessageUnit,	MessageAmount=@MessageAmount,	
	ClaimsQuantity=@ClaimsQuantity,	ClaimsUnit=@ClaimsUnit	,	ClaimsAmount=@ClaimsAmount,	
	FormsQuantity=@FormsQuantity,	FormsUnit=@FormsUnit,	FormsAmount=@FormsAmount	,	
	InvoiceStatus=@InvoiceStatus,
	Status=@Status,	UpdatedDate =GETDATE(),InvoiceAmount=@InvoiceAmount
	WHERE InvoiceNumber=@InvoiceNumber
	
    COMMIT TRANSACTION
	 SELECT 1;
	END TRY    
	BEGIN CATCH   
	    IF @@TRANCOUNT > 0  
	    ROLLBACK TRANSACTION  
		SELECT 2  --RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)  
END CATCH  

END