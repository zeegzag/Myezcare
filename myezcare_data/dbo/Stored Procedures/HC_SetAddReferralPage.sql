
--updated by --vikas srivastava      
--updated Date -- 14-06-2019  
--Description-- change id to value in @DDType_CareType  
  
  
CREATE PROCEDURE [dbo].[HC_SetAddReferralPage]        
@ReferralID BIGINT = 0,                                                                          
@EmployeeID BIGINT = 0,                                                                          
@DropOffLocation BIGINT = 0,                                                                          
@PickUpLocation BIGINT = 0,                                                                          
@AgencyID BIGINT=0,                                                                          
@AgencyLocationID BigInt=0,                                                            
@PreferenceType_Preference VARCHAR(100),                                                              
@PreferenceType_Skill VARCHAR(100),                                                                     
@DDType_CareType int =1,                                                
@DDType_AdmissionType int =6,                                                   
@DDType_AdmissionSource int=7,                                                   
@DDType_PatientStatus  int=8  ,                                            
@DDType_FacilityCode  int=10,                                              
@DDType_VisitType  int=9,                                
--@CareType INT                                
@DDType_PatientSystemStatus INT = 12,                                
@DDType_PatientFrequencyCode INT = 11 ,                      
@DDType_Gender INT = 16 ,                      
@DDType_LanguagePreference INT = 17            
,@DDType_BeneficiaryType INT = 22        

                            
AS                                                                          
BEGIN                                                                          
                                                                           
 SELECT @EmployeeID = Assignee, @DropOffLocation = DropOffLocation,@PickUpLocation=PickUpLocation,@AgencyID=AgencyID,@AgencyLocationID=AgencyLocationID FROM Referrals WHERE ReferralID=@ReferralID                                                            
  
    
      
        
          
            
              
                
                
 DECLARE @ReferralCareGiver_AgencyID BIGINT=0;                
 SELECT TOP 1 @ReferralCareGiver_AgencyID=AgencyID FROM ReferralCareGivers WHERE ReferralID=@ReferralID AND IsDeleted=0                
                
                        
                        
 SELECT FacilityID, FacilityName  FROM Facilities WHERE IsDeleted=0                        
                                                                           
 SELECT R.*, RG.RegionName, RS.Status,AgencyName=NULL, P.PayorName, CaseManager=dbo.GetGeneralNameFormat(CM.FirstName,CM.LastName),                                                            
 RRU.UsedRespiteHours,CM.Email,CM.MobileNumber AS Phone, ReferralCareGiver_AgencyID=@ReferralCareGiver_AgencyID,  --CM.Fax,CM.Phone,                                                            
  (SELECT                                                                           
     STUFF((SELECT ',' + CAST(RC.EmployeeID AS VARCHAR)                                                                          
     FROM ReferralCaseloads RC WHERE RC.ReferralID = CAST(@ReferralID AS BIGINT) and RC.IsDeleted=0                                                                           
     ORDER BY RC.EmployeeID                                                                          
     FOR XML PATH('')),1,1,'')) AS SetSelectedReferralCaseloadIDs                                                                
                                                                
 FROM Referrals R                                                                          
 LEFT JOIN Regions RG on RG.RegionID=R.RegionID                              
 LEFT JOIN ReferralStatuses RS on RS.ReferralStatusID=R.ReferralStatusID                                                                          
 LEFT JOIN Employees CM on CM.EmployeeId=R.CaseManagerID                                                                           
 --LEFT JOIN Agencies AG on AG.AgencyID=CM.AgencyID                                                                          
 LEFT JOIN ReferralPayorMappings RPM on RPM.ReferralID=R.ReferralID  AND (RPM.IsActive IS NULL OR RPM.IsActive=1)                                                                      
 LEFT JOIN Payors P on P.PayorID=RPM.PayorID                                                                          
 LEFT JOIN ReferralRespiteUsageLimit RRU                                                     
 ON  RRU.ReferralID=R.ReferralID  AND RRU.ReferralRespiteUsageLimitID = (                                      
      SELECT  TOP 1 ReferralRespiteUsageLimitID                                                                 
      FROM ReferralRespiteUsageLimit                                                                
      WHERE ReferralID=R.ReferralID  AND (GETUTCDATE()>=StartDate AND GETUTCDATE()<=EndDate)  ORDER BY ReferralRespiteUsageLimitID DESC                                                                 
      )                                    
 WHERE R.ReferralID = @ReferralID                            exec sp_executesql N'UPDATE [Referrals] SET [FirstName] = @0, [MiddleName] = @1, [LastName] = @2, [ClientNickName] = @3, [Dob] = @4, [Gender] = @5, [ServiceType] = @6, [ClientNumber] = @7, [AHCCCSID] = @8, [AHCCCSEnrollDate] = 
