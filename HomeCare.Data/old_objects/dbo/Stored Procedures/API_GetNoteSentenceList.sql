/****** Object:  StoredProcedure [dbo].[API_GetNoteSentenceList]    Script Date: 2/1/2020 1:40:55 AM by Satya ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[API_GetNoteSentenceList]
AS
BEGIN
	SELECT * FROM NoteSentences WHERE IsDeleted=0
END