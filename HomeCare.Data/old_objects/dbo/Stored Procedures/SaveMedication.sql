Go
CREATE PROCEDURE [dbo].[SaveMedication]
	@MedicationId bigint,
	@MedicationName nvarchar(1000),
	@Generic_Name nvarchar(1000),
	@Brand_Name nvarchar(1000),
	@Product_Type nvarchar(1000),
	@Route nvarchar(1000),
	@Dosage_Form nvarchar(200)
AS
BEGIN
	DECLARE @NewMedicationID BIGINT;  
	IF @MedicationId > 0
		BEGIN 
			UPDATE Medication
			set Route = @Route, Dosage_Form= @Dosage_Form,
			UpdatedDate = GETDATE()
			where MedicationId = @MedicationId
			SET @NewMedicationID=@MedicationId
		END
		ELSE
		BEGIN
			INSERT INTO Medication
			VALUES(@MedicationName,@Generic_Name,@Brand_Name,@Product_Type,@Route,GETDATE(),1,GETDATE(),@Dosage_Form)
			SET @NewMedicationID=@@IDENTITY
		END

	    
    
	SELECT @NewMedicationID; RETURN;    
END
GO


