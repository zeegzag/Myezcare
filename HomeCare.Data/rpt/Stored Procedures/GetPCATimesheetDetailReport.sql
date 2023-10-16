
--   exec [rpt].[GetPCATimesheetDetailReport] @EmployeeVisitID = '60253', @TaskType = 'Task', @ConclusionType = 'Conclusion', @dbname='DEVHomecare'
             
CREATE PROCEDURE [rpt].[GetPCATimesheetDetailReport]            
 @EmployeeVisitID BIGINT=0,            
 @TaskType VARCHAR(20)=NULL,            
 @ConclusionType VARCHAR(20)=NULL,
 @dbname varchar(50) = NULL  
AS                                          
BEGIN          
     
  DECLARE @DateFormat VARCHAR(MAX);  
  SELECT @DateFormat=[Admin_Myezcare_Live].dbo.fn_getDateFormat(@dbname)
  DECLARE @OtherActivity VARCHAR(MAX);          
  DECLARE @OtherActivityTime BIGINT;          
            
  SELECT @OtherActivity=Description,@OtherActivityTime=ServiceTime FROM EmployeeVisits EV           
  INNER JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID AND ReferralTaskMappingID IS NULL AND ServiceTime > 0          
  WHERE EV.EmployeeVisitID=@EmployeeVisitID          
          
  SELECT BeneficiaryName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName),EmployeeName=dbo.GetGeneralNameFormat(e.FirstName,e.LastName),e.EmployeeUniqueID,            
  r.AHCCCSID AS BeneficiaryID,FORMAT(sm.StartDate, @DateFormat) as Date,DATENAME(dw,sm.StartDate) as DayOfWeek,              
  PlaceOfService= CASE WHEN PlaceOfService IS NULL THEN 'NA' ELSE ev.PlaceOfService END,            
  HHA_PCA_Name= dbo.GetGeneralNameFormat(e.FirstName,e.LastName),            
  HHA_PCA_NP= CASE WHEN HHA_PCA_NP IS NULL THEN 'NA' ELSE ev.HHA_PCA_NP END,            
  ev.PatientSignature, EmployeeSignature=es.SignaturePath,            
  CONVERT(varchar(15),  CAST(ev.ClockInTime AS TIME), 100) as ClockInTime,CONVERT(varchar(15),  CAST(ev.ClockOutTime AS TIME), 100) as ClockOutTime,ev.EmployeeVisitID,               
  DATEDIFF(MINUTE,(CAST(ev.ClockInTime AS TIME)),(CAST(ev.ClockOutTime AS TIME)))  as TotalTime,         
  OtherActivity=@OtherActivity,OtherActivityTime=@OtherActivityTime          
                
  FROM EmployeeVisits ev                  
  INNER JOIN ScheduleMasters sm ON sm.ScheduleID=ev.ScheduleID                  
  INNER JOIN Referrals r ON sm.ReferralID=r.ReferralID                  
  INNER JOIN Employees e ON e.EmployeeID=sm.EmployeeID            
  LEFT JOIN EmployeeSignatures es ON es.EmployeeSignatureID=e.EmployeeSignatureID         
  WHERE ev.EmployeeVisitID in (@EmployeeVisitID)


 SELECT V.VisitTaskDetail,RM.ReferralTaskMappingID,V.MinimumTimeRequired,RM.IsRequired,                  
 VG.VisitTaskCategoryName as CategoryName,VG.VisitTaskCategoryID as CategoryId,                   
 VG1.VisitTaskCategoryName as SubCategoryName, VG1.VisitTaskCategoryID as SubCategoryId,EVN.ServiceTime            
 FROM EmployeeVisits EV            
 INNER JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID          
 INNER JOIN ReferralTaskMappings RM on RM.ReferralTaskMappingID=EVN.ReferralTaskMappingID            
 INNER JOIN VisitTasks V on V.VisitTaskID=RM.VisitTaskID            
 LEFT JOIN VisitTaskCategories VG ON VG.VisitTaskCategoryID=V.VisitTaskCategoryID                  
 LEFT JOIN VisitTaskCategories VG1 ON VG1.VisitTaskCategoryID=V.VisitTaskSubCategoryID                  
 WHERE EV.EmployeeVisitID=@EmployeeVisitID AND v.VisitTaskType=@TaskType            
             
            
 SELECT V.VisitTaskDetail,RM.ReferralTaskMappingID,V.MinimumTimeRequired,RM.IsRequired,                  
 VG.VisitTaskCategoryName as CategoryName,VG.VisitTaskCategoryID as CategoryId,              VG1.VisitTaskCategoryName as SubCategoryName, VG1.VisitTaskCategoryID as SubCategoryId,EVN.ServiceTime,EVN.Description as Answer                
 FROM EmployeeVisits EV            
 INNER JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID            
 INNER JOIN ReferralTaskMappings RM on RM.ReferralTaskMappingID=EVN.ReferralTaskMappingID            
 INNER JOIN VisitTasks V on V.VisitTaskID=RM.VisitTaskID            
 LEFT JOIN VisitTaskCategories VG ON VG.VisitTaskCategoryID=V.VisitTaskCategoryID                  
 LEFT JOIN VisitTaskCategories VG1 ON VG1.VisitTaskCategoryID=V.VisitTaskSubCategoryID                  
 WHERE EV.EmployeeVisitID=@EmployeeVisitID AND v.VisitTaskType=@ConclusionType AND EVN.Description IS NOT NULL            
             
             
            
END   