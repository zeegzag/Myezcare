 --  EXEC GetReferralMappedDXcodeList @ReferralID=46
CREATE PROCEDURE GetReferralMappedDXcodeList
@ReferralID BIGINT =0,
@LoggedInUser BIGINT =0

AS
BEGIN
SELECT RDM.ReferralDXCodeMappingID,RDM.ReferralID,RDM.DXCodeID,RDM.Precedence ,DX.DXCodeName, DX.DXCodeWithoutDot, DX.Description
FROM ReferralDXCodeMappings RDM
INNER JOIN DXCodes DX ON DX.DXCodeID=RDM.DXCodeID
WHERE ReferralID=@ReferralID
ORDER BY RDM.Precedence 

END

