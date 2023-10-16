---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------      
-- EXEC SaveVisitTask @VisitTaskID = '9', @VisitTaskType = 'Task', @VisitTaskDetail = 'Task 003', @VisitTaskCategoryID = '', @VisitTaskSubCategoryID = '', @ServiceCodeID = '0', @IsDefault = 'False', @IsRequired = 'True', @MinimumTimeRequired = '150', @loggedInUserId = '1', @IsEditMode = 'True', @SystemID = '::1'              
--EXEC SaveVisitTask @VisitTaskID = '54', @VisitTaskType = 'Task', @VisitTaskDetail = 'Other', @VisitTaskCategoryID = '1', @ServiceCodeID = '2', @IsDefault = 'True', @SendAlert = 'False', @IsRequired = 'False', @MinimumTimeRequired = '', @loggedInUserId = '1', @IsEditMode = 'True', @SystemID = '75.83.81.14', @VisitType = '48', @CareType = '42', @Frequency = '1D'      
      
CREATE PROCEDURE [dbo].[SaveVisitTask]                            
 -- Add the parameters for the stored procedure here                            
 @VisitTaskID BIGINT,                            
 @VisitTaskType VARCHAR(100),                            
 @VisitTaskDetail VARCHAR(100),                  
 @VisitTaskCategoryID BIGINT=NULL,              
 @VisitTaskSubCategoryID BIGINT=NULL,                       
 @ServiceCodeID BIGINT,                      
 @IsDefault BIT,            
 @SendAlert BIT,                
 @IsRequired BIT,                       
 @MinimumTimeRequired BIGINT,                       
 @loggedInUserId BIGINT,                            
 @IsEditMode BIT,                            
 @SystemID VARCHAR(100),        
 @VisitType BIGINT,                           
 @CareType BIGINT,      
 @Frequency NVARCHAR(100),    
 @TaskCode NVARCHAR(MAX)    
AS                            
BEGIN                            
              
PRINT @VisitTaskCategoryID              
                         
IF EXISTS (SELECT TOP 1 VisitTaskID FROM VisitTasks WHERE VisitTaskDetail=@VisitTaskDetail AND VisitTaskType=@VisitTaskType AND VisitTaskID != @VisitTaskID AND dbo.VisitTasks.CareType=@careType)        
BEGIN                      
 SELECT -1 RETURN;                        
END                      
                          
                            
-- If edit mode                            
IF(@IsEditMode=0)                            
BEGIN                            
 INSERT INTO VisitTasks                            
 (VisitTaskType,VisitTaskDetail,VisitTaskCategoryID,VisitTaskSubCategoryID,ServiceCodeID,CreatedBy,CreatedDate,        
 UpdatedBy,UpdatedDate,SystemID,IsDefault,IsRequired,MinimumTimeRequired,SendAlert,VisitType,CareType,Frequency,    
 TaskCode)        
 VALUES        
 (@VisitTaskType,@VisitTaskDetail,@VisitTaskCategoryID,@VisitTaskSubCategoryID,@ServiceCodeID,@loggedInUserId,GETUTCDATE(),        
 @loggedInUserId,GETUTCDATE(),@SystemID,@IsDefault,@IsRequired,@MinimumTimeRequired,@SendAlert,@VisitType,@CareType,@Frequency,    
 @TaskCode);         
  
 SET @VisitTaskID = SCOPE_IDENTITY();  
END                            
ELSE                            
BEGIN                            
 UPDATE VisitTasks                             
 SET                                  
 VisitTaskType=@VisitTaskType,                            
 VisitTaskDetail=@VisitTaskDetail,                  
 VisitTaskCategoryID=@VisitTaskCategoryID,              
 VisitTaskSubCategoryID=@VisitTaskSubCategoryID,              
 ServiceCodeID=@ServiceCodeID,                          
 UpdatedBy=@loggedInUserId,                            
 UpdatedDate=GETUTCDATE(),                            
 SystemID=@SystemID,                    
 IsDefault=@IsDefault,                
 IsRequired=@IsRequired,                            
 MinimumTimeRequired=@MinimumTimeRequired,            
 SendAlert=@SendAlert,        
 VisitType=@VisitType,        
 CareType=@CareType,      
 Frequency=@Frequency,    
 TaskCode=@TaskCode    
 WHERE VisitTaskID=@VisitTaskID;                            
END                            
  

 EXEC UpdateReferralTaskMappings @VisitTaskID, @loggedInUserId, @SystemID

  
 SELECT 1; RETURN;                        
END 