CREATE PROCEDURE [dbo].[GetPatientTaskMappings]        
@ReferralID BIGINT,        
@TaskTypeTask VARCHAR(100),        
@TaskTypeConclusion VARCHAR(100),
@DDType_TaskFrequencyCode INT =13
        
AS        
BEGIN        
         
  SELECT RTM.ReferralTaskMappingID, V.VisitTaskID, V.VisitTaskDetail, RTM.IsRequired, CreatedBy=E.FirstName+' '+E.LastName, RTM.CreatedDate,    
  VG1.VisitTaskCategoryName,VG.VisitTaskCategoryName as ParentCategoryName,RTM.Frequency,RTM.Comment  
  FROM ReferralTaskMappings  RTM        
  INNER JOIN VisitTasks V ON V.VisitTaskID=RTM.VisitTaskID    
  LEFT JOIN VisitTaskCategories VG ON V.VisitTaskCategoryID=VG.VisitTaskCategoryID      
   LEFT JOIN VisitTaskCategories VG1 ON V.VisitTaskSubCategoryID=VG1.VisitTaskCategoryID      
  INNER JOIN Employees E ON E.EmployeeID=RTM.CreatedBy        
  WHERE ReferralID=@ReferralID AND V.VisitTaskType=@TaskTypeTask AND RTM.IsDeleted=0        
  ORDER BY RTM.CreatedBy DESC        
        
        
  SELECT RTM.ReferralTaskMappingID, V.VisitTaskID, V.VisitTaskDetail, RTM.IsRequired, CreatedBy=E.FirstName+' '+E.LastName, RTM.CreatedDate,    
  VG1.VisitTaskCategoryName,VG.VisitTaskCategoryName as ParentCategoryName,RTM.Frequency,RTM.Comment  
  FROM ReferralTaskMappings  RTM        
  INNER JOIN VisitTasks V ON V.VisitTaskID=RTM.VisitTaskID    
  LEFT JOIN VisitTaskCategories VG ON V.VisitTaskCategoryID=VG.VisitTaskCategoryID      
   LEFT JOIN VisitTaskCategories VG1 ON V.VisitTaskSubCategoryID=VG1.VisitTaskCategoryID      
  INNER JOIN Employees E ON E.EmployeeID=RTM.CreatedBy        
  WHERE ReferralID=@ReferralID AND V.VisitTaskType=@TaskTypeConclusion AND RTM.IsDeleted=0        
  ORDER BY RTM.CreatedBy DESC        

 SELECT Value=DDMasterID,Name=Title FROM DDMaster where IsDeleted=0 and ItemType=@DDType_TaskFrequencyCode
        
END
