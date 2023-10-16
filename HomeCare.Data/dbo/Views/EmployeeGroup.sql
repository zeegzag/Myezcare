CREATE view [dbo].[EmployeeGroup]
as
select 
    grp.* ,e.EmployeeID as EmployeeID
from 
    Employees E
    cross apply dbo.GetCSVTable(e.GroupIDs) grp