--EXEC AddCareTypeSlot @ScheduleID = '0', @ReferralID = '1951', @CareTypeID = '1', @Count = '2', @Frequency = '7', @StartDate = '2019/01/07', @EndDate = '', @TodayDate = '2019/01/10', @loggedInUserID = '1', @SystemID = '::1'    
CREATE PROCEDURE [dbo].[AddCareTypeSlot]    
 @CareTypeTimeSlotID BIGINT,    
 @ReferralID BIGINT,    
 @CareTypeID BIGINT,  
 @Count INT,    
 @Frequency INT,    
 @StartDate DATE,                    
 @EndDate DATE=NULL,    
 @TodayDate DATE,    
 @loggedInUserId BIGINT,                    
 @SystemID VARCHAR(100)                    
AS                              
BEGIN                              
DECLARE @TablePrimaryId bigint;      
DECLARE @ReferralTimeSlotMasterIds VARCHAR(MAX);      
                          
  BEGIN TRANSACTION trans                          
 BEGIN TRY    
    
 DECLARE @MaxDate date;          
 SET @MaxDate = '2099-12-31';          
          
 IF(@EndDate='')            
  SET @EndDate=NULL;    
    
 If EXISTS(SELECT TOP 1 CareTypeTimeSlotID FROM CareTypeTimeSlots WHERE ((StartDate>=@StartDate AND StartDate<=IsNull(@EndDate,@MaxDate)) OR  
 (EndDate>=@StartDate AND EndDate<=IsNull(@EndDate,@MaxDate)) OR (@StartDate>=StartDate AND @StartDate<=IsNull(EndDate,@MaxDate)) 
 OR (@EndDate>=StartDate AND @EndDate<=IsNull(EndDate,@MaxDate))) AND ReferralID=@ReferralID AND CareTypeID=@CareTypeID AND CareTypeTimeSlotID!=@CareTypeTimeSlotID)    
 BEGIN    
 SELECT -1 AS TransactionResultId;    
 Return;    
 END    
                        
 IF(@CareTypeTimeSlotID=0)                              
 BEGIN                              
     INSERT INTO CareTypeTimeSlots                              
   (ReferralID,CareTypeID,Count,Frequency,StartDate,EndDate,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID)                              
   VALUES                              
   (@ReferralID,@CareTypeID,@Count,@Frequency,@StartDate,@EndDate,@loggedInUserId,GETUTCDATE(),@loggedInUserId,GETUTCDATE(),@SystemID);                               
                                 
 SET @TablePrimaryId = @@IDENTITY;                       
 END                              
 ELSE                              
 BEGIN    
   UPDATE CareTypeTimeSlots                               
   SET                                    
      ReferralID=@ReferralID,    
   CareTypeID=@CareTypeID,    
   Count=@Count,    
   Frequency=@Frequency,    
      StartDate=@StartDate,    
      EndDate=@EndDate,    
      UpdatedBy=@loggedInUserId,    
      UpdatedDate=GETUTCDATE(),    
      SystemID=@SystemID    
   WHERE CareTypeTimeSlotID=@CareTypeTimeSlotID;    
 END                              
                          
 SELECT 1 AS TransactionResultId,@TablePrimaryId AS TablePrimaryId;                          
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
