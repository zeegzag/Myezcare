CREATE PROCEDURE [dbo].[HC_GetBatchNoteDetailsBasedOnServiceDetails]    
@ServiceCode VARCHAR(MAX),    
@ServiceStartDate DATE,  
@ServiceEndDate DATE,  
@ServiceCode_Mod_01 VARCHAR(100),  
@ServiceCode_Mod_02 VARCHAR(100),  
@ServiceCode_Mod_03 VARCHAR(100),  
@ServiceCode_Mod_04 VARCHAR(100),  
@PatientAHCCCSID VARCHAR(100),  
@BatchID BIGINT  
AS    
BEGIN    
    
 DECLARE @ServiceCode_Mod_01_ID BIGINT;  
 SELECT @ServiceCode_Mod_01_ID=ModifierCode FROM Modifiers WHERE ModifierCode IN (@ServiceCode_Mod_01)  
  
 DECLARE @ServiceCode_Mod_02_ID BIGINT;  
 SELECT @ServiceCode_Mod_02_ID=ModifierCode FROM Modifiers WHERE ModifierCode IN (@ServiceCode_Mod_02)  
  
 DECLARE @ServiceCode_Mod_03_ID BIGINT;  
 SELECT @ServiceCode_Mod_03_ID=ModifierCode FROM Modifiers WHERE ModifierCode IN (@ServiceCode_Mod_03)  
  
 DECLARE @ServiceCode_Mod_04_ID BIGINT;  
 SELECT @ServiceCode_Mod_04_ID=ModifierCode FROM Modifiers WHERE ModifierCode IN (@ServiceCode_Mod_04)  
   
 DECLARE @BatchNoteID BIGINT;  
  
  
    
 SELECT DISTINCT @BatchNoteID=BN.BatchNoteID  
 FROM Notes N  
 INNER JOIN ServiceCodes S ON S.ServiceCodeID = S.ServiceCodeID  -- AND BN.BatchID=@BatchID  
 AND   
 (  ISNULL(@ServiceCode_Mod_01_ID,0)=0 OR @ServiceCode_Mod_01_ID IN (SELECT VAL FROM GetCSVTable(S.ModifierID)) ) AND  
 (  ISNULL(@ServiceCode_Mod_02_ID,0)=0 OR @ServiceCode_Mod_01_ID IN (SELECT VAL FROM GetCSVTable(S.ModifierID)) ) AND  
 (  ISNULL(@ServiceCode_Mod_03_ID,0)=0 OR @ServiceCode_Mod_01_ID IN (SELECT VAL FROM GetCSVTable(S.ModifierID)) ) AND  
 (  ISNULL(@ServiceCode_Mod_04_ID,0)=0 OR @ServiceCode_Mod_01_ID IN (SELECT VAL FROM GetCSVTable(S.ModifierID)) )  
   
 INNER JOIN BatchNotes BN ON BN.NoteID=N.NoteID AND BN.BatchID=@BatchID  
 WHERE N.ServiceCode=@ServiceCode AND N.ServiceDate BETWEEN @ServiceStartDate AND @ServiceEndDate AND N.AHCCCSID=@PatientAHCCCSID  
  
 SELECT ISNULL(@BatchNoteID,0);  
     
END
