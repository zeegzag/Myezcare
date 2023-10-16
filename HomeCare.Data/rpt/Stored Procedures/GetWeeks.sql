CREATE PROCEDURE [rpt].[GetWeeks]
AS
BEGIN
  DECLARE @WeekStartDay INT = [dbo].[GetWeekStartDay]();

  SELECT *
  FROM [dbo].[WeekLookup](ISNULL(@WeekStartDay, 0))
END
