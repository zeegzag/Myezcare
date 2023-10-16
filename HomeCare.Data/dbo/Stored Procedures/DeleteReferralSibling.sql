CREATE procedure [dbo].[DeleteReferralSibling]
@ReferralSiblingMappingID bigint
as
delete from ReferralSiblingMappings where ReferralSiblingMappingID=@ReferralSiblingMappingID
select *  from ReferralSiblingMappings