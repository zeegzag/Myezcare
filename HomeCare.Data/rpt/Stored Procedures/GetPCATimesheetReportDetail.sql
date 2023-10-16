
  
CREATE PROCEDURE [rpt].[GetPCATimesheetReportDetail]            
 @EmployeeVisitID BIGINT=0,            
 @TaskType VARCHAR(20)=null,            
 @ConclusionType VARCHAR(20)=null            
AS                                          
BEGIN          
           
  DECLARE @OtherActivity VARCHAR(MAX);          
  DECLARE @OtherActivityTime BIGINT;          
            
  SELECT @OtherActivity=Description,@OtherActivityTime=ServiceTime 
  FROM EmployeeVisits EV           
  INNER JOIN EmployeeVisitNotes EVN 
	ON EVN.EmployeeVisitID=EV.EmployeeVisitID 
	AND ReferralTaskMappingID IS NULL AND ServiceTime > 0          
  WHERE EV.EmployeeVisitID=@EmployeeVisitID          
          
  SELECT R.ReferralID,
  BeneficiaryName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName),
  e.EmployeeID,
  EmployeeName=dbo.GetGeneralNameFormat(e.FirstName,e.LastName),e.EmployeeUniqueID,            
  r.AHCCCSID AS BeneficiaryID,sm.StartDate AS Date,DATENAME(dw,sm.StartDate) as DayOfWeek,              
  PlaceOfService= CASE WHEN PlaceOfService IS NULL THEN 'NA' ELSE ev.PlaceOfService END,            
  HHA_PCA_Name= dbo.GetGeneralNameFormat(e.FirstName,e.LastName),-- CASE WHEN HHA_PCA_Name IS NULL THEN 'NA' ELSE ev.HHA_PCA_Name END,            
  HHA_PCA_NP= CASE WHEN HHA_PCA_NP IS NULL THEN 'NA' ELSE ev.HHA_PCA_NP END,            
  ev.PatientSignature, EmployeeSignature=es.SignaturePath,            
  CONVERT(varchar(15),  CAST(ev.ClockInTime AS TIME), 100) as ClockInTime,CONVERT(varchar(15),  CAST(ev.ClockOutTime AS TIME), 100) as ClockOutTime,ev.EmployeeVisitID,              
  DATEDIFF(MINUTE,(CAST(ev.ClockInTime AS TIME)),(CAST(ev.ClockOutTime AS TIME)))  as TotalTime,         
  OtherActivity=@OtherActivity,OtherActivityTime=@OtherActivityTime
  INTO #Employee
  FROM EmployeeVisits ev                  
  INNER JOIN ScheduleMasters sm ON sm.ScheduleID=ev.ScheduleID                  
  INNER JOIN Referrals r ON sm.ReferralID=r.ReferralID                  
  INNER JOIN Employees e ON e.EmployeeID=sm.EmployeeID            
  LEFT JOIN EmployeeSignatures es ON es.EmployeeSignatureID=e.EmployeeSignatureID      
  WHERE ev.EmployeeVisitID=@EmployeeVisitID    
              
            
 SELECT EV.EmployeeVisitID,v.VisitTaskType,V.VisitTaskDetail,RM.ReferralTaskMappingID,V.MinimumTimeRequired,RM.IsRequired,                  
 VG.VisitTaskCategoryName as CategoryName,VG.VisitTaskCategoryID as CategoryId,                   
 VG1.VisitTaskCategoryName as SubCategoryName, VG1.VisitTaskCategoryID as SubCategoryId,EVN.ServiceTime, 
 EVN.Description as Answer
 INTO #TaskandConclusionType
 FROM EmployeeVisits EV            
 INNER JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID          
 INNER JOIN ReferralTaskMappings RM on RM.ReferralTaskMappingID=EVN.ReferralTaskMappingID            
 INNER JOIN VisitTasks V on V.VisitTaskID=RM.VisitTaskID            
 LEFT JOIN VisitTaskCategories VG ON VG.VisitTaskCategoryID=V.VisitTaskCategoryID                  
 LEFT JOIN VisitTaskCategories VG1 ON VG1.VisitTaskCategoryID=V.VisitTaskSubCategoryID                  
 WHERE EV.EmployeeVisitID=@EmployeeVisitID AND v.VisitTaskType=@TaskType            
             
 UNION ALL
	 
 SELECT EV.EmployeeVisitID,v.VisitTaskType,V.VisitTaskDetail,RM.ReferralTaskMappingID,V.MinimumTimeRequired,RM.IsRequired,                  
 VG.VisitTaskCategoryName as CategoryName,VG.VisitTaskCategoryID as CategoryId,              
 VG1.VisitTaskCategoryName as SubCategoryName, VG1.VisitTaskCategoryID as SubCategoryId,EVN.ServiceTime,EVN.Description as Answer                
 FROM EmployeeVisits EV            
 INNER JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID            
 INNER JOIN ReferralTaskMappings RM on RM.ReferralTaskMappingID=EVN.ReferralTaskMappingID            
 INNER JOIN VisitTasks V on V.VisitTaskID=RM.VisitTaskID            
 LEFT JOIN VisitTaskCategories VG ON VG.VisitTaskCategoryID=V.VisitTaskCategoryID                  
 LEFT JOIN VisitTaskCategories VG1 ON VG1.VisitTaskCategoryID=V.VisitTaskSubCategoryID                  
 WHERE EV.EmployeeVisitID=@EmployeeVisitID AND v.VisitTaskType=@ConclusionType AND EVN.Description IS NOT NULL            
             
        
 SELECT 
	E.EmployeeVisitID
	,E.EmployeeID
	,E.ReferralID
	,E.BeneficiaryName
	,E.EmployeeName
	,E.EmployeeUniqueID
	,E.BeneficiaryID
	,CONVERT(VARCHAR(15),FORMAT(E.Date,'MM/dd/yyyy')) AS Date
	,E.DayOfWeek              
	,E.PlaceOfService
	,E.HHA_PCA_Name
	,E.HHA_PCA_NP
	,E.PatientSignature
	,E.EmployeeSignature
	,E.ClockInTime
	,E.ClockOutTime
	,E.TotalTime
	,E.OtherActivity
	,E.OtherActivityTime
	,TPE.VisitTaskType
	,TPE.VisitTaskDetail
	,TPE.ReferralTaskMappingID
	,TPE.MinimumTimeRequired
	,TPE.IsRequired                  
	,TPE.CategoryName
	,TPE.CategoryId              
	,TPE.SubCategoryName
	,TPE.SubCategoryId
	,TPE.ServiceTime
	,TPE.Answer   
 FROM #Employee E
 JOIN #TaskandConclusionType TPE
	ON E.EmployeeVisitID=TPE.EmployeeVisitID
            
END