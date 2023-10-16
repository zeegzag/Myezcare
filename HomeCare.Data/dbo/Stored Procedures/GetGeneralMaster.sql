CREATE PROCEDURE [dbo].[GetGeneralMaster]      
 @ItemTypeID BIGINT 
  
AS    
BEGIN    
  
  SELECT DDMasterID,Title FROM DDMaster where ItemType =@ItemTypeID
  
 
   
  
    
END