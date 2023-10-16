--exec Rpt_GetClientInformation
CREATE PROCEDURE [dbo].[Rpt_GetClientInformation]   
 @RegionID bigint=0,                                                              
 @AgencyID bigint=0,                                                               
 @ReferralStatusID bigint =0,                                                              
 @StartDate date=null,                                                                      
 @EndDate date=null,                                                                      
 @AssigneeID bigint= 0,                                                              
 @ClientName varchar(100) = NULL,                                                                       
 @PayorID bigint = 0,                                                              
 @NotifyCaseManagerID int = -1,                                                                      
 @ChecklistID INT = -1,                                                                      
 @ClinicalReviewID INT = -1,                                                                      
 @CaseManagerID int = 0,                                                                      
 @ServiceID int = -1,                                                                            
 @AgencyLocationID bigint =0,                                                                    
 @IsSaveAsDraft int=-1,                                                                      
 @AHCCCSID varchar(20)=null,                                                                      
 @CISNumber varchar(20)=null,                                                    
 @IsDeleted bigint=-1  ,    
  @CheckExpireorNot INT = 0                                                            
AS                                                                      
BEGIN                          
 DECLARE @CurrentDate Date=CAST(GETDATE() AS DATE) ;                                            
 SELECT  rs.Status,r.LastName,r.FirstName,r.MiddleName,r.ClientNickName,r.ClientID,r.AHCCCSID,r.CISNumber,dbo.GetAge(r.Dob) Age,                                                  
          CONVERT(VARCHAR(10),CONVERT(datetime,r.Dob ,1),111)as Birthdate,r.Population,                        
           r.Gender,r.Population,r.Title,                            
           r.RecordRequestEmail,r.RateCode,CONVERT(VARCHAR(10),CONVERT(datetime,R.RateCodeStartDate ,1),111)as RateCodeStartDate,                            
           r.RateCode,CONVERT(VARCHAR(10),CONVERT(datetime,R.RateCodeEndDate ,1),111)as RateCodeEndDate,rg.RegionName,                            
           lan.Name as LanguageName,R.HealthPlan,                        
           a.NickName as AgencyName,p.PayorName,al.LocationName,              
              
          PCNT.LastName +' '+ PCNT.FirstName as PlacementName,PCNT.Email as PlacementEmail,              
          PCNT.Phone1 as PlacmentPhone,PCNT.Address+', '+PCNT.City +', '+ PCNT.State+ '-' + PCNT.ZipCode as PlacementFullAddress,              
                  
          LCNT.LastName +' '+ LCNT.FirstName as LegalGuardianName,LCNT.Email as LegalGuardianEmail,              
          LCNT.Phone1 as LegalGuardianPhone,LCNT.Address+', '+LCNT.City +', '+ LCNT.State+ '-' + LCNT.ZipCode as LegalGuardianFullAddress,              
               
             
          (SELECT  STUFF((SELECT ',' + FC.DXCodeName                                                                 
           FROM ReferralDXCodeMappings F join DXCodes Fc on F.DXCodeID=FC.DXCodeID                                                                
           WHERE F.ReferralID=r.ReferralID                                                                
           FOR XML PATH('')),1,1,'')) AS DXCodeName,              
                         
          (SELECT  STUFF((SELECT distinct ', ' + FC.FirstName + ' '+ FC.LastName + ' - '+ FC.Phone1        
           FROM ContactMappings C join Contacts Fc on c.ContactID=Fc.ContactID  and C.IsEmergencyContact=1                            
           WHERE C.ReferralID=r.ReferralID                                  
           FOR XML PATH('')),1,1,'')) AS EmergencyContact,                                  
                                     
           cm.FirstName+', '+ cm.LastName as Casemanager,cm.Email as CMEmail,cm.Phone,cm.Fax as CMFax,                            
           alp.Location as PickUpLocation,ald.Location as DropOffLocation,                                                          
           CONVERT(VARCHAR(10),CONVERT(datetime,r.LastAttendedDate ,1),111)as LastAttendedDate,                                                  
           fcd.Code,  
                           
           r.BehavioralIssue,r.PlacementRequirement,r.OtherInformation,                                                                
           case when r.CareConsent=1then 'YES' Else'NO'END as CareConsent,                                                                
           case when r.SelfAdministrationofMedication=1then 'YES' Else'NO'END as SelfAdministrationofMedication,                                                      
           case when r.HealthInformationDisclosure=1then 'YES' Else'NO'END as HealthInformationDisclosure,                                                                
           case when r.AdmissionRequirements=1then 'YES' Else'NO'END as AdmissionRequirements,                                                                
           case when r.AdmissionOrientation=1then 'YES' Else'NO'END as AdmissionOrientation,                                                                
                           
          (CASE WHEN ZarephathCrisisPlan='N' THEN 'NO'                            
           WHEN ZarephathCrisisPlan='Y'THEN  'YES'                            
           WHEN ZarephathCrisisPlan='NA' THEN 'Not Applicable' END )as ZarephathCrisisPlan,                      
                                                                    
          case when r.PermissionForVoiceMail=1 then 'Authorized' Else'Not Authorized'END as PermissionForVoiceMail,                                            
          case when r.PermissionForEmail=1 then 'Authorized' Else'Not Authorized'END as PermissionForEmail,                                                                
          case when r.PermissionForSMS=1 then 'Authorized' Else'Not Authorized'END as PermissionForSMS,                            
          case when r.PermissionForMail=1 then 'Authorized' Else'Not Authorized'END as PermissionForMail,                            
                                      
        (CASE WHEN PHI='0' THEN 'NO'                            
         WHEN PHI='1'THEN  'YES' END)as PHI,   
		 
		 CASE WHEN r.PHIExpirationDate IS NOT NULL AND r.PHIExpirationDate < @CurrentDate THEN 'Expired' + ' ' +
		            CONVERT(VARCHAR(10),CONVERT(datetime,r.PHIExpirationDate ,1),111)
		   WHEN r.PHIExpirationDate IS NULL OR (r.PHIExpirationDate IS NOT NULL AND r.PHIExpirationDate >= @CurrentDate) 
		   THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.PHIExpirationDate ,1),111)                       
		   END as PHIExpirationDate,

		   CASE WHEN r.ACAssessmentExpirationDate IS NOT NULL AND r.ACAssessmentExpirationDate < @CurrentDate THEN 'Expired' + ' ' +
		            CONVERT(VARCHAR(10),CONVERT(datetime,r.ACAssessmentExpirationDate ,1),111)
		   WHEN r.ACAssessmentExpirationDate IS NULL OR (r.ACAssessmentExpirationDate IS NOT NULL AND r.ACAssessmentExpirationDate >= @CurrentDate) 
		   THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.ACAssessmentExpirationDate ,1),111)                       
		   END as ACAssessmentExpirationDate,


		   
                         
        --(CASE WHEN PHI='0' THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.PHIExpirationDate ,1),111)  
        -- WHEN PHI='1' AND r.PHIExpirationDate IS NOT NULL AND r.PHIExpirationDate < @CurrentDate THEN   'Expired' + ' ' + CONVERT(VARCHAR(10),CONVERT(datetime,r.PHIExpirationDate ,1),111)                          
        -- ELSE CONVERT(VARCHAR(10),CONVERT(datetime,r.PHIExpirationDate ,1),111) END) as PHIExpirationDate,                          

       (select PHIA.NickName from Agencies PHIA where PHIA.AgencyID=r.PHIAgencyID) as PHIAgencyName,                            
                                
                                                
      (CASE WHEN AROI='N' THEN 'NO'                            
          WHEN AROI='Y'THEN  'YES'                            
          WHEN AROI='NA' THEN 'Not Applicable' END )as AROI,                            
     
	   	 CASE  WHEN r.AROIExpirationDate IS NOT NULL AND r.AROIExpirationDate < @CurrentDate THEN 'Expired' + ' ' +
		            CONVERT(VARCHAR(10),CONVERT(datetime,r.AROIExpirationDate ,1),111)
			   WHEN r.AROIExpirationDate IS NULL OR (r.AROIExpirationDate IS NOT NULL AND r.AROIExpirationDate >= @CurrentDate) 
			   THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.AROIExpirationDate ,1),111)                       
			   END as AROIExpirationDate,
	 
	  --(CASE WHEN AROI='N' THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.AROIExpirationDate ,1),111)  
   --       WHEN AROI='Y' AND r.AROIExpirationDate IS NOT NULL AND r.AROIExpirationDate < @CurrentDate THEN  'Expired' + ' ' + CONVERT(VARCHAR(10),CONVERT(datetime,r.AROIExpirationDate ,1),111)                          
   --       WHEN AROI='NA' Then CONVERT(VARCHAR(10),CONVERT(datetime,r.AROIExpirationDate ,1),111)                      
   --   ELSE CONVERT(VARCHAR(10),CONVERT(datetime,r.AROIExpirationDate ,1),111) END) as AROIExpirationDate ,                          
                                  
    (select AROI.NickName from Agencies AROI where AROI.AgencyID=r.AROIAgencyID) as AROIAgencyName,                            
                                 
    (CASE WHEN r.NetworkCrisisPlan='N' THEN 'NO'                            
          WHEN r.NetworkCrisisPlan='Y'THEN  'YES'                            
          WHEN r.NetworkCrisisPlan='NA'THEN 'Not Applicable' END )as NetworkCrisisPlan,                                 
                                     
    (CASE WHEN r.NetworkServicePlan='0' THEN 'NO'                            
          WHEN r.NetworkServicePlan='1' THEN 'YES' END )as NetworkServicePlan,                                 

		  CASE WHEN r.NSPExpirationDate IS NOT NULL AND r.NSPExpirationDate < @CurrentDate THEN 'Expired' + ' ' +
		            CONVERT(VARCHAR(10),CONVERT(datetime,r.NSPExpirationDate ,1),111)
		   WHEN r.NSPExpirationDate IS NULL OR (r.NSPExpirationDate IS NOT NULL AND r.NSPExpirationDate >= @CurrentDate) 
		   THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.NSPExpirationDate ,1),111)                       
		   END as NSPExpirationDate,

    --(CASE WHEN r.NetworkServicePlan='0' THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.NSPExpirationDate ,1),111)                          
    --        WHEN r.NetworkServicePlan='1' AND r.NSPExpirationDate IS NOT NULL AND r.NSPExpirationDate < @CurrentDate
			 --THEN   'Expired' + ' ' + CONVERT(VARCHAR(10),CONVERT(datetime,r.NSPExpirationDate ,1),111)                          
                                      
    --ELSE CONVERT(VARCHAR(10),CONVERT(datetime,r.NSPExpirationDate ,1),111) END) as NSPExpirationDate,                          
                                     
  (CASE WHEN r.NSPGuardianSignature='0' THEN 'NO'                            
        WHEN r.NSPGuardianSignature='1'  THEN  'YES' END )as NSPGuardianSignature,                                 
  (CASE WHEN r.NSPBHPSigned='0' THEN 'NO'                            
        WHEN r.NSPBHPSigned='1'  THEN  'YES' END )as NSPBHPSigned,                             
  (CASE WHEN r.NSPSPidentifyService='N' THEN 'NO'                       
WHEN r.NSPSPidentifyService='Y'  THEN  'YES' ELSE 'NA' END )as NSPSPidentifyService,                             
  
  --(CASE WHEN r.NSPSPidentifyService='0' THEN 'NO'                            
  --      WHEN r.NSPSPidentifyService='1'  THEN  'YES' END )as NSPSPidentifyService ,                             
                           
                                
 (CASE WHEN r.BXAssessment='0' THEN 'NO'                            
       WHEN r.BXAssessment='1'  THEN  'YES' END )as BXAssessment, 
	   
	   CASE WHEN r.BXAssessmentExpirationDate IS NOT NULL AND r.BXAssessmentExpirationDate < @CurrentDate THEN 'Expired' + ' ' +
		            CONVERT(VARCHAR(10),CONVERT(datetime,r.BXAssessmentExpirationDate ,1),111)
		   WHEN r.BXAssessmentExpirationDate IS NULL OR (r.BXAssessmentExpirationDate IS NOT NULL AND r.BXAssessmentExpirationDate >= @CurrentDate) 
		   THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.BXAssessmentExpirationDate ,1),111)                       
		   END as BXAssessmentExpirationDate,

	                                
 --(CASE WHEN r.BXAssessment='0' THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.BXAssessmentExpirationDate ,1),111)                      
 --      WHEN r.BXAssessment='1' AND r.BXAssessmentExpirationDate IS NOT NULL AND r.BXAssessmentExpirationDate < @CurrentDate 
	--   THEN   'Expired' + ' ' + CONVERT(VARCHAR(10),CONVERT(datetime,r.BXAssessmentExpirationDate ,1),111)                          
 -- ELSE CONVERT(VARCHAR(10),CONVERT(datetime,r.BXAssessmentExpirationDate ,1),111) END) as BXAssessmentExpirationDate,                          

                                       
 (CASE WHEN r.ACAssessment='0' THEN 'NO'                            
       WHEN r.ACAssessment='1'  THEN  'YES' END )as ACAssessment,                      
 (CASE WHEN r.NSPBHPSigned='0' THEN 'NO'                            
       WHEN r.NSPBHPSigned='1'  THEN  'YES' END )as NSPBHPSigned,                      
           
 (CASE WHEN r.BXAssessmentBHPSigned='0' THEN 'NO'                            
       WHEN r.BXAssessmentBHPSigned='1'  THEN  'YES' END )as BXAssessmentBHPSigned,                                   
                              
 (CASE WHEN r.Demographic='N' THEN 'NO'                            
       WHEN r.Demographic='Y'THEN  'YES'                 
       WHEN r.Demographic='NA'THEN 'Not Applicable' END )as Demographic,                     
                                    
 --(CASE WHEN r.Demographic='N' THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.DemographicExpirationDate ,1),111)                       
 --      WHEN r.Demographic='Y' AND r.DemographicExpirationDate IS NOT NULL AND r.DemographicExpirationDate < @CurrentDate THEN   'Expired' + ' ' + CONVERT(VARCHAR(10),CONVERT(datetime,r.DemographicExpirationDate ,1),111)                          
 --      WHEN r.Demographic='NA'THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.DemographicExpirationDate ,1),111)                       

	     CASE WHEN r.DemographicExpirationDate IS NOT NULL AND r.DemographicExpirationDate < @CurrentDate THEN 'Expired' + ' ' +
		            CONVERT(VARCHAR(10),CONVERT(datetime,r.DemographicExpirationDate ,1),111)
		   WHEN r.DemographicExpirationDate IS NULL OR (r.DemographicExpirationDate IS NOT NULL AND r.DemographicExpirationDate >= @CurrentDate) 
		   THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.DemographicExpirationDate ,1),111)                       
		   END as DemographicExpirationDate,


  --ELSE CONVERT(VARCHAR(10),CONVERT(datetime,r.DemographicExpirationDate ,1),111) END) as DemographicExpirationDate,                               
                                  
 (CASE WHEN r.SNCD='N' THEN 'NO'                            
       WHEN r.SNCD='Y'THEN  'YES'                            
       WHEN r.SNCD='NA'THEN 'Not Applicable' END )as SNCD,                                    
              
  CONVERT(VARCHAR(10),CONVERT(datetime,r.SNCDExpirationDate,1),111)as SNCDExpirationDate,RFU.UsedRespiteHours,                       

		 CASE WHEN r.ZSPRespite IS NULL OR r.ZSPRespite=0 THEN 'NO'                          
			   WHEN r.ZSPRespite='1'THEN 'YES' END as ZSPRespite,                       
       
	     --(CASE WHEN r.ZSPRespite IS NULL OR ZSPRespite='0' THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.ZSPRespiteExpirationDate ,1),111)                       
			   --WHEN ZSPRespite='1' AND r.ZSPRespiteExpirationDate IS NOT NULL AND r.ZSPRespiteExpirationDate < @CurrentDate                 
			   --THEN 'Expired' + ' ' + CONVERT(VARCHAR(10),CONVERT(datetime,r.ZSPRespiteExpirationDate ,1),111)
      -- 	   END) as ZSPRespiteExpirationDate,                
        
		  CASE WHEN r.ZSPRespiteExpirationDate IS NOT NULL AND r.ZSPRespiteExpirationDate < @CurrentDate THEN 'Expired' + ' ' +
		            CONVERT(VARCHAR(10),CONVERT(datetime,r.ZSPRespiteExpirationDate ,1),111)
		   WHEN r.ZSPRespiteExpirationDate IS NULL OR (r.ZSPRespiteExpirationDate IS NOT NULL AND r.ZSPRespiteExpirationDate >= @CurrentDate) 
		   THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.ZSPRespiteExpirationDate ,1),111)                       
		   END as ZSPRespiteExpirationDate,

		CASE WHEN r.ZSPRespiteGuardianSignature IS NULL OR  r.ZSPRespiteGuardianSignature=0 THEN 'NO'                          
			 WHEN r.ZSPRespiteGuardianSignature=1 THEN  'YES'
			 END as ZSPRespiteGuardianSignature,       
		
		CASE WHEN r.ZSPRespiteBHPSigned IS NULL OR r.ZSPRespiteBHPSigned='0' THEN 'NO'                          
			 WHEN r.ZSPRespiteBHPSigned='1' THEN  'YES' 
			 END as ZSPRespiteBHPSigned,		                  
                              

		 CASE WHEN r.ZSPLifeSkills='0' THEN 'NO'                          
			  WHEN r.ZSPLifeSkills='1'THEN  'YES' END as ZSPLifeSkills,                                 
			
			--(CASE WHEN r.ZSPLifeSkills IS NULL OR ZSPLifeSkills='0' THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.ZSPLifeSkillsExpirationDate ,1),111)                       
			--  WHEN ZSPLifeSkills='1' AND r.ZSPLifeSkillsExpirationDate IS NOT NULL AND r.ZSPLifeSkillsExpirationDate < @CurrentDate                 
			--  THEN   'Expired' + ' ' + CONVERT(VARCHAR(10),CONVERT(datetime,r.ZSPLifeSkillsExpirationDate ,1),111)
	  --	  END) as ZSPLifeSkillsExpirationDate,  
		  
		  CASE WHEN r.ZSPLifeSkillsExpirationDate IS NOT NULL AND r.ZSPLifeSkillsExpirationDate < @CurrentDate THEN 'Expired' + ' ' +
		            CONVERT(VARCHAR(10),CONVERT(datetime,r.ZSPLifeSkillsExpirationDate ,1),111)
		   WHEN r.ZSPLifeSkillsExpirationDate IS NULL OR (r.ZSPLifeSkillsExpirationDate IS NOT NULL AND r.ZSPLifeSkillsExpirationDate >= @CurrentDate) 
		   THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.ZSPLifeSkillsExpirationDate ,1),111)                       
		   END as ZSPLifeSkillsExpirationDate,
	   
	   CASE WHEN r.ZSPLifeSkillsGuardianSignature IS NULL OR  r.ZSPLifeSkillsGuardianSignature=0 THEN 'NO'                          
			 WHEN r.ZSPLifeSkillsGuardianSignature=1 THEN  'YES'
			 END as ZSPLifeSkillsGuardianSignature,       
		
		CASE WHEN r.ZSPLifeSkillsBHPSigned IS NULL OR r.ZSPLifeSkillsBHPSigned='0' THEN 'NO'                          
			 WHEN r.ZSPLifeSkillsBHPSigned='1'THEN  'YES' 
			 END as ZSPLifeSkillsBHPSigned,		                  
         
		CASE WHEN r.ZSPCounselling='0' THEN 'NO'  
			 WHEN r.ZSPCounselling='1'THEN  'YES' END as ZSPCounselling,                     
	
		--CASE WHEN r.ZSPCounselling IS NULL OR ZSPCounselling='0' THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.ZSPCounsellingExpirationDate ,1),111)                       
		--	  WHEN ZSPCounselling='1' AND r.ZSPCounsellingExpirationDate IS NOT NULL AND r.ZSPCounsellingExpirationDate < @CurrentDate                 
		--	  THEN   'Expired' + ' ' + CONVERT(VARCHAR(10),CONVERT(datetime,r.ZSPCounsellingExpirationDate ,1),111) 
		--	  END as ZSPCounsellingExpirationDate,  


          CASE WHEN r.ZSPCounsellingExpirationDate IS NOT NULL AND r.ZSPCounsellingExpirationDate < @CurrentDate THEN 'Expired' + ' ' +
		            CONVERT(VARCHAR(10),CONVERT(datetime,r.ZSPCounsellingExpirationDate ,1),111)
		   WHEN r.ZSPCounsellingExpirationDate IS NULL OR (r.ZSPCounsellingExpirationDate IS NOT NULL AND r.ZSPCounsellingExpirationDate >= @CurrentDate) 
		   THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.ZSPCounsellingExpirationDate ,1),111)                       
		   END as ZSPCounsellingExpirationDate,

		CASE WHEN r.ZSPCounsellingGuardianSignature IS NULL OR  r.ZSPCounsellingGuardianSignature='0' THEN 'NO'                          
			 WHEN r.ZSPCounsellingGuardianSignature='1'THEN  'YES' 
			 END as ZSPCounsellingGuardianSignature,                     

		CASE WHEN  r.ZSPCounsellingBHPSigned IS NULL OR   r.ZSPCounsellingBHPSigned='0' THEN 'NO'                          
			 WHEN r.ZSPCounsellingBHPSigned='1'THEN  'YES'
			 END as ZSPCounsellingBHPSigned,                       
                                              
                               
			CONVERT(VARCHAR(10),CONVERT(datetime,rp.PayorEffectiveDate,1),111)as PayorEffectiveDate,                                                      
			CONVERT(VARCHAR(10),CONVERT(datetime,rp.PayorEffectiveEndDate,1),111)as PayorEffectiveEndDate,                                
			CONVERT(VARCHAR(10),CONVERT(datetime,r.ReferralDate,1),111)as ReferralDate,                                                  
			CONVERT(VARCHAR(10),CONVERT(datetime,r.OrientationDate,1),111)as OrientationDate,                                                  
			CONVERT(VARCHAR(10),CONVERT(datetime,r.AHCCCSEnrollDate,1),111)as AHCCCSEnrollDate,                                
			CONVERT(VARCHAR(10),CONVERT(datetime,r.FirstDOS,1),111)as FirstDOS,                               
			CONVERT(VARCHAR(10),CONVERT(datetime,r.ClosureDate ,1),111)as ClosureDate,r.ClosureReason,                            
			r.ReferralSourceID,rfs.ReferralSourceName,r.ClosureReason,case when r.NeedPrivateRoom=1 then 'YES' Else'NO'END as NeedPrivateRoom,                            
			CASE                               
			   WHEN R.RespiteService=1 AND R.LifeSkillsService=1  AND R.CounselingService=1 THEN 'Respite, Life Skill, Counseling'                           
			   WHEN R.RespiteService=1 AND R.LifeSkillsService=1 THEN  'Respite,  Life Skill'                                  
			   WHEN R.LifeSkillsService=1 AND R.CounselingService=1 THEN  'Life Skill,  Counseling'                                  
			   WHEN R.RespiteService=1 AND R.CounselingService=1 THEN  'Respite,  Counseling'                               
			   WHEN R.LifeSkillsService=1  THEN  'Life Skill'                               
			   WHEN R.RespiteService=1 THEN  'Respite'                               
			   WHEN R.CounselingService=1 THEN  'Counseling' END  as ProgrameName,                            
                                
			 (SELECT STUFF((SELECT ',' + RN.LastName + ' ' + RN.FirstName + ' | '+ rN.AHCCCSID  FROM Referrals RN                               
			  where ReferralID in (select (case when RM.ReferralID1 = R.ReferralID then RM.ReferralID2 else RM.ReferralID1 end) as ReferralID                              
			  from ReferralSiblingMappings RM                               
			   where RM.ReferralID1= R.ReferralID or RM.ReferralID2=R.ReferralID) FOR XML PATH('')),1,1,'') ) ReferralSiblingMappingValue,
     
				 CASE WHEN r.PCMVoiceMail='0' THEN 'No'                          
					  WHEN r.PCMVoiceMail='1'THEN  'Yes' END as PCMVoiceMail, 
				 CASE WHEN r.PCMEmail='0' THEN 'No'                          
					  WHEN r.PCMEmail='1'THEN  'Yes' END as PCMEmail, 
				 CASE WHEN r.PCMSMS='0' THEN 'No'                          
					  WHEN r.PCMSMS='1'THEN  'Yes' END as PCMSMS, 
				CASE WHEN r.PCMMail='0' THEN 'No'                          
					  WHEN r.PCMMail='1'THEN  'Yes' END as PCMMail,

				 CASE WHEN r.PCMVoiceMail='0' AND r.PCMEmail='0' AND r.PCMSMS ='0' AND r.PCMMail='0'  THEN 'None'                          
					  ELSE null END as PCMStatus,R.MonthlySummaryEmail,


					  (SELECT STUFF((SELECT '|' + E.LastName + ', '+E.FirstName 
								FROM Employees E         
								inner join ReferralCaseloads ircl on e.EmployeeID = ircl.EmployeeID   
					   where ReferralID =r.ReferralID         
					   FOR XML PATH('')),1,1,'') ) CaseLoads 

     from Referrals r                                                                 
     left join ReferralStatuses RS on rs.ReferralStatusID=r.ReferralStatusID                             
     left join Employees e on e.EmployeeID=r.Assignee                                                                      
     left join ReferralPayorMappings rp on rp.ReferralID=r.ReferralID and rp.IsActive=1 and rp.IsDeleted=0           
     left join Payors p on p.PayorID=rp.PayorID                                                                      
     left join CaseManagers cm on cm.CaseManagerID=r.CaseManagerID                                                                      
     left join Agencies a on a.AgencyID=cm.AgencyID                                                                      
     left join AgencyLocations al on al.AgencyLocationID=r.AgencyLocationID     
     left join ReferralCheckLists rc on rc.ReferralID=r.ReferralID                                             
     left join ReferralSparForms rsf on rsf.ReferralID=r.ReferralID                                                                      
     left join Employees es on es.EmployeeID=rsf.SparFormCompletedBy   
     left join Employees er on er.EmployeeID=rc.ChecklistCompletedBy                                                                      
     left join Employees eself on eself.EmployeeID=r.CreatedBy                                                                        
     left join Regions rg on r.RegionID=rg.RegionID                                                                
     left join Languages lan on lan.LanguageID=r.LanguageID                                                                
     left join TransportLocations alp on alp.TransportLocationID=r.PickUpLocation                                                
     left join TransportLocations ald on ald.TransportLocationID=r.DropOffLocation                                                                
     left join FrequencyCodes fcd on fcd.FrequencyCodeID=r.FrequencyCodeID                                                                
     left join ReferralSources rfs on rfs.ReferralSourceID=r.ReferralSourceID                                                                
             
     left join ContactMappings PCT on PCT.ReferralID=r.ReferralID AND PCT.ContactTypeID=1         
     left join Contacts PCNT on PCNT.ContactID=PCT.ContactID        
                 
     left join ContactMappings LCT on LCT.ReferralID=r.ReferralID AND LCT.ContactTypeID=2        
     left join Contacts LCNT on LCNT.ContactID=LCT.ContactID            
             
     left join ReferralRespiteUsageLimit RFU on RFU.ReferralID = R.ReferralID 
	  AND RFU.ReferralRespiteUsageLimitID = (
					 SELECT  TOP 1 ReferralRespiteUsageLimitID 
					 FROM ReferralRespiteUsageLimit
					 WHERE ReferralID=R.ReferralID  AND (GETUTCDATE()>=StartDate AND GETUTCDATE()<=EndDate)  ORDER BY ReferralRespiteUsageLimitID DESC 
					 )
	                  
	 left join ReferralCaseloads RCL on RCL.ReferralID= r.ReferralID
                         
   WHERE      --  R.referralID=2841 and                                                        
  ((CAST(@IsDeleted AS BIGINT)=-1) OR r.IsDeleted=@IsDeleted)                                                                            
   AND (( CAST(@AgencyID AS BIGINT)=0) OR r.AgencyID = CAST(@AgencyID AS BIGINT))                                        
   AND (( CAST(@RegionID AS BIGINT)=0) OR r.RegionID= CAST(@RegionID AS BIGINT))                                                                      
   AND (( CAST(@ReferralStatusID AS BIGINT)=0) OR r.ReferralStatusID = CAST(@ReferralStatusID AS BIGINT))           
   AND ((@StartDate is null OR r.ReferralDate>= @StartDate) and (@EndDate is null OR r.ReferralDate<= @EndDate))                     
   AND ((@ClientName IS NULL OR LEN(@ClientName)=0) OR r.FirstName+' '+r.LastName LIKE '%' + @ClientName + '%' OR r.LastName+' '+r.FirstName LIKE '%' + @ClientName + '%')                                      
   AND ((@AHCCCSID IS NULL OR LEN(@AHCCCSID)=0) OR r.AHCCCSID LIKE '%' + @AHCCCSID + '%')                                                                            
   AND ((@CISNumber IS NULL OR LEN(@CISNumber)=0) OR r.CISNumber LIKE '%' + @CISNumber + '%')                        
   AND (( CAST(@PayorID AS BIGINT)=0) OR rp.PayorID = CAST(@PayorID AS BIGINT))                                                        
              
   AND (( CAST(@AssigneeID AS BIGINT)=0) OR r.Assignee = CAST(@AssigneeID AS BIGINT))                                     
   AND (( CAST(@ChecklistID AS INT)=-1) OR ( CAST(@ChecklistID AS INT)=0  AND (rc.IsCheckListCompleted=0 OR rc.IsCheckListCompleted is null ) ) OR rc.IsCheckListCompleted = CAST(@ChecklistID AS INT) )                                                      







    
   AND (( CAST(@ClinicalReviewID AS INT)=-1) OR ( CAST(@ClinicalReviewID AS INT)=0  AND (rsf.IsSparFormCompleted=0 OR rsf.IsSparFormCompleted is null ) ) OR rsf.IsSparFormCompleted = CAST(@ClinicalReviewID AS INT) )                                        







    
   AND (( CAST(@IsSaveAsDraft AS INT)=-1) OR r.IsSaveAsDraft = CAST(@IsSaveAsDraft AS INT))                                                                            
   AND (( CAST(@NotifyCaseManagerID AS INT)=-1) OR r.NotifyCaseManager = CAST(@NotifyCaseManagerID AS INT))                                                                             
   AND (( CAST(@CaseManagerID AS BIGINT)=0) OR r.CaseManagerID = CAST(@CaseManagerID AS BIGINT))                                                     
   AND (( CAST(@ServiceID AS bigint)=-1)                                                                            
   OR  (CAST(@ServiceID AS bigint) = 0 and r.RespiteService = 1)                                                                             
   OR  (CAST(@ServiceID AS bigint) = 1 and r.LifeSkillsService = 1)                                                                            
   OR  (CAST(@ServiceID AS bigint) = 2 and r.CounselingService = 1)
   OR  (CAST(@ServiceID AS bigint) = 3 and r.ConnectingFamiliesService = 1))                                                                            
   AND (( CAST(@AgencyLocationID AS BIGINT)=0) OR r.AgencyLocationID = CAST(@AgencyLocationID AS BIGINT))                                  
   ORDER BY R.LastName ASC                                
