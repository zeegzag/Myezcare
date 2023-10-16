CREATE PROCEDURE DeleteReferralMedication   
 @ReferralMedicationID bigint=null,    
 @LoggedinUserID bigint     
AS     
BEGIN    
    
 UPDATE [dbo].ReferralMedication    
 SET IsDeleted=1,IsActive=1,UpdatedBy=@LoggedinUserID,ModifiedDate=GETUTCDATE()    
 WHERE ReferralMedicationID=@ReferralMedicationID    
    
END