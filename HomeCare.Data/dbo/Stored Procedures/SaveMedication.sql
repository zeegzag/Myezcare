CREATE PROCEDURE [dbo].[SaveMedication]        
 @MedicationId nvarchar(50),        
 @MedicationName nvarchar(max),        
 @Generic_Name nvarchar(max),        
 @Brand_Name nvarchar(max),        
 @Product_Type nvarchar(max),        
 @Route nvarchar(max),        
 @Dosage_Form nvarchar(max)        
AS        
BEGIN        
 DECLARE @NewMedicationID BIGINT;       
  IF EXISTS (select TOP 1 MedicationId from Medication where MedicationName = @MedicationName and Generic_Name = @Generic_Name)      
  BEGIN     
--SELECT MedicationId FROM Medication WHERE MedicationName=@MedicationName    
  -- SELECT -1 RETURN;
select top 1 MedicationId from Medication where MedicationName like '%'+@MedicationName+'%'   
   END
  --SELECT MedicationId FROM Medication WHERE MedicationName=@MedicationName        
 IF @MedicationId > 0        
  BEGIN         
   UPDATE Medication        
   set Route = @Route, 
       Dosage_Form= @Dosage_Form,        
       UpdatedDate = GETDATE()        
   where MedicationId = @MedicationId        
   SET @NewMedicationID=@MedicationId        
  END        
  ELSE        
  BEGIN        
   INSERT INTO Medication        
   VALUES(@MedicationName,@Generic_Name,@Brand_Name,@Product_Type,@Route,GETDATE(),'1',GETDATE(),@Dosage_Form)        
   SET @NewMedicationID=@@IDENTITY;        
  END        
        
 SELECT @NewMedicationID;           
END