-- exec [HC_SetAgencyListPage]
CREATE PROCEDURE [dbo].[HC_SetAgencyListPage]    
       
AS      
BEGIN      
 -- Add DDMaster for Region  
 -- Kundan: 17-04-2020  
  select DDMasterID as RegionID,Title as RegionName 
 from DDMaster dm
 inner join lu_DDMasterTypes lu on lu.DDMasterTypeID=dm.ItemType
  where lu.Name='Region' AND DM.IsDeleted=0
 --DECLARE @ItemType INT = 27;  
 --SELECT DDMasterID, Title FROM [dbo].[DDMaster] WHERE ItemType=@ItemType AND IsDeleted=0  
 SELECT 0;     
 SELECT 0;     
END