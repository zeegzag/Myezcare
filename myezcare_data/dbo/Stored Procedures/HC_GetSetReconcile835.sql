CREATE PROCEDURE [dbo].[HC_GetSetReconcile835]    
                  
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
      
 SELECT ServiceCodeID,ServiceCode = ServiceCode + CASE WHEN SC.ModifierID IS NOT NULL THEN ' : '+    
 STUFF(              
      (SELECT ', ' + convert(varchar(100), M.ModifierCode, 120)              
      FROM Modifiers M              
      where M.ModifierID IN (SELECT val FROM GetCSVTable(SC.ModifierID))              
      FOR XML PATH (''))              
      , 1, 1, '')    
 ELSE '' END        
 FROM ServiceCodes SC    
END
