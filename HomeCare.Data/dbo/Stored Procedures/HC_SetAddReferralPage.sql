--UpdateBy:Akhilesh                                        
--UpdatedDate:28/march/2020                                        
--Description: for get physicianDetail                                        
  CREATE PROCEDURE [dbo].[HC_SetAddReferralPage]                                  
  @ReferralID bigint = 0,                                  
  @EmployeeID bigint = 0,                                  
  @DropOffLocation bigint = 0,                                  
  @PickUpLocation bigint = 0,                                  
  @AgencyID bigint = 0,                                  
  @AgencyLocationID bigint = 0,                                  
  @PreferenceType_Preference varchar(100),                                  
  @PreferenceType_Skill varchar(100),                                  
  @DDType_CareType int = 1,                                  
  @DDType_AdmissionType int = 6,                                  
  @DDType_AdmissionSource int = 7,                                  
  @DDType_PatientStatus int = 8,                                  
  @DDType_FacilityCode int = 10,                                  
  @DDType_VisitType int = 9,                                  
  --@CareType INT                                                                                
  @DDType_PatientSystemStatus int = 12,                                  
  @DDType_PatientFrequencyCode int = 11,                                  
  @DDType_Gender int = 16,                                  
  @DDType_LanguagePreference int = 17                                  
  , @DDType_BeneficiaryType int = 22,                                  
  @DDType_PriorAuthorizationFrequency int = 24,                                  
  @DDType_RevenueCode int = 0                                  
AS    
BEGIN                                  
 declare @NameFormat VARCHAR(500)=dbo.GetOrgNameFormat();                                   
  SELECT                                  
    @EmployeeID = Assignee,                                  
    @DropOffLocation = DropOffLocation,                                  
    @PickUpLocation = PickUpLocation,                                  
    @AgencyID = AgencyID,                                  
    @AgencyLocationID = AgencyLocationID                                  
  FROM Referrals                                  
  WHERE ReferralID = @ReferralID                                  
                                  
                                  
  DECLARE @ReferralCareGiver_AgencyID bigint = 0;                                  
      select  @ReferralCareGiver_AgencyID= AgencyID from ReferralCaregivers where ReferralID=@ReferralID                            
  SELECT                                  
    FacilityID,                                  
    FacilityName                                  
  FROM Facilities                                  
  WHERE IsDeleted = 0                                
--  declare                              
--@weight float,                              
--@height float,                              
--@heights float,                              
--@BMI float                              
--select @heights= (CAST(Height as float)/100) from Referrals where ReferralID=60055                              
--select @Weight= CAST(Weight AS float) from Referrals where ReferralID=60055                              
--select @BMI=@weight/(CAST(@heights AS float)*CAST(@heights AS float))                              
                                    
