CREATE PROCEDURE [dbo].[GetCaseManagerByReferral]          
 (@ReferralID as Int)       
AS          
BEGIN         
        
    

 select value=r.referralID,casemanager= cm.LastName+','+cm.FirstName,r.FirstName,r.LastName from CaseManagers cm inner join Referrals r on cm.CaseManagerID=r.CaseManagerID where r.ReferralID   =@referralID   
      
END
