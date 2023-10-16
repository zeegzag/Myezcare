  
-- =============================================  
-- Author:  Fenil Gandhi  
-- Create date: 03 Jul 2020  
-- Description: This SP is used to get role mapping for notification configuration.  
-- =============================================  
CREATE PROCEDURE [notif].[GetNotificationConfigurationDetails]  
    @RoleID BIGINT  
AS  
BEGIN  
  
    SELECT   
        NC.*,  
        CASE WHEN NCRM.[NotificationConfigurationRoleMappingID] IS NULL THEN 0 ELSE 1 END [IsSelected]  
    FROM   
        [notif].[NotificationConfigurations] NC  
    LEFT JOIN [notif].[NotificationConfigurationRoleMapping] NCRM  
        ON NC.[NotificationConfigurationID] = NCRM.[NotificationConfigurationID]  
           AND NCRM.[RoleID] = @RoleID;  
  
END