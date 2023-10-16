-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 26 Jun 2020
-- Description:	This SVF is used to replace body with actual data.
-- =============================================
CREATE FUNCTION [notif].[GetMessageBody]
(
	@TemplateBody VARCHAR(MAX),
	@TemplateData VARCHAR(MAX)
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	SELECT @TemplateBody = REPLACE(@TemplateBody, KP.[key], KP.[value])
	FROM 
		OPENJSON(@TemplateData) R
	CROSS APPLY 
		OPENJSON(R.[value]) KP;
	
	RETURN @TemplateBody;
END