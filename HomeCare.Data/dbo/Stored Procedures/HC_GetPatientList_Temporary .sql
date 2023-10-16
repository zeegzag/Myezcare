  
  
-- EXEC HC_GetPatientList_Temporary @BatchTypeID = '4', @PayorId = '12', @StartDate = '2021/10/19', @EndDate = '2021/10/24', @ClientName = '', @ServiceCodeIDs = '', @LoggedInID = '47', @IsDayCare = 'False'  
  
CREATE PROCEDURE HC_GetPatientList_Temporary                
@BatchTypeID bigint=1,                
@PayorId BIGINT=0,                
@ServiceCodeIDs VARCHAR(MAX)=null,                
@ClientName VARCHAR(MAX)=null,                
@StartDate Date='5/25/2020 12:00:00 AM',                
@EndDate Date ='6/07/2020 12:00:00 AM',                
@IsDayCare BIT =0 ,                
@LoggedInID BIGINT                
as                
BEGIN        
      
DECLARE @MedicaidType VARCHAR(50);                
SET @MedicaidType = 'Medicaid'                
      
if(@BatchTypeID = 1) -- 1: Initial Submission                
begin                
delete from dbo.SignatureLogs where noteid in (select noteid from notes where noteid not in (select noteid from batchnotes))                
Delete from notes where noteid not in (select noteid from batchnotes)                
DELETE FROM Notes_Temporary WHERE CreatedBy=@LoggedInID                
------------ Billing related changes ------------                
declare @Id int;                
declare @employeevisitid bigint;                
DROP TABLE IF EXISTS #temp                
select ROW_NUMBER() OVER(ORDER BY a.EmployeeVisitID ASC) AS Id, a.EmployeeVisitID into #temp                
from EmployeeVisits a                
inner join ScheduleMasters sm on sm.ScheduleID = a.ScheduleID                
INNER JOIN Referrals R On R.ReferralID = sm.ReferralID                
Inner JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID and RPM.isDeleted=0                
AND ( CONVERT(date, A.ClockInTime) between cast(RPM.PayorEffectiveDate as date) and RPM.PayorEffectiveEndDate)              
where a.EmployeeVisitID not in (select EmployeeVisitID from Notes b where @StartDate is null OR b.ServiceDate>= @StartDate AND @EndDate is null OR b.ServiceDate <= @EndDate)                
and a.isPCACompleted=1 and a.isDeleted=0 and sm.StartDate >= @StartDate and sm.EndDate <= dateadd(day,1,@EndDate) and SM.PayorID=@PayorId                
AND                
((@ClientName IS NULL OR LEN(R.LastName)=0)                
OR (                
(R.FirstName LIKE '%'+@ClientName+'%' ) OR                
(R.LastName LIKE '%'+@ClientName+'%') OR                
(R.FirstName +' '+R.LastName like '%'+@ClientName+'%') OR                
(R.LastName +' '+R.FirstName like '%'+@ClientName+'%') OR                
(R.FirstName +', '+R.LastName like '%'+@ClientName+'%') OR                
(R.LastName +', '+R.FirstName like '%'+@ClientName+'%')))                
                
--select * from #temp                
Select @ID=Count(*) From #Temp                
While @Id > 0                
Begin                
declare @resultid bigint;                
select @employeevisitid = EmployeeVisitID from #temp where Id=@Id                
-- exec HC_UpdateReferralAuthorization @employeevisitid                
-- Kundan: 11-06-2020                
-- Changes to handle old claims processing method to handle Payor Service code mapping                
IF((SELECT COUNT(*) FROM ReferralBillingAuthorizations                
WHERE PayorID = @PayorId AND Rate IS NOT NULL AND Rate <> 0) > 0)                
BEGIN                
IF(@IsDayCare=1)                
BEGIN                
EXEC [ADC].[API_AddNoteByCareType_Temporary] @employeevisitid, @LoggedInID, null, 0                
Print @employeevisitid                
Print 'D'                
END                
ELSE                
BEGIN                
EXEC [API_AddNoteByCareType_Temporary] @employeevisitid, @LoggedInID, null, 0                
Print @employeevisitid                
Print 'E'                
END                
                
END                
ELSE                
BEGIN                
EXEC [API_AddNoteByCareType_Temporary] @employeevisitid, @LoggedInID, null, 0                
END                
set @Id = (@Id - 1)                
End                
EXEC HC_CreateChildGroupNotes_Temporary  

