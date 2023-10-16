--EXEC HC_GetEmployeeListForScheduling @Name = 'pravin', @StrSkillList = '', @StrPreferenceList = '', @FrequencyCodeID = '0', @StartDate = '2018/08/27', @EndDate = '2018/09/02', @SortExpression = 'Name', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'         
CREATE PROCEDURE [dbo].[HC_GetEmployeeListForScheduling]
  @Name varchar(100) = NULL,
  @FrequencyCodeID bigint = 0,
  @StrSkillList varchar(max) = NULL,
  @StrPreferenceList varchar(max) = NULL,
  @StartDate datetime,
  @EndDate datetime,
  @SortExpression varchar(100),
  @SortType varchar(10),
  @FromIndex int,
  @PageSize int
AS
BEGIN

  IF (@StrSkillList IS NULL
    OR LEN(@StrSkillList) = 0)
    SET @StrSkillList = NULL;

  IF (@StrPreferenceList IS NULL
    OR LEN(@StrPreferenceList) = 0)
    SET @StrPreferenceList = NULL;

  WITH FilteredEmployees AS
  (
    SELECT
      E.EmployeeID,
      E.FirstName,
      E.LastName
    FROM Employees E
    LEFT JOIN EmployeePreferences EP
      ON EP.EmployeeID = E.EmployeeID
    LEFT JOIN Preferences PS
      ON PS.PreferenceID = EP.PreferenceID
    WHERE
      E.IsDeleted = 0
      AND ((@Name IS NULL
          OR LEN(@Name) = 0)
        OR ((E.FirstName LIKE '%' + @Name + '%')
          OR (E.LastName LIKE '%' + @Name + '%')
          OR (E.FirstName + ' ' + E.LastName LIKE '%' + @Name + '%')
          OR (E.LastName + ' ' + E.FirstName LIKE '%' + @Name + '%')
          OR (E.FirstName + ', ' + E.LastName LIKE '%' + @Name + '%')
          OR (E.LastName + ', ' + E.FirstName LIKE '%' + @Name + '%'))
      )
      AND ((@StrSkillList IS NULL
          AND @StrPreferenceList IS NULL)
        OR ((@StrSkillList IS NULL
            AND @StrPreferenceList IS NOT NULL)
          AND (EP.PreferenceID IN
      (
        SELECT
          CONVERT(bigint, VAL)
        FROM GetCSVTable(@StrPreferenceList)
      )
      ))
        OR ((@StrSkillList IS NOT NULL
            AND @StrPreferenceList IS NULL)
          AND (EP.PreferenceID IN
      (
        SELECT
          CONVERT(bigint, VAL)
        FROM GetCSVTable(@StrSkillList)
      )
      ))
        OR ((@StrSkillList IS NOT NULL
            AND @StrPreferenceList IS NOT NULL)
          AND (
      (EP.PreferenceID IN
      (
        SELECT
          CONVERT(bigint, VAL)
        FROM GetCSVTable(@StrPreferenceList)
      )
      )
            OR (EP.PreferenceID IN
      (
        SELECT
          CONVERT(bigint, VAL)
        FROM GetCSVTable(@StrSkillList)
      )
      )
      )

      ))
  ),
  EHours AS
  (
    SELECT
      T.EmployeeID,
      SUM(ISNULL(T.RegularMinutes, 0)) / 60.0 NewAllocatedHrs,
      SUM(ISNULL(T.ScheduledMinutes, 0)) / 60.0 NewUsedHrs
    FROM dbo.EmpHours(NULL, @StartDate, @EndDate) T
    GROUP BY T.EmployeeID
  ),
  CTE01EmployeeListForSchedulings AS
  (
    SELECT
      *,
      COUNT(t2.EmployeeID) OVER () AS Count
    FROM
    (
      SELECT
        ROW_NUMBER() OVER (ORDER BY

        CASE
          WHEN @SortType = 'ASC' THEN CASE
              WHEN @SortExpression = 'Name' THEN LastName
            END
        END ASC,
        CASE
          WHEN @SortType = 'DESC' THEN CASE
              WHEN @SortExpression = 'Name' THEN LastName
            END
        END DESC,

        CASE
          WHEN @SortType = 'ASC' THEN CASE
              WHEN @SortExpression = 'NewUsedHrs' THEN NewUsedHrs
            END
        END ASC,
        CASE
          WHEN @SortType = 'DESC' THEN CASE
              WHEN @SortExpression = 'NewUsedHrs' THEN NewUsedHrs
            END
        END DESC,

        CASE
          WHEN @SortType = 'ASC' THEN CASE
              WHEN @SortExpression = 'NewAllocatedHrs' THEN NewAllocatedHrs
            END
        END ASC,
        CASE
          WHEN @SortType = 'DESC' THEN CASE
              WHEN @SortExpression = 'NewAllocatedHrs' THEN NewAllocatedHrs
            END
        END DESC,

        CASE
          WHEN @SortType = 'ASC' THEN CASE
              WHEN @SortExpression = 'NewRemainingHrs' THEN NewRemainingHrs
            END
        END ASC,
        CASE
          WHEN @SortType = 'DESC' THEN CASE
              WHEN @SortExpression = 'NewRemainingHrs' THEN NewRemainingHrs
            END
        END DESC

        ) AS Row,
        *
      FROM
      (
        SELECT
          EP.*,
          NewAllocatedHrs = CONVERT(decimal(10, 2), NewAllocatedHrs),
          NewUsedHrs = CONVERT(decimal(10, 2), NewUsedHrs),
          NewRemainingHrs = CONVERT(decimal(10, 2), NewAllocatedHrs - CASE
            WHEN NewUsedHrs > NewAllocatedHrs THEN NewAllocatedHrs
            ELSE NewUsedHrs
          END)
        FROM FilteredEmployees EP
        INNER JOIN EHours H
          ON EP.EmployeeID = H.EmployeeID
      ) AS t1
    ) AS t2
  )

  SELECT
    *
  FROM CTE01EmployeeListForSchedulings
  WHERE
    ROW BETWEEN ((@PageSize * (@FromIndex - 1)) + 1) AND (@PageSize * @FromIndex)

END

-- EXEC HC_GetEmployeeListForScheduling @Name='Aaron', @SkillId = '', @PreferenceId = '0',@StartDate ='2018-03-26',@EndDate ='2018-03-26', @SortExpression = 'Name', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50' 