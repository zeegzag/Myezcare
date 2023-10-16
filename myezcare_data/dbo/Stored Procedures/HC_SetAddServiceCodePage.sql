CREATE PROCEDURE [dbo].[HC_SetAddServiceCodePage]                          
@ServiceCodeID BIGINT=0  ,          
@DDType_CareType int        
AS                          
BEGIN                          
                          
SELECT * FROM ServiceCodes WHERE ServiceCodeID=@ServiceCodeID                          
                          
SELECT                
*          
FROM Modifiers  where IsDeleted=0                        
                
SELECT                
Name=Title,                
Value=DDMasterID             
FROM DDMaster              
where IsDeleted=0 and ItemType=@DDType_CareType      
--and DDMasterID not in (select Distinct caretype from servicecodes)          
                    
END
