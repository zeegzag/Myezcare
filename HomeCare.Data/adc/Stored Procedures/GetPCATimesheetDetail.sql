--   exec [adc].[GetPCATimesheetDetail]   50245,'task','conclusion'



CREATE PROCEDURE [adc].[GetPCATimesheetDetail]            
 @EmployeeVisitID BIGINT,            
 @TaskType VARCHAR(20),            
 @ConclusionType VARCHAR(20)            
AS                                          
BEGIN          
           
  DECLARE @OtherActivity VARCHAR(MAX);          
  DECLARE @OtherActivityTime BIGINT;          
            
  SELECT @OtherActivity=Description,@OtherActivityTime=ServiceTime FROM EmployeeVisits EV           
  INNER JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID=EV.EmployeeVisitID AND ReferralTaskMappingID IS NULL AND ServiceTime > 0          
  WHERE EV.EmployeeVisitID=@EmployeeVisitID          
          
  SELECT BeneficiaryName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName),f.FacilityName,
  r.AHCCCSID AS BeneficiaryID,sm.StartDate AS Date,DATENAME(dw,sm.StartDate) as DayOfWeek,              
  PlaceOfService= CASE WHEN PlaceOfService IS NULL THEN 'NA' ELSE ev.PlaceOfService END,            
  HHA_PCA_Name=f.FacilityName ,-- CASE WHEN HHA_PCA_Name IS NULL THEN 'NA' ELSE ev.HHA_PCA_Name END,            
  HHA_PCA_NP= CASE WHEN HHA_PCA_NP IS NULL THEN 'NA' ELSE ev.HHA_PCA_NP END,            
  ev.PatientSignature,ev.PatientSignature_ClockOut,Name,Relation=RL.Title,IsSelf,            
  CONVERT(varchar(15),  CAST(ev.ClockInTime AS TIME), 100) as ClockInTime,CONVERT(varchar(15),  CAST(ev.ClockOutTime AS TIME), 100) as ClockOutTime,ev.EmployeeVisitID,              
  --DATEDIFF(MINUTE, ev.ClockInTime,ev.ClockOutTime) as TotalTime,    
 --UpdatedBy:Akhilesh kamal  
--UpdateDate:20/01/2020  
--Description:For differance ClockIn ClockOut time as TotalTime in minutes  
  DATEDIFF(MINUTE,(CAST(ev.ClockInTime AS TIME)),(CAST(ev.ClockOutTime AS TIME)))  as TotalTime,         
  OtherActivity=@OtherActivity,OtherActivityTime=@OtherActivityTime          
                
  FROM EmployeeVisits ev                  
  INNER JOIN ScheduleMasters sm ON sm.ScheduleID=ev.ScheduleID                  
  INNER JOIN Referrals r ON sm.ReferralID=r.ReferralID and r.DefaultFacilityID=sm.FacilityID                 
  --INNER JOIN Employees e ON e.EmployeeID=sm.EmployeeID            
 -- LEFT JOIN EmployeeSignatures es ON es.EmployeeSignatureID=e.EmployeeSignatureID   
  INNER JOIN Facilities F ON f.FacilityID=sm.FacilityID
  LEFT JOIN DDMaster RL ON RL.DDMasterID = Relation

  

  --LEFT JOIN EmployeeSignatures es ON es.EmployeeSignatureID=ev.EmployeeSignatureID      
  WHERE ev.EmployeeVisitID=@EmployeeVisitID    
              
              
 --   SELECT v.VisitTaskDetail,rm.ReferralTaskMappingID,v.MinimumTimeRequired,rm.IsRequired,                  
 --vg.VisitTaskCategoryName as CategoryName,vg.VisitTaskCategoryID as CategoryId,                   
 --vg1.VisitTaskCategoryName as SubCategoryName, vg1.VisitTaskCategoryID as SubCategoryId,evn.Description as Answer                
 --FROM VisitTasks v                  
 --LEFT JOIN VisitTaskCategories vg ON vg.VisitTaskCategoryID=v.VisitTaskCategoryID                  
 --LEFT JOIN VisitTaskCategories vg1 ON vg1.VisitTaskCategoryID=v.VisitTaskSubCategoryID                  
 --INNER JOIN ReferralTaskMappings rm on v.VisitTaskID=rm.VisitTaskID                          
 --INNER JOIN ScheduleMasters sm ON sm.ReferralID=rm.ReferralID                                
 --INNER JOIN EmployeeVisits ev ON ev.ScheduleID=sm.ScheduleID                
 --LEFT JOIN EmployeeVisitNotes evn ON evn.ReferralTaskMappingID=rm.ReferralTaskMappingID                                 
 --WHERE EV.EmployeeVisitID=@EmployeeVisitID AND v.VisitTaskType=@TaskType            
            
            
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