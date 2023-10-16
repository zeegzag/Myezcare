CREATE PROCEDURE [dbo].[HC_SaveCareFormDetails]                      
@CareFormID bigint,        
@ReferralID bigint,        
@CareFormDate DATETIME=null,        
@LocationOfService NVARCHAR(MAX),         
@Phone NVARCHAR(20),        
@Cell NVARCHAR(20),        
@Email NVARCHAR(300),        
@PSI_StartDate DATETIME=null,        
@PSI_EndDate DATETIME=null,        
@ServiceRequested varchar(MAX) = null,        
@IsMedicallyFrail bit,        
@SpecificFunctionalLimitations NVARCHAR(MAX),        
@IsChargesForServicesRendered int,        
@OnRequest bit,        
@PlanOfSupervision NVARCHAR(MAX),        
@DurationOfServices  NVARCHAR(MAX),        
@StatementsOfGoals  NVARCHAR(MAX),        
@ObjectivesOfServices NVARCHAR(MAX),         
@DischargePlans  NVARCHAR(MAX),        
@DescriptionHowTheTasksArePerformed  NVARCHAR(MAX),        
@PertinentDiagnosis  NVARCHAR(MAX),        
@IsAttachedMedicationForm bit,        
@Medications  NVARCHAR(MAX),        
@Treatments  NVARCHAR(MAX),        
@EquipmentNeeds  NVARCHAR(MAX),        
@Diet NVARCHAR(MAX),        
@NutritionalNeeds  NVARCHAR(MAX),        
@IsPhysiciansOrdersNeeded bit,        
@PhysicianOrdersDescription NVARCHAR(MAX),        
@ClientSignature  NVARCHAR(MAX),        
@ClientSignatureDate DATETIME=null,        
@NurseSignature  NVARCHAR(MAX),        
@NurseSignatureDate DATETIME=null,        
@LoggedInUserId bigint,        
@CurrentDate DATETIME,        
@SystemID  NVARCHAR(MAX)        
AS                                
BEGIN                                  
        
IF(@CareFormID = 0)        
BEGIN        
--Insert        
 INSERT INTO CareForms         
 (ReferralID,CareFormDate,LocationOfService,Phone,Cell,Email,PSI_StartDate,PSI_EndDate,ServiceRequested,IsMedicallyFrail,        
 SpecificFunctionalLimitations,IsChargesForServicesRendered,OnRequest,PlanOfSupervision,DurationOfServices,StatementsOfGoals,        
 ObjectivesOfServices,DischargePlans,DescriptionHowTheTasksArePerformed,PertinentDiagnosis,IsAttachedMedicationForm,        
 Medications,Treatments,EquipmentNeeds,Diet,NutritionalNeeds,IsPhysiciansOrdersNeeded,PhysicianOrdersDescription,        
 ClientSignature,ClientSignatureDate,NurseSignature,NurseSignatureDate,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID)        
 VALUES        
 (@ReferralID,@CareFormDate,@LocationOfService,@Phone,@Cell,@Email,@PSI_StartDate,@PSI_EndDate,@ServiceRequested,@IsMedicallyFrail,        
 @SpecificFunctionalLimitations,@IsChargesForServicesRendered,@OnRequest,@PlanOfSupervision,@DurationOfServices,@StatementsOfGoals,        
 @ObjectivesOfServices,@DischargePlans,@DescriptionHowTheTasksArePerformed,@PertinentDiagnosis,@IsAttachedMedicationForm,        
 @Medications,@Treatments,@EquipmentNeeds,@Diet,@NutritionalNeeds,@IsPhysiciansOrdersNeeded,@PhysicianOrdersDescription,        
 @ClientSignature,@ClientSignatureDate,@NurseSignature,@NurseSignatureDate,@LoggedInUserId,@CurrentDate,@LoggedInUserId,@CurrentDate,@SystemID)        
       
 SELECT * FROM CareForms WHERE CareFormID=SCOPE_IDENTITY()  
     
END        
ELSE        
BEGIN        
--Update        
 UPDATE CareForms        
 SET        
 CareFormDate=@CareFormDate,        
 LocationOfService=@LocationOfService,        
 Phone=@Phone ,        
 Cell=@Cell ,        
 Email=@Email ,        
 PSI_StartDate=@PSI_StartDate ,        
 PSI_EndDate=@PSI_EndDate ,        
 ServiceRequested=@ServiceRequested ,        
 IsMedicallyFrail=@IsMedicallyFrail ,        
 SpecificFunctionalLimitations=@SpecificFunctionalLimitations ,        
 IsChargesForServicesRendered=@IsChargesForServicesRendered ,        
 OnRequest=@OnRequest ,        
 PlanOfSupervision=@PlanOfSupervision ,        
 DurationOfServices=@DurationOfServices ,        
 StatementsOfGoals=@StatementsOfGoals ,        
 ObjectivesOfServices=@ObjectivesOfServices ,        
 DischargePlans=@DischargePlans ,        
 DescriptionHowTheTasksArePerformed=@DescriptionHowTheTasksArePerformed ,        
 PertinentDiagnosis=@PertinentDiagnosis ,        
 IsAttachedMedicationForm=@IsAttachedMedicationForm ,       
 Medications=@Medications ,        
 Treatments=@Treatments ,        
 EquipmentNeeds=@EquipmentNeeds ,        
 Diet=@Diet ,        
 NutritionalNeeds=@NutritionalNeeds ,        
 IsPhysiciansOrdersNeeded=@IsPhysiciansOrdersNeeded ,     
 PhysicianOrdersDescription=@PhysicianOrdersDescription ,        
 ClientSignature=@ClientSignature ,        
 ClientSignatureDate=@ClientSignatureDate ,        
 NurseSignature=@NurseSignature ,        
 NurseSignatureDate=@NurseSignatureDate ,        
 UpdatedBy=@LoggedInUserId ,        
 UpdatedDate=@CurrentDate ,        
 SystemID=@SystemID        
 WHERE CareFormID=@CareFormID   
   
 SELECT * FROM CareForms WHERE ReferralID=@ReferralID    
END        
        
    
            
END
