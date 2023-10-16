-- exec [GetOrganizationDetailsById] 3,
CREATE PROCEDURE [dbo].[GetOrganizationDetailsById]
@OrganizationID INT
AS                                   
BEGIN
	DECLARE @CurrentDate DATE=GETUTCDATE();                         
 
	SELECT 
		*       
	FROM 
		Organizations 
	WHERE 
		OrganizationID = @OrganizationID 
		AND IsActive=1         
		AND @CurrentDate >= Convert(Date,StartDate)
		AND (@CurrentDate <= Convert(Date,EndDate) OR EndDate IS NULL)                                 
END