CREATE PROCEDURE [dbo].[GetSavedFormMappings] 
@ReferralID BIGINT=0,
@EmployeeID BIGINT=0
AS    
BEGIN    
    
SELECT EbriggsFormMppingID, EFM.EBriggsFormID,EFM.OriginalEBFormID,EFM.FormId,EFM.ReferralID,EFM.EmployeeID, EFM.CreatedDate, EFM.UpdatedDate,
PatientName=dbo.GetGeneralNameFormat(R.FirstName,R.LastName) ,EmployeeName=dbo.GetGeneralNameFormat(E.FirstName,E.LastName), 
CreatedBy=dbo.GetGeneralNameFormat(EC.FirstName,EC.LastName) ,UpdatedBy=dbo.GetGeneralNameFormat(EU.FirstName,EU.LastName) 
FROM EbriggsFormMppings EFM
LEFT JOIN Referrals R ON R.ReferralID=EFM.ReferralID
LEFT JOIN Employees E ON E.EmployeeID=EFM.EmployeeID
LEFT JOIN Employees EC ON EC.EmployeeID=EFM.CreatedBy
LEFT JOIN Employees EU ON EU.EmployeeID=EFM.UpdatedBy
WHERE EFM.IsDeleted=0 AND (EFM.ReferralID=@ReferralID  OR EFM.EmployeeID=@EmployeeID)
    
    
END