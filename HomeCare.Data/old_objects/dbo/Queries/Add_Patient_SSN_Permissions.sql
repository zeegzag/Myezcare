


begin 

Declare @EmployeePermissionId int
Declare @IdentityValue int
Declare @max int

declare @IdentityOutput table ( Id int )

set @EmployeePermissionId = (SELECT PermissionID FROM Permissions where PermissionName = 'Patient Intake' and ParentID = 0)
set @max = (select MAX( PermissionID ) FROM Permissions)

insert Permissions
     ( PermissionID , PermissionName , Description , ParentID , OrderID ,IsDeleted ,  UsedInHomeCare , PermissionCode , CompanyHasAccess , PermissionPlatform )
output inserted.PermissionID into @IdentityOutput
values
     (  @max+1,'SSN' , 'Social Security Number' , @EmployeePermissionId  , 1 , 0 , 1 ,'Patient_SSN' , 1 , 'Web');

set @IdentityValue = (select Id from @IdentityOutput)
set @max = (select MAX( PermissionID ) FROM Permissions)

insert Permissions
    (PermissionID , PermissionName , Description , ParentID , OrderID ,IsDeleted ,  UsedInHomeCare , PermissionCode , CompanyHasAccess , PermissionPlatform )
values
    (  @max+1,'Can See' , 'Can See' , @IdentityValue , 1 , 0 , 1 ,'Can_See_SSN_Patient' , 1 , 'Web');

set @max = (select MAX( PermissionID ) FROM Permissions)
insert Permissions
    (PermissionID , PermissionName , Description , ParentID , OrderID ,IsDeleted ,  UsedInHomeCare , PermissionCode , CompanyHasAccess , PermissionPlatform )
values
    ( @max+1, 'Can''t See' , 'Can''t See' , @IdentityValue , 1 , 0 , 1 ,'Can_Not_See_SSN_Patient' , 1 , 'Web');

set @max = (select MAX( PermissionID ) FROM Permissions)

insert Permissions
    (PermissionID , PermissionName , Description , ParentID , OrderID ,IsDeleted ,  UsedInHomeCare , PermissionCode , CompanyHasAccess , PermissionPlatform )
values
    (  @max+1 ,'Can See Last Four Digit' , 'Can See ' , @IdentityValue , 1 , 0 , 1 ,'Can_See_Last_Four_Digit_SSN_Patient' , 1 , 'Web');

end 

--end add ssn permissions