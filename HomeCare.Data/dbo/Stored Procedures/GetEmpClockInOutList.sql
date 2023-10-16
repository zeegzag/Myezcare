--EXEC GetEmpClockInOutList '2022/02/15', '2022/02/16', '', 'ScheduleStartTime','ASC','1','10','','Inprogress',''                      
CREATE procedure [dbo].[GetEmpClockInOutList]        
--DECLARE        
@StartDate DATE =NULL,                                                            
@EndDate  DATE=NULL,                                                    
@EmployeeName VARCHAR(200)= Null,                                                    
@SortExpression NVARCHAR(100),                                                                              
@SortType NVARCHAR(10),                                                                            
@FromIndex INT,                                                                            
@PageSize INT,                                
@CareTypeID NVARCHAR(500) = NULL,                                
@Status varchar(50) = NULL   ,                    
@RegionID Nvarchar(50) =NULL,                    
@TimeSlots Nvarchar(500) =NULL                      
 AS                     
--select  @StartDate = '2023-04-08', @EndDate = '2023-04-08', @EmployeeName = '', @CareTypeID = '', @Status = '', @TimeSlots = '', @RegionID = '', @SortExpression = 'ScheduleStartTime', @SortType = 'DESC', @FromIndex = '1', @PageSize = '1000'             
  
  
                                                           
