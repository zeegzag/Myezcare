--CreatedBy: Abhishek Gautam    
--CreatedDate: 25 sept 2020    
--Description: GetDetail of CMS485 form    
  
--   EXEC GetCMS485detail  0 , 60053        
CREATE PROCEDURE [dbo].[GetCMS485detail]              
 @Cms485ID BIGINT=0,           
 @ReferralID BIGINT=0                
AS                 
  BEGIN     
  DECLARE @NameFormat VARCHAR(500) = dbo.GetOrgNameFormat() 
  select * from CMS485 where Cms485ID=@Cms485ID               
            
  select OrganizationID,SiteLogo,SiteName,SupportEmail,OrganizationAddress,OrganizationCity,OrganizationState,OrganizationZipcode,      
  CONCAT(SiteName,', ',OrganizationAddress,' ',OrganizationCity,' ',OrganizationState,' ',OrganizationZipcode, ', ',Submitter_EDIContact1_PER04_CommunicationNumber) As OrgDetails      
   from OrganizationSettings          
                
         
select r.ReferralID,CONCAT(dbo.GetGenericNameFormat(r.FirstName,r.MiddleName, r.LastName,@NameFormat),',',c.ApartmentNo,' ',c.Address,' ',c.City,' ',c.State,' ',c.ZipCode) AS ReferralAddress,r.Dob,r.Gender from Referrals r             
INNER JOIN ContactMappings cm on cm.ReferralID=r.ReferralID          
inner join Contacts c on c.ContactID=cm.ContactID          
 WHERE r.ReferralID=@ReferralID        
  
  END 