CREATE PROCEDURE [dbo].[GetPatientTaskMappings]            
@ReferralID BIGINT,            
@TaskTypeTask VARCHAR(100),            
@TaskTypeConclusion VARCHAR(100),    
@DDType_TaskFrequencyCode INT =13,   
@CareTypeID NVARCHAR(100) = NULL,
@VisitTaskDetail VARCHAR(100) = NULL
  
            
AS            
BEGIN            
             
  SELECT RTM.ReferralTaskMappingID, V.VisitTaskID,V.CareType As CareTypeID,DM.Title As CareType, V.VisitTaskDetail, RTM.IsRequired, CreatedBy=E.FirstName+' '+E.LastName, RTM.CreatedDate,        
  VG1.VisitTaskCategoryName,VG.VisitTaskCategoryName as ParentCategoryName,RTM.Frequency,RTM.Comment ,VG.VisitTaskCategoryID as ParentCategoryId     
  FROM ReferralTaskMappings  RTM            
  INNER JOIN VisitTasks V ON V.VisitTaskID=RTM.VisitTaskID        
  LEFT JOIN VisitTaskCategories VG ON V.VisitTaskCategoryID=VG.VisitTaskCategoryID          
   LEFT JOIN VisitTaskCategories VG1 ON V.VisitTaskSubCategoryID=VG1.VisitTaskCategoryID          
  LEFT JOIN Employees E ON E.EmployeeID=RTM.CreatedBy    
  LEFT JOIN DDMaster DM on DDMasterID = V.CareType          
  WHERE ReferralID=@ReferralID AND V.VisitTaskType=@TaskTypeTask AND RTM.IsDeleted=0 
  AND ((@VisitTaskDetail IS NULL OR LEN(@VisitTaskDetail)=0) OR (VisitTaskDetail LIKE '%' + @VisitTaskDetail + '%'))  
     AND ((@CareTypeID IS NULL OR @CareTypeID=0) OR (V.CareType =@CareTypeID ))  

             
  ORDER BY RTM.CreatedBy DESC            
            
            
  SELECT RTM.ReferralTaskMappingID, V.VisitTaskID,V.CareType As CareTypeID,DM.Title As CareType, V.VisitTaskDetail, RTM.IsRequired, CreatedBy=E.FirstName+' '+E.LastName, RTM.CreatedDate,        
  VG1.VisitTaskCategoryName,VG.VisitTaskCategoryName as ParentCategoryName,RTM.Frequency,RTM.Comment,VG.VisitTaskCategoryID as ParentCategoryId       
  FROM ReferralTaskMappings  RTM            
  INNER JOIN VisitTasks V ON V.VisitTaskID=RTM.VisitTaskID        
  LEFT JOIN VisitTaskCategories VG ON V.VisitTaskCategoryID=VG.VisitTaskCategoryID          
   LEFT JOIN VisitTaskCategories VG1 ON V.VisitTaskSubCategoryID=VG1.VisitTaskCategoryID          
  LEFT JOIN Employees E ON E.EmployeeID=RTM.CreatedBy    
  LEFT JOIN DDMaster DM on DDMasterID = V.CareType          
  WHERE ReferralID=@ReferralID AND V.VisitTaskType=@TaskTypeConclusion AND RTM.IsDeleted=0  
   AND ((@VisitTaskDetail IS NULL OR LEN(@VisitTaskDetail)=0) OR (VisitTaskDetail LIKE '%' + @VisitTaskDetail + '%'))  
     AND ((@CareTypeID IS NULL OR @CareTypeID=0) OR (V.CareType =@CareTypeID ))   
  ORDER BY RTM.CreatedBy DESC            
    
 SELECT Value=DDMasterID,Name=Title FROM DDMaster where IsDeleted=0 and ItemType=@DDType_TaskFrequencyCode    
   
          
END