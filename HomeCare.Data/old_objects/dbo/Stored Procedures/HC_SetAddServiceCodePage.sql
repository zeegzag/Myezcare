CREATE PROCEDURE [dbo].[HC_SetAddServiceCodePage]                            
	@ServiceCodeID BIGINT=0  ,            
	@DDType_CareType int,
	@DDType_RevenueCode int=0    
AS                            
BEGIN                            
                            
	SELECT * FROM ServiceCodes WHERE ServiceCodeID=@ServiceCodeID                            
                            
	SELECT * FROM Modifiers  WHERE IsDeleted=0                          
                  
	SELECT Name=Title, Value=DDMasterID FROM DDMaster WHERE IsDeleted=0 and ItemType=@DDType_CareType        
	--and DDMasterID not in (select Distinct caretype from servicecodes)            
                      
	--Added for ticket#2frxen
	SELECT Name=Title, Value=DDMasterID FROM DDMaster WHERE IsDeleted=0 and ItemType=@DDType_RevenueCode

	SELECT Name=PayorName, Value=PayorID FROM Payors WHERE IsDeleted = 0

END  