@9, [CISNumber] = @10, [Population] = @11, [HealthPlan] = @12, [Title] = @13, [PolicyNumber] = @14, [CASIIScore] = @15, [RecordRequestEmail] = @16, [MonthlySummaryEmail] = @17, [RateCode] = @18, [RateCodeStartDate] = @19, 
[RateCodeEndDate] = @20, [RegionID] = @21, [LanguageID] = @22, [ReferralStatusID] = @23, [Assignee] = @24, [BeneficiaryType] = @25, [DefaultFacilityID] = @26, [Caseload] = @27, [OrientationDate] = @28, [DropOffLocation] = @29, [PickUpLocation] = @30, [FrequencyCodeID] = @31, [NeedPrivateRoom] = @32, [PlacementRequirement] = @33, [BehavioralIssue] = @34, [OtherInformation] = @35, [ClientID] = @36, [AgencyID] = @37, [AgencyLocationID] = @38, [CaseManagerID] = @39, [PhysicianID] = @40, [CareConsent] = @41, [SelfAdministrationofMedication] = @42, [HealthInformationDisclosure] = @43, [AdmissionRequirements] = @44, [AdmissionOrientation] = @45, [ZarephathCrisisPlan] = @46, [PermissionForVoiceMail] = @47, [PermissionForEmail] = @48, [PermissionForSMS] = @49, [PermissionForMail] = @50, [PHI] = @51, [PHIAgencyID] = @52, [PHIExpirationDate] = @53, [RespiteService] = @54, [ZSPRespite] = @55, [ZSPRespiteExpirationDate] = @56, [ZSPRespiteGuardianSignature] = @57, [ZSPRespiteBHPSigned] = @58, [LifeSkillsService] = @59, [ZSPLifeSkills] = @60, [ZSPLifeSkillsExpirationDate] = @61, [ZSPLifeSkillsGuardianSignature] = @62, [ZSPLifeSkillsBHPSigned] = @63, [CounselingService] = @64, [ZSPCounselling] = @65, [ZSPCounsellingExpirationDate] = @66, [ZSPCounsellingGuardianSignature] = @67, [ZSPCounsellingBHPSigned] = @68, [ConnectingFamiliesService] = @69, [ZSPConnectingFamilies] = @70, [ZSPConnectingFamiliesExpirationDate] = @71, [ZSPConnectingFamiliesGuardianSignature] = @72, [ZSPConnectingFamiliesBHPSigned] = @73, [ACAssessment] = @74, [ACAssessmentExpirationDate] = @75, [AROI] = @76, [AROIAgencyID] = @77, [AROIExpirationDate] = @78, [NetworkCrisisPlan] = @79, [NetworkServicePlan] = @80, [NSPExpirationDate] = @81, [NSPGuardianSignature] = @82, [NSPBHPSigned] = @83, [NSPSPidentifyService] = @84, [NCPExpirationDate] = @85, [BXAssessment] = @86, [BXAssessmentExpirationDate] = @87, [BXAssessmentBHPSigned] = @88, [Demographic] = @89, [DemographicExpirationDate] = @90, [SNCD] = @91, [SNCDExpirationDate] = @92, [ReferralDate] = @93, [ReferralSourceID] = @94, [FirstDOS] = @95, [ClosureDate] = @96, [ClosureReason] = @97, [ReferralTrackingComment] = @98, [ReferralLSTMCaseloadsComment] = @99, [NotifyCaseManager] = @100, 
[NotifyCaseManagerDate] = @101, [IsSaveAsDraft] = @102, [MondaySchedule] = @103, [TuesdaySchedule] = @104, [WednesdaySchedule] = @105, [ThursdaySchedule] = @106, [FridaySchedule] = @107, [SaturdaySchedule] = @108, 
[SundaySchedule] = @109, [CareTypeIds] = @110, [UserName] = @111, [Password] = @112, [PasswordSalt] = @113, [SignatureNeeded] = @114, [ScheduleRequestDates] = @115, [PCMVoiceMail] = @116, [PCMEmail] = @117, [PCMSMS] = @118, 
[PCMMail] = @119, [RoleID] = @120, [DischargeComment] = @121, [DischargeDate] = @122, [CreatedDate] = @123, [CreatedBy] = @124, [UpdatedDate] = @125, [UpdatedBy] = @126, [SystemID] = @127 WHERE [ReferralID] = @128',N'@0 
nvarchar(4000),@1 nvarchar(4000),@2 nvarchar(4000),@3 nvarchar(4000),@4 datetime,@5 nvarchar(4000),@6 nvarchar(4000),@7 nvarchar(4000),@8 nvarchar(4000),@9 nvarchar(4000),@10 nvarchar(4000),@11 nvarchar(4000),@12 
nvarchar(4000),@13 nvarchar(4000),@14 nvarchar(4000),@15 nvarchar(4000),@16 nvarchar(4000),@17 nvarchar(4000),@18 nvarchar(4000),@19 nvarchar(4000),@20 nvarchar(4000),@21 nvarchar(4000),@22 nvarchar(4000),@23 bigint,@24 
bigint,@25 nvarchar(4000),@26 bigint,@27 nvarchar(4000),@28 nvarchar(4000),@29 nvarchar(4000),@30 nvarchar(4000),@31 nvarchar(4000),@32 nvarchar(4000),@33 nvarchar(4000),@34 nvarchar(4000),@35 nvarchar(4000),@36 bigint,@37 
nvarchar(4000),@38 nvarchar(4000),@39 bigint,@40 nvarchar(4000),@41 int,@42 int,@43 int,@44 int,@45 int,@46 nvarchar(4000),@47 int,@48 int,@49 int,@50 int,@51 int,@52 nvarchar(4000),@53 nvarchar(4000),@54 int,@55 int,@56 
nvarchar(4000),@57 nvarchar(4000),@58 nvarchar(4000),@59 int,@60 int,@61 nvarchar(4000),@62 nvarchar(4000),@63 nvarchar(4000),@64 int,@65 int,@66 nvarchar(4000),@67 nvarchar(4000),@68 nvarchar(4000),@69 int,@70 int,@71 
nvarchar(4000),@72 nvarchar(4000),@73 nvarchar(4000),@74 int,@75 nvarchar(4000),@76 nvarchar(4000),@77 nvarchar(4000),@78 nvarchar(4000),@79 nvarchar(4000),@80 int,@81 nvarchar(4000),@82 nvarchar(4000),@83 nvarchar(4000),@84 
nvarchar(4000),@85 nvarchar(4000),@86 int,@87 nvarchar(4000),@88 nvarchar(4000),@89 nvarchar(4000),@90 nvarchar(4000),@91 nvarchar(4000),@92 nvarchar(4000),@93 datetime,@94 int,@95 nvarchar(4000),@96 nvarchar(4000),@97 nvarchar(4000),@98 nvarchar(4000),@99 nvarchar(4000),@100 int,@101 nvarchar(4000),@102 int,@103 int,@104 int,@105 int,@106 int,@107 int,@108 int,@109 int,@110 nvarchar(4000),@111 nvarchar(4000),@112 nvarchar(4000),@113 nvarchar(4000),@114 int,@115 nvarchar(4000),@116 int,@117 int,@118 int,@119 int,@120 bigint,@121 nvarchar(4000),@122 nvarchar(4000),@123 datetime,@124 bigint,@125 datetime,@126 bigint,@127 nvarchar(4000),@128 bigint',@0=N'Alice',@1=NULL,@2=N'Ashford',@3=NULL,@4='1972-06-05 00:00:00',@5=N'F',@6=N'41',@7=NULL,@8=N'2342342342',@9=NULL,@10=NULL,@11=N'Child',@12=NULL,@13=N'XIX',@14=NULL,@15=NULL,@16=NULL,@17=NULL,@18=NULL,@19=NULL,@20=NULL,@21=NULL,@22=N'36',@23=1,@24=1,@25=N'1',@26=1,@27=NULL,@28=NULL,@29=NULL,@30=NULL,@31=NULL,@32=NULL,@33=NULL,@34=NULL,@35=NULL,@36=5,@37=NULL,@38=NULL,@39=2,@40=NULL,@41=0,@42=0,@43=0,@44=0,@45=0,@46=N'N',@47=0,@48=0,@49=0,@50=1,@51=0,@52=NULL,@53=NULL,@54=0,@55=0,@56=NULL,@57=NULL,@58=NULL,@59=0,@60=0,@61=NULL,@62=NULL,@63=NULL,@64=0,@65=0,@66=NULL,@67=NULL,@68=NULL,@69=0,@70=0,@71=NULL,@72=NULL,@73=NULL,@74=0,@75=NULL,@76=N'N',@77=NULL,@78=NULL,@79=N'N',@80=0,@81=NULL,@82=NULL,@83=NULL,@84=N'NA',@85=NULL,@86=0,@87=NULL,@88=NULL,@89=N'N',@90=NULL,@91=N'N',@92=NULL,@93='2019-07-22 00:00:00',@94=5,@95=NULL,@96=NULL,@97=NULL,@98=NULL,@99=NULL,@100=0,@101=NULL,@102=0,@103=1,@104=1,@105=1,@106=1,@107=1,@108=1,@109=1,@110=N'47,42,41',@111=NULL,@112=NULL,@113=NULL,@114=0,@115=NULL,@116=0,@117=0,@118=0,@119=0,@120=0,@121=NULL,@122=NULL,@123='2019-07-22 20:05:03',@124=1,@125='2019-08-14 07:04:00.157',@126=1,@127=N'75.83.81.14',@128=5           
 -- THIS QUERY WILL FETCH REFERRAL STATUS                                
 --SELECT d.DDMasterID AS Value,d.Title AS Name                                
 --FROM dbo.DDMaster d                                
 --WHERE d.ItemType=@DDType_PatientSystemStatus AND IsDeleted=0                              
 SELECT RS.ReferralStatusID,RS.[Status]                                                                          
 FROM ReferralStatuses RS   WHERE UsedInHomeCare=1                                                                        
                                                           
                                                                          
 -- THIS QUERY WILL FETCH THE EMPLOYEES                                           
 SELECT E.EmployeeID, dbo.GetGeneralNameFormat(E.FirstName,E.LastName) AS EmployeeName ,IsDeleted                                                                          
 FROM Employees E                                         
 WHERE E.IsActive = 1 order by E.LastName ASC --AND  (E.IsDeleted = 0 OR (E.EmployeeID = CAST(@EmployeeID AS BIGINT)))                                                              
                                                                          
 -- THIS QUERY WILL FETCH THE FREQUENCY CODES                                                                          
 SELECT FC.FrequencyCodeID,FC.Code                                                                          
 FROM FrequencyCodes FC  WHERE UsedInHomeCare=1 order by FC.Code ASC                                                                    
                                                        
 -- THIS QUERY WILL FETCH THE FREQUENCY CODES                                                                          
 SELECT TL.TransportLocationID, TL.Location                                                              
 FROM TransportLocations TL where (TL.IsDeleted=0 OR (TL.TransportLocationID = CAST(@PickUpLocation AS BIGINT)) OR (TL.TransportLocationID = CAST(@DropOffLocation AS BIGINT)))  order by Location ASC                                                        
   
    
      
        
          
           
                                                          
 -- RETURN 0 FOR GENDER MODEL                                                                          
 --SELECT 0;                                          
 --SELECT Value=Value,Name=Title FROM DDMaster where IsDeleted=0 and ItemType=@DDType_Gender          
 SELECT DISTINCT Value=Value,Name=DD.Title FROM DDMaster DD          
 LEFT JOIN Referrals R ON R.Gender=DD.Value          
 WHERE ItemType=@DDType_Gender AND ((R.ReferralID IS NOT NULL AND R.ReferralID=@ReferralID) OR DD.IsDeleted=0)          
                                                       
                                                                     -- THIS QUERY WILL FETCH THE REGION CODES                                                                          
 SELECT R.RegionID, R.RegionName                            
 FROM REGIONS R  order by RegionName ASC                                                                    
                                                                     -- THIS QUERY WILL FETCH THE Languages                                                                          
 --SELECT L.LanguageID, L.Name                              
 --FROM Languages L order by L.Name ASC                       
 SELECT LanguageID=DDMasterID ,Name=Title FROM DDMaster WHERE ItemType =@DDType_LanguagePreference AND IsDeleted=0                      
                                                                    
                                                                           
 -- RETURN 0 FOR NEED PRIVATE ROOM YES OR NO LIST                                                      
 SELECT 0;                                                                         
                                                                            
    -- RETURN 0 FOR Contact Model                                                                          
 SELECT 0;                                                                          
                   -- RETURN 0 FOR Contact Mapping Model                                             
 SELECT 0;                                                                          
                                                                          
 -- THIS QUERY WILL FETCH THE CONTACT TYPES                                                                         
 SELECT *                                                                          
 FROM ContactTypes WHERE IsDeleted=0 Order BY OrderNumber --order by ContactTypeName ASC                                                                    
                                                                          
 -- RETURN 0 FOR ROI Types Model                                                                          
 SELECT 0;                                                                               
 -- RETURN 0 FOR Primary Contact Legal Guardian Model                                                                         
 SELECT 0;                                       
                                                                          
 -- RETURN 0 FOR DCS Legal Guardian Model                                                                          
 SELECT 0;                                                                       
                                   
 -- RETURN 0 FOR Emergency Contact Model                                                                          
 SELECT 0;                                                                          
                                                                        
 -- RETURN 0 FOR NoticeProviderOnFileList                                                                          
 SELECT 0;                                                                          
                                                  
 -- THIS QUERY WILL FETCH THE CONTACT LIST                                                                          
 SELECT CT.ContactTypeID ,CT.ContactTypeName,CM.ROIType,CM.ROIExpireDate,C.FirstName, C.LastName, C.[Address], C.LanguageID,C.Email,                                                                          
 C.PHONE1,C.Phone2, C.City,C.State,C.ZipCode,CM.IsDCSLegalGuardian,CM.IsEmergencyContact,CM.IsNoticeProviderOnFile,CM.IsPrimaryPlacementLegalGuardian,                                                                
 C.ContactID,CM.ReferralID,CM.ClientID, CM.ContactMappingID,E.FirstName AS EmpFirstName, E.LastName AS EmpLastName, C.Latitude, C.Longitude                                                                          
 FROM Contacts C                                                        
 INNER JOIN ContactMappings CM ON CM.ContactID = C.ContactID                                                                         
 INNER JOIN ContactTypes CT ON CT.ContactTypeID = CM.ContactTypeID AND CT.IsDeleted=0                                                            
 INNER JOIN Employees E ON E.EmployeeID = CM.CreatedBy                                                                          
 WHERE CM.REFERRALID= @ReferralID                                                                     
                                                   