------------ Billing related changes ends ------------                
SELECT count(N.NoteID) as TotalClaims, StrNoteIds=STRING_AGG ( ISNULL(N.NoteID,'0'), ','),  
CONVERT(DECIMAL(10,2), SUM(N.CalculatedAmount)) as TotalAmount,N.ReferralID,dbo.GetGeneralNameFormat(R.FirstName ,R.LastName) as PatientName --,R.FirstName,R.LastName                
,R.AHCCCSID,RPM.BeneficiaryNumber AS 'CISNumber',R.Dob ,P.PayorID ,P.PayorBillingType                
from Notes_Temporary N                
INNER JOIN Payors P On P.PayorID = N.PayorID                
INNER JOIN Referrals R On R.ReferralID = N.ReferralID                
Inner JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID and RPM.isDeleted=0                
and ( N.ServiceDate between cast(rpm.PayorEffectiveDate as date) and rpm.PayorEffectiveEndDate)             
-- and ( @startDate between cast(rpm.PayorEffectiveDate as date) and rpm.PayorEffectiveEndDate) --AND                
-- ( @endDate between cast(rpm.PayorEffectiveDate as date) and rpm.PayorEffectiveEndDate)                
LEFT JOIN DDMaster DM ON DM.DDMasterID = RPM.BeneficiaryTypeID AND DM.Title = @MedicaidType                
LEFT JOIN Notes NE ON NE.EmployeeVisitID = N.EmployeeVisitID                
where                
--N.CreatedBy=@LoggedInID AND                
-- N.IsBillable=1 AND N.IsDeleted=0 AND N.MarkAsComplete=1 AND N.GroupID IS NOT NULL                
-- AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate)))                
N.CreatedBy=@LoggedInID AND NE.NoteID IS NULL AND                
N.IsBillable=1 AND N.IsDeleted=0 AND N.MarkAsComplete=1 AND N.GroupID IS NOT NULL                
AND (((@StartDate is null OR N.ServiceDate>= @StartDate) AND (@EndDate is null OR N.ServiceDate <= @EndDate)))                
AND N.PayorID=@PayorID --AND RPM.PayorID = @PayorID                
AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))                
AND                
((@ClientName IS NULL OR LEN(R.LastName)=0)                
OR (                
(R.FirstName LIKE '%'+@ClientName+'%' ) OR                
(R.LastName LIKE '%'+@ClientName+'%') OR                
(R.FirstName +' '+R.LastName like '%'+@ClientName+'%') OR                
(R.LastName +' '+R.FirstName like '%'+@ClientName+'%') OR                
(R.FirstName +', '+R.LastName like '%'+@ClientName+'%') OR                
(R.LastName +', '+R.FirstName like '%'+@ClientName+'%')))                
--AND                
--N.NoteID NOT IN (                
-- SELECT DISTINCT NoteID FROM (                
-- SELECT BatchNoteID,BN.NoteID,CLP02_ClaimStatusCode, ROW_NUMBER() OVER ( PARTITION BY BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber                
-- FROM BatchNotes BN                
-- ) AS A WHERE RowNumber=1 AND ( CLP02_ClaimStatusCode IS NULL OR ( CLP02_ClaimStatusCode IN (1,2,3,4) ) )                
-- )                
group by N.ReferralID,R.FirstName,R.LastName,R.AHCCCSID,R.CISNumber,R.Dob ,P.PayorID ,P.PayorBillingType,RPM.BeneficiaryNumber                
order by PatientName                
end                
      
      
if(@BatchTypeID = 2) -- 2: Denial Re-Submission                
begin    
SELECT 1
select count(N.NoteID) as TotalClaims,StrNoteIds=STRING_AGG ( ISNULL(N.NoteID,'0'), ','), 
N.ReferralID,dbo.GetGeneralNameFormat(R.FirstName ,R.LastName) as PatientName --,R.FirstName,R.LastName                
,R.AHCCCSID,RPM.BeneficiaryNumber AS 'CISNumber',R.Dob,P.PayorID ,P.PayorBillingType                
from Notes N                
INNER JOIN (                
SELECT t.NoteID,t.CLP01_ClaimSubmitterIdentifier AS Original_ClaimSubmitterIdentifier, t.CLP07_PayerClaimControlNumber AS Original_PayerClaimControlNumber,                
t.ClaimAdjustmentTypeID AS Submitted_ClaimAdjustmentTypeID , t.ClaimAdjustmentReason                
FROM                
(SELECT DISTINCT ROW_NUMBER() OVER              
-- ( PARTITION BY BN.BatchID, BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber,                
( PARTITION BY BN.NoteID Order BY BN.BatchID DESC, MarkAsLatest DESC, BN.BatchNoteID Desc) AS RowNumber,                
NoteID,CLP02_ClaimStatusCode,CLP01_ClaimSubmitterIdentifier,                
CLP07_PayerClaimControlNumber, ClaimAdjustmentTypeID,ClaimAdjustmentReason,CLP04_TotalClaimPaymentAmount FROM BatchNotes BN                
) AS t --WHERE RowNumber=1 AND CLP02_ClaimStatusCode IS NOT NULL AND CLP02_ClaimStatusCode IN (4,22) AND ClaimAdjustmentTypeID IS NULL           
LEFT JOIN BatchNotes BN1 ON t.CLP07_PayerClaimControlNumber=BN1.Original_PayerClaimControlNumber                
WHERE RowNumber=1 AND                
(t.ClaimAdjustmentTypeID IS NULL OR t.ClaimAdjustmentTypeID NOT IN ('Replacement','Void','Write-Off','Denial') ) AND                
( CONVERT(DECIMAL(18,2),ISNULL(t.CLP04_TotalClaimPaymentAmount,0))= 0 AND t.CLP02_ClaimStatusCode IS NOT NULL) AND BN1.Original_PayerClaimControlNumber IS NULL                
--t.CLP02_ClaimStatusCode IS NOT NULL AND t.CLP02_ClaimStatusCode IN (4,22) AND t.ClaimAdjustmentTypeID IS NULL AND BN1.Original_PayerClaimControlNumber IS NULL                
) BND ON BND.NoteID=N.NoteID                
inner join Payors P On P.PayorID = N.PayorID                
inner join Referrals R On R.ReferralID = N.ReferralID                
LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID and RPM.isDeleted=0                
LEFT JOIN DDMaster DM ON DM.DDMasterID = RPM.BeneficiaryTypeID AND DM.Title = @MedicaidType                
where                
N.IsBillable=1 AND N.IsDeleted=0 AND N.MarkAsComplete=1 AND N.GroupID IS NOT NULL                
AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate)))                
AND N.PayorID=@PayorID AND RPM.PayorID = @PayorID                
AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))                
AND                
((@ClientName IS NULL OR LEN(R.LastName)=0)                
OR (                
(R.FirstName LIKE '%'+@ClientName+'%' ) OR                
(R.LastName LIKE '%'+@ClientName+'%') OR                
(R.FirstName +' '+R.LastName like '%'+@ClientName+'%') OR                
(R.LastName +' '+R.FirstName like '%'+@ClientName+'%') OR                
(R.FirstName +', '+R.LastName like '%'+@ClientName+'%') OR                
(R.LastName +', '+R.FirstName like '%'+@ClientName+'%')))                
group by N.ReferralID,R.FirstName,R.LastName,R.AHCCCSID,R.CISNumber,R.Dob ,P.PayorID ,P.PayorBillingType,RPM.BeneficiaryNumber                
order by PatientName                
end                
      
      
if(@BatchTypeID = 3) -- 3: Adjustment(Void/Replace) Submission                
BEGIN                
      
