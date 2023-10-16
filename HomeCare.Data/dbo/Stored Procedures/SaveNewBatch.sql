-- EXEC SaveNewBatch @BatchID = '0', @BatchTypeID = '1', @PayorID = '1', @ApprovedFacilityIds = '', @StartDate = '2017/05/01', @EndDate = '2017/05/05', @CreatedBy = '1', @BatchNoteStatusID = '1', @ListBatchNoteStatusID = '1,2,3,4,5,6'
CREATE procedure [dbo].[SaveNewBatch]                                  
@BatchID  bigint,                                     
@BatchTypeID bigint,                                     
@PayorID bigint,                                    
@ApprovedFacilityIds VARCHAR(500),                                    
@StartDate date,                                          
@EndDate date,                                    
@CreatedBy bigint,                              
@BatchNoteStatusID bigint,            
@ListBatchNoteStatusID varchar(500),
@Comment VARCHAR(MAX)            
AS 

BEGIN


                                                   
  SET NOCOUNT ON;                
  
  declare @NewBatchID bigint                                  
  --Insert Batch Table Record
  INSERT INTO Batches(BatchTypeID,PayorID,StartDate,EndDate,IsDeleted,IsSentBy,IsSent,SentDate,CreatedBy,CreatedDate,                                  
  UpdatedDate,UpdatedBy,SystemID,Comment)                                  
  values(@BatchTypeID,@PayorID,@StartDate,@EndDate,0,NULL,0,NULL,@CreatedBy,getutcdate(),getutcdate(),@CreatedBy,1,@Comment)                                  
  SET @NewBatchID=SCOPE_IDENTITY()                                   
                                  
 --Insert FailityApprovedID
 
 IF(LEN(@ApprovedFacilityIds)=0)
    SELECT @ApprovedFacilityIds=SUBSTRING((SELECT ',' + Convert(varchar(max),FacilityID) FROM Facilities WHERE ParentFacilityID=0 AND IsDeleted=0 ORDER BY FacilityName FOR XML PATH('')),2,200000)     
 
 IF(LEN(@ApprovedFacilityIds)>0)                              
  BEGIN
   INSERT INTO BatchApprovedFacility(BillingProviderID,PayorID,BatchID)  (SELECT val,@PayorID,@NewBatchID FROM GETCSVTABLE(@ApprovedFacilityIds))                                  
  END
 --ELSE 
 -- BEGIN
 -- INSERT INTO BatchApprovedFacility(BillingProviderID,PayorID,BatchID) -- (SELECT val,@PayorID,@NewBatchID FROM GETCSVTABLE(@ApprovedFacilityIds))                                  
 --  (SELECT FacilityID as BillingProviderID, @PayorID,@NewBatchID From  Facilities WHERE ParentFacilityID=0 AND IsDeleted=0)
 -- END
                                    
  ---Insert Batch notes     
  IF(@BatchTypeID=1)  -- 1: Initial Submission
  BEGIN
   INSERT INTO BatchNotes (BatchID,NoteID,BatchNoteStatusID,CLM_BilledAmount,SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount,IsFirstTimeClaimInBatch,
   Submitted_ClaimSubmitterIdentifier,Submitted_ClaimAdjustmentTypeID,Original_ClaimSubmitterIdentifier,Original_PayerClaimControlNumber,ClaimAdjustmentReason)

				SELECT  @NewBatchID as BatchID,N.NoteID,@BatchNoteStatusID,N.CalculatedAmount,N.CalculatedAmount,1,
				NULL, 'Original', NULL, NULL, NULL
				FROM Notes N WHERE N.IsBillable=1  AND N.MarkAsComplete=1 AND N.IsDeleted=0 
				AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate))) 
				AND PayorID=@PayorID AND BillingProviderID in(select val from GETCSVTABLE(@ApprovedFacilityIds))   
				AND N.NoteID NOT IN (SELECT NoteID FROM BatchNotes)  
				--AND N.NoteID NOT IN (SELECT NoteID FROM BatchNotes WHERE BatchID IN (20,35) )  



  END

  ELSE IF (@BatchTypeID=2)     -- 2: Denial Re-Submission
  BEGIN
	 INSERT INTO BatchNotes (BatchID,NoteID,BatchNoteStatusID,CLM_BilledAmount,SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount,IsFirstTimeClaimInBatch,
	 Submitted_ClaimSubmitterIdentifier,Submitted_ClaimAdjustmentTypeID,Original_ClaimSubmitterIdentifier,Original_PayerClaimControlNumber,ClaimAdjustmentReason)

				SELECT  @NewBatchID as BatchID,N.NoteID,@BatchNoteStatusID,N.CalculatedAmount,N.CalculatedAmount,1,
				NULL, 'Denial', Original_ClaimSubmitterIdentifier, Original_PayerClaimControlNumber,ClaimAdjustmentReason
				FROM Notes N 
				INNER JOIN (
				 
				 SELECT t.NoteID,t.CLP01_ClaimSubmitterIdentifier AS Original_ClaimSubmitterIdentifier, t.CLP07_PayerClaimControlNumber AS Original_PayerClaimControlNumber, 
				 t.ClaimAdjustmentTypeID AS Submitted_ClaimAdjustmentTypeID , t.ClaimAdjustmentReason
				 FROM
					  (SELECT DISTINCT ROW_NUMBER() OVER ( PARTITION BY BN.BatchID, BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber, NoteID,CLP02_ClaimStatusCode,CLP01_ClaimSubmitterIdentifier,
					   CLP07_PayerClaimControlNumber, ClaimAdjustmentTypeID,ClaimAdjustmentReason FROM BatchNotes BN 
					  ) AS t --WHERE RowNumber=1 AND CLP02_ClaimStatusCode IS NOT NULL AND CLP02_ClaimStatusCode IN (4,22) AND ClaimAdjustmentTypeID IS NULL
					   LEFT JOIN 	BatchNotes BN1 ON   t.CLP07_PayerClaimControlNumber=BN1.Original_PayerClaimControlNumber
					   WHERE RowNumber=1 AND t.CLP02_ClaimStatusCode IS NOT NULL AND t.CLP02_ClaimStatusCode IN (4,22) AND t.ClaimAdjustmentTypeID IS NULL AND BN1.Original_PayerClaimControlNumber IS NULL

				) BND ON BND.NoteID=N.NoteID
				
				WHERE N.IsBillable=1  AND N.MarkAsComplete=1 AND N.IsDeleted=0 
				AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate))) 
				AND PayorID=@PayorID AND BillingProviderID in(select val from GETCSVTABLE(@ApprovedFacilityIds))   
				--AND N.NoteID IN (
				--					SELECT NoteID,CLP07_PayerClaimControlNumber FROM
				--					(SELECT DISTINCT ROW_NUMBER() OVER ( PARTITION BY BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber, NoteID, CLP02_ClaimStatusCode,ClaimAdjustmentTypeID, CLP07_PayerClaimControlNumber FROM BatchNotes BN 
				--					) AS t WHERE RowNumber=1 AND CLP02_ClaimStatusCode IS NOT NULL AND CLP02_ClaimStatusCode IN (4,22) AND ClaimAdjustmentTypeID IS NULL

				--				 )   --4: Denied && 22:Reversal of Previous Payment
  END

  ELSE IF (@BatchTypeID=3)  -- 3: Adjustment(Void/Replace) Submission
  BEGIN
	 INSERT INTO BatchNotes (BatchID,NoteID,BatchNoteStatusID,CLM_BilledAmount,SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount,IsFirstTimeClaimInBatch,
	 Submitted_ClaimSubmitterIdentifier,Submitted_ClaimAdjustmentTypeID,Original_ClaimSubmitterIdentifier,Original_PayerClaimControlNumber,ClaimAdjustmentReason)

				SELECT  @NewBatchID as BatchID,N.NoteID,@BatchNoteStatusID,N.CalculatedAmount,N.CalculatedAmount,1,
				NULL, Submitted_ClaimAdjustmentTypeID,Original_ClaimSubmitterIdentifier, Original_PayerClaimControlNumber,ClaimAdjustmentReason
				FROM Notes N 
				INNER JOIN (
				 
				 SELECT t.NoteID,t.CLP01_ClaimSubmitterIdentifier AS Original_ClaimSubmitterIdentifier, t.CLP07_PayerClaimControlNumber AS Original_PayerClaimControlNumber,t.ClaimAdjustmentTypeID AS Submitted_ClaimAdjustmentTypeID ,
				 t.ClaimAdjustmentReason
				 FROM
					  (SELECT DISTINCT ROW_NUMBER() OVER ( PARTITION BY BN.BatchID, BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber, NoteID,CLP02_ClaimStatusCode,CLP01_ClaimSubmitterIdentifier, 
					   CLP07_PayerClaimControlNumber, ClaimAdjustmentTypeID , ClaimAdjustmentReason FROM BatchNotes BN 
					  ) AS t -- WHERE RowNumber=1 AND ClaimAdjustmentTypeID IS NOT NULL
					  LEFT JOIN 	BatchNotes BN1 ON   t.CLP07_PayerClaimControlNumber=BN1.Original_PayerClaimControlNumber
					  WHERE RowNumber=1 AND t.ClaimAdjustmentTypeID IS NOT NULL AND BN1.Original_PayerClaimControlNumber IS NULL

				) BND ON BND.NoteID=N.NoteID

				
				WHERE N.IsBillable=1  AND N.MarkAsComplete=1 AND N.IsDeleted=0 
				AND (((@StartDate is null OR ServiceDate>= @StartDate) AND (@EndDate is null OR ServiceDate <= @EndDate))) 
				AND PayorID=@PayorID AND BillingProviderID in(select val from GETCSVTABLE(@ApprovedFacilityIds))   
				--AND N.NoteID NOT IN (
				--					SELECT NoteID FROM
				--					(SELECT DISTINCT ROW_NUMBER() OVER ( PARTITION BY BN.BatchID, BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber, NoteID,CLP02_ClaimStatusCode,ClaimAdjustmentTypeID FROM BatchNotes BN 
				--					) AS t WHERE RowNumber=1 AND ClaimAdjustmentTypeID IS NOT NULL
				--				 )   -- THis will capture all the Notes into Replacement && Void Status
  END
  ELSE IF (@BatchTypeID=4)  -- 4: Data Validation
   SELECT 1
	 -- WILL DO NOTHING FOR THIS



		----  INSERT Batch Related Notes Details Into BatchNoteDetails Table
		--INSERT INTO BatchNoteDetails
		--SELECT  BN.BatchID,BN.BatchNoteID, N.* FROM NOTES N
		--INNER JOIN BatchNotes BN ON N.NoteID = BN.NoteID
		--LEFT JOIN BatchNoteDetails BND ON BND.BatchID=BN.BatchID AND BND.NoteID = BN.NoteID
		--WHERE ISFirstTimeClaimInBatch =1 AND BND.BatchNoteDetailID IS NULL
		--ORDER BY  BN.BatchID ASC,BN.BatchNoteID ASC

  END