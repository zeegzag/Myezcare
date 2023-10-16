CREATE view [dbo].[referralGroup]
as
select 
    grp.* ,R.ReferralID
from 
    Referrals R
    cross apply dbo.GetCSVTable(R.GroupIDs) grp