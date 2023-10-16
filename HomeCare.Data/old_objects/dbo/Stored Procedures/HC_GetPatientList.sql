-- exec HC_GetPatientList 1,18,null,null,'2018-08-15','2018-09-15'                
CREATE PROCEDURE [dbo].[HC_GetPatientList]                  
@BatchTypeID bigint=1,                                                       
@PayorId BIGINT=0,                              
@ServiceCodeIDs VARCHAR(MAX)=null,                                  
@ClientName VARCHAR(MAX)=null,                                  
@StartDate Date,                    
@EndDate Date                          
AS                                                
BEGIN                     
DECLARE @MedicaidType VARCHAR(50);
SET @MedicaidType = 'Medicaid'                   
if(@BatchTypeID = 1) -- 1: Initial Submission                  
begin    

    ------------ Billing related changes ------------
declare @Id int;
declare @employeevisitid bigint;
DROP TABLE IF EXISTS #temp

select ROW_NUMBER() OVER(ORDER BY  a.EmployeeVisitID ASC) AS Id, a.EmployeeVisitID into #temp from EmployeeVisits a
where a.EmployeeVisitID not in (select EmployeeVisitID from Notes b  where @StartDate is null OR b.ServiceDate>= @StartDate AND @EndDate is null OR b.ServiceDate <= @EndDate) 
and a.isPCACompleted=1 and isDeleted=0 
Select @Id = Count(*) From #Temp
While @Id > 0
Begin
	declare @resultid bigint;
	select @employeevisitid = EmployeeVisitID from #temp where Id=@Id

	EXEC [API_AddNoteByCareType_01] @employeevisitid, 0, null, 0
	set @Id = (@Id - 1)
