--  EXEC st.GetEmployeeGroup  152      
      
CREATE PROCEDURE st.GetEmployeeGroup        
  @GroupId NVARCHAR(MAX)=NULL      
AS        
BEGIN        
        
        
  DECLARE @DDType_EmployeeGroup int = (SELECT DDMasterTypeID FROM lu_DDMasterTypes WHERE Name = 'Employee Group')                  
  SELECT                  
    d.DDMasterID AS Value,                  
    d.Title AS Name                  
  FROM dbo.DDMaster d                  
  WHERE d.ItemType = @DDType_EmployeeGroup   and DDMasterID=@GroupId               
  AND IsDeleted = 0    
    Select e.EmployeeID,E.EmployeeUniqueID,dbo.GetGeneralNameFormat(e.LastName,e.FirstName) AS EmployeeName,dm.Title as Designation,dbo.GetCommaSepCategories(EmployeeID) AS [GroupNames]      
   , case when e.groupids like '%'+@GroupId+'%' then 'true' else 'false' end as IsChecked      
   from Employees e       
   inner join DDMaster dm on dm.DDMasterID=e.Designation      
    where  e.IsDeleted=0 and e.IsActive=1      
      
 Select e.EmployeeID,E.EmployeeUniqueID,dbo.GetGeneralNameFormat(e.LastName,e.FirstName) AS EmployeeName,Email,MobileNumber,GroupIDs from Employees e      
  where e.IsDeleted=0 and e.IsActive=1 and e.GroupIDs=@GroupId   
END