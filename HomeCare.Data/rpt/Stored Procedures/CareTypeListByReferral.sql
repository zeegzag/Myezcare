CREATE PROCEDURE [rpt].[CareTypeListByReferral]   (@ReferralID bigint)
  
AS  
BEGIN  
 SET NOCOUNT ON;  
      
 WITH CTE AS (  
 SELECT 0 AS CareTypeID,'All' AS CareTypeName  
 UNION ALL  
 SELECT DISTINCT DDMasterID AS CareTypeID,DDM.Title AS CareTypeName   
 FROM dbo.DDMaster DDM inner join referrals R on ddm.DDMasterID in (select val from dbo.GetCSVTable(r.CareTypeIds)) WHERE DDM.IsDeleted=0 AND ItemType=1 and r.ReferralID=@ReferralID)  
 SELECT * FROM CTE  
 WHERE CareTypeName IS NOT NULL  
 ORDER BY CASE WHEN CareTypeName = 'All' THEN '0'  
  ELSE CareTypeName END ASC;  
END