-- RETURN 0 FOR Contact Model used in the POPUP for Add Contact                                                                          
 SELECT 0;                                                                          
                                              
 -- RETURN 0 FOR Contact Model Temporaory model used in the POPUP for Add Contact                                                                          
 SELECT 0;                                                                          
                                                                          
 -- RETURN 0 FOR Contact Search Model used in the POPUP for Add Contact                                                                          
 SELECT 0;                                                                          
                                                                          
 SELECT A.AgencyID,A.NickName,A.AgencyType                                                                          
 FROM Agencies A where A.IsDeleted=0 OR A.AgencyID=@AgencyID     order by A.NickName ASC                                                                     
                                                                           
 -- RETURN AGENCY LOCATION List Model                                                                          
 SELECT AL.AgencyLocationID,AL.LocationName,AL.AgencyID                                                                 
 FROM AgencyLocations AL where AL.IsDeleted=0 OR AL.AgencyLocationID=@AgencyLocationID;                                                                          
                                                                          
 -- RETURN CASE MANAGER List Model                      
 SELECT 0;                                                                          
 --SELECT CM.CaseManagerID, CM.FirstName, CM.LastName                                                                          
 --FROM CASEMANAGERS CM                        
 --WHERE CM.IsDeleted=0;                                                                          
                                                                          
 -- RETURN 0 FOR CASE MANAGER Model                                                                          
 SELECT 0;                                                                          
                                                                          
 -- RETURN 0 FOR Referral Internal Message                                                                          
 SELECT 0;                                                                          
                                      
 -- RETURN 0 FOR CareConsentList                                                                          
 SELECT 0;                                                                          
                                                                          
 -- RETURN 0 FOR SelfAdministrationList                                                                          
 SELECT 0;          
                             
 -- RETURN 0 FOR HealthInformationList                                                                          
 SELECT 0;                                                                          
                                                          
  -- RETURN 0 FOR AdmissionRequirementList                                                       
 SELECT 0;                                                                          
                                                                
 -- RETURN 0 FOR AdmissionOrientationList                                           
 SELECT 0;                                                       
                                                                          
 -- RETURN 0 FOR ZarephathCrisisPlanList                                                                        
 SELECT 0;                                                                          
                                                                          
 -- RETURN 0 FOR NetworkCrisisPlanList                                                                          
 SELECT 0;                                      
                                                                          
 -- RETURN 0 FOR VoiceMailList                                                                          
 SELECT 0;                                                                          
                                                   
 -- RETURN 0 FOR PermissionEmailList                                                                          
 SELECT 0;                                                                          
                                                                          
 -- RETURN 0 FOR PermissionSMSList                                                                          
 SELECT 0;                                                                      
                                                                           
 -- RETURN 0 FOR PHI YES/NO List                                                                 
 SELECT 0;                                                                          
                                                                          
 -- RETURN 0 FOR ROI YES/NO List                                                
 SELECT 0;                                                                          
                                                                          
