
     
CREATE PROCEDURE [dbo].[GetEmployeeVisitList]    
@EmployeeVisitID BIGINT = 0,    
@EmployeeIDs NVARCHAR(500) = NULL,    
@ReferralIDs NVARCHAR(500) = NULL,    
@StartDate DATE = NULL,    
@EndDate DATE = NULL,    
@StartTime VARCHAR(20)=NULL,    
@EndTime VARCHAR(20)=NULL,    
@IsDeleted int=-1,    
@ActionTaken int=0,    
@SortExpression NVARCHAR(100),    
@SortType NVARCHAR(10),    
@FromIndex INT,    
@PageSize INT,    
@PayorIDs NVARCHAR(500) = NULL,    
@CareTypeIDs NVARCHAR(500) = NULL,    
@ServiceTypeID int=0,    
@IsPCACompleted nvarchar(50)=null ,    
@IsAuthExpired int = null,    
@ExecludeIncompletePending bit = 0,    
@ClaimActionStatus nvarchar(100)=null    
AS
--select @EmployeeVisitID = '0', @EmployeeIDs = '', @ReferralIDs = '', @StartTime = '', @EndTime = '', @StartDate = '2022/06/01', @EndDate = '2022/07/31', @IsDeleted = '0', @ActionTaken = '0', @PayorIDs = '', @CareTypeIDs = '', @ServiceTypeID = '', @SortExpression = 'EmployeeVisitID', @SortType = 'ASC', @FromIndex = '1', @PageSize = '50' 
BEGIN    
    
    
    
