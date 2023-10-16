create procedure GetEmployeeEmailSignature  
(  
@EmployeeID bigint  
)  
as  
begin  
  
    select e.Name,e.Description from EmployeeEmailSignature e where e.EmployeeID=@EmployeeID;  
end