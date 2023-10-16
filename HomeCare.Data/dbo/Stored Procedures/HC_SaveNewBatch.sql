CREATE Procedure [dbo].[HC_SaveNewBatch]                                
@BatchID  bigint,                                
@BatchTypeID bigint,                                                                     
@PayorID bigint,                                   
@ServiceCodeIDs VARCHAR(MAX)=null,                              
@ReferralsIds varchar(max)=null,                              
                                                               
@StartDate date,                                                                          
@EndDate date,                                
@Comment VARCHAR(MAX),                                
                                                                  
@CreatedBy bigint,                                                              
@BatchNoteStatusID bigint,                        
@IsDayCare BIT,                        
@IsCaseManagement BIT,    
@ResultRequired bit = 1    
AS                                 
BEGIN                                
                        
IF(@IsCaseManagement = 0 )                         
EXEC Move_Notes_From_Tempporary_To_Permanent @CreatedBy, @IsDayCare , @ReferralsIds, @ResultRequired                     
ELSE                       
BEGIN                
EXEC Move_Notes_From_Tempporary_To_Permanent_CaseManagement  @PayorID, @ServiceCodeIDs,'', @StartDate, @EndDate ,@CreatedBy, ''  ,@ReferralsIds                   
EXEC HC_RefreshAndGroupingNotes @ResultRequired                
END                
                        
                        
 -- Kundan Kumar Rai : 29-05-2020                          
 -- Update for PayorServiceCodeMapping to ReferralBillingAuthorizations                          
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
                                 
 INSERT INTO BatchNotes                               
 (BatchID,NoteID,BatchNoteStatusID,                              
 CLM_UNIT,CLM_BilledAmount,CLP03_TotalClaimChargeAmount,                              
 Original_Unit,Original_Amount,                              
 IsFirstTimeClaimInBatch,Submitted_ClaimAdjustmentTypeID,IsUseInBilling,IsNewProcess)                                
 SELECT                                
 @NewBatchID as BatchID,N.NoteID,@BatchNoteStatusID,                                
    TempCalculatedUnit=                               
  CASE WHEN (RBA.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> RBA.BillingUnitLimit)              
  THEN RBA.BillingUnitLimit ELSE SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit)) END ,                                 
    TempCalculatedAmount=                   
  CASE WHEN (RBA.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> RBA.BillingUnitLimit)                                 
        THEN CONVERT( DECIMAL(10,2), (SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)) / SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))) * RBA.BillingUnitLimit )                                 
        ELSE CONVERT(DECIMAL(10,2), SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount))) END,                                  
    TempCalculatedAmount=                               
 CASE WHEN (RBA.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> RBA.BillingUnitLimit)                                 
        THEN CONVERT( DECIMAL(10,2), (SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)) / SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))) * RBA.BillingUnitLimit )                                 
  ELSE CONVERT(DECIMAL(10,2), SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount))) END,                                  
 Original_Unit=N.CalculatedUnit,Original_Amount=N.CalculatedAmount,                                
    1,'Original', CASE WHEN N.ParentID IS NULL THEN 1 ELSE 0 END,1                                
    FROM Notes N                                 
    LEFT JOIN ChildNotes CN ON CN.ParentNoteID=N.NoteID                          
 --INNER JOIN                          
 --(                          
 --SELECT BillingUnitLimit, PayorServiceCodeMappingID AS ID FROM PayorServiceCodeMapping WHERE PayorServiceCodeMapping.PayorID=@PayorID                          
 -- UNION ALL                          
 --SELECT BillingUnitLimit, ReferralBillingAuthorizationID AS ID FROM ReferralBillingAuthorizations WHERE ReferralBillingAuthorizations.PayorID=@PayorID                          
 --) RBA ON RBA.ID = N.PayorServiceCodeMappingID OR RBA.ID = N.ReferralBillingAuthorizationID                          
                          
    --INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID                                
 INNER JOIN ReferralBillingAuthorizations RBA ON RBA.ReferralBillingAuthorizationID=N.ReferralBillingAuthorizationID and RBA.PayorID=@PayorID                            
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
    GROUP BY N.NoteID, N.ParentID, CN.ParentNoteID, RBA.BillingUnitLimit,N.CalculatedAmount,N.CalculatedUnit                                
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
  CASE WHEN (RBA.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> RBA.BillingUnitLimit)                                 
  THEN RBA.BillingUnitLimit            
  ELSE SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit)) END ,                                 
 TempCalculatedAmount=                               
  CASE WHEN (RBA.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> RBA.BillingUnitLimit)                                 
  THEN CONVERT( DECIMAL(10,2), (SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)) / SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))) * RBA.BillingUnitLimit )                        
  ELSE CONVERT(DECIMAL(10,2), SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)))END ,                                             
 TempCalculatedAmount=                               
  CASE WHEN (RBA.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> RBA.BillingUnitLimit)                                 
  THEN CONVERT( DECIMAL(10,2), (SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)) / SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))) * RBA.BillingUnitLimit )                                         
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
 --INNER JOIN                          
 --(                          
 --SELECT BillingUnitLimit, PayorServiceCodeMappingID AS ID FROM PayorServiceCodeMapping WHERE PayorServiceCodeMapping.PayorID=@PayorID                          
 -- UNION ALL                          
 --SELECT BillingUnitLimit, ReferralBillingAuthorizationID AS ID FROM ReferralBillingAuthorizations WHERE ReferralBillingAuthorizations.PayorID=@PayorID                    
 --) RBA ON RBA.ID = N.PayorServiceCodeMappingID OR RBA.ID = N.ReferralBillingAuthorizationID                          
    --INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID                                
 INNER JOIN ReferralBillingAuthorizations RBA ON RBA.ReferralBillingAuthorizationID=N.ReferralBillingAuthorizationID and RBA.PayorID=@PayorID                          
    WHERE                               
 N.IsBillable=1  AND N.MarkAsComplete=1 AND N.IsDeleted=0  AND N.GroupID IS NOT NULL                              
    AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate)))                                 
    AND N.PayorID=@PayorID                               
 AND N.ReferralID in (SELECT val FROM GETCSVTABLE(@ReferralsIds))                              
    AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))                                     
    GROUP BY N.NoteID,N.ParentID,CN.ParentNoteID,Submitted_ClaimAdjustmentTypeID,Original_ClaimSubmitterIdentifier,Original_PayerClaimControlNumber,ClaimAdjustmentReason,RBA.BillingUnitLimit,N.CalculatedAmount,N.CalculatedUnit                           
    ORDER BY N.NoteID                                
                               
 END                                
                                
  ELSE IF (@BatchTypeID=3)  -- 3: Adjustment(Void/Replace) Submission                                
  BEGIN                                
                              
 INSERT INTO BatchNotes                               
 (BatchID,NoteID,BatchNoteStatusID,CLM_UNIT,CLM_BilledAmount,CLP03_TotalClaimChargeAmount,Original_Unit,Original_Amount,IsFirstTimeClaimInBatch,                                
 Submitted_ClaimAdjustmentTypeID,Original_ClaimSubmitterIdentifier,Original_PayerClaimControlNumber,ClaimAdjustmentReason,IsUseInBilling,IsNewProcess)      
       
 SELECT  @NewBatchID as BatchID,N.NoteID,@BatchNoteStatusID,                
 TempCalculatedUnit= CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)                 
         THEN PSM.BillingUnitLimit ELSE SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit)) END ,                 
 TempCalculatedAmount= CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)                 
         THEN CONVERT( DECIMAL(10,2), (SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)) / SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))) * PSM.BillingUnitLimit )      
   ELSE CONVERT(DECIMAL(10,2), SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount))) END,                  
 TempCalculatedAmount= CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)                 
          THEN CONVERT( DECIMAL(10,2), (SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)) / SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))) * PSM.BillingUnitLimit )        
    ELSE CONVERT(DECIMAL(10,2), SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount))) END,                  
                
 Original_Unit=N.CalculatedUnit, Original_Amount=N.CalculatedAmount,1,Submitted_ClaimAdjustmentTypeID,Original_ClaimSubmitterIdentifier,      
 Original_PayerClaimControlNumber,ClaimAdjustmentReason,CASE WHEN N.ParentID IS NULL THEN 1 ELSE 0 END,1                
      
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
       t.ClaimAdjustmentTypeID NOT IN ('Write-Off','Denial','Data-Validation', 'Resend','Payor-Change','Other')                
       )                 
       AND BN1.Original_PayerClaimControlNumber IS NULL                
                
    ) BND ON BND.NoteID=N.NoteID                
    LEFT JOIN ChildNotes CN ON CN.ParentNoteID=N.NoteID                
    --INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID                
 INNER JOIN ReferralBillingAuthorizations PSM ON PSM.ReferralBillingAuthorizationID=N.ReferralBillingAuthorizationID and PSM.PayorID=@PayorID       
    WHERE N.IsBillable=1  AND N.MarkAsComplete=1 AND N.IsDeleted=0  AND N.GroupID IS NOT NULL --AND N.ParentID IS NULL                
 AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))          
    AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate)))                 
    AND N.PayorID=@PayorID AND N.ReferralID in (SELECT val FROM GETCSVTABLE(@ReferralsIds))                       
    GROUP BY N.NoteID,N.ParentID,CN.ParentNoteID,Submitted_ClaimAdjustmentTypeID,Original_ClaimSubmitterIdentifier,Original_PayerClaimControlNumber,ClaimAdjustmentReason,PSM.BillingUnitLimit,N.CalculatedAmount,N.CalculatedUnit                
    ORDER BY N.NoteID         
      
      
  END                                
         
         
      
      
