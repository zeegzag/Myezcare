
-- =============================================
-- Author:		Ali Bukhari
-- Create date: 21 Jan 2021
-- Description:	Function returns caretypes for multiple caretypeids
-- =============================================
CREATE FUNCTION [dbo].[Fn_GetCareTypes]
(
	@CareTypeIds varchar(max)
)
RETURNS VARCHAR(MAX)
AS
begin

	DECLARE @temp VARCHAR(MAX)
	SELECT @temp = COALESCE(@temp+', ' ,'') + Title
	FROM DDMaster WHERE Charindex(',' + CAST(DDMasterID  as Varchar)+ ',', ','+ @CareTypeIds + ',') > 0
	return @temp

end


GO