-- RETURN 0 FOR ZSP Respite Service Plan YES/NO List                                                                          
 SELECT 0;                                                                          
                                                                          
 -- RETURN 0 FOR ZSP Life Skills Service Plan YES/NO List                                                              
 SELECT 0;                                        
                                                                          
 -- RETURN 0 FOR ZSP Counselling Service Plan YES/NO List                                                                          
 SELECT 0;                                                                 
                                                                           
 -- RETURN 0 FOR ZSPRespiteGuardianSignatureList YES/NO List                                                           
 SELECT 0;                                                                          
                        
 -- RETURN 0 FOR ZSPRespiteBHPSignedList YES/NO List                                                                           
 SELECT 0;                                                                          
                                                                           
 -- RETURN 0 FOR ZSPLifeSkillsGuardianSignatureList YES/NO List                                                       
 SELECT 0;                                                   
                                                                      
 -- RETURN 0 FOR ZSPLifeSkillsBHPSignedList YES/NO List                                                                           
 SELECT 0;                                                                          
                                                                          
  -- RETURN 0 FOR ZSPCounsellingGuardianSignatureList YES/NO List                                                                           
 SELECT 0;                                               
                                                                          
 -- RETURN 0 FOR ZSPCounsellingBHPSignedList YES/NO List                                                                           
 SELECT 0;                                                                          
                                                                         
 -- RETURN 0 FOR NetworkServicePlanList YES/NO List                                                                           
 SELECT 0;                                    
                                                                          
 -- RETURN 0 FOR NetworkServiceGuardianSignatureList YES/NO List                                                                       
 SELECT 0;                                                                          
                                                                   
 -- RETURN 0 FOR NetworkServiceBHPSignedList YES/NO List                                                   
 SELECT 0;                                                                          
                                                                          
 -- RETURN 0 FOR NSPSPidentifyServiceList YES/NO List                                                                
 SELECT 0;                                                                          
                                                                          
  -- RETURN 0 FOR BXAssessmentList YES/NO List                                                          
 SELECT 0;                                                                          
                                                                        
  -- RETURN 0 FOR BXAssessmentBHPSignedList YES/NO List                                                                           
 SELECT 0;                                                                          
                                              
  -- RETURN 0 FOR Demographic LIST YES/NO List                                                                           
 SELECT 0;                                                                          
                                                                          
  -- RETURN 0 FOR SNCDList LIST YES/NO List                                                         
 SELECT 0;                                                   
                                                                           
  -- RETURN 0 FOR ACAssessmentList LIST YES/NO List                                                                           
 SELECT 0;                                                                          
                                                                           
 -- THIS QUERY WILL FETCH THE DX CODE LIST                                                                          
 --SELECT D.DXCodeID,D.DXCodeName                                                                          
