
/*    
Created by : Neeraj Sharma    
Created Date: 14 August 2020    
Updated by :    
Updated Date :    
    
Purpose: This stored procedure is used to get EBForms Data list    
    
*/
-- EXEC GetFormList @MarketID = '0', @FormCategoryID = '0', @FormName = '', @FormNumber = '', @IsDeleted = '0', @SortExpression = 'EBFormID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'      

CREATE PROCEDURE [dbo].[EBFormsGetList]
  @MarketID bigint = NULL,
  @FormCategoryID bigint = NULL,
  @FormName varchar(20) = NULL,
  @FormNumber varchar(20) = NULL,
  @IsDeleted bigint = -1,
  @SORTEXPRESSION nvarchar(100),
  @SORTTYPE nvarchar(10),
  @FROMINDEX int,
  @PAGESIZE int
AS
BEGIN
  ;
  WITH CTEFormList
  AS (SELECT
    *,
    COUNT(EBFormID) OVER () AS COUNT
  FROM (SELECT
    ROW_NUMBER() OVER (ORDER BY

    CASE
      WHEN @SortType = 'ASC' THEN CASE
          WHEN @SortExpression = 'Name' THEN EF.Name
        END
    END ASC,
    CASE
      WHEN @SortType = 'DESC' THEN CASE
          WHEN @SortExpression = 'Name' THEN EF.Name
        END
    END DESC,

    CASE
      WHEN @SortType = 'ASC' THEN CASE
          WHEN @SortExpression = 'FormLongName' THEN EF.FormLongName
        END
    END ASC,
    CASE
      WHEN @SortType = 'DESC' THEN CASE
          WHEN @SortExpression = 'FormLongName' THEN EF.FormLongName
        END
    END DESC,

    CASE
      WHEN @SortType = 'ASC' THEN CASE
          WHEN @SortExpression = 'Version' THEN EF.Version
        END
    END ASC,
    CASE
      WHEN @SortType = 'DESC' THEN CASE
          WHEN @SortExpression = 'Version' THEN EF.Version
        END
    END DESC,

    CASE
      WHEN @SortType = 'ASC' THEN CASE
          WHEN @SortExpression = 'FormCategory' THEN EC.Name
        END
    END ASC,
    CASE
      WHEN @SortType = 'DESC' THEN CASE
          WHEN @SortExpression = 'FormCategory' THEN EC.Name
        END
    END DESC,

    CASE
      WHEN @SortType = 'ASC' THEN CASE
          WHEN @SortExpression = 'FormPrice' THEN CONVERT(decimal(18, 2), EF.FormPrice)
        END
    END ASC,
    CASE
      WHEN @SortType = 'DESC' THEN CASE
          WHEN @SortExpression = 'FormPrice' THEN CONVERT(decimal(18, 2), EF.FormPrice)
        END
    END DESC


    ) AS ROW,

    EF.*,
    EBCategoryIDs = EF.EBCategoryID,
    FormCategory = EC.Name
  FROM EBForms EF
  CROSS APPLY (SELECT
    STRING_AGG(EC.Name, ', ') Name
  FROM EBCategories EC
  WHERE EC.EBCategoryID IN (SELECT
    CONVERT(bigint, VAL)
  FROM GetCSVTable(EF.EBCategoryID))) EC
  WHERE

  --EF.IsActive=1  AND   
  ((@IsDeleted = -1)
  OR EF.IsDeleted = @IsDeleted)
  AND ((@FormName IS NULL
  OR LEN(@FormName) = 0)
  OR EF.FormLongName LIKE '%' + @FormName + '%')
  AND ((@FormNumber IS NULL
  OR LEN(@FormNumber) = 0)
  OR (EF.Name LIKE '%' + @FormNumber + '%'))
  AND ((@FormCategoryID IS NULL
  OR LEN(@FormCategoryID) = 0
  OR @FormCategoryID = 0)
  OR (@FormCategoryID IN (SELECT
    CONVERT(bigint, VAL)
  FROM GetCSVTable(EF.EBCategoryID))
  ))
  AND ((@MarketID IS NULL
  OR LEN(@MarketID) = 0
  OR @MarketID = 0)
  OR (@MarketID IN (SELECT
    CONVERT(bigint, VAL)
  FROM GetCSVTable(EF.EbMarketIDs))
  ))) AS P1)
  SELECT
    *
  FROM CTEFormList
  WHERE ROW BETWEEN ((@PAGESIZE * (@FROMINDEX - 1)) + 1) AND (@PAGESIZE * @FROMINDEX)

END