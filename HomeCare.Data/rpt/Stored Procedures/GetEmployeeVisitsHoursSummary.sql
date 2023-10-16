-- Note: Please also do required changes in EmpHours.sql  
-- Base function is available there for getting EmpHours, we have replicated it here due to additional filters and grouping  
--  [RPT].[GetEmployeeVisitsHoursSummary] NULL,NULL, '0',0,0  
CREATE PROCEDURE [rpt].[GetEmployeeVisitsHoursSummary]  
  @FromDate DATE = NULL,  
  @ToDate DATE = NULL,  
  @EmployeeID VARCHAR(4000) = '0',  
  @CareType VARCHAR(4000) = '0',  
  @Payor VARCHAR(4000) = '0'  
AS  
BEGIN  
  DECLARE @WeekStartDay INT = [dbo].[GetWeekStartDay]();  
  
  WITH FilteredScheduleMasters  
  AS (  
    SELECT SM.*  
    FROM dbo.ScheduleMasters SM  
    WHERE ISNULL(SM.OnHold, 0) = 0 
      AND (  
        ISNULL(@CareType, '0') = '0'  
        OR SM.CareTypeId IN (  
          SELECT CTL.[val]  
          FROM dbo.GetCSVTable(@CareType) CTL  
          )  
        )  
      AND (  
        ISNULL(@Payor, '0') = '0'  
        OR SM.PayorID IN (  
          SELECT PL.[val]  
          FROM dbo.GetCSVTable(@Payor) PL  
          )  
        )  
    ),  
  EmpHours  
  AS (  
    SELECT E.EmployeeID,  
      dbo.GetGeneralNameFormat(E.FirstName, E.LastName) EmployeeName,  
      ET.DWM,  
      DR.IndividualDate,  
      CASE ROW_NUMBER() OVER (  
          PARTITION BY E.EmployeeID,  
          ET.DWM ORDER BY DR.IndividualDate  
          )  
        WHEN 1  
          THEN CAST(CASE   
                WHEN ISNULL(E.RegularHours, 0) = 0  
                  THEN 8  
                ELSE ISNULL(E.RegularHours, 0)  
                END * 60 AS INT)  
        ELSE 0  
        END RegularMinutes,  
      --EDT.EmployeeTSDateIDs,  
      --EDT.AllocatedMinutes,  
      S.ScheduleIDs,  
      S.ScheduledMinutes,  
      S.Distance,  
      V.VisitCount,  
      V.VisitIDs,  
      V.VisitedMinutes,  
      PTO.PTOIDs,  
      PTO.PTOMinutes,  
      PTO.PersonalPTOMinutes,  
      PTO.SickPTOMinutes,  
      PTO.HolidayPTOMinutes  
    FROM dbo.Employees E  
    CROSS JOIN DateRange(@FromDate, @ToDate) DR  
    CROSS APPLY (  
      SELECT DATEDIFF(DAY, '1900-01-01', DR.IndividualDate) + 1 DayNum,  
        DATEDIFF(WEEK, '1900-01-01', DATEADD(D, - 1 * @WeekStartDay, DR.IndividualDate)  
        ) + 1 WeekNum,  
        DATEDIFF(MONTH, '1900-01-01', DR.IndividualDate) + 1 MonthNum,  
        CONVERT(DATETIME2, DR.IndividualDate) IndividualStartDateTime,  
        DATEADD(D, 1, CONVERT(DATETIME2, DR.IndividualDate)) IndividualEndDateTime  
      ) DT  
    CROSS APPLY (  
      SELECT CASE ISNULL(E.RegularHourType, 1)  
          WHEN 1  
            THEN 'D' + CONVERT(VARCHAR(max), DT.DayNum)  
          WHEN 2  
            THEN 'W' + CONVERT(VARCHAR(max), DT.WeekNum)  
          WHEN 3  
            THEN 'M' + CONVERT(VARCHAR(max), DT.MonthNum)  
          END DWM  
      ) ET  
    --OUTER APPLY (  
    --  SELECT STRING_AGG(ETSD.EmployeeTSDateID, ',') EmployeeTSDateIDs,  
    --    SUM(ISNULL(CASE   
    --          WHEN ETSD.EmployeeTSStartTime < ETSD.EmployeeTSEndTime  
    --            THEN DATEDIFF(MINUTE, ETSD.EmployeeTSStartTime, ETSD.  
    --                EmployeeTSEndTime)  
    --          ELSE 1440 + DATEDIFF(MINUTE, ETSD.EmployeeTSStartTime, ETSD.  
    --              EmployeeTSEndTime)  
    --          END, 0)) AllocatedMinutes  
    --  FROM dbo.EmployeeTimeSlotDates ETSD  
    --  WHERE ETSD.EmployeeID = E.EmployeeID  
    --    AND ETSD.EmployeeTSDate = DR.IndividualDate  
    --  ) EDT  
    OUTER APPLY (  
      SELECT STRING_AGG(SM.ScheduleID, ',') ScheduleIDs,  
        SUM(ISNULL(CASE   
              WHEN SM.StartDate < SM.EndDate  
                THEN DATEDIFF(MINUTE, SM.StartDate, SM.EndDate)  
              ELSE 1440 + DATEDIFF(MINUTE, SM.StartDate, SM.EndDate)  
              END, 0)) ScheduledMinutes,  
        SUM(D.Distance) Distance  
      FROM FilteredScheduleMasters SM  
      LEFT JOIN Referrals R  
        ON R.ReferralID = SM.ReferralID  
      LEFT JOIN ContactMappings CM  
        ON CM.ReferralID = R.ReferralID  
          AND CM.ContactTypeID = 1  
      LEFT JOIN Contacts C  
        ON C.ContactID = CM.ContactID  
      OUTER APPLY (  
        SELECT (  
            ([dbo].GetGeoFromLatLng(E.Latitude, E.Longitude)).  
            STDistance([dbo].GetGeoFromLatLng(C.Latitude, C.Longitude)) *   
            0.000621371  
            ) Distance  
        ) D  
      WHERE SM.EmployeeID = E.EmployeeID 
	    AND SM.IsDeleted = 0 
        AND ISNULL(SM.AnyTimeClockIn, 0) = 0  
        AND DR.IndividualDate BETWEEN CONVERT(DATE, SM.StartDate)  
          AND CONVERT(DATE, SM.EndDate)  
      ) S  
    OUTER APPLY (  
      SELECT COUNT(EV.EmployeeVisitID) VisitCount,  
        STRING_AGG(EV.EmployeeVisitID, ',') VisitIDs,  
        SUM(ISNULL(CASE   
              WHEN EV.ClockInTime < EV.ClockOutTime  
                THEN DATEDIFF(MINUTE, EV.ClockInTime, EV.ClockOutTime)  
              ELSE 1440 + DATEDIFF(MINUTE, EV.ClockInTime, EV.ClockOutTime)  
              END, 0)) VisitedMinutes  
      FROM FilteredScheduleMasters SM  
      INNER JOIN dbo.EmployeeVisits EV  
        ON EV.ScheduleID = SM.ScheduleID  
          AND EV.IsPCACompleted = 1  
      WHERE SM.EmployeeID = E.EmployeeID  
        AND DR.IndividualDate BETWEEN CONVERT(DATE, SM.StartDate)  
          AND CONVERT(DATE, SM.EndDate)  
      ) V  
    OUTER APPLY (  
      SELECT STRING_AGG(EDO.EmployeeDayOffID, ',') PTOIDs,  
        SUM(DATEDIFF(MINUTE, DO.StartTime, DO.EndTime)) PTOMinutes,  
        SUM(CASE   
            WHEN EDO.DayOffTypeID = 1  
              THEN DATEDIFF(MINUTE, DO.StartTime, DO.EndTime)  
            ELSE 0  
            END) PersonalPTOMinutes,  
        SUM(CASE   
            WHEN EDO.DayOffTypeID = 2  
              THEN DATEDIFF(MINUTE, DO.StartTime, DO.EndTime)  
            ELSE 0  
            END) SickPTOMinutes,  
        SUM(CASE   
            WHEN EDO.DayOffTypeID = 3  
              THEN DATEDIFF(MINUTE, DO.StartTime, DO.EndTime)  
            ELSE 0  
            END) HolidayPTOMinutes  
      FROM EmployeeDayOffs EDO  
      CROSS APPLY (  
        SELECT CASE   
            WHEN EDO.StartTime > DT.IndividualStartDateTime  
              THEN EDO.StartTime  
            ELSE DT.IndividualStartDateTime  
            END StartTime,  
          CASE   
            WHEN EDO.EndTime < DT.IndividualEndDateTime  
              THEN EDO.EndTime  
            ELSE DT.IndividualEndDateTime  
            END EndTime  
        ) DO  
      WHERE EDO.IsDeleted = 0  
        AND EDO.DayOffStatus = 'Approved'  
        AND EDO.EmployeeID = E.EmployeeID  
        AND DR.IndividualDate BETWEEN CONVERT(DATE, EDO.StartTime)  
          AND CONVERT(DATE, EDO.EndTime)  
      ) PTO  
    WHERE (  
        (  
          ISNULL(@EmployeeID, '0') = '0'  
          AND E.IsDeleted = 0  
          )  
        OR E.EmployeeID IN (  
          SELECT EL.[val]  
          FROM dbo.GetCSVTable(@EmployeeID) EL  
          )  
        )  
      AND DR.IndividualDate IS NOT NULL  
    )  
  SELECT EmployeeID EmpID,  
    EmployeeName AS [Employee Name],  
    SUM(T.VisitCount) AS EmployeeVisits,  
    SUM(VisitHours) AS VisitHours, --SUM(PTOHours) AS PTOHours,  
    0 AS OtherRegularHrs,  
    0 AS VacationHrs,  
    SUM(HolidayHours) AS HolidayHours,  
    SUM(PersonalHours) AS PersonalHours,  
    SUM(SickHours) AS SickHours,  
    CASE   
      WHEN SUM(VisitedMinutes) - SUM(RegularMinutes) <= 0  
        THEN 0  
      ELSE SUM(VisitedMinutes) - SUM(RegularMinutes)  
      END AS OTHours,  
    0 AS TravelHrs,  
    ROUND(SUM(Distance), 2) AS Miles,  
    NULL CareType,  
    NULL InsCompany  
  FROM (  
    SELECT EH.EmployeeID,  
      EH.EmployeeName,  
      EH.VisitCount,  
      CONVERT(DECIMAL, EH.VisitedMinutes, 114) VisitHours,  
      CONVERT(DECIMAL, EH.PersonalPTOMinutes, 114) PersonalHours,  
      CONVERT(DECIMAL, EH.SickPTOMinutes, 114) SickHours,  
      CONVERT(DECIMAL, EH.HolidayPTOMinutes, 114) HolidayHours,  
      EH.VisitedMinutes,  
      EH.RegularMinutes,  
      EH.Distance  
    FROM EmpHours EH  
    ) T  
  GROUP BY EmployeeID,  
    EmployeeName  
END  