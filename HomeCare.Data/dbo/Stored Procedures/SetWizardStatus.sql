CREATE PROCEDURE [dbo].[SetWizardStatus]
	@Menu nvarchar(50),
	@OrganizationID bigint,
	@IsCompleted bit,
	@loggedInUserId bigint,
	@SystemID varchar(100)
AS
BEGIN
	UPDATE [OnboardingWizardLog]
    SET 
      [IsCompleted] = CAST(@IsCompleted AS BIT)
      ,[UpdatedDate] = GETDATE()
      ,[UpdatedBy] = @loggedInUserId
      ,[SystemID] = @SystemID
	WHERE [OrganizationID] = @OrganizationID AND [Menu] = @Menu AND [IsDeleted] = 0

	IF(@@ROWCOUNT = 0)
		INSERT INTO [dbo].[OnboardingWizardLog]
           ([OrganizationID]
           ,[Menu]
           ,[IsCompleted]
           ,[IsDeleted]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[SystemID])
		VALUES
           (@OrganizationID
           ,@Menu
           ,CAST(@IsCompleted AS BIT)
           ,0
           ,@loggedInUserId
           ,GETDATE()
           ,@SystemID)
	SELECT @@ROWCOUNT
END