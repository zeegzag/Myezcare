         
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
 @TaskCode NVARCHAR(MAX),    
 @TaskOption NVARCHAR(MAX)=NULL,  
 @DefaultTaskOption BIT=0  
AS                                  
BEGIN                                  
                    
PRINT @VisitTaskCategoryID                    
                               
                           
                                
                                  
-- If edit mode                                  
IF(@IsEditMode=0)                                  
BEGIN    
IF EXISTS (SELECT TOP 1 VisitTaskID FROM VisitTasks WHERE VisitTaskDetail=@VisitTaskDetail AND VisitTaskType=@VisitTaskType AND VisitTaskID != @VisitTaskID AND dbo.VisitTasks.CareType=@careType)              
BEGIN                            
 SELECT -1 RETURN;                              
END 
 INSERT INTO VisitTasks                                  
 (VisitTaskType,VisitTaskDetail,VisitTaskCategoryID,VisitTaskSubCategoryID,ServiceCodeID,CreatedBy,CreatedDate,              
 UpdatedBy,UpdatedDate,SystemID,IsDefault,IsRequired,MinimumTimeRequired,SendAlert,VisitType,CareType,Frequency,          
 TaskCode,TaskOption,DefaultTaskOption)              
 VALUES              
 (@VisitTaskType,@VisitTaskDetail,@VisitTaskCategoryID,@VisitTaskSubCategoryID,@ServiceCodeID,@loggedInUserId,GETUTCDATE(),              
 @loggedInUserId,GETUTCDATE(),@SystemID,@IsDefault,@IsRequired,@MinimumTimeRequired,@SendAlert,@VisitType,@CareType,@Frequency,          
 @TaskCode,@TaskOption,@DefaultTaskOption);               
        
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
 TaskCode=@TaskCode ,   
 DefaultTaskOption=@DefaultTaskOption,  
 TaskOption=@TaskOption  
 --TaskResponseCode=@TaskResponseCode    
 WHERE VisitTaskID=@VisitTaskID;                                  
END                                  
        
      
 EXEC UpdateReferralTaskMappings @VisitTaskID, @loggedInUserId, @SystemID      
      
        
 SELECT 1; RETURN;                              
END 