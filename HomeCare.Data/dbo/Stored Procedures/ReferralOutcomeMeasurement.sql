CREATE procedure [dbo].[ReferralOutcomeMeasurement]                                  
@ReferralID  bigint,
@ReferralOutcomeMeasurementID bigint,
@LoginID bigint
as                                                      
  SET NOCOUNT ON;                
  
  if(@ReferralOutcomeMeasurementID > 0)
	BEGIN
		SELECT ROM.*,ES.SignaturePath as Signature,E.LastName+', '+E.FirstName As CompletedBy 
		FROM ReferralOutcomeMeasurements ROM
		inner join Employees E on E.EmployeeID=ROM.UpdatedBy
		left join EmployeeSignatures ES on ES.EmployeeSignatureID=E.EmployeeSignatureID
		where ROM.ReferralOutcomeMeasurementID=@ReferralOutcomeMeasurementID
	END
  ELSE
	BEGIN
		SELECT  ES.SignaturePath as Signature , E.LastName+', '+E.FirstName As CompletedBy  FROM Employees E 
		LEFT JOIN EmployeeSignatures ES on ES.EmployeeSignatureID=E.EmployeeSignatureID
		where  E.EmployeeID=@LoginID
	END