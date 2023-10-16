CREATE FUNCTION [dbo].[Fn_GetPayor]
(
	@ID varchar(max)
)
RETURNS VARCHAR(MAX)
AS
BEGIN
DECLARE @Title VARCHAR(max)
SELECT @Title = COALESCE(@Title + ',', '') + p.PayorName from ReferralPayorMappings rpm, Payors p where p.PayorID=rpm.PayorID and rpm.IsDeleted=0 and p.IsDeleted=0 and rpm.ReferralID= @ID
return @Title

END


