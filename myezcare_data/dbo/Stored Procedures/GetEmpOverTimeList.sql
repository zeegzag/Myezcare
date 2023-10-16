--SELECT * FROM Referrals WHERE LastName='abraham'      
--DELETE FROM ReferralTimeSlotDates WHERE ReferralID=1951      
--DELETE FROM ScheduleMasters       
--DELETE FROM EmployeeVisits      
--DELETE FROM EmployeeVisitNotes      
      
CREATE PROCEDURE [dbo].[GetEmpOverTimeList]          
@StartDate DATE =NULL,      
@EndDate  DATE=NULL,               
@SortExpression NVARCHAR(100),                        
@SortType NVARCHAR(10),                      
@FromIndex INT,                      
@PageSize INT          
          
AS                      
BEGIN                      
            
DECLARE @ExceededHrs BIGINT=40;      
                   
;WITH CTEGetEmpClockInOutList AS                      
 (                       
  SELECT *,COUNT(T1.EmployeeID) OVER() AS Count FROM                       
  (                      
   SELECT ROW_NUMBER() OVER (ORDER BY                       
                  
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Employee' THEN t.EmpFirstName END END ASC,                
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Employee' THEN t.EmpFirstName END END DESC,                 
               
       
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'WeeklyAllocatedHours' THEN  CONVERT(BIGINT, t.WeeklyAllocatedHours) END END ASC,                                CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'WeeklyAlloca     
 
tedHours' THEN  CONVERT(BIGINT, t.WeeklyAllocatedHours, 105) END END DESC,      
      
      
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'WeeklyUsedHours' THEN  CONVERT(BIGINT, t.WeeklyUsedHours) END END ASC,                                          CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'WeeklyUsedHo     
 
urs' THEN  CONVERT(BIGINT, t.WeeklyUsedHours, 105) END END DESC,      
      
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'WeeklyOverTimeHours' THEN  CONVERT(BIGINT, t.WeeklyOverTimeHours) END END ASC,                                  CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'WeeklyOverTi     
 
meHours' THEN  CONVERT(BIGINT, t.WeeklyOverTimeHours, 105) END END DESC      
              
   ) AS ROW,                      
   t.*  FROM     (            
      
SELECT *,      
WeeklyOverTimeHours= CASE WHEN @ExceededHrs < WeeklyUsedHours THEN WeeklyUsedHours - @ExceededHrs ELSE 0 END      
 FROM (      
SELECT DISTINCT E.EmployeeID,      
EmpFirstName=E.FirstName, EmpLastName=E.LastName,      
WeeklyAllocatedHours=SUM(DATEDIFF(HOUR, ETD.EmployeeTSStartTime, ETD.EmployeeTSEndTime)) OVER(PARTITION BY ETD.EmployeeID),      
WeeklyUsedHours=SUM(DATEDIFF(HOUR, SM.StartDate, SM.EndDate)) OVER(PARTITION BY ETD.EmployeeID)      
      
FROM EmployeeTimeSlotDates ETD      
INNER JOIN Employees E ON E.EmployeeID= ETD.EmployeeID      
LEFT JOIN ScheduleMasters SM ON SM.EmployeeTSDateID= ETD.EmployeeTSDateID AND SM.IsDeleted=0       
      
WHERE 1=1       
--AND (((@StartDate is null OR ETD.EmployeeTSDate >= @StartDate) and (@EndDate is null OR ETD.EmployeeTSDate <= @EndDate))      
--    OR(@StartDate is null OR CONVERT(DATE,ETD.EmployeeTSDate)=@StartDate) OR(@EndDate is null OR CONVERT(DATE,ETD.EmployeeTSDate)=@EndDate))      
AND ((@StartDate IS NOT NULL AND @EndDate IS NULL AND ETD.EmployeeTSDate >= @StartDate) OR     
(@EndDate IS NOT NULL AND @StartDate IS NULL AND ETD.EmployeeTSDate <= @EndDate) OR    
(@StartDate IS NOT NULL AND @EndDate IS NOT NULL AND (ETD.EmployeeTSDate >= @StartDate AND ETD.EmployeeTSDate <= @EndDate)) OR    
(@StartDate IS NULL AND @EndDate IS NULL)    
)  
) AS TEMP WHERE @ExceededHrs < TEMP.WeeklyUsedHours      
      
      
      
) AS T      
      
)  AS T1 )          
          
SELECT * FROM CTEGetEmpClockInOutList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)                       
END
