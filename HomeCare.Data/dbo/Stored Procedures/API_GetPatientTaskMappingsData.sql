
CREATE PROCEDURE [dbo].[API_GetPatientTaskMappingsData]            
    @ReferralID INT            
AS                                               
BEGIN          
 SELECT           
    RTM.[ReferralID],      
    ISNULL(VT.[VisitTaskType], 'NA') AS [VisitTaskType],      
    ISNULL(VT.[VisitTaskDetail], 'NA') AS [VisitTaskDetail],      
    ISNULL(VTC.[VisitTaskCategoryName], 'NA') AS [VisitTaskCategoryName],      
    ISNULL(D.[Title], 'NA') AS [CarePlan]  
    FROM           
        [dbo].[ReferralTaskMappings] RTM         
    LEFT JOIN [dbo].[VisitTasks] VT          
        ON VT.[VisitTaskID] = RTM.[VisitTaskID]      
 LEFT JOIN [dbo].[VisitTaskCategories] VTC          
        ON VTC.[VisitTaskCategoryID] = VT.[VisitTaskCategoryID]         
 LEFT JOIN [dbo].[DDMaster] D          
        ON D.[DDMasterID] = VT.[CareType]         
    WHERE          
        RTM.[ReferralID] = @ReferralID      
  AND RTM.[IsDeleted] = 0      
  ORDER BY VT.[VisitTaskType] DESC    
END