CREATE PROCEDURE [dbo].[HC_PrivateDuty_GetScheduleListByReferrals]  
 @EmployeeIDs VARCHAR(MAX)= NULL,              
 @ReferralIDs VARCHAR(MAX)= NULL,              
 @StartDate DATE,                      
 @EndDate DATE,              
 @SchStatus BIGINT=0              
AS                      
BEGIN                       
     DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()          
 --------  Schecudle List For Perticular Facility  ---------               
 -- TRUNCATE TABLE ReferralTimeSlotDates                     
 -- SELECT * FROM ReferralTimeSlotDates WHERE ReferralID=1951 ORDER BY ReferralTSDate ASC              
              
 SELECT R.ReferralID, R.FirstName,R.LastName,dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) AS ReferralName,C.Phone1,C.Phone2, C.Email,SM.ScheduleID,SS.ScheduleStatusName,RTD.ReferralTSDateID,              
 RTD.UsedInScheduling,RTD.OnHold,ScheduleComment=RTD.Notes, SM.ScheduleID, SM.ScheduleStatusID,RTD.IsDenied,    
 StartDate= CASE WHEN SM.StartDate IS NOT NULL THEN SM.StartDate ELSE RTD.ReferralTSStartTime END,              
 EndDate= CASE WHEN SM.StartDate IS NOT NULL THEN SM.EndDate ELSE RTD.ReferralTSEndTime END,              
 EmpFirstName = E.FirstName, EmpLastName= E.LastName,dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS EmployeeName,EmpEmail=E.Email, EmpMobile=E.MobileNumber, E.EmployeeUniqueID  ,E.EmployeeID                 
 FROM Referrals R              
 INNER JOIN ReferralTimeSlotDates RTD ON RTD.ReferralID=R.ReferralID AND (RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate)              
 LEFT JOIN ScheduleMasters SM ON SM.ReferralTSDateID=RTD.ReferralTSDateID AND ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted = 0              
 LEFT JOIN ContactMappings CM on CM.ReferralID=R.ReferralID and CM.ContactTypeID=1                
 LEFT JOIN Contacts C on C.ContactID = CM.ContactID              
 LEFT JOIN ScheduleStatuses SS on SS.ScheduleStatusID= SM.ScheduleStatusID                      
 LEFT JOIN Employees E ON E.EmployeeID = SM.EmployeeID              
 WHERE 1=1              
 AND           
 (@EmployeeIDs IS NULL OR LEN(@EmployeeIDs)=0 OR SM.EmployeeID IS NULL OR SM.EmployeeID IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@EmployeeIDs)))            
 AND (R.ReferralID IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@ReferralIDs)))           
-- (@ReferralIDs IS NULL OR LEN(@ReferralIDs)=0 OR R.ReferralID IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@ReferralIDs)))                     
-- AND (@SchStatus=0 OR (@SchStatus=1 AND SM.ScheduleStatusID IS NOT NULL) OR (@SchStatus=2 AND SM.ScheduleStatusID IS NULL AND RTD.UsedInScheduling=1 AND RTD.OnHold=0) )              
      
END  