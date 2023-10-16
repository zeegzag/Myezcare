   
CREATE PROCEDURE [dbo].[API_GetEmpRefList]          
@EmployeeID BIGINT                      
AS                                          
BEGIN      
 DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()     
DECLARE @RoleID BIGINT;      
DECLARE @Cnt int    
SELECT @RoleID=RoleID FROM Employees WHERE EmployeeID=@EmployeeID      
--SELECT       distinct e.EmployeeID, dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS EmployeeName    
--FROM            Roles INNER JOIN    
--                         Employees AS e ON Roles.RoleID = e.RoleID INNER JOIN    
--                         Permissions AS p INNER JOIN    
--                         RolePermissionMapping AS rpm ON p.PermissionID = rpm.PermissionID ON Roles.RoleID = rpm.RoleID    
--WHERE        (e.IsDeleted = 0) AND (e.IsActive = 1) AND (p.PermissionCode = 'Record_Access_All_Record')      
    
    
select @cnt=count(*) from permissions p inner join RolePermissionMapping rpm on rpm.PermissionID=p.PermissionID where rpm.RoleID=@RoleID and p.PermissionCode = 'Record_Access_All_Record' and rpm.isdeleted=0    
if (@cnt>0)    
BEGIN    
select distinct e.EmployeeID, dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS EmployeeName from employees e WHERE        (e.IsDeleted = 0) AND (e.IsActive = 1)    
END    
else    
Begin    
SELECT       distinct e.EmployeeID, dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS EmployeeName    
FROM  employees E inner join EmployeeGroup EG on EG.employeeID=e.employeeid     
where eg.val in (select eg1.val from employeeGroup EG1 where EG1.EmployeeID=@EmployeeID)     
and e.IsDeleted=0 and e.IsActive=1     
end    
        
    
SELECT DISTINCT sm.ReferralID,PatientName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) FROM ScheduleMasters sm          
INNER JOIN Referrals r ON r.ReferralID=sm.ReferralID     inner join employees e on e.employeeid=sm.employeeid and sm.startdate>getdate()-1    
WHERE r.IsDeleted=0  and (r.casemanagerid=@employeeid or r.assignee=@employeeid or sm.employeeid=@employeeid)   and  referralStatusID=1     
--WHERE ((@RoleID!=1 AND EmployeeID=@EmployeeID) OR @RoleID=1) AND r.IsDeleted=0      
Union    
select R.referralid, PatientName=dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) from Referrals R Inner join ReferralGroup RG on R.referralID=RG.ReferralID    
inner join EmployeeGroup EG on RG.val=EG.val    
where EG.EmployeeID=@EmployeeID and ReferralStatusID=1 and R.IsDeleted=0    
    
    
END 