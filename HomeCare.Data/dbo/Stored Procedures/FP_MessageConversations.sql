CREATE PROCEDURE [dbo].[FP_MessageConversations]
	-- Add the parameters for the stored procedure here
	@UserID		bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--select * from dbo.Messages

	
	;With 
	CTE AS (
		Select Distinct ConversationID As CID, CategoryID, --ConversationDate = Cast(CreatedDate As Date)
		ConversationDate = Max(CreatedDate)
		from dbo.[Messages]
		Where CreatedBy = @UserID
		Group By ConversationID, CategoryID
	),
	CTE2 AS (
		Select Distinct CreatedBy from dbo.[Messages] M
		Inner Join CTE on CTE.CID = M.ConversationID
		Where M.CreatedBy != @UserID
	), 
	CTE3 As
	(
	Select CTE.CID As ConversationID, CTE.CategoryID, CTE2.CreatedBy, DDMaster.Title as CategoryTitle, ConversationDate
	from CTE, CTE2, DDMaster
	Where 1 = 1
	--AND EmployeeID = CTE2.CreatedBy
	AND CTE.CategoryID = DDMaster.DDMasterID
	)

	Select Distinct 
				CTE3.*,
				Employees.FirstName as CreatedByFirst, 
				Employees.LastName as CreatedByLast  
	from CTE3
	left outer join Employees on CTE3.CreatedBy = Employees.EmployeeID

	--select * from DDMaster



END