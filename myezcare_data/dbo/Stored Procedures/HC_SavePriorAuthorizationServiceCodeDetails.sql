CREATE PROCEDURE [dbo].[HC_SavePriorAuthorizationServiceCodeDetails]
@ReferralBillingAuthorizationServiceCodeID BIGINT,
@ReferralBillingAuthorizationID BIGINT,                  
@ServiceCodeID BIGINT,                  
@DailyUnitLimit INT,                  
@MaxUnitLimit INT,                  
@loggedInID BIGINT,                  
@SystemID VARCHAR(100)                  
                         
AS                                            
BEGIN                                            
                
        
               
BEGIN TRANSACTION trans                                    
 BEGIN TRY                  
                  
 IF(@ReferralBillingAuthorizationServiceCodeID=0)                  
 BEGIN  
                 
  INSERT INTO ReferralBillingAuthorizationServiceCodes (ReferralBillingAuthorizationID,ServiceCodeID,DailyUnitLimit,MaxUnitLimit,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted) VALUES(@ReferralBillingAuthorizationID,@ServiceCodeID,@DailyUnitLimit,@MaxUnitLimit,GETUTCDATE(),@loggedInID,GETUTCDATE(),@loggedInID,@SystemID,0)                  
          
  SET @ReferralBillingAuthorizationServiceCodeID =@@IDENTITY;          
          
  --INSERT INTO ReferralBillingAuthorizationServiceCodes(ReferralBillingAuthorizationID,ServiceCodeID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted)          
  --SELECT @ReferralBillingAuthorizationID, ServiceCodeID=val,GETUTCDATE(),@loggedInID,GETUTCDATE(),@loggedInID,@SystemID,0 FROM dbo.GetCSVTable(@StrServiceCodeIDs)          
          
 END                  
 ELSE                  
 BEGIN               
           
              
  UPDATE ReferralBillingAuthorizationServiceCodes SET                  
  ServiceCodeID=@ServiceCodeID,DailyUnitLimit=@DailyUnitLimit,MaxUnitLimit=@MaxUnitLimit,
  UpdatedDate=GETUTCDATE(),UpdatedBy=@loggedInID,SystemID=@SystemID
  WHERE ReferralBillingAuthorizationServiceCodeID=@ReferralBillingAuthorizationServiceCodeID            
            
          
  --DECLARE @TempServiceCodes TABLE(ServiceCodeID BIGINT)          
  --INSERT INTO @TempServiceCodes SELECT VAL FROM dbo.GetCSVTable(@StrServiceCodeIDs)          
          
          
  ---- DELETE          
  --UPDATE RS SET RS.IsDeleted=1,RS.UpdatedDate=GETUTCDATE(), RS.UpdatedBy=@loggedInID FROM ReferralBillingAuthorizationServiceCodes RS          
  --LEFT JOIN @TempServiceCodes T ON T.ServiceCodeID=RS.ServiceCodeID        
  --WHERE RS.ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID AND RS.IsDeleted=0   AND T.ServiceCodeID IS NULL            
          
  ---- UPDATE          
  --UPDATE RS SET  RS.ServiceCodeID=T.ServiceCodeID,RS.UpdatedDate=GETUTCDATE(), RS.UpdatedBy=@loggedInID  FROM ReferralBillingAuthorizationServiceCodes RS          
  --INNER JOIN @TempServiceCodes T ON T.ServiceCodeID=RS.ServiceCodeID AND RS.ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID AND RS.IsDeleted=0          
  --WHERE T.ServiceCodeID IS NOT NULL           
            
  ---- INSERT          
  --INSERT INTO ReferralBillingAuthorizationServiceCodes(ReferralBillingAuthorizationID,ServiceCodeID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted)          
  --SELECT @ReferralBillingAuthorizationID, T.ServiceCodeID,GETUTCDATE(),@loggedInID,GETUTCDATE(),@loggedInID,@SystemID,0           
  --FROM ReferralBillingAuthorizationServiceCodes RS          
  --RIGHT JOIN @TempServiceCodes T ON T.ServiceCodeID=RS.ServiceCodeID AND RS.ReferralBillingAuthorizationID=@ReferralBillingAuthorizationID AND RS.IsDeleted=0          
  --WHERE RS.ReferralBillingAuthorizationServiceCodeID IS  NULL           
                  
 END                  
                  
                  
 SELECT 1 AS TransactionResultId;                  
               
               
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
