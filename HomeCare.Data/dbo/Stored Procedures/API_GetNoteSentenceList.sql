CREATE PROCEDURE [dbo].[API_GetNoteSentenceList]
AS
BEGIN
	SELECT * FROM NoteSentences WHERE IsDeleted=0
END