--FROM DXCodes D                                                          
                                              
  -- RETURN 0 FOR DX CODE MODEL                                                                          
 --SELECT 0;                                                         
                                                                          
 SELECT RD.ReferralDXCodeMappingID,D.DXCodeID,D.DXCodeName,RD.Precedence,RD.StartDate,Rd.EndDate,D.Description,D.EffectiveFrom,D.EffectiveTo,RD.IsDeleted,DT.DxCodeShortName,                                                                    
 Rd.CreatedDate,Rd.CreatedBy,D.DXCodeWithoutDot                                                                    
 FROM ReferralDXCodeMappings RD                                                                          
 INNER JOIN DxCodes D ON D.DXCodeID=RD.DXCodeID                                                                          
 INNER Join DxCodeTypes DT on DT.DxCodeTypeID=D.DxCodeType              
 WHERE RD.ReferralID = @ReferralID                                                                          
 ORDER BY case when RD.Precedence is null then 1 else 0 end,  RD.Precedence                                          
                                              
 SELECT * from LU_OutcomeMeasurementOptions ORDER BY OutcomeMeasurementOptionID DESC                                                                
                                                                
 SELECT 0;                                                       
                                                                 
 SELECT  R.ReferralID,R.LastName+', '+ R.FirstName as Name,R.AHCCCSID,R.CISNumber,C.Phone1,C.Address,C.Email,dbo.GetGeneralNameFormat(C.FirstName,C.LastName) as ParentName                                                                   
