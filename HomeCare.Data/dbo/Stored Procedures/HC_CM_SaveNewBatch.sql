

-- exec HC_SaveNewBatch 0,1,1,null,'2394,3880','2017-12-01','2017-12-31','test',1,1      
CREATE Procedure [dbo].[HC_CM_SaveNewBatch]
@BatchID  bigint,        
@BatchTypeID bigint,                                             
@PayorID bigint,           
@ServiceCodeIDs VARCHAR(MAX)=null,      
@ReferralsIds varchar(max)=null,      
                                       
@StartDate date,                                                  
@EndDate date,        
@Comment VARCHAR(MAX),        
                                          
@CreatedBy bigint,                                      
@BatchNoteStatusID bigint                   
AS         
BEGIN        
        
 DECLARE @NewBatchID bigint                                          
 --Insert Batch Table Record        
        
 INSERT INTO Batches(BatchTypeID,PayorID,StartDate,EndDate,IsDeleted,IsSentBy,IsSent,SentDate,CreatedBy,CreatedDate,                                          
 UpdatedDate,UpdatedBy,SystemID,Comment)                                          
 values(@BatchTypeID,@PayorID,@StartDate,@EndDate,0,NULL,0,NULL,@CreatedBy,getutcdate(),getutcdate(),@CreatedBy,1,@Comment)                                          
        
 SET @NewBatchID=SCOPE_IDENTITY()         
      
         
 IF(LEN(@ServiceCodeIDs)=0)        
  SELECT @ServiceCodeIDs=SUBSTRING((SELECT ',' + Convert(varchar(max),ServiceCodeID) FROM ServiceCodes WHERE IsDeleted=0 ORDER BY ServiceCode FOR XML PATH('')),2,200000)             
        
 IF(LEN(@ServiceCodeIDs)>0)                                      
 BEGIN        
  INSERT INTO BatchApprovedServiceCodes(ServiceCodeID,BatchID)  (SELECT val,@NewBatchID FROM GETCSVTABLE(@ServiceCodeIDs))                                          
 END        
             
  ---Insert Batch notes             
 IF(@BatchTypeID=1)  -- 1: Initial Submission        
 BEGIN        

 -- NOTES


 --EXEC HC_CM_CreateBillingNotes  @PayorID, @ServiceCodeIDs, @ReferralsIds,@StartDate,@EndDate,@CreatedBy,'';
 --EXEC HC_RefreshAndGroupingNotes;
         
 INSERT INTO BatchNotes       
 (BatchID,NoteID,BatchNoteStatusID,      
 CLM_UNIT,CLM_BilledAmount,CLP03_TotalClaimChargeAmount,      
 Original_Unit,Original_Amount,      
 IsFirstTimeClaimInBatch,Submitted_ClaimAdjustmentTypeID,IsUseInBilling,IsNewProcess)        
 SELECT        
 @NewBatchID as BatchID,N.NoteID,@BatchNoteStatusID,        
    TempCalculatedUnit=       
  CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)         
  THEN PSM.BillingUnitLimit ELSE SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit)) END ,         
    TempCalculatedAmount=       
  CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)         
        THEN CONVERT( DECIMAL(10,2), (SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)) / SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))) * PSM.BillingUnitLimit )         
        ELSE CONVERT(DECIMAL(10,2), SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount))) END,          
    TempCalculatedAmount=       
  CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)         
        THEN CONVERT( DECIMAL(10,2), (SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)) / SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))) * PSM.BillingUnitLimit )         
  ELSE CONVERT(DECIMAL(10,2), SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount))) END,          
 Original_Unit=N.CalculatedUnit,Original_Amount=N.CalculatedAmount,        
    1,'Original', CASE WHEN N.ParentID IS NULL THEN 1 ELSE 0 END,1        
    FROM Notes N         
    LEFT JOIN ChildNotes CN ON CN.ParentNoteID=N.NoteID        
    INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID        
    WHERE      
 N.IsBillable=1  AND N.MarkAsComplete=1 AND N.IsDeleted=0 AND N.GroupID IS NOT NULL      
    AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate)))         
    AND N.PayorID=@PayorID       
 AND N.ReferralID in (SELECT val FROM GETCSVTABLE(@ReferralsIds))           
    AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs))))               
    AND N.NoteID NOT IN (        
     SELECT DISTINCT NoteID FROM (        
     SELECT BatchNoteID,BN.NoteID,CLP02_ClaimStatusCode, ROW_NUMBER() OVER ( PARTITION BY BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber        
     FROM BatchNotes BN        
     ) AS A WHERE RowNumber=1  AND ( CLP02_ClaimStatusCode IS NULL OR ( CLP02_ClaimStatusCode IN (1,2,3,4) ) )        
    )          
    GROUP BY N.NoteID, N.ParentID, CN.ParentNoteID, PSM.BillingUnitLimit,N.CalculatedAmount,N.CalculatedUnit        
    ORDER BY N.NoteID        
       
 END        
       
 ELSE IF (@BatchTypeID=2)     -- 2: Denial Re-Submission        
 BEGIN        
       
 INSERT INTO BatchNotes       
 (BatchID,NoteID,BatchNoteStatusID,CLM_UNIT,CLM_BilledAmount,CLP03_TotalClaimChargeAmount,Original_Unit,Original_Amount,IsFirstTimeClaimInBatch,        
 Submitted_ClaimAdjustmentTypeID,Original_ClaimSubmitterIdentifier,Original_PayerClaimControlNumber,ClaimAdjustmentReason,IsUseInBilling,IsNewProcess)        
 SELECT        
 @NewBatchID as BatchID,N.NoteID,@BatchNoteStatusID,        
 TempCalculatedUnit=       
  CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)         
  THEN PSM.BillingUnitLimit       
  ELSE SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit)) END ,         
 TempCalculatedAmount=       
  CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)         
  THEN CONVERT( DECIMAL(10,2), (SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)) / SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))) * PSM.BillingUnitLimit )            
  ELSE CONVERT(DECIMAL(10,2), SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)))END ,                     
 TempCalculatedAmount=       
  CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)         
  THEN CONVERT( DECIMAL(10,2), (SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)) / SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))) * PSM.BillingUnitLimit )                 
  ELSE CONVERT(DECIMAL(10,2), SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount))) END ,           
 Original_Unit=N.CalculatedUnit,Original_Amount=N.CalculatedAmount,        
 1,'Denial', Original_ClaimSubmitterIdentifier, Original_PayerClaimControlNumber,ClaimAdjustmentReason,        
 CASE WHEN N.ParentID IS NULL THEN 1 ELSE 0 END,1        
 FROM Notes N        
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
 LEFT JOIN ChildNotes CN ON CN.ParentNoteID=N.NoteID        
    INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID        
    WHERE       
 N.IsBillable=1  AND N.MarkAsComplete=1 AND N.IsDeleted=0  AND N.GroupID IS NOT NULL      
    AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate)))         
    AND N.PayorID=@PayorID       
 AND N.ReferralID in (SELECT val FROM GETCSVTABLE(@ReferralsIds))      
    AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))             
    GROUP BY N.NoteID,N.ParentID,CN.ParentNoteID,Submitted_ClaimAdjustmentTypeID,Original_ClaimSubmitterIdentifier,Original_PayerClaimControlNumber,ClaimAdjustmentReason,PSM.BillingUnitLimit,N.CalculatedAmount,N.CalculatedUnit        
    ORDER BY N.NoteID        
       
 END        
        
  ELSE IF (@BatchTypeID=3)  -- 3: Adjustment(Void/Replace) Submission        
  BEGIN        
      
 INSERT INTO BatchNotes       
 (BatchID,NoteID,BatchNoteStatusID,CLM_UNIT,CLM_BilledAmount,CLP03_TotalClaimChargeAmount,Original_Unit,Original_Amount,IsFirstTimeClaimInBatch,        
 Submitted_ClaimAdjustmentTypeID,Original_ClaimSubmitterIdentifier,Original_PayerClaimControlNumber,ClaimAdjustmentReason,IsUseInBilling,IsNewProcess)        
 SELECT        
 @NewBatchID as BatchID,N.NoteID,@BatchNoteStatusID,        
    TempCalculatedUnit=       
  CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)         
  THEN PSM.BillingUnitLimit       
  ELSE SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit)) END ,         
    TempCalculatedAmount=       
  CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)         
        THEN CONVERT( DECIMAL(10,2), (SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)) / SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))) * PSM.BillingUnitLimit )                
  ELSE CONVERT(DECIMAL(10,2), SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount))) END,          
    TempCalculatedAmount=       
  CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)         
        THEN CONVERT( DECIMAL(10,2), (SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)) / SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))) * PSM.BillingUnitLimit )        
        ELSE CONVERT(DECIMAL(10,2), SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount))) END,          
    Original_Unit=N.CalculatedUnit,Original_Amount=N.CalculatedAmount,        
    1,Submitted_ClaimAdjustmentTypeID,Original_ClaimSubmitterIdentifier, Original_PayerClaimControlNumber,ClaimAdjustmentReason,        
     CASE WHEN N.ParentID IS NULL THEN 1 ELSE 0 END,1        
    FROM Notes N         
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
    LEFT JOIN ChildNotes CN ON CN.ParentNoteID=N.NoteID        
    INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID        
    WHERE       
 N.IsBillable=1  AND N.MarkAsComplete=1 AND N.IsDeleted=0  AND N.GroupID IS NOT NULL      
    AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate)))         
    AND N.PayorID=@PayorID       
 AND N.ReferralID in (SELECT val FROM GETCSVTABLE(@ReferralsIds))        
    AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))          
    GROUP BY N.NoteID,N.ParentID,CN.ParentNoteID,Submitted_ClaimAdjustmentTypeID,Original_ClaimSubmitterIdentifier,Original_PayerClaimControlNumber,ClaimAdjustmentReason,PSM.BillingUnitLimit,N.CalculatedAmount,N.CalculatedUnit        
    ORDER BY N.NoteID        
  END        
END