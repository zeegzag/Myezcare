--exec GetServicePlanList
CREATE PROCEDURE [dbo].[GetServicePlanList] 

@ReferralStatusIDs varchar(max)='1',
@PayorIDs varchar(150)='2'

AS          
BEGIN          

	select  R.ReferralID,R.FirstName,R.LastName,R.Dob,R.AHCCCSID, CM.FirstName + ' '+ CM.LastName AS CaseManager,A.NickName,
    	 ISNULL(R.RecordRequestEmail,CM.Email) RecordRequestEmail

	from Referrals R
		 INNER JOIN CaseManagers CM on CM.CaseManagerID=R.CaseManagerID
		 INNER JOIN Agencies A on A.AgencyID=R.AgencyID
		 INNER JOIN ReferralPayorMappings RP on RP.ReferralID=R.ReferralID and RP.IsActive=1 and RP.IsDeleted=0

	where
	R.IsDeleted=0 AND  RP.PayorID IN  (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@PayorIDs)) 
	AND R.ReferralStatusID IN  (SELECT CAST(Val AS BIGINT) FROM GETCSVTABLE(@ReferralStatusIDs))

END