
CREATE PROCEDURE [dbo].[HC_SaveMIFDetail]      
@ReferralID BIGINT,  
@FormName NVARCHAR(200),  
@FromWhoToWho NVARCHAR(100),      
@Type NVARCHAR(20),      
@IsResponseRequired BIT=NULL,      
@ServiceType_AHD BIT,      
@ServiceType_ALS BIT,      
@ServiceType_ERS BIT,      
@ServiceType_HDM BIT,      
@ServiceType_HDS BIT,      
@ServiceType_PSS BIT,      
@ServiceType_EPS BIT,      
@IsInitialServiceOffered BIT=NULL,      
@InitialServiceNoReason NVARCHAR(MAX),      
@InitialServiceDate DATE=NULL,      
@InitialServiceFrequencyID BIGINT,      
@ChangeFYI_RecommendationForChange BIT,      
@ChangeFYI_ChangeInHealthFuncStatus BIT,      
@ChangeFYI_Hospitalization BIT,      
@ChangeFYI_ServiceNotDelivered BIT,      
@ChangeFYI_ChangeInFrequencyByCM BIT,      
@ChangeFYI_ChangeInPhysician BIT,      
@ChangeFYI_Other BIT,      
@ChangeFYI_FYI BIT,      
@Explanation NVARCHAR(MAX),      
@EffectiveDateOfChange DATE=NULL,      
@DischargeReason NVARCHAR(MAX),      
@DateOfDischarge DATE=NULL,      
@Comments NVARCHAR(MAX),      
@PriorAuthorizationDateFrom DATE=NULL,      
@PriorAuthorizationDateTo DATE=NULL,      
@PriorAuthorizationNo NVARCHAR(200),      
@SignaturePath NVARCHAR(MAX),      
@LoggedIdID BIGINT,      
@SystemID VARCHAR(100)      
AS      
BEGIN      
      
DECLARE @TablePrimaryId bigint;      
 BEGIN TRANSACTION trans                      
 BEGIN TRY          
      
 INSERT INTO MIFDetails (ReferralID,FormName,FromWhoToWho,Type,IsResponseRequired,ServiceType_AHD,ServiceType_ALS,ServiceType_ERS,ServiceType_HDM,ServiceType_HDS,      
ServiceType_PSS,ServiceType_EPS,IsInitialServiceOffered,InitialServiceNoReason,InitialServiceDate,InitialServiceFrequencyID,ChangeFYI_RecommendationForChange,      
ChangeFYI_ChangeInHealthFuncStatus,ChangeFYI_Hospitalization,ChangeFYI_ServiceNotDelivered,ChangeFYI_ChangeInFrequencyByCM,ChangeFYI_ChangeInPhysician,      
ChangeFYI_Other,ChangeFYI_FYI,Explanation,EffectiveDateOfChange,DischargeReason,DateOfDischarge,Comments,PriorAuthorizationDateFrom,PriorAuthorizationDateTo,      
PriorAuthorizationNo,SignaturePath,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)      
VALUES(@ReferralID,@FormName,@FromWhoToWho,@Type,@IsResponseRequired,@ServiceType_AHD,@ServiceType_ALS,@ServiceType_ERS,@ServiceType_HDM,@ServiceType_HDS,      
@ServiceType_PSS,@ServiceType_EPS,@IsInitialServiceOffered,@InitialServiceNoReason,@InitialServiceDate,@InitialServiceFrequencyID,@ChangeFYI_RecommendationForChange,      
@ChangeFYI_ChangeInHealthFuncStatus,@ChangeFYI_Hospitalization,@ChangeFYI_ServiceNotDelivered,@ChangeFYI_ChangeInFrequencyByCM,@ChangeFYI_ChangeInPhysician,      
@ChangeFYI_Other,@ChangeFYI_FYI,@Explanation,@EffectiveDateOfChange,@DischargeReason,@DateOfDischarge,@Comments,@PriorAuthorizationDateFrom,@PriorAuthorizationDateTo,      
@PriorAuthorizationNo,@SignaturePath,GETDATE(),@LoggedIdID,GETDATE(),@LoggedIdID,@SystemID)      
      
SELECT * FROM MIFDetails WHERE MIFFormID=@@IDENTITY      
      
      
    
   IF @@TRANCOUNT > 0                      
    BEGIN                       
     COMMIT TRANSACTION trans                       
    END                      
                      
 END TRY                      
 BEGIN CATCH                      
      
  IF @@TRANCOUNT > 0              
   BEGIN                       
    ROLLBACK TRANSACTION trans                       
   END                      
 END CATCH                     
      
END
