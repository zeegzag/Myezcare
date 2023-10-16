-- =============================================
-- Author:		Kalpesh Patel
-- Create date: 05/27/2020
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [rpt].[GetPayors] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	WITH CTE AS (
	 SELECT '0' AS PayorID, 'ALL' AS PayorName UNION ALL
	SELECT PayorID,PayorName from Payors with(nolock)
	WHERE IsDeleted=0
	)
	SELECT * FROM CTE
	Order by CASE WHEN PayorName = 'All' THEN '0'
		ELSE PayorName END ASC;
END