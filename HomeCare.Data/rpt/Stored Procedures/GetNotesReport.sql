
 

-- =============================================
-- Author:		Akhilesh Kamal
-- Create date: 22/4/2020
-- Description:	Employee Visits Hours
-- Modify By : Kalpesh Patel
-- Modify Date : 05/07/2020
-- EXEc [rpt].[GetNotesReport] @FromDate=NULL,@ToDate=NULL,@PatientID='12'
-- =============================================
create PROC [rpt].[GetNotesReport]
	@FromDate DATE = NULL,
	@ToDate DATE = NULL,
	@PatientID AS VARCHAR(MAX)=''
AS  
BEGIN  
 SELECT 
	ReferralID,PatientName,SOCDate,Status,StatusDate,EmergencyNotes
FROM  
(  
	 select   
		r.ReferralID,
		PatientName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName),
		CONVERT(VARCHAR(10), sm.startDate, 111) as SOCDate,
		rs.Status as Status,
		CONVERT(VARCHAR(10), 
		r.CreatedDate, 111) as StatusDate,'Notes' as EmergencyNotes
	from Referrals r
	LEFT join  ScheduleMasters sm on sm.ReferralID=r.ReferralID
	LEFT JOIN ReferralStatuses RS on RS.ReferralStatusID=R.ReferralStatusID
	WHERE R.isdeleted=0
	AND  ((@FromDate is null or @ToDate is null) or  sm.startDate BETWEEN @FromDate AND @Todate)
	AND  (@PatientID = '0' OR  TRY_CAst(R.referralID AS varchar(100)) in (select Item from dbo.SplitString(@PatientID, ',')))
) T  
END