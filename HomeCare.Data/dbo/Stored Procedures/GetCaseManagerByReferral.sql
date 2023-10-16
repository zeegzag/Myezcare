CREATE PROCEDURE [dbo].[GetCaseManagerByReferral]            
 (@ReferralID as Int)         
AS            
BEGIN           
   DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat()        
      
  
 select value=r.referralID,casemanager=dbo.GetGenericNameFormat(cm.FirstName,'', cm.LastName,@NameFormat) ,r.FirstName,r.LastName 
 from CaseManagers cm inner join Referrals r on cm.CaseManagerID=r.CaseManagerID where r.ReferralID   =@referralID     
        
END  