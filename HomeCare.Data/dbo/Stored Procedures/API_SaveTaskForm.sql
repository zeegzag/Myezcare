CREATE PROCEDURE [dbo].[API_SaveTaskForm]              
@ReferralID BIGINT,    
@TaskFormMappingID BIGINT,
@ReferralTaskMappingID BIGINT,            
@EBriggsFormID NVARCHAR(MAX)=NULL,            
@OriginalEBFormID NVARCHAR(MAX)=NULL,            
@FormId NVARCHAR(MAX)=NULL,            
@LoggedInID BIGINT,    
@ServerCurrentDateTime DATETIME,    
@SystemID NVARCHAR(100)    
AS              
BEGIN              
            
  IF(@ReferralID = 0) SET @ReferralID = NULL            
  IF(@TaskFormMappingID = 0) SET @TaskFormMappingID = NULL            
              
  IF EXISTS (SELECT TOP 1 EbriggsFormMppingID FROM EbriggsFormMppings WHERE OriginalEBFormID=@OriginalEBFormID AND FormId=@FormId AND          
  EBriggsFormID=@EBriggsFormID AND (ReferralID=@ReferralID OR TaskFormMappingID=@TaskFormMappingID))        
  BEGIN            
            
  UPDATE EbriggsFormMppings SET             
  ReferralID=@ReferralID, TaskFormMappingID=@TaskFormMappingID,ReferralTaskMappingID=@ReferralTaskMappingID, 
  UpdatedDate=@ServerCurrentDateTime,UpdatedBy=@LoggedInID, SystemID=@SystemID             
  WHERE OriginalEBFormID=@OriginalEBFormID AND FormId=@FormId AND  EBriggsFormID=@EBriggsFormID AND (ReferralID=@ReferralID OR TaskFormMappingID=@TaskFormMappingID)            
            
  END            
            
  ELSE            
  BEGIN            
            
  INSERT INTO EbriggsFormMppings
  (EBriggsFormID,OriginalEBFormID,FormId,ReferralID,EmployeeID,TaskFormMappingID,ReferralTaskMappingID,
  CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted)             
  SELECT @EBriggsFormID,@OriginalEBFormID,@FormId,@ReferralID,NULL,@TaskFormMappingID,@ReferralTaskMappingID,
  @ServerCurrentDateTime, @LoggedInID, @ServerCurrentDateTime, @LoggedInID,@SystemID,0                 
  END            
              
END