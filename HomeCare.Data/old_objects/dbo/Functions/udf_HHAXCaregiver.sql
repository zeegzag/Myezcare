CREATE FUNCTION dbo.udf_HHAXCaregiver (
  @ProviderTaxID nvarchar(max),
  @EmployeeID bigint)
RETURNS nvarchar(max)
AS
BEGIN
  RETURN
  (
    SELECT TOP 1
      @ProviderTaxID [providerTaxID],
      'ExternalID' [qualifier],
      E.EmployeeID [externalID],
      E.SocialSecurityNumber [ssn],
      E.DateOfBirth [dateOfBirth],
      E.LastName [lastName],
      E.FirstName [firstName],
      CASE
        WHEN E.EmpGender = 'F' THEN 'Female'
        WHEN E.EmpGender = 'M' THEN 'Male'
        ELSE 'Other'
      END [gender],
      E.Email [email],
      E.MobileNumber [phoneNumber],
      'Both' [type],
      E.StateRegistrationID [stateRegistrationID],
      E.ProfessionalLicenseNumber [professionalLicenseNumber],
      E.HireDate [hireDate],
      E.[Address] [address.addressLine1],
      E.ApartmentNo [address.addressLine2],
      E.City [address.city],
      E.StateCode [address.state],
      E.ZipCode [address.zipcode]
    FROM dbo.Employees E
    WHERE
      E.EmployeeID = @EmployeeID
      AND (E.HHAXLastSent IS NULL
        OR E.HHAXLastSent < E.UpdatedDate)
    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
  )
END;