BEGIN      
  
 --IF (@TimeSlots = '' OR @TimeSlots IS NULL OR @TimeSlots LIKE '%Day%' OR @TimeSlots LIKE '%Night%')        
 -- SET @EndDate = NULL 
 
 DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
         
    if (@TimeSlots = 'Day') -- For 24Hrs Filter                      
    begin                    
        set @TimeSlots = ''         
    end                
         
                        
    ; WITH CTEGetEmpClockInOutList1                    
        AS (SELECT *,                    
                    COUNT(T1.ReferralID) OVER () AS Count                    
            FROM (   SELECT ROW_NUMBER() OVER (ORDER BY CASE                    
                                                                WHEN @SortType = 'ASC' THEN                    
                                                                    CASE                    
                                                                        WHEN @SortExpression = 'Employee' THEN                    
                                                                            t.EmpFirstName END END ASC,                    
                                                        CASE                    
                                                                WHEN @SortType = 'DESC' THEN                    
                                                                    CASE                    
                                                                        WHEN @SortExpression = 'Employee' THEN                    
                                                                            t.EmpFirstName END END DESC,                    
                                                        CASE                    
                                                                WHEN @SortType = 'ASC' THEN                    
                                                                    CASE                    
                                                                        WHEN @SortExpression = 'Patient' THEN                    
                                                                            CONVERT(varchar(50), t.LastName) END END ASC,                    
                                                        CASE                    
                                                                WHEN @SortType = 'DESC' THEN                    
                                                                    CASE                    
                                                                        WHEN @SortExpression = 'Patient' THEN                    
                                                                            CONVERT(varchar(50), t.LastName) END END DESC,                    
                                   CASE                    
                                         WHEN @SortType = 'ASC' THEN                    
                                        CASE                    
                                                                        WHEN @SortExpression = 'ScheduleStartTime' THEN                    
                                                                            CONVERT(date, t.ScheduleStartTime, 105) END END ASC,                    
                                            CASE                    
                                                      WHEN @SortType = 'DESC' THEN                    
                                                  CASE                    
                                                                        WHEN @SortExpression = 'ScheduleStartTime' THEN                    
                                                                   CONVERT(date, t.ScheduleStartTime, 105) END END DESC,                    
                                                        CASE                    
                                                                WHEN @SortType = 'ASC' THEN                    
                                                                    CASE                    
                                                                        WHEN @SortExpression = 'ScheduleEndTime' THEN                    
                                                                            CONVERT(date, t.ScheduleEndTime, 105) END END ASC,                    
                                                        CASE                    
                                                                WHEN @SortType = 'DESC' THEN                    
                                                                    CASE                    
                                                                        WHEN @SortExpression = 'ScheduleEndTime' THEN                    
                                                                            CONVERT(date, t.ScheduleEndTime, 105) END END DESC,                    
                                                        CASE                    
                                                                WHEN @SortType = 'ASC' THEN                    
                                                                    CASE                    
                                                                        WHEN @SortExpression = 'ClockIn' THEN                    
                                                                            CAST(t.ClockIn AS BIT)END END ASC,                    
                                                        CASE                    
                                                                WHEN @SortType = 'DESC' THEN                    
                                                                    CASE                    
                                                                        WHEN @SortExpression = 'ClockIn' THEN                    
                                                                            CAST(t.ClockIn AS BIT)END END DESC,                    
                                                        CASE                    
                                                                WHEN @SortType = 'ASC' THEN                    
                                                                    CASE                    
                                                                        WHEN @SortExpression = 'ClockOut' THEN                    
                                                                            CAST(t.ClockOut AS BIT)END END ASC,                    
                                                        CASE                    
                                                                WHEN @SortType = 'DESC' THEN                    
                                                                    CASE                    
                                                                 WHEN @SortExpression = 'ClockOut' THEN                    
                                                                CAST(t.ClockOut AS BIT)END END DESC,                    
            CASE                    
                                                                WHEN @SortType = 'ASC' THEN                    
                                                                    CASE                    
                                             WHEN @SortExpression = 'CareType' THEN                    
                                                                            t.CareType END END ASC,                    
                                                        CASE                    
                                                                WHEN @SortType = 'DESC' THEN                    
                                                                 CASE                    
                                                             WHEN @SortExpression = 'CareType' THEN                    
                                                                            t.CareType END END ASC,                    
                                                        CASE                    
                                                                WHEN @SortType = 'ASC' THEN                    
CASE                    
                                                                        WHEN @SortExpression = 'Phone' THEN                    
                                                                            t.MobileNumber END END ASC,                    
                                                        CASE                    
                                                                WHEN @SortType = 'DESC' THEN                    
                                                                    CASE                    
                                                                        WHEN @SortExpression = 'Phone' THEN                    
                                                                            t.MobileNumber END END DESC                    
                            -- ,t.Timing                                                       
                            ) AS ROW,                    
                            t.*                    
                        FROM (                    
       SELECT AA.RegionID,aa.ScheduleID,                    
                                        aa.EmployeeVisitid,                    
                                        aa.ReferralID,                    
                                        aa.EmployeeID,                    
                                        aa.EmpFirstName,                    
                                        aa.EmpLastName,   
							 		   Employee = dbo.GetGenericNameFormat(aa.EmpFirstName,aa.EmpMiddleName, aa.EmpLastName,@NameFormat),  
                                        aa.FirstName,                    
                                        aa.LastName,  
									    Patient = dbo.GetGenericNameFormat(aa.FirstName,aa.MiddleName, aa.LastName,@NameFormat),  
                                        aa.CareType,                    
                                        aa.ScheduleStartTime,                    
                                        aa.ScheduleEndTime,                    
                                        aa.ClockIn,                    
                                        aa.ClockOut,                    
                                        aa.IsPCACompleted,                    
                                        aa.[Address],                    
                                        aa.lat,                    
                                        aa.lng,                    
                                aa.Phone,                    
                                        aa.StartTime,                    
                                        aa.EndTime,                    
                                        aa.MobileNumber,                    
                                        [Status],                    
                    Timing,    
          aa.ReferralName,    
          aa.StartDate,    
          aa.EndDate    
                                    from (                       
          SELECT R.RegionID,SM.ScheduleID,  ReferralName= dbo.GetGenericNameFormat(R.FirstName,R.MiddleName, R.LastName,@NameFormat),   
     SM.StartDate,    
               SM.EndDate,    
                                                    EV.EmployeeVisitid,                    
                                                    R.ReferralID,                    
                                                    E.EmployeeID,                    
             EmpFirstName = E.FirstName,                    
                                                    EmpLastName = E.LastName,  
             EmpMiddleName = E.MiddleName,  
                                                    R.FirstName,                    
                                                    R.LastName,   
             R.MiddleName,  
             dm.Title As CareType,                    
                                                    ScheduleStartTime = SM.StartDate,                    
                                                    ScheduleEndTime = SM.EndDate,                    
                    ClockIn = CASE                    
                                                                    WHEN EV.ClockInTime IS NULL THEN 0                    
                                                                    ELSE 1 END,                    
                                                    ClockOut = CASE                    
                                                                    WHEN EV.ClockOutTime IS NULL THEN 0                    
                                                                    ELSE 1 END,                    
                                                    ISNULL(EV.IsPCACompleted, 0) IsPCACompleted,                    
                                                    CONCAT(c.Address, c.ApartmentNo, c.City, c.ZipCode) As Address,                    
                                                    c.Latitude as lat,                    
                                                    c.Longitude as lng,                    
                                                    c.Phone1 as Phone,                    
                                                    FORMAT(sm.StartDate, 'hh:mm tt') as StartTime,                    
                        FORMAT(sm.EndDate, 'hh:mm tt') as EndTime,                    
                                                    E.MobileNumber,                    
             AD.*                    
                                                FROM ScheduleMasters SM                    
                                            INNER JOIN Referrals R                    
                                                ON R.ReferralID          = SM.ReferralID                    
                                            INNER JOIN Employees E                    
                                                ON E.EmployeeID          = SM.EmployeeID                    
                                            Left JOIN EmployeeVisits EV                    
                                                ON EV.ScheduleID         = SM.ScheduleID                    
                                                AND EV.IsDeleted          = 0                    
                                            LEFT join ContactMappings cm                    
                                                on cm.referralid         = R.referralid                    
                                         AND cm.ContactTypeID = 1                    
                                            LEFT join Contacts c                    
                                                on c.ContactID           = cm.ContactID                    
                                                AND c.IsDeleted           = 0                    
                                            INNER JOIN DDMaster dm        
                                                on dm.DDMasterID         = sm.CareTypeId                    
           LEFT JOIN Regions RG                 
            ON RG.RegionID=R.RegionID                    
           CROSS APPLY (                    
            SELECT                    
             case                    
                                                        when EV.ClockInTime IS NOT NULL                    
            AND EV.ClockOutTime IS NULL then 'Inprogress'                    
                                                        when EV.EmployeeVisitid IS NULL                    
                                                        AND SM.ScheduleID IS NOT NULL then 'Missed'         
                                                        when EV.IsPCACompleted = 1 then 'Complete'                    
                                                        ELSE 'OnHold' end as [Status],                    
                                                    (case                    
                                                        when convert(char(10), SM.startdate, 108) >= '00:00:00'           
                                                            and convert(char(10), SM.startdate, 108) < '12:00:00' then                    
                                                            'Morning'                    
                                                        when convert(char(10), SM.startdate, 108) >= '12:00:01'                    
                                        and convert(char(10), SM.startdate, 108) < '17:00:00' then                    
                                                            'Afternoon'                    
                                                        when convert(char(10), SM.startdate, 108) >= '17:00:01'                    
                                                            and convert(char(10), SM.startdate, 108) < '18:59:00' then                    
                                                            'Evening'                    
                                                        else 'Night' end) as Timing                    
           ) AD                    
                                            WHERE ISNULL(SM.OnHold, 0)                    = 0                    
                                                AND SM.IsDeleted                            = 0                    
                             
            AND ((@RegionID is null or @RegionID='' ) or (R.RegionID IN (SELECT VAL FROM GetCSVTable(@RegionID))))                    
                                                AND R.ReferralStatusID                      = 1                    
                                                AND (   (   @CareTypeID IS NULL                    
                                                        OR   LEN(@CareTypeID)                = 0)                    
                                                    OR   (dm.DDMasterID in ( SELECT CONVERT(BIGINT, VAL) FROM GetCSVTable(                    
                                               @CareTypeID) )))                    
                                                AND ((@StartDate IS NOT NULL AND @EndDate IS NULL AND CONVERT(DATE, SM.StartDate) = @StartDate)                  
            OR  (@EndDate IS NOT NULL AND  @StartDate IS NULL AND   CONVERT(DATE, SM.EndDate) = @EndDate)                  
            OR  (@StartDate IS NOT NULL AND @EndDate IS NOT NULL  AND  ((CONVERT(DATE, SM.StartDate) >= @StartDate  AND  CONVERT(DATE, SM.EndDate)   <= @EndDate) OR CONVERT(DATE, SM.StartDate) = @EndDate))                  
            OR  (@StartDate IS NULL AND   @EndDate IS NULL))                    
                                                --AND (ISNULL(@TimeSlots, '') = '' OR AD.Timing = @TimeSlots)        
            AND ((@TimeSlots is null or @TimeSlots='' ) or (AD.Timing IN (SELECT VAL FROM GetCSVTable(@TimeSlots))))        
                                                AND (   (   @EmployeeName IS NULL                    
                                                        OR   LEN(e.LastName)                 = 0)                    
                                                    OR   ( (E.FirstName LIKE '%' + @EmployeeName + '%')                    
                                                        OR   (E.LastName LIKE '%' + @EmployeeName + '%')                    
                                                        OR   (E.FirstName + ' ' + E.LastName like '%' + @EmployeeName            
                                                                                                + '%')                    
                                                        OR   (E.LastName + ' ' + E.FirstName like '%' + @EmployeeName                    
                                                                 + '%')                    
                                                        OR   (E.FirstName + ', ' + E.LastName like '%' + @EmployeeName                    
                                                                                                + '%')                    
                                                        OR   (E.LastName + ', ' + E.FirstName like '%' + @EmployeeName                    
                                                                                                + '%')))                    
          ) aa                    
                                    WHERE (ISNULL(@Status, '') = '' OR [Status] = @Status)                    
         ) AS T                     
    ) AS T1                     
 )                    
    SELECT *                    
        FROM CTEGetEmpClockInOutList1                    
        outer apply (   select count(ap.EmployeeID) as TotalSchedule                    
                      from CTEGetEmpClockInOutList1 ap) as TotalSchedule                    
        outer apply (   select count(ap.EmployeeID) as Inprogress                    
                        from CTEGetEmpClockInOutList1 ap                    
                        where ap.ClockIn  = 1                    
                        and ap.ClockOut = 0) as Inprogress                    
        outer apply (   select count(ap.EmployeeID) as ClocOutnDone                    
                        from CTEGetEmpClockInOutList1 ap                    
                   where ap.ClockIn  = 1                    
                        and ap.ClockOut = 1) as ClocOutnDone                    
        outer apply (   select count(ap.EmployeeID) as MissedSchedule                    
                        from CTEGetEmpClockInOutList1 ap                    
                        where ap.EmployeeVisitid IS NULL                    
                        AND ap.ScheduleID IS NOT NULL) as MissedSchedule                    
        outer apply (   select count(ap.EmployeeID) as TotalComplete                    
                        from CTEGetEmpClockInOutList1 ap                    
                        where ap.IsPCACompleted = 1) as TotalComplete                    
        WHERE ROW BETWEEN ((@PageSize * (@FromIndex - 1)) + 1) AND (@PageSize * @FromIndex)                    
                        
END 