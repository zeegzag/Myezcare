CREATE PROCEDURE [dbo].[GetEmployeeList]
  @Name varchar(100) = NULL,
  @Email varchar(50) = NULL,
  @DepartmentID bigint = 0,
  @RoleID bigint = 0,
  @IsDepartmentSupervisor int = -1,
  @Degree varchar(100) = NULL,
  @CredentialID varchar(50) = NULL,
  @MobileNumber varchar(10) = NULL,
  @GroupIDs nvarchar(max) = NULL,
  @IsDeleted int = -1,
  @SortExpression nvarchar(100),
  @SortType nvarchar(10),
  @FromIndex int,
  @PageSize int,
  @EmployeeId int = NULL,
  @EmployeeUniqueID varchar(50) = NULL,
  @DesignationID bigint = 0,
  @RecordAccess int = 0,
  @LoggedInUserID bigint = NULL
AS
BEGIN

  DECLARE @LoginUserGroupIDs nvarchar(max) =
  (
    SELECT
      e.GroupIDs
    FROM dbo.Employees e
    WHERE
      e.EmployeeID = @LoggedInUserID
  )
  ;
  WITH CTEEmployeeList AS
  (
    SELECT
      *,
      COUNT(t1.EmployeeID) OVER () AS Count
    FROM
    (
      SELECT
        ROW_NUMBER() OVER (ORDER BY

        CASE
          WHEN @SortType = 'ASC' THEN CASE
              WHEN @SortExpression = 'EmployeeID' THEN e.EmployeeID
            END
        END ASC,
        CASE
          WHEN @SortType = 'DESC' THEN CASE
              WHEN @SortExpression = 'EmployeeID' THEN e.EmployeeID
            END
        END DESC,

        CASE
          WHEN @SortType = 'ASC' THEN CASE
              WHEN @SortExpression = 'MobileNumber' THEN MobileNumber
            END
        END ASC,
        CASE
          WHEN @SortType = 'DESC' THEN CASE
              WHEN @SortExpression = 'MobileNumber' THEN MobileNumber
            END
        END DESC,

        CASE
          WHEN @SortType = 'ASC' THEN CASE
              WHEN @SortExpression = 'EmployeeUniqueID' THEN EmployeeUniqueID
              WHEN @SortExpression = 'DepartmentName' THEN DepartmentName
              WHEN @SortExpression = 'Name' THEN LastName
              WHEN @SortExpression = 'Email' THEN Email
              WHEN @SortExpression = 'RoleName' THEN RoleName
              WHEN @SortExpression = 'CredentialName' THEN C.CredentialName
              WHEN @SortExpression = 'Degree' THEN Degree
              WHEN @SortExpression = 'Address' THEN E.Address
            END
        END ASC,
        CASE
          WHEN @SortType = 'DESC' THEN CASE
              WHEN @SortExpression = 'EmployeeUniqueID' THEN EmployeeUniqueID
              WHEN @SortExpression = 'DepartmentName' THEN DepartmentName
              WHEN @SortExpression = 'Name' THEN LastName
              WHEN @SortExpression = 'Email' THEN Email
              WHEN @SortExpression = 'RoleName' THEN RoleName
              WHEN @SortExpression = 'CredentialName' THEN C.CredentialName
              WHEN @SortExpression = 'Degree' THEN Degree
              WHEN @SortExpression = 'Address' THEN E.Address
            END
        END DESC,
        CASE
          WHEN @SortType = 'ASC' THEN CASE
              WHEN @SortExpression = 'IsDepartmentSupervisor' THEN IsDepartmentSupervisor
            END
        END ASC,
        CASE
          WHEN @SortType = 'DESC' THEN CASE
              WHEN @SortExpression = 'IsDepartmentSupervisor' THEN IsDepartmentSupervisor
            END
        END DESC
        ) AS Row,
        e.EmployeeUniqueID,
        e.EmployeeID,
        dbo.GetGeneralNameFormat(e.FirstName, e.LastName) AS Name,
        e.UserName,
        e.Email,
        e.IsSecurityQuestionSubmitted,
        e.IsDepartmentSupervisor,
        d.DepartmentName,
        R.RoleName,
        e.IsDeleted,
        C.CredentialName,
        E.Degree,
        E.EmployeeSignatureID,
        E.MobileNumber,
        dbo.GetCommaSepCategories(E.EmployeeID) AS [Address],
        E.City,
        E.ZipCode,
        E.StateCode,
        e.FirstName,
        ProfileImagePath,
        dm.Title AS Designation
      FROM employees e
      LEFT JOIN Departments d
        ON e.DepartmentID = d.DepartmentID

      LEFT JOIN Roles R
        ON R.RoleID = E.RoleID
      LEFT JOIN EmployeeCredentials C
        ON C.CredentialID = E.CredentialID
      LEFT JOIN DDMaster dm
        ON dm.DDMasterID = e.Designation
      WHERE
        ((CAST(@IsDeleted AS bigint) = -1)
          OR e.IsDeleted = @IsDeleted)
        AND ((@Email IS NULL
            OR LEN(@Email) = 0)
          OR e.Email LIKE '%' + @Email + '%')
        AND ((@EmployeeUniqueID IS NULL
            OR LEN(@EmployeeUniqueID) = 0)
          OR e.EmployeeUniqueID LIKE '%' + @EmployeeUniqueID + '%')

        AND ((@Name IS NULL
            OR LEN(e.LastName) = 0)
          OR (
        (e.FirstName LIKE '%' + @Name + '%')
            OR (e.LastName LIKE '%' + @Name + '%')
            OR (e.FirstName + ' ' + e.LastName LIKE '%' + @Name + '%')
            OR (e.LastName + ' ' + e.FirstName LIKE '%' + @Name + '%')
            OR (e.FirstName + ', ' + e.LastName LIKE '%' + @Name + '%')
            OR (e.LastName + ', ' + e.FirstName LIKE '%' + @Name + '%')))


        AND ((@Degree IS NULL
            OR LEN(@Degree) = 0)
          OR (E.Degree LIKE '%' + @Degree + '%'))
        AND ((CAST(@DepartmentID AS bigint) = 0)
          OR d.DepartmentID = CAST(@DepartmentID AS bigint))
        AND ((CAST(@RoleID AS bigint) = 0)
          OR R.RoleID = CAST(@RoleID AS bigint))

        AND ((@GroupIDs IS NULL
            OR LEN(@GroupIDs) = 0)
          OR EXISTS
				(
				  SELECT
					1
				  FROM GetCSVTable(e.GroupIDs) eg
				  INNER JOIN GetCSVTable(@GroupIDs) SG
					ON SG.val = eg.val
				)
        )

        AND ((CAST(@DesignationID AS bigint) = 0)
          OR CAST(e.Designation AS bigint) = CAST(@DesignationID AS bigint))

        AND ((@CredentialID IS NULL
            OR LEN(@CredentialID) = 0)
          OR (C.CredentialID = @CredentialID))

        AND ((@MobileNumber IS NULL
            OR LEN(@MobileNumber) = 0)
          OR (E.MobileNumber LIKE '%' + @MobileNumber + '%'))

        AND ((CAST(@IsDepartmentSupervisor AS int) = -1)
          OR e.IsDepartmentSupervisor = CAST(@IsDepartmentSupervisor AS int))

        AND e.UserName NOT IN ('me-admin')
        AND e.UserName NOT LIKE '%_myzc'

        AND (@EmployeeId IS NULL
          OR e.EmployeeID = @EmployeeId)

        AND (@RecordAccess = -1
          OR (@RecordAccess = 1
              AND EXISTS
					(
					  SELECT
						1
					  FROM GetCSVTable(e.GroupIDs) eg
					  INNER JOIN GetCSVTable(@LoginUserGroupIDs) LG
						ON LG.val = eg.val
					)
			)

        OR (@RecordAccess IN (1, 0)
              AND (e.EmployeeID = @LoggedInUserID
				OR e.createdby = @LoggedInUserID)
			)
		)  
    ) AS t1
  )

  SELECT
    *
  FROM CTEEmployeeList
  WHERE
    ROW BETWEEN ((@PageSize * (@FromIndex - 1)) + 1) AND (@PageSize * @FromIndex)

END