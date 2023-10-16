CREATE PROCEDURE HC_SyncNotes  
@BatchID BIGINT,  
@BatchTypeID BIGINT  
AS  
BEGIN  
  
-- UPDATE NOTES  
-- UPDATE NOTE DX CODES  
  
IF(@BatchTypeID = 1)  
BEGIN   
  
 IF(@BatchID > 0 )  
 BEGIN  
  UPDATE NDT  SET NDT.BatchTypeID = @BatchTypeID , NDT.BatchID = T1.BatchID  
  FROM NoteDXCodeMappings NDT   
  INNER JOIN   
  (  
  SELECT * FROM (  
  SELECT  ND.NOTEID, BN.BatchID   
  ,ROW_NUMBER() OVER ( PARTITION BY BN.BatchID, BN.NoteID ORDER BY BN.BatchNoteID DESC) AS RowNumber      
  FROM NoteDXCodeMappings  ND  
  INNER JOIN BatchNotes BN ON BN.NoteID = ND.NoteID  
  INNER JOIN Batches B ON B.BatchID = BN.BatchID  
  WHERE -- ND.ReferralDXCodeMappingID IS NULL AND   
  B.BatchTypeID =1 AND B.BatchID=@BatchID  
  GROUP BY ND.NoteID,BN.BatchID,BN.BatchNoteID,BN.NoteID   
  --ORDER BY  COUNT(ND.NOTEID)  DESC  
  ) T WHERE RowNumber = 1  
  
  )  AS T1 ON T1.NoteID = NDT.NoteID  
  
 END  
  
   PRINT ('Notes DX Codes Updated for Initial batch')  
  
END  
ELSE IF (@BatchTypeID = 2 OR @BatchTypeID = 3 OR @BatchTypeID = 4)  -- 3: Adjustment(Void/Replace) Submission                                
BEGIN   
  
 -- SYNC NOTES - TODO  
  
 -- SYNC NoteDX Code   
 -- New DX Code will Insert Here  
  
 IF(@BatchID > 0 )  
 BEGIN  
  
  INSERT INTO NoteDXCodeMappings      
  SELECT     
     ReferralDXCodeMappingID = NULL,                    
     N.ReferralID,                    
     N.NoteID,                    
     NDT.DXCodeID,                    
     NDT.DXCodeName,                    
     NDT.DxCodeType,                    
     Precedence =  1,                    
     StartDate = NULL,                    
     EndDate = NULL,                   
     NDT.Description,                    
     NDT.DXCodeWithoutDot,                    
     DxCodeShortName  = NULL,  
     BatchTypeID = @BatchTypeID,  
     BatchID=@BatchID  
  
  FROM BatchNotes BN   
  INNER JOIN  Notes N   ON  N.NoteID = BN.NoteID AND BN.BatchID = @BatchID  
  LEFT JOIN ReferralBillingAuthorizations RBA ON RBA.ReferralBillingAuthorizationID = N.ReferralBillingAuthorizationID    
  LEFT JOIN DXCodes NDT ON  CHARINDEX(',' + CONVERT(NVARCHAR(MAX),NDT.DXCodeID) + ',', ',' + RBA.DxCodeID + ',') > 0    
  WHERE  BN.BatchID = @BatchID  -- N.EmployeeVisitID = @EmployeeVisitID    
  
 END  
  
 PRINT ('Notes DX Codes Updated')  
END  
  
  
/*  
ELSE IF (@BatchTypeID = 2)     -- 2: Denial Re-Submission           
BEGIN   
  PRINT ('No - Notes DX Codes Updated for Denial batch')  
END  
ELSE IF (@BatchTypeID = 3)     -- 3: Adjustment(Void/Replace) Submission  
BEGIN   
  PRINT ('No - Notes DX Codes Updated for Denial batch')  
END  
ELSE  IF (@BatchTypeID = 4)  -- 4: Resend / Data-Validation Submission      
BEGIN   
  PRINT ('No - Notes DX Codes Updated for Denial batch')  
END  
*/  
  
  
  
  
END  
  