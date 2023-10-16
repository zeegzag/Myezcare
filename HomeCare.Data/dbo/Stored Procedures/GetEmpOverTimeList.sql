CREATE PROCEDURE [dbo].[GetEmpOverTimeList]  
  @StartDate date,  
  @EndDate date,  
  @SortExpression nvarchar(100),  
  @SortType nvarchar(10),  
  @FromIndex int,  
  @PageSize int,  
  @WeekStartDay int = 0  
AS  
BEGIN  
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
  ;  
  WITH EHours  
  AS  
  (  
    SELECT  
      OT.EmployeeID,  
      SUM(OT.RegularMinutes) RegularMinutes,  
      SUM(OT.VisitedMinutes) VisitedMinutes,  
      SUM(OT.OverTimeVisitedMinutes) OverTimeVisitedMinutes  
    FROM  
    (  
      SELECT  
        T.EmployeeID,  
        T.DWM,  
        MIN(T.IndividualDate) IndividualDate,  
        SUM(T.RegularMinutes) RegularMinutes,  
        SUM(T.VisitedMinutes) VisitedMinutes,  
        CASE  
          WHEN SUM(T.VisitedMinutes) - SUM(T.RegularMinutes) <= 0 THEN 0  
          ELSE SUM(T.VisitedMinutes) - SUM(T.RegularMinutes)  
        END OverTimeVisitedMinutes  
      FROM dbo.EmpHours(NULL, @StartDate, @EndDate, @WeekStartDay) T  
      GROUP BY T.EmployeeID,  
               T.DWM  
    ) OT  
    GROUP BY OT.EmployeeID  
  ),  
  CTEGetEmpClockInOutList  
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
              WHEN @SortExpression = 'Employee' THEN t.EmpFirstName  
            END  
        END ASC,  
        CASE  
          WHEN @SortType = 'DESC' THEN CASE  
              WHEN @SortExpression = 'Employee' THEN t.EmpFirstName  
            END  
        END DESC,  
  
  
        CASE  
          WHEN @SortType = 'ASC' THEN CASE  
              WHEN @SortExpression = 'WeeklyAllocatedHours' THEN CONVERT(bigint, t.WeeklyAllocatedHours)  
            END  
        END ASC, CASE  
          WHEN @SortType = 'DESC' THEN CASE  
              WHEN @SortExpression = 'WeeklyAllocatedHours' THEN CONVERT(bigint, t.WeeklyAllocatedHours, 105)  
            END  
        END DESC,  
  
  
        CASE  
          WHEN @SortType = 'ASC' THEN CASE  
              WHEN @SortExpression = 'WeeklyUsedHours' THEN CONVERT(bigint, t.WeeklyUsedHours)  
            END  
        END ASC, CASE  
          WHEN @SortType = 'DESC' THEN CASE  
              WHEN @SortExpression = 'WeeklyUsedHours' THEN CONVERT(bigint, t.WeeklyUsedHours, 105)  
            END  
        END DESC,  
  
        CASE  
          WHEN @SortType = 'ASC' THEN CASE  
              WHEN @SortExpression = 'WeeklyOverTimeHours' THEN CONVERT(bigint, t.WeeklyOverTimeHours)  
            END  
        END ASC, CASE  
          WHEN @SortType = 'DESC' THEN CASE  
              WHEN @SortExpression = 'WeeklyOverTimeHours' THEN CONVERT(bigint, t.WeeklyOverTimeHours, 105)  
            END  
        END DESC  
  
        ) AS ROW,  
        t.*  
      FROM  
      (  
          SELECT  
            E.EmployeeID,  
            EmpFirstName = E.FirstName,  
            EmpLastName = E.LastName,  
			Employee=dbo.GetGenericNameFormat(E.FirstName,E.MiddleName, E.LastName,@NameFormat),
            WeeklyAllocatedHours = CAST(ROUND(EH.RegularMinutes / 60.0, 2) AS numeric(36, 2)),  
            WeeklyUsedHours = CAST(ROUND(EH.VisitedMinutes / 60.0, 2) AS numeric(36, 2)),  
   WeeklyOverTimeHours = CAST(ROUND(EH.OverTimeVisitedMinutes / 60.0, 2) AS numeric(36, 2))  
          FROM EHours EH  
          INNER JOIN Employees E  
            ON E.EmployeeID = EH.EmployeeID  
    WHERE  
   EH.OverTimeVisitedMinutes > 0  
      ) AS T  
  
    ) AS T1  
  )  
  
  SELECT  
    *  
  FROM CTEGetEmpClockInOutList  
  WHERE  
    ROW BETWEEN ((@PageSize * (@FromIndex - 1)) + 1) AND (@PageSize * @FromIndex)  
END