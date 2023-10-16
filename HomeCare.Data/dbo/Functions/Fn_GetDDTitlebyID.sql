

-- =============================================
-- Author:		Pallav Saxena
-- Create date: 10 Dec 2021
-- Description:	Function returns Description for the DD Master
-- =============================================
Create FUNCTION [dbo].[Fn_GetDDTitlebyID]
(
	@DDMasterID bigint
)
RETURNS VARCHAR(MAX)
AS
begin

	DECLARE @temp VARCHAR(MAX)
	SELECT @temp = Title
	FROM DDMaster WHERE DDMasterID=@DDMasterID
	return @temp

end