--  SELECT                                  
--    case when @BMI<CAST(18.5 AS float)  then 'Underweight '                               
--when  @BMI>CAST(25 AS float) AND @BMI<CAST(29.9 AS float)  then 'Overweight '                              
--when  @BMI>CAST(18.5 AS float) AND @BMI<CAST(24.8 AS float)  then 'Normal weight '                              
--ELSE 'Obesity ' end as BMI,R.*,                            
SELECT                            
 r.BMI,R.*,             
 Name = dbo.GetGenericNameFormat(R.FirstName,R.MiddleName,R.LastName,@NameFormat),          
    RG.RegionName,                 
    RS.Status,                                  
    AgencyName = NULL,                                  
    P.PayorName,                                  
    CaseManager = dbo.GetGenericNameFormat(CM.FirstName,'',CM.LastName,@NameFormat),                                  
    RRU.UsedRespiteHours,                                  
    CM.Email,                                  
    CM.Phone AS Phone,                                  
    ReferralCareGiver_AgencyID = @ReferralCareGiver_AgencyID,  --CM.Fax,CM.Phone,                 
    (SELECT                                  
      STUFF((SELECT                                  
        ',' + CAST(RC.EmployeeID AS varchar)                 
      FROM ReferralCaseloads RC                                  
      WHERE RC.ReferralID = CAST(@ReferralID AS bigint)                                  
      AND RC.IsDeleted = 0                                  
      ORDER BY RC.EmployeeID                   
      FOR xml PATH ('')), 1, 1, ''))                                  
    AS SetSelectedReferralCaseloadIDs                                  
                                  
  FROM Referrals R                            
  LEFT JOIN Regions RG                                  
    ON RG.RegionID = R.RegionID                                  
  LEFT JOIN ReferralStatuses RS                                  
    ON RS.ReferralStatusID = R.ReferralStatusID                                  
  LEFT JOIN CaseManagers CM                                  
    ON CM.CaseManagerID = R.CaseManagerID                                  
  --LEFT JOIN Agencies AG on AG.AgencyID=CM.AgencyID                                                                                                                          
  LEFT JOIN ReferralPayorMappings RPM                          
    ON RPM.ReferralID = R.ReferralID                                  
    AND (RPM.IsActive IS NULL                                  
    OR RPM.IsActive = 1)                                  
  LEFT JOIN Payors P                                  
    ON P.PayorID = RPM.PayorID                                  
  LEFT JOIN ReferralRespiteUsageLimit RRU                                  
    ON RRU.ReferralID = R.ReferralID                                  
    AND RRU.ReferralRespiteUsageLimitID = (SELECT TOP 1                                  
      ReferralRespiteUsageLimitID                                  
    FROM ReferralRespiteUsageLimit                                  
    WHERE ReferralID = R.ReferralID                                  
    AND (GETUTCDATE() >= StartDate                                  
    AND GETUTCDATE() <= EndDate)                                  
    ORDER BY ReferralRespiteUsageLimitID DESC)                                  
  WHERE R.ReferralID = @ReferralID                                  
  -- THIS QUERY WILL FETCH REFERRAL STATUS                                                      
  --SELECT d.DDMasterID AS Value,d.Title AS Name                                                                                
  --FROM dbo.DDMaster d                                                                                
  --WHERE d.ItemType=@DDType_PatientSystemStatus AND IsDeleted=0   
  
   SELECT  RS.ReferralStatusID, RS.[Status]  FROM ReferralStatuses RS   WHERE UsedInHomeCare = 1    
  UNION SELECT  ReferralStatusID='99999999',[Status]='Add/Edit/Item'     
  ORDER BY Status asc  
                                  
                                  
  -- THIS QUERY WILL FETCH THE EMPLOYEES                                                                                           
  SELECT                                  
    E.EmployeeID,                                  
    dbo.GetGenericNameFormat(E.FirstName,E.MiddleName,E.LastName,@NameFormat) AS EmployeeName,                                  
    IsDeleted                           
  FROM Employees E                                  
  WHERE E.IsActive = 1                                  
  ORDER BY E.LastName ASC --AND  (E.IsDeleted = 0 OR (E.EmployeeID = CAST(@EmployeeID AS BIGINT)))            
                                  
  -- THIS QUERY WILL FETCH THE FREQUENCY CODES                                                                                             
  SELECT                                  
    FC.FrequencyCodeID,                                  
    FC.Code                                  
  FROM FrequencyCodes FC                                  
  WHERE UsedInHomeCare = 1                                  
  ORDER BY FC.Code ASC                                  
                                  
  -- THIS QUERY WILL FETCH THE FREQUENCY CODES                                                                                                    
  SELECT                                  
    TL.TransportLocationID,                                  
    TL.Location                                  
  FROM TransportLocations TL                                  
  WHERE (TL.IsDeleted = 0                                  
  OR (TL.TransportLocationID = CAST(@PickUpLocation AS bigint))                                  
  OR (TL.TransportLocationID = CAST(@DropOffLocation AS bigint)))                         
  ORDER BY Location ASC                                  
                                  
                                  
  -- RETURN 0 FOR GENDER MODEL                                                                              
  --SELECT 0;                                                                                          
  --SELECT Value=Value,Name=Title FROM DDMaster where IsDeleted=0 and ItemType=@DDType_Gender                                                          
  SELECT DISTINCT                                  
    Value = Value,                                  
    Name = DD.Title                                  
  FROM DDMaster DD                                  
  LEFT JOIN Referrals R                                  
    ON R.Gender = DD.Value                                  
  WHERE ItemType = @DDType_Gender                                  
  AND ((R.ReferralID IS NOT NULL                                  
  AND R.ReferralID = @ReferralID)                                  
  OR DD.IsDeleted = 0)                                  
                                  
  -- THIS QUERY WILL FETCH THE REGION CODES                                                     
  --SELECT                                  
  --  R.RegionID,                                  
  --  R.RegionName                                  
  --FROM REGIONS R                                  
  --ORDER BY RegionName ASC                 
   select dm.DDMasterID AS RegionID,dm.Title AS RegionName from DDMaster dm                  
