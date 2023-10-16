-- [HC_SetAddServiceCodePage] 1, 1, 5  
CREATE PROCEDURE [dbo].[HC_SetAddServiceCodePage]    
 @ServiceCodeID BIGINT=0  ,              
 @DDType_CareType int=1,  
 @DDType_RevenueCode int=0      
AS                              
BEGIN                              
                              
 SELECT * FROM ServiceCodes WHERE ServiceCodeID=@ServiceCodeID                              
                              
 SELECT * FROM Modifiers  WHERE IsDeleted=0                            
                    
 SELECT Name=Title, Value=DDMasterID FROM DDMaster WHERE IsDeleted=0 and ItemType=@DDType_CareType          
 --and DDMasterID not in (select Distinct caretype from servicecodes)              
                        
 SELECT Name=Title, Value=DDMasterID FROM DDMaster WHERE IsDeleted=0 and ItemType=@DDType_RevenueCode  
  
 SELECT Name=PayorName, Value=PayorID FROM Payors WHERE IsDeleted = 0 
 


 SELECT DISTINCT Name=DM.Title, Value=DM.DDMasterID FROM DDMaster DM  
 INNER JOIN lu_DDMasterTypes lu ON DM.ItemType = lu.ParentID  
 WHERE DDMasterTypeID=@DDType_CareType AND DM.IsDeleted=0

  
 
  
END 
GO