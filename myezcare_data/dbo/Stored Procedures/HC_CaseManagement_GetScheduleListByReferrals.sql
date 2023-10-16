CREATE PROCEDURE [dbo].[HC_CaseManagement_GetScheduleListByReferrals]      
 @FacilityIDs VARCHAR(MAX)= NULL,              
 @ReferralIDs VARCHAR(MAX)= NULL,              
 @StartDate DATE,                      
 @EndDate DATE,              
 @IsScheduled VARCHAR='-1'              
AS                      
BEGIN                       
              
 --------  Schecudle List For Perticular Facility  ---------               
              
 SELECT R.ReferralID, R.FirstName,R.LastName,C.Phone1,C.Phone2, C.Email,RTD.ReferralTSDateID,              
 RTD.UsedInScheduling,RTD.OnHold,ScheduleComment=RTD.Notes,
 --SM.ScheduleID,SS.ScheduleStatusName, SM.ScheduleID, SM.ScheduleStatusID, SM.IsPatientAttendedSchedule, SM.AbsentReason,             
 StartDate= RTD.ReferralTSStartTime,              
 EndDate= RTD.ReferralTSEndTime--,              
 --F.FacilityName, F.FacilityID,FacilityNPI=F.NPI,      
 --EmpFirstName = E.FirstName, EmpLastName= E.LastName,EmpEmail=E.Email, EmpMobile=E.MobileNumber, E.EmployeeUniqueID  ,E.EmployeeID                 
 FROM Referrals R              
 INNER JOIN ReferralTimeSlotDates RTD ON RTD.ReferralID=R.ReferralID AND (RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate)              
 --LEFT JOIN ScheduleMasters SM ON SM.ReferralTSDateID=RTD.ReferralTSDateID AND SM.IsDeleted = 0              
 LEFT JOIN ContactMappings CM on CM.ReferralID=R.ReferralID and CM.ContactTypeID=1                
 LEFT JOIN Contacts C on C.ContactID = CM.ContactID              
 --LEFT JOIN ScheduleStatuses SS on SS.ScheduleStatusID= SM.ScheduleStatusID                      
 --LEFT JOIN Employees E ON E.EmployeeID = SM.EmployeeID              
 --LEFT JOIN Facilities F ON F.FacilityID= SM.FacilityID      
 WHERE 1=1              
 --AND           
 --(@FacilityIDs IS NULL OR LEN(@FacilityIDs)=0 OR SM.EmployeeID IS NULL OR SM.FacilityID IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@FacilityIDs)))            
 AND (R.ReferralID IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@ReferralIDs)))           
 --AND ( (@IsScheduled IS NULL OR LEN(@IsScheduled)=0  ) OR (@IsScheduled = '1' AND SM.ScheduleID>0 ) OR (@IsScheduled = '2' AND SM.ScheduleID IS NULL ) )      
      
END
