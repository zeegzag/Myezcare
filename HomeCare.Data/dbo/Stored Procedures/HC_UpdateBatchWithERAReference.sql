CREATE PROCEDURE HC_UpdateBatchWithERAReference  
@BatchIDs  NVARCHAR(MAX),  
@EraID NVARCHAR(MAX)  
AS  
  
BEGIN  
  
  
DECLARE   
@BatchID BIGINT;  
  
  
DECLARE BatchCursor_db CURSOR  
FOR SELECT val FROM GetCSVTable(@BatchIDs);  
  
OPEN BatchCursor_db;  
  
FETCH NEXT FROM BatchCursor_db INTO  @BatchID;  
  
WHILE @@FETCH_STATUS = 0  
BEGIN  
      
  
 DECLARE @ExistingERA NVARCHAR(MAX);  
 SELECT  @ExistingERA = EraIDs FROM Batches WHERE BatchID= @BatchID;  
  
  
 PRINT  'BatchID: '+CAST(@BatchID AS NVARCHAR(MAX)) +' & ERA: '+CAST(@EraID AS NVARCHAR(MAX));  
 PRINT  '@ExistingERA: '+CAST(@ExistingERA AS NVARCHAR(MAX));  
  
 IF @ExistingERA LIKE '%' + @EraID + '%'  
  PRINT 'ERA is already exist'  
 ELSE  
  BEGIN  
   IF(@ExistingERA IS NULL OR LEN(@ExistingERA)=0)  
    UPDATE Batches SET EraIDs= @EraID WHERE BatchID= @BatchID;  
   ELSE   
    UPDATE Batches SET EraIDs= EraIDs+','+@EraID WHERE BatchID= @BatchID;  
  
  END  
  
   
  
    FETCH NEXT FROM BatchCursor_db INTO  @BatchID;  
         
END;  
  
CLOSE BatchCursor_db;  
DEALLOCATE BatchCursor_db;  
  
  
  
  
  
END