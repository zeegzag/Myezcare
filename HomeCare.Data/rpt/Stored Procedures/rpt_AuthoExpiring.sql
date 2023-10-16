 CREATE procedure [rpt].[rpt_AuthoExpiring]    
@PatientName bigint=0,    
@PayorName bigint=0,    
@dbname varchar(50) = NULL    
    
AS    
BEGIN    
      declare @patientid bigint;    
   declare @payorid bigint;    
   set @patientid=0;    
   set @payorid=0;    
   if @PatientName>0    
   begin    
   set @patientid= @PatientName    
   end    
   if @PayorName>0    
   begin    
   set @payorid= @PayorName    
   end    
DECLARE @DateFormat VARCHAR(MAX);      
SELECT @DateFormat=Admin_Myezcare_Live.dbo.fn_getDateFormat(@dbname)    
    
if @patientid>0 and @payorid>0    
begin    
    
    
 select dbo.GetGeneralNameFormat(r.FirstName,r.LastName) as[PatientName],count(ev.EmployeeVisitID)as[TotalVisits],rbz.AuthorizationCode,p.PayorName,    
 --convert(varchar,rbz.EndDate,101) as [ExpireDate],    
 FORMAT(rbz.EndDate, @DateFormat) as [ExpireDate],    
 (count(sm.ScheduleID)-count(ev.EmployeeVisitID))as[RemainingVisit] from EmployeeVisits ev    
inner join ScheduleMasters sm on sm.ScheduleID=ev.ScheduleID     
inner join Referrals r on r.ReferralID=sm.ReferralID AND r.IsDeleted=0    
inner join ReferralBillingAuthorizations rbz on rbz.ReferralID=sm.ReferralID     
inner join Payors p on p.PayorID=rbz.PayorID      
where cast(rbz.EndDate as date)< cast(getdate()as date) and r.ReferralID=@patientid and p.PayorID=@payorid    
 group by sm.ReferralID,r.FirstName,rbz.AuthorizationCode,p.PayorName,rbz.EndDate,r.FirstName,r.MiddleName,r.LastName     
 end    
    
     
    
 if @patientid>0 and @payorid=0    
 begin    
    
  select dbo.GetGeneralNameFormat(r.FirstName,r.LastName) as[PatientName],    
  count(ev.EmployeeVisitID)as[TotalVisits],    
  rbz.AuthorizationCode,    
  p.PayorName,    
  FORMAT(rbz.EndDate, @DateFormat) as [ExpireDate],    
  (count(sm.ScheduleID)-count(ev.EmployeeVisitID))as[RemainingVisit]    
 from EmployeeVisits ev    
inner join ScheduleMasters sm on sm.ScheduleID=ev.ScheduleID     
inner join Referrals r on r.ReferralID=sm.ReferralID AND r.IsDeleted=0    
inner join ReferralBillingAuthorizations rbz on rbz.ReferralID=sm.ReferralID     
inner join Payors p on p.PayorID=rbz.PayorID      
where cast(rbz.EndDate as date)< cast(getdate()as date) and r.ReferralID=@patientid    
 group by sm.ReferralID,r.FirstName,rbz.AuthorizationCode,p.PayorName,rbz.EndDate,r.FirstName,r.MiddleName,r.LastName     
 end    
    
  if @payorid>0 and @patientid=0    
 begin    
  select dbo.GetGeneralNameFormat(r.FirstName,r.LastName) as[PatientName],count(ev.EmployeeVisitID)as[TotalVisits],rbz.AuthorizationCode,p.PayorName,    
 FORMAT(rbz.EndDate, @DateFormat) as [ExpireDate],    
  (count(sm.ScheduleID)-count(ev.EmployeeVisitID))as[RemainingVisit]    
  from EmployeeVisits ev    
inner join ScheduleMasters sm on sm.ScheduleID=ev.ScheduleID     
inner join Referrals r on r.ReferralID=sm.ReferralID AND r.IsDeleted=0    
inner join ReferralBillingAuthorizations rbz on rbz.ReferralID=sm.ReferralID     
inner join Payors p on p.PayorID=rbz.PayorID      
where cast(rbz.EndDate as date)< cast(getdate()as date) and p.PayorID=@payorid    
 group by sm.ReferralID,r.FirstName,rbz.AuthorizationCode,p.PayorName,rbz.EndDate,r.FirstName,r.MiddleName,r.LastName     
 end    
    
  if @payorid=0 and @patientid=0    
 begin    
  select dbo.GetGeneralNameFormat(r.FirstName,r.LastName) as[PatientName],count(ev.EmployeeVisitID)as[TotalVisits],rbz.AuthorizationCode,p.PayorName,    
  FORMAT(rbz.EndDate, @DateFormat) as [ExpireDate],    
  (count(sm.ScheduleID)-count(ev.EmployeeVisitID))as[RemainingVisit],    
  case    
  when (cast(rbz.EndDate as date) < DATEADD(DAY,8, getdate()) and cast(rbz.EndDate as date) > DATEADD(DAY,-8, getdate()))    
  then '0 to 8 days'    
  when (cast(rbz.EndDate as date) < DATEADD(DAY,15, getdate()) and cast(rbz.EndDate as date) > DATEADD(DAY,-15, getdate()))    
  then '8 to 15 days'    
  when (cast(rbz.EndDate as date) < DATEADD(DAY,30, getdate()) and cast(rbz.EndDate as date) > DATEADD(DAY,-30, getdate()))    
  then '15 to 30 days'    
  when (cast(rbz.EndDate as date) < DATEADD(DAY,90, getdate()) and cast(rbz.EndDate as date) > DATEADD(DAY,-90, getdate()))    
  then '30 to 90 days'    
  else 'More than 90 days'    
  end as "Expairy_range"    
  from EmployeeVisits ev    
inner join ScheduleMasters sm on sm.ScheduleID=ev.ScheduleID     
inner join Referrals r on r.ReferralID=sm.ReferralID AND r.IsDeleted=0    
inner join ReferralBillingAuthorizations rbz on rbz.ReferralID=sm.ReferralID     
inner join Payors p on p.PayorID=rbz.PayorID      
--where cast(rbz.EndDate as date)< cast(getdate()as date)    
 group by sm.ReferralID,r.FirstName,rbz.AuthorizationCode,p.PayorName,rbz.EndDate,r.FirstName,r.MiddleName,r.LastName     
 end    
    
 end