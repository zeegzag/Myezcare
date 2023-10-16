CREATE PROCEDURE [dbo].[DeleteReferralTaskForm]
    @ReferralTaskFormMappingID BIGINT  
AS                
BEGIN  
           
    DELETE 
    FROM
        [dbo].[ReferralTaskFormMappings] 
    WHERE
        ReferralTaskFormMappingID = @ReferralTaskFormMappingID
END