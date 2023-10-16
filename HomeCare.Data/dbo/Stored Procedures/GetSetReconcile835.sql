CREATE PROCEDURE [dbo].[GetSetReconcile835]  
        
AS             
           
BEGIN  

	UPDATE BatchNotes SET IsShowOnParentReconcile=0 WHERE NoteID IN (
		SELECT DISTINCT NoteID FROM (
		SELECT BatchNoteID, NoteID, DENSE_RANK() OVER(PARTITION BY NoteID ORDER BY IsUseInBilling, BatchNoteID ASC) AS DRANK ,IsUseInBilling  FROM BatchNotes
		) AS T
		WHERE DRANK=1 AND IsUseInBilling=0
	)

                    
 select * from Payors where IsDeleted=0 AND IsBillingActive=1 Order BY PayorName;  
 select * from Modifiers where IsDeleted=0 Order BY ModifierName;  
 select * from PlaceOfServices where IsDeleted=0 Order BY PosID;  
 select ClaimStatusCodeID,ClaimStatusName = Convert(varchar(2), ClaimStatusCodeID)+': '+ClaimStatusName  from ClaimStatusCodes where IsDeleted=0 Order BY ClaimStatusCodeID;  
 select ClaimAdjustmentGroupCodeID,ClaimAdjustmentGroupCodeName=ClaimAdjustmentGroupCodeID+': '+ ClaimAdjustmentGroupCodeName from ClaimAdjustmentGroupCodes 
 where IsDeleted=0 Order BY OrderID;  
 select ClaimAdjustmentReasonCodeID,ClaimAdjustmentReasonDescription=ClaimAdjustmentReasonCodeID+': '+ClaimAdjustmentReasonDescription from ClaimAdjustmentReasonCodes 
 where IsDeleted=0 Order BY OrderID;  
 SELECT ServiceCodeID,ServiceCode = ServiceCode + CASE WHEN M.ModifierCode IS NOT NULL THEN ' : '+M.ModifierCode ELSE '' END  FROM ServiceCodes SC LEFT JOIN Modifiers M ON M.ModifierID=SC.ModifierID
END