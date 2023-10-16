-- GetReferralDetails 24254
CREATE PROCEDURE [dbo].[GetReferralDetails]
@ReferralID BIGINT
AS
BEGIN

	DECLARE @CareGiverName VARCHAR(100)
	DECLARE @AgencyID BIGINT
	DECLARE @AgencyPhone VARCHAR(50)
	DECLARE @AgencyAddress VARCHAR(MAX)
	DECLARE @AgencyTIN VARCHAR(100)
	DECLARE @AgencyEIN VARCHAR(100)
	DECLARE @AgencyMobile VARCHAR(100)
	DECLARE @AgencyCity VARCHAR(100)
	DECLARE @AgencyState VARCHAR(100)
	DECLARE @AgencyZipCode VARCHAR(100)
	SELECT
		TOP 1
		@CareGiverName = A.NickName,
		@AgencyID = A.AgencyID,
		@AgencyPhone = A.Phone,
		@AgencyAddress = A.[Address],
		@AgencyCity = A.City,
		@AgencyState = S.StateName,
		@AgencyZipCode = A.ZipCode,
		@AgencyTIN = A.TIN,
		@AgencyEIN = A.EIN,
		@AgencyMobile = A.Mobile
	FROM
		ReferralCareGivers RCG
		INNER JOIN Agencies A ON RCG.AgencyID = A.AgencyID
		LEFT JOIN States S ON A.StateCode = S.StateCode
	WHERE
		RCG.ReferralID = @ReferralID
	ORDER BY
		RCG.ReferralCareGiverID DESC

	SELECT
		AHCCCSID,
		HealthPlan,
		@CareGiverName AS CareGiver,
		@AgencyID AS AgencyID,
		@AgencyPhone AS AgencyPhone,
		@AgencyAddress AS AgencyAddress,
		@AgencyCity AS AgencyCity,
		@AgencyState AS AgencyState,
		@AgencyZipCode AS AgencyZipCode,
		@AgencyTIN AS AgencyTIN,
		@AgencyEIN AS AgencyEIN,
		@AgencyMobile AS AgencyMobile,
		CM.FirstName + ' ' + CM.LastName AS CaseManagerName
	FROM
		Referrals R
		LEFT JOIN CaseManagers CM ON R.CaseManagerID = CM.CaseManagerID
	WHERE
		ReferralID = @ReferralID

	SELECT
		C.FirstName,
		C.LastName,
		C.Address,
		C.City,
		C.State,
		C.ZipCode,
		C.Phone1,
		C.Phone2 
	FROM
		ContactMappings CM
		INNER JOIN Contacts C ON CM.ContactID = C.ContactID
	WHERE
		CM.ReferralID = @ReferralID

	SELECT
		RBA.Type,
		RBA.AuthorizationCode,
		RBA.StartDate,
		RBA.EndDate,
		P.PayorID,
		P.PayorName
	FROM
		ReferralBillingAuthorizations RBA
		LEFT JOIN Payors P ON RBA.PayorID = P.PayorID
	WHERE
		RBA.ReferralID = @ReferralID

	SELECT
		D.DxCodeType,
		D.DXCodeName,
		D.Description,
		RDM.Precedence
	FROM
		ReferralDXCodeMappings RDM
		INNER JOIN DXCodes D ON RDM.DXCodeID = D.DXCodeID
	WHERE
		RDM.ReferralID = @ReferralID

	SELECT
		RPM.ReferralPayorMappingID,
		RPM.PayorID,P.PayorName,
		RPM.PayorEffectiveDate,
		RPM.PayorEffectiveEndDate,
		RPM.Precedence,
		RPM.IsPayorNotPrimaryInsured
	FROM
		ReferralPayorMappings RPM
		INNER JOIN Payors P ON RPM.PayorID = P.PayorID
	WHERE
		RPM.ReferralID = @ReferralID
END
