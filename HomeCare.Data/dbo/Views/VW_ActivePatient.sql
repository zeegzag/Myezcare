CREATE VIEW [dbo].[VW_ActivePatient]
	AS 
	select DISTINCT r.ReferralID,r.LastName+', '+r.FirstName as Name,--CONVERT( nvarchar,R.Dob)+' ('+ r.Gender+') '+ CONVERT(nvarchar, (dbo.GetAge(r.Dob))) AS GenDobAge ,
   p.PayorName,c.Phone1 as PhoneNo,RS.Status,c.Address+','+c.ZipCode+','+c.State as Address,c.City,d.Title As CareType,dd.Title AS BeneficiaryType,rp.BeneficiaryNumber,p.payorid
     
   from Referrals r
   left join ReferralStatuses RS ON rs.ReferralStatusID=r.ReferralStatusID                           
   left join ReferralPayorMappings rp on rp.ReferralID=r.ReferralID and rp.IsActive=1 and rp.IsDeleted=0 AND rp.Precedence=1          
   right join Payors p on p.PayorID=rp.PayorID 
   left join ContactMappings cmp on cmp.ReferralID= r.ReferralID and cmp.ContactTypeID = 1                
   left join Contacts c on c.ContactID=cmp.ContactID
   INNER JOIN ScheduleMasters sm on r.ReferralID=sm.ReferralID  
   left join DDMaster d on d.DDMasterID=sm.CareTypeId 
   left join DDMaster dd on dd.DDMasterID=rp.BeneficiaryTypeID
   Where r.IsDeleted=0
   and
	r.ReferralStatusID=1