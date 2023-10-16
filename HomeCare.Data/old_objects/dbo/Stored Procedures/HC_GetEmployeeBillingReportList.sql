-- EXEC HC_GetEmployeeBillingReportList 'sunil 5','2018-09-10','2018-09-16','EmployeeName','ASC','1','100'          
CREATE PROCEDURE [dbo].[HC_GetEmployeeBillingReportList]
  @EmployeeName nvarchar(200) = NULL,
  @StartDate date = NULL,
  @EndDate date = NULL,
  @SortExpression nvarchar(100) = 'EmployeeName',
  @SortType nvarchar(10) = 'ASC',
  @FromIndex int,
  @PageSize int
AS
BEGIN

  ;
  WITH FilteredEmployees
  AS
  (
    SELECT
      E.EmployeeID,
      dbo.GetGeneralNameFormat(E.FirstName, E.LastName) AS EmployeeName,
      E.RegularPayHours,
      E.OvertimePayHours
    FROM Employees E
    WHERE
      ((@EmployeeName IS NULL
          OR LEN(@EmployeeName) = 0)
        OR (
      E.FirstName LIKE '%' + @EmployeeName + '%'
          OR E.LastName LIKE '%' + @EmployeeName + '%'
          OR E.FirstName + ' ' + E.LastName LIKE '%' + @EmployeeName + '%'
          OR E.LastName + ' ' + E.FirstName LIKE '%' + @EmployeeName + '%'
          OR E.FirstName + ', ' + E.LastName LIKE '%' + @EmployeeName + '%'
          OR E.LastName + ', ' + E.FirstName LIKE '%' + @EmployeeName + '%'))
      AND E.IsDeleted = 0
  ),
  EHours
  AS
  (
    SELECT
      OT.EmployeeID,
      SUM(OT.ScheduledMinutes) AllocatedHour,
      SUM(OT.PTOMinutes) PTOHour,
      CAST(ROUND(SUM(OT.VisitedMinutes) / 60.0, 2) AS numeric(36, 2)) WorkingHour,
      CAST(ROUND(SUM(OT.RegularMinutes) / 60.0, 2) AS numeric(36, 2)) RegularHours,
      CAST(ROUND(SUM(OT.OverTimeVisitedMinutes) / 60.0, 2) AS numeric(36, 2)) OvertimeHours
    FROM
    (
      SELECT
        T.EmployeeID,
        T.DWM,
        MIN(T.IndividualDate) IndividualDate,
        SUM(T.ScheduledMinutes) ScheduledMinutes,
        SUM(T.VisitedMinutes) VisitedMinutes,
        SUM(T.PTOMinutes) PTOMinutes,
        SUM(T.RegularMinutes) RegularMinutes,
        CASE
          WHEN SUM(T.VisitedMinutes) - SUM(T.RegularMinutes) <= 0 THEN 0
          ELSE SUM(T.VisitedMinutes) - SUM(T.RegularMinutes)
        END OverTimeVisitedMinutes
      FROM dbo.EmpHours((SELECT STRING_AGG(EmployeeID, ',') FROM FilteredEmployees), @StartDate, @EndDate) T
      GROUP BY T.EmployeeID,
               T.DWM
    ) OT
    GROUP BY OT.EmployeeID
  ),
  CTEEmployeeBillingReportList
  AS
  (
    SELECT
      *,
      COUNT(T1.EmployeeID) OVER () AS Count
    FROM
    (
      SELECT
        ROW_NUMBER() OVER (ORDER BY

        CASE
          WHEN @SortType = 'ASC' THEN CASE
              WHEN @SortExpression = 'EmployeeName' THEN EmployeeName
            END
        END ASC,
        CASE
          WHEN @SortType = 'DESC' THEN CASE
              WHEN @SortExpression = 'EmployeeName' THEN EmployeeName
            END
        END DESC,

        CASE
          WHEN @SortType = 'ASC' THEN CASE
              WHEN @SortExpression = 'AllocatedHour' THEN AllocatedHour
            END
        END ASC,
        CASE
          WHEN @SortType = 'DESC' THEN CASE
              WHEN @SortExpression = 'AllocatedHour' THEN AllocatedHour
            END
        END DESC,

        CASE
          WHEN @SortType = 'ASC' THEN CASE
              WHEN @SortExpression = 'PTOHour' THEN PTOHour
            END
        END ASC,
        CASE
          WHEN @SortType = 'DESC' THEN CASE
              WHEN @SortExpression = 'PTOHour' THEN PTOHour
            END
        END DESC,

        CASE
          WHEN @SortType = 'ASC' THEN CASE
              WHEN @SortExpression = 'WorkingHour' THEN WorkingHour
            END
        END ASC,
        CASE
          WHEN @SortType = 'DESC' THEN CASE
              WHEN @SortExpression = 'WorkingHour' THEN WorkingHour
            END
        END DESC
        ) AS Row,
        *
      FROM
      (
        SELECT
          E.EmployeeID,
          E.EmployeeName,
          T.AllocatedHour,
          T.PTOHour,
          T.WorkingHour,
          AllocatedHourInStr = CAST(ROUND(T.AllocatedHour / 60.0, 2) AS numeric(36, 2)),
          PTOHourInStr = CAST(ROUND(T.PTOHour / 60.0, 2) AS numeric(36, 2)),
          WorkingHourInStr = T.WorkingHour,
          RegHourInStr = T.RegularHours,
          OvertimeHoursInStr = T.OvertimeHours,
          RegularPayHours = CAST(ROUND(E.RegularPayHours, 2) AS numeric(36, 2)),
          OvertimePayHours = CAST(ROUND(E.OvertimePayHours, 2) AS numeric(36, 2))
        FROM FilteredEmployees E
        INNER JOIN EHours T
          ON T.EmployeeID = E.EmployeeID
      ) AS T
    ) AS T1
  )
  SELECT
    *
  FROM CTEEmployeeBillingReportList
  WHERE
    ROW BETWEEN ((@PageSize * (@FromIndex - 1)) + 1) AND (@PageSize * @FromIndex)
END