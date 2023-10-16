CREATE PROCEDURE [dbo].[SavePatientHealthPlan]  
 @BenificiaryId int,
 @BenificiaryType nvarchar(2000),  
 @BenificiaryTypeNumber nvarchar(2000),
 @CreatedBy nvarchar(2000),
 @IsActive bit
  
AS  
BEGIN  
  IF NOT EXISTS (select BenificiaryId from PatientHealthPlan where BenificiaryId = @BenificiaryId) 
  BEGIN  
  insert into PatientHealthPlan(BenificiaryType,BenificiaryTypeNumber,CreatedDate,CreatedBy,UpdateDate,UpdateBy,IsDeleted,IsActive)
  values(@BenificiaryType,@BenificiaryTypeNumber,GETUTCDATE(),@CreatedBy,GETUTCDATE(),@CreatedBy,0,@IsActive)   
  END  
 ELSE  
  BEGIN  
   UPDATE [dbo].PatientHealthPlan  
   SET BenificiaryType=@BenificiaryType,BenificiaryTypeNumber=@BenificiaryTypeNumber,UpdateDate=GETUTCDATE()  ,UpdateBy=@CreatedBy,IsActive=@IsActive    
   WHERE BenificiaryId=@BenificiaryId  
  END  
END
