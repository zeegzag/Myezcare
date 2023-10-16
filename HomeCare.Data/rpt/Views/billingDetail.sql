CREATE view [rpt].[billingDetail]      
as      
select t.* from (      
Select  sm.ScheduleID,      
       ev.EmployeeVisitID,      
       rpm.BeneficiaryNumber as 'Medicaid#/ Client ID',      
       rba.StartDate as 'Auth Start Date',      
       a.CompanyName as Agency,      
       r.FirstName + ' ' + r.LastName as [Name],      
       r.Dob,      
       r.Gender,      
       rba.EndDate as ExpirationDate,      
       bnd.Notes,      
       bnd.ServiceDate,      
       cast(sm.StartDate as date) scheduleDate,      
       isnull(bnd.CalculatedUnit, 0) CalculatedUnit,      
    isnull(bnd.ChargeAmount, 0) ChargeAmount,      
       isnull(bnd.PaidAmount, 0) PaidAmount,      
       isnull(bnd.ChargeAmount, 0) - isnull(bnd.PaidAmount, 0) DeniedAmount,      
       isnull(bnd.BilledAmount, 0) BilledAmount,      
    bnd.NoteID,      
    bnd.BatchNoteID,      
       bnd.BatchID,      
       case when bnd.NoteID is null then 0 else 1 end hasNoteId,      
       case when bnd.BatchNoteID is null then 0 else 1 end hasBatchNoteID,      
       Case when bnd.BatchID is null then 0 else 1 end hasBatchID,      
       case when ev.EmployeeVisitID is null then 0 else 1 end isVisited,      
    bnd.IsSent batchSent,row_number() over (partition by r.referralid,sm.ScheduleID order by bn.BatchNoteID desc) as seqnum  
      
from schedulemasters sm      
    inner join referrals r      
        on r.ReferralID = sm.ReferralID      
    left join employeevisits ev      
        on sm.ScheduleID = ev.ScheduleID      
     and ev.IsDeleted = 0      
    left join Notes N      
   on n.EmployeeVisitID = ev.EmployeeVisitID      
     AND n.isDeleted = 0      
 left join batchnotes bn      
  on bn.NoteID = n.NoteID         
 left join batches b      
  on b.BatchID = bn.BatchID      
    OUTER APPLY (      
  SELECT TOP 1      
   n.NoteID,      
   n.NoteComments as Notes,      
     n.ServiceDate,      
     n.CalculatedUnit,      
      bn.BatchNoteID,      
   bn.Original_Amount BilledAmount,      
   cast(bn.SVC02_SubmittedLineItemServiceChargeAmount_BilledAmount as float) ChargeAmount,      
   cast(bn.SVC03_LineItemProviderPaymentAmoun_PaidAmount as float) PaidAmount,      
   b.BatchID,      
   b.IsSent      
    ) bnd      
 left join Agencies A      
        on r.AgencyID = a.AgencyID      
    left join ReferralBillingAuthorizations rba      
        on r.ReferralID = rba.ReferralID      
           and rba.ReferralBillingAuthorizationID = sm.ReferralBillingAuthorizationID --and (rba.StartDate>=sm.StartDate and rba.EndDate<=sm.EndDate)      
    left join ReferralPayorMappings RPM      
        on rba.PayorID = rpm.PayorID      
           and rpm.ReferralID = r.ReferralID      
           and rba.ReferralID = r.ReferralID      
           and sm.StartDate between rpm.PayorEffectiveDate and rpm.PayorEffectiveEndDate      
     and rpm.IsDeleted = 0      
  WHERE sm.IsDeleted = 0) t where seqnum=1   
  