inner join lu_DDMasterTypes lu on lu.DDMasterTypeID=dm.ItemType                  
where lu.Name='Location of Service' AND DM.IsDeleted=0                
  -- THIS QUERY WILL FETCH THE Languages                             
  --SELECT L.LanguageID, L.Name                                                                              
  --FROM Languages L order by L.Name ASC                                                                       
  SELECT                                  
    LanguageID = DDMasterID,                                  
    Name = Title                                  
  FROM DDMaster                                  
  WHERE ItemType = @DDType_LanguagePreference                                  
  AND IsDeleted = 0                                  
                                  
                                  
  -- RETURN 0 FOR NEED PRIVATE ROOM YES OR NO LIST                                                                                                      
  SELECT                                  
    0;                                  
                    
  -- RETURN 0 FOR Contact Model                                                                           
  SELECT                                  
    C.FirstName,            
    C.LastName,            
 dbo.GetGenericNameFormat(C.FirstName,'',C.LastName,@NameFormat) AS FullName,          
    C.Address,                                  
    C.City,                                  
    C.State,                                  
    C.ZipCode,                   
    C.Phone1                                  
  FROM Contacts C                                  
  INNER JOIN ContactMappings CM                                  
    ON CM.ContactID = C.ContactID                                  
  WHERE CM.ReferralID = @ReferralID                                  
  -- RETURN 0 FOR Contact Mapping Model                                                         
  SELECT                                  
    0;                                  
                                  
  -- THIS QUERY WILL FETCH THE CONTACT TYPES                                                                                                                         
  SELECT                                  
    *                                  
  FROM ContactTypes                                  
  WHERE IsDeleted = 0                                  
  ORDER BY OrderNumber --order by ContactTypeName ASC                                                                                                                    
                                
  -- RETURN 0 FOR ROI Types Model                                                                                                                          
  SELECT                                  
    0;                                  
  -- RETURN 0 FOR Primary Contact Legal Guardian Model                                                                                                                         
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR DCS Legal Guardian Model                                         
  SELECT                             
    0;                                  
                                  
  -- RETURN 0 FOR Emergency Contact Model                                                                                                                          
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR NoticeProviderOnFileList                                                                                                                          
  SELECT                                  
    0;                                  
                                  
  -- THIS QUERY WILL FETCH THE CONTACT LIST                                                                   
  SELECT                                  
    CT.ContactTypeID,                                  
    CT.ContactTypeName,                                  
    CM.ROIType,                                  
    CM.ROIExpireDate,                                  
    C.FirstName,                                  
    C.LastName,         
 dbo.GetGenericNameFormat(C.FirstName,'',C.LastName,@NameFormat) AS FullName,          
    C.[Address],                                  
    C.LanguageID,                                  
    C.Email,                                  
    C.PHONE1,                                  
    C.Phone2,                                  
    C.City,                                  
    C.State,                                  
    C.ZipCode,                                  
    CM.IsDCSLegalGuardian,                                  
    CM.IsEmergencyContact,                   
    CM.IsNoticeProviderOnFile,                                  
    CM.IsPrimaryPlacementLegalGuardian,                                  
    C.ContactID,                              
    CM.ReferralID,                                  
    CM.ClientID,                                  
    CM.ContactMappingID,                                  
    E.FirstName AS EmpFirstName,                                  
    E.LastName AS EmpLastName,              
    C.Latitude,                          
    C.Longitude                                  
  FROM Contacts C                                  
  INNER JOIN ContactMappings CM                                  
    ON CM.ContactID = C.ContactID                                  
  INNER JOIN ContactTypes CT                                  
    ON CT.ContactTypeID = CM.ContactTypeID                                  
    AND CT.IsDeleted = 0                                 
  INNER JOIN Employees E                                  
    ON E.EmployeeID = CM.CreatedBy                                  
  WHERE CM.REFERRALID = @ReferralID                                  
                                  
  -- RETURN 0 FOR Contact Model used in the POPUP for Add Contact                                                                                                                          
  SELECT                                  
    0;                             
                                  
  -- RETURN 0 FOR Contact Model Temporaory model used in the POPUP for Add Contact                                                                                                                          
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR Contact Search Model used in the POPUP for Add Contact                                                                                                                          
  SELECT                                  
    0;                                  
                                  
  SELECT                                  
    A.AgencyID,                                  
    A.NickName,                                  
    A.AgencyType                                  
  FROM Agencies A                                  
  WHERE A.IsDeleted = 0                                  
  OR A.AgencyID = @AgencyID                                  
  ORDER BY A.NickName ASC                                  
                                  
  -- RETURN AGENCY LOCATION List Model                                                             
  SELECT                                  
    AL.AgencyLocationID,                                  
    AL.LocationName,                                  
    AL.AgencyID                      
  FROM AgencyLocations AL                                  
  WHERE AL.IsDeleted = 0                                  
  OR AL.AgencyLocationID = @AgencyLocationID;                                  
                                  
  -- RETURN CASE MANAGER List Model                                                                      
  SELECT                             
    0;                                  
  --SELECT CM.CaseManagerID, CM.FirstName, CM.LastName                                                         
  --FROM CASEMANAGERS CM                                                                        
  --WHERE CM.IsDeleted=0;                                                                                                                          
                                  
  -- RETURN 0 FOR CASE MANAGER Model                                                                                                                          
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR Referral Internal Message                                                      
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR CareConsentList                                                   
  SELECT                                  
    0;                                  
  -- RETURN 0 FOR SelfAdministrationList                                                                                                                          
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR HealthInformationList                                                             
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR AdmissionRequirementList                                                                                                       
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR AdmissionOrientationList                          
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR ZarephathCrisisPlanList                                                                                                                        
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR NetworkCrisisPlanList                                                                                                                          
  SELECT                                  
    0;                 
                                  
  -- RETURN 0 FOR VoiceMailList                                              
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR PermissionEmailList                                                                                                                          
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR PermissionSMSList                                                                                                                          
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR PHI YES/NO List                        
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR ROI YES/NO List                                                                                                
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR ZSP Respite Service Plan YES/NO List                                                                                                                          
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR ZSP Life Skills Service Plan YES/NO List                                                                        
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR ZSP Counselling Service Plan YES/NO List                                                                                   
  SELECT      0;                                  
                                  
  -- RETURN 0 FOR ZSPRespiteGuardianSignatureList YES/NO List                                                                                                           
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR ZSPRespiteBHPSignedList YES/NO List                                                                                                                           
  SELECT                                 
    0;                                  
                                  
  -- RETURN 0 FOR ZSPLifeSkillsGuardianSignatureList YES/NO List                                                                                                       
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR ZSPLifeSkillsBHPSignedList YES/NO List                                                                                                                           
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR ZSPCounsellingGuardianSignatureList YES/NO List                                                                                                                           
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR ZSPCounsellingBHPSignedList YES/NO List                                                                                                                           
  SELECT                         
    0;                                  
                                  
  -- RETURN 0 FOR NetworkServicePlanList YES/NO List                                                                                                                           
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR NetworkServiceGuardianSignatureList YES/NO List                                                                                                                       
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR NetworkServiceBHPSignedList YES/NO List                                            
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR NSPSPidentifyServiceList YES/NO List                    
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR BXAssessmentList YES/NO List                                                                                                          
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR BXAssessmentBHPSignedList YES/NO List                                           
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR Demographic LIST YES/NO List                                                                                                                           
  SELECT                                  
    0;                                  
           
  -- RETURN 0 FOR SNCDList LIST YES/NO List                                                                                                         
  SELECT                                  
    0;                                  
                                  
  -- RETURN 0 FOR ACAssessmentList LIST YES/NO List                                                                                            
  SELECT                                  
    0;                                  
                                  
  -- THIS QUERY WILL FETCH THE DX CODE LIST                                                                                                                          
  --SELECT D.DXCodeID,D.DXCodeName                                                                                                                          
  --FROM DXCodes D                         
                             
  -- RETURN 0 FOR DX CODE MODEL                                                                                       
  --SELECT 0;                                                                                                         
                                  
  SELECT                                  
    RD.ReferralDXCodeMappingID,                                  
    D.DXCodeID,                                  
    D.DXCodeName,                                  
    RD.Precedence,                                  
    RD.StartDate,                                  
    Rd.EndDate,                                  
    D.Description,                                  
    D.EffectiveFrom,         
    D.EffectiveTo,                                  
    RD.IsDeleted,                                  
    DT.DxCodeShortName,                                  
    Rd.CreatedDate,                                  
    Rd.CreatedBy,                                  
    D.DXCodeWithoutDot                                  
  FROM ReferralDXCodeMappings RD                                  
  INNER JOIN DxCodes D                                  
    ON D.DXCodeID = RD.DXCodeID                                  
  INNER JOIN DxCodeTypes DT                                  
    ON DT.DxCodeTypeID = D.DxCodeType                                  
  WHERE RD.ReferralID = @ReferralID                                  
  ORDER BY CASE                
    WHEN RD.Precedence IS NULL THEN 1                                  
    ELSE 0                                  
  END, RD.Precedence                                  
                                  
  SELECT                                  
    *                                  
  FROM LU_OutcomeMeasurementOptions                                  
  ORDER BY OutcomeMeasurementOptionID DESC                                  
                                  
  SELECT                                  
    0;                                  
                                  
  SELECT                                  
    R.ReferralID,                                  
   -- R.LastName + ', ' + R.FirstName AS Name,               
   dbo.GetGenericNameFormat(R.FirstName,R.MiddleName,R.LastName,@NameFormat) as Name,              
    R.AHCCCSID,                                  
    R.CISNumber,                        
    C.Phone1,                                  
    C.Address,                                  
    C.Email,                                  
    dbo.GetGenericNameFormat(C.FirstName,'',C.LastName,@NameFormat) AS ParentName,                                  
    r.Gender,                                  
    dbo.GetAge(R.Dob) AS Age,                                  
    rs.Status,                               
    RSM.ReferralID1,                                  
    RSM.ReferralID2,                                  
    RSM.ReferralSiblingMappingID,                                  
    RSM.CreatedDate,                                  
    RSM.CreatedBy                                  
                                  
  FROM Referrals R                                  
  INNER JOIN ReferralSiblingMappings RSM                                  
    ON (RSM.ReferralID1 = R.ReferralID                                  
    AND RSM.ReferralID2 = @ReferralID)                                  
    OR (RSM.ReferralID2 = R.ReferralID                                  
  AND RSM.ReferralID1 = @ReferralID)                                  
  LEFT JOIN ContactMappings CM                                  
    ON CM.ReferralID = R.ReferralID                                  
    AND (CM.IsDCSLegalGuardian = 1                                  
    OR CM.IsPrimaryPlacementLegalGuardian = 1)                                  
  LEFT JOIN ReferralStatuses RS                                  
    ON rs.ReferralStatusID = r.ReferralStatusID                                  
  LEFT JOIN Contacts C                                  
    ON CM.ContactID = C.ContactID                           
                                  
                                  
  --This query fetch the DocumentTypes from ComplianceMaster                                                                                
  SELECT                                  
    DocumentTypeID = ComplianceID,                                  
    DocumentKind = DocumentationType,                                  
    DocumentTypeName = DocumentName                                  
  FROM Compliances                                  
  WHERE IsDeleted = 0                                  
  AND UserType = 2                                  
                              
                                  
                                  
                        
                                  
  -- RETURN 0 fro kind of document                                                                                                                          
  SELECT                                  
    0;                                  
                                  
  -- THIS QUERY WILL FETCH THE PAYOR FROM THE TABLE PAYOR                                                                                             
  SELECT                                  
    P.PayorID,                                  
    P.PayorName,                          
    P.ClaimProcessor                          
  FROM Payors P                                  
  WHERE P.IsDeleted = 0                                  
  OR P.PayorID IN (SELECT                                  
    PayorID                          
  FROM ReferralPayorMappings                                  
  WHERE ReferralID = @ReferralID                            
  AND IsDeleted = 0)                                  
  ORDER BY P.PayorName ASC                                  
                                  
  -- RETURN 0 FOR ReferralPayorMapping Model                                   
  SELECT                                  
    RPM.*,                                  
    RPM.PayorID AS TempPayorID                                  
  FROM ReferralPayorMappings RPM                                  
  WHERE RPM.IsDeleted = 0                                  
  AND RPM.IsActive = 1                                  
  AND RPM.ReferralID = @ReferralID                                  
                                  
  -- RETURN 0 FOR ReferralDocument Model                                                                                                   
  SELECT                                  
    0;                                  
                                  
  -- THIS QUERY WILL FETCH ALL THE PAYOR FOR THE REFERRAL                                                                 
  SELECT                                  
    RPM.ReferralPayorMappingID,                                  
    RPM.PayorEffectiveDate,                                  
    RPM.PayorEffectiveEndDate,                                  
    RPM.IsActive,                                  
    P.PayorName,                                  
    dbo.GetGenericNameFormat(E.FirstName,E.MiddleName,E.LastName,@NameFormat) AS AddedByName,                                  
    dbo.GetGenericNameFormat(E1.FirstName,E1.MiddleName,E1.LastName,@NameFormat) AS UpdatedByName,                                  
    RPM.CreatedDate,                                  
    RPM.UpdatedDate                                  
  FROM ReferralPayorMappings RPM                                  
  INNER JOIN Payors P                                  
    ON P.PayorID = RPM.PayorID                                  
  INNER JOIN Employees E                                  
    ON E.EmployeeID = RPM.CreatedBy                                  
  INNER JOIN Employees E1                                  
    ON E1.EmployeeID = RPM.UpdatedBy                                  
    AND RPM.IsDeleted = 0                                  
    AND RPM.ReferralID = @ReferralID                              
  ORDER BY RPM.UpdatedDate DESC             
                                  
  -- This query will fetch count of Schedule history                                                                                                                          
  SELECT                                  
    COUNT(*)                                  
  FROM ScheduleMasters                                  
  WHERE ReferralID = @ReferralID                                  
  AND IsDeleted = 0                                  
                                  
  SELECT                                  
    ReferralSourceID, ReferralSourceName                               
  FROM ReferralSources  where IsDeleted=0        
  UNION SELECT  ReferralSourceID='99999999',ReferralSourceName='Add/Edit/Item'     
  ORDER BY ReferralSourceName ASC                                  
                                  
  --select top 1 ISNULL(ServiceDate,NULL) from Notes where ReferralID=@ReferralID and IsBillable=1 order by NoteID ASC                                 
  --if((select count(*) from Notes where ReferralID=@ReferralID and IsBillable=1) > 0)                                                                                                                          
  SELECT TOP 1                                  
    ServiceDate                                  
  FROM Notes                    
  WHERE ReferralID = @ReferralID                                  
  AND IsBillable = 1                                  
  AND IsDeleted = 0                                  
  ORDER BY StartTime ASC                                  
  --else                                                                                                  
  --select null ServiceDate;                                                                                                                          
                                  
  SELECT                                  
    0;           
                                  
  SELECT                                  
    0;                                  
  SELECT                                  
    0;                                  
                                  
  SELECT                                  
    rp.*,                                  
    p.PreferenceName                                  
  FROM ReferralPreferences rp                                  
  INNER JOIN Preferences p                                  
    ON rp.PreferenceID = p.PreferenceID                                  
  WHERE ReferralID = @ReferralID                                  
  AND P.KeyType = @PreferenceType_Preference                                  
                                  
                                  
  -- SKILLS MASTER                                                                                                              
  --SELECT * FROM Preferences P WHERE P.KeyType=@PreferenceType_Skill                                                           
                                  
  -- SKILLS MASTER                                                                             
  IF (@ReferralID = 0)                        
    SELECT                                  
      *                                  
    FROM Preferences P                                  
    WHERE P.KeyType = @PreferenceType_Skill                                  
    AND IsDeleted = 0                                  
  ELSE                                  
    SELECT                                  
      *                                  
    FROM Preferences P                                  
    WHERE P.KeyType = @PreferenceType_Skill --AND IsDeleted=0                                                            
                                  
                                  
                                  
  SELECT                                  
    EP.PreferenceID                                  
  FROM ReferralPreferences EP                                  
  INNER JOIN Preferences P                                  
    ON EP.PreferenceID = P.PreferenceID                                  
  WHERE ReferralID = @ReferralID                                  
  AND P.KeyType = @PreferenceType_Skill                                  
                                  
  SELECT                                  
    *                                  
  FROM States                                  
                                  
  SELECT                                  
    Name = Title,                                  
    Value = DDMasterID                                  
  FROM DDMaster                                  
  WHERE IsDeleted = 0                                  
  AND ItemType = @DDType_CareType                                  
                                  
  SELECT                                  
    0;                                  
                                  
  SELECT                                  
    Value = DDMasterID,                                  
    Name = Title                               
  FROM DDMaster                                  
  WHERE IsDeleted = 0                                  
  AND ItemType = @DDType_AdmissionType                                  
  SELECT                                  
    Value = DDMasterID,                                  
    Name = Title                                  
  FROM DDMaster                                  
