-- Exec GetInternalMessagesGroupParticipantsByType 60053, 'Support', 32
CREATE PROCEDURE [dbo].[GetInternalMessagesGroupParticipantsByType]
	-- Add the parameters for the stored procedure here
	@ReferralID			BIGINT,
	@CommType			VARCHAR(20),
	@SupportGroupID		BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Assignee BIGINT = 0

	Select Top 1 @Assignee = Assignee
	FROM Referrals (nolock) 
	WHERE ReferralID = @ReferralID

	SELECT @Assignee AS Assignee

    -- Insert statements for procedure here
	Select Top 1 ParticipantID = @Assignee, ParticipantType = @CommType
	WHERE @CommType = 'Assignee'
	
	UNION ALL

	Select Top 1 ReferralCareGiverID,  @CommType
	FROM ReferralCareGivers (nolock) 
	WHERE ReferralID = @ReferralID AND @CommType = 'Caregiver'

	UNION ALL
	
		
	SELECT DISTINCT EmployeeID, @CommType --, GroupIDs, FirstName, LastName, val
	FROM dbo.Employees
	CROSS APPLY dbo.f_split(GroupIDs, ',')
	WHERE 
	@CommType = 'Support' AND
	IsDeleted = 0 AND 
	ISNULL(GroupIDs, '') <> '' AND 
	ISNULL(val, '') <> '' AND
	val IN (
		SELECT DDMasterid --, DDMasterTypeID, Name, Title 
		FROM dbo.lu_DDMasterTypes 
		INNER JOIN	dbo.DDMaster ON DDMaster.ItemType = lu_DDMasterTypes.DDMasterTypeID
		WHERE DDMasterTypeID = @SupportGroupID
		AND	@CommType = 'Support'
	)


END