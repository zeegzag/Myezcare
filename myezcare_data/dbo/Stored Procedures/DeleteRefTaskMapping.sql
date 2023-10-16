CREATE PROCEDURE [dbo].[DeleteRefTaskMapping]
@ReferralTaskMappingID BIGINT,
@ReferralID BIGINT,
@TaskTypeTask VARCHAR(100),
@TaskTypeConclusion VARCHAR(100),
@LoggedIn BIGINT
AS

BEGIN
 
 UPDATE  ReferralTaskMappings SET IsDeleted=1, UpdatedBy=@LoggedIn, UpdatedDate=GETDATE()
 WHERE  ReferralTaskMappingID=@ReferralTaskMappingID AND ReferralID=@ReferralID

 EXEC GetPatientTaskMappings  @ReferralID,@TaskTypeTask,@TaskTypeConclusion
END
