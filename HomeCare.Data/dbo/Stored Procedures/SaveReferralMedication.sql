-- =============================================        
-- Author:  <Author,,Name>        
-- Create date: <Create Date,,>        
-- Description: <Description,,>        
-- =============================================      
 -- EXEC [SaveReferralMedication] '60053','12064','3','1 gm','TEST','33','3','','','1','1','WW','WWW','ERE','34',''     
CREATE PROCEDURE [dbo].[SaveReferralMedication]        
 @ReferralID bigint,        
 @MedicationId bigint,        
 @PhysicianID bigint,        
 @Dose nvarchar(200),        
 @Unit nvarchar(100),        
 @Frequency nvarchar(200),        
 @Route nvarchar(200),        
 @Quantity nvarchar(200),        
 @StartDate Datetime,        
 @EndDate DateTime,        
 @IsActive bit,        
 @SystemID varchar(100),        
 @HealthDiagnostics nvarchar(2000),        
 @PatientInstructions nvarchar(2000),        
 @PharmacistInstructions nvarchar(2000),    
 @ReferralMedicationID bigint=null      ,  
 @DosageTime nvarchar(2000)=null  
  
AS        
BEGIN        
 --DECLARE @NewReferralMedicationID BIGINT;          
 --IF EXISTS (select 1 from ReferralMedication where MedicationId = @MedicationId and PhysicianID = @PhysicianID and ReferralID = @ReferralID)        
 -- BEGIN         
 --  set @NewReferralMedicationID = 0        
 -- END        
IF(@ReferralMedicationID>0)    
BEGIN    
UPDATE ReferralMedication SET     
Dose=@Dose,    
Unit=@Unit,    
Frequency=@Frequency,    
Route=@Route,    
Quantity=@Quantity,    
StartDate=@StartDate,    
EndDate=@EndDate,    
IsActive=@IsActive,    
HealthDiagnostics=@HealthDiagnostics,    
PatientInstructions=@PatientInstructions,    
PharmacistInstructions=@PharmacistInstructions,  
DosageTime=@DosageTime  
 --where MedicationId=@MedicationId   
 where ReferralMedicationID=@ReferralMedicationID  
   
END    
ELSE    
  BEGIN        
   INSERT INTO ReferralMedication        
   VALUES(@ReferralID,@MedicationId,@PhysicianID,@Dose,@Unit,@Frequency,@Route,@Quantity,@StartDate,@EndDate,GETDATE(),GETDATE(),@IsActive,@SystemID,@HealthDiagnostics,@PatientInstructions,@PharmacistInstructions,0,1,@DosageTime)        
  END     
      
      
      
      
          
 -- BEGIN        
 --  INSERT INTO ReferralMedication        
 --  VALUES(@ReferralID,@MedicationId,@PhysicianID,@Dose,@Unit,@Frequency,@Route,@Quantity,@StartDate,@EndDate,GETDATE(),GETDATE(),@IsActive,@SystemID,@HealthDiagnostics,@PatientInstructions,@PharmacistInstructions,0,1)        
 -- END        
        
 --SET @NewReferralMedicationID=@@IDENTITY            
            
 --SELECT @NewReferralMedicationID;            
END