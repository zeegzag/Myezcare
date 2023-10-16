CREATE PROCEDURE [dbo].[GetPatientScheduledEmployees]  
 @ReferralID bigint=null,  
 @startDate DateTime=null,    
 @EndDate DateTime =null  
AS  
BEGIN  
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
 select s.EmployeeID, dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) EmployeeName from ScheduleMasters s  
 inner join Employees e on e.EmployeeID=s.EmployeeID  
 where s.IsDeleted=0 and (@ReferralID=null or s.ReferralID=@ReferralID)  
 and (@startDate is null or (cast(s.StartDate as date) >= @startDate AND cast(s.StartDate as date) <=DATEADD(DAY, 8 - DATEPART(WEEKDAY, @startDate), CAST(@startDate AS DATE))))  
 group by s.EmployeeID, e.LastName, e.FirstName  ,e.MiddleName
END  
  
  