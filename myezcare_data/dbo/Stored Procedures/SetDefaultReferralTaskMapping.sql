CREATE PROCEDURE [dbo].[SetDefaultReferralTaskMapping]      
@ReferralID bigint,    
@CreatedDate DateTime,    
@CreatedBy bigint,    
@SystemID Varchar(MAX)      
AS      
BEGIN     
     
INSERT INTO ReferralTaskMappings (VisitTaskID,IsRequired,IsDeleted,ReferralID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,Frequency)
SELECT V.VisitTaskID,V.IsRequired,0,R.ReferralID,@CreatedDate,@CreatedBy,@CreatedDate,@CreatedBy,@SystemID,V.Frequency
FROM Referrals R       
CROSS JOIN VisitTasks V        
LEFT JOIN ReferralTaskMappings RT ON RT.ReferralID=R.ReferralID AND RT.IsDeleted=0 AND RT.VisitTaskID=V.VisitTaskID      
WHERE V.IsDefault=1 AND V.IsDeleted=0  AND RT.ReferralTaskMappingID IS NULL AND ( ISNULL(@ReferralID,0)=0 OR R.ReferralID=@ReferralID )    
      
END
