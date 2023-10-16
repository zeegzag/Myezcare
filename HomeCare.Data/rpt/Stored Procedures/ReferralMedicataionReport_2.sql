--  EXEC [rpt].[ReferralMedicataionReport] @ReferralID=30051  
CREATE PROCEDURE [rpt].[ReferralMedicataionReport]                          
@ReferralID BIGINT=0         
AS                              
BEGIN  
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()
SELECT DISTINCT R.ReferralID, R.FirstName, R.LastName, dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat) AS FullName, M.MedicationName,
dbo.GetGenericNameFormat(P.FirstName,P.MiddleName, P.LastName,@NameFormat) AS PhysicianName,  
RM.Dose, ISNULL(RM.Quantity, 'NA') AS Amount, RM.Route, RM.Frequency, RM.Unit, RM.HealthDiagnostics, dbo.GetGenericNameFormat(CM.FirstName,'', CM.LastName,@NameFormat) AS CaseManager,   
convert(varchar,RM.CreatedDate,101) as CreatedDate,convert(varchar,RM.ModifiedDate,101) as ModifiedDate, convert(varchar,R.CreatedDate,101) as AdmissionDate, CONCAT(RM.Dose, ' ',RM.Unit) AS DoseWithUnit     
FROM ReferralMedication RM                       
INNER JOIN Referrals R ON R.ReferralID = RM.ReferralID AND R.IsDeleted=0 AND  RM.IsDeleted=0   
INNER JOIN Medication M ON M.MedicationId = RM.MedicationId AND RM.IsDeleted=0   
LEFT JOIN Physicians P ON P.PhysicianID = RM.PhysicianID  
LEFT JOIN CaseManagers CM ON CM.CaseManagerID = R.CaseManagerID AND CM.IsDeleted=0   
WHERE R.ReferralID = @ReferralID AND RM.IsActive=1  
  
END  