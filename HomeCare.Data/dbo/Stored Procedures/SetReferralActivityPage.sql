CREATE PROC [dbo].[SetReferralActivityPage]
AS BEGIN

	SELECT * FROM ReferralActivityCategory

	SELECT ReferralID
		,ReferralName = dbo.GetGeneralNameFormat(FirstName, LastName)
		,AHCCCSID
	FROM Referrals
	WHERE IsDeleted = 0
	ORDER BY LastName ASC

	SELECT null Date,null ReferralActivityMasterId,null Description,null Initials  
END