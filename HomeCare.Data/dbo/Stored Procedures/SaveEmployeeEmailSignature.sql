CREATE procedure SaveEmployeeEmailSignature  
(  
@Name nvarchar(250),  
@Description nvarchar(max),  
@EmployeeID bigint,  
@UpdatedBy bigint  
)  
as  
begin  
       if(select count(*) from EmployeeEmailSignature e where e.EmployeeID=@EmployeeID)<=0  
    begin  
   insert into EmployeeEmailSignature(Name,Description,EmployeeID,UpdatedBy,UpdatedOn)  
   values(@Name,@Description,@EmployeeID,@UpdatedBy,GETDATE())  
    end  
    else  
    begin  
         update EmployeeEmailSignature set Name=@Name,Description=@Description,UpdatedBy=@UpdatedBy,UpdatedOn=GETDATE()  
   where EmployeeID=@EmployeeID  
    end  
end