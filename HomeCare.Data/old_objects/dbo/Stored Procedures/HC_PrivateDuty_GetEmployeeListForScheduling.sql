CREATE PROCEDURE [dbo].[HC_PrivateDuty_GetEmployeeListForScheduling]
 @Name VARCHAR(100)=null,                            
 @FrequencyCodeID bigint=0,                              
 @StrSkillList VARCHAR(MAX) = NULL,                              
 @StrPreferenceList VARCHAR(MAX) = NULL,        
 @StartDate DATETIME,        
 @EndDate DATETIME,        
 @SortExpression VARCHAR(100),                                
 @SortType VARCHAR(10),                              
 @FromIndex INT,                              
 @PageSize INT                     
AS                              
BEGIN          
        
IF(@StrSkillList IS NULL OR LEN(@StrSkillList)=0)        
SET @StrSkillList=NULL;        
        
        
IF(@StrPreferenceList IS NULL OR LEN(@StrPreferenceList)=0)        
SET @StrPreferenceList=NULL;        
        
        
 DECLARE @TempEmpHrs TABLE(        
  EmployeeID BIGINT,        
  FirstName VARCHAR(50),        
  LastName VARCHAR(50),        
  NewAllocatedHrs FLOAT        
 )        
        
 ;WITH CTE01TempEmployeeList AS                              
    (            
  SELECT DISTINCT E.EmployeeID, E.FirstName,E.LastName        
  FROM Employees E        
  LEFT JOIN EmployeePreferences EP ON EP.EmployeeID = E.EmployeeID        
  LEFT JOIN Preferences PS ON PS.PreferenceID = EP.PreferenceID        
  WHERE E.IsDeleted=0 AND E.RoleID!=1 AND ((@Name IS NULL OR LEN(@Name)=0)                   
  OR                  
    ((E.FirstName LIKE '%'+@Name+'%' )OR                    
     (E.LastName  LIKE '%'+@Name+'%') OR                    
     (E.FirstName +' '+E.LastName like '%'+@Name+'%') OR                    
     (E.LastName +' '+E.FirstName like '%'+@Name+'%') OR                    
     (E.FirstName +', '+E.LastName like '%'+@Name+'%') OR                    
     (E.LastName +', '+E.FirstName like '%'+@Name+'%'))                  
  )         
  --AND (  (@SkillId=0 AND @PreferenceId=0) OR        
  --    ((@SkillId=0 AND @PreferenceId!=0) AND (EP.PreferenceID = @PreferenceId)) OR        
  --    ((@SkillId!=0 AND @PreferenceId=0) AND (EP.PreferenceID = @SkillId)) OR        
  --    ((@SkillId!=0 AND @PreferenceId!=0) AND (EP.PreferenceID IN (@SkillId ,@PreferenceId)))  )        
        
  AND (  (@StrSkillList IS NULL  AND @StrPreferenceList IS NULL ) OR        
      ((@StrSkillList IS NULL  AND @StrPreferenceList IS NOT NULL ) AND (EP.PreferenceID IN (SELECT CONVERT(BIGINT, VAL) FROM GetCSVTable(@StrPreferenceList)) )) OR        
      ((@StrSkillList IS NOT NULL  AND @StrPreferenceList IS NULL) AND (EP.PreferenceID IN (SELECT CONVERT(BIGINT, VAL) FROM GetCSVTable(@StrSkillList)) )) OR        
      ((@StrSkillList IS NOT NULL AND @StrPreferenceList IS NOT NULL) AND         
      (        
       (EP.PreferenceID IN (SELECT CONVERT(BIGINT, VAL) FROM GetCSVTable(@StrPreferenceList)) ) OR        
       (EP.PreferenceID IN (SELECT CONVERT(BIGINT, VAL) FROM GetCSVTable(@StrSkillList)) )        
      )        
              
      ))        
        
        
        
   )        
        
         
    INSERT INTO @TempEmpHrs        
 SELECT DISTINCT E.EmployeeID, E.FirstName,E.LastName,        
 NewAllocatedHrs= SUM(DATEDIFF(SECOND, ETD.EmployeeTSStartTime, ETD.EmployeeTSEndTime) / 3600.0) OVER(PARTITION BY ETD.EmployeeID)        
 FROM CTE01TempEmployeeList E        
 LEFT JOIN EmployeeTimeSlotDates ETD ON ETD.EmployeeID=E.EmployeeID AND (ETD.EmployeeTSDate >= @StartDate AND ETD.EmployeeTSDate<=@EndDate)        
 WHERE 1=1        
         
        
 --SELECT * FROM EmployeeTimeSlotDates ORDER BY EmployeeTSDate ASC        
        
         
