             
                
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
  @PageSize int,                  
  @WeekStartDay int = 0                  
AS        
        
--select @Name = '', @StrSkillList = '', @StrPreferenceList = '', @FrequencyCodeID = '0', @WeekStartDay = '1', @StartDate = '2022/12/19', @EndDate = '2022/12/25', @SortExpression = 'Name', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'        
BEGIN  
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
  declare @ecount bigint        
  select @ecount=count(EmployeeID) from employees where IsDeleted=0        
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
      E.LastName ,
	  EmployeeName=dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat)
    FROM Employees E                      
    WHERE                    
   E.IsDeleted = 0                  
  AND (                  
  (                  
   @Name IS NULL                  
   OR LEN(@Name) = 0                  
   )                  
  OR (                  
   (E.FirstName LIKE '%' + @Name + '%')                  
   OR (E.LastName LIKE '%' + @Name + '%')                  
   OR (E.FirstName + ' ' + E.LastName LIKE '%' + @Name + '%')                  
   OR (E.LastName + ' ' + E.FirstName LIKE '%' + @Name + '%')                  
   OR (E.FirstName + ', ' + E.LastName LIKE '%' + @Name + '%')                  
   OR (E.LastName + ', ' + E.FirstName LIKE '%' + @Name + '%')                  
   )                  
  )                  
  AND (                  
  @StrSkillList IS NULL                  
  OR EXISTS (                  
   SELECT *                  
   FROM EmployeePreferences EP                  
   WHERE EP.EmployeeID = E.EmployeeID                  
   AND EP.PreferenceID IN (                  
    SELECT CONVERT(BIGINT, VAL)                  
    FROM GetCSVTable(@StrSkillList)                  
    )                  
   )                  
  )                  
  AND (                  
  @StrPreferenceList IS NULL                  
  OR EXISTS (                  
   SELECT *                  
   FROM EmployeePreferences EP                  
   WHERE EP.EmployeeID = E.EmployeeID                  
   AND EP.PreferenceID IN (                  
    SELECT CONVERT(BIGINT, VAL)                  
    FROM GetCSVTable(@StrPreferenceList)                  
    )                  
   )                  
  )                  
  ),                    
  EHours AS                    
  (                    
    SELECT                    
      T.EmployeeID,                    
      SUM(ISNULL(T.RegularMinutes, 0)) / 60.0 NewAllocatedHrs,                    
      SUM(ISNULL(T.ScheduledMinutes, 0)) / 60.0 NewUsedHrs                    
    --FROM dbo.EmpHoursOnScheduleMaster(NULL, @StartDate, @EndDate, @WeekStartDay, @FromIndex, @PageSize, @Name) T                  
  FROM dbo.EmpHours(NULL, @StartDate, @EndDate, @WeekStartDay) T                    
    GROUP BY T.EmployeeID                    
  ),                    
  CTE01EmployeeListForSchedulings AS                    
  (                    
    SELECT                    
      *,    
   --@ecount as Count   
   COUNT(t2.EmployeeID) OVER () as Count   
    FROM                    
    (                    
      SELECT                    
       --((@PageSize * (@FromIndex - 1)) )+ ROW_NUMBER() OVER (ORDER BY                    
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
