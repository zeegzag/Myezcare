
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