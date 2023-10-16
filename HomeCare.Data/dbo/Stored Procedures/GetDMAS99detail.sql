
--CreatedBy: Abhishek Gautam
--CreatedDate: 10 sept 2020
--Description: Get Details DMAS99 form
 
 --   EXEC GetDMAS99detail  0, 60053        
CREATE PROCEDURE [dbo].[GetDMAS99detail]          
 @Dmas99ID BIGINT=0,  
 @ReferralID BIGINT=0            
AS             
  BEGIN       
        
  select * from DMAS99 where Dmas99ID=@Dmas99ID          
        
  select OrganizationID,SiteLogo,SiteName,SupportEmail,OrganizationAddress,OrganizationCity,OrganizationState,OrganizationZipcode from OrganizationSettings    
  
     
DECLARE @InfinateDate DATE='2099/12/31'; 
select p.PayorID,p.PayorName, rpm.BeneficiaryNumber
from Referrals r 
INNER JOIN ReferralPayorMappings rpm ON RPM.ReferralID=R.ReferralID
INNER JOIN Payors P ON P.PayorID=RPM.PayorID and p.IsDeleted=0
 WHERE RPM.ReferralID=@ReferralID  AND GETDATE() BETWEEN RPM.PayorEffectiveDate AND ISNULL(RPM.PayorEffectiveEndDate,@InfinateDate) AND RPM.IsDeleted=0

    
select r.ReferralID,PatientName = r.[FirstName] + ' ' + r.[LastName],r.Dob,CONCAT(c.ApartmentNo,' ',c.Address) as address1,CONCAT(c.City,' ',c.State,' ',c.ZipCode) as address2,r.SocialSecurityNumber,c.Phone1, a.NPI, a.NickName 
from Referrals r     
INNER JOIN ContactMappings cm on cm.ReferralID=r.ReferralID  
INNER JOIN Contacts c on c.ContactID=cm.ContactID
LEFT JOIN Agencies a on a.AgencyID=r.AgencyID
WHERE r.ReferralID=@ReferralID
END