--set @ClaimStatus='Paid'    
;WITH CTEEmployeeVisitList AS    
(    
SELECT *,COUNT(t1.EmployeeVisitID) OVER() AS Count FROM    
(    
SELECT ROW_NUMBER() OVER (ORDER BY    
    
CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'EmployeeVisitID' THEN ev.EmployeeVisitID END END ASC,    
CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'EmployeeVisitID' THEN ev.EmployeeVisitID END END DESC,    
    
CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Name' THEN e.LastName END END ASC,    
CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Name' THEN e.LastName END END DESC,    
    
CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PCACompletedQ' THEN ev.IsPCACompleted END END ASC,    
CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PCACompletedQ' THEN ev.IsPCACompleted END END DESC,    
    
    
CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PatientName' THEN r.LastName END END ASC,    
CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PatientName' THEN r.LastName END END DESC,    
    
CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ClockInTime' THEN ClockInTime END END ASC,    
CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ClockInTime' THEN ClockInTime END END DESC,    
CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ClockOutTime' THEN ClockOutTime END END ASC,    
CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ClockOutTime' THEN ClockOutTime END END DESC,    
    
CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PayorName' THEN p.PayorName END END ASC,    
CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PayorName' THEN p.PayorName END END DESC,    
    
CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CareType' THEN d.Title END END ASC,    
CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CareType' THEN d.Title END END DESC,   
  
CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Distance' THEN MD.Distance END END ASC,                                                                                
CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Distance' THEN MD.Distance END END DESC,  
    
CASE WHEN @SortType = 'ASC' THEN    
CASE    
WHEN @SortExpression = 'StartDate' THEN sm.StartDate    
END    
END ASC,    
CASE WHEN @SortType = 'DESC' THEN    
CASE    
WHEN @SortExpression = 'StartDate' THEN sm.StartDate    
END    
END DESC,    
CASE WHEN @SortType = 'ASC' THEN    
CASE    
WHEN @SortExpression = 'EndDate' THEN sm.EndDate    
END    
END ASC,    
CASE WHEN @SortType = 'DESC' THEN    
CASE    
WHEN @SortExpression = 'EndDate' THEN sm.EndDate    
END    
END DESC,    
CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'TimeSheetComplete' THEN timesheetcomplete.totalhours END END ASC,    
CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'TimeSheetComplete' THEN timesheetcomplete.totalhours END END DESC    
) AS Row,    
--UpdatedBy:Akhilesh kamal    
--UpdateDate:17/01/2020    
--Description:For select BeneficiaryID,PlaceOfService,EmployeeUniqueID,Date,DayOfWeek    
ev.EmployeeVisitID,    
CASE    
WHEN (Submitted_ClaimAdjustmentTypeID IS NULL OR Submitted_ClaimAdjustmentTypeID NOT IN ('Void')) AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0 AND CSC.ClaimStatusName IS NOT NULL THEN 'Denied'    
WHEN Submitted_ClaimAdjustmentTypeID = 'Void' AND Original_PayerClaimControlNumber IS NOT NULL AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0 AND CSC.ClaimStatusName='Reversal of Previous Payment' THEN 'Paid'    
WHEN Submitted_ClaimAdjustmentTypeID = 'Void' AND Original_PayerClaimControlNumber IS NOT NULL AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0 AND CSC.ClaimStatusName!='Reversal of Previous Payment' THEN 'Denied'    
WHEN CSC.ClaimStatusName IS NOT NULL THEN 'Paid'    
WHEN ((bth.IsSent=1 or nts.EmployeeVisitID >0) and CSC.ClaimStatusName is null) then 'Billed'   
WHEN ISNULL(EV.IsApproved, 1) = 0 THEN 'Pending'    

else 'Not Billed' end as ClaimStatus,    
e.EmployeeID,Name=dbo.GetGeneralNameFormat(e.FirstName,e.LastName) ,r.ReferralID,    
PatientName=dbo.GetGeneralNameFormat(r.FirstName,r.LastName),r.AHCCCSID AS BeneficiaryID,PlaceOfService= CASE WHEN PlaceOfService IS NULL THEN 'NA' ELSE ev.PlaceOfService END,HHA_PCA_NP= CASE WHEN HHA_PCA_NP IS NULL THEN 'NA'    
ELSE e.EmployeeUniqueID END    
--,CONVERT(TIME(3),ev.ClockInTime) AS ClockInTime,CONVERT(TIME(3),ev.ClockOutTime) AS ClockOutTime    
,ev.ClockInTime AS ClockInTime, ev.ClockOutTime AS ClockOutTime    
,ev.IsDeleted,sm.StartDate,sm.EndDate,ev.IsByPassClockIn,ev.IsByPassClockOut,ev.ByPassReasonClockIn,ev.ByPassReasonClockOut,ev.ActionTaken,ev.RejectReason, ev    
.IsApprovalRequired,ev.SurveyCompleted ,ev.IsPCACompleted,ev.IVRClockIn,ev.IVRClockOut    
,p.PayorID,p.PayorName,d.DDMasterID as CareTypeID,d.Title As CareType,ev.Latitude as ClockInTimeLatitude,ev.Longitude as ClockInTimeLongitude,    
ev.PCACompletedLat as ClockOutTimeLatitude,ev.PCACompletedLong as ClockOutTimeLongitude,ev.SignedLat as SignedLatitude ,ev.SignedLong as SignedLongitude    
,PatientLocation.Latitude as PatientLatitude,PatientLocation.Longitude as PatientLongitude,TotalTask.TotalcreatedTask,payorcount.payorcount,timesheetcomplete.totalhours as TimeSheetCompleteHours    
,    
sm.StartDate AS Date,DATENAME(dw,sm.StartDate) as DayOfWeek, WorkingHour=DATEDIFF(minute,ev.ClockInTime,EV.ClockOutTime),ScheduleTime=DATEDIFF(minute,sm.StartDate,sm.EndDate)    
-- kunal patel: #2njec1 : Add Employee Visit more information Tip on the report.    
,ev.IsSigned, ev.SurveyComment, ev.IsEarlyClockIn, ev.EarlyClockInComment, sm.ReferralBillingAuthorizationID,rba.unittype, rba.PayRate,rba.Rate , rba.PerUnitQuantity, 
----------*********************************   FIX FOR TOTAL AMOUNT
----------********************************* 
--CAST (
--CASE  
--WHEN (rba.unittype = 2)   THEN ISNULL(rba.Rate,0)  
--WHEN ( rba.unittype = 5)  THEN  ISNULL(( rba.PerUnitQuantity / (DATEPART(hour, timesheetcomplete.totalhours) + DATEPART(minute,timesheetcomplete.totalhours) / 60.0 + DATEPART(second, timesheetcomplete.totalhours) / 3600.0)  ) * rba.Rate,0)  
--WHEN ( rba.unittype = 1)  THEN ISNULL( rba.Rate * (DATEPART(hour, timesheetcomplete.totalhours) + DATEPART(minute,timesheetcomplete.totalhours) / 60.0 + DATEPART(second, timesheetcomplete.totalhours) / 3600.0)  ,0)  
--END  
--as varchar )AS TotalAmount,
CAST (
CASE  
WHEN ( rba.unittype = 1)  THEN (DATEDIFF(MINUTE,ev.ClockInTime,ev.ClockOutTime)/ case when rba.PerUnitQuantity=0 then null else rba.PerUnitQuantity end)*rba.Rate
WHEN (rba.unittype = 2)   THEN ISNULL(rba.Rate,0)  
WHEN ( rba.unittype = 5) THEN ISNULL(rba.Rate,0)
END  
as varchar )AS TotalAmount, 
----------*********************************  FIX FOR TOTAL AMOUNT
----------*********************************  

rba.AuthorizationCode,F.FacilityName,ev.ScheduleID,case when rba.EndDate < CONVERT(date ,sm.startDate) then 1 else 0 end as IsAuthExpired    
,MD.Distance        
    
FROM EmployeeVisits ev    
Inner JOIN ScheduleMasters sm on sm.ScheduleID=ev.ScheduleID    
LEFT JOIN Employees e on e.EmployeeID=sm.EmployeeID    
INNER JOIN Referrals r on r.ReferralID=sm.ReferralID    
outer APPLY (SELECT [dbo].GetGeoFromLatLng(E.Latitude, E.Longitude) EmpLatLong) ELL  
outer APPLY (  
    SELECT CASE           
            WHEN EmpLatLong IS NULL          
            OR C.Latitude IS NULL          
            OR C.Longitude IS NULL          
            THEN NULL          
            ELSE (EmpLatLong.STDistance([dbo].GetGeoFromLatLng(C.Latitude, C.Longitude)) * 0.000621371)          
            END Distance  
 FROM ContactMappings cm  
 INNER JOIN Contacts c ON c.ContactID=cm.ContactID  
 where r.ReferralID=cm.ReferralID AND cm.ContactTypeID = 1  
) MD  
LEFT JOIN Payors p on p.PayorID=sm.PayorID    
LEFT JOIN DDMaster d on d.DDMasterID=sm.CareTypeId    
LEFT JOIN ReferralBillingAuthorizations rba ON rba.IsDeleted=0 AND rba.ReferralBillingAuthorizationID = sm.ReferralBillingAuthorizationID AND rba.ReferralID = sm.ReferralID    
LEFT JOIN Facilities F on F.FacilityID=sm.FacilityID    
LEFT JOIN Notes nts on nts.EmployeeVisitID=ev.EmployeeVisitID    
LEFT JOIN BatchNotes bn on bn.NoteId=nts.NoteId  AND BN.BatchNoteID=(SELECT MAX(BatchNoteID) FROM BatchNotes WHERE NoteID=NTS.NoteID  )  
--OUTER APPLY (SELECT )  
LEFT JOIN [Batches] bth on bth.BatchId=bn.BatchId    
LEFT JOIN ClaimStatusCodes CSC ON CSC.ClaimStatusCodeID=BN.CLP02_ClaimStatusCode    
  
outer apply(select top 1 c.Latitude,c.Longitude from ContactMappings cm,Contacts as c where r.ReferralID=cm.ReferralID and c.ContactID=cm.ContactID) PatientLocation    
outer apply(select count(*) TotalcreatedTask from EmployeeVisitNotes evn,ReferralTaskMappings rtm,VisitTasks v where rtm.ReferralTaskMappingID=evn.ReferralTaskMappingID    
and v.VisitTaskID=rtm.VisitTaskID and evn.IsDeleted=0 AND (v.VisitTaskType='Task' OR (evn.ServiceTime>0 AND Description IS NOT NULL))and evn.EmployeeVisitID=ev.EmployeeVisitID) as TotalTask    
outer apply(select count(p.PayorID) payorcount from [ReferralPayorMappings] as rpm inner join Payors p on p.PayorID=rpm.PayorID where rpm.ReferralID=r.ReferralID) as PayorCount    
outer apply (SELECT CONVERT(varchar,DATEADD(minute, DATEDIFF(minute, CONVERT(DATETIME, CONVERT(CHAR(8), getdate(), 112) + ' ' + CONVERT(CHAR(8), pcaev.ClockInTime, 108)),CONVERT(DATETIME, CONVERT(CHAR(8), getdate(), 112)+ ' ' + CONVERT(CHAR(8),    
pcaev.ClockOutTime, 108))), 0), 8) as totalhours from EmployeeVisits as pcaev where pcaev.EmployeeVisitID=ev.EmployeeVisitID )as timesheetcomplete    
WHERE --bth.IsSent=1 and    
(@ExecludeIncompletePending = 0 OR (@ExecludeIncompletePending = 1 and IsNull(ev.ActionTaken,-1) <> 1)) AND    
(case when rba.EndDate < CONVERT(date ,sm.startDate) then 1 else 0 end = @IsAuthExpired OR @IsAuthExpired IS NULL) AND    
((CAST(@IsDeleted AS BIGINT)=-1) OR ev.IsDeleted=@IsDeleted)    
AND ((@ActionTaken=0) OR ev.ActionTaken=@ActionTaken)    
--Updated by Kundan: removed clock in time    
--Update date: 25 Jan 2020    
--and (ev.ClockInTime is not null or ev.ClockInTime = '')    
--and ((@ServiceTypeID=0) OR r.ServiceType=@ServiceTypeID)    
--Search By Time    
--AND ((@StartTime IS NULL OR LEN(@StartTime)=0) OR FORMAT(ev.ClockInTime,'HH:mm:00') LIKE '%' + @StartTime + '%')    
--AND ((@EndTime IS NULL OR LEN(@EndTime)=0) OR FORMAT(ev.ClockOutTime,'HH:mm:00') LIKE '%' + @EndTime + '%')    
    
AND ((@StartTime IS NULL OR LEN(@StartTime)=0) OR format(ev.ClockInTime,'HH:mm:00') >= format(CONVERT(datetime,@StartTime),'HH:mm:00'))    
AND ((@EndTime IS NULL OR LEN(@EndTime)=0) OR format(ev.ClockOutTime,'HH:mm:00') <= format(CONVERT(datetime,@EndTime),'HH:mm:00'))    
    
AND ((@StartDate IS NULL OR LEN(@StartDate)=0) OR CONVERT(DATE,sm.StartDate) >= @StartDate)    
AND ((@EndDate IS NULL OR LEN(@EndDate)=0) OR CONVERT(DATE,sm.EndDate) <= @EndDate)    
--AND ((@StartDate IS NULL OR LEN(@StartDate)=0) OR CONVERT(VARCHAR(20),sm.StartDate,120) LIKE '%' + CONVERT(VARCHAR(20),@StartDate) + '%')    
-- AND ((@EndDate IS NULL OR LEN(@EndDate)=0) OR CONVERT(VARCHAR(20),sm.EndDate,120) LIKE '%' + CONVERT(VARCHAR(20),@EndDate) + '%')    
-- AND ((@StartDate IS NULL OR LEN(@StartDate)=0) OR (@EndDate IS NULL OR LEN(@EndDate)=0) OR    
--(CONVERT(Date,sm.StartDate) > CONVERT(DATE,@StartDate)) AND (CONVERT(Date,sm.EndDate) < CONVERT(DATE,@EndDate)))    
AND ((@EmployeeIDs IS NULL OR LEN(@EmployeeIDs)=0) OR (e.EmployeeID in (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@EmployeeIDs))))    
AND ((@ReferralIDs IS NULL OR LEN(@ReferralIDs)=0) OR (r.ReferralID in (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@ReferralIDs))))    
AND ((@PayorIDs IS NULL OR LEN(@PayorIDs)=0) OR (p.PayorID in (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@PayorIDs))))    
AND ((@CareTypeIDs IS NULL OR LEN(@CareTypeIDs)=0) OR (d.DDMasterID in (SELECT CONVERT(BIGINT,VAL) FROM GetCSVTable(@CareTypeIDs))))    
AND ((@IsPCACompleted is null) OR ev.IsPCACompleted=@IsPCACompleted)    
AND (isnull(@ClaimActionStatus,'')='' or    
(@ClaimActionStatus='Billed' AND ((bth.IsSent=1 or nts.EmployeeVisitID >0 ) and CSC.ClaimStatusName is null ))    
or    
(@ClaimActionStatus='Denied' AND    
(((Submitted_ClaimAdjustmentTypeID IS NULL OR Submitted_ClaimAdjustmentTypeID NOT IN ('Void'))    
AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0    
AND CSC.ClaimStatusName IS NOT NULL)or    
Submitted_ClaimAdjustmentTypeID = 'Void' AND Original_PayerClaimControlNumber IS NOT NULL    
AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0    
AND CSC.ClaimStatusName!='Reversal of Previous Payment'    
)    
) or    
(@ClaimActionStatus='Paid' AND ((    
Submitted_ClaimAdjustmentTypeID = 'Void' AND Original_PayerClaimControlNumber IS NOT NULL    
AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0    
AND CSC.ClaimStatusName='Reversal of Previous Payment') or CSC.ClaimStatusName IS NOT NULL)    
)    
or    
(    
@ClaimActionStatus='Not Billed' AND    
(bth.IsSent is null )    
)    
or    
(    
@ClaimActionStatus='Invalid' AND (CareType is null or rba.EndDate < CONVERT(date ,sm.startDate))    
)    
)    
) AS t1 )    
    
