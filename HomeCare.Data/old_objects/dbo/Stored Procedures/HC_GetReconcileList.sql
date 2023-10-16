--EXEC HC_GetReconcileList @@ClaimStatusCodeID = '0', @@Get835ProcessedOnly = '0', @@Upload835FileID = '0', @@ServiceID = '-1', @@ClaimAdjustmentTypeID = '-1', @@NoteID = '', @@PayorClaimNumber = '', @@SortExpression = 'ServiceDate', @@SortType = 'DESC',@@FromIndex = '1', @@PageSize = '50'  
CREATE PROCEDURE [dbo].[HC_GetReconcileList]      
@ReferralID bigint=0,  
 @PayorID bigint =0,                  
 @Batch varchar(200)=null,                  
 @ClaimNumber Varchar(200)=null,                  
 @Client varchar(200)=null,                  
 @ServiceCode varchar(100)=null,                  
 @ServiceStartDate date=null,                  
 @ServiceEndDate date=null,                  
 @ModifierID varchar(100)=null,  
 @PosID bigint=0,                  
 @ClaimStatusCodeID bigint=0,             
 @ReconcileStatus VARCHAR(100)=null,             
 @Upload835FileID bigint=0,                  
 @ClaimAdjustmentGroupCodeID VARCHAR(100)=null,                  
 @ClaimAdjustmentReasonCodeID varchar(100)=null,                  
 @ClaimAdjustmentTypeID VARCHAR(100)=null,                  
 @Get835ProcessedOnly BIGINT =-1,                     
 @ServiceID VARCHAR(500)=null,            
 @NoteID VARCHAR(MAX)=null,            
 @PayorClaimNumber VARCHAR(MAX)=null,                     
 @IsDeleted BIGINT =-1,                     
 @SortExpression VARCHAR(100),                      
 @SortType VARCHAR(10),                    
 @FromIndex INT,                    
 @PageSize INT                   
