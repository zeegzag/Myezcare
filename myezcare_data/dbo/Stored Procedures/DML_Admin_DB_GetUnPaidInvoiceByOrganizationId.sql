CREATE PROCEDURE [dbo].[GetUnPaidInvoiceByOrganizationId]
@OrganizationId BIGINT
AS                    
BEGIN    
 SELECT INC.InvoiceNumber, INC.OrganizationId,ORG.DisplayName,ORG.CompanyName,INC.InvoiceAmount,INC.InvoiceDate,INC.DueDate,INC.IsPaid FROM dbo.Invoice INC WITH(NOLOCK) 
 INNER JOIN  dbo.Organizations ORG WITH(NOLOCK)
 ON ORG.OrganizationID=INC.OrganizationId
 WHERE  INC.OrganizationId=@OrganizationId
 AND ISNULL(INC.IsPaid,0)=0
END


	

