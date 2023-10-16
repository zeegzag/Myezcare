-- =============================================
-- Author:		Kalpesh Patel
-- Create date: 20/4/2020
-- Description:	Treated Patient Report
-- EXEc [rpt].[GetDiagnosisReportData] @PayorID=0,@FromDate='1/1/2020',@ToDate='5/24/2020'
-- =============================================
CREATE PROC [rpt].[GetDiagnosisReportData]
	--@FromDate AS DATE=NULL,
	--@Todate AS DATE=NULL
	@PayorID VARCHAR(4000)='0'
AS
BEGIN
 SELECT ReferralID,PayorName,'' AS Admission_No,PatientName,
		'' AS Oder_Type,SOCDate AS OrderDate,
		'' AS Medications,Diagnosis_Code
FROM
(
select r.ReferralID AS ReferralID,
	P.PayorName,
	PatientName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName),
	dc.DXCodeWithoutDot AS Diagnosis_Code,
	dc.Description AS Description,
	CONVERT(VARCHAR(10), sm.StartDate, 111) as SOCDate
from Referrals r
left join ReferralDXCodeMappings rdm on  rdm.ReferralID=r.ReferralID
left join DXCodes dc on dc.DXCodeID=rdm.DXCodeID
left JOIN ScheduleMasters SM ON SM.ReferralID=r.ReferralID
left JOIN ReferralPayorMappings RPM ON RPM.ReferralID=R.ReferralID
left JOIN Payors P ON p.PayorID=RPM.ReferralID
Where r.IsDeleted=0
--AND ((@FromDate is null or @ToDate is null) or  sm.startDate BETWEEN @FromDate AND @Todate)
AND (@PayorID = '0' OR  TRY_CAst(P.PayorID AS varchar(100)) in (select Item from dbo.SplitString(@PayorID, ',')))
) T
END