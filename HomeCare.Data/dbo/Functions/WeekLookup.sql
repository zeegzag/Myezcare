CREATE FUNCTION [dbo].[WeekLookup]
(
	@WeekStartDay int
)
RETURNS @returntable TABLE
(
	WEEKSTARTDATE date,
	[WeekDesc] nvarchar(max)
)
AS
BEGIN

    DECLARE @Date date = GETDATE()
	;WITH wk (n, WeekNum)
	AS
	(
	  SELECT 1, DATEDIFF(WEEK, '1900-01-01', DATEADD(D, -1 * @WeekStartDay, @Date))
	  UNION ALL
	  SELECT wk.n + 1, wk.WeekNum - 1
	  FROM wk
	  WHERE wk.n < 20
	),
	cte_numbers (n, WEEKNUMBER, WEEKSTARTDATE, WEEKENDDATE)
	AS
	(
	  SELECT wk.n, wk.WeekNum, wks.WeekStart, wke.WeekEnd
	  FROM wk
	  CROSS APPLY
	  (
		SELECT CONVERT(DATE, DATEADD(D, @WeekStartDay - 1, DATEADD(WEEK, wk.WeekNum, '1900-01-01'))) WeekStart
	  ) wks
	  CROSS APPLY
	  (
		SELECT CONVERT(DATE, DATEADD(DAY, 6, wks.WeekStart)) WeekEnd
	  ) wke
	)
	INSERT @returntable
	SELECT WEEKSTARTDATE, 'Week ' + CAST(CASE DATEPART(wk, WEEKSTARTDATE) % 53 WHEN 0 THEN 1 ELSE DATEPART(wk, WEEKSTARTDATE) END AS nvarchar(max)) + ' - (' + FORMAT(WEEKSTARTDATE, 'dd MMM yyyy') + '-' + FORMAT(WEEKENDDATE, 'dd MMM yyyy') + ')' AS [WeekDesc]
	FROM cte_numbers
	ORDER BY WEEKNUMBER DESC;
	
	RETURN
END
