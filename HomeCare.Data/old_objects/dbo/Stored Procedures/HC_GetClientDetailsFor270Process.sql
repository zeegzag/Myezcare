CREATE PROCEDURE [dbo].[HC_GetClientDetailsFor270Process]
@PayorIDs VARCHAR(MAX),  
@ServiceIDs VARCHAR(MAX),  
@ReferralStatusIDs VARCHAR(MAX)=NULL,  
@ClientName VARCHAR(MAX),  
@AllServiceText VARCHAR(MAX)  
AS  
BEGIN  
  
SELECT * FROM PayorEdi270Settings  
  
-- UPDATE PayorEdi270Settings SET ISA13_InterchangeControlNumber='5', ISA06_InterchangeSenderId='73886401',BHT03_ReferenceIdentification=NULL,BHT06_TransactionTypeCode=NULL  
  
SELECT TOP 1 HeirarchicalLevelCode=Source_HeirarchicalLevelCode, EntityIdentifierCode=Source_EntityIdentifierCode,EntityTypeQualifier=Source_EntityTypeQualifier,  
NameLastOrOrganizationName=Source_NameLastOrOrganizationName,IdCodeQualifier=Source_IdCodeQualifier,IdCodeQualifierEnum=Source_IdCodeQualifierEnum  FROM PayorEdi270Settings  
  
SELECT TOP 1 HeirarchicalLevelCode=Receiver_HeirarchicalLevelCode, EntityIdentifierCode=Receiver_EntityIdentifierCode,EntityTypeQualifier=Receiver_EntityTypeQualifier,  
NameLastOrOrganizationName=Receiver_NameLastOrOrganizationName,IdCodeQualifier=Receiver_IdCodeQualifier,IdCodeQualifierEnum=Receiver_IdCodeQualifierEnum  FROM PayorEdi270Settings  
  
--SELECT HeirarchicalLevelCode='20',EntityIdentifierCode='PR',EntityTypeQualifier='2',NameLastOrOrganizationName='AHCCCS',IdCodeQualifier='FI',IdCodeQualifierEnum='866004791'  
--SELECT HeirarchicalLevelCode='21',EntityIdentifierCode='1P',EntityTypeQualifier='2',NameLastOrOrganizationName='ZAREPHATH',IdCodeQualifier='XX',IdCodeQualifierEnum='1730439167'  
  
IF(LEN(@PayorIDs)=0)  
    SELECT @PayorIDs=SUBSTRING((SELECT ',' + Convert(varchar(max),PayorID) FROM Payors WHERE IsDeleted=0 ORDER BY PayorName FOR XML PATH('')),2,200000)       
  
IF(LEN(@ServiceIDs)=0)  
    SET @ServiceIDs=@AllServiceText;  
  
SELECT  R.FirstName, R.LastName,Dob=CONVERT(VARCHAR(10), R.Dob, 112), R.Gender ,R.AHCCCSID,R.CISNumber, MedicalRecordNumber=R.AHCCCSID, R.PolicyNumber,  
CASE WHEN P.ShortName='UHC' THEN R.AHCCCSID ELSE R.AHCCCSID END AS SubmitterIdCodeQualifierEnum,   
SubmitterIdCodeQualifier='MI',SubmitterDateTimePeriodFormatQualifier='D8',  SubmitterDTPQualifier='291', SubmitterDTPFormatQualifier='D8',SubmitterDTPDateTimePeriod=CONVERT(VARCHAR(8),GETDATE(),112),  
SubmitterEligibility01='30',  
SubmitterEntityIdentifierCode='IL',SubmitterEntityTypeQualifier='1',  
HeirarchicalLevelCode='22'  
FROM Referrals R  
INNER JOIN ReferralPayorMappings RPM ON RPM.ReferralID=R.ReferralID AND RPM.IsActive=1  
INNER JOIN Payors P ON P.PayorID=RPM.PayorID  
WHERE R.IsDeleted=0 --AND R.ReferralStatusID=@ReferralStatusID   
--AND (  
--    ( @ServiceIDs =@AllServiceText AND (RespiteService=1 OR LifeSkillsService=1 OR CounselingService=1 OR R.ConnectingFamiliesService=1) ) OR  
-- ( @ServiceIDs  ='Respite' AND RespiteService=1 )  OR  
-- ( @ServiceIDs  ='Life Skills' AND LifeSkillsService=1 )  OR  
-- ( @ServiceIDs  ='Counselling' AND CounselingService=1 )  OR  
-- ( @ServiceIDs  ='Connecting Families' AND ConnectingFamiliesService=1 )  
--)  
 AND ( @ReferralStatusIDs IS NULL OR LEN(@ReferralStatusIDs)=0 OR  R.ReferralStatusID IN (SELECT CAST(val AS bigint) FROM GETCSVTABLE(@ReferralStatusIDs))  )  
 AND P.PayorID IN (SELECT CAST(val AS bigint) FROM GETCSVTABLE(@PayorIDs))  
 AND ((@ClientName IS NULL OR LEN(R.LastName)=0)   
    OR  
  ( (R.FirstName LIKE '%'+@ClientName+'%' )OR    
    (R.LastName  LIKE '%'+@ClientName+'%') OR    
    (R.FirstName +' '+R.LastName like '%'+@ClientName+'%') OR    
    (R.LastName +' '+R.FirstName like '%'+@ClientName+'%') OR    
    (R.FirstName +', '+R.LastName like '%'+@ClientName+'%') OR    
    (R.LastName +', '+R.FirstName like '%'+@ClientName+'%'))  
  )     
                                  
  
  
SELECT PayorIDs=@PayorIDs, ServiceIDs=@ServiceIDs, ClientName=@ClientName  
  
END
