
-- exec [HC_AddReferralBillingSetting] 0,14232,2,'P12345',2,'abc123',1,2,3,4,1,'192.168.1.153'  
CREATE PROCEDURE [dbo].[HC_AddReferralBillingSetting]              
@ReferralBillingSettingID BIGINT,          
@ReferralID BIGINT,    
@AuthrizationCodeType INT,  
@AuthrizationCode_CMS1500 NVARCHAR(20) = null,  
@POS_CMS1500 INT=0,  
@AuthrizationCode_UB04 NVARCHAR(20) =null,  
@POS_UB04 INT=0,  
@AdmissionTypeCode_UB04 INT=0,  
@AdmissionSourceCode_UB04 INT=0,  
@PatientStatusCode_UB04  INT=0,  
@LoggedInID BIGINT,    
@SystemID NVARCHAR(100)=NULL    
AS               
 BEGIN              
    DECLARE @TablePrimaryId bigint;     
  
 BEGIN TRANSACTION trans              
 BEGIN TRY              
  IF (@ReferralBillingSettingID>0)    
  Begin   
  --update  
   if(@AuthrizationCodeType = 1)  
   Begin  
    update ReferralBillingSettings   
    set  
    AuthrizationCode_CMS1500=@AuthrizationCode_CMS1500,  
    POS_CMS1500=@POS_CMS1500,  
    UpdatedDate=GETDATE(),  
    UpdatedBy=@LoggedInID,  
    SystemID=@SystemID  
    where  
    ReferralBillingSettingID=@ReferralBillingSettingID  
   End  
   Else  
   Begin  
    update ReferralBillingSettings   
    set  
    AuthrizationCode_UB04=@AuthrizationCode_UB04,  
    POS_UB04=@POS_UB04,  
    AdmissionTypeCode_UB04=@AdmissionTypeCode_UB04,  
    AdmissionSourceCode_UB04=@AdmissionSourceCode_UB04,  
    PatientStatusCode_UB04=@PatientStatusCode_UB04,  
    UpdatedDate=GETDATE(),  
    UpdatedBy=@LoggedInID,  
    SystemID=@SystemID  
    where  
    ReferralBillingSettingID=@ReferralBillingSettingID  
   End  
   SET @TablePrimaryId = @ReferralBillingSettingID;  
  End  
  Else  
  Begin  
  --insert  
   if(@AuthrizationCodeType = 1)  
   Begin  
    insert into ReferralBillingSettings   
    (ReferralID,AuthrizationCode_CMS1500,POS_CMS1500,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)  
    values  
    (@ReferralID,@AuthrizationCode_CMS1500,@POS_CMS1500,GETDATE(),@LoggedInID,GETDATE(),@LoggedInID,@SystemID);  
   End  
   Else  
   Begin  
    insert into ReferralBillingSettings  
    (ReferralID,AuthrizationCode_UB04,POS_UB04,AdmissionTypeCode_UB04,AdmissionSourceCode_UB04,PatientStatusCode_UB04,  
    CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)  
    values  
    (@ReferralID,@AuthrizationCode_UB04,@POS_UB04,@AdmissionTypeCode_UB04,@AdmissionSourceCode_UB04,@PatientStatusCode_UB04,  
    GETDATE(),@LoggedInID,GETDATE(),@LoggedInID,@SystemID);  
   End  
   SET @TablePrimaryId = @@IDENTITY;  
  End  
    
  select * from ReferralBillingSettings where ReferralBillingSettingID=@TablePrimaryId;            
  IF @@TRANCOUNT > 0              
  BEGIN               
  COMMIT TRANSACTION trans               
  END              
      
 END TRY              
 BEGIN CATCH              
  select null;              
  IF @@TRANCOUNT > 0              
  BEGIN               
  ROLLBACK TRANSACTION trans               
  END              
 END CATCH              
END 

