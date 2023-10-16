--updatedBy      UpdatedDate      Description     --vikas          06-june-2019               change from EmployeeID to EmployeesID in where condition      --vikas    17-july-2019              alter proc for getting comma seprated value from Employee id  
 -- exec GetCommonNotes '4296',''     --EXEC GetCommonNotes @ReferralID = 4012, @EmployeeID = '621', @EmployeesID = 622              
CREATE PROCEDURE [dbo].[GetCommonNotes]
  @LoggedInUser nvarchar(200),
  @ReferralID bigint = NULL,
  @EmployeeID nvarchar(50) = NULL
AS
BEGIN
  IF @ReferralID = 0
    SET @ReferralID = NULL
  IF @EmployeeID = 0
    SET @EmployeeID = NULL


  SELECT
    c.*,
    dbo.GetGeneralNameFormat(e.FirstName, e.LastName) AS CreatedByName,
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
  ORDER BY c.CreatedDate DESC
END
