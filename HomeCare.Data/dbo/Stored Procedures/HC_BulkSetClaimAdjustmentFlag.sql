CREATE PROCEDURE [dbo].[HC_BulkSetClaimAdjustmentFlag]          
@ClaimAdjustmentType as varchar(50)=null,            
@ClaimAdjustmentReason as varchar(MAX)=null,            
@BatchIDs NVARCHAR(MAX), --'162',      
@ReferralIDs NVARCHAR(MAX)= '',--'41,63',      
@NoteIDs NVARCHAR(MAX)='' --'5504,5506,5510'      
AS            
BEGIN            
            
      
--DECLARE @BatchIDs NVARCHAR(MAX)='162'      
--DECLARE @ReferralIDs NVARCHAR(MAX)= ''--'41,63'      
--DECLARE @NoteIDs NVARCHAR(MAX)=''--'5504,5506,5510'      
      
      
 IF OBJECT_ID(N'tempdb..#TempBatchNotes') IS NOT NULL      
 BEGIN      
 DROP TABLE #TempBatchNotes      
 END      
      
 SELECT * INTO #TempBatchNotes FROM  (      
 SELECT BN.BatchNoteID, BN.NoteID,BN.BatchID, N.ReferralID,RowNumber = ROW_NUMBER() OVER ( PARTITION BY BN.NoteID,BN.BatchID ORDER BY MarkAsLatest DESC,BN.BatchNoteID DESC)      
 FROM BatchNotes BN      
 INNER JOIN Notes N ON N.NoteID = BN.NoteID      
 ) AS T       
 WHERE RowNumber = 1       
 AND BatchID IN (SELECT val FROM GetCSVTable(@BatchIDs))       
 AND ( LEN(@ReferralIDs)=0 OR ReferralID IN (SELECT val FROM GetCSVTable(@ReferralIDs))  )      
 AND ( LEN(@NoteIDs)=0 OR NoteID IN (SELECT val FROM GetCSVTable(@NoteIDs)) )       
      
      
 --SELECT * FROM #TempBatchNotes      
      
      
      
 Declare @BatchID BIGINT;            
 Declare @NoteID BIGINT;            
            
 DECLARE BulkSetClaimAdjustmen_cursor CURSOR FOR  SELECT BatchID, NoteID FROM #TempBatchNotes      
 OPEN BulkSetClaimAdjustmen_cursor               
 FETCH NEXT FROM BulkSetClaimAdjustmen_cursor INTO @BatchID,@NoteID            
            
 WHILE @@FETCH_STATUS = 0               
 BEGIN               
            
  PRINT '@BatchID = ' + CONVERT(NVARCHAR(MAX),@BatchID) +' AND @NoteID = ' + CONVERT(NVARCHAR(MAX),@NoteID)      
       
  EXEC HC_SetClaimAdjustmentFlag01 @ClaimAdjustmentType,@ClaimAdjustmentReason,@BatchID,@NoteID            
  FETCH NEXT FROM BulkSetClaimAdjustmen_cursor INTO @BatchID,@NoteID            
      
 END               
            
 CLOSE BulkSetClaimAdjustmen_cursor               
 DEALLOCATE BulkSetClaimAdjustmen_cursor            
            
            
END 