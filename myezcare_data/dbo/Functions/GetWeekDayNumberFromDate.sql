CREATE FUNCTION [dbo].[GetWeekDayNumberFromDate](@DATE DATETIME)
RETURNS INT
WITH SCHEMABINDING, RETURNS NULL ON NULL INPUT
AS
BEGIN

DECLARE @DayName VARCHAR(20);
SET @DayName= LEFT(DATENAME(DW,@DATE),3)


RETURN 
(




SELECT 
    CASE @DayName
        WHEN 'Sun'  THEN 1
        WHEN 'Mon'  THEN 2
        WHEN 'Tue'  THEN 3
        WHEN 'Wed'  THEN 4
        WHEN 'Thu'  THEN 5
        WHEN 'Fri'  THEN 6
        WHEN 'Sat'  THEN 7
    END
)
END
