CREATE PROCEDURE [rpt].[locationList]           
          
AS          
BEGIN               
With CTE as (  
--select  rs.RegionID,rs.RegionName          
--from  Regions rs    
  select dm.DDMasterID AS RegionID,dm.Title AS RegionName from DDMaster dm  
inner join lu_DDMasterTypes lu on lu.DDMasterTypeID=dm.ItemType  
where lu.Name='Location of Service' AND DM.IsDeleted=0   
  
)Select distinct * from CTE order by RegionID,RegionName;      
END 

GO