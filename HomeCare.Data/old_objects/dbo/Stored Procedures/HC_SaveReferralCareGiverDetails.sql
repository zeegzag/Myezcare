CREATE PROCEDURE [dbo].[HC_SaveReferralCareGiverDetails]
@ReferralID BIGINT,      
@AgencyID BIGINT,      
@LoggedInUserID BIGINT,      
@SystemID Varchar(MAX)        
AS        
BEGIN       


IF NOT EXISTS (SELECT 1 FROM ReferralCaregivers RC WHERE RC.ReferralID=@ReferralID AND RC.AgencyID=@AgencyID AND RC.IsDeleted=0)
BEGIN

UPDATE ReferralCaregivers SET IsDeleted=1, EndDate=GETUTCDATE(),UpdatedDate=GETUTCDATE(),UpdatedBy=@LoggedInUserID WHERE IsDeleted=0 AND ReferralID=@ReferralID

INSERT INTO ReferralCaregivers(ReferralID,AgencyID,StartDate,EndDate,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID,IsDeleted)
VALUES(@ReferralID,@AgencyID,GETUTCDATE(),NULL,GETUTCDATE(),@LoggedInUserID,GETUTCDATE(),@LoggedInUserID,@SystemID,0)

END
       

        
END
