CREATE PROCEDURE [dbo].[Rpt_GetServicePlanExpiration]
    @RegionID BIGINT = 0 ,
    @AgencyID BIGINT = 0 ,
    @ReferralStatusID BIGINT = 0 ,
    @StartDate DATE = NULL ,
    @EndDate DATE = NULL ,
    @AssigneeID BIGINT = 0 ,
    @ClientName VARCHAR(100) = NULL ,
    @PayorID BIGINT = 0 ,
    @NotifyCaseManagerID INT = -1 ,
    @ChecklistID INT = -1 ,
    @ClinicalReviewID INT = -1 ,
    @CaseManagerID INT = 0 ,
    @ServiceID INT = -1 ,
    @AgencyLocationID BIGINT = 0 ,
    @IsSaveAsDraft INT = -1 ,
    @AHCCCSID VARCHAR(20) = NULL ,
    @CISNumber VARCHAR(20) = NULL ,
    @IsDeleted BIGINT = -1 ,
    @CheckExpireorNot INT = 0
AS 
    BEGIN                                      
        DECLARE @CurrentDate DATE= CAST(GETDATE() AS DATE);              


 
      
        SELECT  rs.Status ,r.LastName+', '+r.FirstName AS Name, r.CISNumber , r.AHCCCSID ,CONVERT(VARCHAR(10), CONVERT(DATETIME, r.Dob, 1), 111) AS Birthdate ,
				a.NickName AS AgencyName ,lan.Name as LanguageName,
                CASE WHEN r.CareConsent = 1 THEN 'YES' ELSE 'NO' END AS CareConsent ,
                CASE WHEN r.RespiteService = 1 THEN 'YES' ELSE 'NO' END AS RespiteService ,
                CONVERT(VARCHAR(10), CONVERT(DATETIME, r.ZSPRespiteExpirationDate, 1), 111) AS ZSPRespiteExpirationDate ,
            

    CASE WHEN r.ZSPRespiteGuardianSignature = 1 THEN 'YES' ELSE 'NO' END AS ZSPRespiteGuardianSignature ,
                CASE WHEN r.ZSPRespiteBHPSigned = 1 THEN 'YES' ELSE 'NO' END AS ZSPRespiteBHPSigned ,
                CASE WHEN r.LifeSkillsService =1 THEN 'YES' ELSE 'NO' END AS LifeSkillsService ,
                CONVERT(VARCHAR(10), CONVERT(DATETIME, r.ZSPLifeSkillsExpirationDate, 1), 111) AS ZSPLifeSkillsExpirationDate ,
                CASE WHEN r.ZSPLifeSkillsGuardianSignature = 1 THEN 'YES' ELSE 'NO' END AS ZSPLifeSkillsGuardianSignature ,
                CASE WHEN r.ZSPLifeSkillsBHPSigned = 1 THEN 'YES' ELSE 'NO' END AS ZSPLifeSkillsBHPSigned ,
                CASE WHEN r.CounselingService = 1 THEN 'YES' ELSE 'NO' END AS ZSPCounselling ,
				CONVERT(VARCHAR(10), CONVERT(DATETIME, r.ZSPCounsellingExpirationDate, 1), 111) AS ZSPCounsellingExpirationDate ,
                CASE WHEN r.ZSPCounsellingGuardianSignature = 1 THEN 'YES' ELSE 'NO' END AS ZSPCounsellingGuardianSignature ,
                CASE WHEN r.ZSPCounsellingBHPSigned = 1 THEN 'YES' ELSE 'NO' END AS ZSPCounsellingBHPSigned ,
                CASE WHEN r.SelfAdministrationofMedication = 1 THEN 'YES' ELSE 'NO' END AS SelfAdministrationofMedication ,
                CASE WHEN r.HealthInformationDisclosure = 1 THEN 'YES' ELSE 'NO' END AS HealthInformationDisclosure ,
				CASE WHEN r.PHI = 1 THEN 'YES' ELSE 'NO' END AS PHI ,
                CASE WHEN r.AdmissionRequirements = 1 THEN 'YES' ELSE 'NO' END AS AdmissionRequirements ,
                CASE WHEN r.AdmissionOrientation = 1 THEN  'YES' ELSE 'NO' END AS AdmissionOrientation ,
                CASE WHEN r.PermissionForVoiceMail = 1 THEN 'Authorized' ELSE 'Not Authorized' END AS PermissionForVoiceMail ,
                CASE WHEN r.ZarephathCrisisPlan = 'Y' THEN 'YES' WHEN r.ZarephathCrisisPlan = 'N' THEN 'NO' WHEN r.ZarephathCrisisPlan = 'NA' THEN 'Not Applicable' ELSE '' END AS ZarephathCrisisPlan ,
                CONVERT(VARCHAR(10), CONVERT(DATETIME, r.NCPExpirationDate, 1), 111) AS NCPExpirationDate ,
				 CASE WHEN r.RespiteService = 1 THEN 'YES' ELSE 'NO' END AS RespiteService ,
                CASE WHEN r.ACAssessment = 1 THEN 'YES' ELSE 'NO' END AS ACAssessment ,
                CASE WHEN r.PermissionForEmail = 1 THEN 'Authorized' ELSE 'Not Authorized' END AS PermissionForEmail ,
                CASE WHEN r.PermissionForSMS = 1 THEN 'Authorized' ELSE 'Not Authorized' END AS PermissionForSMS,
				CONVERT(VARCHAR(10), (select min(StartDate) from ScheduleMasters where ReferralID=R.ReferralID and StartDate >= GETUTCDATE() and IsDeleted=0), 111) AS NextAttDate 
				
				FROM Referrals r
				LEFT JOIN ReferralStatuses RS ON rs.ReferralStatusID = r.ReferralStatusID
                --LEFT JOIN Employees e ON e.EmployeeID = r.Assignee
                --LEFT JOIN ReferralPayorMappings rp ON rp.ReferralID = r.ReferralID AND rp.IsActive = 1 AND rp.IsDeleted = 0
                --LEFT JOIN Payors p ON p.PayorID = rp.PayorID
                --LEFT JOIN CaseManagers cm ON cm.CaseManagerID = r.CaseManagerID
      
           LEFT JOIN Agencies a ON a.AgencyID = r.AgencyID  
				 --LEFT JOIN AgencyLocations al ON al.AgencyLocationID = r.AgencyLocationID
                --LEFT JOIN ReferralCheckLists rc ON rc.ReferralID = r.ReferralID
                --LEFT JOIN ReferralSparForms rsf ON rsf.ReferralID = r.ReferralID
				--LEFT JOIN Employees es ON es.EmployeeID = rsf.SparFormCompletedBy
                --LEFT JOIN Employees er ON er.EmployeeID = rc.ChecklistCompletedBy
                --LEFT JOIN Employees eself ON eself.EmployeeID = r.CreatedBy
                --LEFT JOIN Regions rg ON r.RegionID = rg.RegionID
                LEFT JOIN Languages lan ON lan.LanguageID = r.LanguageID
                --LEFT JOIN AgencyLocations alp ON alp.AgencyLocationID = r.PickUpLocation
                --LEFT JOIN AgencyLocations ald ON ald.AgencyLocationID = r.DropOffLocation
                --LEFT JOIN FrequencyCodes fcd ON fcd.FrequencyCodeID = r.FrequencyCodeID
                --LEFT JOIN ReferralSources rfs ON rfs.ReferralSourceID = r.ReferralSourceID
                --LEFT JOIN ContactMappings ct ON ct.ReferralID = r.ReferralID AND ( ct.IsPrimaryPlacementLegalGuardian = 1 OR ct.IsDCSLegalGuardian = 1)
                --LEFT JOIN Contacts cnt ON cnt.ContactID = ct.ContactID
  
      WHERE  
	            ( ( CAST(@IsDeleted AS BIGINT) = -1 ) OR r.IsDeleted = @IsDeleted ) AND ( ( CAST(@AgencyID AS BIGINT) = 0 ) OR r.AgencyID = CAST(@AgencyID AS BIGINT))
			AND ( ( CAST(@RegionID AS BIGINT) = 0 ) OR r.RegionID = CAST(@RegionID AS 
BIGINT))
            AND ( ( CAST(@ReferralStatusID AS BIGINT) = 0 ) OR r.ReferralStatusID = CAST(@ReferralStatusID AS BIGINT))
			AND ( ( @StartDate IS NULL OR r.ReferralDate >= @StartDate ) AND ( @EndDate IS NULL OR r.ReferralDate <= @EndDate ) )


            AND ( ( CAST(@ServiceID AS BIGINT) = -1 ) OR ( CAST(@ServiceID AS BIGINT) = 0 AND r.RespiteService = 1)
                      OR ( CAST(@ServiceID AS BIGINT) = 1 AND r.LifeSkillsService = 1 )
                      OR ( CAST(@ServiceID AS BIGINT) = 2 AND r.CounselingService = 1)
					  OR  (CAST(@ServiceID AS bigint) = 3 and r.ConnectingFamiliesService = 1)
                )
                 
             AND ( 
                 (CAST(@CheckExpireorNot AS BIGINT) = -1) 
                  OR ( CAST(@CheckExpireorNot AS BIGINT) = 0
                   AND (( ( CAST(@ServiceID AS BIGINT) = 1 ) AND r.ZSPLifeSkillsExpirationDate < @CurrentDate) 
                    OR ( ( CAST(@ServiceID AS BIGINT) = 2 ) AND r.ZSPCounsellingExpirationDate < @CurrentDate)
                    OR ( ( CAST(@ServiceID AS BIGINT) = 0 ) AND r.ZSPRespiteExpirationDate < @CurrentDate)
                    OR ( ( CAST(@ServiceID AS BIGINT) = -1 )AND ( r.ZSPRespiteExpirationDate < @CurrentDate
                                                                OR r.ZSPLifeSkillsExpirationDate < @CurrentDate
                                                                OR r.ZSPCounsellingExpirationDate < @CurrentDate
                                                                )
                       )))
                   OR
                    (CAST(@CheckExpireorNot AS BIGINT) = 1
                     AND(
                        (( CAST(@ServiceID AS BIGINT) = 1)  AND (DATEDIFF(day,r.ZSPLifeSkillsExpirationDate,GETDATE()) <= 90) and r.ZSPLifeSkillsExpirationDate > @CurrentDate)
                     OR (( CAST(@ServiceID AS BIGINT) = 2)  AND (DATEDIFF(day,r.ZSPCounsellingExpirationDate,GETDATE()) <= 90) and r.ZSPCounsellingExpirationDate >@CurrentDate)
                     OR (( CAST(@ServiceID AS BIGINT) = 0)  AND (DATEDIFF(day,

r.ZSPRespiteExpirationDate,GETDATE()) <= 90)and r.ZSPRespiteExpirationDate >@CurrentDate)
                     OR (( CAST(@ServiceID AS BIGINT) = -1 )
                                      AND ((DATEDIFF(day,r.ZSPRespiteExpirationDate,GETDATE()) <= 90 and

 r.ZSPRespiteExpirationDate >@CurrentDate )
                                      OR (DATEDIFF(day,r.ZSPLifeSkillsExpirationDate,GETDATE()) <= 90 and r.ZSPLifeSkillsExpirationDate >@CurrentDate )
                                      OR (DATEDIFF(day,r.ZSPCounsellingExpirationDate,GETDATE()) <= 90 and r.ZSPCounsellingExpirationDate >@CurrentDate ))
                        ))
                     )
                    )
        ORDER BY R.LastName ASC        
                             
    END
