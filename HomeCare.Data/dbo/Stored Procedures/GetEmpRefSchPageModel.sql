CREATE PROCEDURE [dbo].[GetEmpRefSchPageModel]
@Preference_Skill VARCHAR(50),
@Preference_Preference VARCHAR(50)
AS
BEGIN
SELECT * FROM Preferences WHERE IsDeleted=0 AND KeyType=@Preference_Preference
 SELECT * FROM Preferences WHERE IsDeleted=0 AND KeyType=@Preference_Skill
END