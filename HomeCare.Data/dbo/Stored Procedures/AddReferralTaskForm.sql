CREATE PROCEDURE [dbo].[AddReferralTaskForm]  
    @EmployeeVisitID BIGINT,
    @ReferralTaskMappingID BIGINT,    
    @TaskFormMappingID BIGINT,    
    @ReferralDocumentID BIGINT    
AS                  
BEGIN    
             
    INSERT INTO  
        [dbo].[ReferralTaskFormMappings] ([ReferralTaskMappingID], [TaskFormMappingID], [ReferralDocumentID], [EmployeeVisitID])  
    VALUES  
        (@ReferralTaskMappingID, @TaskFormMappingID, @ReferralDocumentID, @EmployeeVisitID)  
END