CREATE PROCEDURE [dbo].[HC_BulkSetClaimAdjustmentFlag]  
@ClaimAdjustmentType as varchar(50)=null,    
@ClaimAdjustmentReason as varchar(MAX)=null,    
@ItemID as VARCHAR(MAX)    
AS    
BEGIN    
    
  Declare @Value VARCHAR(MAX);    
  Declare @BatchID BIGINT;    
  Declare @NoteID BIGINT;    
    
  DECLARE BulkSetClaimAdjustmen_cursor CURSOR FOR  SELECT val FROM GetCSVTable(@ItemID)    
  OPEN BulkSetClaimAdjustmen_cursor       
  FETCH NEXT FROM BulkSetClaimAdjustmen_cursor INTO @Value    
    
  WHILE @@FETCH_STATUS = 0       
  BEGIN       
    
   SELECT @BatchID = (SELECT CONVERT(BIGINT,splitdata) FROM fnSplitString(@Value,'|') WHERE RowID = 1),    
    @NoteID  = (SELECT CONVERT(BIGINT,splitdata) FROM fnSplitString(@Value,'|') WHERE RowID = 2)     
    
   EXEC SetClaimAdjustmentFlag01 @ClaimAdjustmentType,@ClaimAdjustmentReason,@BatchID,@NoteID    
   FETCH NEXT FROM BulkSetClaimAdjustmen_cursor INTO @Value      
  END       
    
  CLOSE BulkSetClaimAdjustmen_cursor       
  DEALLOCATE BulkSetClaimAdjustmen_cursor    
    
    
END
