CREATE PROCEDURE [dbo].[API_GetDocumentInfo]              
@ReferralDocumentID BIGINT=0,    
@EbriggsFormMppingID BIGINT=0,
@ComplianceID BIGINT=0
AS              
BEGIN    
 IF(@ReferralDocumentID>0)    
 BEGIN    
  SELECT Name=FileName,RD.CreatedDate,RD.UpdatedDate,CreatedBy=dbo.GetGeneralNameFormat(EC.FirstName,EC.LastName),
  UpdatedBy=dbo.GetGeneralNameFormat(EU.FirstName,EU.LastName)    
  FROM ReferralDocuments RD    
  LEFT JOIN Employees EC ON EC.EmployeeID=RD.CreatedBy    
  LEFT JOIN Employees EU ON EU.EmployeeID=RD.UpdatedBy    
  WHERE ReferralDocumentID=@ReferralDocumentID    
 END    
    
 IF(@EbriggsFormMppingID>0)    
 BEGIN    
  SELECT Name=EBF.FormName,EBF.CreatedDate,EBF.UpdatedDate,CreatedBy=dbo.GetGeneralNameFormat(EC.FirstName,EC.LastName),  
  UpdatedBy=dbo.GetGeneralNameFormat(EU.FirstName,EU.LastName)    
  FROM EbriggsFormMppings EBF    
  LEFT JOIN Employees EC ON EC.EmployeeID=EBF.CreatedBy    
  LEFT JOIN Employees EU ON EU.EmployeeID=EBF.UpdatedBy    
  WHERE EbriggsFormMppingID=@EbriggsFormMppingID    
 END

 IF(@ComplianceID>0)    
 BEGIN    
  SELECT Name=C.DocumentName,C.CreatedDate,C.UpdatedDate,CreatedBy=dbo.GetGeneralNameFormat(EC.FirstName,EC.LastName),  
  UpdatedBy=dbo.GetGeneralNameFormat(EU.FirstName,EU.LastName)    
  FROM Compliances C
  LEFT JOIN Employees EC ON EC.EmployeeID=C.CreatedBy    
  LEFT JOIN Employees EU ON EU.EmployeeID=C.UpdatedBy    
  WHERE ComplianceID=@ComplianceID
 END
END
