-- EXEC SetReferralMissingDocument @ReferralID = '2',@EMAILTEMPLATETYPEID = '7', @AgencyROI = 'Agency ROI', @NetworkServicePlan = 'Network Service Plan', @BXAssessment = 'BX Assessment', @Demographic = 'Demographic', @SNCD = 'SNCD' , @NetworkCrisisPlan='',@CAZOnly='',@MISSING='MISSING',@EXPIRED='EXPIRED'
CREATE PROCEDURE [dbo].[SetReferralMissingDocument]            
@REFERRALID BIGINT,            
@EMAILTEMPLATETYPEID VARCHAR(100),            
@AGENCYROI VARCHAR(50),            
@NETWORKSERVICEPLAN VARCHAR(50),            
@BXASSESSMENT VARCHAR(50),            
@DEMOGRAPHIC VARCHAR(50),            
@SNCD VARCHAR(50),            
@NetworkCrisisPlan VARCHAR(50),            
@CAZOnly VARCHAR(50),            
@MISSING VARCHAR(15),            
@EXPIRED VARCHAR(15)            
AS            
BEGIN            
 DECLARE @CurrentDate Date=CAST(GETDATE() AS DATE);             
 DECLARE @Temptable TABLE(MissingDocumentName VARCHAR(50),MissingDocumentType VARCHAR(50));            
             
 -- return Email Template            
 SELECT * FROM EmailTemplates WHERE EmailTemplateTypeID=@EmailTemplateTypeID            
             
 -- List od missing documents            

 --INSERT INTO @Temptable            
 -- SELECT @AgencyROI,CASE WHEN AROI='N' THEN @MISSING WHEN AROI='Y' AND AROIExpirationDate IS NOT NULL AND AROIExpirationDate < @CurrentDate THEN @Expired END             
 -- FROM referrals WHERE ReferralID=@REFERRALID             
 -- AND (((AROI='N') or ((AROI='Y')and AROIExpirationDate < @CurrentDate)))            
            
 INSERT INTO @Temptable            
  SELECT @NETWORKSERVICEPLAN,CASE WHEN NetworkServicePlan=0 THEN @MISSING WHEN NetworkServicePlan=1 AND NSPExpirationDate IS NOT NULL AND NSPExpirationDate < @CurrentDate THEN @Expired END             
  FROM referrals WHERE ReferralID=@REFERRALID            
   AND (((NetworkServicePlan=0) or ((NetworkServicePlan=1)and NSPExpirationDate < @CurrentDate)))            
            
 INSERT INTO @Temptable            
  SELECT @BXASSESSMENT,CASE WHEN BXAssessment=0 THEN @MISSING WHEN BXAssessment=1 AND BXAssessmentExpirationDate IS NOT NULL AND BXAssessmentExpirationDate < @CurrentDate THEN @Expired END             
  FROM referrals WHERE ReferralID=@REFERRALID            
   AND (((BXAssessment=0) or ((BXAssessment=1)and BXAssessmentExpirationDate < @CurrentDate)))            
            
 --INSERT INTO @Temptable            
 -- SELECT @DEMOGRAPHIC,CASE WHEN Demographic='N' THEN @MISSING WHEN Demographic='Y' AND DemographicExpirationDate IS NOT NULL AND DemographicExpirationDate < @CurrentDate THEN @Expired END             
 -- FROM referrals WHERE ReferralID=@REFERRALID            
 --  AND (((Demographic='N') or ((Demographic='Y')and DemographicExpirationDate < @CurrentDate)))            
            
 INSERT INTO @Temptable            
  SELECT @SNCD,CASE WHEN SNCD='N' THEN @MISSING WHEN SNCD='Y' AND SNCDExpirationDate IS NOT NULL AND SNCDExpirationDate < @CurrentDate THEN @Expired END             
  FROM referrals WHERE ReferralID=@REFERRALID            
   AND (((SNCD='N') or ((SNCD='Y')and SNCDExpirationDate < @CurrentDate)))            
            
 INSERT INTO @Temptable            
  SELECT @NetworkCrisisPlan,CASE WHEN NetworkCrisisPlan='N' THEN @MISSING WHEN NetworkCrisisPlan='Y' AND NCPExpirationDate IS NOT NULL AND NCPExpirationDate < @CurrentDate THEN @Expired END             
  FROM referrals WHERE ReferralID=@REFERRALID             
   AND (((NetworkCrisisPlan='N') or ((NetworkCrisisPlan='Y')and NCPExpirationDate < @CurrentDate)))            
            
 INSERT INTO @Temptable            
  SELECT @CAZOnly, CASE WHEN rcl.ReferralCheckList is null or rcl.ReferralCheckList=0 THEN @MISSING  END             
  FROM referrals ref LEFT JOIN ReferralCheckLists rcl on ref.ReferralID=rcl.ReferralID            
  LEFT JOIN ReferralPayorMappings RPM on RPM.ReferralID=REF.ReferralID            
     LEFT JOIN Payors P on P.PayorID=RPM.PayorID            
  WHERE ref.ReferralID=@REFERRALID AND (RPM.IsActive IS NULL OR RPM.IsActive=1) and p.PayorName like '%Cenpatico%'        
  AND ((rcl.ReferralCheckList is null) or ((rcl.ReferralCheckList=0)))            
            
  -- return list of missing or expired documents            
  SELECT * FROM @Temptable;            
              
  -- return Clientname, ToEmail fro fill email body with token            
  SELECT R.LastName + ', ' + R.FirstName AS ClientName, RecordRequestEmail AS ToEmail,C.Email,C.FirstName as CaseManagerFirstName,C.LastName  as CaseManagerLastName,R.ClientNickName       
  ,R.AHCCCSID, CONVERT(VARCHAR(10),CONVERT(datetime,R.Dob ,1),101) as DateofBirth
   from  Referrals  R         
  Left join CaseManagers C on C.CaseManagerID=R.CaseManagerID        
  where REFERRALID=@REFERRALID        
          
END