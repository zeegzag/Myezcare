CREATE PROCEDURE [dbo].[GetDashboardInternalMissingDocument]        
@REFERRALID BIGINT,                
@MISSING VARCHAR(15),        
@EXPIRED VARCHAR(15),

@CareConsent VARCHAR(50),        
@SelfAdministrationofMedication VARCHAR(50),        
@HealthInformationDisclosure VARCHAR(50),        
@AdmissionRequirements VARCHAR(50),        
@AdmissionOrientation VARCHAR(50),        
@ZarephathCrisisPlan VARCHAR(50),        
@PHI VARCHAR(50),
@ZarephathServicePlan  VARCHAR(50)

AS        
BEGIN        
 DECLARE @CurrentDate Date=CAST(GETDATE() AS DATE) ;         
 DECLARE @Temptable TABLE(MissingDocumentName VARCHAR(50),MissingDocumentType VARCHAR(50));          
      
 INSERT INTO @Temptable SELECT @CareConsent,CASE WHEN CareConsent=0 THEN @MISSING END FROM referrals WHERE ReferralID=@REFERRALID   AND CareConsent=0        
 INSERT INTO @Temptable SELECT @SelfAdministrationofMedication,CASE WHEN SelfAdministrationofMedication=0 THEN @MISSING END FROM referrals WHERE ReferralID=@REFERRALID   AND SelfAdministrationofMedication=0        
 INSERT INTO @Temptable SELECT @HealthInformationDisclosure,CASE WHEN HealthInformationDisclosure=0 THEN @MISSING END FROM referrals WHERE ReferralID=@REFERRALID   AND HealthInformationDisclosure=0        
 INSERT INTO @Temptable SELECT @AdmissionRequirements,CASE WHEN AdmissionRequirements=0 THEN @MISSING END FROM referrals WHERE ReferralID=@REFERRALID   AND AdmissionRequirements=0        
 INSERT INTO @Temptable SELECT @AdmissionOrientation,CASE WHEN AdmissionOrientation=0 THEN @MISSING END FROM referrals WHERE ReferralID=@REFERRALID   AND AdmissionOrientation=0        
 INSERT INTO @Temptable SELECT @ZarephathCrisisPlan,CASE WHEN ZarephathCrisisPlan='N' THEN @MISSING END FROM referrals WHERE ReferralID=@REFERRALID   AND ZarephathCrisisPlan='N'        
 INSERT INTO @Temptable        
  SELECT @PHI,CASE WHEN PHI=0 THEN @MISSING WHEN PHI=1 AND PHIExpirationDate IS NOT NULL AND PHIExpirationDate < @CurrentDate THEN @Expired END         
  FROM referrals WHERE ReferralID=@REFERRALID   AND (((PHI=0) or ((PHI=1)and PHIExpirationDate < @CurrentDate))) 
  
 INSERT INTO @Temptable        
  SELECT @ZarephathServicePlan,

  CASE WHEN (RespiteService=0 AND LifeSkillsService=0 AND CounselingService=0 AND ConnectingFamiliesService=0) THEN @MISSING 
       WHEN RespiteService=1 AND ZSPRespite=0 THEN @MISSING WHEN RespiteService=1 AND ZSPRespite=1 AND ZSPRespiteExpirationDate IS NOT NULL AND ZSPRespiteExpirationDate < @CurrentDate THEN @Expired 
	   WHEN LifeSkillsService=1 AND ZSPLifeSkills=0 THEN @MISSING WHEN ZSPLifeSkills=1 AND ZSPLifeSkills=1 AND ZSPLifeSkillsExpirationDate IS NOT NULL AND ZSPLifeSkillsExpirationDate < @CurrentDate THEN @Expired
	   WHEN CounselingService=1 AND ZSPCounselling=0 THEN @MISSING WHEN ZSPCounselling=1 AND ZSPCounselling=1 AND ZSPCounsellingExpirationDate IS NOT NULL AND ZSPCounsellingExpirationDate < @CurrentDate THEN @Expired 
	   WHEN ConnectingFamiliesService=1 AND ZSPConnectingFamilies=0 THEN @MISSING WHEN ZSPConnectingFamilies=1 AND ZSPConnectingFamilies=1 AND ZSPConnectingFamiliesExpirationDate IS NOT NULL AND ZSPConnectingFamiliesExpirationDate < @CurrentDate
	   THEN @Expired END
	   
  FROM referrals WHERE ReferralID=@REFERRALID   AND 
		(
					(RespiteService=0 AND  LifeSkillsService=0 AND CounselingService=0 AND ConnectingFamiliesService=0)
				OR (RespiteService=1 AND (ZSPRespite=0 OR (ZSPRespite=1 AND ZSPRespiteExpirationDate<@CurrentDate)) )
				OR (LifeSkillsService=1 AND (ZSPLifeSkills=0 OR (ZSPLifeSkills=1 AND ZSPLifeSkillsExpirationDate<@CurrentDate)) )
				OR (CounselingService=1 AND (ZSPCounselling=0 OR (ZSPCounselling=1 AND ZSPCounsellingExpirationDate<@CurrentDate)) )
				OR (ConnectingFamiliesService=1 AND (ZSPConnectingFamilies=0 OR (ZSPConnectingFamilies=1 AND ZSPConnectingFamiliesExpirationDate < getdate()))) 		
		 )         
        
-- return list of missing or expired documents        
  SELECT * FROM @Temptable;        
END