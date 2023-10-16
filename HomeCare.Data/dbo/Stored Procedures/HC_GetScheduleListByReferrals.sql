﻿--EXEC HC_GetScheduleListByReferrals '','', '2019/08/19', '2019/08/23'                            
-- EXEC HC_GetScheduleListByReferrals '','1951,2824', '2018/07/09',  '2018/07/15'                         
CREATE PROCEDURE [dbo].[HC_GetScheduleListByReferrals]                            
 @EmployeeIDs VARCHAR(MAX)= NULL,                            
 @ReferralIDs VARCHAR(MAX)= NULL,                            
 @StartDate DATE,                                    
 @EndDate DATE,                            
 @SchStatus BIGINT=0,                            
 @ServicetypeId int=0          
AS--select @EmployeeIDs = '', @ReferralIDs = '44', @StartDate = '2023/04/03', @EndDate = '2023/04/09', @SchStatus = '-1', @ServicetypeId = ''                                    
BEGIN  
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
                             
 --------  Schecudle List For Perticular Facility  ---------                             
 -- TRUNCATE TABLE ReferralTimeSlotDates                                   
 -- SELECT * FROM ReferralTimeSlotDates WHERE ReferralID=1951 ORDER BY ReferralTSDate ASC                            
                            
 SELECT distinct R.ReferralID, R.FirstName,R.LastName,dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) AS ReferralName, C.Phone1,C.Phone2, C.Email,SM.ScheduleID,SS.ScheduleStatusName,RTD.ReferralTSDateID,                            
 RTD.UsedInScheduling,RTD.OnHold,ScheduleComment=RTD.Notes, SM.ScheduleID, SM.ScheduleStatusID,RTD.IsDenied,                  
 IsPendingSchProcessed= CASE WHEN SM.ReferralTSDateID IS NULL AND SM.EmployeeTSDateID IS NULL THEN 1 ELSE 0 END,                
 StartDate= CASE WHEN SM.StartDate IS NOT NULL THEN  SM.StartDate ELSE RTD.ReferralTSStartTime END,                            
 EndDate= CASE WHEN SM.StartDate IS NOT NULL THEN SM.EndDate ELSE RTD.ReferralTSEndTime END,                            
 EmpFirstName = E.FirstName, EmpLastName= E.LastName,dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS EmployeeName,EmpEmail=E.Email, EmpMobile=E.MobileNumber, E.EmployeeUniqueID  ,E.EmployeeID                 
 ,CONVERT(varchar,EV.ClockInTime) as strClockInTime, CONVERT(varchar,EV.ClockOutTime) as strClockOutTime,EV.IVRClockIn,EV.IVRClockOut,EV.IsPCACompleted,              
 (SELECT ShortName FROM Payors WHERE PayorID = SM.PayorID) AS Payor,              
 (SELECT Title FROM DDMaster WHERE DDMasterID = ISNULL(SM.CareTypeId, RTSD.CareTypeId)) AS CareType, ISNULL(SM.CareTypeId, RTSD.CareTypeId) CareTypeId,      
 SM.ReferralBillingAuthorizationID, EV.IsApprovalRequired, SM.IsVirtualVisit                            
 FROM Referrals R                            
 INNER JOIN ReferralTimeSlotDates RTD ON RTD.ReferralID=R.ReferralID AND (RTD.ReferralTSDate BETWEEN @StartDate AND @EndDate)                            
 INNER JOIN ReferralTimeSlotDetails RTSD ON RTSD.ReferralTimeSlotDetailID = RTD.ReferralTimeSlotDetailID -- AND ISNULL(rtm.CareTypeId,0) <> 0      
 INNER JOIN ReferralTimeSlotMaster rtm on rtm.ReferralTimeSlotMasterID=rtd.ReferralTimeSlotMasterID    
 LEFT JOIN ScheduleMasters SM ON ISNULL(SM.OnHold, 0) = 0 AND SM.IsDeleted = 0 AND SM.ReferralTSDateID=RTD.ReferralTSDateID      
                
 LEFT JOIN ContactMappings CM on CM.ReferralID=R.ReferralID and CM.ContactTypeID=1                              
 LEFT JOIN Contacts C on C.ContactID = CM.ContactID                            
 LEFT JOIN ScheduleStatuses SS on SS.ScheduleStatusID= SM.ScheduleStatusID                                    
 LEFT JOIN Employees E ON E.EmployeeID = SM.EmployeeID                    
 LEFT JOIN EmployeeVisits EV ON EV.ScheduleID = SM.ScheduleID                
 LEFT JOIN EmployeeVisitNotes EN ON EN.EmployeeVisitID = EV.EmployeeVisitID                
 WHERE 1=1                            
 AND                         
 (@EmployeeIDs IS NULL OR LEN(@EmployeeIDs)=0 OR SM.EmployeeID IS NULL OR SM.EmployeeID IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@EmployeeIDs)))                          
 AND (R.ReferralID IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@ReferralIDs)))           
  AND ( ISNULL(CAST(@ServicetypeId AS BIGINT), 0) = 0 OR ','+ R.ServiceType + ',' LIKE '%,' + CAST(@ServicetypeId AS VARCHAR(MAX)) + ',%')    --and rtsd.IsDeleted=0                     
-- (@ReferralIDs IS NULL OR LEN(@ReferralIDs)=0 OR R.ReferralID IN (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@ReferralIDs)))                                   
-- AND (@SchStatus=0 OR (@SchStatus=1 AND SM.ScheduleStatusID IS NOT NULL) OR (@SchStatus=2 AND SM.ScheduleStatusID IS NULL AND RTD.UsedInScheduling=1 AND RTD.OnHold=0) )                            
                    
END 