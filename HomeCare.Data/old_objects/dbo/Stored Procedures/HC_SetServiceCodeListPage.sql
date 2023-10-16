
CREATE PROCEDURE [dbo].[HC_SetServiceCodeListPage]                
@DDType_CareType int  
AS                
BEGIN                
 SELECT Name=CONCAT(ModifierCode,' - ',ModifierName),Value=ModifierID FROM Modifiers                
 SELECT Name=ServiceCodeTypeName,Value=ServiceCodeTypeID FROM ServiceCodeTypes WHERE ServiceCodeTypeID=4                
 SELECT          
 Name=Title,          
 Value=DDMasterID       
 FROM DDMaster        
 where IsDeleted=0 and ItemType= @DDType_CareType        
END 

