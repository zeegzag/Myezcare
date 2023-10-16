-- EXEC HC_GetScheduleListByReferrals '','', '2018/03/19', '2018/03/25'              
-- EXEC HC_GetScheduleListByReferrals '','1951,2824', '2018/07/09',  '2018/07/15'           
CREATE PROCEDURE [dbo].[HC_DayCare_GetScheduleListByReferrals]      
 @FacilityIDs VARCHAR(MAX)= NULL,              
 @ReferralIDs VARCHAR(MAX)= NULL,              
 @StartDate DATE,                      
 @EndDate DATE,              
 @IsScheduled VARCHAR='-1'              
AS                      
BEGIN                       
              
 --------  Schecudle List For Perticular Facility  ---------               
              
 SELECT R.ReferralID, R.FirstName,R.LastName,C.Phone1,C.Phone2, C.Email,SM.ScheduleID,SS.ScheduleStatusName,RTD.ReferralTSDateID,              
 RTD.UsedInScheduling,RTD.OnHold,ScheduleComment=RTD.Notes, SM.ScheduleID, SM.ScheduleStatusID, SM.IsPatientAttendedSchedule, SM.AbsentReason,             
 StartDate= CASE WHEN SM.StartDate IS NOT NULL THEN SM.StartDate ELSE RTD.ReferralTSStartTime END,              
 EndDate= CASE WHEN SM.StartDate IS NOT NULL THEN SM.EndDate ELSE RTD.ReferralTSEndTime END,              
 F.FacilityName, F.FacilityID,FacilityNPI=F.NPI,      
 EmpFirstName = E.FirstName, EmpLastName= E.LastName,EmpEmail=E.Email, EmpMobile=E.MobileNumber, E.EmployeeUniqueID  ,E.EmployeeID,
 --UnAllocated =  CASE WHEN SM.FacilityID > 0 AND SM.PayorID > 0 THEN 0 ELSE 1 END   ,
 UnAllocated =  CASE WHEN SM.FacilityID > 0 THEN 0 ELSE 1 END, -- Changed by Kundan: 10/21/2020 removed payor dependency
 EV.ClockInTime, EV.ClockOutTime
 FROM Referrals R              
 INNER JOIN ReferralTimeSlotDates RTD ON RTD.ReferralID=R.ReferralID AND (RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate)              
 LEFT JOIN ScheduleMasters SM ON SM.ReferralTSDateID=RTD.ReferralTSDateID AND SM.IsDeleted = 0              
 LEFT JOIN ContactMappings CM on CM.ReferralID=R.ReferralID and CM.ContactTypeID=1                
 LEFT JOIN Contacts C on C.ContactID = CM.ContactID              
 LEFT JOIN ScheduleStatuses SS on SS.ScheduleStatusID= SM.ScheduleStatusID                      
 LEFT JOIN Employees E ON E.EmployeeID = SM.EmployeeID              
 LEFT JOIN Facilities F ON F.FacilityID= SM.FacilityID    
 LEFT JOIN EmployeeVisits EV ON EV.ScheduleID = SM.ScheduleID
 WHERE 1=1              
 AND           
 (@FacilityIDs IS NULL OR LEN(@FacilityIDs)=0 OR SM.EmployeeID IS NULL OR SM.FacilityID IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@FacilityIDs)))            
 AND (R.ReferralID IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@ReferralIDs)))           
 AND ( (@IsScheduled IS NULL OR LEN(@IsScheduled)=0  ) OR (@IsScheduled = '1' AND SM.ScheduleID>0 ) OR (@IsScheduled = '2' AND SM.ScheduleID IS NULL ) )      
      
END  