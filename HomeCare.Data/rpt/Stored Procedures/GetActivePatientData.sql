    
-- =============================================    
-- Author:  Kalpesh Patel    
-- Create date: 22/4/2020    
-- Description: Active Patient Data    
-- EXEc [rpt].[GetActivePatientData] @FromDate=NULL,@ToDate=NULL,@PayorID='0'    
-- =============================================    
CREATE PROCEDURE [rpt].[GetActivePatientData]    
 -- Add the parameters for the stored procedure here    
 @Status VARCHAR(4000) =  '0',    
 --@ToDate DATE = NULL,    
 @PayorID VARCHAR(4000) = '0' ,  
 @zone varchar(4000) =null,  
 @FromDate date=null,  
 @ToDate date=null,
 @ActiveForLastNDays INT = NULL
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
 SET NOCOUNT ON;    
    
 --IF ISNULL(@FromDate,'') = ''    
 -- SET @FromDate ='01/01/1900'    
 --IF ISNULL(@ToDate,'') = ''    
 -- SET @ToDate = '12/31/2099'    
    
    -- Insert statements for procedure here    
 SELECT     
 R.ReferralID AS PatientID,     
 0 as AmissionNumber,    
 ISNULL(R.SocialSecurityNumber,'') AS SSN,    
 --ISNULL(dbo.fn_getDateFormat(DOB),'') AS DOB,  
  
 [dbo].[GetGeneralNameFormat](r.FirstName,r.LastName) as PatientName,  
 ISNULL(CONVERT(VARCHAR(15),FORMAT(DOB,'MM/dd/yyyy')),'') AS DOB,    
 --r.LastName+', '+r.FirstName as PatientName,    
 '' as PolicyNumber,    
 c.Address+',' + c.City + ',' + c.State + ',' + c.ZipCode as Address,    
 c.City AS Country,    
 PHY.LastName+', ' + PHY.FirstName as PhysicianName,    
 '' as CaseManagerName,    
 ISNULL(Phone1,Phone2) As PhoneNumber,    
 CASE WHEN ISNULL(PHY.Phone,'')='' THEN PHY.Mobile ELSE PHY.Phone END as PhyPhone,    
 '' as PhyFax,    
 '' as CPFROM,    
 '' as CPThrough,    
 '' AS SOC,    
 (    
 STUFF((SELECT ', ' + Dx.DXCodeName    
 FROM JO_ReferralDXCodeMappings DXR WITH(NOLOCK)    
 JOIN DXCodes DX with(nolock) ON DX.DXCodeID=DXR.DXCodeID    
 WHERE DXr.ReferralID=R.ReferralID AND DXR.IsDeleted=0    
 for xml path('')),1,1,'')    
 ) AS PrincipalDiag,    
 '' as Disciplines,    
 CASE WHEN P.IsBillingActive=1 THEN 'Active' ELSE '' END AS EligibilityStatus,    
 P.PayorName,    
 RS.Status AS AdmissionStatus,
 LA.LastActiveDate LastVisitDate
 FROM     
 referrals R with(nolock)    
 left join ContactMappings cmp with(nolock) on cmp.ReferralID= r.ReferralID and cmp.ContactTypeID = 1                      
 left join Contacts c with(nolock)  on c.ContactID=cmp.ContactID    
 LEFT join ReferralPayorMappings rp with(nolock) on rp.ReferralID=r.ReferralID and rp.IsActive=1 and rp.IsDeleted=0 AND rp.Precedence=1      
 LEFT JOIN  dbo.Payors P with(nolock) on P.PayorID=rp.PayorID    
 LEFT JOIN Physicians PHY WITH(NOLOCK) ON PHY.PhysicianID=R.PhysicianID    
 --LEFT JOIN CaseManagers CM WITH(NOLOCK) ON CM.CaseManagerID=R.CaseManagerID    
 --LEFT JOIN DDMaster DDM WITH(NOLOCK) ON DDM.DDMasterID IN(R.CareTypeIds)    
 LEFT JOIN ReferralStatuses RS WITH(NOLOCK) on RS.ReferralStatusID=R.ReferralStatusID    
 --left join employees e on e.EmployeeID=r.Assignee  
OUTER APPLY (
	SELECT TOP 1 A.LastActiveDate, D.LastActiveDays  FROM dbo.ScheduleMasters SM 
	LEFT JOIN dbo.EmployeeVisits EV ON EV.IsDeleted = 0 AND EV.ScheduleID = SM.ScheduleID
	OUTER APPLY ( SELECT CONVERT(DATE, EV.ClockInTime) LastActiveDate ) A
	OUTER APPLY ( SELECT DATEDIFF(DD, A.LastActiveDate, CONVERT(DATE, GETDATE())) LastActiveDays ) D
	WHERE SM.IsDeleted = 0 AND SM.ReferralID = R.ReferralID
	ORDER BY EV.EmployeeVisitID DESC
) LA
 WHERE     
 r.IsDeleted=0    
 --AND ((@FromDate is null or @ToDate is null) or  R.ReferralDate BETWEEN @FromDate AND @Todate)    
 AND (@Status = '0' OR  TRY_CAst(R.ReferralStatusID AS varchar(100)) in (select Item from dbo.SplitString(@Status, ',')))    
 AND (@PayorID = '0' OR  TRY_CAst(P.PayorID AS varchar(100)) in (select Item from dbo.SplitString(@PayorID, ',')))    
 And (@zone is null or  c.City in (select Item from dbo.SplitString(@zone, ',')))  
 --and r.UpdatedDate between @FromDate and @ToDate  
 AND (@ActiveForLastNDays IS NULL OR LA.LastActiveDays <= @ActiveForLastNDays)
    
END