End
EXEC HC_RefreshAndGroupingNotes
------------ Billing related changes ends ------------

 select count(N.NoteID) as TotalClaims,N.ReferralID,dbo.GetGeneralNameFormat(R.FirstName ,R.LastName) as PatientName --,R.FirstName,R.LastName                
 ,R.AHCCCSID,RPM.BeneficiaryNumber AS 'CISNumber',R.Dob ,P.PayorID  ,P.PayorBillingType         
 from Notes N              
 Left JOIN ChildNotes CN ON N.NoteID=CN.NoteID        
 INNER JOIN Payors P  On P.PayorID = N.PayorID                
 INNER JOIN Referrals R  On R.ReferralID = N.ReferralID
 Inner JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID and RPM.isDeleted=0
 LEFT JOIN DDMaster DM ON DM.DDMasterID = RPM.BeneficiaryTypeID AND DM.Title = @MedicaidType
 where             
 N.IsBillable=1 AND N.IsDeleted=0 AND N.MarkAsComplete=1  AND N.GroupID IS NOT NULL            
 AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate)))                 
 AND N.PayorID=@PayorID             
 AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))             
 AND            
 ((@ClientName IS NULL OR LEN(R.LastName)=0)                 
 OR (                
    (R.FirstName LIKE '%'+@ClientName+'%' ) OR                  
    (R.LastName  LIKE '%'+@ClientName+'%')  OR                  
    (R.FirstName +' '+R.LastName like '%'+@ClientName+'%') OR                  
    (R.LastName +' '+R.FirstName like '%'+@ClientName+'%') OR                  
    (R.FirstName +', '+R.LastName like '%'+@ClientName+'%') OR                  
    (R.LastName +', '+R.FirstName like '%'+@ClientName+'%')))              
 AND             
 N.NoteID NOT  IN (                  
     SELECT DISTINCT NoteID FROM (                  
     SELECT BatchNoteID,BN.NoteID,CLP02_ClaimStatusCode, ROW_NUMBER() OVER ( PARTITION BY BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber                  
     FROM BatchNotes BN                  
     ) AS A WHERE RowNumber=1  AND ( CLP02_ClaimStatusCode IS NULL OR ( CLP02_ClaimStatusCode IN (1,2,3,4) ) )                  
     -- SELECT NoteID FROM BatchNotes WHERE NoteID NOT IN ( SELECT NoteID FROM BatchNotes  WHERE CLP02_ClaimStatusCode IN (22) AND Submitted_ClaimAdjustmentTypeID IN ('Void','Replacement') )                  
    )                   
 group by N.ReferralID,R.FirstName,R.LastName,R.AHCCCSID,R.CISNumber,R.Dob ,P.PayorID  ,P.PayorBillingType,RPM.BeneficiaryNumber        
 order by PatientName                
end                
                                       
if(@BatchTypeID = 2) -- 2: Denial Re-Submission                  
begin                
 select count(N.NoteID) as TotalClaims,N.ReferralID,dbo.GetGeneralNameFormat(R.FirstName ,R.LastName) as PatientName --,R.FirstName,R.LastName              
 ,R.AHCCCSID,RPM.BeneficiaryNumber AS 'CISNumber',R.Dob,P.PayorID ,P.PayorBillingType             
 from Notes N                
 INNER JOIN (                  
                       
  SELECT t.NoteID,t.CLP01_ClaimSubmitterIdentifier AS Original_ClaimSubmitterIdentifier, t.CLP07_PayerClaimControlNumber AS Original_PayerClaimControlNumber,                   
  t.ClaimAdjustmentTypeID AS Submitted_ClaimAdjustmentTypeID , t.ClaimAdjustmentReason                  
  FROM                  
  (SELECT DISTINCT ROW_NUMBER() OVER                   
  -- ( PARTITION BY BN.BatchID, BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber,                  
  ( PARTITION BY BN.NoteID Order BY BN.BatchID DESC, MarkAsLatest DESC,  BN.BatchNoteID Desc)  AS RowNumber,                   
  NoteID,CLP02_ClaimStatusCode,CLP01_ClaimSubmitterIdentifier,                  
  CLP07_PayerClaimControlNumber, ClaimAdjustmentTypeID,ClaimAdjustmentReason,CLP04_TotalClaimPaymentAmount FROM BatchNotes BN                   
  ) AS t --WHERE RowNumber=1 AND CLP02_ClaimStatusCode IS NOT NULL AND CLP02_ClaimStatusCode IN (4,22) AND ClaimAdjustmentTypeID IS NULL                  
  LEFT JOIN  BatchNotes BN1 ON   t.CLP07_PayerClaimControlNumber=BN1.Original_PayerClaimControlNumber                  
  WHERE RowNumber=1 AND                   
(t.ClaimAdjustmentTypeID IS NULL OR t.ClaimAdjustmentTypeID NOT IN ('Replacement','Void','Write-Off','Denial') ) AND                  
  ( CONVERT(DECIMAL(18,2),ISNULL(t.CLP04_TotalClaimPaymentAmount,0))= 0 AND t.CLP02_ClaimStatusCode IS NOT NULL) AND BN1.Original_PayerClaimControlNumber IS NULL                  
  --t.CLP02_ClaimStatusCode IS NOT NULL AND t.CLP02_ClaimStatusCode IN (4,22) AND t.ClaimAdjustmentTypeID IS NULL AND BN1.Original_PayerClaimControlNumber IS NULL                  
                  
 ) BND ON BND.NoteID=N.NoteID                 
 inner join Payors P  On P.PayorID = N.PayorID                
 inner join Referrals R  On R.ReferralID = N.ReferralID   
 LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID and RPM.isDeleted=0
 LEFT JOIN DDMaster DM ON DM.DDMasterID = RPM.BeneficiaryTypeID AND DM.Title = @MedicaidType   
 where             
 N.IsBillable=1 AND N.IsDeleted=0 AND N.MarkAsComplete=1  AND N.GroupID IS NOT NULL            
 AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate)))               
 AND N.PayorID=@PayorID             
 AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))            
 AND            
 ((@ClientName IS NULL OR LEN(R.LastName)=0)                 
 OR (                
    (R.FirstName LIKE '%'+@ClientName+'%' ) OR                  
    (R.LastName  LIKE '%'+@ClientName+'%')  OR                  
    (R.FirstName +' '+R.LastName like '%'+@ClientName+'%') OR                  
    (R.LastName +' '+R.FirstName like '%'+@ClientName+'%') OR                  
    (R.FirstName +', '+R.LastName like '%'+@ClientName+'%') OR                  
    (R.LastName +', '+R.FirstName like '%'+@ClientName+'%')))              
 group by N.ReferralID,R.FirstName,R.LastName,R.AHCCCSID,R.CISNumber,R.Dob ,P.PayorID ,P.PayorBillingType,RPM.BeneficiaryNumber                 
    order by PatientName               