-- EXEC HC_GetEmployeeListForScheduling @SkillId = '', @PreferenceId = '0',@StartDate ='2018-03-26',@EndDate ='2018-03-26', @SortExpression = 'Name', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'        
 --SELECT * FROM @TempEmpHrs;        
                         
 ;WITH CTE01EmployeeListForSchedulings AS                              
 (            
         
         
                            
 SELECT  *,COUNT(t2.EmployeeID) OVER() AS Count FROM                               
  (                              
  SELECT ROW_NUMBER() OVER (ORDER BY                         
                        
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Name' THEN LastName  END END ASC,                            
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Name' THEN LastName  END END DESC,        
        
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'NewUsedHrs' THEN NewUsedHrs  END END ASC,                            
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'NewUsedHrs' THEN NewUsedHrs  END END DESC,        
        
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'NewAllocatedHrs' THEN NewAllocatedHrs  END END ASC,                            
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'NewAllocatedHrs' THEN NewAllocatedHrs  END END DESC  ,        
        
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'NewRemainingHrs' THEN NewRemainingHrs  END END ASC,                            
  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'NewRemainingHrs' THEN NewRemainingHrs  END END DESC        
        
   ) AS Row,         
      * FROM (        
   SELECT --*,       
   --NewRemainingHrs= NewAllocatedHrs -   CASE WHEN NewUsedHrs > NewAllocatedHrs THEN ISNULL(NewAllocatedHrs,0) ELSE ISNULL(NewUsedHrs,0) END        
   EmployeeID, FirstName, LastName,       
   NewAllocatedHrs= CONVERT(DECIMAL(10,2),NewAllocatedHrs),    
   NewUsedHrs= CONVERT(DECIMAL(10,2),NewUsedHrs),      
   NewRemainingHrs=CONVERT(DECIMAL(10,2), NewAllocatedHrs -   CASE WHEN NewUsedHrs > NewAllocatedHrs THEN ISNULL(NewAllocatedHrs,0) ELSE ISNULL(NewUsedHrs,0) END )       
         
   FROM  (          
    SELECT DISTINCT E.*,--SM.ScheduleID,ETD.EmployeeTSDateID,SM.StartDate, SM.EndDate,        
    --NewUsedHrs01= SUM(DATEDIFF(second, ETD.EmployeeTSStartTime, ETD.EmployeeTSEndTime) / 3600.0) OVER(PARTITION BY SM.EmployeeID),        
    NewUsedHrs=SUM(DATEDIFF(second, SM.StartDate, SM.EndDate) / 3600.0) OVER(PARTITION BY SM.EmployeeID)  
 --CASE WHEN (R.IsDeleted=0 OR R.ReferralStatusID=1) THEN SUM(DATEDIFF(second, SM.StartDate, SM.EndDate) / 3600.0) OVER(PARTITION BY SM.EmployeeID) ELSE 0 END  
    FROM @TempEmpHrs E        
    LEFT JOIN ScheduleMasters SM ON SM.EmployeeID =  E.EmployeeID AND SM.IsDeleted=0  
    AND SM.StartDate BETWEEN @StartDate AND @EndDate+1        
    AND SM.EndDate BETWEEN @StartDate AND @EndDate+1  
 --LEFT JOIN Referrals R ON R.ReferralID = SM.ReferralID  
    --LEFT JOIN EmployeeTimeSlotDates ETD ON ETD.EmployeeID=SM.EmployeeID AND ETD.EmployeeTSDateID = SM.EmployeeTSDateID        
    --AND (ETD.EmployeeTSDate BETWEEN @StartDate AND @EndDate)        
    WHERE 1=1 --AND ETD.EmployeeTSDateID IS NOT NULL        
   ) AS t          
       ) AS t1              
   ) AS t2 )                              
                               
 SELECT * FROM CTE01EmployeeListForSchedulings WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                        
                           
END        
        
-- EXEC HC_GetEmployeeListForScheduling @Name='Aaron', @SkillId = '', @PreferenceId = '0',@StartDate ='2018-03-26',@EndDate ='2018-03-26', @SortExpression = 'Name', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50'
