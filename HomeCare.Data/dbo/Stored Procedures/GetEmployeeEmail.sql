create procedure GetEmployeeEmail  
(  
  @EmployeeID bigint  
)  
as  
begin  
select e.Email  from Employees e where e.EmployeeID=@EmployeeID  
end