WHERE IsDeleted = 0                                  
  AND ItemType = @DDType_PriorAuthorizationFrequency                                  
  SELECT                                  
    Value = DDMasterID,                                  
    Name = Title                                  
  FROM DDMaster                                  
  WHERE IsDeleted = 0                                  
  AND ItemType = @DDType_AdmissionSource                                  
  SELECT                                  
    Value = DDMasterID,                                  
    Name = Title                                  
  FROM DDMaster                                  
  WHERE IsDeleted = 0                                  
  AND ItemType = @DDType_PatientStatus                                  
  --SELECT Value=DDMasterID,Name=Title FROM DDMaster where IsDeleted=0 and ItemType=@DDType_FacilityCode                                                              
                                  
  SELECT DISTINCT                                  
    Value = DDMasterID,                                  
    Name = Title                                  
  FROM DDMaster DD                                  
  LEFT JOIN ReferralBillingSettings RBS                                  
    ON RBS.POS_CMS1500 = DD.DDMasterID                                  
  LEFT JOIN ReferralBillingSettings RBS_U                                  
    ON RBS_U.POS_UB04 = DD.DDMasterID                                  
  WHERE ItemType = @DDType_FacilityCode                                  
  AND (((RBS.ReferralBillingSettingID IS NOT NULL                                  
  AND RBS.ReferralID = @ReferralID)                                  
  OR DD.IsDeleted = 0)                                  
  OR ((RBS_U.ReferralBillingSettingID IS NOT NULL                                  
  AND RBS_U.ReferralID = @ReferralID)                                  
  OR DD.IsDeleted = 0))                                  
                                  
  SELECT                          
    Value = DDMasterID,                                  
    Name = Title                                  
  FROM DDMaster                                  
  WHERE IsDeleted = 0                                  
  AND ItemType = @DDType_VisitType                                  
                                  
  --Select Compliance(Internal Documentation) Details                                                                                  
  SELECT                                  
    C.ComplianceID,                                  
    C.DocumentName,                                  
    C.IsTimeBased,                            
    RCM.ReferralComplianceID,                                  
    RCM.Value,                                  
    RCM.ExpirationDate,                                  
    ReferralID = @ReferralID,                                  
    SectionName = C1.DocumentName--,SubSectionName=DSS.Title                                                                  
  FROM Compliances C                                  
  LEFT JOIN ReferralComplianceMappings RCM                                  
    ON RCM.ComplianceID = C.ComplianceID                                  
    AND RCM.ReferralID = @ReferralID                                  
  LEFT JOIN Compliances C1                                  
    ON C1.ParentID = C.ComplianceID      WHERE C.UserType = 2                                  
  AND C.DocumentationType = 1                                  
  AND C.IsDeleted = 0                                  
                                  
  --Select Compliance(External Documentation) Master                                       
  SELECT                                  
    C.ComplianceID,                                  
    C.DocumentName,                                  
    C.IsTimeBased,                                  
    RCM.ReferralComplianceID,                                  
    RCM.Value,                                  
    RCM.ExpirationDate,                                  
    ReferralID = @ReferralID,                                  
    SectionName = C1.DocumentName--,SubSectionName=DSS.Title                                                          
  FROM Compliances C                                  
  LEFT JOIN ReferralComplianceMappings RCM                                  
    ON RCM.ComplianceID = C.ComplianceID                                  
    AND RCM.ReferralID = @ReferralID                                  
  LEFT JOIN Compliances C1                                  
    ON C1.ParentID = C.ComplianceID                                  
  WHERE C.UserType = 2                                  
  AND C.DocumentationType = 2                                  
  AND C.IsDeleted = 0                                  
                                  
  SELECT                                  
    d.DDMasterID AS Value,                                  
    d.Title AS Name          
  FROM dbo.DDMaster d                                  
  WHERE d.ItemType = @DDType_PatientFrequencyCode                                  
  AND IsDeleted = 0                                  
                                  
  DECLARE @DDType_EmployeeGroup int = (SELECT DDMasterTypeID FROM lu_DDMasterTypes WHERE Name = 'Employee Group')                                  
  SELECT              
    d.DDMasterID AS Value,                                  
    d.Title AS Name                                  
  FROM dbo.DDMaster d                                  
  WHERE d.ItemType = @DDType_EmployeeGroup                                  
  AND IsDeleted = 0                                  
                                  
  SELECT                                  
    d.DDMasterID AS Value,                                  
    d.Title AS Name                                  
  FROM dbo.DDMaster d                                  
  WHERE d.ItemType = @DDType_BeneficiaryType                                  
  AND IsDeleted = 0                                  
                                  
  SELECT                                  
    RBT.ReferralBeneficiaryTypeID,                                  
    RBT.ReferralID,                                  
    RBT.BeneficiaryTypeID,                                  
    DM.[Value] AS BeneficiaryTypeName,                                  
    RBT.BeneficiaryNumber                                  
  FROM ReferralBeneficiaryTypes RBT           
  INNER JOIN DDMaster DM                                  
    ON RBT.BeneficiaryTypeID = DM.DDMasterID                                  
  WHERE RBT.ReferralID = @ReferralID                                  
                                  
  SELECT                                  
    RP.ReferralPhysicianID,                                  
    RP.ReferralID,                     
    RP.PhysicianID,                                  
    P.FirstName,                                  
    P.MiddleName,                                  
    P.LastName,                                  
    P.Address,                                  
    P.Email,                                  
    P.Phone,                                  
