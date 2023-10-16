
CREATE PROCEDURE [dbo].[DeleteRefTaskMapping]  
@ReferralTaskMappingID BIGINT,  
@ReferralID BIGINT,  
@TaskTypeTask VARCHAR(100),  
@TaskTypeConclusion VARCHAR(100),  
@LoggedIn BIGINT ,
@ListOfIdsInCsv varchar(max)  
AS  
  
BEGIN  
 IF(LEN(@ListOfIdsInCsv)>0)                  
 BEGIN                    
   UPDATE ReferralTaskMappings SET IsDeleted= 1 ,UpdatedBy=@LoggedIn,UpdatedDate=GETDATE() 
   WHERE ReferralTaskMappingID in (SELECT CAST(Val AS VARCHAR(100)) FROM GetCSVTable(@ListOfIdsInCsv)) AND ReferralID=@ReferralID    
   
  EXEC GetPatientTaskMappings  @ReferralID,@TaskTypeTask,@TaskTypeConclusion 
  END 
   ELSE
   BEGIN
 UPDATE  ReferralTaskMappings SET IsDeleted=1, UpdatedBy=@LoggedIn, UpdatedDate=GETDATE()  
 WHERE  ReferralTaskMappingID=@ReferralTaskMappingID AND ReferralID=@ReferralID  
  
 EXEC GetPatientTaskMappings  @ReferralID,@TaskTypeTask,@TaskTypeConclusion  
 END
END  