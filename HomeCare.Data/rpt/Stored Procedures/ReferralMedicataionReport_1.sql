--  EXEC [rpt].[ReferralMedicataionReport] @ReferralID=30051
CREATE PROCEDURE [rpt].[ReferralMedicataionReport]                        
@ReferralID BIGINT=0       
AS                            
BEGIN

SELECT DISTINCT R.ReferralID, R.FirstName, R.LastName, CONCAT(R.FirstName,' ',R.LastName) AS FullName, M.MedicationName, CONCAT(P.FirstName,' ',P.LastName) AS PhysicianName,
RM.Dose, ISNULL(RM.Quantity, 'NA') AS Amount, RM.Route, RM.Frequency, RM.Unit, RM.HealthDiagnostics, CONCAT(CM.FirstName,' ',CM.LastName) AS CaseManager, 
convert(varchar,RM.CreatedDate,101) as CreatedDate,convert(varchar,RM.ModifiedDate,101) as ModifiedDate, convert(varchar,R.CreatedDate,101) as AdmissionDate, CONCAT(RM.Dose, ' ',RM.Unit) AS DoseWithUnit   
FROM ReferralMedication RM                     
INNER JOIN Referrals R ON R.ReferralID = RM.ReferralID AND R.IsDeleted=0 AND  RM.IsDeleted=0 
INNER JOIN Medication M ON M.MedicationId = RM.MedicationId AND RM.IsDeleted=0 
LEFT JOIN Physicians P ON P.PhysicianID = RM.PhysicianID
LEFT JOIN CaseManagers CM ON CM.CaseManagerID = R.CaseManagerID AND CM.IsDeleted=0 
WHERE R.ReferralID = @ReferralID AND RM.IsActive=1

END