SELECT 1      
      
      
SELECT TotalClaims = COUNT(NoteID), StrNoteIds=STRING_AGG ( ISNULL(NoteID,'0'), ','),  
TotalAmount = CONVERT(DECIMAL(10,2), SUM(CalculatedAmount)),       
ReferralID, PatientName,AHCCCSID,CISNumber,Dob ,PayorID ,PayorBillingType--, Submitted_ClaimAdjustmentTypeID      
FROM (      
SELECT N.NoteID, N.CalculatedAmount,N.ReferralID      
,dbo.GetGeneralNameFormat(R.FirstName ,R.LastName) as PatientName ,R.AHCCCSID,CISNumber = RPM.BeneficiaryNumber,R.Dob ,P.PayorID ,P.PayorBillingType         
--,Submitted_ClaimAdjustmentTypeID      
      
 FROM Notes N                 
    INNER JOIN (                
                     
     SELECT t.NoteID,t.CLP01_ClaimSubmitterIdentifier AS Original_ClaimSubmitterIdentifier, t.CLP07_PayerClaimControlNumber AS Original_PayerClaimControlNumber,t.ClaimAdjustmentTypeID AS Submitted_ClaimAdjustmentTypeID ,t.ClaimAdjustmentReason           
   
   
 FROM                
       (SELECT DISTINCT ROW_NUMBER() OVER                 
       ( PARTITION BY BN.NoteID Order BY BN.BatchID DESC,MarkAsLatest DESC,  BN.BatchNoteID Desc)  AS RowNumber,                 
       -- ( PARTITION BY BN.BatchID, BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber,                 
       NoteID,CLP02_ClaimStatusCode,CLP01_ClaimSubmitterIdentifier,                 
        CLP07_PayerClaimControlNumber, ClaimAdjustmentTypeID , ClaimAdjustmentReason FROM BatchNotes BN                 
       ) AS t -- WHERE RowNumber=1 AND ClaimAdjustmentTypeID IS NOT NULL                
       LEFT JOIN  BatchNotes BN1 ON   t.CLP07_PayerClaimControlNumber=BN1.Original_PayerClaimControlNumber AND t.NoteID = BN1.NoteID      
       WHERE RowNumber=1 AND (t.ClaimAdjustmentTypeID IS NOT NULL AND                 
       t.ClaimAdjustmentTypeID NOT IN ('Write-Off','Denial','Resend','Data-Validation','Payor-Change','Other')                
       )                 
       AND BN1.Original_PayerClaimControlNumber IS NULL                
                
    ) BND ON BND.NoteID=N.NoteID                
    LEFT JOIN ChildNotes CN ON CN.ParentNoteID=N.NoteID                
 INNER JOIN Payors P On P.PayorID = N.PayorID                
 INNER JOIN Referrals R On R.ReferralID = N.ReferralID      
 Inner JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID and RPM.isDeleted=0                
    --INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID                
 INNER JOIN ReferralBillingAuthorizations PSM ON PSM.ReferralBillingAuthorizationID=N.ReferralBillingAuthorizationID and PSM.PayorID=@PayorID       
    WHERE N.IsBillable=1  AND N.MarkAsComplete=1 AND N.IsDeleted=0  AND N.GroupID IS NOT NULL --AND N.ParentID IS NULL                
 AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))                
    AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate)))                 
    AND N.PayorID=@PayorID --AND N.ReferralID in (SELECT val FROM GETCSVTABLE(@ReferralsIds))                       
 AND                
 ((@ClientName IS NULL OR LEN(R.LastName)=0)                
 OR (                
 (R.FirstName LIKE '%'+@ClientName+'%' ) OR                
 (R.LastName LIKE '%'+@ClientName+'%') OR                
 (R.FirstName +' '+R.LastName like '%'+@ClientName+'%') OR                
 (R.LastName +' '+R.FirstName like '%'+@ClientName+'%') OR                
 (R.FirstName +', '+R.LastName like '%'+@ClientName+'%') OR                
 (R.LastName +', '+R.FirstName like '%'+@ClientName+'%')))          
    GROUP BY  N.NoteID, N.CalculatedAmount,N.ReferralID,R.FirstName,R.LastName,R.AHCCCSID,R.CISNumber,R.Dob ,P.PayorID ,P.PayorBillingType,RPM.BeneficiaryNumber       
 --, Submitted_ClaimAdjustmentTypeID      
       
) AS TEMP_T       
GROUP BY ReferralID, PatientName,AHCCCSID,CISNumber,Dob ,PayorID ,PayorBillingType--, Submitted_ClaimAdjustmentTypeID      
ORDER BY PatientName       
       
      
      
 -- EXEC HC_GetPatientList_Temporary @BatchTypeID = '3', @PayorId = '12', @StartDate = '2021/10/19', @EndDate = '2021/10/24', @ClientName = '', @ServiceCodeIDs = '', @LoggedInID = '47', @IsDayCare = 'False'      
      
      
