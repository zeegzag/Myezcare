CREATE PROCEDURE [dbo].[SetAddFacilityHousePage]            
@FacilityID BIGINT ,  
@ExcludedStatus varchar(max)  
           
AS            
BEGIN             
          

 --SELECT * FROM Regions;            
            select dm.DDMasterID AS RegionID,dm.Title AS RegionName from DDMaster dm  
inner join lu_DDMasterTypes lu on lu.DDMasterTypeID=dm.ItemType  
where lu.Name='Location of Service' AND DM.IsDeleted=0

 select PayorID,PayorName from Payors WHERE IsDeleted=0 OR PayorID IN(SELECT PayorID FROM FacilityApprovedPayors WHERE FacilityID=@FacilityID) order by PayorName ASC            
            
 select FacilityID as ParentFacilityID ,FacilityName  from  Facilities where ParentFacilityID=0 and IsDeleted=0            
            
 SELECT *,(SELECT             
     STUFF((SELECT ',' + CAST(s.PayorID AS VARCHAR)            
     FROM FacilityApprovedPayors s WHERE FacilityID = CAST(@FacilityID AS BIGINT)             
     ORDER BY s.PayorID            
     FOR XML PATH('')),1,1,'')) AS SetSelectedPayors            
 FROM Facilities WHERE  FacilityID = CAST(@FacilityID AS BIGINT)             
            
    
 select *  from States;    
    
 select FacilityID Value,FacilityName Name from Facilities where IsDeleted=0 order by FacilityName ASC    
    
 select TransportLocationID Value,Location Name from TransportLocations where IsDeleted=0 order by Location ASC    
   
 select * from ScheduleStatuses where ScheduleStatusID not in (select CAST(val AS BIGINT) from GetCSVTable(@ExcludedStatus))  
   
END