CREATE PROCEDURE [dbo].[HC_AddReferralBillingAuthrization]                                        
 @ReferralBillingAuthorizationID BIGINT,                            
 @ReferralID BIGINT,                            
 @Type nvarchar(20),                            
 @AuthorizationCode NVARCHAR(20),        
 @ServiceCodeID BIGINT,        
 @Rate decimal(18, 2),        
 @RevenueCode BIGINT,        
 @UnitType INT,        
 @PerUnitQuantity DECIMAL = 0,        
 @RoundUpUnit INT = 0,        
 @MaxUnit INT = 0,        
 @DailyUnitLimit INT = 0,        
 @CareType BIGINT,        
 @ModifierID BIGINT,        
 @TaxonomyID BIGINT,        
 @StartDate DATETIME,                            
 @EndDate DATETIME,                    
 @PayorID  BIGINT,                    
 @StrServiceCodeIDs  NVARCHAR(MAX),                    
 @AllowedTime   BIGINT,             
 @AllowedTimeType  VARCHAR(100),                   
 @loggedInID BIGINT,                            
 @SystemID VARCHAR(100),               
 @PriorAuthorizationFrequencyType BIGINT,        
 @AttachmentFileName NVARCHAR(MAX),         
 @AttachmentFilePath NVARCHAR(MAX),      
 @UnitLimitFrequency INT = NULL,    
 @DxCode NVARCHAR(MAX)=NULL,    
 @DxCodeID NVARCHAR(MAX)=NULL,
 @PayRate decimal(18, 2)=NULL,    
 @FacilityCode NVARCHAR(MAX)=NULL    
AS        
BEGIN                                                  
      set @DxCode= replace(@DxCode,',',' | ')            
-- New change request for Billing Module        
-- Added new fields to creating mapping of payor to service code with referrals.        
-- 18-05-2020: Kundan Kumar Rai        
IF EXISTS(SELECT 1 FROM ReferralBillingAuthorizations WHERE                          
Type=@Type AND (StartDate = @StartDate AND EndDate = @EndDate)              
AND ReferralID=@ReferralID AND PayorID = @PayorID AND ServiceCodeID = @ServiceCodeID         
AND AuthorizationCode = @AuthorizationCode AND IsDeleted=0                   
AND  ReferralBillingAuthorizationID != @ReferralBillingAuthorizationID)                        
 BEGIN                          
  Select -2 AS TransactionResultId;                           
  Return;                          
 END                      
 DECLARE @Title varchar(100);        
 select @Title = Title from  DDMaster where DDMasterID = @PriorAuthorizationFrequencyType;        
IF ((@AllowedTimeType = 'Minutes'  AND  @AllowedTime >  (case         
               when 'Daily' = @Title then 1440        
               when 'Weekly' = @Title then 1440*7        
               when 'Monthly' = @Title then 1440*30        
               when 'Yearly' = @Title then 1440*365        
               END))          
 OR (@AllowedTimeType = 'Hours'  AND  @AllowedTime >  (case         
               when 'Daily' = @Title then 24        
               when 'Weekly' = @Title then 24*7        
               when 'Monthly' = @Title then 24*30        
               when 'Yearly' = @Title then 24*365        
               END)))        
BEGIN                          
  Select -4 AS TransactionResultId;                           
  Return;                          
 END                    
                         
