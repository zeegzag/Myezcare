--EXEC API_CreateCareTypeSchedule @CareTypeID = N'41', @EmployeeID = N'29', @ReferralID = N'225', @ServerCurrentDate = N'2021-01-29', @SystemID = N'75.83.81.14', @StartTime = N'2021-01-29T13:06:00', @EndTime = N'2021-01-29T14:06:00'        
CREATE PROCEDURE [dbo].[API_CreateCareTypeSchedule]                      
@CareTypeID BIGINT,                      
@EmployeeID BIGINT,                      
@ReferralID BIGINT,                      
@ServerCurrentDate DATETIME,                      
@SystemID VARCHAR(100),          
@StartTime TIME(7)=NULL,                          
@EndTime TIME(7)=NULL                          
AS                      
BEGIN                      
          DECLARE @ScheduleID bigint;                 
                      
BEGIN TRANSACTION trans                      
BEGIN TRY                      
                      
DECLARE @MaxDate DATE='2099-12-31'                      
DECLARE @CareTypeTimeSlotID BIGINT            
DECLARE @PayorID BIGINT            
--DECLARE @ScheduleID BIGINT      
--select @StartTime= convert(varchar, getutcdate(), 127)    
 set @StartTime=   [dbo].[GetOrgCurrentDateTime]()  
select @EndTime= convert(varchar, '23:59:00.000000', 127)    
                      
SELECT @CareTypeTimeSlotID=CareTypeTimeSlotID FROM CareTypeTimeSlots WHERE CareTypeID=@CareTypeID AND ReferralID=@ReferralID AND IsDeleted=0                   
AND CONVERT(DATE,@ServerCurrentDate) BETWEEN CONVERT(DATE,StartDate) AND CONVERT(DATE,ISNULL(EndDate,@MaxDate))            
            
SELECT @PayorID=PayorID FROM ReferralPayorMappings             
WHERE ReferralID=@ReferralID AND Precedence=1 AND (CONVERT(DATE,@ServerCurrentDate) BETWEEN PayorEffectiveDate AND PayorEffectiveEndDate)            
                  
--SELECT ScheduleID=@ScheduleID FROM ScheduleMasters WHERE ReferralID=@ReferralID AND CareTypeTimeSlotID=@CareTypeTimeSlotID                  
--AND CONVERT(DATE,@ServerCurrentDate) BETWEEN CONVERT(DATE,StartDate) AND CONVERT(DATE,ISNULL(EndDate,@MaxDate))                  
                      
IF(@CareTypeTimeSlotID>0)                      
 BEGIN                      
  INSERT INTO ScheduleMasters (ReferralID,EmployeeID,CareTypeTimeSlotID,StartDate,EndDate,ScheduleStatusID,                  
  CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,IsDeleted,PayorID,StartTime,EndTime, CareTypeId ,VisitType) VALUES                      
  (@ReferralID,@EmployeeID,@CareTypeTimeSlotID,@ServerCurrentDate+CAST(@StartTime AS DATETIME)              
  ,@ServerCurrentDate+CAST(@EndTime AS DATETIME),                
  2,@EmployeeID,@ServerCurrentDate,@EmployeeID,@ServerCurrentDate,@SystemID,0,@PayorID,@StartTime,@EndTime, @CareTypeID,1 )            
                      
  SET @ScheduleID = @@IDENTITY          
  SELECT @ScheduleID AS TablePrimaryId,1 AS TransactionResultId                      
 END                      
ELSE                      
 BEGIN                      
  INSERT INTO CareTypeTimeSlots (ReferralID,CareTypeID,Count,Frequency,StartDate,EndDate,IsDeleted,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID) VALUES                      
  (@ReferralID,@CareTypeID,1,1,@ServerCurrentDate,@ServerCurrentDate,0,@ServerCurrentDate,@EmployeeID,@ServerCurrentDate,@EmployeeID,@SystemID)                      
                      
  INSERT INTO ScheduleMasters (ReferralID,EmployeeID,CareTypeTimeSlotID,StartDate,EndDate,ScheduleStatusID,                    
  CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,SystemID,IsDeleted,PayorID,StartTime,EndTime, CareTypeId,VisitType ) VALUES                      
  (@ReferralID,@EmployeeID,@@IDENTITY,@ServerCurrentDate+CAST(@StartTime AS DATETIME)              
  ,@ServerCurrentDate+CAST(@EndTime AS DATETIME),                
  2,@EmployeeID,@ServerCurrentDate,@EmployeeID,@ServerCurrentDate,@SystemID,0,@PayorID,@StartTime,@EndTime, @CareTypeID,1 )            
                    
  SET @ScheduleID = @@IDENTITY            
  SELECT @ScheduleID AS TablePrimaryId,1 AS TransactionResultId                      
 END                      
IF @@TRANCOUNT > 0                                                                                                    
 BEGIN                                                                                                    
  COMMIT TRANSACTION trans            
  EXEC [dbo].[ScheduleEventBroadcast] 'CreateSchedule', @ScheduleID,'',''                    
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
GO

