CREATE PROCEDURE [dbo].[SetCompliancePage]  
@UserType INT    
AS        
BEGIN      
 SELECT Name=DocumentName,Value=ComplianceID,UserType FROM Compliances WHERE ParentID = 0 AND IsDeleted=0 AND (UserType=@UserType OR UserType=0)  
END
