CREATE procedure [dbo].[GetPatientAttacmentTags]  
(  
 @ReferralID bigint  
)  
as  
begin  
  
select r.LastName+', '+r.FirstName as[PatientName],CONVERT(VARCHAR(10), r.Dob, 101)as[DOB],c.LastName+', '+c.FirstName   
as[CaseManager],e.LastName+', '+e.FirstName as[Assignee],(select ct.Email  from Contacts ct where ct.ContactID=  
(select top 1 contactid from ContactMappings cm where cm.ReferralID=@ReferralID))as[Email],  
(select ct.Phone1  from Contacts ct where ct.ContactID=(select top 1 contactid from ContactMappings cm where cm.ReferralID=@ReferralID))as[Phone],  
(select ct.Address+'- '+ct.ZipCode  from Contacts ct where ct.ContactID=(select top 1 contactid from ContactMappings cm where cm.ReferralID=@ReferralID))as[Address]      
from Referrals r left join CaseManagers c on c.CaseManagerID=r.CaseManagerID left join Employees e on e.EmployeeID=r.Assignee   
where r.ReferralID=@ReferralID;  
  
  
  
  
end