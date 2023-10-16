
--CreatedBy: Abhishek Gautam
--CreatedDate: 10 sept 2020
--Description: Get Details DMAS97AB form

 --   EXEC GetDMAS97ABdetail 1,60053
CREATE PROCEDURE [dbo].[GetDMAS97ABdetail]    
 @Dmas97ID BIGINT=0, 
 @ReferralID BIGINT=0      
AS       
  BEGIN       
     select * from Dmas97ab where Dmas97ID=@Dmas97ID     
  
  select OrganizationID,SiteLogo,SiteName,SupportEmail,OrganizationAddress,OrganizationCity,OrganizationState,OrganizationZipcode from OrganizationSettings
   
   DECLARE @InfinateDate DATE='2099/12/31'; 
select p.PayorID,p.PayorName, rpm.BeneficiaryNumber
from Referrals r 
INNER JOIN ReferralPayorMappings rpm ON RPM.ReferralID=R.ReferralID
INNER JOIN Payors P ON P.PayorID=RPM.PayorID and p.IsDeleted=0
 WHERE RPM.ReferralID=@ReferralID  AND GETDATE() BETWEEN RPM.PayorEffectiveDate AND ISNULL(RPM.PayorEffectiveEndDate,@InfinateDate) AND RPM.IsDeleted=0  

select r.ReferralID,PatientName = r.[FirstName] + ' ' + r.[LastName], a.NPI from Referrals r
LEFT JOIN Agencies a on a.AgencyID=r.AgencyID
where ReferralID=@ReferralID and r.IsDeleted=0
END