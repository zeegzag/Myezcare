CREATE PROCEDURE [dbo].[HC_AddReferralBillingAuthrization]                          
@ReferralBillingAuthorizationID BIGINT,              
@ReferralID BIGINT,              
@Type nvarchar(20),              
@AuthorizationCode NVARCHAR(20),              
@StartDate DATETIME,              
@EndDate DATETIME,      
@PayorID  BIGINT,      
@StrServiceCodeIDs  NVARCHAR(MAX),      
@AllowedTime   BIGINT,             
@loggedInID BIGINT,              
@SystemID VARCHAR(100)              
                     
AS                                        
BEGIN                                        
            
IF EXISTS(SELECT 1 FROM ReferralBillingAuthorizations WHERE            
Type=@Type AND (StartDate <= @EndDate AND EndDate >= @StartDate)   
AND ReferralID=@ReferralID AND IsDeleted=0 
AND  ReferralBillingAuthorizationID != @ReferralBillingAuthorizationID AND AuthorizationCode=@AuthorizationCode   AND PayorID=@payorid)
 BEGIN            
  Select -2 AS TransactionResultId;             
  Return;            
 END        
       
       
           
BEGIN TRANSACTION trans                                
 BEGIN TRY              
              
 IF(@ReferralBillingAuthorizationID=0)              
 BEGIN              
  INSERT INTO ReferralBillingAuthorizations (ReferralID,Type,AuthorizationCode,PayorID,AllowedTime,StartDate,EndDate,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)      
  VALUES(@ReferralID,@Type,@AuthorizationCode,@PayorID,@AllowedTime,@StartDate,@EndDate,GETUTCDATE(),@loggedInID,GETUTCDATE(),@loggedInID,@SystemID)              
      
      
  SET @ReferralBillingAuthorizationID =@@IDENTITY;      
      
  INSERT INTO ReferralBillingAuthorizationServiceCodes(ReferralBillingAuthorizationID,ServiceCodeID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted)      
  SELECT @ReferralBillingAuthorizationID, ServiceCodeID=val,GETUTCDATE(),@loggedInID,GETUTCDATE(),@loggedInID,@SystemID,0 FROM dbo.GetCSVTable(@StrServiceCodeIDs)      
      
 END              
 ELSE              
 BEGIN           
       
          
  UPDATE ReferralBillingAuthorizations SET              
  ReferralID=@ReferralID,Type=@Type,AuthorizationCode=@AuthorizationCode,StartDate=@StartDate,EndDate=@EndDate,              
  UpdatedDate=GETUTCDATE(),UpdatedBy=@loggedInID,SystemID=@SystemID,      
  PayorID=@PayorID, AllowedTime=@AllowedTime              
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

