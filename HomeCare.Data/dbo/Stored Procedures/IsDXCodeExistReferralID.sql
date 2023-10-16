

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[IsDXCodeExistReferralID]
	-- Add the parameters for the stored procedure here
	@referralID int=0,
	@payorID int=0
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @onCash INT;
set @onCash=(select COUNT(*) from Payors where PayorID=@payorID and PayorInvoiceType=2)
 IF @onCash > 0 
    BEGIN
        --its on cash
		-- 1 mean Dx is exist or not matter for cash
        select 1;
    END
    ELSE
    BEGIN
--select count(*)
--from Referrals r
--inner join ReferralPayorMappings rpm on rpm.ReferralID=r.ReferralID
--inner join Payors p on p.PayorID=rpm.PayorID
--left join ReferralDXCodeMappings rdc on rdc.ReferralID=r.ReferralID
--where r.IsDeleted=0 and rpm.IsDeleted=0 and rdc.IsDeleted=0
--and r.ReferralID=@referralID and PayorInvoiceType=2
select count(rdc.ReferralDXCodeMappingID)
from referrals r
inner join ReferralDXCodeMappings rdc on rdc.ReferralID=r.ReferralID
where r.ReferralID=@referralID and rdc.IsDeleted=0

    END

END