SELECT * FROM CTEEmployeeVisitList    
    
    
    
outer apply(select count(ap.EmployeeVisitID) as InvalidVisitCount from CTEEmployeeVisitList ap where ap.CareType is null or ap.IsAuthExpired=1 ) as TotalInvalidVisit    
outer apply(select count(ap.EmployeeVisitID) as NotBilledCount from CTEEmployeeVisitList ap where ap.ClaimStatus='Not Billed' ) as TotalNotBilled    
outer apply(select count(ap.EmployeeVisitID) as DeniedCount from CTEEmployeeVisitList ap where ap.ClaimStatus='Denied' ) as TotalDenied    
outer apply(select count(ap.EmployeeVisitID) as PaidCount from CTEEmployeeVisitList ap where ap.ClaimStatus='Paid' ) as TotalPaid    
outer apply(select count(ap.EmployeeVisitID) as BilledCount from CTEEmployeeVisitList ap where ap.ClaimStatus='Billed' or ap.ClaimStatus='Sent' ) as TotalBilled    
outer apply(select count(ap.EmployeeVisitID) as ARPcount from CTEEmployeeVisitList ap where ap.ActionTaken=1 ) as TotalApprovalPending    
outer apply(select count(ap.EmployeeVisitID) as ARAcount from CTEEmployeeVisitList ap where ap.ActionTaken=2 ) as TotalApprovalApproved    
outer apply(select count(ap.EmployeeVisitID) as ARRcount from CTEEmployeeVisitList ap where ap.ActionTaken=3 ) as TotalApprovalRejected    
outer apply(select count(ap.EmployeeVisitID) as IsPCACompletedcount from CTEEmployeeVisitList ap where ap.IsPCACompleted=1 ) as TotalIsPCACompleted    
outer apply(select count(ap.EmployeeVisitID) as NotIsPCACompletedcount from CTEEmployeeVisitList ap where ap.IsPCACompleted=0 ) as TotalNotIsPCACompleted    
WHERE    
-- AND (isnull(@ClaimStatus,'')='' or ClaimStatus=@ClaimStatus)    
ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)    
END    