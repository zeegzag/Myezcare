CREATE PROCEDURE [dbo].[GetCareType]    
AS        
BEGIN       
 
--select * from DDMaster where ItemType=1 and IsDeleted=0  
 SELECT       
    dm.Title AS Title, dm.DDMasterID AS DDMasterID       
  FROM DDMaster dm       
  INNER JOIN lu_DDMasterTypes lu       
    ON lu.DDMasterTypeID = dm.ItemType       
  WHERE lu.Name = 'Care Type'  and dm.IsDeleted=0
       
END