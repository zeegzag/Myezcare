-- =============================================  
-- Author:  Ali Bukhari  
-- Create date: 15 Jan 2021  
-- Description: Updates terms and condition for organization  
-- =============================================  
CREATE PROCEDURE UpdateOrganizationSettings  
 -- Add the parameters for the stored procedure here  
 @OrganizationID int,  
 @TermsCondition nvarchar(MAX),  
 @UserId bigint  
AS  
BEGIN  
 UPDATE OrganizationSettings SET TermsCondition = @TermsCondition, UpdatedBy = @UserId, UpdatedDate = GETDATE() WHERE OrganizationID = @OrganizationID       
END  