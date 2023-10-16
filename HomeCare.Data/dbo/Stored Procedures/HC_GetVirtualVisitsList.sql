     
CREATE PROCEDURE [dbo].[HC_GetVirtualVisitsList]  
  @ServerDateTime datetime,  
  @VisitType nvarchar(max) = NULL,  
  @EmployeeID bigint = NULL,  
  @ReferralName nvarchar(max) = NULL,  
  @StartDate datetime = NULL,  
  @EndDate datetime2 = NULL,  
  @AllRecordAccess bit = NULL,  
  @LoggedInID bigint = NULL,  
  @SortExpression nvarchar(100) = '',  
  @SortType nvarchar(10) = 'ASC',  
  @FromIndex int = '1',  
  @PageSize int = '50'  
AS  
BEGIN  
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
  DECLARE @VisitStartDate datetime = NULL,  
          @VisitEndDate datetime2 = NULL,  
          @ServerDate datetime2 = CONVERT(date, @ServerDateTime)  
  
  IF (@VisitType = 'todaysVisits')  
  BEGIN  
    SET @VisitStartDate = @ServerDate  
    SET @VisitEndDate = DATEADD(ns, -100, DATEADD(DAY, 1, @ServerDate))  
  END  
  ELSE  
  IF (@VisitType = 'futureVisits')  
  BEGIN  
    SET @VisitStartDate = DATEADD(DAY, 1, @ServerDate)  
  END  
  ELSE  
  IF (@VisitType = 'pastVisits')  
  BEGIN  
    SET @VisitEndDate = DATEADD(ns, -100, @ServerDate)  
  END  
  
  
  IF (@EndDate IS NOT NULL)  
  BEGIN  
    SET @EndDate = DATEADD(ns, -100, DATEADD(DAY, 1, @EndDate))  
  END  
  
  
  ;  
  WITH CTEList  
  AS  
  (  
    SELECT  
      *,  
      COUNT(T1.ReferralID) OVER () AS [Count]  
    FROM  
    (  
      SELECT  
        *,  
        ROW_NUMBER() OVER (ORDER BY  
        CASE  
          WHEN @SortType = 'ASC' THEN CASE  
              WHEN @SortExpression = 'EmployeeName' THEN EmpName  
              WHEN @SortExpression = 'ReferralName' THEN RefName  
              WHEN @SortExpression = 'StartDate' THEN StartDateSort  
              WHEN @SortExpression = 'EndDate' THEN EndDateSort  
              WHEN @SortExpression = 'Payor' THEN Payor  
              WHEN @SortExpression = 'CareType' THEN CareType  
              WHEN @SortExpression = 'ScheduleStatusName' THEN ScheduleStatusName  
            END  
        END ASC,  
        CASE  
          WHEN @SortType = 'DESC' THEN CASE  
              WHEN @SortExpression = 'EmployeeName' THEN EmpName  
              WHEN @SortExpression = 'ReferralName' THEN RefName  
              WHEN @SortExpression = 'StartDate' THEN StartDateSort  
              WHEN @SortExpression = 'EndDate' THEN EndDateSort  
              WHEN @SortExpression = 'Payor' THEN Payor  
              WHEN @SortExpression = 'CareType' THEN CareType  
              WHEN @SortExpression = 'ScheduleStatusName' THEN ScheduleStatusName  
            END  
        END DESC  
        ) AS [Row]  
      FROM  
      (  
  
  
        SELECT DISTINCT  
          R.ReferralID,  
          R.FirstName,  
          R.LastName,  
		  dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) AS ReferralName,
          C.Phone1,  
          C.Phone2,  
          C.Email,  
          SS.ScheduleStatusName,  
          RTD.ReferralTSDateID,  
          RTD.UsedInScheduling,  
          RTD.OnHold,  
          ScheduleComment = RTD.Notes,  
          SM.ScheduleID,  
          SM.ScheduleStatusID,  
          RTD.IsDenied,  
          IsPendingSchProcessed =  
                                 CASE  
                                   WHEN SM.ReferralTSDateID IS NULL AND  
                                     SM.EmployeeTSDateID IS NULL THEN 1  
                                   ELSE 0  
                                 END,  
          StartDate =  
                     CASE  
                       WHEN SM.StartDate IS NOT NULL THEN SM.StartDate  
                       ELSE RTD.ReferralTSStartTime  
                     END,  
          EndDate =  
                   CASE  
                     WHEN SM.StartDate IS NOT NULL THEN SM.EndDate  
                     ELSE RTD.ReferralTSEndTime  
                   END,  
          EmpFirstName = E.FirstName,  
          EmpLastName = E.LastName,  
		  dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS EmployeeName,
          EmpEmail = E.Email,  
          EmpMobile = E.MobileNumber,  
          E.EmployeeUniqueID,  
          E.EmployeeID,  
          CONVERT(varchar, EV.ClockInTime) AS strClockInTime,  
          CONVERT(varchar, EV.ClockOutTime) AS strClockOutTime,  
          EV.IVRClockIn,  
          EV.IVRClockOut,  
          EV.IsPCACompleted,  
          (  
            SELECT  
              ShortName  
            FROM Payors  
            WHERE  
              PayorID = SM.PayorID  
          )  
          AS Payor,  
          (  
            SELECT  
              Title  
            FROM DDMaster  
            WHERE  
              DDMasterID = SM.CareTypeId  
          )  
          AS CareType,  
          RTSD.CareTypeId,  
          EV.IsApprovalRequired,  
          SM.IsVirtualVisit,  
          IsBetween =  
                     CASE  
                       WHEN GETDATE() BETWEEN SM.StartDate AND SM.EndDate THEN 1  
                       ELSE 0  
                     END,  
          IsPastVisit =  
                       CASE  
                         WHEN SM.EndDate <= GETDATE() THEN 1  
                         ELSE 0  
                       END,  
          AD.*  
        FROM Referrals R  
        INNER JOIN ReferralTimeSlotDates RTD  
          ON RTD.ReferralID = R.ReferralID  
  
        LEFT JOIN ScheduleMasters SM  
          ON ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted = 0  
          AND ((SM.ReferralTSDateID = RTD.ReferralTSDateID)  
          OR (RTD.ReferralTSDate BETWEEN CONVERT(date, SM.StartDate) AND CONVERT(date, SM.EndDate)  
          AND SM.ReferralTSDateID IS NULL  
          AND SM.EmployeeTSDateID IS NULL))  
  
        LEFT JOIN ContactMappings CM  
          ON CM.ReferralID = R.ReferralID  
          AND CM.ContactTypeID = 1  
        LEFT JOIN Contacts C  
          ON C.ContactID = CM.ContactID  
        LEFT JOIN ScheduleStatuses SS  
          ON SS.ScheduleStatusID = SM.ScheduleStatusID  
        LEFT JOIN Employees E  
          ON E.EmployeeID = SM.EmployeeID  
        LEFT JOIN EmployeeVisits EV  
          ON EV.ScheduleID = SM.ScheduleID  
        LEFT JOIN EmployeeVisitNotes EN  
          ON EN.EmployeeVisitID = EV.EmployeeVisitID  
        LEFT JOIN ReferralTimeSlotDetails RTSD  
          ON RTSD.ReferralTimeSlotDetailID = RTD.ReferralTimeSlotDetailID  
        CROSS APPLY  
        (  
          SELECT  
            ISNULL(R.LastName, '') + CASE  
              WHEN LEN(ISNULL(R.LastName, '')) > 0 THEN ', '  
              ELSE ''  
            END + ISNULL(R.FirstName, '') [RefName],  
            ISNULL(E.LastName, '') + CASE  
              WHEN LEN(ISNULL(E.LastName, '')) > 0 THEN ', '  
              ELSE ''  
            END + ISNULL(E.FirstName, '') [EmpName],  
            CONVERT(nvarchar(max), StartDate, 120) StartDateSort,  
            CONVERT(nvarchar(max), EndDate, 120) EndDateSort  
        ) AD  
        WHERE  
          1 = 1  
          AND (SM.ScheduleID IS NULL  
            OR (SM.ScheduleID IS NOT NULL  
              AND SM.EmployeeID IS NOT NULL))  
          AND SM.IsVirtualVisit = 1  
          AND (ISNULL(@EmployeeID, 0) = 0  
            OR ISNULL(SM.EmployeeID, 0) = 0  
            OR SM.EmployeeID = @EmployeeID)  
          AND (ISNULL(@AllRecordAccess, 0) = 1  
            OR ISNULL(@LoggedInID, 0) = 0  
            OR ISNULL(SM.EmployeeID, 0) = 0  
            OR SM.EmployeeID = @LoggedInID)  
          AND (@ReferralName IS NULL  
            OR LEN(@ReferralName) = 0  
            OR AD.[RefName] LIKE '%' + @ReferralName + '%')  
          AND (  
          (@StartDate IS NULL  
              OR @StartDate <= StartDate)  
            AND (@EndDate IS NULL  
              OR @EndDate >= StartDate)  
          )  
          AND (  
          (@VisitStartDate IS NULL  
              OR @VisitStartDate <= StartDate)  
            AND (@VisitEndDate IS NULL  
              OR @VisitEndDate >= StartDate)  
          )  
          AND RTSD.IsDeleted = 0  
  
      ) AS T2  
    ) AS T1  
  )  
  
  SELECT  
    *  
  FROM CTEList  
  WHERE  
    [Row] BETWEEN ((@PageSize * (@FromIndex - 1)) + 1) AND (@PageSize * @FromIndex)  
END  