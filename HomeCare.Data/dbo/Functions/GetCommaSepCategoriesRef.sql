CREATE FUNCTION [dbo].[GetCommaSepCategoriesRef]
(
@ID bigint
)
RETURNS varchar(max) -- or whatever length you need
AS
BEGIN
DECLARE @Title VARCHAR(max)
SELECT @Title = COALESCE(@Title + ',', '') + d.Title from referralGroup rg,DDMaster d where d.DDMasterID=rg.val and rg.ReferralID= @ID and ISNUMERIC(rg.val) = 1
return @Title

END