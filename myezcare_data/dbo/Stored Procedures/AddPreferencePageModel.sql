CREATE PROCEDURE [dbo].[AddPreferencePageModel]
@PreferenceID BIGINT
AS
BEGIN

SELECT * FROM Preferences WHERE PreferenceID=@PreferenceID

END
