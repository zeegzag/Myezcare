-- EXEC [SetReportPage]          
CREATE PROCEDURE [dbo].[SetReportPage]               
AS              
BEGIN              
 SELECT * FROM ReferralStatuses ORDER BY  Status ASC          
 select RegionID,RegionName  from Regions ORDER BY RegionName ASC  
 select AgencyID,NickName from Agencies ORDER BY NickName ASC  
 select *  from ScheduleStatuses  ORDER BY ScheduleStatusName ASC         
 SELECT PayorID,PayorName from Payors           
 select EmployeeID,LastName+', '+FirstName as Name from Employees ORDER BY LastName ASC  
 select EmployeeID,LastName+', '+FirstName as EmployeeName,IsDeleted from Employees where IsDeleted=0  ORDER BY LastName ASC        
          
 select AgencyLocationID,LocationName from AgencyLocations ORDER BY LocationName ASC  
 SELECT CaseManagerID,LastName+', '+FirstName as Name From CaseManagers ORDER BY LastName ASC   
  SELECT Value=EmployeeID,Name=LastName+', '+FirstName  FROM Employees  Order by LastName ASC       
 SELECT 0;              
 SELECT 0;              
 SELECT 0;              
 SELECT 0;              
 SELECT 0;              
 SELECT 0;         
 SELECT 0; 
 SELECT 0;          

 SELECT Value=FacilityID,Name=FacilityName from Facilities order by FacilityName ASC;       
         
END 
