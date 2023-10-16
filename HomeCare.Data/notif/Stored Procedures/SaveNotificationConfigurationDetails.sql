  
-- =============================================  
-- Author:  Fenil Gandhi  
-- Create date: 03 Jul 2020  
-- Description: This SP is used to save role mapping and update templates.  
-- =============================================  
CREATE PROCEDURE [notif].[SaveNotificationConfigurationDetails]  
 @RoleID BIGINT,  
 @NotificationConfigurationIDs NVARCHAR(MAX),  
 @LoggedInUserID BIGINT,  
 @SystemID VARCHAR(100)  
AS  
BEGIN  
  
 DECLARE @CurrDateTime DATETIME = GETUTCDATE();  
  
 -- Update Roles  
 MERGE   
  [notif].[NotificationConfigurationRoleMapping] T  
 USING ( SELECT   
    val [NotificationConfigurationID],  
    @RoleID [RoleID],  
    @CurrDateTime [CurrDate],  
    @LoggedInUserID [ChangedBy],  
    @SystemID [SystemID]  
   FROM  
    [dbo].[GetCSVTable](@NotificationConfigurationIDs) ) S  
 ON   
  T.[NotificationConfigurationID] = S.[NotificationConfigurationID]  
  AND T.[RoleID] = S.[RoleID]  
 WHEN MATCHED THEN  
  UPDATE SET  
   [NotificationConfigurationID] = S.[NotificationConfigurationID],  
   [RoleID] = S.[RoleID],  
   [UpdatedDate] = S.[CurrDate],  
   [UpdatedBy] = S.[ChangedBy],  
   [SystemID] = S.[SystemID]  
 WHEN NOT MATCHED BY TARGET THEN  
  INSERT  
  (  
   [NotificationConfigurationID],  
   [RoleID],  
   [CreatedDate],  
   [CreatedBy],  
   [UpdatedDate],  
   [UpdatedBy],  
   [SystemID]  
  )  
  VALUES  
  (  
   S.[NotificationConfigurationID],  
   S.[RoleID],  
   S.[CurrDate],  
   S.[ChangedBy],  
   S.[CurrDate],  
   S.[ChangedBy],  
   S.[SystemID]  
  )  
 WHEN NOT MATCHED BY SOURCE   
   AND T.[RoleID] = @RoleID THEN  
  DELETE;  
  
END