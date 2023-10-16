CREATE FUNCTION [dbo].[GetAggregatorInfo]
(
  @ScheduleID BIGINT
)
RETURNS @returntable TABLE 
(
  ScheduleID BIGINT,
  EmployeeID BIGINT,
  AnyTimeClockIn BIT,
  AuthorizationCode NVARCHAR(MAX),
  Rate NVARCHAR(MAX),
  PayorName NVARCHAR(MAX),
  PayorShortName NVARCHAR(MAX),
  PayorSubmissionName NVARCHAR(MAX),
  ClaimProcessor NVARCHAR(MAX),
  VisitBilledBy NVARCHAR(MAX),
  PayorIdentificationNumber NVARCHAR(MAX),
  NPINumber NVARCHAR(MAX),
  AgencyTaxNumber NVARCHAR(MAX),
  AgencyNPID NVARCHAR(MAX),
  CareTypeValue NVARCHAR(MAX),
  BeneficiaryNumber NVARCHAR(MAX),
  MemberID NVARCHAR(MAX),
  JurisdictionCode NVARCHAR(MAX),
  TimezoneCode NVARCHAR(MAX),
  ServiceCode NVARCHAR(MAX),
  ModifierID NVARCHAR(MAX)
)
AS
BEGIN

  INSERT @returntable
  SELECT TOP 1
	SM.ScheduleID,
    SM.EmployeeID,
	SM.AnyTimeClockIn,
    RBA.AuthorizationCode,
    RBA.Rate,
    P.PayorName,
    P.ShortName PayorShortName,
    P.PayorSubmissionName,
    P.ClaimProcessor,
    P.VisitBilledBy,
    P.PayorIdentificationNumber,
    P.NPINumber,
    P.AgencyTaxNumber,
    P.AgencyNPID,
    CT.[Value] CareTypeValue,
    RPM.BeneficiaryNumber,
    RPM.MemberID,
    MJ.Code JurisdictionCode,
    MT.Code TimezoneCode,
    SC.ServiceCode,
    SC.ModifierID
  FROM ScheduleMasters SM
  LEFT JOIN ReferralBillingAuthorizations RBA
    ON RBA.ReferralBillingAuthorizationID = SM.ReferralBillingAuthorizationID
  LEFT JOIN Payors P
    ON P.PayorID = RBA.PayorID
  LEFT JOIN ReferralPayorMappings RPM
    ON RPM.ReferralID = SM.ReferralID
      AND RPM.PayorID = P.PayorID
      AND RPM.IsDeleted = 0
  LEFT JOIN ServiceCodes SC
    ON SC.ServiceCodeID = RBA.ServiceCodeID
  LEFT JOIN MasterJurisdictions MJ
    ON MJ.MasterJurisdictionID = RPM.MasterJurisdictionID
  LEFT JOIN MasterTimezones MT
    ON MT.MasterTimezoneID = RPM.MasterTimezoneID
  LEFT JOIN DDMaster CT
    ON CT.ItemType = 1 AND CT.DDMasterID = RBA.CareType
  WHERE SM.ScheduleID = @ScheduleID

  RETURN
END
