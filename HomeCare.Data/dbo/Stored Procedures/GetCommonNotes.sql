
 -- exec GetCommonNotes '4296',''     --EXEC GetCommonNotes @ReferralID = 4012, @EmployeeID = '621', @EmployeesID = 622                    
CREATE PROCEDURE [dbo].[GetCommonNotes]      
  @LoggedInUser nvarchar(200),      
  @ReferralID bigint = NULL,      
  @EmployeeID nvarchar(50) = NULL      
AS      
BEGIN   
DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
  IF @ReferralID = 0      
    SET @ReferralID = NULL      
  IF @EmployeeID = 0      
    SET @EmployeeID = NULL      
      
      
  SELECT      
    c.*,      
    dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS CreatedByName,      
    d.Title AS 'Category'      
  FROM CommonNotes c      
  LEFT JOIN Employees e      
    ON e.EmployeeID = C.CreatedBy      
  LEFT JOIN Employees le      
    ON le.EmployeeID = @LoggedInUser      
  LEFT JOIN DDMaster d      
    ON c.CategoryID = d.DDMasterID      
  WHERE      
    c.IsDeleted = 0      
    AND (@EmployeeID IS NULL      
      OR c.EmployeeID = @EmployeeID)      
    AND (@ReferralID IS NULL      
      OR c.ReferralID = @ReferralID)      
    AND (le.RoleID = 1      
      OR c.Createdby = @LoggedInUser      
      OR c.UpdatedBy = @LoggedInUser      
      OR ',' + c.EmployeesID + ',' LIKE '%,' + @LoggedInUser + ',%'      
      OR ',' + c.RoleID + ',' LIKE '%,' + CONVERT(varchar(100), le.RoleID) + ',%'      
    )      
  union      
  SELECT      
    c.*,      
   dbo.GetGenericNameFormat(e.FirstName,e.MiddleName, e.LastName,@NameFormat) AS CreatedByName,      
    d.Title AS 'Category'      
  FROM CommonNotes c      
  LEFT JOIN Employees e      
    ON e.EmployeeID = C.CreatedBy      
  LEFT JOIN Employees le      
    ON le.EmployeeID = @LoggedInUser      
  LEFT JOIN DDMaster d      
    ON c.CategoryID = d.DDMasterID      
 where isPrivate=0  and ReferralID=@ReferralID    
      
END 