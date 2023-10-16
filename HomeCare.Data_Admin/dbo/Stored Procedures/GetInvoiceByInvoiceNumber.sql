


--CreatedBy: BALWINDER SINGH
--CreatedDate: 09-04-2020
--Description: Get Invoice details  By InvoiceNumber

CREATE PROCEDURE [dbo].[GetInvoiceByInvoiceNumber]
	-- Add the parameters for the stored procedure here
	@InvoiceNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	SELECT * FROM dbo.Invoice  WITH(NOLOCK) WHERE InvoiceNumber =@InvoiceNumber
END