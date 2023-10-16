CREATE PROCEDURE [dbo].[SetScheduleMasterPage]
AS      
BEGIN      
SELECT * from ScheduleStatuses order by ScheduleStatusName ASC      
      
SELECT TransportLocationID,Location ,IsDeleted FROM TransportLocations order by Location ASC --where IsDeleted=0      
      
select FacilityID,FacilityName,IsDeleted from Facilities order by FacilityName ASC --where IsDeleted = 0        
  
  
select RegionID Value,RegionName Name from Regions  

select * from WeekMasters

select Value=LanguageID, Name from Languages     
END