ELSE  IF (@BatchTypeID = 4)  -- 4: Resend / Data-Validation Submission      
BEGIN      
       
      
 DECLARE @TempTable TABLE (BatchNoteID BIGINT, NoteID BIGINT);          
 INSERT INTO @TempTable          
 SELECT DISTINCT BatchNoteID, NoteID FROM (          
     SELECT BatchNoteID,BN.NoteID,CLP02_ClaimStatusCode,ParentBatchNoteID,IsFirstTimeClaimInBatch,IsUseInBilling,          
      ROW_NUMBER() OVER ( PARTITION BY BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber          
     FROM BatchNotes BN          
     --WHERE BN.IsDeleted=0 AND (ClaimAdjustmentTypeID IN ('Data-Validation'))          
  WHERE (ClaimAdjustmentTypeID IN ('Data-Validation','Resend'))          
          
     ) AS A WHERE RowNumber=1 --AND ParentBatchNoteID IS NULL         
  AND IsFirstTimeClaimInBatch=1 AND IsUseInBilling=1          
          
          
          
          
          
          
        
 IF OBJECT_ID('tempdb.dbo.#TempBatchNotesTable', 'U') IS NOT NULL                    
 DROP TABLE #TempBatchNotesTable;                    
        
          
          
    SELECT  @NewBatchID as BatchID,N.NoteID,@BatchNoteStatusID as BatchNoteStatusID,          
    TempCalculatedUnit= CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)           
                         THEN PSM.BillingUnitLimit ELSE SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit)) END ,           
       CLM_BilledAmount= CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)           
                           THEN CONVERT( DECIMAL(10,2), (SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)) / SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))) * PSM.BillingUnitLimit )           
            ELSE CONVERT(DECIMAL(10,2), SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount))) END,            
    CLP03_TotalClaimChargeAmount= CASE WHEN (PSM.BillingUnitLimit IS NOT NULL AND SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))> PSM.BillingUnitLimit)           
                          THEN CONVERT( DECIMAL(10,2), (SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount)) / SUM(ISNULL(CN.CalculatedUnit,N.CalculatedUnit))) * PSM.BillingUnitLimit )           
           ELSE CONVERT(DECIMAL(10,2), SUM(ISNULL(CN.CalculatedAmount,N.CalculatedAmount))) END,            
    Original_Unit=N.CalculatedUnit,          
    Original_Amount=N.CalculatedAmount,          
              
          
          
    IsFirstTimeClaimInBatch=1,Submitted_ClaimAdjustmentTypeID='Original', IsUseInBilling=CASE WHEN N.ParentID IS NULL THEN 1 ELSE 0 END,IsNewProcess=1          
 INTO #TempBatchNotesTable        
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
       t.ClaimAdjustmentTypeID IN ('Data-Validation','Resend')                
       )                 
       AND BN1.Original_PayerClaimControlNumber IS NULL                
                
    ) BND ON BND.NoteID=N.NoteID     
    
    
    LEFT JOIN ChildNotes CN ON CN.NoteID = N.NoteID  --AND CN.ParentNoteID=N.NoteID        
    --INNER JOIN PayorServiceCodeMapping PSM ON PSM.PayorServiceCodeMappingID=N.PayorServiceCodeMappingID          
   INNER JOIN ReferralBillingAuthorizations PSM ON PSM.ReferralBillingAuthorizationID=N.ReferralBillingAuthorizationID and PSM.PayorID=@PayorID       
    WHERE N.IsBillable=1  AND N.MarkAsComplete=1 AND N.IsDeleted=0 AND N.GroupID IS NOT NULL --AND N.ParentID IS NULL          
    AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate)))           
    AND N.PayorID=@PayorID AND N.ReferralID in (SELECT val FROM GETCSVTABLE(@ReferralsIds))         
    AND (@ServiceCodeIDs is null or @ServiceCodeIDs = '' or (N.ServiceCodeID in (SELECT val FROM GetCSVTable(@ServiceCodeIDs)) ))       
    --AND N.NoteID IN (          
    --   SELECT DISTINCT NoteID FROM @TempTable          
    --)            
    GROUP BY N.NoteID, N.ParentID, CN.ParentNoteID, PSM.BillingUnitLimit,N.CalculatedAmount,N.CalculatedUnit          
    ORDER BY  N.NoteID          
    --AND N.NoteID NOT IN (SELECT NoteID FROM BatchNotes WHERE BatchID IN (20,35) )            
          
          
        
        
  INSERT INTO BatchNotes (BatchID,NoteID,BatchNoteStatusID,CLM_UNIT,CLM_BilledAmount,CLP03_TotalClaimChargeAmount,Original_Unit,Original_Amount,IsFirstTimeClaimInBatch,Submitted_ClaimAdjustmentTypeID,IsUseInBilling,        
  IsNewProcess)        
  SELECT BatchID,NoteID,BatchNoteStatusID,TempCalculatedUnit,CLM_BilledAmount,CLP03_TotalClaimChargeAmount,Original_Unit,Original_Amount,IsFirstTimeClaimInBatch,Submitted_ClaimAdjustmentTypeID,        
  IsUseInBilling,IsNewProcess FROM #TempBatchNotesTable         
          
        
   --UPDATE BN SET BN.IsDeleted=1         
   --FROM BatchNotes BN        
   --INNER JOIN #TempBatchNotesTable TN ON TN.NoteID=BN.NoteID        
   --WHERE BN.BatchNoteID IN (SELECT BatchNoteID FROM @TempTable)        
        
      
END      
          
  
EXEC HC_SyncNotes @NewBatchID,  @BatchTypeID       
  
DELETE FROM Notes_Temporary WHERE CreatedBy= @CreatedBy AND ( @ReferralsIds IS NULL OR LEN(@ReferralsIds)=0 OR ReferralID in (SELECT val FROM GETCSVTABLE(@ReferralsIds)) )       
          
END 