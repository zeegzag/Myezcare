-- EXEC CreateChildGroupNotes
CREATE PROCEDURE [dbo].[CreateChildGroupNotes]
AS 
BEGIN

--SELECT GroupID,ParentID,NoteID,N.ReferralID, N.PayorID, N.ServiceCodeID, N.ServiceCode, N.ModifierID, N.ServiceDate, N.PosID, N.ZarephathService, N.PayorServiceCodeMappingID, N.RenderingProviderID, N.BillingProviderID FROM Notes N WHERE N.ServiceCode IS




-- DELETE FROM ChildNotes


--NOT NULL AND IsBillable=1 --AND GroupID != 1
-- ORDER BY ReferralID,ServiceDate,ServiceCode

BEGIN TRAN

UPDATE Notes SET GroupID=NULL, ParentID=NULL WHERE ServiceCode IS NOT NULL AND IsBillable=1


UPDATE N SET N.GroupID=G.GroupID
FROM Notes N 
INNER JOIN (
SELECT *,ROW_NUMBER() OVER (PARTITION BY  ReferralID, PayorID, ServiceCodeID, ServiceCode, ModifierID, ServiceDate, PosID,  PayorServiceCodeMappingID, RenderingProviderID, BillingProviderID ORDER BY NoteID ASC) AS GroupID 
FROM (SELECT NoteID,N.ReferralID, N.PayorID, N.ServiceCodeID, N.ServiceCode, N.ModifierID, N.ServiceDate, N.PosID, N.PayorServiceCodeMappingID, N.RenderingProviderID, N.BillingProviderID
--ROW_NUMBER() OVER (PARTITION BY  N.ReferralID, N.PayorID, N.ServiceCodeID, N.ServiceCode, N.ModifierID, N.ServiceDate, N.PosID, N.ZarephathService, N.PayorServiceCodeMappingID, N.RenderingProviderID, N.BillingProviderID ORDER BY NoteID ASC) AS GroupID
FROM Notes N
WHERE ServiceCode IS NOT NULL AND IsBillable=1 AND IsDeleted=0 AND MarkAsComplete=1) AS T
) AS G  ON N.NoteID=G.NoteID



UPDATE N1
  SET N1.ParentID = ID
  FROM Notes N1 INNER JOIN 
    (
		SELECT MIN(NoteID) AS ID, N.ReferralID, N.PayorID, N.ServiceCodeID, N.ServiceCode, N.ModifierID, N.ServiceDate, N.PosID, N.PayorServiceCodeMappingID, N.RenderingProviderID, N.BillingProviderID  FROM NOTES N
		WHERE ServiceCode IS NOT NULL AND IsBillable=1 AND IsDeleted=0 AND MarkAsComplete=1
		GROUP BY N.ReferralID, N.PayorID, N.ServiceCodeID, N.ServiceCode, N.ModifierID, N.ServiceDate, N.PosID, N.PayorServiceCodeMappingID, N.RenderingProviderID, N.BillingProviderID 
	
	) agg on N1.ReferralID = agg.ReferralID AND N1.PayorID=agg.PayorID AND N1.ServiceCodeID=agg.ServiceCodeID AND N1.ServiceCode=agg.ServiceCode 
	     AND ISNULL(N1.ModifierID,0)=ISNULL(agg.ModifierID,0) AND N1.ServiceDate=agg.ServiceDate AND N1.PosID=agg.PosID AND N1.PayorServiceCodeMappingID=agg.PayorServiceCodeMappingID
		 AND N1.RenderingProviderID=agg.RenderingProviderID AND N1.BillingProviderID=agg.BillingProviderID 
		 WHERE N1.ServiceCode IS NOT NULL AND N1.IsBillable=1  AND IsDeleted=0 AND MarkAsComplete=1 AND N1.GroupID != 1


   --UPDATE NOTES SET ParentID=NULL


   --SELECT * FROM Notes WHERE GroupID IS NOT NULL

   INSERT INTO ChildNotes(ParentNoteID,NoteID, CalculatedUnit, CalculatedAmount) 
   SELECT ISNULL(ParentID,NoteID),NoteID,CalculatedUnit, CalculatedAmount FROM NOTES WHERE NOTEID NOT IN (SELECT NoteID FROM ChildNotes) AND GroupID IS NOT NULL AND IsDeleted=0 AND MarkAsComplete=1


   UPDATE CN SET CN.CalculatedUnit=N.CalculatedUnit,CN.CalculatedAmount=N.CalculatedAmount,CN.ParentNoteID=ISNULL(N.ParentID,N.NoteID)
   FROM ChildNotes CN 
   INNER JOIN Notes N ON N.NoteID=CN.NoteID
   WHERE N.GroupID IS NOT NULL


   DELETE CN FROM ChildNotes CN
   INNER JOIN Notes N
   ON N.NoteID=CN.NoteID
   WHERE N.IsDeleted=1 OR MarkAsComplete=0 

  
  
  COMMIT

END