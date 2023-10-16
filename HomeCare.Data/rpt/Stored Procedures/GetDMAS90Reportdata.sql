/*          
 =============================================          
 Author:  Kalpesh Patel          
 Create date: 02/07/2020          
 Description: DMAS90          
 EXEc [rpt].[GetDMAS90Reportdata] @ReferralID=5,@CareType='0',          
    @EmployeeID= 0,@date= '2020-05-25'          
 =============================================          
 */          
CREATE PROCEDURE [rpt].[GetDMAS90Reportdata]          
 -- Add the parameters for the stored procedure here          
 @ReferralID int = '30043',          
 @CareType int = '41',           
 @EmployeeID INT = 0,          
 @date datetime = '05/25/2020 10:00:00 AM'            
          
AS    
BEGIN      
SET DATEFIRST 7;           
 -- SET NOCOUNT ON added to prevent extra result sets from          
 -- interfering with SELECT statements.          
 SET NOCOUNT ON;          
          
           
 --IF OBJECT_ID('tempdb..#VisitDetail') IS NOT NULL DROP TABLE #VisitDetail          
 --IF OBJECT_ID('tempdb..#VisitDetailFinal') IS NOT NULL DROP TABLE #VisitDetailFinal          
 Declare @VisitDetail table(          
  ReportID bigint,          
  ScheduleID bigint,          
  CareType nvarchar(max),          
  CareTypeId nvarchar(max),          
  ReferralID nvarchar(max),          
  PatientName nvarchar(max),          
  EmployeeName nvarchar(max),          
  EmployeeID bigint,          
  Phonenumber nvarchar(max),          
  ScheduleDate date)          
            
 insert into @visitdetail SELECT              
  --Dense_RANK() over (Partition by TRY_CAST(sm.StartDate AS DATE)  order by sm.ScheduleID) as ReportID,          
  Dense_RANK() OVER (ORDER BY CT.CareType) as ReportID,        
  Sm.ScheduleID,          
  dm.Title + ISNULL(' - ' + SC.ServiceCode + ISNULL(':' + M.ModifierCode, ''), '') AS CareType,sm.CareTypeId,        
  R.ReferralID,R.LastName + ',' + R.FirstName AS PatientName,          
  emp.LastName + ',' + emp.FirstName AS EmployeeName,emp.EmployeeID,c.Phone1 AS Phonenumber,          
  TRY_CAST(sm.StartDate AS DATE) as ScheduleDate          
 FROM         
 ScheduleMasters AS sm         
  Inner JOIN dbo.Referrals r ON sm.ReferralID = r.ReferralID        
  --inner JOIN VisitTasks AS vt ON vt.VisitTaskType='Task'        
  --inner JOIN ReferralTaskMappings AS rtm ON vt.VisitTaskID = rtm.VisitTaskID --AND rtm.ReferralID=@ReferralID             
  LEFT JOIN ReferralBillingAuthorizations RBA        
 ON RbA.ReferralBillingAuthorizationID = SM.ReferralBillingAuthorizationID        
  LEFT JOIN ServiceCodes SC        
 ON SC.ServiceCodeID = RBA.ServiceCodeID        
  LEFT JOIN Modifiers M        
 ON M.ModifierID = SC.ModifierID                 
            
  LEFT OUTER JOIN  employees emp ON emp.EmployeeID = sm.EmployeeID            
  inner join DDMaster dm on dm.DDMasterID=sm.CareTypeId          
  CROSS APPLY ( SELECT dm.Title + ISNULL(' - ' + SC.ServiceCode + ISNULL(':' + M.ModifierCode, ''), '') AS CareType) CT        
  Inner join ContactMappings cm on cm.ReferralID=sm.ReferralID AND cm.ContactTypeID = 1        
  inner join Contacts c on c.ContactID=cm.ContactID            
 WHERE                    
 SM.IsDeleted = 0 AND (sm.ReferralID = @ReferralID)           
 and (@EmployeeID = 0 or sm.EmployeeID=@EmployeeID)           
 AND (@CareType = 0 or sm.CareTypeId=@CareType)           
 and cast(sm.StartDate as date) between @date and DATEADD(dd, 6,@date)          
           
           
 ;WITH CTE AS (          
 SELECT V.*,          
  CONVERT(NVARCHAR(MAX), CONVERT(VARBINARY(MAX), CONVERT(IMAGE, CAST(EV.PatientSignature AS VARBINARY(MAX))),2)) AS PatientSignature,          
  --TRY_CAST(EV.PatientSignature AS varbinary(MAX)) AS PatientSignature,          
  EV.ClockOutTime as PatientSignatureDate,          
  EV.CreatedDate As EmployeeSignatureDate,          
  CASE WHEN PatientSignature IS NULL AND IVRClockIn=1 THEN EV.ClockInTime END AS TelephonyClockIn,          
  CASE WHEN PatientSignature IS NULL AND IVRClockOut=1 THEN EV.ClockInTime END AS TelephonyClockOut,          
  CASE WHEN PatientSignature IS NULL AND IsPCACompleted=1 THEN EV.PlaceOfService END AS TelephonyPlaceOfService,          
  CASE WHEN (IVRClockIn=1 OR IVRClockOut=1) THEN 1 ELSE 0 END AS IVRClockCompleted,          
            
  --CONVERT(NVARCHAR(MAX), CONVERT(VARBINARY(MAX), CONVERT(IMAGE, CAST(ES.SignaturePath AS VARBINARY(MAX))),2)) AS EmployeeSignaturePath,          
  dbo.GetOrgDomain()+ES.SignaturePath AS EmployeeSignaturePath,          
  row_number() OVER (Partition By V.ReportID Order by sm.EndDate DESC) AS EV_RowNum,          
  STUFF(( SELECT  ', ' + CAST(CDtmp.ScheduleID AS VARCHAR(100))          
     FROM @visitDetail CDtmp          
       WHERe CDtmp.ReportID = V.ReportID          
      FOR          
     XML PATH('')          
      ), 1, 2, '') AS ScheduleIDs          
 FROM @VisitDetail V          
 inner JOIN ScheduleMasters AS sm ON  sm.ScheduleID = V.ScheduleID          
 inner JOIN EmployeeVisits EV ON  sm.ScheduleID = EV.ScheduleID          
 LEFT JOIN EmployeeSignatures es ON es.EmployeeSignatureID=EV.EmployeeSignatureID        
 --WHERE EV.PatientSignature IS NOT NULL        
 --Group by V.ReportID,V.ScheduleID,V.CareType,V.PatientName,v.EmployeeName,V.Phonenumber,V.ScheduleDate          
 )          
 SELECT           
  ReportID,CareType,ReferralID,PatientName,EmployeeName,Phonenumber,ScheduleDate,          
  PS.PatientSignature,PatientSignatureDate,EmployeeSignaturePath,EmployeeSignatureDate,          
  TelephonyClockIn,TelephonyClockOut,TelephonyPlaceOfService,IVRClockCompleted,          
  ScheduleIDs,CareTypeId,EmployeeID          
 FROM CTE 
 OUTER APPLY (SELECT TOP 1 PatientSignature FROM CTE WHERE PatientSignature IS NOT NULL ORDER BY EV_RowNum) PS
 WHERE EV_RowNum=1;          
           
          
END