CREATE procedure [dbo].[GetReferralDocument]
(
@ReferralDocumentId bigint
)
as
begin
        select * from ReferralDocuments r where r.ReferralDocumentID=@ReferralDocumentId;
end
GO

