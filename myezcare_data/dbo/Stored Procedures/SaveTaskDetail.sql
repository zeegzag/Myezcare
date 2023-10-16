CREATE PROCEDURE [dbo].[SaveTaskDetail]  
@ReferralTaskMappingID BIGINT,
@Comment NVARCHAR(100),
@Frequency NVARCHAR(MAX),
@UpdatedDate DATETIME,
@LoggedInID BIGINT,    
@SystemID VARCHAR(30)  
AS    
BEGIN    

UPDATE ReferralTaskMappings SET Frequency=@Frequency,Comment=@Comment,UpdatedDate=@UpdatedDate,UpdatedBy=@LoggedInID,SystemID=@SystemID
WHERE ReferralTaskMappingID=@ReferralTaskMappingID
SELECT 1 RETURN;
    
END
