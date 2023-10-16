
  
CREATE Procedure [dbo].[HC_ExportReconcileList]        
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
@ClaimAdjustmentGroupCodeID varchar(100)=null,                  
@ClaimAdjustmentReasonCodeID varchar(100)=null,                  
@ClaimAdjustmentTypeID VARCHAR(100)=null,               
@Get835ProcessedOnly BIGINT =-1,                     
@ServiceID VARCHAR(500),                 
@NoteID VARCHAR(MAX),            
@PayorClaimNumber VARCHAR(MAX),                
@IsDeleted BIGINT =-1,                     
@SortExpression VARCHAR(100),                      
@SortType VARCHAR(10),                    
@FromIndex INT,                    
@PageSize INT                  
AS            
BEGIN            
    DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()         
SELECT *,COUNT(t1.NoteID) OVER() AS Count FROM                     
  (                    
  SELECT ROW_NUMBER() OVER (ORDER BY                     
                         
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Status' THEN Status END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Status' THEN Status END END DESC ,                    
          
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ClaimStatus' THEN ClaimStatus END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ClaimStatus' THEN ClaimStatus END END DESC ,                    
                         
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ClaimNumber' THEN ClaimNumber END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ClaimNumber' THEN ClaimNumber END END DESC ,                    
                         
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PayorClaimNumber' THEN CONVERT(bigint,PayorClaimNumber) END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PayorClaimNumber' THEN  CONVERT(bigint,PayorClaimNumber) END END DESC ,                    
                         
 CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'S277' THEN S277 END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'S277' THEN S277 END END DESC ,                    
            
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'S277CA' THEN S277CA END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'S277CA' THEN S277CA END END DESC ,             
            
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'BillingProviderNPI' THEN BillingProviderNPI END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'BillingProviderNPI' THEN BillingProviderNPI END END DESC ,                    
                         
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'BillingProvider' THEN BillingProvider END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'BillingProvider' THEN BillingProvider END END DESC ,                    
                         
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ClientName' THEN ClientName END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ClientName' THEN ClientName END END DESC ,                    
                         
CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ClientDob' THEN CONVERT(datetime, ClientDob, 103) END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ClientDob' THEN CONVERT(datetime, ClientDob, 103) END END DESC,                    
                         
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AHCCCSID' THEN AHCCCSID END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AHCCCSID' THEN AHCCCSID END END DESC ,                    
                         
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CISNumber' THEN CISNumber END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CISNumber' THEN CISNumber END END DESC ,                    
                         
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Population' THEN Population END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Population' THEN Population END END DESC ,                    
               
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Title' THEN Title END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Title' THEN Title END END DESC ,                    
                    
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ServiceDate' THEN CONVERT(datetime, ServiceDate, 103) END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ServiceDate' THEN CONVERT(datetime, ServiceDate, 103) END END DESC,                    
                    
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'ServiceCode' THEN ServiceCode END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'ServiceCode' THEN ServiceCode END END DESC ,                    
                    
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'Modifier' THEN Modifier END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'Modifier' THEN Modifier END END DESC ,                    
                    
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PosID' THEN CONVERT(bigint,POS) END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PosID' THEN CONVERT(bigint,POS) END END DESC ,                    
                    
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'CalculatedUnit' THEN CONVERT(bigint,CalculatedUnit)  END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'CalculatedUnit' THEN CONVERT(bigint,CalculatedUnit) END END DESC ,                    
            
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AdjustmentCode' THEN ClaimAdjustmentGroupCodeID END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AdjustmentCode' THEN ClaimAdjustmentGroupCodeID END END DESC ,                    
                    
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'BilledAmount' THEN CONVERT(decimal,BilledAmount) END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'BilledAmount' THEN CONVERT(decimal,BilledAmount) END END DESC ,                    
                    
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'AllowedAmount' THEN CONVERT(decimal,AllowedAmount) END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'AllowedAmount' THEN CONVERT(decimal,AllowedAmount) END END DESC ,                    
                    
     CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'PaidAmount' THEN CONVERT(decimal,PaidAmount) END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'PaidAmount' THEN CONVERT(decimal,PaidAmount) END END DESC ,                    
                    
                    
  CASE WHEN @SortType = 'ASC' THEN CASE WHEN @SortExpression = 'LoadDate' THEN CONVERT(datetime, LoadDate, 103) END END ASC,                    
     CASE WHEN @SortType = 'DESC' THEN CASE WHEN @SortExpression = 'LoadDate' THEN CONVERT(datetime, LoadDate, 103) END END DESC --,                    
                 
  ) AS Row,S277,S277CA,            
            
   ClientName,AHCCCSID, CISNumber,ClientDob,Gender,Population,Title,Payor,            
   BatchNoteID,BatchID,BatchTypeName,BatchStartDate,BatchEndDate,GatheredDate,SendDate, CONVERT(datetime,ServiceDate,1) AS ServiceDate,            
   GatheredBy,SentBy,BillingProvider,BillingProviderNPI,RenderingProvider,RenderingProviderNPI,            
   NoteID,ServiceCode,Description,POS,POSDetail,NoteDetails,Assessment,ActionPlan,            
   Modifier,Status,ClaimStatus,ClaimNumber,PayorClaimNumber,CalculatedUnit,BilledAmount,AllowedAmount,PaidAmount,             
   ClaimStatusCodeID,ClaimAdjustmentGroupCodeID,ClaimAdjustmentGroupCodeName,ClaimAdjustmentReasonCodeID,ClaimAdjustmentReasonDescription,ClaimAdjustmentType,            
   LoadDate,ReceivedDate,ProcessedDate,ExtractDate,      
   DXCode,CheckDate,CheckNumber,CheckAmount,ProgrameName,NoteCreatedBy,NoteSigned,NoteSignedDate,      
   RowNumber FROM ( SELECT DISTINCT BN.CLP02_ClaimStatusCode,BN.S277,BN.S277CA,            
    ClientName= dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),ClientDob= R.Dob,R.Gender, R.AHCCCSID, R.CISNumber, R.Population, R.Title,Payor=BND.PayorShortName,                
    BN.BatchNoteID,BN.BatchID,BT.BatchTypeName,BillingProvider=BND.BillingProviderName,BND.BillingProviderNPI,RenderingProvider=BND.RenderingProviderName,BND.RenderingProviderNPI,      
    B.StartDate AS BatchStartDate,B.EndDate AS BatchEndDate, B.CreatedDate AS GatheredDate, B.SentDate AS SendDate,            
    EG.UserName AS GatheredBy,ES.UserName AS SentBy,BND.NoteID,BND.ServiceDate,BND.ServiceCode,BND.Description,BND.PosID AS POS,BND.POSDetail,      
 dbo.udf_StripHTML(BND.NoteDetails) AS NoteDetails,dbo.udf_StripHTML(BND.Assessment) AS Assessment,dbo.udf_StripHTML(BND.ActionPlan) AS ActionPlan,            
    STUFF(                    
      (SELECT ', ' + convert(varchar(100), M.ModifierCode, 120)                    
      FROM Modifiers M                    
      where M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))                    
      FOR XML PATH (''))                    
      , 1, 1, '')  AS Modifier,            
  Status=CASE WHEN (Submitted_ClaimAdjustmentTypeID IS NULL OR Submitted_ClaimAdjustmentTypeID NOT IN ('Void')) AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0 AND CSC.ClaimStatusName IS NOT NULL THEN 'Denied'            
      WHEN Submitted_ClaimAdjustmentTypeID = 'Void' AND Original_PayerClaimControlNumber IS NOT NULL AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0 AND CSC.ClaimStatusName='Reversal of Previous Payment' THEN 'Paid'            
   WHEN Submitted_ClaimAdjustmentTypeID = 'Void' AND Original_PayerClaimControlNumber IS NOT NULL AND CONVERT(DECIMAL(18,2),ISNULL(BN.CLP04_TotalClaimPaymentAmount,0))= 0 AND CSC.ClaimStatusName!='Reversal of Previous Payment' THEN 'Denied'            
   WHEN CSC.ClaimStatusName IS NULL THEN NULL ELSE  'Paid' END,          
          
 ClaimStatus= CSC.ClaimStatusName,          
          
    ClaimNumber=BN.CLP01_ClaimSubmitterIdentifier,PayorClaimNumber=BN.CLP07_PayerClaimControlNumber,            
    --N.CalculatedUnit,            
               
            
 --BilledAmount=CASE WHEN BN.CLP03_TotalClaimChargeAmount IS NULL OR LEN(BN.CLP03_TotalClaimChargeAmount)=0 THEN '0' ELSE BN.CLP03_TotalClaimChargeAmount END,          
 CalculatedUnit=BN.CLM_UNIT,          
 BilledAmount=CASE WHEN BN.CLM_BilledAmount IS NULL OR LEN(BN.CLM_BilledAmount)=0 THEN '0' ELSE BN.CLM_BilledAmount END,          
 AllowedAmount=CASE WHEN BN.AMT01_ServiceLineAllowedAmount_AllowedAmount IS NULL OR LEN(BN.AMT01_ServiceLineAllowedAmount_AllowedAmount)=0 THEN '0' ELSE BN.AMT01_ServiceLineAllowedAmount_AllowedAmount END,           
 PaidAmount=CASE WHEN BN.CLP04_TotalClaimPaymentAmount IS NULL OR LEN(BN.CLP04_TotalClaimPaymentAmount)=0 THEN '0' ELSE BN.CLP04_TotalClaimPaymentAmount END,          
                
    CSC.ClaimStatusCodeID, CAGC.ClaimAdjustmentGroupCodeID,CAGC.ClaimAdjustmentGroupCodeName,CARC.ClaimAdjustmentReasonCodeID,CARC.ClaimAdjustmentReasonDescription,ClaimAdjustmentTypeID AS ClaimAdjustmentType,             
            
    BN.LoadDate,BN.ReceivedDate,BN.ProcessedDate,ExtractDate=B.CreatedDate, BN.CheckDate,BN.CheckNumber,            
                
    CheckAmount=CONVERT(DECIMAL(18,2),ISNULL(NULLIF(BN.CheckAmount, ''), NULL)),            
            
    (SELECT  STUFF((SELECT TOP 1 ', ' + F.DXCodeWithoutDot            
    FROM NoteDXCodeMappings F where F.ReferralID=BND.ReferralID   Order BY F.Precedence            
    FOR XML PATH('')),1,1,'')) AS DXCode,            
            
    ProgrameName=BND.ZarephathService, EC.UserName AS NoteCreatedBy,            
            
       CASE WHEN BND.MarkAsComplete=1 THEN 'Yes' ELSE 'No' END AS NoteSigned,            
    BND.SignatureDate AS NoteSignedDate,            
                
    ROW_NUMBER() OVER ( PARTITION BY BN.NoteID Order BY BN.BatchID DESC, MarkAsLatest DESC, BN.BatchNoteID Desc) AS RowNumber            
    FROM BatchNotes BN                   
    INNER JOIN Batches B on B.BatchID=BN.BatchID AND B.IsSent=1 --AND IsShowOnParentReconcile=1 AND BN.IsUseInBilling=1      
    INNER JOIN BatchTypes BT on BT.BatchTypeID=B.BatchTypeID      
 INNER JOIN Notes N ON N.NoteID=BN.NoteID      
 INNER JOIN ServiceCodes SC ON SC.ServiceCodeID=N.ServiceCodeID      
    INNER JOIN Employees EG on EG.EmployeeID=B.CreatedBy            
    LEFT JOIN Employees ES on ES.EmployeeID=B.IsSentBy            
    INNER JOIN BatchNoteDetails BND ON BND.NoteID=BN.NoteID AND BND.BatchID=BN.BatchID               
    LEFT JOIN Employees EC on EC.EmployeeID=BND.CreatedBy            
    LEFT JOIN ClaimStatusCodes CSC  ON CSC.ClaimStatusCodeID=BN.CLP02_ClaimStatusCode                    
    LEFT JOIN ClaimAdjustmentGroupCodes CAGC  ON CAGC.ClaimAdjustmentGroupCodeID=BN.CAS01_ClaimAdjustmentGroupCode                    
    LEFT JOIN ClaimAdjustmentReasonCodes CARC  ON CARC.ClaimAdjustmentReasonCodeID=BN.CAS02_ClaimAdjustmentReasonCode                    
    INNER JOIN Referrals R ON R.ReferralID=N.ReferralID                    
    LEFT JOIN Modifiers MD ON MD.ModifierID=BND.ModifierID                   
    WHERE 1=1 --AND BN.ParentBatchNoteID IS NULL                     
       --AND N.IsBillable=1             
       AND ((CAST(@IsDeleted AS BIGINT)=-1) OR BND.IsDeleted=@IsDeleted)                    
       AND (( CAST(@PayorID AS BIGINT)=0) OR B.PayorID= CAST(@PayorID AS BIGINT))                      
       AND ((@Batch IS NULL OR LEN(@Batch)=0) OR B.BatchID = CAST(@Batch AS BIGINT))                   
       AND ((@ClaimNumber IS NULL OR LEN(@ClaimNumber)=0) OR BN.CLP01_ClaimSubmitterIdentifier LIKE '%'+@ClaimNumber+'%')                      
       AND  ((@Client IS NULL OR LEN(@Client)=0) OR  (                  
                  R.FirstName LIKE '%'+@Client+'%' OR                  
                  R.LastName  LIKE '%'+@Client+'%' OR                  
                  R.FirstName +' '+r.LastName like '%'+@Client+'%' OR                  
                  R.LastName +' '+r.FirstName like '%'+@Client+'%' OR                  
                  R.FirstName +', '+r.LastName like '%'+@Client+'%' OR                  
   R.LastName +', '+r.FirstName like '%'+@Client+'%'                  
                 ))                  
       AND ((@ServiceCode IS NULL OR LEN(@ServiceCode)=0) OR BND.ServiceCodeID In (SELECT val FROM GetCSVTable(@ServiceCode)))          
    --AND ((@ServiceCode IS NULL OR LEN(@ServiceCode)=0) OR N.ServiceCode LIKE '%'+@ServiceCode+'%')                      
       AND ((@ModifierID IS NULL OR LEN(@ModifierID)=0) OR SC.ModifierID like '%'+@ModifierID+'%')        
       AND (( CAST(@PosID AS BIGINT)=0) OR BND.PosID= CAST(@PosID AS BIGINT))                       
       AND (( CAST(@Upload835FileID AS BIGINT)=0) OR BN.Upload835FileID= CAST(@Upload835FileID AS BIGINT))                       
           
       AND ((@ServiceStartDate is null OR BND.ServiceDate>= @ServiceStartDate) and (@ServiceEndDate  is null OR BND.ServiceDate<= @ServiceEndDate))                      
    --    AND(  (CAST(@ClaimStatusCodeID AS BIGINT)=-2 AND BN.CLP02_ClaimStatusCode IS NULL)  OR            
    --(CAST(@ClaimStatusCodeID AS BIGINT)=0 OR BN.CLP02_ClaimStatusCode= CAST(@ClaimStatusCodeID  AS BIGINT) ))                  
       AND ((@ClaimAdjustmentGroupCodeID IS NULL OR LEN(@ClaimAdjustmentGroupCodeID)=0) OR BN.CAS01_ClaimAdjustmentGroupCode LIKE '%'+@ClaimAdjustmentGroupCodeID+'%')                      
       AND ((@ClaimAdjustmentReasonCodeID IS NULL OR LEN(@ClaimAdjustmentReasonCodeID)=0) OR BN.CAS02_ClaimAdjustmentReasonCode = @ClaimAdjustmentReasonCodeID)           AND ((@NoteID IS NULL OR LEN(@NoteID)=0) OR BN.NoteID = CAST(@NoteID           
          
AS BIGINT))                 
    AND ((@PayorClaimNumber IS NULL OR LEN(@PayorClaimNumber)=0) OR BN.CLP07_PayerClaimControlNumber LIKE '%'+@PayorClaimNumber+'%')            
              
       AND (    (@Get835ProcessedOnly=0) OR             
        (@Get835ProcessedOnly=1 AND BN.CLP02_ClaimStatusCode IS NULL) OR            
        (@Get835ProcessedOnly=2 AND BN.CLP02_ClaimStatusCode IS NOT NULL)            
     )              
         AND ( (@ServiceID='-1') OR ( @ServiceID  = BND.ZarephathService)  )             
             
       --AND (( CAST(@ServiceID AS bigint)=-1)                        
       -- OR (CAST(@ServiceID AS bigint) = 0 and r.RespiteService = 1)                         
       -- OR (CAST(@ServiceID AS bigint) = 1 and r.LifeSkillsService = 1)                        
       -- OR (CAST(@ServiceID AS bigint) = 2 and r.CounselingService = 1)            
       -- OR (CAST(@ServiceID AS bigint) = 3 and r.ConnectingFamiliesService = 1))                          
            
   ) AS t  WHERE   (          
  (@ReconcileStatus = '-2' AND Status IS NULL) OR          
  (@ReconcileStatus IS NULL OR LEN(@ReconcileStatus)=0 OR Status=@ReconcileStatus)          
            
  )  AND          
   (  (CAST(@ClaimStatusCodeID AS BIGINT)=-2 AND t.CLP02_ClaimStatusCode IS NULL)  OR            
    (CAST(@ClaimStatusCodeID AS BIGINT)=0 OR t.CLP02_ClaimStatusCode= CAST(@ClaimStatusCodeID  AS BIGINT) ))            
 --AND RowNumber=1       
 AND ( (@ClaimAdjustmentTypeID IS NULL OR LEN(@ClaimAdjustmentTypeID)=0 OR @ClaimAdjustmentTypeID='-1') OR ClaimAdjustmentType=@ClaimAdjustmentTypeID)  --WHERE RowNumber=1            
  ) AS t1             
            
            
            
END 