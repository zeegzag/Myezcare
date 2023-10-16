-- EXEC HC_CreateChildGroupNotes_Temporary                  
CREATE PROCEDURE HC_CM_CreateChildGroupNotes_Temporary      
AS                   
BEGIN                  
                  
                
---- UPDATE Notes SET GroupID=NULL, ParentID=NULL WHERE ServiceCode IS NOT NULL AND IsBillable=1                  
                  
---------------UPDATE NOTES-----------------------------------------                  
                  
DECLARE @BillableNoteTable TABLE (NoteID BIGINT,ReferralID BIGINT,PayorID BIGINT,ServiceCodeID BIGINT,ServiceCode VARCHAR(MAX), ModifierID  VARCHAR(MAX),ServiceDate DATE,                  
 PosID BIGINT, PayorServiceCodeMappingID BIGINT, RenderingProviderID BIGINT,BillingProviderID BIGINT)                  
INSERT INTO @BillableNoteTable                  
SELECT NoteID,N.ReferralID, N.PayorID, N.ServiceCodeID, N.ServiceCode, N.ModifierID, N.ServiceDate, N.PosID, N.PayorServiceCodeMappingID, N.RenderingProviderID, N.BillingProviderID                  
FROM Notes_Temporary N WHERE ServiceCode IS NOT NULL AND IsBillable=1 AND IsDeleted=0 AND MarkAsComplete=1                  
                  
                  
                  
DECLARE @BillableNoteTable01 TABLE (NoteID BIGINT,ReferralID BIGINT,PayorID BIGINT,ServiceCodeID BIGINT,ServiceCode VARCHAR(MAX), ModifierID  VARCHAR(MAX),ServiceDate DATE,                  
 PosID BIGINT, PayorServiceCodeMappingID BIGINT, RenderingProviderID BIGINT,BillingProviderID BIGINT,GroupID BIGINT,MinNoteID BIGINT)                  
INSERT INTO @BillableNoteTable01                  
SELECT *,                  
ROW_NUMBER() OVER (PARTITION BY  ReferralID, PayorID, ServiceCodeID, ServiceCode, ModifierID, ServiceDate, PosID,  PayorServiceCodeMappingID, RenderingProviderID, BillingProviderID ORDER BY NoteID ASC) AS GroupID,                  
MIN(NoteID) OVER (PARTITION BY  ReferralID, PayorID, ServiceCodeID, ServiceCode, ModifierID, ServiceDate, PosID,  PayorServiceCodeMappingID, RenderingProviderID, BillingProviderID ORDER BY NoteID ASC) AS MinNoteID                    
FROM @BillableNoteTable                  
                  
                   
UPDATE N SET N.GroupID=G.GroupID,N.ParentID= CASE WHEN G.NoteID = G.MinNoteID THEN NULL ELSE G.MinNoteID END                  
--SELECT G.GroupID,G.NoteID, G.MinNoteID,ParentID= CASE WHEN G.NoteID = G.MinNoteID THEN NULL ELSE G.MinNoteID END                  
FROM Notes_Temporary N                   
INNER JOIN @BillableNoteTable01 G  ON N.NoteID=G.NoteID                  
                  
                  
      
---------------UPDATE CHILD NOTES-----------------------------------------                  
                  
   --INSERT INTO ChildNotes(ParentNoteID,NoteID, CalculatedUnit, CalculatedAmount)                   
   --SELECT ISNULL(ParentID,NoteID),NoteID,CalculatedUnit, CalculatedAmount FROM NOTES                   
   --WHERE NOTEID NOT IN (SELECT NoteID FROM ChildNotes) AND GroupID IS NOT NULL AND IsDeleted=0 AND MarkAsComplete=1                  
   INSERT INTO ChildNotes_Temporary(ParentNoteID,NoteID, CalculatedUnit, CalculatedAmount)                   
   SELECT ISNULL(N.ParentID,N.NoteID),N.NoteID,N.CalculatedUnit,N.CalculatedAmount FROM Notes_Temporary N                  
   LEFT JOIN  ChildNotes_Temporary CN ON CN.NoteID=N.NoteID                  
   WHERE N.GroupID IS NOT NULL AND N.IsDeleted=0 AND N.MarkAsComplete=1                  
   AND CN.ChildNoteID IS NULL and  N.CalculatedUnit is not null                
                  
   UPDATE CN SET CN.CalculatedUnit=N.CalculatedUnit,CN.CalculatedAmount=N.CalculatedAmount,CN.ParentNoteID=ISNULL(N.ParentID,N.NoteID)                  
   FROM ChildNotes_Temporary CN                   
   INNER JOIN Notes_Temporary N ON N.NoteID=CN.NoteID                  
   WHERE N.GroupID IS NOT NULL                  
                  
                  
   DELETE CN FROM ChildNotes_Temporary CN                  
   INNER JOIN Notes_Temporary N                  
   ON N.NoteID=CN.NoteID                  
   WHERE N.IsDeleted=1 OR MarkAsComplete=0                   
                  
   SELECT 1               
                    
                
END 