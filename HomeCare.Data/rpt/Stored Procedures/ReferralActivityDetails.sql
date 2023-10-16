

--  EXEC [rpt].[ReferralActivityDetails] 40 ,'March' ,'2022'
CREATE PROCEDURE [rpt].[ReferralActivityDetails]
@ReferralID BIGINT=0,
@Month nvarchar(100)=NULL,
@Year nvarchar(100)=NULL

AS

BEGIN

 
  WITH CTEEmployeeList
  AS (SELECT	distinct
   *,
   COUNT(t1.ReferralActivityListId) OVER () AS Count
  FROM (SELECT
  --  ROW_NUMBER() OVER (ORDER BY

    --CASE
    --  WHEN @SortType = 'ASC' THEN CASE
    --      WHEN @SortExpression = 'ReferralActivityListId' THEN RA.ReferralActivityListId
    --    END
    --END ASC,
    --CASE
    --  WHEN @SortType = 'DESC' THEN CASE
    --      WHEN @SortExpression = 'ReferralActivityListId' THEN RA.ReferralActivityListId
    --    END
    --END DESC
   
    --) AS Row,
      RA.ReferralActivityListId,RAC.Name,RAC.Category, RAM.ReferralId, RAM.Month, RAM.Year,RAM.ReferralActivityMasterId, RA.ReferralActivityCategoryId, RA.Day1, RA.Day2, RA.Day3, RA.Day4, RA.Day5, RA.Day6, RA.Day7, RA.Day8, RA.Day9, RA.Day10, RA.Day11, RA.Day12, RA.Day13, RA.Day14, RA.Day15, RA.Day16, RA.Day17, RA.Day18, RA.Day19, RA.Day20, RA.Day21, RA.Day22, RA.Day23, RA.Day24, RA.Day25, RA.Day26, RA.Day27, RA.Day28, RA.Day29, RA.Day30, RA.Day31
  from ReferralActivityList RA
  inner join ReferralActivityCategory RAC on RAC.ReferralActivityCategoryId=RA.ReferralActivityCategoryId
  inner join ReferralActivityMaster RAM on RAM.ReferralActivityMasterId=RA.ReferralActivityMasterId

  where
  ((@ReferralID =0) OR (RAM.ReferralID=@ReferralID))
   AND ((@Month IS NULL OR LEN(RAM.Month) = 0) OR ((RAM.Month LIKE '%' + @Month + '%')))
   AND ((@Year IS NULL OR LEN(RAM.Year) = 0) OR ((RAM.Year LIKE '%' + @Year + '%')))

   )AS t1)

  SELECT
   cte.ReferralActivityListId,cte.Name,cte.Category, cte.ReferralId, cte.Month, cte.Year,cte.ReferralActivityMasterId, cte.ReferralActivityCategoryId, cte.Day1, cte.Day2, cte.Day3, cte.Day4, cte.Day5, cte.Day6, cte.Day7, cte.Day8, cte.Day9, cte.Day10, cte.Day11, cte.Day12, cte.Day13, cte.Day14, cte.Day15, cte.Day16, cte.Day17, cte.Day18, cte.Day19, cte.Day20, cte.Day21, cte.Day22, cte.Day23, cte.Day24, cte.Day25, cte.Day26, cte.Day27, cte.Day28, cte.Day29, cte.Day30, cte.Day31
  FROM CTEEmployeeList  cte
  order by cte.Category

END
