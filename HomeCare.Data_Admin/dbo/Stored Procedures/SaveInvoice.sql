

--CreatedBy: BALWINDER SINGH
--CreatedDate: 09-04-2020
--Description: Save New Invoice FOR ADMIN UI


CREATE PROCEDURE [dbo].[SaveInvoice]
	-- Add the parameters for the stored procedure here
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
    INSERT INTO dbo.Invoice
	(	MonthId,	OrganizationId,	InvoiceDate,	PlanName ,	DueDate ,	ActivePatientQuantity,	--ActivePatientUnit ,	ActivePatientAmount ,
	NumberOfTimeSheetQuantity ,	--NumberOfTimeSheetUnit ,	NumberOfTimeSheetAmount ,	
	IVRQuantity ,	--IVRUnit ,	IVRAmount,	
	MessageQuantity ,	--MessageUnit,	MessageAmount,	
	ClaimsQuantity,	--ClaimsUnit	,	ClaimsAmount,	
	FormsQuantity,	--FormsUnit,	FormsAmount	,	
	InvoiceStatus,
	Status,	CreatedDate,InvoiceAmount
	) 
	VALUES(
	@MonthId,	@OrganizationId,	@InvoiceDate,	@PlanName ,	@DueDate ,	@ActivePatientQuantity,--	@ActivePatientUnit ,	@ActivePatientAmount ,
	@NumberOfTimeSheetQuantity ,--@NumberOfTimeSheetUnit ,	@NumberOfTimeSheetAmount ,	
	@IVRQuantity ,	--@IVRUnit ,	@IVRAmount,	
	@MessageQuantity ,	--@MessageUnit,	@MessageAmount,
	@ClaimsQuantity,--@ClaimsUnit	,	@ClaimsAmount,	
	@FormsQuantity,	--@FormsUnit,	@FormsAmount,
	@InvoiceStatus,	@Status,	(select GETDATE())--,	
	,@InvoiceAmount
	)
    COMMIT TRANSACTION
	 SELECT @@IDENTITY;
	END TRY    
	BEGIN CATCH   
	    IF @@TRANCOUNT > 0  
	    ROLLBACK TRANSACTION  
		SELECT 2  --RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)  
END CATCH  

END