dm.Title AS PhysicianType,                                  
   dbo.GetGenericNameFormat(P.FirstName,P.MiddleName,P.LastName,@NameFormat) AS PhysicianName                                  
  FROM ReferralPhysicians RP                                  
  INNER JOIN Physicians P                                  
    ON RP.PhysicianID = P.PhysicianID                                  
  INNER JOIN DDMaster dm                                  
    ON dm.DDMasterID = p.PhysicianTypeID                                  
  WHERE RP.ReferralID = @ReferralID                                  
                           
  --select dm.DDMasterID As Value,dm.Title As Name                                          
  --from DDMaster dm                                        
  --inner join lu_DDMasterTypes lu on lu.DDMasterTypeID=dm.ItemType where lu.Name='PhysicianType'                                        
                                  
  SELECT                                  
    dm.DDMasterID AS Value,                                  
    dm.Title AS Name                                  
  FROM DDMaster dm                                  
  INNER JOIN lu_DDMasterTypes lu                                  
    ON lu.DDMasterTypeID = dm.ItemType                                  
  WHERE lu.Name = 'Service Type'                                  
  AND dm.IsDeleted = 0                                  
                                  
  -- DDType_RevenueCode                                      
  -- Kundan: 18-05-2020                                      
  SELECT                                  
    DDMasterID AS Value,                                  
    Title AS Name                                  
  FROM DDMaster                                  
  WHERE IsDeleted = 0                                  
  AND ItemType = @DDType_RevenueCode                                  
  -- Service Code List                                      
  -- Kundan: 18-05-2020                                      
  SELECT                                  
    ServiceCodeID AS Value,                                  
    (ServiceCode + ' - ' + ServiceName) AS Name                                  
  FROM ServiceCodes                                  
  WHERE IsDeleted = 0                                  
  -- Taxonomy Code List                                      
  -- Kundan: 10-06-2020                                      
  SELECT                                  
    dm.Title AS Name,                                  
    dm.DDMasterID AS Value                                  
  FROM DDMaster dm                                  
  INNER JOIN lu_DDMasterTypes lu                                  
    ON lu.DDMasterTypeID = dm.ItemType                                  
  WHERE lu.Name = 'Taxonomy Code'                                 
                                  
    SELECT                                  
    dm.Title AS Name,                                  
    dm.DDMasterID AS Value                               
  FROM DDMaster dm                                  
  INNER JOIN lu_DDMasterTypes lu                                  
    ON lu.DDMasterTypeID = dm.ItemType                                  
  WHERE lu.Name = 'Race'                                                                  
  SELECT                                  
    dm.Title AS Name,                                  
    dm.DDMasterID AS Value                                  
  FROM DDMaster dm                                  
  INNER JOIN lu_DDMasterTypes lu                                  
    ON lu.DDMasterTypeID = dm.ItemType                                  
  WHERE lu.Name = 'Ethnicity'  AND DM.IsDeleted=0              
    SELECT                                  
    dm.Title AS Name,                                  
    dm.DDMasterID AS Value                                  
  FROM DDMaster dm                                  
  INNER JOIN lu_DDMasterTypes lu                                  
    ON lu.DDMasterTypeID = dm.ItemType                                  
  WHERE lu.Name = 'CodeStatus'   AND DM.IsDeleted=0                              
                      
                      
 SELECT DDMasterID AS Value , Title Name FROM lu_DDMasterTypes T                      
 INNER JOIN DDMaster DM ON                         
 DM.ItemType = T.DDMasterTypeID                        
 WHERE T.Name = 'Dosage Time'  AND DM.IsDeleted=0                      
                      
                      
END 