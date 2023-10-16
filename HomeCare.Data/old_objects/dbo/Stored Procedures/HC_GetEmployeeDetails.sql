--UpdatedBy: Akhilesh                        
--UpdatedDate:12/JUN/2020                        
--Sedcription: Add DDMaster join and get designation name.            

--exec HC_GetEmployeeDetails 10031,1,2                               
--exec HC_GetEmployeeDetails '40067','Preference','Skill','1','18','17'                              

--UpdatedBy: islam                      
--updatedDate:21/03/2020                      
--description add social security number ssn                       

CREATE PROCEDURE [dbo].[HC_GetEmployeeDetails]
  @EmployeeID bigint,
  @PreferenceType_Preference varchar(100),
  @PreferenceType_Skill varchar(100),
  @DDType_CareType int = 1,
  @DDType_Designation int = 18,
  @DDType_LanguagePreference int = 17,
  @DDType_Gender int = 16,
  @ReferenceCode varchar(2) = 'EM'
AS
BEGIN

  --SELECT                                                       
  --EmployeeID,EmployeeUniqueID,FirstName,MiddleName,LastName,Email,UserName,Password,PasswordSalt,IVRPin,PhoneWork,PhoneHome,DepartmentID,                                                      
  --IsDepartmentSupervisor,SecurityQuestionID,SecurityAnswer,IsActive,IsVerify,RoleID,IsSecurityQuestionSubmitted,CreatedDate,CreatedBy,                                                      
  --UpdatedDate,UpdatedBy,SystemID,IsDeleted,EmpSignature,EmployeeSignatureID,CredentialID,Degree,Department,Roles,MobileNumber,Address,                                                      
  --City,StateCode,ZipCode,Latitude,Longitude,HHA_NPI_ID,ProfileImagePath ,Designation ,CareTypeIds,AssociateWith,ApartmentNo,DateOfBirth,FacilityID ,SocialSecurityNumber,RegularHours,RegularPayhourly,OvertimePayHourly                      
  --FROM Employees Where EmployeeID=@EmployeeID               
  SELECT
    e.EmployeeID,
    e.EmployeeUniqueID,
    e.FirstName,
    e.MiddleName,
    e.LastName,
    e.Email,
    e.UserName,
    e.Password,
    e.PasswordSalt,
    e.IVRPin,
    e.PhoneWork,
    e.PhoneHome,
    e.DepartmentID,
    e.IsDepartmentSupervisor,
    e.SecurityQuestionID,
    e.SecurityAnswer,
    e.IsActive,
    e.IsVerify,
    e.RoleID,
    e.IsSecurityQuestionSubmitted,
    e.CreatedDate,
    e.CreatedBy,
    e.UpdatedDate,
    e.UpdatedBy,
    e.SystemID,
    e.IsDeleted,
    e.EmpSignature,
    e.EmployeeSignatureID,
    e.CredentialID,
    e.Degree,
    e.Department,
    e.Roles,
    e.MobileNumber,
    e.Address,
    e.City,
    e.StateCode,
    e.ZipCode,
    e.Latitude,
    e.Longitude,
    e.HHA_NPI_ID,
    ProfileImagePath,
    e.Designation,
    dm.Title AS DesignationName,
    e.CareTypeIds,
    e.AssociateWith,
    e.ApartmentNo,
    e.DateOfBirth,
    e.FacilityID,
    e.SocialSecurityNumber,
    e.RegularHours,
    e.RegularPayHours,
    e.OvertimePayHours,
    e.GroupIDs,
    e.RegularHourType,
    e.HireDate,
    e.EmpGender,
    e.StateRegistrationID,
    e.ProfessionalLicenseNumber
  FROM Employees e
  LEFT JOIN ddmaster dm
    ON dm.DDMasterID = CONVERT(bigint, e.Designation)
  WHERE
    EmployeeID = @EmployeeID


  SELECT
    *
  FROM Departments
  WHERE
    IsDeleted = 0
  ORDER BY DepartmentName ASC
  SELECT
    *
  FROM SecurityQuestions
  SELECT
    *
  FROM Roles
  ORDER BY RoleName ASC
  SELECT
    *
  FROM EmployeeCredentials
  ORDER BY CredentialID ASC
  SELECT
    *
  FROM States
  ORDER BY StateName ASC



  -- EMPLOYEE PREFERENCES                                                                
  SELECT
    EP.*,
    P.PreferenceName
  FROM EmployeePreferences EP
  INNER JOIN Preferences P
    ON EP.PreferenceID = p.PreferenceID
  WHERE
    EmployeeID = @EmployeeID
    AND P.KeyType = @PreferenceType_Preference



  -- SKILLS MASTER                                             
  IF (@EmployeeID = 0)
    SELECT
      *
    FROM Preferences P
    WHERE
      P.KeyType = @PreferenceType_Skill
      AND IsDeleted = 0
  ELSE
    SELECT DISTINCT
      P.*
    FROM Preferences P
    LEFT JOIN EmployeePreferences EP
      ON EP.PreferenceID = P.PreferenceID
    WHERE
      P.KeyType = @PreferenceType_Skill
      AND ((EP.PreferenceID IS NOT NULL
          AND EP.EmployeeID = @EmployeeID)
        OR P.IsDeleted = 0)
  --AND IsDeleted=0                                     


  SELECT
    EP.PreferenceID
  FROM EmployeePreferences EP
  INNER JOIN Preferences P
    ON EP.PreferenceID = p.PreferenceID
  WHERE
    EmployeeID = @EmployeeID
    AND P.KeyType = @PreferenceType_Skill

  SELECT
    DocumentTypeID = ComplianceID,
    DocumentTypeName = DocumentName
  FROM Compliances
  WHERE
    UserType = 1
    AND IsDeleted = 0

  --SELECT Name=Title,ID=DDMasterID FROM DDMaster where IsDeleted=0 and ItemType=@DDType_CareType                                       
  --SELECT Name=Title,ID=DDMasterID FROM DDMaster where IsDeleted=0 and ItemType=@DDType_Designation                                               

  EXEC GetGeneralMasterList @DDType_CareType
  EXEC GetGeneralMasterList @DDType_Designation

  DECLARE @DDType_EmployeeGroup int =
  (
    SELECT
      DDMasterTypeID
    FROM lu_DDMasterTypes
    WHERE
      [Name] = 'Employee Group'
  )
  EXEC GetGeneralMasterList @DDType_EmployeeGroup


  -- RETURN 0 FOR Language List Model                                           
  SELECT
    LanguageID = DDMasterID,
    Name = Title
  FROM DDMaster
  WHERE
    ItemType = @DDType_LanguagePreference
    AND IsDeleted = 0;

  SELECT DISTINCT
    Value = Value,
    Name = DD.Title
  FROM DDMaster DD
  LEFT JOIN Employees E
    ON E.EmpGender = DD.Value
  WHERE
    ItemType = @DDType_Gender
    AND ((E.EmployeeID IS NOT NULL
        AND E.EmployeeID = @EmployeeID)
      OR DD.IsDeleted = 0)

  -- RETURN 0 FOR ContactTypes List Model                                     
  IF NOT EXISTS
    (
      SELECT
        *
      FROM ContactTypes
      WHERE
        IsDeleted = 0
        AND [ContactTypeName] = 'Employee Address'
    )
  BEGIN
    INSERT INTO ContactTypes
    (
      ContactTypeID,
      ContactTypeName,
      OrderNumber,
      IsDeleted
    )
    VALUES
    (
      7,
      'Employee Address',
      6,
      0
    )
  END

  SELECT
    *
  FROM ContactTypes
  WHERE
    IsDeleted = 0
  ORDER BY OrderNumber;

  -- THIS QUERY WILL FETCH THE CONTACT LIST(ContactInformationList)                                            
  SELECT
    CT.ContactTypeID,
    CT.ContactTypeName,
    CM.ROIType,
    CM.ROIExpireDate,
    C.FirstName,
    C.LastName,
    C.[Address],
    C.LanguageID,
    C.Email,
    C.ApartmentNo,
    C.Phone1,
    C.Phone2,
    C.City,
    C.State,
    C.ZipCode,
    CM.IsDCSLegalGuardian,
    CM.IsEmergencyContact,
    CM.IsNoticeProviderOnFile,
    CM.IsPrimaryPlacementLegalGuardian,
    C.ContactID,
    CM.EmployeeID,
    CM.ClientID,
    CM.ContactMappingID,
    E.FirstName AS EmpFirstName,
    E.LastName AS EmpLastName,
    C.Latitude,
    C.Longitude


  FROM Contacts C
  INNER JOIN EmployeeContactMappings CM
    ON CM.ContactID = C.ContactID
  INNER JOIN ContactTypes CT
    ON CT.ContactTypeID = CM.ContactTypeID
    AND CT.IsDeleted = 0
  INNER JOIN Employees E
    ON E.EmployeeID = CM.CreatedBy
  LEFT JOIN ReferenceMaster RM
    ON RM.ReferenceID = c.ReferenceMasterID
    AND RM.IsActive = 1
  WHERE
    ((RM.ReferenceCode IS NULL)
      OR RM.ReferenceCode = @ReferenceCode)
    AND CM.EmployeeID = @EmployeeID;

  -- RETURN 0 for AddAndListContactInformation model                                          
  SELECT
    0;

  -- RETURN 0  form AddContactModel Model                                          
  SELECT
    0;

  -- RETURN 0 FOR Contact Model                                                                                                                          
  SELECT
    C.FirstName,
    C.LastName,
    C.Address,
    C.City,
    C.State,
    C.ZipCode,
    C.Phone1
  FROM Contacts C
  INNER JOIN EmployeeContactMappings CM
    ON CM.ContactID = C.ContactID
  LEFT JOIN ReferenceMaster RM
    ON RM.ReferenceID = c.ReferenceMasterID
    AND RM.IsActive = 1
  WHERE
    ((RM.ReferenceCode IS NULL)
      OR RM.ReferenceCode = @ReferenceCode)
    AND CM.EmployeeID = @EmployeeID;

  -- RETURN 0 FOR Contact Mapping Model                                                                                               
  SELECT
    0;
  -- RETURN 0 FOR EmergencyContactList Model                                                                                               
  SELECT
    0;
  -- RETURN 0 FOR Contact Mapping Model                                                                      
  SELECT
    0;

  -- THIS QUERY WILL FETCH THE Reference master LIST(ContactInformation)                                          

  IF NOT EXISTS
    (
      SELECT
        *
      FROM ReferenceMaster RM
      WHERE
        RM.Isactive = 1
        AND RM.ReferenceCode = @ReferenceCode
    )
  BEGIN
    INSERT INTO ReferenceMaster
    (
      IsActive,
      ReferenceName,
      ReferenceCode
    )
    VALUES
    (
      1,
      'ContactEmployee',
      @ReferenceCode
    )
  END
  IF NOT EXISTS
    (
      SELECT
        *
      FROM ReferenceMaster RM
      WHERE
        RM.Isactive = 1
        AND RM.ReferenceCode = 'RF'
    )
  BEGIN
    INSERT INTO ReferenceMaster
    (
      IsActive,
      ReferenceName,
      ReferenceCode
    )
    VALUES
    (
      1,
      'ContactReferral',
      'RF'
    )
  END
  SELECT
    RM.ReferenceID,
    RM.IsActive,
    RM.ReferenceName,
    RM.ReferenceCode
  FROM ReferenceMaster RM
  WHERE
    RM.Isactive = 1
    AND RM.ReferenceCode = @ReferenceCode;

  -- RETURN 0 FOR SetEmployeeBillingReportListPage                                  
  SELECT
    0;
  SELECT
    *
  FROM Facilities
  WHERE
    IsDeleted = 0
  ORDER BY FacilityID ASC
  --UpdatedBy: Akhilesh                  
  --UpdatedDate:1/may/2020                  
  --Sedcription: Add OrganizationSettings select statement                  
  SELECT
    OrganizationID,
    SiteLogo,
    SiteName,
    SiteBaseUrl,
    FavIcon
  FROM OrganizationSettings

END