CREATE PROCEDURE [dbo].[HC_GetMIFDetailForPDF]
@MIFFormID BIGINT
AS
BEGIN
	SELECT *,MemberFullName=dbo.GetGeneralNameFormat(R.FirstName,R.LastName),MedicaidNo=R.CISNumber,FrequencyCode=DD.Title
	FROM MIFDetails MD
	INNER JOIN Referrals R ON R.ReferralID=MD.ReferralID
	LEFT JOIN DDMaster DD ON DD.DDMasterID=MD.InitialServiceFrequencyID
	WHERE MIFFormID=@MIFFormID
END
