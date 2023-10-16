


--CreatedBy: BALWINDER SINGH
--CreatedDate: 09-04-2020
--Description: Get MAX InvoiceNumber

CREATE PROCEDURE [dbo].[GetInvoiceNumber]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT OFF;
	DECLARE @MAXNUM BIGINT;
    -- Insert statements for procedure here
	set @MAXNUM=1001;
	IF(EXISTS(SELECT 1 FROM dbo.Invoice))
	BEGIN
	SET @MAXNUM=(SELECT MAX(InvoiceNumber)+1 FROM dbo.Invoice NOLOCK)
	END;	
	SELECT  @MAXNUM
END