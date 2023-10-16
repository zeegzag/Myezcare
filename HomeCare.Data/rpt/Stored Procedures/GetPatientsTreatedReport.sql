-- =============================================
-- Author:		Kalpesh Patel
-- Create date: 20/4/2020
-- Description:	Treated Patient Report
-- EXEc [Rpt].[GetPatientsTreatedReport] @FromDate=NULL,@ToDate=NULL
-- =============================================
CREATE PROC [rpt].[GetPatientsTreatedReport]
	@FromDate AS DATE=NULL,
	@Todate AS DATE=NULL
AS
BEGIN
 SELECT ReferralID,PatientName,0 AS AdmissionID,SSN,DXCodeWithoutDot,Description,SOCDate
FROM
(
select r.ReferralID AS ReferralID,
	PatientName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName),
	r.SocialSecurityNumber AS SSN,
	dc.DXCodeWithoutDot AS DXCodeWithoutDot,
	dc.Description AS Description,
CONVERT(VARCHAR(10), sm.startDate, 111) as SOCDate
from Referrals r
inner join ReferralDXCodeMappings rdm on  rdm.ReferralID=r.ReferralID
inner join DXCodes dc on dc.DXCodeID=rdm.DXCodeID
INNER JOIN ScheduleMasters SM ON SM.ReferralID=r.ReferralID
Where r.IsDeleted=0
AND ((@FromDate is null or @ToDate is null) or  sm.startDate BETWEEN @FromDate AND @Todate)
) T
END