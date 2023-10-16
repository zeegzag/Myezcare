GO

CREATE PROCEDURE [dbo].[GetReferralMedications]
@ReferralId BIGINT,
@IsActive BIT = 0
AS
BEGIN
	SELECT 
		RM.ReferralMedicationID,
		RM.ReferralID,
		M.MedicationName,
		P.FirstName + ' ' + P.LastName as PhysicianName,
		RM.Dose,
		RM.Unit,
		RM.Frequency,
		RM.Route,
		RM.Quantity,
		RM.StartDate,
		RM.EndDate,
		RM.CreatedDate,
		RM.ModifiedDate,
		RM.IsActive,
		RM.SystemID,
		RM.HealthDiagnostics,
		RM.PatientInstructions,
		RM.PharmacistInstructions 
	FROM ReferralMedication RM
	LEFT JOIN Medication M on M.MedicationId = RM.MedicationId
	LEFT JOIN Physicians P on P.PhysicianID = RM.PhysicianID
	WHERE RM.ReferralID = @ReferralId and RM.IsActive = @IsActive
END
GO