,r.Gender,dbo.GetAge(R.Dob) as Age,rs.Status,RSM.ReferralID1,RSM.ReferralID2,RSM.ReferralSiblingMappingID,RSM.CreatedDate,RSM.CreatedBy                                  
                                
 from Referrals R                                                                        
inner join ReferralSiblingMappings RSM on (RSM.ReferralID1=R.ReferralID AND RSM.ReferralID2=@ReferralID) OR (RSM.ReferralID2=R.ReferralID AND RSM.ReferralID1=@ReferralID)                                                                
left join ContactMappings CM on CM.ReferralID=R.ReferralID and (CM.IsDCSLegalGuardian=1 OR CM.IsPrimaryPlacementLegalGuardian=1)                                                                        
left join ReferralStatuses RS on rs.ReferralStatusID=r.ReferralStatusID                                                                     
left join Contacts C on CM.ContactID=C.ContactID                                                                
                                                                 
                                                                          
 --This query fetch the DocumentTypes from ComplianceMaster                                
 SELECT DocumentTypeID=ComplianceID,DocumentKind=DocumentationType,DocumentTypeName=DocumentName FROM Compliances                                 
 WHERE IsDeleted=0 AND UserType=2                                
                                                                
                                                                
                                                                
                                                                  
                                                                          
 -- RETURN 0 fro kind of document                                                                          
 select 0;                                                          
                                           
 -- THIS QUERY WILL FETCH THE PAYOR FROM THE TABLE PAYOR                                             
 SELECT P.PayorID,P.PayorName                                                                          
 FROM Payors P                                                                        
 WHERE P.IsDeleted = 0 OR P.PayorID IN (select PayorID from ReferralPayorMappings where ReferralID=@ReferralID and IsDeleted=0) order by P.PayorName ASC                                                                         
           
 -- RETURN 0 FOR ReferralPayorMapping Model                                                  
 SELECT RPM.*, RPM.PayorID AS TempPayorID                                      
 FROM ReferralPayorMappings RPM                                                                          
 WHERE RPM.IsDeleted = 0 AND RPM.IsActive =1  AND RPM.ReferralID =@ReferralID                                                                          
                                                                          
 -- RETURN 0 FOR ReferralDocument Model                                                   
 select 0;                 
                                                                          
 -- THIS QUERY WILL FETCH ALL THE PAYOR FOR THE REFERRAL                                                                          
 SELECT RPM.ReferralPayorMappingID,RPM.PayorEffectiveDate,RPM.PayorEffectiveEndDate, RPM.IsActive ,P.PayorName,dbo.GetGeneralNameFormat(E.FirstName,E.LastName)   AS AddedByName,dbo.GetGeneralNameFormat(E1.FirstName,E1.LastName) AS UpdatedByName,         
  
    
      
        
          
            
              
                
                  
                    
                      
             
                          
                          
                          
                            
                              
                              
                                
                                
                                  
                                    
                                       
                                        
                                          
                                            
                                              
                                               
                                                   
                                                    
                                               
                                                        
                                           
                                                                
 RPM.CreatedDate,RPM.UpdatedDate                                                                          
 FROM ReferralPayorMappings RPM                                                                           
 INNER JOIN Payors P ON P.PayorID = RPM.PayorID                                                                          
 INNER JOIN Employees E ON E.EmployeeID = RPM.CreatedBy                                                         
 INNER JOIN Employees E1 ON E1.EmployeeID = RPM.UpdatedBy                                                                          
 AND RPM.IsDeleted = 0 AND RPM.ReferralID = @ReferralID                                                                           
 ORDER BY RPM.UpdatedDate DESC                                                                          
                                                                          
 -- This query will fetch count of Schedule history                                                                          
 select Count(*) from ScheduleMasters where ReferralID=@ReferralID AND IsDeleted=0                                                                     
                                                                          
 select * from ReferralSources order by ReferralSourceName ASC                                                                          
                                                                          
 --select top 1 ISNULL(ServiceDate,NULL) from Notes where ReferralID=@ReferralID and IsBillable=1 order by NoteID ASC                                                                          
 --if((select count(*) from Notes where ReferralID=@ReferralID and IsBillable=1) > 0)                                                                          
 select top 1 ServiceDate from Notes where ReferralID=@ReferralID and IsBillable=1 AND IsDeleted=0 order by StartTime ASC                                                                          
 --else                                                  
 --select null ServiceDate;                                                                          
                                                                          
   select 0;                                                             
                                                                    
   select 0;                                                                    
   select 0;                                                              
                                                              
    SELECT rp.*,p.PreferenceName FROM ReferralPreferences rp                                                                 
 INNER JOIN Preferences p ON rp.PreferenceID=p.PreferenceID                                                                  
 WHERE ReferralID=@ReferralID AND P.KeyType=@PreferenceType_Preference                                                                  
                                                          
                              
 -- SKILLS MASTER                                                              
  --SELECT * FROM Preferences P WHERE P.KeyType=@PreferenceType_Skill                                                              
                                                              
  -- SKILLS MASTER                             
  IF(@ReferralID=0)                                                              
  SELECT * FROM Preferences P WHERE P.KeyType=@PreferenceType_Skill AND IsDeleted=0                                                              
  ELSE                                                          
  SELECT * FROM Preferences P WHERE P.KeyType=@PreferenceType_Skill --AND IsDeleted=0                                                              
                                                              
                                                              
                                                              
  SELECT EP.PreferenceID FROM ReferralPreferences EP                           
  INNER JOIN Preferences P ON EP.PreferenceID=P.PreferenceID                                                                
  WHERE ReferralID=@ReferralID AND P.KeyType=@PreferenceType_Skill                                                              
                                                                
 SELECT * FROM States                                                              
                                                        
 SELECT Name=Title,Value=DDMasterID FROM DDMaster where IsDeleted=0 and ItemType=@DDType_CareType                                                        
                                                              
 SELECT 0;                                                  
                            
 SELECT Value=DDMasterID,Name=Title FROM DDMaster where IsDeleted=0 and ItemType=@DDType_AdmissionType                                                  
 SELECT Value=DDMasterID,Name=Title FROM DDMaster where IsDeleted=0 and ItemType=@DDType_AdmissionSource                                                                     
 SELECT Value=DDMasterID,Name=Title FROM DDMaster where IsDeleted=0 and ItemType=@DDType_PatientStatus                                                        
 --SELECT Value=DDMasterID,Name=Title FROM DDMaster where IsDeleted=0 and ItemType=@DDType_FacilityCode              
               
 SELECT DISTINCT Value=DDMasterID,Name=Title FROM DDMaster DD              
 LEFT JOIN ReferralBillingSettings RBS ON RBS.POS_CMS1500=DD.DDMasterID              
 LEFT JOIN ReferralBillingSettings RBS_U ON RBS_U.POS_UB04=DD.DDMasterID              
 WHERE ItemType=@DDType_FacilityCode AND (((RBS.ReferralBillingSettingID IS NOT NULL AND RBS.ReferralID=@ReferralID) OR DD.IsDeleted=0) OR              
 ((RBS_U.ReferralBillingSettingID IS NOT NULL AND RBS_U.ReferralID=@ReferralID) OR DD.IsDeleted=0))              
                                                         
 SELECT Value=DDMasterID,Name=Title FROM DDMaster where IsDeleted=0 and ItemType=@DDType_VisitType                          
                                  
 --Select Compliance(Internal Documentation) Details                                  
 SELECT C.ComplianceID,C.DocumentName,C.IsTimeBased,RCM.ReferralComplianceID,RCM.Value,RCM.ExpirationDate,ReferralID=@ReferralID,                  
 SectionName=C1.DocumentName--,SubSectionName=DSS.Title                  
 FROM Compliances C                                 
 LEFT JOIN ReferralComplianceMappings RCM ON RCM.ComplianceID=C.ComplianceID AND RCM.ReferralID=@ReferralID                    
 LEFT JOIN Compliances C1 ON C1.ParentID=C.ComplianceID        
 WHERE C.UserType=2 AND C.DocumentationType=1 AND C.IsDeleted=0                                        
                                  
 --Select Compliance(External Documentation) Master                                  
 SELECT C.ComplianceID,C.DocumentName,C.IsTimeBased,RCM.ReferralComplianceID,RCM.Value,RCM.ExpirationDate,ReferralID=@ReferralID,                  
 SectionName=C1.DocumentName--,SubSectionName=DSS.Title                  
 FROM Compliances C                    
 LEFT JOIN ReferralComplianceMappings RCM ON RCM.ComplianceID=C.ComplianceID AND RCM.ReferralID=@ReferralID                    
 LEFT JOIN Compliances C1 ON C1.ParentID=C.ComplianceID                  
 WHERE C.UserType=2 AND C.DocumentationType=2 AND C.IsDeleted=0                                  
                    
 SELECT d.DDMasterID AS Value,d.Title AS Name                                
 FROM dbo.DDMaster d                                
 WHERE d.ItemType=@DDType_PatientFrequencyCode AND IsDeleted=0                                              
               
 SELECT d.DDMasterID AS Value,d.Title AS Name               
 FROM dbo.DDMaster d                                
 WHERE d.ItemType=@DDType_BeneficiaryType AND IsDeleted=0                     
            
 SELECT        
  RBT.ReferralBeneficiaryTypeID,        
  RBT.ReferralID,        
  RBT.BeneficiaryTypeID,        
  DM.[Value] AS BeneficiaryTypeName,        
  RBT.BeneficiaryNumber        
 FROM        
  ReferralBeneficiaryTypes RBT        
  INNER JOIN DDMaster DM ON RBT.BeneficiaryTypeID = DM.DDMasterID        
 WHERE        
  RBT.ReferralID = @ReferralID       
        
  SELECT        
  RP.ReferralPhysicianID,        
  RP.ReferralID,        
  RP.PhysicianID,        
  P.FirstName,        
  P.MiddleName,        
  P.LastName,        
  P.Address,        
  P.Email,        
  P.Phone      
 FROM        
  ReferralPhysicians RP      
  INNER JOIN Physicians P ON RP.PhysicianID = P.PhysicianID        
 WHERE        
  RP.ReferralID = @ReferralID    
  
  select DDMasterID As Value,Title As Name  from DDMaster where ItemType=23     
END
