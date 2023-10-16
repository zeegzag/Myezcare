CREATE PROCEDURE [dbo].[SetScheduleAssignmentModel]        
@IgnoreFrequency int      
AS        
BEGIN        
 select RegionID,RegionName,RegionCode from Regions order by RegionName ASC        
 select PayorID,PayorName,ShortName from Payors order by PayorName ASC        
 select FrequencyCodeID,Code from FrequencyCodes where FrequencyCodeID not in(@IgnoreFrequency) order by Code ASC        
 SELECT * from ScheduleStatuses order by ScheduleStatusName ASC            
 SELECT TransportLocationID,Location ,IsDeleted FROM TransportLocations order by Location ASC --where IsDeleted=0            
 select FacilityID,FacilityName,IsDeleted from Facilities order by FacilityName ASC --where IsDeleted = 0              
        
 select * from WeekMasters order by StartDate DESC      
END 