END               
      
      
                               
if(@BatchTypeID = 4) -- 4: Resend Data Validation Submission                                         
begin                          
       
SELECT 1       
       
SELECT TotalClaims = COUNT(NoteID), StrNoteIds=STRING_AGG ( ISNULL(NoteID,'0'), ','), 
TotalAmount = CONVERT(DECIMAL(10,2), SUM(CalculatedAmount)),       
ReferralID, PatientName,AHCCCSID,CISNumber,Dob ,PayorID ,PayorBillingType--, Submitted_ClaimAdjustmentTypeID      
FROM (      
SELECT N.NoteID, N.CalculatedAmount,N.ReferralID      
,dbo.GetGeneralNameFormat(R.FirstName ,R.LastName) as PatientName ,R.AHCCCSID,CISNumber = RPM.BeneficiaryNumber,R.Dob ,P.PayorID ,P.PayorBillingType         
--,Submitted_ClaimAdjustmentTypeID      
      
 FROM Notes N                 
    INNER JOIN (                
                     
     SELECT t.NoteID,t.CLP01_ClaimSubmitterIdentifier AS Original_ClaimSubmitterIdentifier, t.CLP07_PayerClaimControlNumber AS Original_PayerClaimControlNumber,t.ClaimAdjustmentTypeID AS Submitted_ClaimAdjustmentTypeID ,t.ClaimAdjustmentReason            
  
   
 FROM                
       (SELECT DISTINCT ROW_NUMBER() OVER                 
       ( PARTITION BY BN.NoteID Order BY BN.BatchID DESC,MarkAsLatest DESC,  BN.BatchNoteID Desc)  AS RowNumber,                 
       -- ( PARTITION BY BN.BatchID, BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber,                 
       NoteID,CLP02_ClaimStatusCode,CLP01_ClaimSubmitterIdentifier,                 
        CLP07_PayerClaimControlNumber, ClaimAdjustmentTypeID , ClaimAdjustmentReason FROM BatchNotes BN                 
       ) AS t -- WHERE RowNumber=1 AND ClaimAdjustmentTypeID IS NOT NULL                
       LEFT JOIN  BatchNotes BN1 ON   t.CLP07_PayerClaimControlNumber=BN1.Original_PayerClaimControlNumber AND t.NoteID = BN1.NoteID      
       WHERE RowNumber=1 AND (t.ClaimAdjustmentTypeID IS NOT NULL AND                 
       t.ClaimAdjustmentTypeID IN ('Resend','Data-Validation')                
       )                 
       AND BN1.Original_PayerClaimControlNumber IS NULL                
                
    ) BND ON BND.NoteID=N.NoteID                
    LEFT JOIN ChildNotes CN ON CN.ParentNoteID=N.NoteID                
 INNER JOIN Payors P On P.PayorID = N.PayorID                
 INNER JOIN Referrals R On R.ReferralID = N.ReferralID      
 Inner JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID and RPM.isDeleted=0                
    --INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID                
 INNER JOIN ReferralBillingAuthorizations PSM ON PSM.ReferralBillingAuthorizationID=N.ReferralBillingAuthorizationID and PSM.PayorID=@PayorID       
    WHERE N.IsBillable=1  AND N.MarkAsComplete=1 AND N.IsDeleted=0  AND N.GroupID IS NOT NULL --AND N.ParentID IS NULL                
 AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))                
    AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate)))                 
    AND N.PayorID=@PayorID --AND N.ReferralID in (SELECT val FROM GETCSVTABLE(@ReferralsIds))                       
 AND                
 ((@ClientName IS NULL OR LEN(R.LastName)=0)                
 OR (                
 (R.FirstName LIKE '%'+@ClientName+'%' ) OR                
 (R.LastName LIKE '%'+@ClientName+'%') OR                
 (R.FirstName +' '+R.LastName like '%'+@ClientName+'%') OR                
 (R.LastName +' '+R.FirstName like '%'+@ClientName+'%') OR                
 (R.FirstName +', '+R.LastName like '%'+@ClientName+'%') OR                
 (R.LastName +', '+R.FirstName like '%'+@ClientName+'%')))          
    GROUP BY  N.NoteID, N.CalculatedAmount,N.ReferralID,R.FirstName,R.LastName,R.AHCCCSID,R.CISNumber,R.Dob ,P.PayorID ,P.PayorBillingType,RPM.BeneficiaryNumber       
 --, Submitted_ClaimAdjustmentTypeID      
       
) AS TEMP_T       
GROUP BY ReferralID, PatientName,AHCCCSID,CISNumber,Dob ,PayorID ,PayorBillingType--, Submitted_ClaimAdjustmentTypeID      
ORDER BY PatientName       
       
      
      
end           
      
END 