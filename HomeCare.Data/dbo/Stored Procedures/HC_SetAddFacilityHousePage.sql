CREATE PROCEDURE [dbo].[HC_SetAddFacilityHousePage]              
@FacilityID BIGINT --,    
--@ExcludedStatus varchar(max)    
             
AS              
BEGIN               
         
  --SELECT * FROM Regions;  
  select dm.DDMasterID AS RegionID,dm.Title AS RegionName from DDMaster dm  
inner join lu_DDMasterTypes lu on lu.DDMasterTypeID=dm.ItemType  
where lu.Name='Location of Service' AND DM.IsDeleted=0
         
 SELECT *  FROM States;      
   
 SELECT *  FROM Facilities WHERE  FacilityID =@FacilityID;      
  
 SELECT  
  FE.FacilityEquipmentID,  
  FE.FacilityID,  
  D.DDMasterID AS EquipmentID,  
  D.Title AS EquipmentName  
 FROM  
  FacilityEquipments FE  
  INNER JOIN DDMaster D ON FE.DDMasterID = D.DDMasterID  
 WHERE  
  FE.FacilityID = @FacilityID  
   
END