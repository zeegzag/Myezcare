CREATE PROC [dbo].[HC_GetPdfData]
(
@FormUniqueId int,
@OrgId int
)
AS
BEgin
	
	if(@FormUniqueId <0)
	begin
			set @FormUniqueId = 6
			set @OrgId = 1
	end

	SELECT FormData,*
	FROM dbo.OrganizationFormData
	WHERE 
	OrganizationId = @orgid and OrganizationFormDataId = @FormUniqueId
end