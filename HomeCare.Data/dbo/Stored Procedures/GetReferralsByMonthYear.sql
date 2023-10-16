
CREATE PROC [dbo].[GetReferralsByMonthYear]
@Month NVARCHAR(200),@Year INT,@AddOrEdit VARCHAR(10)
AS BEGIN

	--SELECT * FROM ReferralActivityCategory
	IF(@AddOrEdit='Add')
	begin
		SELECT ReferralID
			,ReferralName = dbo.GetGeneralNameFormat(FirstName, LastName)
			,AHCCCSID
		FROM Referrals
		WHERE IsDeleted = 0
		and ReferralID not in (
		select referralId from ReferralActivityMaster where Month=@Month and Year=@Year
		)
		ORDER BY LastName ASC
	end
	else if(@AddOrEdit='Edit')
	begin
		SELECT ReferralID
		,ReferralName = dbo.GetGeneralNameFormat(FirstName, LastName)
		,AHCCCSID
		FROM Referrals
		WHERE IsDeleted = 0
		and ReferralID  in (
		select referralId from ReferralActivityMaster where Month=@Month and Year=@Year
		)
		ORDER BY LastName ASC
	end
END

