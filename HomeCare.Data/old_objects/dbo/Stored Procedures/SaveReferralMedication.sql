GO
CREATE PROCEDURE [dbo].[SaveReferralMedication]
	@ReferralID bigint,
	@MedicationId bigint,
	@PhysicianID bigint,
	@Dose nvarchar(200),
	@Unit int,
	@Frequency nvarchar(200),
	@Route nvarchar(200),
	@Quantity nvarchar(200),
	@StartDate Datetime,
	@EndDate DateTime,
	@IsActive bit,
	@SystemID varchar(100),
	@HealthDiagnostics nvarchar(2000),
	@PatientInstructions nvarchar(2000),
	@PharmacistInstructions nvarchar(2000)
AS
BEGIN
	DECLARE @NewReferralMedicationID BIGINT;  
	IF EXISTS (select 1 from ReferralMedication where MedicationId = @MedicationId and PhysicianID = @PhysicianID and ReferralID = @ReferralID)
		BEGIN 
			set @NewReferralMedicationID = 0
		END
		ELSE
		BEGIN
			INSERT INTO ReferralMedication
			VALUES(@ReferralID,@MedicationId,@PhysicianID,@Dose,@Unit,@Frequency,@Route,@Quantity,@StartDate,@EndDate,GETDATE(),GETDATE(),@IsActive,@SystemID,@HealthDiagnostics,@PatientInstructions,@PharmacistInstructions)
		END

	SET @NewReferralMedicationID=@@IDENTITY    
    
	SELECT @NewReferralMedicationID;    
END
GO


