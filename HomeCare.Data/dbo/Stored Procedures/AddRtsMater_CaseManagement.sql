CREATE PROCEDURE [dbo].[AddRtsMater_CaseManagement]             
 @ReferralBillingAuthorizationID BIGINT,              
 @ReferralID BIGINT,      
 @PayorID BIGINT,      
 @StartDate DATE,      
 @EndDate DATE,       
 @TodayDate DATE,      
 @SlotEndDate DATE,      
 @IsEndDateAvailable BIT,      
 @loggedInUserId BIGINT,              
 @SystemID VARCHAR(100),    
 @PriorAuthorizationFrequencyType INT    
AS                        
BEGIN                        
                 
 BEGIN TRANSACTION trans                    
 BEGIN TRY      
  DECLARE @ReferralTimeSlotMasterID BIGINT      
  DECLARE @Result BIGINT = 1      
      
  IF EXISTS      
  (      
   SELECT       
    1       
   FROM       
    ReferralPayorMappings RPM      
   WHERE      
    RPM.ReferralID = @ReferralID      
    AND RPM.PayorID = @PayorID      
    AND RPM.IsActive = 1      
    AND RPM.IsDeleted = 0      
    AND RPM.Precedence = 1      
    AND (PayorEffectiveDate <= @EndDate AND PayorEffectiveEndDate >= @StartDate)      
  )      
  BEGIN      
   -- The payor is primary payor      
   IF EXISTS      
   (      
    SELECT      
     1      
    FROM      
     ReferralTimeSlotMaster RTS      
    WHERE      
     RTS.ReferralBillingAuthorizationID = @ReferralBillingAuthorizationID      
   )      
   BEGIN      
    UPDATE      
     ReferralTimeSlotMaster      
    SET      
     @ReferralTimeSlotMasterID = ReferralTimeSlotMasterID,      
     StartDate = @StartDate,      
     EndDate = @EndDate,      
     UpdatedBy = @loggedInUserId,      
     UpdatedDate = GETDATE()      
    WHERE      
     ReferralBillingAuthorizationID = @ReferralBillingAuthorizationID      
   END      
   ELSE      
   BEGIN      
    EXEC AddRtsMaster @ReferralTimeSlotMasterID = 0, @ReferralID = @ReferralID, @StartDate = @StartDate, @EndDate = @EndDate,      
    @IsEndDateAvailable = @IsEndDateAvailable, @TodayDate = @TodayDate, @SlotEndDate = @SlotEndDate, @loggedInUserId = @loggedInUserId,      
    @SystemID = @SystemID, @ReferralBillingAuthorizationID = @ReferralBillingAuthorizationID      
      
    SET @ReferralTimeSlotMasterID = @@IDENTITY       
   END      
  END      
      
  SELECT @Result AS TransactionResultId, @ReferralTimeSlotMasterID AS TablePrimaryId;                    
      
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