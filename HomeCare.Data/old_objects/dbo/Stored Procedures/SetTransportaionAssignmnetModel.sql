
--exec [SetTransportaionAssignmnetModel]
CREATE procedure [dbo].[SetTransportaionAssignmnetModel]
AS  
BEGIN  
select TransportLocationID [Value],Location [Name],IsDeleted  from TransportLocations order by Location ASC;
select FacilityID [Value],FacilityName Name,IsDeleted  from  Facilities order by FacilityName ASC;
select EmployeeID [value],LastName+', '+FirstName Name,IsDeleted from Employees order by LastName ASC;
select * from TransportationFilters;
  
END

