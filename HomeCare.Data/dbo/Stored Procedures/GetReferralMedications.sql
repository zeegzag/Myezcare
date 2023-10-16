--   EXEC GetReferralMedications @ReferralId = '30051', @IsActive = 'False'      
      
-- exec [GetReferralMedications]  60053, 1            
            
CREATE PROCEDURE [dbo].[GetReferralMedications]                
@ReferralId BIGINT,                
@IsActive BIT = 0                
AS                
BEGIN       
update ReferralMedication set IsActive=0 where convert(date,EndDate )<convert(date,getdate() )      
               
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
  RM.PharmacistInstructions,            
  M.MedicationId,            
  P.PhysicianID ,DosageTime   
 ,( SELECT STRING_AGG(Title , ', ') FROM  GetCSVTable(DosageTime)C INNER JOIN DDMaster  DM ON  DM.DDMasterID = C.val) DosageTimeIds  
 FROM ReferralMedication RM                
 LEFT JOIN Medication M on M.MedicationId = RM.MedicationId                
 LEFT JOIN Physicians P on P.PhysicianID = RM.PhysicianID                
 WHERE RM.ReferralID = @ReferralId and RM.IsActive = @IsActive and RM.IsDeleted=0              
--SELECT PhysicianID,dbo.GetGeneralNameFormat(FirstName,LastName) FROM Physicians          
      
      
      
END 