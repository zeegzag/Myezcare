--select * from ReferralSuspentions


CREATE PROCEDURE [dbo].[DeleteReferralSuspension]
@ResetBXContract bit=0,
@ReferralID bigint
AS
BEGIN

DELETE  FROM ReferralSuspentions WHERE ReferralID=@ReferralID;

IF(@ResetBXContract=1)
UPDATE ReferralBehaviorContracts SET IsActive=0 Where ReferralID=@ReferralID;

SELECT 1;
END


-- EXEC DeleteRecord @@CustomWhere = 'ReferralID=117', @@TableName = 'ReferralSuspentions'



--EXEC DeleteRecord @@CustomWhere = 'ReferralID=117', @@TableName = 'ReferralSuspentions'