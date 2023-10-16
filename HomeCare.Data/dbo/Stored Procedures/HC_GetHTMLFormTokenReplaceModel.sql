CREATE PROCEDURE [dbo].[HC_GetHTMLFormTokenReplaceModel]    
@ReferralID BIGINT    
AS    
BEGIN    
    
  
DECLARE @Org_Logo NVARCHAR(MAX)='';  
  
SELECT TOP 1  @Org_Logo= TemplateLogo FROM OrganizationSettings   
  
  
SELECT  MEMBER_NAME=R.FirstName, MEMBER_LAST_NAME=R.LastName ,MEMBER_DOB=R.Dob, MEMBER_ACCOUNT=R.AHCCCSID  
FROM Referrals R   
     
WHERE R.REferralID=@ReferralID    
    
END