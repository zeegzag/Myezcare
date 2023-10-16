

CREATE PROCEDURE [dbo].[GetContactInformation]
	@ReferralID BIGINT = 0
AS
BEGIN
	SELECT CM.ContactMappingID, 
	CT.ContactTypeID ,CT.ContactTypeName,CM.ROIType,CM.ROIExpireDate,
	C.FirstName, C.LastName, C.[Address], C.LanguageID,
	C.PHONE1,
	C.Phone2, 
	C.City,
	C.State,
	C.ZipCode,
	CM.IsDCSLegalGuardian,
	CM.IsEmergencyContact,
	CM.IsNoticeProviderOnFile,
	CM.IsPrimaryPlacementLegalGuardian,
	C.ContactID,
	CM.ReferralID,
	CM.ClientID, 
	IsNull(E.FirstName, R.FirstName) AS EmpFirstName, 
	IsNull(E.LastName, R.LastName) AS EmpLastName
	FROM Contacts C
	INNER JOIN ContactMappings CM ON CM.ContactID = C.ContactID
	INNER JOIN ContactTypes CT ON CT.ContactTypeID = CM.ContactTypeID
	Left Outer JOIN Employees E ON E.EmployeeID = C.CreatedBy
	Left Outer JOIN Referrals R ON R.ReferralID = C.CreatedBy
	WHERE CM.REFERRALID= @ReferralID
END

