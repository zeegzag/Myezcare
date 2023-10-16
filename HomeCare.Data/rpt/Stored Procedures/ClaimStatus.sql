
CREATE Procedure rpt.ClaimStatus
 
@todate date, @fromdate date,@referralid bigint=null,@missedbilling bit=0
as
--select @todate ='2022-01-01', @fromdate ='2022-01-31',@referralid =null,@missedbilling =0




begin
select r.ReferralID, r.firstname, r.LastName,rba.AuthorizationCode,rba.EndDate 'PA expiration Date',convert(date,sm.startdate) 'Schedule Date',n.noteid,
case  when rba.EndDate<convert(date,sm.startdate) then 'Expired' else 'Active' end 'PA Status' ,rba.Rate,
case when  n.noteid is Null then 'Not generated' else 'generated' end ClaimStatus
from EmployeeVisits EV
inner join ScheduleMasters sm on sm.scheduleid=ev.ScheduleID
inner join referrals r on r.referralid=sm.ReferralID
left JOIN ReferralBillingAuthorizations AS RBA   ON RBA.ReferralID = r.ReferralID  AND RBA.ReferralBillingAuthorizationID = SM.ReferralBillingAuthorizationID and rba.IsDeleted=0
left join notes N on n.EmployeeVisitID=ev.EmployeeVisitID
left join Notes_Temporary nt on nt.EmployeeVisitID=ev.EmployeeVisitID 
where  convert(date,sm.StartDate) between @fromdate and @todate --
and ((@missedbilling=0 or @missedbilling is null) or n.NoteID is null)
and (@referralid is null or sm.ReferralID=@referralid)
--and rba.EndDate <= sm.EndDate and ev.IsDeleted=0 and sm.IsDeleted=0
--Group by r.ReferralID, r.firstname, r.LastName,rba.EndDate,
order by r.FirstName
end