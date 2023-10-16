CREATE PROCEDURE [dbo].[HC_SetReferralMissingDocument]                  
@ReferralID BIGINT,                  
@EmailTemplateTypeID VARCHAR(100),             
@Missing VARCHAR(15),                  
@Expired VARCHAR(15),  
@OrganizationDate DATE  
AS                  
BEGIN                  
 DECLARE @CurrentDate Date=CAST(GETDATE() AS DATE);                   
 DECLARE @Temptable TABLE(MissingDocumentName VARCHAR(50),DocumentationType VARCHAR(50),MissingDocumentType VARCHAR(50));             
                   
 -- return Email Template                  
 SELECT * FROM EmailTemplates WHERE EmailTemplateTypeID=@EmailTemplateTypeID                  
                   
 -- List of missing documents (OLD CODE)  
 -- INSERT INTO @Temptable    
 --SELECT MissingDocumentName=C.DocumentName,DocumentationType=CASE WHEN C.DocumentationType=1 THEN 'Internal' ELSE 'External' END, MissingDocumentType=@Missing    
 --FROM ReferralComplianceMappings RCM    
 --INNER JOIN Compliances C ON RCM.ComplianceID=C.ComplianceID AND C.IsDeleted=0    
 --WHERE RCM.ReferralID=@ReferralID AND Value=0    
  
 -- List of missing documents (NEW CODE)  
  INSERT INTO @Temptable    
 SELECT MissingDocumentName=C.DocumentName,DocumentationType=CASE WHEN C.DocumentationType=1 THEN 'Internal' ELSE 'External' END, MissingDocumentType='Missing'    
 FROM Compliances C  
 LEFT JOIN ReferralComplianceMappings RCM ON RCM.ReferralID=@ReferralID AND C.ComplianceID=RCM.ComplianceID  
 WHERE C.UserType=2 AND C.IsDeleted=0 AND (RCM.Value=0 OR RCM.ReferralComplianceID IS NULL)  
    
  INSERT INTO @Temptable    
 SELECT MissingDocumentName=C.DocumentName,DocumentationType=CASE WHEN C.DocumentationType=1 THEN 'Internal' ELSE 'External' END, MissingDocumentType=@Expired    
 FROM ReferralComplianceMappings RCM    
 INNER JOIN Compliances C ON RCM.ComplianceID=C.ComplianceID AND C.IsDeleted=0 AND IsTimeBased=1    
 WHERE RCM.ReferralID=@ReferralID AND RCM.Value=1 AND RCM.ExpirationDate<@OrganizationDate  
                  
  -- return list of missing or expired documents                  
  SELECT * FROM @Temptable;                  
                    
  -- return Clientname, ToEmail fro fill email body with token                  
  SELECT R.LastName + ', ' + R.FirstName AS ClientName, E.Email AS ToEmail,C.Email,E.FirstName as CaseManagerFirstName,E.LastName  as CaseManagerLastName,R.ClientNickName
  ,R.AHCCCSID, CONVERT(VARCHAR(10),CONVERT(datetime,R.Dob ,1),101) as DateofBirth      
   FROM  Referrals R
  INNER JOIN ContactMappings CM ON CM.ReferralID=R.ReferralID AND CM.ContactTypeID=1    
  INNER JOIN Contacts C ON C.ContactID=CM.ContactID AND C.IsDeleted=0
  LEFT JOIN Employees E ON E.EmployeeID=R.CaseManagerID
  WHERE R.ReferralID=@ReferralID    
    
END ---issue
