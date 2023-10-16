  
-- EXEC HC_SaveMannualPaymentPostingDetails @BatchID = '110673', @BatchNoteID = '119170', @MPP_AdjustmentAmount = '74.04', @MPP_AdjustmentGroupCodeID = 'OA', @MPP_AdjustmentGroupCodeName = '', @MPP_AdjustmentComment = ''  
CREATE PROCEDURE HC_SaveMannualPaymentPostingDetails      
@BatchID BIGINT,      
@BatchNoteID BIGINT,      
@MPP_AdjustmentAmount NVARCHAR(MAX),      
@MPP_AdjustmentGroupCodeID NVARCHAR(MAX),      
@MPP_AdjustmentGroupCodeName  NVARCHAR(MAX),    
@MPP_AdjustmentComment NVARCHAR(MAX)    
      
AS      
BEGIN      
PRINT @MPP_AdjustmentAmount      
UPDATE BatchNotes  SET       
MPP_AdjustmentAmount=@MPP_AdjustmentAmount,      
MPP_AdjustmentGroupCodeID=@MPP_AdjustmentGroupCodeID,      
MPP_AdjustmentGroupCodeName=@MPP_AdjustmentGroupCodeName,    
MPP_AdjustmentComment=@MPP_AdjustmentComment    
WHERE BatchNoteID= @BatchNoteID AND BatchID=@BatchID      
      
      
      
END 