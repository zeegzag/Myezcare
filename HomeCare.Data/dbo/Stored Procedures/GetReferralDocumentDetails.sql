create procedure [dbo].[GetReferralDocumentDetails]  
(  
@ReferralId varchar(max)  
)  
as   
begin      
  --    declare @employeeid bigint;      
  -- select @employeeid= r.assignee from referrals r where r.ReferralId=@ReferralId       
  --select top 5 p.LastName+', '+FirstName as[PhysicianName],p.Email from Physicians p,ReferralPhysicians rp where rp.PhysicianID=p.PhysicianID and rp.ReferralID=@ReferralId;      
  --    select cm.LastName+', '+cm.FirstName as[CaseManager],cm.Email from CaseManagers cm left join Referrals r on r.casemanagerid=cm.casemanagerid where r.referralid=@ReferralId;      
  --  select e.LastName+', '+e.FirstName as[Assignee],e.Email from Employees e where e.EmployeeId=@employeeid;      
  --   select c.LastName+', '+c.FirstName as[Relative],c.Email from Contacts c  inner join contactmappings cm on cm.ContactID=c.ContactID and cm.ReferralID=@ReferralId      
    declare @employeeid bigint;   
   select @employeeid= r.assignee from referrals r where r.ReferralId in (select CONVERT(BIGINT,VAL)   from GetCSVTable(@ReferralId))    --@ReferralId       
  select top 5 p.LastName+', '+FirstName as[PhysicianName],p.Email from Physicians p,ReferralPhysicians rp where rp.PhysicianID=p.PhysicianID and rp.ReferralID in (select CONVERT(BIGINT,VAL)   from GetCSVTable(@ReferralId)) --=@ReferralId;      
       
   select cm.LastName+', '+cm.FirstName as[CaseManager],cm.Email from CaseManagers cm left join Referrals r on r.casemanagerid=cm.casemanagerid where r.referralid in (select CONVERT(BIGINT,VAL)   from GetCSVTable(@ReferralId))--@ReferralId;      
    select e.LastName+', '+e.FirstName as[Assignee],e.Email from Employees e where e.EmployeeId in (select CONVERT(BIGINT,VAL)   from GetCSVTable(@ReferralId)) --=@employeeid;      
     select c.LastName+', '+c.FirstName as[Relative],c.Email from Contacts c  inner join contactmappings cm on cm.ContactID=c.ContactID and cm.ReferralID in (select CONVERT(BIGINT,VAL)   from GetCSVTable(@ReferralId))--=@ReferralId      
       
	    select top 6 p.LastName+', '+FirstName as [Name],p.Email from Physicians p,ReferralPhysicians rp where rp.PhysicianID=p.PhysicianID and rp.ReferralID in (select CONVERT(BIGINT,VAL)   from GetCSVTable(@ReferralId)) --=@ReferralId;      
       union 
   select cm.LastName+', '+cm.FirstName as[Name],cm.Email from CaseManagers cm left join Referrals r on r.casemanagerid=cm.casemanagerid where r.referralid in (select CONVERT(BIGINT,VAL)   from GetCSVTable(@ReferralId))--@ReferralId;      
    union
	select e.LastName+', '+e.FirstName as[Name],e.Email from Employees e where e.EmployeeId in (select CONVERT(BIGINT,VAL)   from GetCSVTable(@ReferralId)) --=@employeeid;      
    union
	select c.LastName+', '+c.FirstName as[Name],c.Email from Contacts c  inner join contactmappings cm on cm.ContactID=c.ContactID and cm.ReferralID in (select CONVERT(BIGINT,VAL)   from GetCSVTable(@ReferralId))--=@ReferralId      
       
 end 