end                   
                
if(@BatchTypeID = 3) -- 3: Adjustment(Void/Replace) Submission                 
begin                
 select count(N.NoteID) as TotalClaims,N.ReferralID,dbo.GetGeneralNameFormat(R.FirstName ,R.LastName) as PatientName --,R.FirstName,R.LastName                
 ,R.AHCCCSID,RPM.BeneficiaryNumber AS 'CISNumber',R.Dob   ,P.PayorID,P.PayorBillingType         
 from Notes N                
 INNER JOIN (                  
                       
  SELECT t.NoteID,t.CLP01_ClaimSubmitterIdentifier AS Original_ClaimSubmitterIdentifier, t.CLP07_PayerClaimControlNumber AS Original_PayerClaimControlNumber,t.ClaimAdjustmentTypeID AS Submitted_ClaimAdjustmentTypeID ,                  
  t.ClaimAdjustmentReason                  
  FROM                  
  (SELECT DISTINCT ROW_NUMBER() OVER                   
  ( PARTITION BY BN.NoteID Order BY BN.BatchID DESC,MarkAsLatest DESC,  BN.BatchNoteID Desc)  AS RowNumber,                   
  -- ( PARTITION BY BN.BatchID, BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber,                   
  NoteID,CLP02_ClaimStatusCode,CLP01_ClaimSubmitterIdentifier,                   
  CLP07_PayerClaimControlNumber, ClaimAdjustmentTypeID , ClaimAdjustmentReason FROM BatchNotes BN                   
  ) AS t -- WHERE RowNumber=1 AND ClaimAdjustmentTypeID IS NOT NULL                  
  LEFT JOIN  BatchNotes BN1 ON   t.CLP07_PayerClaimControlNumber=BN1.Original_PayerClaimControlNumber                  
  WHERE RowNumber=1 AND (t.ClaimAdjustmentTypeID IS NOT NULL AND                   
  --t.ClaimAdjustmentTypeID !='Write-Off'                  
  t.ClaimAdjustmentTypeID NOT IN ('Write-Off','Denial')                  
  )                   
  AND BN1.Original_PayerClaimControlNumber IS NULL                  
                  
 ) BND ON BND.NoteID=N.NoteID                  
 inner join Payors P  On P.PayorID = N.PayorID                
 inner join Referrals R  On R.ReferralID = N.ReferralID        
  LEFT JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID and RPM.isDeleted=0
  LEFT JOIN DDMaster DM ON DM.DDMasterID = RPM.BeneficiaryTypeID AND DM.Title = @MedicaidType
 where             
 N.IsBillable=1 AND N.IsDeleted=0 AND N.MarkAsComplete=1  AND N.GroupID IS NOT NULL            
 AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate)))               
 AND N.PayorID=@PayorID             
 AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))             
 AND            
 ((@ClientName IS NULL OR LEN(R.LastName)=0)                 
 OR (                
    (R.FirstName LIKE '%'+@ClientName+'%' ) OR                  
    (R.LastName  LIKE '%'+@ClientName+'%')  OR                  
    (R.FirstName +' '+R.LastName like '%'+@ClientName+'%') OR                  
    (R.LastName +' '+R.FirstName like '%'+@ClientName+'%') OR                  
    (R.FirstName +', '+R.LastName like '%'+@ClientName+'%') OR                  
    (R.LastName +', '+R.FirstName like '%'+@ClientName+'%')))              
 group by N.ReferralID,R.FirstName,R.LastName,R.AHCCCSID,R.CISNumber,R.Dob   ,P.PayorID,P.PayorBillingType,RPM.BeneficiaryNumber   
 order by PatientName               
end                
                                         
END
