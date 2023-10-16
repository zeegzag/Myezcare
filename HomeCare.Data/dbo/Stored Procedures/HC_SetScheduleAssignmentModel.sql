CREATE PROCEDURE [dbo].[HC_SetScheduleAssignmentModel]          
@IgnoreFrequency int  
AS          
BEGIN    
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
 select RegionID,RegionName,RegionCode from Regions order by RegionName ASC          
 select PayorID,PayorName,ShortName from Payors order by PayorName ASC          
 select FrequencyCodeID,Code from FrequencyCodes where FrequencyCodeID NOT IN(@IgnoreFrequency)  AND UsedInHomeCare=1 order by Code ASC               
 SELECT * from ScheduleStatuses order by ScheduleStatusName ASC              
 SELECT TransportLocationID,Location ,IsDeleted FROM TransportLocations order by Location ASC --where IsDeleted=0              
 select FacilityID,FacilityName,IsDeleted from Facilities order by FacilityName ASC --where IsDeleted = 0                
          
 select * from WeekMasters order by StartDate DESC        
  
 SELECT EmployeeID,FirstName,LastName,dbo.GetGenericNameFormat(FirstName,MiddleName, LastName,@NameFormat) AS EmployeeName, MobileNumber FROM Employees E WHERE IsDeleted=0 AND IsActive=1 ORDER BY  E.FirstName ASC        
END   