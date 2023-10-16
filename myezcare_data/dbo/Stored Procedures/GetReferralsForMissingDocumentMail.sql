-- EXEC [GetReferralsForMissingDocumentMail] @Yes='Yes',@NotApplicable='NA',@MISSING='Missing',@EXPIRED='Expire'
CREATE PROCEDURE [dbo].[GetReferralsForMissingDocumentMail]        
--@AGENCYROI VARCHAR(50),          
--@NETWORKSERVICEPLAN VARCHAR(50),          
--@BXASSESSMENT VARCHAR(50),          
--@DEMOGRAPHIC VARCHAR(50),          
--@SNCD VARCHAR(50),          
--@NetworkCrisisPlan VARCHAR(50),          
--@CAZOnly VARCHAR(50),          
@Yes VARCHAR(10),  
@NotApplicable VARCHAR(100),  
@MISSING VARCHAR(15),          
@EXPIRED VARCHAR(15)          
AS          
BEGIN          
	DECLARE @CurrentDate Date=CAST(GETDATE() AS DATE) ; 
	 SELECT
		R.ReferralID,R.FirstName,R.LastName,R.Dob,R.AHCCCSID,C.FirstName+' '+ C.LastName as CaseManager,
		ISNULL(R.RecordRequestEmail,C.Email) RecordRequestEmail,
		--'brad@zrpath.com' RecordRequestEmail,
		--'jyadav@kairasoftware.com' RecordRequestEmail,
		
			
		    ServicePlanExpirationDate= (CASE  WHEN (R.NetworkServicePlan=0) THEN @MISSING --CONVERT(VARCHAR(MAX),R.NSPExpirationDate)
									   WHEN ((R.NetworkServicePlan=1) AND R.NSPExpirationDate <= @CurrentDate) THEN  @EXPIRED +'<br/> '+  CONVERT(VARCHAR(MAX),R.NSPExpirationDate,101)
									   WHEN ((R.NetworkServicePlan=1) AND R.NSPExpirationDate > @CurrentDate) THEN  CONVERT(VARCHAR(MAX),R.NSPExpirationDate,101)
									   ELSE @MISSING END), 

		    ServicePlanExpirationStatus= (CASE  WHEN (R.NetworkServicePlan=0) THEN  0
										 WHEN ((R.NetworkServicePlan=1) AND R.NSPExpirationDate <= @CurrentDate) THEN 0
										 WHEN ((R.NetworkServicePlan=1) AND R.NSPExpirationDate > @CurrentDate) THEN 1
										 ELSE 0	END),
			
			SPGuardianSignature= (CASE WHEN (R.NSPGuardianSignature=0) THEN @MISSING WHEN (R.NSPGuardianSignature=1) THEN @Yes ELSE @MISSING END),
			SPGuardianSignatureStatus= (CASE WHEN (R.NSPGuardianSignature=0) then 0  WHEN (R.NSPGuardianSignature=1) then 1 ELSE 0  END),
			
			SPBHPSignature= (CASE WHEN (R.NSPBHPSigned=0) THEN @MISSING WHEN (R.NSPBHPSigned=1) THEN @Yes ELSE @MISSING END) ,			
			SPBHPSignatureStatus=(CASE WHEN (R.NSPBHPSigned=0) THEN 0 WHEN (R.NSPBHPSigned=1) THEN 1 ELSE 0 END) ,

			SPIdentify= (CASE WHEN (R.NSPSPidentifyService='N') THEN @MISSING WHEN (R.NSPSPidentifyService='Y') THEN @Yes ELSE 'NA' END) ,			
			SPIdentifyStatus=(CASE WHEN (R.NSPSPidentifyService='N') THEN 0 WHEN (R.NSPSPidentifyService='Y') THEN 1 ELSE 1 END) ,

			BXAssessmentExpirationDate= (CASE WHEN (R.BXAssessment=0) THEN  @MISSING -- CONVERT(VARCHAR(MAX),,R.BXAssessmentExpirationDate)
										  WHEN ((R.BXAssessment=1) AND R.BXAssessmentExpirationDate <= @CurrentDate) then @EXPIRED + '<br/> ' + CONVERT(VARCHAR(MAX),R.BXAssessmentExpirationDate,101)
										  WHEN ((R.BXAssessment=1) AND R.BXAssessmentExpirationDate > @CurrentDate) then CONVERT(VARCHAR(MAX),R.BXAssessmentExpirationDate,101)		
										  ELSE @MISSING END),
			
			BXAssessmentExpirationStatus= (CASE WHEN (R.BXAssessment=0) then 0
										  WHEN ((R.BXAssessment=1) AND R.BXAssessmentExpirationDate <= @CurrentDate) then 0
										  WHEN ((R.BXAssessment=1) AND R.BXAssessmentExpirationDate > @CurrentDate) then 1
										  ELSE 0 END) ,


			BXAssessmentBHPSigned= (CASE WHEN (BXAssessmentBHPSigned=0) THEN @MISSING WHEN (BXAssessmentBHPSigned=1) THEN @Yes ELSE @MISSING	END),
			BXAssessmentBHPSignedStatus= (CASE WHEN (BXAssessmentBHPSigned=0) THEN 0 WHEN (BXAssessmentBHPSigned=1) THEN 1 ELSE 0 END),



			Demographic= (CASE WHEN (R.Demographic='N') THEN @MISSING 
							  WHEN  (R.Demographic='NA') THEN @NotApplicable
							  WHEN ((R.Demographic='Y') AND R.DemographicExpirationDate <= @CurrentDate) then @EXPIRED +'<br/> '+ CONVERT(VARCHAR(MAX),R.DemographicExpirationDate,101)
							  WHEN ((R.Demographic='Y') AND R.DemographicExpirationDate > @CurrentDate) then  CONVERT(VARCHAR(MAX),R.DemographicExpirationDate,101)
							  ELSE @MISSING END) ,

			DemographicStatus =(CASE WHEN (R.Demographic='N') THEN 0
									 WHEN  (R.Demographic='NA') THEN 1
								     WHEN ((R.Demographic='Y') AND R.DemographicExpirationDate <= @CurrentDate) then 0
								     WHEN ((R.Demographic='Y') AND R.DemographicExpirationDate > @CurrentDate) then 1
									 ELSE 0 end) ,


			ROIExpirationDate= (CASE WHEN (R.AROI='N') THEN @MISSING 
									 WHEN (R.AROI='NA') THEN @NotApplicable
								     WHEN ((R.AROI='Y') AND R.AROIExpirationDate <= @CurrentDate) then @EXPIRED +'<br/> '+ CONVERT(VARCHAR(MAX),R.AROIExpirationDate,101)
								     WHEN ((R.AROI='Y') AND R.AROIExpirationDate > @CurrentDate) then  CONVERT(VARCHAR(MAX),R.AROIExpirationDate,101)
								     ELSE @MISSING END) ,

			ROIExpirationDateStatus =(CASE WHEN (R.AROI='N')  THEN 0
										   WHEN (R.AROI='NA') THEN 1
									       WHEN ((R.AROI='Y') AND R.AROIExpirationDate <= @CurrentDate) then 0
									       WHEN ((R.AROI='Y') AND R.AROIExpirationDate > @CurrentDate) then 1
										   ELSE 0 end) ,
			
			
			SNCDCompletionDate= (CASE WHEN (R.SNCD='N') THEN @MISSING 
									 WHEN (R.SNCD='NA') THEN @NotApplicable
								     WHEN ((R.SNCD='Y') AND R.SNCDExpirationDate <= @CurrentDate) then 'Completed' +'<br/> '+ CONVERT(VARCHAR(MAX),R.SNCDExpirationDate,101)
								     WHEN ((R.SNCD='Y') AND R.SNCDExpirationDate > @CurrentDate) then  CONVERT(VARCHAR(MAX),R.SNCDExpirationDate,101)
								     ELSE @MISSING END) ,

			SNCDCompletionDateStatus =(CASE WHEN (R.SNCD='N')  THEN 0
										   WHEN (R.SNCD='NA') THEN 1
									       WHEN ((R.SNCD='Y') AND R.SNCDExpirationDate <= @CurrentDate) then 1
									       WHEN ((R.SNCD='Y') AND R.SNCDExpirationDate > @CurrentDate) then 1
										   ELSE 0 end) ,
								
									
			SNCD = (CASE WHEN (R.SNCD='N') THEN @MISSING
						      WHEN (R.SNCD='NA') THEN @NotApplicable
				              WHEN (SNCD='Y') THEN @Yes
							  ELSE @MISSING END), 

            SNCDStatus = (CASE WHEN (R.SNCD='N') THEN 0
						      WHEN (R.SNCD='NA') THEN 1
				              WHEN (SNCD='Y') THEN 1
						      ELSE 0 END) 

	 FROM Referrals R
	 INNER JOIN CaseManagers C on R.CaseManagerID=C.CaseManagerID
	 WHERE R.ReferralStatusID=1  --AND AHCCCSID='A96410541' --AND ReferralID in(2700,1323,1324,1325,2503,1951,2273,304,809,938,1957)
END
