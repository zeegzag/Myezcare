-- EXEC UpdateFormPrice @EBFormID = '5bee5a6f8a063bc765ef1bda', @FormPrice = '5.23', @LoggedInID = '1'

CREATE PROCEDURE [dbo].[UpdateFormPrice]
@EBFormID NVARCHAR(MAX),
@FormPrice float,
@LoggedInID BIGINT=0
AS
BEGIN

IF(@FormPrice='' OR @FormPrice=0) 
SET @FormPrice=NULL;


UPDATE EBForms SET FormPrice=CONVERT(DECIMAL(10,2),@FormPrice),UpdatedDate=GETUTCDATE(),UpdatedBy=@LoggedInID WHERE EBFormID=@EBFormID


END