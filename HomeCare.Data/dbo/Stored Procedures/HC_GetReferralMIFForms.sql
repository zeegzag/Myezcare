CREATE PROCEDURE [dbo].[HC_GetReferralMIFForms]  
 @ReferralID BIGINT=NULL  
AS     
BEGIN  
 SELECT MF.*,CreatedByName=dbo.GetGeneralNameFormat(E.FirstName,E.LastName) FROM MIFDetails MF
 INNER JOIN Employees E ON E.EmployeeID=MF.CreatedBy
 WHERE MF.IsDeleted=0 AND MF.ReferralID=@ReferralID ORDER BY MF.CreatedDate DESC  
END