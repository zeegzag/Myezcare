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
GO

-- =============================================
-- Author:		Fenil Gandhi
-- Create date: 26 Jun 2020
-- Description:	This TVF is used to return subject and body after replacing actual data.
-- =============================================
CREATE FUNCTION [notif].[GetMessage]
(
	@TemplateID BIGINT,
	@TemplateData VARCHAR(MAX)
)
RETURNS @Template TABLE 
(
	[Subject] VARCHAR(500),
	[Body] VARCHAR(MAX)
)
AS
BEGIN
	INSERT INTO @Template
	SELECT 
		[EmailTemplateSubject] [Subject],
		[notif].[GetMessageBody]([EmailTemplateBody], @TemplateData) [Body]
	FROM 
		[dbo].[EmailTemplates]
	WHERE 
		[EmailTemplateID] = @TemplateID;
	
	RETURN;
END
GO