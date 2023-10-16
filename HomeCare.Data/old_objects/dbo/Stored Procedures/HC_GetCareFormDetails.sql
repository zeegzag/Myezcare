-- [HC_GetCareFormDetails] 0,14232        
-- [HC_GetCareFormDetails] 1,0        
CREATE PROCEDURE [dbo].[HC_GetCareFormDetails]                      
@CareFormID bigint,        
@ReferralID bigint        
AS                                
BEGIN                                  
      
IF (@CareFormID > 0 AND @ReferralID = 0)        
BEGIN        
     
 SELECT CF.*,      
 Str_ServiceRequested =      
 CASE       
 WHEN (CF.ServiceRequested IS Null Or CF.ServiceRequested = '') THEN ''       
 ELSE      
 STUFF(              
 (SELECT ', ' + convert(varchar(100), DM.Title, 120)              
 FROM DDMaster DM  where DM.DDMasterID IN (SELECT val FROM GetCSVTable(CF.ServiceRequested)) AND DM.IsDeleted=0             
 FOR XML PATH ('')) , 1, 1, '')      
 END ,  
 ClientName=dbo.GetGeneralNameFormat(R.FirstName,R.LastName),RecordID = R.AHCCCSID  
 FROM CareForms CF   
 INNER JOIN Referrals R ON R.ReferralID = CF.ReferralID  
 WHERE CF.CareFormID=@CareFormID  
  
END        
ELSE        
BEGIN        
 SELECT 
 EmployeeSignaturePath = ES.SignaturePath ,
 ClientName=dbo.GetGeneralNameFormat(R.FirstName,R.LastName),
 RecordID = R.AHCCCSID  ,
 LocationOfService =RG.RegionName  ,
 Phone=C.Phone1,Cell=C.Phone2,Email=C.Email
 FROM Referrals R        
 LEFT JOIN ContactMappings CM ON CM.ReferralID = R.ReferralID  AND CM.ContactTypeID=1        
 LEFT JOIN Contacts C ON C.ContactID = CM.ContactID    
 LEFT JOIN Employees E ON E.EmployeeID = R.CaseManagerID      
 LEFT JOIN EmployeeSignatures ES ON E.EmployeeSignatureID = ES.EmployeeSignatureID 
 LEFT JOIN Regions RG ON R.RegionID = RG.RegionID
 WHERE R.ReferralID=@ReferralID        
END        
                             
    
    
        
--IF EXISTS(SELECT * FROM CareForms WHERE ReferralID=@ReferralID)        
--BEGIN        
-- SELECT CF.*,      
-- Str_ServiceRequested =      
-- CASE       
-- WHEN (CF.ServiceRequested IS Null Or CF.ServiceRequested = '') THEN ''       
-- ELSE      
--  STUFF(              
--  (SELECT ', ' + convert(varchar(100), DM.Title, 120)              
--  FROM DDMaster DM  where DM.DDMasterID IN (SELECT val FROM GetCSVTable(CF.ServiceRequested)) AND DM.IsDeleted=0             
--  FOR XML PATH ('')) , 1, 1, '')      
-- END,      
-- EmployeeSignaturePath = ES.SignaturePath      
-- FROM CareForms CF       
-- INNER JOIN Referrals R ON R.ReferralID = CF.ReferralID      
-- LEFT JOIN Employees E ON E.EmployeeID = R.CaseManagerID      
-- LEFT JOIN EmployeeSignatures ES ON E.EmployeeSignatureID = ES.EmployeeSignatureID       
-- WHERE CF.ReferralID=@ReferralID        
--END        
--ELSE        
--BEGIN        
-- SELECT Phone=C.Phone1,Cell=C.Phone2,Email=C.Email FROM Referrals R        
-- INNER JOIN  ContactMappings CM ON CM.ReferralID = R.ReferralID  AND CM.ContactTypeID=1        
-- INNER JOIN  Contacts C ON C.ContactID = CM.ContactID        
-- WHERE R.ReferralID=@ReferralID        
--END        
                              
END
