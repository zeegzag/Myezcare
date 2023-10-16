
-- EXEC HC_GetEmployeeBillingReportList 'sunil 5','2018-09-10','2018-09-16','EmployeeName','ASC','1','100'        
CREATE PROCEDURE [dbo].[HC_GetEmployeeBillingReportList]
 @EmployeeName NVARCHAR(200) = NULL,         
 @StartDate DATE = NULL,                                                      
 @EndDate DATE = NULL,                            
 @SortExpression NVARCHAR(100),                                                        
 @SortType NVARCHAR(10),                                                      
 @FromIndex INT,                                                      
 @PageSize INT                                                       
AS                                                      
BEGIN          
    
DECLARE @Temp_CalculateAllocatedhour TABLE (EmpId bigint,AllocatedHour bigint)      
      
INSERT INTO @Temp_CalculateAllocatedhour      
SELECT       
SM.EmployeeID,SUM(DATEDIFF(minute,SM.StartDate,SM.EndDate)) AS AllocatedHour      
FROM ScheduleMasters SM      
WHERE SM.IsDeleted=0     
AND ((@StartDate IS NULL OR LEN(@StartDate)=0) OR CONVERT(DATE,SM.StartDate) >= @StartDate)    
AND ((@EndDate IS NULL OR LEN(@EndDate)=0) OR CONVERT(DATE,SM.EndDate) <= @EndDate)    
GROUP BY SM.EmployeeID      
      
--select * from @Temp_CalculateAllocatedhour      
      
DECLARE @Temp_CalculatePTOhour TABLE (EmpId bigint,PTOHour bigint)      
INSERT INTO @Temp_CalculatePTOhour      
SELECT EDO.EmployeeID, SUM(DATEDIFF(minute,EDO.StartTime,EDO.EndTime)) AS PTOHour      
FROM EmployeeDayOffs EDO    
WHERE EDO.IsDeleted=0 AND EDO.DayOffStatus='Approved'     
AND ((@StartDate IS NULL OR LEN(@StartDate)=0) OR CONVERT(DATE,EDO.StartTime) >= @StartDate)    
AND ((@EndDate IS NULL OR LEN(@EndDate)=0) OR CONVERT(DATE,EDO.EndTime) <= @EndDate)    
GROUP BY EmployeeID      
      
--select * from @Temp_CalculatePTOhour      
      
DECLARE @Temp_CalculateWorkingHour TABLE (EmpId bigint,WorkingHour bigint)      
INSERT INTO @Temp_CalculateWorkingHour      
SELECT E.EmployeeID,    
--SUM(DATEDIFF(minute,SM.StartDate,SM.EndDate)) AS WorkingHour     
SUM(EVN.ServiceTime) AS WorkingHour      
FROM        
ScheduleMasters SM        
INNER JOIN EmployeeVisits EV ON EV.ScheduleID = SM.ScheduleID AND EV.IsDeleted=0 AND ((IsApprovalRequired=1 AND ActionTaken=2) OR (IsApprovalRequired=0) OR (IsApprovalRequired IS NULL))
INNER JOIN EmployeeVisitNotes EVN ON EVN.EmployeeVisitID = EV.EmployeeVisitID AND EVN.IsDeleted=0     
INNER JOIN Employees E ON E.EmployeeID = SM.EmployeeID AND E.IsDeleted=0       
WHERE SM.IsDeleted=0      
AND ((@StartDate IS NULL OR LEN(@StartDate)=0) OR CONVERT(DATE,SM.StartDate) >= @StartDate)    
AND ((@EndDate IS NULL OR LEN(@EndDate)=0) OR CONVERT(DATE,SM.EndDate) <= @EndDate)      
GROUP BY E.EmployeeID      
      
--select * from @Temp_CalculateWorkingHour      
      
                                                      
 ;WITH CTEEmployeeBillingReportList AS                                                      
 (                                                       
  SELECT *,COUNT(T1.EmployeeID) OVER() AS Count FROM                                                       
  (                                                      
   SELECT ROW_NUMBER() OVER (ORDER BY          
                                                        
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EmployeeName' THEN EmployeeName END END ASC,                           
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EmployeeName' THEN EmployeeName END END DESC,                                                      
        
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AllocatedHour' THEN AllocatedHour END END ASC,                      
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AllocatedHour' THEN AllocatedHour END END DESC,        
      
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PTOHour' THEN PTOHour END END ASC,                      
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PTOHour' THEN PTOHour END END DESC,        
      
   CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'WorkingHour' THEN WorkingHour END END ASC,                      
   CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'WorkingHour' THEN WorkingHour END END DESC        
  ) AS Row,                         
  * FROM        
   (      
 select E.EmployeeID,dbo.GetGeneralNameFormat(E.FirstName,E.LastName) AS EmployeeName,TAH.AllocatedHour,TPH.PTOHour,TWH.WorkingHour ,    
 AllocatedHourInStr =CONCAT(TAH.AllocatedHour/60,'hrs ',TAH.AllocatedHour%60,'min'),    
 PTOHourInStr =CONCAT(TPH.PTOHour/60,'hrs ',TPH.PTOHour%60,'min'),    
 WorkingHourInStr =CONCAT(TWH.WorkingHour/60,'hrs ',TWH.WorkingHour%60,'min')    
 from Employees E      
 Left JOIN @Temp_CalculateAllocatedhour TAH on TAH.EmpId=E.EmployeeID      
 Left JOIN @Temp_CalculatePTOhour TPH on TPH.EmpId=E.EmployeeID      
 Left JOIN @Temp_CalculateWorkingHour TWH on TWH.EmpId=E.EmployeeID      
 WHERE (AllocatedHour IS NOT NULL OR PTOHour IS NOT NULL)      
 AND ((@EmployeeName IS NULL OR LEN(@EmployeeName)=0) OR  (                    
 E.FirstName LIKE '%'+@EmployeeName+'%' OR E.LastName  LIKE '%'+@EmployeeName+'%' OR                    
    E.FirstName +' '+ E.LastName like '%'+@EmployeeName+'%' OR                    
    E.LastName +' '+  E.FirstName like '%'+@EmployeeName+'%' OR                    
    E.FirstName +', '+E.LastName like '%'+@EmployeeName+'%' OR                    
    E.LastName +', '+ E.FirstName like '%'+@EmployeeName+'%'))           
 )AS T          
 )AS T1      
 )                  
 SELECT * FROM CTEEmployeeBillingReportList WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)       
 END
