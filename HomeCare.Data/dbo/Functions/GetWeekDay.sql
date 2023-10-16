CREATE FUNCTION [dbo].[GetWeekDay](@d int)
RETURNS VARCHAR(9)
WITH SCHEMABINDING, RETURNS NULL ON NULL INPUT
AS
BEGIN
RETURN 
(
SELECT 
    CASE @d
        WHEN 1 THEN 'Sun'
        WHEN 2 THEN 'Mon'
        WHEN 3 THEN 'Tue'
        WHEN 4 THEN 'Wed'
        WHEN 5 THEN 'Thu'
        WHEN 6 THEN 'Fri'
        WHEN 7 THEN 'Sat'
    END
)
END