END



 
 -- DECLARE @CurrentDate Date=CAST(GETDATE() AS DATE) ;
 --select CASE WHEN r.ZSPCounselling IS NULL OR ZSPCounselling=0 THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.ZSPCounsellingExpirationDate ,1),111)                       
	--		  WHEN ZSPCounselling=1 AND r.ZSPCounsellingExpirationDate IS NOT NULL AND r.ZSPCounsellingExpirationDate < @CurrentDate                 
	--		  THEN   'Expired' + ' ' + CONVERT(VARCHAR(10),CONVERT(datetime,r.ZSPCounsellingExpirationDate ,1),111) 
	--		  END as ZSPCounsellingExpirationDate	,
			  
	--		CASE WHEN r.ZSPLifeSkills IS NULL OR ZSPLifeSkills='0' THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.ZSPLifeSkillsExpirationDate ,1),111)                       
	--		WHEN ZSPLifeSkills='1' AND r.ZSPLifeSkillsExpirationDate IS NOT NULL AND r.ZSPLifeSkillsExpirationDate < @CurrentDate                 
	--		THEN   'Expired' + ' ' + CONVERT(VARCHAR(10),CONVERT(datetime,r.ZSPLifeSkillsExpirationDate ,1),111)
	-- 		END as ZSPLifeSkillsExpirationDate,
		  
	--	   CASE 
		  
	--	   WHEN r.ZSPLifeSkillsExpirationDate IS NOT NULL AND r.ZSPCounsellingExpirationDate < @CurrentDate THEN 'Expired' + ' ' + CONVERT(VARCHAR(10),CONVERT(datetime,r.ZSPCounsellingExpirationDate ,1),111)
	--	   WHEN r.ZSPLifeSkillsExpirationDate IS NULL OR (r.ZSPLifeSkillsExpirationDate IS NOT NULL AND r.ZSPCounsellingExpirationDate >= @CurrentDate) THEN CONVERT(VARCHAR(10),CONVERT(datetime,r.ZSPCounsellingExpirationDate ,1),111)                       
	--	   END as ZSPCounsellingExpirationDate,r.ZSPCounselling

 --from Referrals r where firstname='ashish'
