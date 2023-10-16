-- EXEC GetReferralMedicationsById 42

CREATE PROCEDURE GetReferralMedicationsById
@ReferralMedicationID BIGINT 
AS
BEGIN
SELECT RM.ReferralMedicationID,RM.ReferralID,RM.MedicationId,M.MedicationName,RM.PhysicianID,dbo.GetGeneralNameFormat(P.FirstName,P.LastName) AS PhysicianName,RM.Dose,RM.Unit,RM.Frequency,RM.Route,RM.Quantity,RM.StartDate,RM.EndDate,RM.IsActive,
RM.HealthDiagnostics,RM.PatientInstructions,RM.PharmacistInstructions
 FROM ReferralMedication RM
INNER JOIN Medication M ON M.MedicationId=RM.MedicationId
INNER JOIN Physicians P ON P.PhysicianID=RM.PhysicianID

 WHERE ReferralMedicationID=@ReferralMedicationID


END