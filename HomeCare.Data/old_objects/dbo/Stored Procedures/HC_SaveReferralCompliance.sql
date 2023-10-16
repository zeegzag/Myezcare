CREATE PROCEDURE [dbo].[HC_SaveReferralCompliance]      
@ReferralComplianceID BIGINT,
@ReferralID BIGINT,
@ComplianceID BIGINT,
@Value BIT,
@ExpirationDate DATE=NULL,
@CreatedDate DATETIME,
@CreatedBy BIGINT,
@UpdatedDate DATETIME,
@UpdatedBy BIGINT,
@SystemID VARCHAR(100)
AS       
BEGIN      
	IF(@ReferralComplianceID=0)
		BEGIN
			INSERT INTO ReferralComplianceMappings (ReferralID,ComplianceID,Value,ExpirationDate,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy,SystemID)
			VALUES (@ReferralID,@ComplianceID,@Value,@ExpirationDate,@CreatedDate,@CreatedBy,@UpdatedDate,@UpdatedBy,@SystemID)
		END
	ELSE
		BEGIN
			UPDATE ReferralComplianceMappings SET Value=@Value,ExpirationDate=@ExpirationDate,UpdatedDate=@UpdatedDate,UpdatedBy=@UpdatedBy,SystemID=@SystemID
			WHERE ReferralComplianceID=@ReferralComplianceID
		END
END
