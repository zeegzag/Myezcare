-- EXEC HC_CM_GetPatientList_Temporary @BatchTypeID = '1', @PayorId = '5', @StartDate = '2023/03/01', @EndDate = '2023/03/10', @ClientName = '', @ServiceCodeIDs = '', @LoggedInID = '1'    
CREATE PROCEDURE [dbo].[HC_CM_GetPatientList_Temporary]                           
@BatchTypeID bigint=1,                                                                               
@PayorId BIGINT=0,                                                      
@ServiceCodeIDs VARCHAR(MAX)=null,                                                          
@ClientName VARCHAR(MAX)=null,                                                          
@StartDate Date,                                            
@EndDate Date    ,                    
@LoggedInID BIGINT                    
AS                                                                        
BEGIN 
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
SELECT 1      
  
if(@BatchTypeID = 1) -- 1: Initial Submission                                          
begin                              
                      
                      
DELETE FROM Notes_Temporary WHERE CreatedBy=@LoggedInID                 
                 
EXEC HC_CM_CreateBillingNotes_Temporary  @PayorID, @ServiceCodeIDs, @ClientName,@StartDate,@EndDate,@LoggedInID,'';                      
                      
DECLARE @t TABLE (ResfreshResult BIGINT)                      
insert @t (ResfreshResult) EXEC HC_CM_CreateChildGroupNotes_Temporary;                      
                      
                                
 select COUNT(N.NoteID) as TotalClaims, StrNoteIds=STRING_AGG ( ISNULL(N.NoteID,'0'), ','),   
 CONVERT(DECIMAL(10,2), SUM(N.CalculatedAmount)) as TotalAmount, N.ReferralID,dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) as PatientName --,R.FirstName,R.LastName                                        
 ,R.AHCCCSID,--R.CISNumber,          
 RPM.BeneficiaryNumber AS 'CISNumber', R.Dob ,P.PayorID  ,P.PayorBillingType                                 
 from Notes_Temporary  N                                      
 --INNER JOIN ChildNotes CN ON CN.NoteID=N.NoteID                                
 INNER JOIN Payors P  On P.PayorID = N.PayorID                                        
 INNER JOIN Referrals R  On R.ReferralID = N.ReferralID                                        
 INNER JOIN ReferralPayorMappings RPM ON RPM.ReferralID = R.ReferralID and RPM.isDeleted=0                                
 and  (  N.ServiceDate between cast(rpm.PayorEffectiveDate as date)  and rpm.PayorEffectiveEndDate)                       
 LEFT JOIN Notes NE ON NE.ReferralTSDateID = N.ReferralTSDateID            
 where                    
 N.CreatedBy=@LoggedInID  AND NE.NoteID  IS NULL  AND            
 N.IsBillable=1 AND N.IsDeleted=0 AND N.MarkAsComplete=1  AND N.GroupID IS NOT NULL                                    
 AND (((@StartDate is null OR N.ServiceDate>= @StartDate) AND (@EndDate is null OR N.ServiceDate <= @EndDate)))                                         
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
 --AND                                     
 --N.NoteID NOT IN (                                          
 --    SELECT DISTINCT NoteID FROM (                                          
 --    SELECT BatchNoteID,BN.NoteID,CLP02_ClaimStatusCode, ROW_NUMBER() OVER ( PARTITION BY BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber                                          
 --    FROM BatchNotes BN                              
 --    ) AS A WHERE RowNumber=1  AND ( CLP02_ClaimStatusCode IS NULL OR ( CLP02_ClaimStatusCode IN (1,2,3,4) ) )                                          
 --    -- SELECT NoteID FROM BatchNotes WHERE NoteID NOT IN ( SELECT NoteID FROM BatchNotes  WHERE CLP02_ClaimStatusCode IN (22) AND Submitted_ClaimAdjustmentTypeID IN ('Void','Replacement') )                                          
 --   )                                           
 group by N.ReferralID,R.FirstName,R.LastName,R.MiddleName, R.AHCCCSID,--R.CISNumber,          
 RPM.BeneficiaryNumber,R.Dob ,P.PayorID  ,P.PayorBillingType                                    
 order by PatientName                                        
end                                        
                     
if(@BatchTypeID = 2) -- 2: Denial Re-Submission                                          
begin                                        
 select count(N.NoteID) as TotalClaims,StrNoteIds=STRING_AGG ( ISNULL(N.NoteID,'0'), ','),   
 N.ReferralID,dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) as PatientName --,R.FirstName,R.LastName                                      
 ,R.AHCCCSID,R.CISNumber,R.Dob,P.PayorID ,P.PayorBillingType                                     
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
 group by N.ReferralID,R.FirstName,R.LastName,R.MiddleName, R.AHCCCSID,R.CISNumber,R.Dob ,P.PayorID ,P.PayorBillingType                                         
    order by PatientName                                       
