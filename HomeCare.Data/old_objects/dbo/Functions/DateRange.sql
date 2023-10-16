
CREATE FUNCTION [dbo].[DateRange]
(     
      @StartDate              DATETIME,
      @EndDate                DATETIME
)
RETURNS  
@SelectedRange    TABLE 
(IndividualDate DATE,DayNameStr VARCHAR(100),DayNameInt Int)
AS 
BEGIN
      ;WITH cteRange (DateRange) AS (
						SELECT @StartDate
						UNION ALL
						SELECT  DATEADD(dd, 1, DateRange) FROM cteRange
						WHERE DateRange <= DATEADD(dd, -1, @EndDate)
                  )
          
      INSERT INTO @SelectedRange (IndividualDate,DayNameStr,DayNameInt)
      SELECT CONVERT(date, DateRange),DATENAME(dw,DateRange),DATEPart(dw,DateRange)
      FROM cteRange
      OPTION (MAXRECURSION 3660);
      RETURN
END