AS                    
BEGIN                        
 ;WITH CTEReconcile835List AS (      
  SELECT *,COUNT(t1.ReferralID) OVER() AS Count FROM       
  (                    
   SELECT ROW_NUMBER() OVER (ORDER BY      
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Dob' THEN  CONVERT(date, T.Dob, 105) END END ASC,      
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Dob' THEN  CONVERT(date, T.Dob, 105) END END DESC,      
      
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Employee' THEN T.LastName END END ASC,          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Employee' THEN T.LastName END END DESC,      
      
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Patient' THEN T.LastName END END ASC,          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Patient' THEN T.LastName END END DESC,      
      
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CISNumber' THEN T.CISNumber END END ASC,          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CISNumber' THEN T.CISNumber END END DESC,      
      
    CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AHCCCSID' THEN T.AHCCCSID END END ASC,          
    CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AHCCCSID' THEN T.AHCCCSID END END DESC      
          
   ) AS Row,ReferralID,Patient=dbo.GetGeneralNameFormat(FirstName,LastName),AHCCCSID,CISNumber,Dob,BilledAmount,NoteCount,Status,CLP02_ClaimStatusCode,  
   ClaimAdjustmentTypeID  
   FROM(      
   SELECT DISTINCT R.ReferralID,R.FirstName,R.LastName,R.AHCCCSID,R.CISNumber,R.Dob,SUM(N.CalculatedAmount) AS BilledAmount,COUNT(N.NoteID) AS NoteCount,  
   Status=CASE WHEN (Submitted_ClaimAdjustmentTypeID IS NULL OR Submitted_ClaimAdjustmentTypeID NOT IN ('Void')) AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0 AND CSC.ClaimStatusName IS NOT NULL THEN 'Denied'                  
     WHEN Submitted_ClaimAdjustmentTypeID = 'Void' AND Original_PayerClaimControlNumber IS NOT NULL AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0 AND CSC.ClaimStatusName='Reversal of Previous Payment' THEN 'Paid'                
  
    WHEN Submitted_ClaimAdjustmentTypeID = 'Void' AND Original_PayerClaimControlNumber IS NOT NULL AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0 AND CSC.ClaimStatusName!='Reversal of Previous Payment' THEN 'Denied'  
    WHEN CSC.ClaimStatusName IS NULL THEN NULL ELSE  'Paid' END,BN.CLP02_ClaimStatusCode,ClaimAdjustmentTypeID  
   FROM BatchNotes BN       
    INNER JOIN Notes N ON N.NoteID=BN.NoteID    
 INNER JOIN Referrals R ON R.ReferralID=N.ReferralID    
 INNER JOIN ServiceCodes SC ON SC.ServiceCodeID=N.ServiceCodeID      
    LEFT JOIN Batches B on B.BatchID=BN.BatchID AND IsShowOnParentReconcile=1                  
    LEFT JOIN BatchNoteDetails BND ON BND.NoteID=BN.NoteID AND BND.BatchID=BN.BatchID              
    LEFT JOIN ClaimStatusCodes CSC  ON CSC.ClaimStatusCodeID=BN.CLP02_ClaimStatusCode                            
    LEFT JOIN ClaimAdjustmentGroupCodes CAGC  ON CAGC.ClaimAdjustmentGroupCodeID=BN.CAS01_ClaimAdjustmentGroupCode                            
    LEFT JOIN ClaimAdjustmentReasonCodes CARC  ON CARC.ClaimAdjustmentReasonCodeID=BN.CAS02_ClaimAdjustmentReasonCode              
    LEFT JOIN NoteModifierMappings NMM ON NMM.NoteID=BN.NoteID     
   WHERE --EV.IsDeleted=0 AND   
   R.IsDeleted=0  
   AND ((CAST(@IsDeleted AS BIGINT)=-1) OR n.IsDeleted=@IsDeleted)                    
   AND (( CAST(@PayorID AS BIGINT)=0) OR B.PayorID= CAST(@PayorID AS BIGINT))      
   AND ((@Batch IS NULL OR LEN(@Batch)=0) OR B.BatchID = CAST(@Batch AS BIGINT))      
   AND ((@ClaimNumber IS NULL OR LEN(@ClaimNumber)=0) OR BN.CLP01_ClaimSubmitterIdentifier LIKE '%'+@ClaimNumber+'%')      
   AND ((@PayorClaimNumber IS NULL OR LEN(@PayorClaimNumber)=0) OR BN.CLP07_PayerClaimControlNumber LIKE '%'+@PayorClaimNumber+'%')  
   AND (    (@Get835ProcessedOnly=0) OR       
        (@Get835ProcessedOnly=1 AND BN.CLP02_ClaimStatusCode IS NULL) OR      
        (@Get835ProcessedOnly=2 AND BN.CLP02_ClaimStatusCode IS NOT NULL)      
        )     
   AND  (      
     (@Client IS NULL OR LEN(@Client)=0)       
      OR      
     (      
      R.FirstName LIKE '%'+@Client+'%' OR      
      R.LastName  LIKE '%'+@Client+'%' OR      
      R.FirstName +' '+r.LastName like '%'+@Client+'%' OR      
      R.LastName +' '+r.FirstName like '%'+@Client+'%' OR      
      R.FirstName +', '+r.LastName like '%'+@Client+'%' OR      
      R.LastName +', '+r.FirstName like '%'+@Client+'%'      
     )      
    )      
   AND ((@ServiceCode IS NULL OR LEN(@ServiceCode)=0) OR N.ServiceCodeID In (SELECT val FROM GetCSVTable(@ServiceCode))) --LIKE '%'+@ServiceCode+'%')      
   AND ((@ModifierID IS NULL OR LEN(@ModifierID)=0) OR SC.ModifierID like '%'+@ModifierID+'%')   
   AND (( CAST(@PosID AS BIGINT)=0) OR N.PosID= CAST(@PosID AS BIGINT))      
   AND (( CAST(@Upload835FileID AS BIGINT)=0) OR BN.Upload835FileID= CAST(@Upload835FileID AS BIGINT))      
   AND ((@ServiceStartDate is null OR N.ServiceDate>= @ServiceStartDate) and (@ServiceEndDate  is null OR N.ServiceDate<= @ServiceEndDate))      
   --AND(  (CAST(@ClaimStatusCodeID AS BIGINT)=-2 OR @ClaimStatusCodeID IS NULL)  OR        
   --(CAST(@ClaimStatusCodeID AS BIGINT)=0 OR BN.CLP02_ClaimStatusCode= CAST(@ClaimStatusCodeID  AS BIGINT) ))      
   AND ((@ClaimAdjustmentGroupCodeID IS NULL OR LEN(@ClaimAdjustmentGroupCodeID)=0) OR BN.CAS01_ClaimAdjustmentGroupCode LIKE '%'+@ClaimAdjustmentGroupCodeID+'%')      
   AND ((@ClaimAdjustmentReasonCodeID IS NULL OR LEN(@ClaimAdjustmentReasonCodeID)=0) OR BN.CAS02_ClaimAdjustmentReasonCode = @ClaimAdjustmentReasonCodeID)      
   AND ((@NoteID IS NULL OR LEN(@NoteID)=0) OR BN.NoteID = CAST(@NoteID AS BIGINT))      
   GROUP BY R.ReferralID,R.FirstName,R.LastName,R.AHCCCSID,R.CISNumber,R.Dob,  
   Submitted_ClaimAdjustmentTypeID,CLP04_TotalClaimPaymentAmount,ClaimStatusName,Original_PayerClaimControlNumber,CLP02_ClaimStatusCode,ClaimAdjustmentTypeID  
   ) AS T  
  
   WHERE         
         
    (      
    (@ReconcileStatus = '-2' AND Status IS NULL) OR      
    (@ReconcileStatus IS NULL OR LEN(@ReconcileStatus)=0 OR Status=@ReconcileStatus)      
        
  )      
   AND      
   (  (CAST(@ClaimStatusCodeID AS BIGINT)=-2 AND t.CLP02_ClaimStatusCode IS NULL)  OR        
    (CAST(@ClaimStatusCodeID AS BIGINT)=0 OR t.CLP02_ClaimStatusCode= CAST(@ClaimStatusCodeID  AS BIGINT) ))        
 AND ( (@ClaimAdjustmentTypeID IS NULL OR LEN(@ClaimAdjustmentTypeID)=0 OR @ClaimAdjustmentTypeID='-1') OR ClaimAdjustmentTypeID=@ClaimAdjustmentTypeID)    
  
)AS T1      
 )       
 SELECT * FROM CTEReconcile835List WHERE ROW BETWEEN ((@PageSize*(@FromIndex-1))+1) AND (@PageSize*@FromIndex)      
END