end                                           
                                        
if(@BatchTypeID = 3) -- 3: Adjustment(Void/Replace) Submission                                         
begin                          
       
       
       
SELECT TotalClaims = COUNT(NoteID),  StrNoteIds=STRING_AGG ( ISNULL(NoteID,'0'), ','),   
TotalAmount = CONVERT(DECIMAL(10,2), SUM(CalculatedAmount)),       
ReferralID, PatientName,AHCCCSID,CISNumber,Dob ,PayorID ,PayorBillingType--, Submitted_ClaimAdjustmentTypeID      
FROM (      
SELECT N.NoteID, N.CalculatedAmount,N.ReferralID      
,dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) as PatientName ,R.AHCCCSID,CISNumber = RPM.BeneficiaryNumber,R.Dob ,P.PayorID ,P.PayorBillingType         
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
    GROUP BY  N.NoteID, N.CalculatedAmount,N.ReferralID,R.FirstName,R.LastName,R.MiddleName, R.AHCCCSID,R.CISNumber,R.Dob ,P.PayorID ,P.PayorBillingType,RPM.BeneficiaryNumber       
 --, Submitted_ClaimAdjustmentTypeID      
       
) AS TEMP_T       
GROUP BY ReferralID, PatientName,AHCCCSID,CISNumber,Dob ,PayorID ,PayorBillingType--, Submitted_ClaimAdjustmentTypeID      
ORDER BY PatientName       
       

      
end                                        
      
      
                                     
if(@BatchTypeID = 4) -- 4: Resend Data Validation Submission                                         
begin                          
       
       
       
SELECT TotalClaims = COUNT(NoteID), StrNoteIds=STRING_AGG ( ISNULL(NoteID,'0'), ','),   
TotalAmount = CONVERT(DECIMAL(10,2), SUM(CalculatedAmount)),       
ReferralID, PatientName,AHCCCSID,CISNumber,Dob ,PayorID ,PayorBillingType--, Submitted_ClaimAdjustmentTypeID      
FROM (      
SELECT N.NoteID, N.CalculatedAmount,N.ReferralID      
,dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) as PatientName ,R.AHCCCSID,CISNumber = RPM.BeneficiaryNumber,R.Dob ,P.PayorID ,P.PayorBillingType         
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
    GROUP BY  N.NoteID, N.CalculatedAmount,N.ReferralID,R.FirstName,R.LastName,R.MiddleName, R.AHCCCSID,R.CISNumber,R.Dob ,P.PayorID ,P.PayorBillingType,RPM.BeneficiaryNumber       
 --, Submitted_ClaimAdjustmentTypeID      
       
) AS TEMP_T       
GROUP BY ReferralID, PatientName,AHCCCSID,CISNumber,Dob ,PayorID ,PayorBillingType--, Submitted_ClaimAdjustmentTypeID      
ORDER BY PatientName       
       
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
end                                        
                                                                      
END  