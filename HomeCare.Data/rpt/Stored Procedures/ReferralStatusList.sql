-- =============================================
-- Author:		Kalpesh Patel
-- Create date: 05/24/2020
-- Description:	List of all the patients
-- =============================================
CREATE PROCEDURE [rpt].[ReferralStatusList] 
--@orgID int =null, @dbname varchar(4000)=null,@domainName varchar(4000)=null

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--select r.ReferralID,r.LastName+', '+r.FirstName from Referrals r
    
	WITH CTE AS (
	SELECT 0 AS ReferralStatusID,'All' AS ReferralStatusName
	UNION ALL
	SELECT DISTINCT ReferralStatusID,Status AS ReferralStatusName
	FROM dbo.ReferralStatuses e)
	SELECT * FROM CTE
	WHERE ReferralStatusName IS NOT NULL
	ORDER BY CASE WHEN ReferralStatusName = 'All' THEN '0'
		ELSE ReferralStatusName END ASC;
END