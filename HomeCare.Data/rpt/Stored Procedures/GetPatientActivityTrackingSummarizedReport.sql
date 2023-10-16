
-- =============================================
-- Author:		Akhilesh Kamal
-- Create date: 22/4/2020
-- Description:	Employee Visits Hours
-- Modify By : Kalpesh Patel
-- Modify Date : 05/07/2020
-- EXEc [rpt].[GetPatientActivityTrackingSummarizedReport] @PatientID='0',@PayorID='0'
-- ============================================= 
create PROC [rpt].[GetPatientActivityTrackingSummarizedReport]  
	--@FromDate DATE = NULL,
	--@ToDate DATE = NULL,
	@PatientID AS VARCHAR(MAX)='',
	@PayorID AS VARCHAR(MAX)=''
AS  
BEGIN  
 SELECT 
	ReferralID,
	PatientName,
	ServiceID,
	Service,
	0 AS Encounters,
	startDate,
	EndDate,
	0 AS PayrollDuration,
	PayrollType,
	BillingType,
	0 AS BillingDuration
FROM  
(  
    SELECT     
		r.ReferralID AS ReferralID,
		ev.EmployeeVisitID,
		PatientName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName),   
		d.Title As Service,
		sm.CareTypeId as ServiceID,
		CONVERT(VARCHAR(10), sm.startDate, 111) as startDate,
		CONVERT(VARCHAR(10), sm.EndDate, 111) as EndDate,
		'Visit(s)' as PayrollType,
		rba.AllowedTimeType as BillingType 
	from EmployeeVisits ev    
	inner join ScheduleMasters sm on sm.ScheduleID=ev.ScheduleID    
	inner join Referrals r on r.ReferralID=sm.ReferralID    
	inner join Payors p on p.PayorID=sm.PayorID         
	inner JOIN DDMaster d on d.DDMasterID=sm.CareTypeId 
	inner join ReferralBillingAuthorizations rba on rba.ReferralID=r.ReferralID    
	WHERE r.isDeleted=0
	AND  (@PatientID = '0' OR  TRY_CAst(R.referralID AS varchar(100)) in (select Item from dbo.SplitString(@PatientID, ',')))
	AND  (@PayorID = '0' OR  TRY_CAst(p.PayorID AS varchar(100)) in (select Item from dbo.SplitString(@PayorID, ',')))
) T  

END