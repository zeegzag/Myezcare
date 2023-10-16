
CREATE procedure [dbo].[SearchMedication]
(
 @MedicationName varchar(100)
	
)
as
begin
	select m.Generic_Name,m.Dosage_Form,'' as [labeler_name],m.Route from Medication m where upper(m.MedicationName) like '%'+upper(@MedicationName)+'%'
end