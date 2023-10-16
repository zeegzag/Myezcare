CREATE Procedure [dbo].[HC_UpdateReferralAuthorization](@EmployeeVisitID bigint)
as
Declare @ScheduleID bigint
		select @ScheduleID= sm.scheduleID from ScheduleMasters sm inner join employeevisits ev on sm.ScheduleID=ev.ScheduleID where ev.EmployeeVisitID=@EmployeeVisitID

		UPDATE SM SET SM.ReferralBillingAuthorizationID = RBA.ReferralBillingAuthorizationID
		--select ev.employeevisitid,rbmax.AuthorizationCode, sm.ReferralBillingAuthorizationID,rba.ReferralBillingAuthorizationID,rba.CareType,sm.CareTypeId,sm.StartDate
		FROM EmployeeVisits EV INNER JOIN ScheduleMasters SM ON SM.ScheduleID = EV.ScheduleID
							   INNER JOIN ReferralBillingAuthorizations RBA ON RBA.CareType = SM.CareTypeId and rba.ReferralID=sm.ReferralID and (cast(sm.StartDate as date) between rba.StartDate and rba.enddate) and (cast(sm.EndDate as date) between rba.startdate and rba.EndDate)
							   Inner join PayorServiceCodeMapping psm on psm.ServiceCodeID=RBA.ServiceCodeID and sm.CareTypeId=psm.CareType and psm.CareType=rba.CareType and psm.PayorID=sm.PayorID
							   left join 
										(select rba1.ReferralBillingAuthorizationID,rba1.AuthorizationCode,max(enddate) MaxEndDate from ReferralBillingAuthorizations rba1 group by rba1.ReferralBillingAuthorizationID,rba1.AuthorizationCode) as rbmax 
													on rbmax.ReferralBillingAuthorizationID=rba.ReferralBillingAuthorizationID and rba.EndDate=rbmax.MaxEndDate 
		WHERE RBA.IsDeleted = 0 AND SM.IsDeleted = 0 AND EV.IsDeleted = 0 and ISNULL(SM.OnHold, 0) = 0 AND sm.ScheduleID=@ScheduleID