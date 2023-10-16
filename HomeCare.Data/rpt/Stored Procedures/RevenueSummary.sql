CREATE procedure [rpt].[RevenueSummary]
@StartDate date=null,
@EndDate date=null,
@payorID int=null
as
Begin
select count(bd.ScheduleID) TotalSchedules,
       count(bd.EmployeeVisitID) TotalVisits,
       count(bd.ScheduleID) - count(bd.EmployeeVisitID) TotalMissingTimesheet,

	   sum(case
                when bd.hasBatchNoteID = 1 then 1
                else 0 end) TotalClaims,
       sum(case
                when bd.hasBatchNoteID = 1 and bd.batchSent = 1 then 1
                else 0 end) TotalClaimsSent,
       sum(case
                when bd.hasBatchNoteID = 1 and bd.batchSent = 0 then 1
                else 0 end) TotalClaimsNotSent,

	   sum(case
                when bd.hasBatchNoteID = 1 then bd.BilledAmount
                else 0.0 end) TotalBilledAmount,
       sum(case
                when bd.hasBatchNoteID = 1 and bd.batchSent = 1 then bd.BilledAmount
                else 0.0 end) TotalBilledAmountSent,
       sum(case
                when bd.hasBatchNoteID = 1 and bd.batchSent = 0 then bd.BilledAmount
                else 0.0 end) TotalBilledAmountNotSent,
				
	   sum(case
                when bd.batchSent = 1 then bd.ChargeAmount
                else 0.0 end) TotalChargeAmount,
       sum(case
                when bd.batchSent = 1 then bd.PaidAmount
                else 0.0 end) TotalPaidAmount,
       sum(case
                when bd.batchSent = 1 then bd.DeniedAmount
                else 0.0 end) TotalDeniedAmount
  from [rpt].[billingDetail] bd
where bd.scheduleDate between @StartDate and @EndDate
End