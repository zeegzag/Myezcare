  
  
  
CREATE PROCEDURE [rpt].[TransportGroupList]   
(  
@StartDate datetime = null,   
@EndDate datetime = null,  
@FacilityID BIGINT = 0  
)  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
      
 WITH CTE AS (  
 SELECT 0 AS TransportGroupID,'All' AS [Name]  
 UNION ALL  
 SELECT TransportGroupID,[Name]=[Name]  FROM [fleet].TransportGroup   (nolock)           
 where IsNull(IsDeleted,0)=0  
 and (StartDate >= @StartDate or @StartDate is null)  
 and (EndDate >= @EndDate or @EndDate is null)  
 and FacilityID = @FacilityID  
 )  
 SELECT * FROM CTE  
 WHERE [Name] IS NOT NULL  
 ORDER BY CASE WHEN [Name] = 'All' THEN '0'  
  ELSE [Name] END ASC;  
END
GO

