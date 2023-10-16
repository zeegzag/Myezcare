-- =============================================
-- Author:		Pallav Saxena
-- Create date: 05/24/2020
-- Description:	List of all the patients
-- =============================================
CREATE PROCEDURE [rpt].[ReferralList] 
@orgID int =null, @dbname varchar(4000)=null,@domainName varchar(4000)=null

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--select r.ReferralID,r.LastName+', '+r.FirstName from Referrals r
    
	WITH CTE AS (
	SELECT 0 AS ReferralID,'All' AS ReferralName
	UNION ALL
	SELECT DISTINCT ReferralID,LastName + ', ' + FirstName AS ReferralName 
	FROM dbo.Referrals e where IsDeleted=0)
	SELECT * FROM CTE
	WHERE ReferralName IS NOT NULL
	ORDER BY CASE WHEN ReferralName = 'All' THEN '0'
		ELSE ReferralName END ASC;
END