BEGIN TRANSACTION trans                                              
 BEGIN TRY                            
                            
 IF(@ReferralBillingAuthorizationID=0)                            
 BEGIN                            
  INSERT INTO ReferralBillingAuthorizations (ReferralID,Type,AuthorizationCode,PayorID,AllowedTime,StartDate,EndDate,PriorAuthorizationFrequencyType,
  ServiceCodeID,Rate,RevenueCode,UnitType,PerUnitQuantity,RoundUpUnit,MaxUnit,DailyUnitLimit,CareType,ModifierID,TaxonomyID,AllowedTimeType,
  CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID, AttachmentFileName, AttachmentFilePath,
  UnitLimitFrequency,DxCode,DxCodeID,PayRate, FacilityCode)                    
          
  VALUES(@ReferralID,@Type,@AuthorizationCode,@PayorID,@AllowedTime,@StartDate,@EndDate,@PriorAuthorizationFrequencyType,        
  @ServiceCodeID,@Rate,@RevenueCode,@UnitType,@PerUnitQuantity,@RoundUpUnit,@MaxUnit,@DailyUnitLimit,@CareType,@ModifierID,@TaxonomyID,@AllowedTimeType,
  GETUTCDATE(),@loggedInID,GETUTCDATE(),@loggedInID,@SystemID, @AttachmentFileName, @AttachmentFilePath, 
  @UnitLimitFrequency,@DxCode
  ,@DxCodeID,@PayRate, @FacilityCode)       
       
                    
                    
  SET @ReferralBillingAuthorizationID =@@IDENTITY;                    
                    
  INSERT INTO ReferralBillingAuthorizationServiceCodes(ReferralBillingAuthorizationID,ServiceCodeID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted)                    
  SELECT @ReferralBillingAuthorizationID, ServiceCodeID=val,GETUTCDATE(),@loggedInID,GETUTCDATE(),@loggedInID,@SystemID,0 FROM dbo.GetCSVTable(@StrServiceCodeIDs)                                  
 END                            
 ELSE                            
 BEGIN                         
                     
                        
  UPDATE ReferralBillingAuthorizations SET                            
  ReferralID=@ReferralID,Type=@Type,AuthorizationCode=@AuthorizationCode,StartDate=@StartDate,EndDate=@EndDate,                            
  UpdatedDate=GETUTCDATE(),UpdatedBy=@loggedInID,SystemID=@SystemID,                    
  PayorID=@PayorID, AllowedTime=@AllowedTime, PriorAuthorizationFrequencyType=@PriorAuthorizationFrequencyType,          
  AllowedTimeType = @AllowedTimeType,        
  ServiceCodeID = @ServiceCodeID,        
  Rate = @Rate,        
  RevenueCode = @RevenueCode,        
  UnitType = @UnitType,        
  PerUnitQuantity = @PerUnitQuantity,        
  RoundUpUnit = @RoundUpUnit,        
  MaxUnit = @MaxUnit,        
  DailyUnitLimit = @DailyUnitLimit,        
  CareType = @CareType,        
  ModifierID = @ModifierID,        
  TaxonomyID = @TaxonomyID,        
  AttachmentFileName = @AttachmentFileName,        
  AttachmentFilePath = @AttachmentFilePath ,      
  UnitLimitFrequency = @UnitLimitFrequency ,    
  DxCode=@DxCode,    
  DxCodeID=@DxCodeID,
  PayRate = @PayRate,
  FacilityCode = @FacilityCode
  WHERE ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID                      
                      
                    
  DECLARE @TempServiceCodes TABLE(ServiceCodeID BIGINT)                    
  INSERT INTO @TempServiceCodes SELECT VAL FROM dbo.GetCSVTable(@StrServiceCodeIDs)                    
                    
                    
  -- DELETE                    
  UPDATE RS SET RS.IsDeleted=1,RS.UpdatedDate=GETUTCDATE(), RS.UpdatedBy=@loggedInID FROM ReferralBillingAuthorizationServiceCodes RS                    
  LEFT JOIN @TempServiceCodes T ON T.ServiceCodeID=RS.ServiceCodeID                  
  WHERE RS.ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID AND RS.IsDeleted=0   AND T.ServiceCodeID IS NULL                      
                    
  -- UPDATE                    
  UPDATE RS SET  RS.ServiceCodeID=T.ServiceCodeID,RS.UpdatedDate=GETUTCDATE(), RS.UpdatedBy=@loggedInID  FROM ReferralBillingAuthorizationServiceCodes RS                    
  INNER JOIN @TempServiceCodes T ON T.ServiceCodeID=RS.ServiceCodeID AND RS.ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID AND RS.IsDeleted=0                    
  WHERE T.ServiceCodeID IS NOT NULL           
                      
  -- INSERT                    
  INSERT INTO ReferralBillingAuthorizationServiceCodes(ReferralBillingAuthorizationID,ServiceCodeID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted)                    
  SELECT @ReferralBillingAuthorizationID, T.ServiceCodeID,GETUTCDATE(),@loggedInID,GETUTCDATE(),@loggedInID,@SystemID,0                     
  FROM ReferralBillingAuthorizationServiceCodes RS                    
  RIGHT JOIN @TempServiceCodes T ON T.ServiceCodeID=RS.ServiceCodeID AND RS.ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID AND RS.IsDeleted=0                    
  WHERE RS.ReferralBillingAuthorizationServiceCodeID IS  NULL                     
                    
                    
                    
                    
  ---- DELETE                    
  --SELECT * FROM ReferralBillingAuthorizationServiceCodes RS                    
  --LEFT JOIN @TempServiceCodes T ON T.ServiceCodeID=RS.ServiceCodeID AND RS.ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID AND RS.IsDeleted=0                    
  --WHERE T.ServiceCodeID IS NULL                      
                    
  ---- UPDATE                    
  --SELECT * FROM ReferralBillingAuthorizationServiceCodes RS                    
  --INNER JOIN @TempServiceCodes T ON T.ServiceCodeID=RS.ServiceCodeID AND RS.ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID AND RS.IsDeleted=0                    
  --WHERE T.ServiceCodeID IS NOT NULL                     
                      
  ---- INSERT                    
  --SELECT * FROM ReferralBillingAuthorizationServiceCodes RS                    
  --RIGHT JOIN @TempServiceCodes T ON T.ServiceCodeID=RS.ServiceCodeID AND RS.ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID AND RS.IsDeleted=0                    
  --WHERE RS.ReferralBillingAuthorizationServiceCodeID IS  NULL                     
                      
                      
                      
                       
                      
                            
 END          
         
        
 SELECT @ReferralBillingAuthorizationID AS TransactionResultId;               
                           
                        
                        
                        
                        
                        
                        
                        
                         
 IF @@TRANCOUNT > 0                                              
  BEGIN                                               
   COMMIT TRANSACTION trans                                               
  END                                              
 END TRY                               
                            
 BEGIN CATCH                                              
  SELECT -1 AS TransactionResultId,ERROR_MESSAGE() AS ErrorMessage;                                              
  IF @@TRANCOUNT > 0                                              
  BEGIN                                               
   ROLLBACK TRANSACTION trans                                               
  END                                              
 END CATCH                            
END 