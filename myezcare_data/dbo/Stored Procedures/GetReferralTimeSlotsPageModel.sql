CREATE PROCEDURE [dbo].[GetReferralTimeSlotsPageModel]
	@DDType_CareType INT = 1
AS
BEGIN
	SELECT Value=ReferralID, Name=LastName+', ' +FirstName  FROM Referrals WHERE IsDeleted=0 ORDER BY LastName ASC
	
	SELECT Name=Title,ID=DDMasterID FROM DDMaster where IsDeleted=0 and ItemType=@DDType_CareType
END
