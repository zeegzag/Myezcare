--exec [GetEmployeeContactInformation] 10031  
CREATE PROCEDURE [dbo].[GetEmployeeContactInformation]  
 @EmployeeID BIGINT = 0,  
 @ReferenceCode VARCHAR(2)='EM'     
AS  
BEGIN  
 SELECT CM.ContactMappingID, CT.ContactTypeID ,CT.ContactTypeName,CM.ROIType,CM.ROIExpireDate,C.FirstName, C.LastName, C.[Address], C.LanguageID,  
 C.PHONE1,C.Phone2, C.City,C.State,C.ZipCode,CM.IsDCSLegalGuardian,CM.IsEmergencyContact,CM.IsNoticeProviderOnFile,CM.IsPrimaryPlacementLegalGuardian,  
 C.ContactID,CM.EmployeeID,CM.ClientID, E.FirstName AS EmpFirstName, E.LastName AS EmpLastName  
 FROM Contacts C  
 INNER JOIN EmployeeContactMappings CM ON CM.ContactID = C.ContactID  
 INNER JOIN ContactTypes CT ON CT.ContactTypeID = CM.ContactTypeID  
 INNER JOIN Employees E ON E.EmployeeID = C.CreatedBy  
 left JOIN ReferenceMaster RM ON RM.ReferenceID = c.ReferenceMasterID AND RM.IsActive=1                                                                                        
 WHERE ((RM.ReferenceCode is null) OR RM.ReferenceCode=@ReferenceCode) and  CM.EmployeeID=@EmployeeID;    
   
END  
  