-- EXEc [rpt].[PatientScheduleReport] @FromDate=NULL,@ToDate=NULL,@PayerID='0',@CareType='0'
-- =============================================
CREATE PROC [rpt].[PatientScheduleReport]  
	@FromDate DATE = NULL,
	@ToDate DATE = NULL,
	@PayerID VARCHAR(4000)='0',
	@PatientID VARCHAR(4000)='0',
	@EmployeeID VARCHAR(4000)='0',
	@PatientStatusID VARCHAR(4000)='0',
	@CareType VARCHAR(4000)='0'
AS  
BEGIN  
 SELECT PayorName,ReferralID,PatientName,AdmissionStatus,AdmissionCode,Phone,Address,Date, CareGiver,Service,
 StartDate,EndDate
FROM  
(  
select 
	P.PayorName,
	r.ReferralID AS ReferralID,
	PatientName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName), 
	RS.Status AS AdmissionStatus,
	'' AS AdmissionCode,
	c.Phone1 as Phone,  
	c.ApartmentNo+ ',' + c.Address+','+c.City + ',' + c.State+','+c.ZipCode AS Address,
	CONVERT(VARCHAR(10), sm.startDate, 111) as Date,
	CareGiver=dbo.GetGeneralNameFormat(e.FirstName,e.LastName),
	dm.Title as Service,
	FORMAT(sm.StartDate,'MM/dd/yyyy') as StartDate,
	FORMAT(sm.EndDate,'MM/dd/yyyy') as EndDate
from Referrals r  
 LEFT JOIN ReferralStatuses RS on RS.ReferralStatusID=R.ReferralStatusID 
 left join ScheduleMasters sm on sm.ReferralID=R.ReferralID
 left join Employees e on e.EmployeeID=sm.EmployeeID
 left join ContactMappings cm on cm.ReferralID=r.ReferralID  
 left join Contacts c on c.ContactID=cm.ContactID  
 left join DDMaster dm on dm.DDMasterID=sm.CareTypeId
 join ReferralPayorMappings rp with(nolock) on rp.ReferralID=r.ReferralID 
	and rp.IsActive=1 and rp.IsDeleted=0 AND rp.Precedence=1  
 JOIN  dbo.Payors P with(nolock) on P.PayorID=rp.PayorID
where r.IsDeleted=0 
AND ((@FromDate is null or @ToDate is null) or  sm.StartDate BETWEEN @FromDate AND @Todate)
	AND 
	(@PayerID = '0' OR  TRY_CAst(P.PayorID AS varchar(100)) in (select Item from dbo.SplitString(@PayerID, ',')))
	AND 
	(@PatientID = '0' OR  TRY_CAst(R.ReferralID AS varchar(100)) in (select Item from dbo.SplitString(@PatientID, ',')))
	AND 
	(@EmployeeID = '0' OR  TRY_CAst(E.EmployeeID AS varchar(100)) in (select Item from dbo.SplitString(@EmployeeID, ',')))
	AND 
	(@PatientStatusID = '0' OR  TRY_CAst(R.ReferralStatusID AS varchar(100)) in (select Item from dbo.SplitString(@PatientStatusID, ',')))
	AND 
	(@CareType = '0' OR  TRY_CAst(sm.CareTypeId AS varchar(100)) in (select Item from dbo.SplitString(@CareType, ',')))
) T  
END